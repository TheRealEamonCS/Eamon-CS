
// ISettingsCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using static TheVileGrimoireOfJaldial.Game.Plugin.Globals;

namespace TheVileGrimoireOfJaldial.Framework.Commands
{
	/// <inheritdoc />
	public interface ISettingsCommand : EamonRT.Framework.Commands.ISettingsCommand
	{
		bool? ShowCombatDamage { get; set; }

		bool? ExitDirNames { get; set; }

		long? WeatherFreqPct { get; set; }

		long? EncounterFreqPct { get; set; }

		long? FlavorFreqPct { get; set; }
	}
}
