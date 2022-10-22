
// CombatComponent.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using Eamon.Framework;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Components;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static TheDeepCanyon.Game.Plugin.Globals;

namespace TheDeepCanyon.Game.Components
{
	[ClassMappings]
	public class CombatComponent : EamonRT.Game.Components.CombatComponent, ICombatComponent
	{
		public override void ExecuteAttack()
		{
			var origAgility = 0L;

			// Various bats versus falcon

			if (DobjMonster != null && DobjMonster.Uid > 6 && DobjMonster.Uid < 11 && !BlastSpell && ActorMonster?.Weapon == 5)
			{
				origAgility = DobjMonster.Agility;

				DobjMonster.Agility /= 3;
			}

			base.ExecuteAttack();

			if (DobjMonster != null && origAgility != 0)
			{
				DobjMonster.Agility = origAgility;
			}
		}

		public override void PrintWeaponHitsUser()
		{
			// Falcon

			if (ActorWeapon.Uid == 5)
			{
				gOut.Write("{0}  Weapon injures user!", Environment.NewLine);
			}
			else
			{
				base.PrintWeaponHitsUser();
			}
		}

		public override void PrintSparksFly(IRoom room, IMonster monster, IArtifact weapon, WeaponRevealType weaponRevealType)
		{
			Debug.Assert(room != null && monster != null && weapon != null /* && Enum.IsDefined(typeof(WeaponRevealType), weaponRevealType) */);

			// Falcon

			if (weapon.Uid == 5)
			{
				gOut.Write("{0}  {1} fails to launch!",
					Environment.NewLine,
					monster.IsCharacterMonster() || room.IsLit() ?
						(
							(weaponRevealType == WeaponRevealType.Never ||
							(weaponRevealType == WeaponRevealType.OnlyIfSeen && !weapon.Seen)) ?
								weapon.GetArticleName(true) :
								weapon.GetTheName(true)
						) :
						"A weapon");
			}
			else
			{
				base.PrintSparksFly(room, monster, weapon, weaponRevealType);
			}
		}

		public override void PrintWeaponDamaged()
		{
			// Falcon

			if (ActorWeapon.Uid == 5)
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

			if (ActorWeapon.Uid == 5)
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

			if (ActorWeapon.Uid == 5)
			{
				gOut.Write("{0}  Fleeing weapon injures user!", Environment.NewLine);
			}
			else
			{
				base.PrintBrokenWeaponHitsUser();
			}
		}

		public override void PrintHealthStatus(IRoom room, IMonster actorMonster, IMonster dobjMonster, bool blastSpell)
		{
			Debug.Assert(room != null && dobjMonster != null);

			base.PrintHealthStatus(room, actorMonster, dobjMonster, blastSpell);

			if (dobjMonster.IsDead())
			{
				gOut.Print("{0}{1} dead, Jim.", Environment.NewLine, dobjMonster.IsCharacterMonster() || room.IsLit() ? dobjMonster.EvalGender("He's", "She's", "It's") : "It's");
			}
		}

		public override void AttackMiss()
		{
			// Attack always hits sleeping Fido

			if (DobjMonster.Uid == 11 && gGameState.FidoSleepCounter > 0)
			{
				if (_rl < 97 || ActorWeaponUid == 0)
				{
					_rl = _odds;

					CombatState = CombatState.AttackHit;

					goto Cleanup;
				}
			}

			base.AttackMiss();

		Cleanup:

			;
		}

		public override void AttackHit()
		{
			base.AttackHit();

			var room = DobjMonster.GetInRoom();

			Debug.Assert(room != null);

			var netArtifact = gADB[24];

			Debug.Assert(netArtifact != null);

			var rl = gEngine.RollDice(1, 100, 0);

			var isNetCarriedByDefender = DobjMonster.IsCharacterMonster() ? netArtifact.IsCarriedByCharacter() : netArtifact.IsCarriedByMonster(DobjMonster);

			// Various bats strangled by net

			if (ActorMonster.Uid > 6 && ActorMonster.Uid < 11 && isNetCarriedByDefender && rl > 50)
			{
				if (DobjMonster.IsCharacterMonster() || room.IsLit())
				{
					gOut.Print("{0}{1} flies into the net that {2} carrying and is strangled!", Environment.NewLine, room.EvalLightLevel("The offender", ActorMonster.GetTheName(true)), DobjMonster.IsCharacterMonster() ? "you are" : DobjMonster.GetTheName() + " is");
				}
				else
				{
					gOut.Print("{0}The offender is strangled by the defender!", Environment.NewLine);
				}

				Dobj = ActorMonster;

				_d2 = DobjMonster.Hardiness - DobjMonster.DmgTaken;

				CombatState = CombatState.CheckArmor;
			}
		}

		public override void CheckMonsterStatus()
		{
			Debug.Assert(DobjMonster != null);

			Debug.Assert(_d2 >= 0);

			DobjMonster.DmgTaken += _d2;

			if (!OmitMonsterStatus || ActorMonster == DobjMonster)
			{
				PrintHealthStatus(ActorRoom, ActorMonster, DobjMonster, BlastSpell);
			}

			if (DobjMonster.IsDead())
			{
				var room = DobjMonster.GetInRoom();

				Debug.Assert(room != null);

				var ringArtifact = gADB[22];

				Debug.Assert(ringArtifact != null);

				if (DobjMonster.IsCharacterMonster())
				{
					// Resurrect

					if (ringArtifact.IsCarriedByCharacter() || ringArtifact.IsWornByCharacter())
					{
						gOut.Print("{0}", gEngine.LineSep);

						gOut.Write("{0}Press any key to continue: ", Environment.NewLine);

						gEngine.Buf.Clear();

						var rc = gEngine.In.ReadField(gEngine.Buf, gEngine.BufSize02, null, ' ', '\0', true, null, gEngine.ModifyCharToNull, null, gEngine.IsCharAny);

						Debug.Assert(gEngine.IsSuccess(rc));

						gEngine.Thread.Sleep(150);

						gOut.Print("{0}", gEngine.LineSep);

						gEngine.ClearActionLists();

						// gSentenceParser.PrintDiscardingCommands() not called for this abrupt reality shift

						gSentenceParser.Clear();

						gEngine.PrintEffectDesc(3);

						DobjMonster.DmgTaken = 0;

						gEngine.ResetMonsterStats(DobjMonster);

						gEngine.MagicRingLowersMonsterStats(DobjMonster);

						DobjWeapon = DobjMonster.Weapon > 0 ? gADB[DobjMonster.Weapon] : null;

						if (DobjWeapon != null)
						{
							gOut.EnableOutput = false;

							var dropCommand = gEngine.CreateInstance<IDropCommand>(x =>
							{
								x.ActorMonster = DobjMonster;

								x.ActorRoom = room;

								x.Dobj = DobjWeapon;
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
							SetNextStateFunc(gEngine.CreateInstance<IAfterPlayerMoveState>(x =>
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
							SetNextStateFunc(gEngine.CreateInstance<IPlayerDeadState>(x =>
							{
								x.PrintLineSep = true;
							}));
						}
					}
				}
				else
				{
					// Resurrect

					if (ringArtifact.IsCarriedByMonster(DobjMonster) || ringArtifact.IsWornByMonster(DobjMonster))
					{
						gEngine.ResurrectMonsterUid = DobjMonster.Uid;

						DobjMonster.SetInLimbo();

						ringArtifact.SetInLimbo();
					}
					else
					{
						gEngine.MonsterDies(ActorMonster, DobjMonster);
					}
				}
			}

			CombatState = CombatState.EndAttack;
		}
	}
}
