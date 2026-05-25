
// MonsterActionState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.Globals;

namespace EamonRT.Game.States
{
	[ClassMappings]
	public class MonsterActionState : State, IMonsterActionState
	{
		/// <summary></summary>
		public virtual IList<IArtifact> WeaponArtifactList { get; set; }

		/// <summary></summary>
		public virtual IList<IMonster> FocusMonsterList { get; set; }

		/// <summary></summary>
		public virtual IArtifact WeaponArtifact { get; set; }

		/// <summary></summary>
		public virtual IMonster FocusMonster { get; set; }

		/// <summary></summary>
		public virtual IMonster LoopMonster { get; set; }

		/// <summary></summary>
		public virtual IRoom LoopMonsterRoom { get; set; }

		/// <summary></summary>
		public virtual ICommand ActionCommand { get; set; }

		/// <summary></summary>
		public virtual long OldParry { get; set; }

		/// <summary></summary>
		public virtual long NewParry { get; set; }

		public override void Execute()
		{
			LoopMonster = gMDB[gEngine.LoopMonsterUid];

			Debug.Assert(LoopMonster != null);

			LoopMonsterRoom = LoopMonster.GetInRoom();

			Debug.Assert(LoopMonsterRoom != null);

			gEngine.LoopGroupCount = LoopMonster.CurrGroupCount;

			MonsterFleesCheck();

			if (GotoCleanup)
			{
				goto Cleanup;
			}

			MonsterAdjustsParryCheck();

			if (GotoCleanup)
			{
				goto Cleanup;
			}

			MonsterAdjustsRangeCheck();

			if (GotoCleanup)
			{
				goto Cleanup;
			}

			NextState = gEngine.CreateInstance<IMonsterMemberLoopInitializeState>();

		Cleanup:

			if (NextState == null)
			{
				NextState = gEngine.CreateInstance<IMonsterLoopIncrementState>();
			}

			gEngine.NextState = NextState;
		}

		public virtual void MonsterFleesCheck()
		{
			if (LoopMonster.CheckNBTLHostility() && LoopMonster.CanMoveToRoomUid(0, true) && !LoopMonster.CheckCourage())
			{
				Debug.Assert(LoopMonster.Reaction != Friendliness.Neutral);

				ActionCommand = gEngine.CreateInstance<IFleeCommand>(x =>
				{
					x.ActorMonster = LoopMonster;

					x.ActorRoom = LoopMonsterRoom;
				});

				ActionCommand.Execute();

				if (LoopMonster.CurrGroupCount >= gEngine.LoopGroupCount)
				{
					GotoCleanup = true;
				}
			}
		}

		public virtual void MonsterAdjustsParryCheck()
		{
			if (LoopMonster.CheckParryAdjustment())
			{
				OldParry = LoopMonster.Parry;

				NewParry = LoopMonster.GetParryAdjustment();

				if (OldParry != NewParry)
				{
					ActionCommand = gEngine.CreateInstance<IParryCommand>(x =>
					{
						x.ActorMonster = LoopMonster;

						x.ActorRoom = LoopMonsterRoom;

						x.Parry = NewParry;

						x.PrintCombatStanceChanged = LoopMonster.ShouldPrintCombatStanceChanged(OldParry, NewParry);
					});

					ActionCommand.Execute();

					if (ActionCommand.GotoCleanup)
					{
						GotoCleanup = true;
					}
				}
			}
		}

		public virtual void MonsterAdjustsRangeCheck()
		{
			if (LoopMonster.CheckRangeAdjustment())
			{
				// Step 1: approach unarmed weapon if needed (existing behavior, unchanged)
				MonsterApproachesWeaponCheck();

				if (GotoCleanup)
				{
					goto Cleanup;
				}

				if (LoopMonster.Weapon >= 0)
				{
					// Step 2: select or refresh the focus target
					MonsterSelectsFocusTarget();

					if (GotoCleanup)
					{
						goto Cleanup;
					}

					// Step 3: voluntarily re-evaluate and switch weapons if RearmCode active.
					// Fires before repositioning so the correct weapon's optimal range is used.
					MonsterRearmsWeaponCheck();

					if (GotoCleanup)
					{
						goto Cleanup;
					}

					// Step 4: reposition based on TravelCode
					MonsterRepositions();

					if (GotoCleanup)
					{
						goto Cleanup;
					}
				}
			}

		Cleanup:

			;
		}

