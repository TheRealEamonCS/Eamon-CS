
// CombatSystem.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Game.Attributes;
using EamonRT.Framework.Combat;
using static BeginnersForest.Game.Plugin.PluginContext;

namespace BeginnersForest.Game.Combat
{
	[ClassMappings]
	public class CombatSystem : EamonRT.Game.Combat.CombatSystem, ICombatSystem
	{
		public override void PrintHealthStatus()
		{
			// Repetitive Spooks' repetitive death description

			var monsterDies = DfMonster.IsDead();

			if (DfMonster.Uid == 9 && monsterDies)
			{
				if (!BlastSpell)
				{
					gOut.WriteLine();
				}

				gEngine.PrintEffectDesc(8, false);
			}
			else
			{
				base.PrintHealthStatus();
			}
		}
	}
}
