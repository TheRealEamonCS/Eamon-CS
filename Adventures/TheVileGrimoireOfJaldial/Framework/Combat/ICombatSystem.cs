
// ICombatSystem.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using static TheVileGrimoireOfJaldial.Game.Plugin.PluginContext;

namespace TheVileGrimoireOfJaldial.Framework.Combat
{
	public interface ICombatSystem : EamonRT.Framework.Combat.ICombatSystem
	{
		bool CrossbowTrap { get; set; }
	}
}