		public virtual void MonsterApproachesWeaponCheck()
		{
			// Fires only when the monster has no ready weapon and should acquire one.
			// This is the unarmed weapon-fetch path — distinct from MonsterRearmsWeaponCheck
			// which handles voluntary switching while already armed.
			if (LoopMonster.ShouldReadyWeapon() && (((LoopMonster.CombatCode == CombatCode.NaturalWeapons || LoopMonster.CombatCode == CombatCode.NaturalAttacks) && LoopMonster.Weapon <= 0) || ((LoopMonster.CombatCode == CombatCode.Weapons || LoopMonster.CombatCode == CombatCode.Attacks) && LoopMonster.Weapon < 0)))
			{
				if (LoopMonster.TravelCode == TravelCode.None)
				{
					goto Cleanup;
				}

				WeaponArtifactList = gEngine.GetReadyableWeaponList(LoopMonster);

				if (WeaponArtifactList.Count <= 0)
				{
					goto Cleanup;
				}

				var minRange = WeaponArtifactList
					.Min(a => gEngine.GetRange(LoopMonster.Coord, a.Coord));

				WeaponArtifact = WeaponArtifactList
					.Where(a => gEngine.GetRange(LoopMonster.Coord, a.Coord) == minRange)
					.OrderBy(_ => gEngine.RollDice(1, 100, 0))
					.First();

				if (WeaponArtifact != null && !LoopMonster.CanReach(WeaponArtifact))
				{
					var currentRange = gEngine.GetRange(LoopMonster.Coord, WeaponArtifact.Coord);

					if (currentRange == 0)
					{
						goto Cleanup;
					}

					var travelRange = LoopMonster.GetTravelRange();

					ActionCommand = gEngine.CreateInstance<IApproachCommand>(x =>
					{
						x.ActorMonster = LoopMonster;
						x.ActorRoom = LoopMonsterRoom;
						x.Dobj = WeaponArtifact.IsCarriedByContainer() ? WeaponArtifact.GetCarriedByContainer() : WeaponArtifact;
						x.Range = Math.Min(currentRange, travelRange);
					});

					ActionCommand.Execute();

					if (ActionCommand is IApproachCommand approachCommand && approachCommand.PartialTurnMove)
					{
						NextState = gEngine.CreateInstance<IMonsterMemberLoopInitializeState>();
					}

					GotoCleanup = true;

					goto Cleanup;
				}
			}

		Cleanup:

			;
		}

