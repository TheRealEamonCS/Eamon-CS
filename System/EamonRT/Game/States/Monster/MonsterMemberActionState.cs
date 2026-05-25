
// MonsterMemberActionState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Eamon.Framework;
using Eamon.Framework.Primitive.Classes;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using Enums = Eamon.Framework.Primitive.Enums;
using static EamonRT.Game.Plugin.Globals;

namespace EamonRT.Game.States
{
	[ClassMappings]
	public class MonsterMemberActionState : State, IMonsterMemberActionState
	{
		/// <summary></summary>
		public Enums.Spell _spellCast;

		/// <summary></summary>
		public IGameBase _spellTarget;

		/// <summary></summary>
		public virtual IMonster LoopMonster { get; set; }

		/// <summary></summary>
		public virtual IRoom LoopMonsterRoom { get; set; }

		/// <summary></summary>
		public virtual IArtifact WeaponArtifact { get; set; }

		/// <summary></summary>
		public virtual IList<IArtifact> WeaponArtifactList { get; set; }

		/// <summary></summary>
		public virtual IMonsterSpell MonsterSpell { get; set; }

		/// <summary></summary>
		public virtual ICommand ActionCommand { get; set; }

		/// <summary></summary>
		public virtual long WeaponArtifactListIndex { get; set; }

		/// <summary></summary>
		public virtual ContainerType WeaponContainerType { get; set; }

		/// <summary></summary>
		public virtual string ContainerPrepName { get; set; }

		public override void Execute()
		{
			LoopMonster = gMDB[gEngine.LoopMonsterUid];

			Debug.Assert(LoopMonster != null);

			LoopMonsterRoom = LoopMonster.GetInRoom();

			Debug.Assert(LoopMonsterRoom != null);

			MonsterMemberMiscActionCheck();

			if (GotoCleanup)
			{
				goto Cleanup;
			}

			MonsterMemberReadiesWeaponCheck();

			if (GotoCleanup)
			{
				goto Cleanup;
			}

			MonsterMemberMiscActionCheck01();

			if (GotoCleanup)
			{
				goto Cleanup;
			}

			MonsterMemberReadiesNaturalWeaponCheck();

			if (GotoCleanup)
			{
				goto Cleanup;
			}

			MonsterMemberMiscActionCheck02();

			if (GotoCleanup)
			{
				goto Cleanup;
			}

			MonsterMemberCastsSpellCheck();

			if (GotoCleanup)
			{
				goto Cleanup;
			}

			MonsterMemberMiscActionCheck03();

			if (GotoCleanup)
			{
				goto Cleanup;
			}

			MonsterMemberAttacksEnemyCheck();

			if (GotoCleanup)
			{
				goto Cleanup;
			}

			MonsterMemberMiscActionCheck04();

			if (GotoCleanup)
			{
				goto Cleanup;
			}

		Cleanup:

			if (NextState == null)
			{
				NextState = gEngine.CreateInstance<IMonsterMemberLoopIncrementState>();
			}

			gEngine.NextState = NextState;
		}

		public virtual void MonsterMemberMiscActionCheck()
		{
			// Do nothing
		}

