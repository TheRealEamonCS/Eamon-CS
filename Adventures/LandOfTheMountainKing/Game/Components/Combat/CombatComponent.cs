
// CombatComponent.cs

// Copyright (c) 2014+ by Kenneth Pedersen.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Components;
using EamonRT.Framework.Primitive.Enums;
using static LandOfTheMountainKing.Game.Plugin.Globals;

namespace LandOfTheMountainKing.Game.Components
{
	[ClassMappings]
	public class CombatComponent : EamonRT.Game.Components.CombatComponent, ICombatComponent
	{
		public override void PrintHealthStatus(IRoom room, IMonster actorMonster, IMonster dobjMonster, bool blastSpell)
		{
			Debug.Assert(room != null && dobjMonster != null);

			// Alt "deaths":

			var monsterDies = dobjMonster.IsDead();

			if (dobjMonster.Uid != gGameState.Cm && monsterDies)
			{
				gOut.WriteLine();
			}

			if (dobjMonster.Uid == 1 && monsterDies)
			{
				gEngine.PrintEffectDesc(44);
				gEngine.CheckPlayerSkillGains();
				gEngine.In.KeyPress(gEngine.Buf);
				gOut.Print("{0}", gEngine.LineSep);
				gEngine.PrintEffectDesc(45);
				gEngine.PrintEffectDesc(46);
				gEngine.PrintEffectDesc(47);
				gEngine.PrintEffectDesc(48);
				gEngine.In.KeyPress(gEngine.Buf);
				gOut.Print("{0}", gEngine.LineSep);
				gEngine.PrintEffectDesc(49);
				gEngine.PrintEffectDesc(50);
				gEngine.PrintEffectDesc(51);
				gEngine.PrintEffectDesc(58);
				gEngine.In.KeyPress(gEngine.Buf);
				gADB[30].SetCarriedByCharacter();
				gEngine.ExitType = ExitType.FinishAdventure;
				gEngine.MainLoop.ShouldShutdown = true;
				OmitFinalNewLine = true;
			}
			else if (dobjMonster.Uid == 2 && monsterDies)
			{
				gEngine.PrintEffectDesc(43);
				gEngine.CheckPlayerSkillGains();
				gEngine.PrintEffectDesc(42);
				gCharMonster.DmgTaken = 0;
			}
			else if (dobjMonster.Uid == 3 && monsterDies)
			{
				gEngine.PrintEffectDesc(52);
			}
			else if (dobjMonster.Uid == 4 && monsterDies)
			{
				gEngine.PrintEffectDesc(53);
				gEngine.CheckPlayerSkillGains();
				gEngine.PrintEffectDesc(42);
				gADB[22].SetInLimbo();
				gCharMonster.DmgTaken = 0;
			}
			else if (dobjMonster.Uid == 5 && monsterDies)
			{
				gEngine.PrintEffectDesc(54);
				gEngine.CheckPlayerSkillGains();
				gEngine.PrintEffectDesc(42);
				gCharMonster.DmgTaken = 0;
				gADB[33].SetInRoomUid(3);
			}
			else if (dobjMonster.Uid == 6 && monsterDies)
			{
				gEngine.PrintEffectDesc(55);
				gEngine.CheckPlayerSkillGains();
				gEngine.PrintEffectDesc(42);
				gCharMonster.DmgTaken = 0;
				gLMKKP1.SwampMonsterKilled = 1;
			}
			else if (dobjMonster.Uid == 7 && monsterDies)
			{
				gEngine.PrintEffectDesc(56);
				gEngine.CheckPlayerSkillGains();
				gEngine.PrintEffectDesc(86);
				gCharMonster.DmgTaken = 0;
			}
			else if (dobjMonster.Uid == 8 && monsterDies)
			{
				gEngine.PrintEffectDesc(57);
				gEngine.CheckPlayerSkillGains();
				gEngine.PrintEffectDesc(87);
				gEngine.PrintEffectDesc(42);
				gCharMonster.DmgTaken = 0;
			}
			else
			{
				base.PrintHealthStatus(room, actorMonster, dobjMonster, blastSpell);
			}
		}

		public override void AttackHit()
		{
			// Werewolf can only be hit by silver sword:

			if (DobjMonster.Uid == 7 && ActorWeaponUid != 25)
			{
				gEngine.PrintEffectDesc(26);
				gEngine.PrintEffectDesc(27);
				gEngine.PrintEffectDesc(28);
				CombatState = CombatState.EndAttack;
			}
			else
			{
				base.AttackHit(); //This command calls the system version of AttackHit();

				if (DobjMonster.Uid == 7 && ActorWeaponUid == 25)
				{
					gOut.WriteLine();
					gEngine.PrintEffectDesc(29);
				}
			}
		}
	}
}
