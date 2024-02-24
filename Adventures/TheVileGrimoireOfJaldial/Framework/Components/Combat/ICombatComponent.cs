
// ICombatComponent.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using static TheVileGrimoireOfJaldial.Game.Plugin.Globals;

namespace TheVileGrimoireOfJaldial.Framework.Components
{
	/// <inheritdoc />
	public interface ICombatComponent : EamonRT.Framework.Components.ICombatComponent
	{
		bool CrossbowTrap { get; set; }
	}
}