		public virtual void MonsterMemberReadiesWeaponCheck()
		{
			if (LoopMonster.ShouldReadyWeapon() && (((LoopMonster.CombatCode == CombatCode.NaturalWeapons || LoopMonster.CombatCode == CombatCode.NaturalAttacks) && LoopMonster.Weapon <= 0) || ((LoopMonster.CombatCode == CombatCode.Weapons || LoopMonster.CombatCode == CombatCode.Attacks) && LoopMonster.Weapon < 0)))
			{
				if (gGameState.EnhancedCombat && gEngine.LoopRearmArtifact != null)
				{
					// Rearm already selected the best weapon this turn -- use it directly.
					WeaponArtifactList = new List<IArtifact>() { gEngine.LoopRearmArtifact };

					gEngine.LoopRearmArtifact = null;
				}
				else if (gEngine.EnforceRangeUsage && LoopMonster.RearmCode != RearmCode.None && LoopMonster.Weapon >= -1)
				{
					// Weapon was lost mid-combat (broken, fumbled, etc.) and Rearm has not
					// run yet this turn. Apply RearmCode selection logic now so the Monster
					// readies the tactically correct weapon rather than the first available.
					// Falls back to BuildLoopWeaponArtifactList if no suitable weapon is found.
					var bestWeapon = MonsterMemberSelectBestWeapon();

					WeaponArtifactList = bestWeapon != null
						? new List<IArtifact>() { bestWeapon }
						: gEngine.BuildLoopWeaponArtifactList(LoopMonster);
				}
				else
				{
					WeaponArtifactList = gEngine.BuildLoopWeaponArtifactList(LoopMonster);
				}

				if (WeaponArtifactList != null && WeaponArtifactList.Count > 0)
				{
					for (WeaponArtifactListIndex = 0; WeaponArtifactListIndex < WeaponArtifactList.Count; WeaponArtifactListIndex++)
					{
						WeaponArtifact = WeaponArtifactList[(int)WeaponArtifactListIndex];

						Debug.Assert(WeaponArtifact != null);

						if (!WeaponArtifact.IsCarriedByMonster(LoopMonster))
						{
							WeaponContainerType = WeaponArtifact.GetCarriedByContainerContainerType();

							if (Enum.IsDefined(typeof(ContainerType), WeaponContainerType))
							{
								ContainerPrepName = gEngine.EvalContainerType(WeaponContainerType, "in", "on", "under", "behind");

								ActionCommand = gEngine.CreateInstance<IRemoveCommand>(x =>
								{
									x.ActorMonster = LoopMonster;

									x.ActorRoom = LoopMonsterRoom;

									x.Dobj = WeaponArtifact;

									x.Iobj = WeaponArtifact.GetCarriedByContainer();

									x.Prep = gEngine.Preps.FirstOrDefault(prep => prep.Name.Equals(ContainerPrepName, StringComparison.OrdinalIgnoreCase));
								});
							}
							else
							{
								ActionCommand = gEngine.CreateInstance<IGetCommand>(x =>
								{
									x.ActorMonster = LoopMonster;

									x.ActorRoom = LoopMonsterRoom;

									x.Dobj = WeaponArtifact;
								});
							}

							ActionCommand.Execute();

							try
							{
								gEngine.UseRevealContentMonsterTheName = true;

								gEngine.CheckRevealContainerContents();
							}
							finally
							{
								gEngine.UseRevealContentMonsterTheName = false;
							}
						}

						if (WeaponArtifact.IsCarriedByMonster(LoopMonster))
						{
							ActionCommand = gEngine.CreateInstance<IReadyCommand>(x =>
							{
								x.ActorMonster = LoopMonster;

								x.ActorRoom = LoopMonsterRoom;

								x.Dobj = WeaponArtifact;
							});

							ActionCommand.Execute();
						}

						if (LoopMonster.Weapon > 0)
						{
							GotoCleanup = true;

							break;
						}
					}
				}
			}
		}