		public virtual void MonsterSelectsFocusTarget()
		{
			// Re-evaluate focus: if the turns timer fires, roll to drop the current target
			if (LoopMonster.FocusMonsterUid > 0 && gGameState.CurrTurn % LoopMonster.FocusTurns == 0)
			{
				var rl = gEngine.RollDice(1, 100, 0);

				if (rl <= LoopMonster.FocusOdds)
				{
					LoopMonster.FocusMonsterUid = 0;
				}
			}

			// Validate current focus — clear if focus monster has left the room
			if (LoopMonster.FocusMonsterUid > 0)
			{
				FocusMonster = gMDB[LoopMonster.FocusMonsterUid];

				Debug.Assert(FocusMonster != null);

				if (!FocusMonster.IsInRoom(LoopMonsterRoom))
				{
					LoopMonster.FocusMonsterUid = 0;
				}
			}

			// Select a new focus target if we don't have one
			if (LoopMonster.FocusMonsterUid == 0)
			{
				FocusMonsterList = LoopMonster.EvalReaction
				(
					gEngine.GetMonsterList(m => m.Reaction == Friendliness.Friend && m.IsInRoom(LoopMonsterRoom)),
					new List<IMonster>(),
					gEngine.GetMonsterList(m => m.Reaction == Friendliness.Enemy && m.IsInRoom(LoopMonsterRoom))
				);

				if (FocusMonsterList.Count <= 0)
				{
					goto Cleanup;
				}

				switch (LoopMonster.FocusCode)
				{
					case FocusCode.Farthest:
					{
						var maxRange = FocusMonsterList
							.Max(m => gEngine.GetRange(LoopMonster.Coord, m.Coord));

						FocusMonster = FocusMonsterList
							.Where(m => gEngine.GetRange(LoopMonster.Coord, m.Coord) == maxRange)
							.OrderBy(_ => gEngine.RollDice(1, 100, 0))
							.First();

						break;
					}

					case FocusCode.Weakest:
					{
						var minHardiness = FocusMonsterList
							.Min(m => m.GroupCount * m.Hardiness);

						FocusMonster = FocusMonsterList
							.Where(m => m.GroupCount * m.Hardiness == minHardiness)
							.OrderBy(_ => gEngine.RollDice(1, 100, 0))
							.First();

						break;
					}

					case FocusCode.Strongest:
					{
						var maxHardiness = FocusMonsterList
							.Max(m => m.GroupCount * m.Hardiness);

						FocusMonster = FocusMonsterList
							.Where(m => m.GroupCount * m.Hardiness == maxHardiness)
							.OrderBy(_ => gEngine.RollDice(1, 100, 0))
							.First();

						break;
					}

					case FocusCode.MostInjured:
					{
						Func<IMonster, long> injuryPct = m =>
							(long)Math.Round(((m.GroupCount - m.CurrGroupCount) * m.Hardiness + m.DmgTaken)
							/ (double)(m.GroupCount * m.Hardiness) * 100);

						var maxInjury = FocusMonsterList.Max(injuryPct);

						FocusMonster = FocusMonsterList
							.Where(m => injuryPct(m) == maxInjury)
							.OrderBy(_ => gEngine.RollDice(1, 100, 0))
							.First();

						break;
					}

					case FocusCode.LeastInjured:
					{
						Func<IMonster, long> injuryPct = m =>
							(long)Math.Round(((m.GroupCount - m.CurrGroupCount) * m.Hardiness + m.DmgTaken)
							/ (double)(m.GroupCount * m.Hardiness) * 100);

						var minInjury = FocusMonsterList.Min(injuryPct);

						FocusMonster = FocusMonsterList
							.Where(m => injuryPct(m) == minInjury)
							.OrderBy(_ => gEngine.RollDice(1, 100, 0))
							.First();

						break;
					}

					case FocusCode.Random:
					{
						FocusMonster = FocusMonsterList
							.OrderBy(_ => gEngine.RollDice(1, 100, 0))
							.First();

						break;
					}

					case FocusCode.Closest:
					default:
					{
						var minRange = FocusMonsterList
							.Min(m => gEngine.GetRange(LoopMonster.Coord, m.Coord));

						FocusMonster = FocusMonsterList
							.Where(m => gEngine.GetRange(LoopMonster.Coord, m.Coord) == minRange)
							.OrderBy(_ => gEngine.RollDice(1, 100, 0))
							.First();

						break;
					}
				}

				LoopMonster.FocusMonsterUid = FocusMonster.Uid;
			}

		Cleanup:

			;
		}

