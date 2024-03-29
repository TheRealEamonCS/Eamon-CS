﻿
// CombatComponent.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework;
using Eamon.Game.Attributes;
using EamonRT.Framework.Components;
using static ARuncibleCargo.Game.Plugin.Globals;

namespace ARuncibleCargo.Game.Components
{
	[ClassMappings]
	public class CombatComponent : EamonRT.Game.Components.CombatComponent, ICombatComponent
	{
		public override void PrintHealthStatus(IRoom room, IMonster actorMonster, IMonster dobjMonster, bool blastSpell, bool nonCombat)
		{
			Debug.Assert(room != null && dobjMonster != null);

			// Alt "death" for Hokas, Larkspur, and Cargo-Stealing Lil

			var monsterDies = dobjMonster.IsDead();

			if (dobjMonster.Uid == 4 && monsterDies)
			{
				if (!blastSpell)
				{
					gOut.WriteLine();
				}

				gEngine.PrintEffectDesc(149, false);
			}
			else if (dobjMonster.Uid == 36 && monsterDies)
			{
				if (!blastSpell)
				{
					gOut.WriteLine();
				}

				gEngine.PrintEffectDesc(151, false);
			}
			else if (dobjMonster.Uid == 37 && monsterDies)
			{
				if (!blastSpell)
				{
					gOut.WriteLine();
				}

				gEngine.PrintEffectDesc(150, false);
			}
			else
			{
				base.PrintHealthStatus(room, actorMonster, dobjMonster, blastSpell, nonCombat);
			}
		}
	}
}
