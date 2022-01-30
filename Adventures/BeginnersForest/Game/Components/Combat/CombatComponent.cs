
// CombatComponent.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework;
using Eamon.Game.Attributes;
using EamonRT.Framework.Components;
using static BeginnersForest.Game.Plugin.PluginContext;

namespace BeginnersForest.Game.Components
{
	[ClassMappings]
	public class CombatComponent : EamonRT.Game.Components.CombatComponent, ICombatComponent
	{
		public override void PrintHealthStatus(IRoom room, IMonster actorMonster, IMonster dobjMonster, bool blastSpell)
		{
			Debug.Assert(room != null && dobjMonster != null);

			// Repetitive Spooks' repetitive death description

			var monsterDies = dobjMonster.IsDead();

			if (dobjMonster.Uid == 9 && monsterDies)
			{
				if (!blastSpell)
				{
					gOut.WriteLine();
				}

				gEngine.PrintEffectDesc(8, false);
			}
			else
			{
				base.PrintHealthStatus(room, actorMonster, dobjMonster, blastSpell);
			}
		}
	}
}
