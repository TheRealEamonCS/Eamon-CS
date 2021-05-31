
// CombatSystem.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Combat;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static TheDeepCanyon.Game.Plugin.PluginContext;

namespace TheDeepCanyon.Game.Combat
{
	[ClassMappings]
	public class CombatSystem : EamonRT.Game.Combat.CombatSystem, ICombatSystem
	{
		public override void ExecuteAttack()
		{
			Debug.Assert(OfMonster != null);

			Debug.Assert(DfMonster != null);

			var origAgility = 0L;

			// Various bats versus falcon

			if (DfMonster.Uid > 6 && DfMonster.Uid < 11 && OfMonster.Weapon == 5)
			{
				origAgility = DfMonster.Agility;

				DfMonster.Agility /= 3;
			}

			base.ExecuteAttack();

			if (origAgility != 0)
			{
				DfMonster.Agility = origAgility;
			}
		}

		public override void PrintWeaponHitsUser()
		{
			// Falcon

			if (OfWeapon.Uid == 5)
			{
				gOut.Write("{0}  Weapon injures user!", Environment.NewLine);
			}
			else
			{
				base.PrintWeaponHitsUser();
			}
		}

		public override void PrintSparksFly()
		{
			// Falcon

			if (OfWeapon.Uid == 5)
			{
				gOut.Write("{0}  {1} fails to launch!",
					Environment.NewLine,
					OfMonster.IsCharacterMonster() || OfMonster.IsInRoomLit() ?
						(
							(WeaponRevealType == WeaponRevealType.Never ||
							(WeaponRevealType == WeaponRevealType.OnlyIfSeen && !OfWeapon.Seen)) ?
								OfWeapon.GetArticleName(true) :
								OfWeapon.GetTheName(true)
						) :
						"A weapon");
			}
			else
			{
				base.PrintSparksFly();
			}
		}

		public override void PrintWeaponDamaged()
		{
			// Falcon

			if (OfWeapon.Uid == 5)
			{
				gOut.Write("{0}  Weapon injured!", Environment.NewLine);
			}
			else
			{
				base.PrintWeaponDamaged();
			}
		}

		public override void PrintWeaponBroken()
		{
			// Falcon

			if (OfWeapon.Uid == 5)
			{
				gOut.Write("{0}  Weapon flees!", Environment.NewLine);
			}
			else
			{
				base.PrintWeaponBroken();
			}
		}

		public override void PrintBrokenWeaponHitsUser()
		{
			// Falcon

			if (OfWeapon.Uid == 5)
			{
				gOut.Write("{0}  Fleeing weapon injures user!", Environment.NewLine);
			}
			else
			{
				base.PrintBrokenWeaponHitsUser();
			}
		}

		public override void PrintHealthStatus()
		{
			base.PrintHealthStatus();

			var room = DfMonster.GetInRoom();

			Debug.Assert(room != null);

			if (DfMonster.IsDead())
			{
				gOut.Print("{0}{1} dead, Jim.", Environment.NewLine, DfMonster.IsCharacterMonster() || room.IsLit() ? DfMonster.EvalGender("He's", "She's", "It's") : "It's");
			}
		}

		public override void AttackHit()
		{
			base.AttackHit();

			var room = DfMonster.GetInRoom();

			Debug.Assert(room != null);

			var netArtifact = gADB[24];

			Debug.Assert(netArtifact != null);

			var rl = gEngine.RollDice(1, 100, 0);

			var isNetCarriedByDefender = DfMonster.IsCharacterMonster() ? netArtifact.IsCarriedByCharacter() : netArtifact.IsCarriedByMonster(DfMonster);

			// Various bats strangled by net

			if (OfMonster.Uid > 6 && OfMonster.Uid < 11 && isNetCarriedByDefender && rl > 50)
			{
				if (DfMonster.IsCharacterMonster() || room.IsLit())
				{
					gOut.Write("{0}{1}{2} flies into the net that {3} carrying and is strangled!", Environment.NewLine, OmitBboaPadding ? "" : "  ", room.EvalLightLevel("The offender", OfMonster.GetTheName(true)), DfMonster.IsCharacterMonster() ? "you are" : DfMonster.GetTheName() + " is");
				}
				else
				{
					gOut.Write("{0}{1}The offender is strangled by the defender!", Environment.NewLine, OmitBboaPadding ? "" : "  ");
				}

				DfMonster = OfMonster;

				_d2 = DfMonster.Hardiness - DfMonster.DmgTaken;

				CombatState = CombatState.CheckArmor;
			}
		}

