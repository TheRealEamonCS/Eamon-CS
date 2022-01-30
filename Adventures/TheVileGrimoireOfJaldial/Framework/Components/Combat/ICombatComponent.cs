
// ICombatComponent.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using static TheVileGrimoireOfJaldial.Game.Plugin.PluginContext;

namespace TheVileGrimoireOfJaldial.Framework.Components
{
	public interface ICombatComponent : EamonRT.Framework.Components.ICombatComponent
	{
		bool CrossbowTrap { get; set; }
	}
}