		public virtual IArtifact MonsterMemberSelectBestWeapon()
		{
			IArtifact result = null;

			// Applies RearmCode selection logic to pick the best replacement weapon
			// from the Monster's carried inventory when a weapon has been lost mid-combat
			// and Rearm has not yet run this turn. Returns null if no suitable weapon is
			// found, in which case the caller falls back to BuildLoopWeaponArtifactList.
			//
			// Mirrors MonsterRearmsWeaponCheck scoring but without TurnsCheck, OddsRoll,
			// or improvement guard -- when a weapon is lost the Monster must pick something,
			// so all candidates are considered unconditionally.

			var candidateList = gEngine.GetReadyableWeaponList(LoopMonster)
				.Where(a => a.IsCarriedByMonster(LoopMonster))
				.ToList();

			if (candidateList.Count == 0)
			{
				goto Cleanup;
			}

			// Current range to focus target. If no focus target is set yet, use 0 --
			// MonsterSelectsFocusTarget will set it later this turn.
			var currentRange = LoopMonster.FocusMonsterUid > 0
				? gEngine.GetRange(LoopMonster.Coord, gMDB[LoopMonster.FocusMonsterUid]?.Coord ?? LoopMonster.Coord)
				: 0;

			switch (LoopMonster.RearmCode)
			{
				case RearmCode.BestForRange:
				{
					// Score each candidate by how well its optimal band fits the current range.
					// Score = 0 if inside optimal band; otherwise distance to nearest edge.
					// Candidates not legally usable at current range are excluded.
					var scoredCandidateList = candidateList
						.Select(a =>
						{
							var ac = a.GeneralWeapon;

							Debug.Assert(ac != null);

							var wMinRange = ac.Field11;
							var wOptMin   = ac.Field12;
							var wOptMax   = ac.Field13;
							var wMaxRange = ac.Field14;

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
						goto Cleanup;
					}

					var minScore = scoredCandidateList.Min(x => x.score);

					result = scoredCandidateList
						.Where(x => x.score == minScore)
						.OrderBy(_ => gEngine.RollDice(1, 100, 0))
						.Select(x => x.artifact)
						.First();

					break;
				}

				case RearmCode.Strongest:
				{
					// Pick the highest average damage weapon regardless of range fit.
					var maxDmg = candidateList
						.Max(a => a.GeneralWeapon.Field3 * a.GeneralWeapon.Field4);

					result = candidateList
						.Where(a => a.GeneralWeapon.Field3 * a.GeneralWeapon.Field4 == maxDmg)
						.OrderBy(_ => gEngine.RollDice(1, 100, 0))
						.First();

					break;
				}

				case RearmCode.Random:
				{
					result = candidateList
						.OrderBy(_ => gEngine.RollDice(1, 100, 0))
						.First();

					break;
				}

				default:
				{
					// Do nothing

					break;
				}
			}

		Cleanup:

			return result;
		}

		public virtual void MonsterMemberMiscActionCheck01()
		{
			// Do nothing
		}

		public virtual void MonsterMemberReadiesNaturalWeaponCheck()
		{
			if ((LoopMonster.CombatCode == CombatCode.NaturalWeapons || LoopMonster.CombatCode == CombatCode.NaturalAttacks) && LoopMonster.Weapon < 0)
			{
				LoopMonster.Weapon = 0;
			}
		}

		public virtual void MonsterMemberMiscActionCheck02()
		{
			// Do nothing
		}

		public virtual void MonsterMemberCastsSpellCheck()
		{
			if (LoopMonster.ShouldCastSpell(ref _spellCast, ref _spellTarget))
			{
				MonsterSpell = LoopMonster.GetMonsterSpell(_spellCast);

				if (MonsterSpell != null)
				{
					ActionCommand = null;

					switch (_spellCast)
					{
						case Spell.Blast:

							if (LoopMonster.CombatCode != CombatCode.NeverFights)
							{
								Debug.Assert(_spellTarget != null);

								ActionCommand = gEngine.CreateInstance<IBlastCommand>();
							}

							break;

						case Spell.Heal:

							ActionCommand = gEngine.CreateInstance<IHealCommand>();

							break;

						case Spell.Speed:

							Debug.Assert(_spellTarget == null);

							ActionCommand = gEngine.CreateInstance<ISpeedCommand>();

							break;

						case Spell.Power:

							Debug.Assert(_spellTarget == null);

							ActionCommand = gEngine.CreateInstance<IPowerCommand>();

							break;
					}

					if (ActionCommand != null)
					{
						ActionCommand.NextState = gEngine.CreateInstance<IMonsterMemberLoopIncrementState>();

						ActionCommand.ActorMonster = LoopMonster;

						ActionCommand.ActorRoom = LoopMonsterRoom;

						ActionCommand.Dobj = _spellTarget;

						ActionCommand.Execute();

						NextState = ActionCommand.NextState;

						GotoCleanup = true;
					}
				}
			}
		}

		public virtual void MonsterMemberMiscActionCheck03()
		{
			// Do nothing
		}

		public virtual void MonsterMemberAttacksEnemyCheck()
		{
			if (LoopMonster.CombatCode != CombatCode.NeverFights && LoopMonster.CheckNBTLHostility() && LoopMonster.Weapon >= 0)
			{
				NextState = gEngine.CreateInstance<IMonsterAttackLoopInitializeState>();

				GotoCleanup = true;
			}
		}

		public virtual void MonsterMemberMiscActionCheck04()
		{
			// Do nothing
		}

		public MonsterMemberActionState()
		{
			Name = "MonsterMemberActionState";
		}
	}
}
