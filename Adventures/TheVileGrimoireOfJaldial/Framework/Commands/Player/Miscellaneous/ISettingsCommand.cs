
// ISettingsCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using static TheVileGrimoireOfJaldial.Game.Plugin.PluginContext;

namespace TheVileGrimoireOfJaldial.Framework.Commands
{
	public interface ISettingsCommand : EamonRT.Framework.Commands.ISettingsCommand
	{
		bool? ShowCombatDamage { get; set; }

		bool? ExitDirNames { get; set; }

		long? WeatherFreqPct { get; set; }

		long? EncounterFreqPct { get; set; }

		long? FlavorFreqPct { get; set; }
	}
}