		public override void CheckMonsterStatus()
		{
			Debug.Assert(DfMonster != null);

			Debug.Assert(_d2 >= 0);

			DfMonster.DmgTaken += _d2;

			if (!OmitMonsterStatus || OfMonster == DfMonster)
			{
				PrintHealthStatus();
			}

			if (DfMonster.IsDead())
			{
				var room = DfMonster.GetInRoom();

				Debug.Assert(room != null);

				var ringArtifact = gADB[22];

				Debug.Assert(ringArtifact != null);

				if (DfMonster.IsCharacterMonster())
				{
					// Resurrect

					if (ringArtifact.IsCarriedByCharacter() || ringArtifact.IsWornByCharacter())
					{
						gOut.Print("{0}", Globals.LineSep);

						gOut.Write("{0}Press any key to continue: ", Environment.NewLine);

						Globals.Buf.Clear();

						var rc = Globals.In.ReadField(Globals.Buf, Constants.BufSize02, null, ' ', '\0', true, null, gEngine.ModifyCharToNull, null, gEngine.IsCharAny);

						Debug.Assert(gEngine.IsSuccess(rc));

						Globals.Thread.Sleep(150);

						gOut.Print("{0}", Globals.LineSep);

						// gSentenceParser.PrintDiscardingCommands() not called for this abrupt reality shift

						gSentenceParser.Clear();

						gEngine.PrintEffectDesc(3);

						DfMonster.DmgTaken = 0;

						gEngine.ResetMonsterStats(DfMonster);

						var stat = gEngine.GetStats(Stat.Hardiness);

						Debug.Assert(stat != null);

						DfMonster.Hardiness -= (long)Math.Round((double)DfMonster.Hardiness * 0.4);

						if (DfMonster.Hardiness < stat.MinValue)
						{
							DfMonster.Hardiness = stat.MinValue;
						}

						stat = gEngine.GetStats(Stat.Agility);

						Debug.Assert(stat != null);

						DfMonster.Agility -= (long)Math.Round((double)DfMonster.Agility * 0.4);

						if (DfMonster.Agility < stat.MinValue)
						{
							DfMonster.Agility = stat.MinValue;
						}

						DfWeapon = DfMonster.Weapon > 0 ? gADB[DfMonster.Weapon] : null;

						if (DfWeapon != null)
						{
							gOut.EnableOutput = false;

							var dropCommand = Globals.CreateInstance<IDropCommand>(x =>
							{
								x.ActorMonster = DfMonster;

								x.ActorRoom = room;

								x.Dobj = DfWeapon;
							});

							dropCommand.Execute();

							gOut.EnableOutput = true;
						}

						ringArtifact.SetInLimbo();

						room = gRDB[1];

						Debug.Assert(room != null);

						room.Seen = false;

						gEngine.EnforceCharacterWeightLimits02(room);

						gGameState.Ro = 1;

						gGameState.R2 = gGameState.Ro;

						if (SetNextStateFunc != null)
						{
							SetNextStateFunc(Globals.CreateInstance<IAfterPlayerMoveState>(x =>
							{
								x.MoveMonsters = false;
							}));
						}
					}
					else
					{
						gGameState.Die = 1;

						if (SetNextStateFunc != null)
						{
							SetNextStateFunc(Globals.CreateInstance<IPlayerDeadState>(x =>
							{
								x.PrintLineSep = true;
							}));
						}
					}
				}
				else
				{
					// Resurrect

					if (ringArtifact.IsCarriedByMonster(DfMonster) || ringArtifact.IsWornByMonster(DfMonster))
					{
						Globals.ResurrectMonsterUid = DfMonster.Uid;

						DfMonster.SetInLimbo();

						ringArtifact.SetInLimbo();
					}
					else
					{
						gEngine.MonsterDies(OfMonster, DfMonster);
					}
				}
			}

			CombatState = CombatState.EndAttack;
		}
	}
}
