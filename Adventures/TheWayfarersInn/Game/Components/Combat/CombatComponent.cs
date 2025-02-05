
// CombatComponent.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Components;
using EamonRT.Framework.Primitive.Enums;
using static TheWayfarersInn.Game.Plugin.Globals;

namespace TheWayfarersInn.Game.Components
{
	[ClassMappings]
	public class CombatComponent : EamonRT.Game.Components.CombatComponent, ICombatComponent
	{
		public override void ExecuteAttack()
		{
			// Witherbloom always hits, ignoring armor

			if (ActorMonster?.Uid == 18 && !gGameState.InteractiveFiction)
			{
				FixedResult = AttackResult.Hit;

				OmitArmor = true;

				OmitBboaPadding = true;
			}

			base.ExecuteAttack();
		}

		public override void AttackHit()
		{
			// Black pudding cleaved in two with Sword weapons

			if (DobjMonster.Uid == 1 && ActorWeaponType == Weapon.Sword && !gGameState.InteractiveFiction)
			{
				PrintStarPlus(DobjMonster);

				if (_rl >= 5)
				{
					PrintHit();
				}
				else
				{
					PrintCriticalHit();
				}

				gOut.WriteLine();

				gOut.Print("{0} takes no damage and is cleaved in two!", ActorRoom.EvalViewability("The defender", DobjMonster.GetTheName(true, true, false, false, true)));

				DobjMonster.Hardiness = Math.Max(1, (long)Math.Round(((double)DobjMonster.Hardiness * (double)DobjMonster.CurrGroupCount) / (double)(DobjMonster.CurrGroupCount + 1)));

				DobjMonster.GroupCount++;

				DobjMonster.InitGroupCount++;

				DobjMonster.CurrGroupCount++;

				// Nolan figures it out

				if (ActorMonster.Uid == 24)
				{
					if (ActorRoom.IsViewable())
					{
						gOut.Print("{0}{1} reconsiders his strategy!", Environment.NewLine, ActorMonster.GetTheName(true));
					}

					gGameState.NolanPuddingAttackOdds /= 5;
				}

				CombatState = CombatState.EndAttack;
			}
			else
			{
				base.AttackHit();
			}
		}

		public override void CheckArmorBlowTurned()
		{
			base.CheckArmorBlowTurned();

			CheckRustMonsterDamagesWeaponsAndArmor();
		}

		public override void CheckMonsterStatus()
		{
			base.CheckMonsterStatus();

			CheckRustMonsterDamagesWeaponsAndArmor();
		}

		public override void CheckArtifactStatus()
		{
			// Armoire

			if (DobjArtifact != null && DobjArtAc != null && _d2 >= 0 && DobjArtifact.Uid == 184 && BreakageStrength - _d2 <= 1000)
			{
				gOut.Print("{0} lock smashes to pieces!", DobjArtifact.GetTheName(true));

				DobjArtAc.SetOpen(false);

				CombatState = CombatState.EndAttack;
			}
			else
			{
				base.CheckArtifactStatus();
			}
		}

		public virtual void CheckRustMonsterDamagesWeaponsAndArmor()
		{
			// Rust monster damages armor

			if (ActorMonster != null && ActorMonster.Uid == 20 && DobjMonster != null && DobjMonster.IsCharacterMonster() && gGameState.Ar > 0 && gGameState.Ar == gGameState.MetalArmorArtifactUid)
			{
				gEngine.MiscEventFuncList02.Add(() =>
				{
					var armorArtifact = gADB[gGameState.Ar];

					Debug.Assert(armorArtifact != null);

					gOut.EnableOutput = false;

					var removeCommand = gEngine.CreateInstance<IRemoveCommand>(x =>
					{
						x.ActorMonster = DobjMonster;

						x.ActorRoom = ActorRoom;

						x.Dobj = armorArtifact;
					});

					removeCommand.Execute();

					gOut.EnableOutput = true;

					armorArtifact.Wearable.Field1 -= 2;

					if (armorArtifact.Wearable.Field1 <= 0)
					{
						gEngine.PrintArtifactBreaks(ActorRoom, DobjMonster, armorArtifact, true);

						armorArtifact.SetInLimbo();

						armorArtifact.Wearable.Field1 = 0;
					}
					else
					{
						if (ActorRoom.IsViewable())
						{
							gOut.Print("{0}{1} grow{2} rusty!", Environment.NewLine, armorArtifact.GetTheName(true), armorArtifact.EvalPlural("s", ""));
						}

						gOut.EnableOutput = false;

						var wearCommand = gEngine.CreateInstance<IWearCommand>(x =>
						{
							x.ActorMonster = DobjMonster;

							x.ActorRoom = ActorRoom;

							x.Dobj = armorArtifact;
						});

						wearCommand.Execute();

						gOut.EnableOutput = true;
					}
				});
			}

			// Rust monster damages Axe / Spear / Sword weapons

			if (ActorMonster != null && DobjMonster != null && DobjMonster.Uid == 20 && (ActorWeaponType == Weapon.Axe || ActorWeaponType == Weapon.Spear || ActorWeaponType == Weapon.Sword))
			{
				gEngine.MiscEventFuncList02.Add(() =>
				{
					Debug.Assert(ActorWeapon != null);

					if (--ActorWeapon.GeneralWeapon.Field4 <= 0)
					{
						gEngine.PrintArtifactBreaks(ActorRoom, ActorMonster, ActorWeapon, true);

						ActorWeapon.RemoveStateDesc(ActorWeapon.GetReadyWeaponDesc());

						ActorMonster.Weapon = -1;

						ActorWeapon.SetInLimbo();

						ActorWeapon.GeneralWeapon.Field4 = 1;
					}
					else if (ActorRoom.IsViewable())
					{
						gOut.Print("{0}{1} grow{2} rusty!", Environment.NewLine, ActorWeapon.GetTheName(true), ActorWeapon.EvalPlural("s", ""));
					}

					// Nolan figures it out

					if (ActorMonster.Uid == 24)
					{
						if (ActorRoom.IsViewable())
						{
							gOut.Print("{0}{1} reconsiders his strategy!", Environment.NewLine, ActorMonster.GetTheName(true));
						}

						gGameState.NolanRustMonsterAttackOdds /= 10;
					}
				});
			}
		}

		public CombatComponent()
		{
			WeaponRevealType = WeaponRevealType.Always;
		}
	}
}