		public virtual void MonsterRearmsWeaponCheck()
		{
			// Only fires when armed with Artifact and RearmCode is active.
			// RearmCode.None (default) exits immediately — fully backward compatible.
			if (LoopMonster.RearmCode == RearmCode.None || LoopMonster.FocusMonsterUid == 0 || LoopMonster.Weapon == 0)
			{
				goto Cleanup;
			}

			// TurnsCheck — evaluate only on the correct turn interval
			if (LoopMonster.RearmTurns > 1 && gGameState.CurrTurn % LoopMonster.RearmTurns != 0)
			{
				goto Cleanup;
			}

			// OddsRoll — probability of actually committing to the switch
			if (LoopMonster.RearmOdds < 100)
			{
				var rl = gEngine.RollDice(1, 100, 0);

				if (rl > LoopMonster.RearmOdds)
				{
					goto Cleanup;
				}
			}

			// Build candidate list: readyable weapons in inventory excluding current weapon
			var candidateList = gEngine.GetReadyableWeaponList(LoopMonster)
				.Where(a => a.IsCarriedByMonster(LoopMonster) && a.Uid != LoopMonster.Weapon)
				.ToList();

			if (candidateList.Count == 0)
			{
				goto Cleanup;
			}

			FocusMonster = gMDB[LoopMonster.FocusMonsterUid];

			Debug.Assert(FocusMonster != null);

			// Current range to focus target for BestForRange scoring.
			var currentRange = gEngine.GetRange(LoopMonster.Coord, FocusMonster.Coord);

			IArtifact bestWeapon = null;

			switch (LoopMonster.RearmCode)
			{
				case RearmCode.BestForRange:
				{
					// Score the current weapon first. A switch only occurs if a candidate
					// scores STRICTLY better — ties leave the current weapon in place,
					// preventing oscillation between equally-scored weapons.
					var currentAc = gADB[LoopMonster.Weapon]?.GeneralWeapon;

					long currentWeaponScore = long.MaxValue;

					if (currentAc != null)
					{
						var cwMinRange = currentAc.Field11;
						var cwOptMin = currentAc.Field12;
						var cwOptMax = currentAc.Field13;
						var cwMaxRange = currentAc.Field14;

						if (currentRange >= cwMinRange && currentRange <= cwMaxRange)
						{
							if (currentRange >= cwOptMin && currentRange <= cwOptMax)
							{
								currentWeaponScore = 0;
							}
							else if (currentRange < cwOptMin)
							{
								currentWeaponScore = cwOptMin - currentRange;
							}
							else
							{
								currentWeaponScore = currentRange - cwOptMax;
							}
						}
					}

					// Among candidates whose legal range [minRange, maxRange] contains currentRange,
					// prefer the one whose optimal band [optimalMin, optimalMax] is the best fit.
					// Score = 0 if currentRange is within optimal band; otherwise distance to the
					// nearest optimal edge. Lower score is better. Ties broken randomly.
					// Candidates with currentRange outside their legal range are excluded entirely.
					var scoredCandidateList = candidateList
						.Select(a =>
						{
							var ac = a.GeneralWeapon;

							Debug.Assert(ac != null);

							var wMinRange = ac.Field11;
							var wOptMin = ac.Field12;
							var wOptMax = ac.Field13;
							var wMaxRange = ac.Field14;

							// Must be legally usable at current range
							if (currentRange < wMinRange || currentRange > wMaxRange)
							{
								return (artifact: a, score: long.MaxValue);
							}

							long score;

							if (currentRange >= wOptMin && currentRange <= wOptMax)
							{
								score = 0;
							}
							else if (currentRange < wOptMin)
							{
								score = wOptMin - currentRange;
							}
							else
							{
								score = currentRange - wOptMax;
							}

							return (artifact: a, score);
						})
						.Where(x => x.score < long.MaxValue)
						.ToList();

					if (scoredCandidateList.Count == 0)
					{
						// No candidate is usable at current range — keep current weapon
						goto Cleanup;
					}

					var minScore = scoredCandidateList.Min(x => x.score);

					// Only switch if a candidate scores strictly better than the current weapon.
					// >= means ties go to the current weapon — no switch, no oscillation.
					if (minScore >= currentWeaponScore)
					{
						goto Cleanup;
					}

					bestWeapon = scoredCandidateList
						.Where(x => x.score == minScore)
						.OrderBy(_ => gEngine.RollDice(1, 100, 0))
						.Select(x => x.artifact)
						.First();

					break;
				}

				case RearmCode.Strongest:
				{
					// Highest average damage (Dice * Sides) among all carried candidates,
					// regardless of whether the range is optimal or even legal.
					// Score the current weapon first — only switch if a candidate does
					// strictly more damage, preventing oscillation between equal weapons.
					var currentAc = gADB[LoopMonster.Weapon]?.GeneralWeapon;

					var currentDmg = currentAc != null
						? currentAc.Field3 * currentAc.Field4
						: 0;

					var maxDmg = candidateList
						.Max(a => a.GeneralWeapon.Field3 * a.GeneralWeapon.Field4);

					// Only switch if a candidate does strictly more damage than current weapon.
					if (maxDmg <= currentDmg)
					{
						goto Cleanup;
					}

					bestWeapon = candidateList
						.Where(a => a.GeneralWeapon.Field3 * a.GeneralWeapon.Field4 == maxDmg)
						.OrderBy(_ => gEngine.RollDice(1, 100, 0))
						.First();

					break;
				}

				case RearmCode.Random:
				{
					// Random switching is intentional — no improvement check needed.
					bestWeapon = candidateList
						.OrderBy(_ => gEngine.RollDice(1, 100, 0))
						.First();

					break;
				}
			}

			if (bestWeapon == null)
			{
				goto Cleanup;
			}

			var origWeapon = gADB[LoopMonster.Weapon];

			Debug.Assert(origWeapon != null);

			origWeapon.RemoveStateDesc(origWeapon.GetReadyWeaponDesc());

			LoopMonster.Weapon = -1;

			gEngine.LoopRearmArtifact = bestWeapon;

			NextState = gEngine.CreateInstance<IMonsterMemberLoopInitializeState>();

			GotoCleanup = true;

		Cleanup:

			;
		}

