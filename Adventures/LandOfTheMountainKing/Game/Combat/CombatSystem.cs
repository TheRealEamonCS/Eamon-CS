
// CombatSystem.cs

// Copyright (c) 2014+ by Kenneth Pedersen.  All rights reserved.

using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Combat;
using System;
using EamonRT.Framework.Primitive.Enums;
using static LandOfTheMountainKing.Game.Plugin.PluginContext;

namespace LandOfTheMountainKing.Game.Combat
{
	[ClassMappings]
	public class CombatSystem : EamonRT.Game.Combat.CombatSystem, ICombatSystem
	{
		public override void PrintHealthStatus()
		{
			// Alt "deaths":

			var monsterDies = DfMonster.IsDead();

			if (DfMonster.Uid != gGameState.Cm && monsterDies)
			{
				gOut.WriteLine();
			}

			if (DfMonster.Uid == 1 && monsterDies)
			{
				gEngine.PrintEffectDesc(44);
				Globals.In.KeyPress(Globals.Buf);
				gOut.Print("{0}", Globals.LineSep);
				gEngine.PrintEffectDesc(45);
				gEngine.PrintEffectDesc(46);
				gEngine.PrintEffectDesc(47);
				gEngine.PrintEffectDesc(48);
				Globals.In.KeyPress(Globals.Buf);
				gOut.Print("{0}", Globals.LineSep);
				gEngine.PrintEffectDesc(49);
				gEngine.PrintEffectDesc(50);
				gEngine.PrintEffectDesc(51);
				gEngine.PrintEffectDesc(58);
				Globals.In.KeyPress(Globals.Buf);
				gADB[30].SetCarriedByCharacter();
				Globals.ExitType = ExitType.FinishAdventure;
				Globals.MainLoop.ShouldShutdown = true;
				OmitFinalNewLine = true;
			}
			else if (DfMonster.Uid == 2 && monsterDies)
			{
				gEngine.PrintEffectDesc(43);
				gEngine.PrintEffectDesc(42);
				gCharMonster.DmgTaken = 0;
			}
			else if (DfMonster.Uid == 3 && monsterDies)
			{
				gEngine.PrintEffectDesc(52);
			}
			else if (DfMonster.Uid == 4 && monsterDies)
			{
				gEngine.PrintEffectDesc(53);
				gEngine.PrintEffectDesc(42);
				gADB[22].SetInLimbo();
				gCharMonster.DmgTaken = 0;
			}
			else if (DfMonster.Uid == 5 && monsterDies)
			{
				gEngine.PrintEffectDesc(54);
				gEngine.PrintEffectDesc(42);
				gCharMonster.DmgTaken = 0;
				gADB[33].SetInRoomUid(3);
			}
			else if (DfMonster.Uid == 6 && monsterDies)
			{
				gEngine.PrintEffectDesc(55);
				gEngine.PrintEffectDesc(42);
				gCharMonster.DmgTaken = 0;
				gLMKKP1.SwampMonsterKilled = 1;
			}
			else if (DfMonster.Uid == 7 && monsterDies)
			{
				gEngine.PrintEffectDesc(56);
				gCharMonster.DmgTaken = 0;
			}
			else if (DfMonster.Uid == 8 && monsterDies)
			{
				gEngine.PrintEffectDesc(57);
				gEngine.PrintEffectDesc(42);
				gCharMonster.DmgTaken = 0;
			}
			else
			{
				base.PrintHealthStatus();
			}
		}

		public override void AttackHit()
		{
			// Werewolf can only be hit by silver sword:

			if (DfMonster.Uid == 7 && OfWeaponUid != 25)
			{
				gEngine.PrintEffectDesc(26);
				gEngine.PrintEffectDesc(27);
				gEngine.PrintEffectDesc(28);
				CombatState = CombatState.EndAttack;
			}
			else
			{
				base.AttackHit(); //This command calls the system version of AttackHit();

				if (DfMonster.Uid == 7 && OfWeaponUid == 25)
				{
					gOut.WriteLine();
					gEngine.PrintEffectDesc(29);
				}
			}
		}
	}
}
