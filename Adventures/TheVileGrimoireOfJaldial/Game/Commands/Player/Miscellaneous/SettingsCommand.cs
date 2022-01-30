
// SettingsCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static TheVileGrimoireOfJaldial.Game.Plugin.PluginContext;

namespace TheVileGrimoireOfJaldial.Game.Commands
{
	[ClassMappings(typeof(ISettingsCommand))]
	public class SettingsCommand : EamonRT.Game.Commands.SettingsCommand, Framework.Commands.ISettingsCommand
	{
		public virtual bool? ShowCombatDamage { get; set; }

		public virtual bool? ExitDirNames { get; set; }

		public virtual long? WeatherFreqPct { get; set; }

		public virtual long? EncounterFreqPct { get; set; }

		public virtual long? FlavorFreqPct { get; set; }

		public override void PrintSettingsUsage()
		{
			base.PrintSettingsUsage();

			gOut.WriteLine("  {0,-22}{1,-22}{2,-22}", "ShowCombatDamage", "True, False", gGameState.ShowCombatDamage);

			gOut.WriteLine("  {0,-22}{1,-22}{2,-22}", "ExitDirNames", "True, False", gGameState.ExitDirNames);

			gOut.WriteLine("  {0,-22}{1,-22}{2,-22}", "WeatherFreqPct", "0 .. 100", gGameState.WeatherFreqPct);

			gOut.WriteLine("  {0,-22}{1,-22}{2,-22}", "EncounterFreqPct", "0 .. 100", gGameState.EncounterFreqPct);

			gOut.WriteLine("  {0,-22}{1,-22}{2,-22}", "FlavorFreqPct", "0 .. 100", gGameState.FlavorFreqPct);
		}

		public override void Execute()
		{
			if (ShowCombatDamage == null && ExitDirNames == null && WeatherFreqPct == null && EncounterFreqPct == null && FlavorFreqPct == null)
			{
				base.Execute();

				goto Cleanup;
			}

			if (ShowCombatDamage != null)
			{
				gGameState.ShowCombatDamage = (bool)ShowCombatDamage;
			}

			if (ExitDirNames != null)
			{
				gGameState.ExitDirNames = (bool)ExitDirNames;
			}

			if (WeatherFreqPct != null)
			{
				Debug.Assert(WeatherFreqPct >= 0 && WeatherFreqPct <= 100);

				gGameState.WeatherFreqPct = (long)WeatherFreqPct;
			}

			if (EncounterFreqPct != null)
			{
				Debug.Assert(EncounterFreqPct >= 0 && EncounterFreqPct <= 100);

				gGameState.EncounterFreqPct = (long)EncounterFreqPct;
			}

			if (FlavorFreqPct != null)
			{
				Debug.Assert(FlavorFreqPct >= 0 && FlavorFreqPct <= 100);

				gGameState.FlavorFreqPct = (long)FlavorFreqPct;
			}

			gOut.Print("Settings changed.");

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IStartState>();
			}

		Cleanup:

			;
		}
	}
}