		public virtual void MonsterRepositions()
		{
			// Gate 1: TravelCode.None — movement system completely inactive for this monster
			if (LoopMonster.TravelCode == TravelCode.None || LoopMonster.FocusMonsterUid == 0)
			{
				goto Cleanup;
			}

			// Gate 2: TravelTurns frequency — only evaluate on the correct turn interval
			if (LoopMonster.TravelTurns > 1 && gGameState.CurrTurn % LoopMonster.TravelTurns != 0)
			{
				goto Cleanup;
			}

			// Gate 3: TravelOdds — fires unconditionally regardless of range band, including
			// below MinRange. A failed roll means the monster skips repositioning entirely and
			// falls through to the attack state machine. Below MinRange that means the weapon
			// is blocked and the monster idles — this is intentional and rewards players who
			// successfully close an archer.
			if (LoopMonster.TravelOdds < 100)
			{
				var rl = gEngine.RollDice(1, 100, 0);

				if (rl > LoopMonster.TravelOdds)
				{
					goto Cleanup;
				}
			}

			FocusMonster = gMDB[LoopMonster.FocusMonsterUid];

			Debug.Assert(FocusMonster != null);

			var currentRange = gEngine.GetRange(LoopMonster.Coord, FocusMonster.Coord);

			var travelRange = LoopMonster.GetTravelRange();

			switch (LoopMonster.TravelCode)
			{
				case TravelCode.Close:
					MonsterRepositionsClose(currentRange, travelRange);
					break;

				case TravelCode.Maintain:
					MonsterRepositionsMaintain(currentRange, travelRange);
					break;

				case TravelCode.Open:
					MonsterRepositionsOpen(currentRange, travelRange);
					break;
			}

		Cleanup:

			;
		}

		public virtual void MonsterRepositionsClose(long currentRange, long travelRange)
		{
			// TravelCode.Close — always minimizes distance regardless of optimal band.
			// Stops naturally when currentRange reaches 0.
			if (currentRange == 0)
			{
				goto Cleanup;
			}

			ActionCommand = gEngine.CreateInstance<IApproachCommand>(x =>
			{
				x.ActorMonster = LoopMonster;
				x.ActorRoom = LoopMonsterRoom;
				x.Dobj = FocusMonster;
				x.Range = Math.Min(currentRange, travelRange);
			});

			ActionCommand.Execute();

			if (ActionCommand is IApproachCommand approachCommand && approachCommand.PartialTurnMove)
			{
				NextState = gEngine.CreateInstance<IMonsterMemberLoopInitializeState>();
			}

			GotoCleanup = true;

		Cleanup:

			;
		}

