
// CombatSystem.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Game.Attributes;
using EamonRT.Framework.Combat;
using static ARuncibleCargo.Game.Plugin.PluginContext;

namespace ARuncibleCargo.Game.Combat
{
	[ClassMappings]
	public class CombatSystem : EamonRT.Game.Combat.CombatSystem, ICombatSystem
	{
		public override void PrintHealthStatus()
		{
			// Alt "death" for Hokas, Larkspur, and Cargo-Stealing Lil

			var monsterDies = DfMonster.IsDead();

			if (DfMonster.Uid == 4 && monsterDies)
			{
				if (!BlastSpell)
				{
					gOut.WriteLine();
				}

				gEngine.PrintEffectDesc(149, false);
			}
			else if (DfMonster.Uid == 36 && monsterDies)
			{
				if (!BlastSpell)
				{
					gOut.WriteLine();
				}

				gEngine.PrintEffectDesc(151, false);
			}
			else if (DfMonster.Uid == 37 && monsterDies)
			{
				if (!BlastSpell)
				{
					gOut.WriteLine();
				}

				gEngine.PrintEffectDesc(150, false);
			}
			else
			{
				base.PrintHealthStatus();
			}
		}
	}
}
