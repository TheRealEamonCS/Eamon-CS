
// CombatComponent.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using Eamon.Framework;
using Eamon.Game.Attributes;
using EamonRT.Framework.Components;
using static TheSubAquanLaboratory.Game.Plugin.Globals;

namespace TheSubAquanLaboratory.Game.Components
{
	[ClassMappings]
	public class CombatComponent : EamonRT.Game.Components.CombatComponent, ICombatComponent
	{
		public override void PrintBlowTurned(IMonster monster, bool omitBboaPadding)
		{
			Debug.Assert(monster != null);

			if (monster.Uid != gGameState.Cm || monster.Armor > 0)
			{
				var armorDesc = monster.GetArmorDescString();

				gOut.Write("{0}{1}Blow bounces off {2}!", Environment.NewLine, omitBboaPadding ? "" : "  ", armorDesc);
			}
			else
			{
				gOut.Write("{0}{1}Blow turned!", Environment.NewLine, omitBboaPadding ? "" : "  ");
			}
		}
	}
}