		public virtual void MonsterRepositionsMaintain(long currentRange, long travelRange)
		{
			// TravelCode.Maintain — stays within optimal range band for current weapon.
			// Approaches if too far, retreats if too close or below MinRange.
			// Wrap-around is used when cornered to escape past the focus monster.

			long minRange;

			long optimalMin;

			long optimalMax;

			var artifact = LoopMonster.Weapon > 0 ? gADB[LoopMonster.Weapon] : null;

			var ac = artifact != null ? artifact.GeneralWeapon : null;

			if (ac != null)
			{
				minRange   = ac.Field11;
				optimalMin = ac.Field12;
				optimalMax = ac.Field13;
			}
			else
			{
				minRange   = LoopMonster.NwMinRange;
				optimalMin = LoopMonster.NwOptimalMin;
				optimalMax = LoopMonster.NwOptimalMax;
			}

			// Already optimal — nothing to do, fall through to attack state machine
			if (currentRange >= optimalMin && currentRange <= optimalMax)
			{
				goto Cleanup;
			}

			if (currentRange > optimalMax)
			{
				// Too far — approach to close gap to optimalMax exactly if travelRange allows
				ActionCommand = gEngine.CreateInstance<IApproachCommand>(x =>
				{
					x.ActorMonster = LoopMonster;
					x.ActorRoom = LoopMonsterRoom;
					x.Dobj = FocusMonster;
					x.Range = Math.Min(currentRange - optimalMax, travelRange);
				});
			}
			else if (currentRange >= minRange)
			{
				// Suboptimal close (>= minRange but < optimalMin) — retreat to gain distance.
				// Maximize distance gained per retreat to reduce oscillation.
				var distanceToWall = FocusMonster.Coord > LoopMonster.Coord
					? LoopMonster.Coord
					: LoopMonsterRoom.MaxCoord - LoopMonster.Coord;

				if (distanceToWall > 0)
				{
					// Room to retreat normally
					ActionCommand = gEngine.CreateInstance<IRetreatCommand>(x =>
					{
						x.ActorMonster = LoopMonster;
						x.ActorRoom = LoopMonsterRoom;
						x.Dobj = FocusMonster;
						x.Range = travelRange;
					});
				}
				else
				{
					// Cornered against wall with weapon still in legal range.
					// Wrap around through the focus monster to gain distance on the other side.
					// TravelOdds < 100 prevents this from becoming an infinite chase.
					if (currentRange < travelRange)
					{
						// Capture retreat direction BEFORE teleporting — once coords are equal
						// RetreatCommand cannot determine which way to go.
						var retreatDirection = LoopMonster.Coord < FocusMonster.Coord ? 1 : 0;

						// Silently move to focus monster coord, then retreat with remaining movement.
						// The retreat message tells the player-facing story of this maneuver.
						LoopMonster.Coord = FocusMonster.Coord;

						var remainingRange = travelRange - currentRange;

						var distanceToFarWall = retreatDirection == 0
							? LoopMonster.Coord
							: LoopMonsterRoom.MaxCoord - LoopMonster.Coord;

						if (remainingRange > 0 && distanceToFarWall > 0)
						{
							ActionCommand = gEngine.CreateInstance<IRetreatCommand>(x =>
							{
								x.ActorMonster = LoopMonster;
								x.ActorRoom = LoopMonsterRoom;
								x.Dobj = FocusMonster;
								x.Range = remainingRange;
								x.Direction = retreatDirection;
							});
						}
						else
						{
							// Teleport succeeded but no room to retreat further — GotoCleanup
							// without executing a command. Monster has still gained position.
							GotoCleanup = true;
							goto Cleanup;
						}
					}
					else
					{
						// travelRange insufficient to reach focus monster — cannot wrap around.
						// Attack with suboptimal penalty this turn.
						goto Cleanup;
					}
				}
			}
			else
			{
				// Below MinRange — weapon completely blocked.
				// Retreat if room allows, wrap around if cornered.
				// No attack fallback here — weapon is unusable below MinRange.
				var distanceToWall = FocusMonster.Coord > LoopMonster.Coord
					? LoopMonster.Coord
					: LoopMonsterRoom.MaxCoord - LoopMonster.Coord;

				if (distanceToWall > 0)
				{
					// Room to retreat normally
					ActionCommand = gEngine.CreateInstance<IRetreatCommand>(x =>
					{
						x.ActorMonster = LoopMonster;
						x.ActorRoom = LoopMonsterRoom;
						x.Dobj = FocusMonster;
						x.Range = travelRange;
					});
				}
				else
				{
					// Cornered below MinRange — wrap around through the focus monster.
					// Same mechanic as the suboptimal-close wrap-around above.
					if (currentRange < travelRange)
					{
						// Capture retreat direction BEFORE teleporting.
						var retreatDirection = LoopMonster.Coord < FocusMonster.Coord ? 1 : 0;

						LoopMonster.Coord = FocusMonster.Coord;

						var remainingRange = travelRange - currentRange;

						var distanceToFarWall = retreatDirection == 0
							? LoopMonster.Coord
							: LoopMonsterRoom.MaxCoord - LoopMonster.Coord;

						if (remainingRange > 0 && distanceToFarWall > 0)
						{
							ActionCommand = gEngine.CreateInstance<IRetreatCommand>(x =>
							{
								x.ActorMonster = LoopMonster;
								x.ActorRoom = LoopMonsterRoom;
								x.Dobj = FocusMonster;
								x.Range = remainingRange;
								x.Direction = retreatDirection;
							});
						}
						else
						{
							// Teleport succeeded but no room to retreat further — GotoCleanup
							// without executing a command. Monster has still gained position.
							GotoCleanup = true;
							goto Cleanup;
						}
					}
					else
					{
						GotoCleanup = true;

						// Cannot reach focus monster coord — truly stuck this turn.
						// RearmCode may switch to a melee weapon next evaluation,
						// or MonsterFleesCheck will evacuate the room next turn.
						goto Cleanup;
					}
				}
			}

			ActionCommand.Execute();

			if ((ActionCommand is IApproachCommand approachCmd && approachCmd.PartialTurnMove) || (ActionCommand is IRetreatCommand retreatCmd && retreatCmd.PartialTurnMove))
			{
				NextState = gEngine.CreateInstance<IMonsterMemberLoopInitializeState>();
			}

			GotoCleanup = true;

		Cleanup:

			;
		}

		public virtual void MonsterRepositionsOpen(long currentRange, long travelRange)
		{
			// TravelCode.Open — always maximizes distance from focus monster regardless of
			// optimal band. Used for cowardly or support monsters.
			// Wrap-around used when already at maximum wall distance.

			long minRange;

			long maxRange;

			var artifact = LoopMonster.Weapon > 0 ? gADB[LoopMonster.Weapon] : null;

			var ac = artifact != null ? artifact.GeneralWeapon : null;

			if (ac != null)
			{
				minRange = ac.Field11;
				maxRange = ac.Field14;
			}
			else
			{
				minRange = LoopMonster.NwMinRange;
				maxRange = LoopMonster.NwMaxRange;
			}

			var distanceToWall = FocusMonster.Coord > LoopMonster.Coord
				? LoopMonster.Coord
				: LoopMonsterRoom.MaxCoord - LoopMonster.Coord;

			if (distanceToWall > 0)
			{
				// Room to retreat — maximize distance
				ActionCommand = gEngine.CreateInstance<IRetreatCommand>(x =>
				{
					x.ActorMonster = LoopMonster;
					x.ActorRoom = LoopMonsterRoom;
					x.Dobj = FocusMonster;
					x.Range = travelRange;
				});
			}
			else
			{
				// Already at maximum distance against the wall.
				// Wrap around through the focus monster to gain distance on the other side.
				if (currentRange < travelRange)
				{
					// Capture retreat direction BEFORE teleporting.
					var retreatDirection = LoopMonster.Coord < FocusMonster.Coord ? 1 : 0;

					LoopMonster.Coord = FocusMonster.Coord;

					var remainingRange = travelRange - currentRange;

					var distanceToFarWall = retreatDirection == 0
						? LoopMonster.Coord
						: LoopMonsterRoom.MaxCoord - LoopMonster.Coord;

					if (remainingRange > 0 && distanceToFarWall > 0)
					{
						ActionCommand = gEngine.CreateInstance<IRetreatCommand>(x =>
						{
							x.ActorMonster = LoopMonster;
							x.ActorRoom = LoopMonsterRoom;
							x.Dobj = FocusMonster;
							x.Range = remainingRange;
							x.Direction = retreatDirection;
						});
					}
					else
					{
						// Teleport succeeded but no room to retreat further — GotoCleanup
						// without executing a command. Monster has still gained position.
						GotoCleanup = true;
						goto Cleanup;
					}
				}
				else
				{
					if (currentRange < minRange || currentRange > maxRange)
					{
						GotoCleanup = true;
					}

					// Cannot wrap around — truly maximally distant, idle this turn (or attack if in range)
					goto Cleanup;
				}
			}

			ActionCommand.Execute();

			if (ActionCommand is IRetreatCommand retreatCommand && retreatCommand.PartialTurnMove)
			{
				NextState = gEngine.CreateInstance<IMonsterMemberLoopInitializeState>();
			}

			GotoCleanup = true;

		Cleanup:

			;
		}

		public MonsterActionState()
		{
			Name = "MonsterActionState";
		}
	}
}
