
// SettingsCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class SettingsCommand : Command, ISettingsCommand
	{
		public virtual bool? VerboseRooms { get; set; } = null;

		public virtual bool? VerboseMonsters { get; set; } = null;

		public virtual bool? VerboseArtifacts { get; set; } = null;

		public virtual bool? MatureContent { get; set; } = null;

		public virtual bool? EnhancedParser { get; set; } = null;

		public virtual bool? ShowPronounChanges { get; set; } = null;

		public virtual bool? ShowFulfillMessages { get; set; } = null;

		public virtual long? PauseCombatMs { get; set; } = null;

		public override void Execute()
		{
			Debug.Assert(VerboseRooms != null || VerboseMonsters != null || VerboseArtifacts != null || MatureContent != null || EnhancedParser != null || ShowPronounChanges != null || ShowFulfillMessages != null || PauseCombatMs != null);

			if (VerboseRooms != null)
			{
				gGameState.Vr = (bool)VerboseRooms;
			}

			if (VerboseMonsters != null)
			{
				gGameState.Vm = (bool)VerboseMonsters;
			}

			if (VerboseArtifacts != null)
			{
				gGameState.Va = (bool)VerboseArtifacts;
			}

			if (MatureContent != null)
			{
				gGameState.MatureContent = (bool)MatureContent;
			}

			if (EnhancedParser != null)
			{
				gGameState.EnhancedParser = (bool)EnhancedParser;

				if (!gGameState.EnhancedParser)
				{
					gSentenceParser.LastInputStr = "";

					gSentenceParser.Clear();

					gCommandParser.LastHimNameStr = "";

					gCommandParser.LastHerNameStr = "";

					gCommandParser.LastItNameStr = "";

					gCommandParser.LastThemNameStr = "";

					gGameState.ShowPronounChanges = false;

					gGameState.ShowFulfillMessages = false;
				}
			}

			if (ShowPronounChanges != null)
			{
				gGameState.ShowPronounChanges = (bool)ShowPronounChanges;
			}

			if (ShowFulfillMessages != null)
			{
				gGameState.ShowFulfillMessages = (bool)ShowFulfillMessages;
			}

			if (PauseCombatMs != null)
			{
				Debug.Assert(PauseCombatMs >= 0 && PauseCombatMs <= 10000);

				gGameState.PauseCombatMs = (long)PauseCombatMs;
			}

			gOut.Print("Settings changed.");

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IStartState>();
			}
		}

		public override bool ShouldPreTurnProcess()
		{
			return false;
		}

		public virtual void PrintUsage()
		{
			gOut.Print("Usage:  SETTINGS [Option] [Value]{0}", Environment.NewLine);

			gOut.WriteLine("  {0,-22}{1,-22}{2,-22}", "Option", "Value", "Setting");
			gOut.WriteLine("  {0,-22}{1,-22}{2,-22}", "------", "-----", "-------");
			gOut.WriteLine("  {0,-22}{1,-22}{2,-22}", "VerboseRooms", "True, False", gGameState.Vr);
			gOut.WriteLine("  {0,-22}{1,-22}{2,-22}", "VerboseMonsters", "True, False", gGameState.Vm);
			gOut.WriteLine("  {0,-22}{1,-22}{2,-22}", "VerboseArtifacts", "True, False", gGameState.Va);
			gOut.WriteLine("  {0,-22}{1,-22}{2,-22}", "MatureContent", "True, False", gGameState.MatureContent);
			gOut.WriteLine("  {0,-22}{1,-22}{2,-22}", "EnhancedParser", "True, False", gGameState.EnhancedParser);

			if (gGameState.EnhancedParser)
			{
				gOut.WriteLine("  {0,-22}{1,-22}{2,-22}", "ShowPronounChanges", "True, False", gGameState.ShowPronounChanges);
				gOut.WriteLine("  {0,-22}{1,-22}{2,-22}", "ShowFulfillMessages", "True, False", gGameState.ShowFulfillMessages);
			}

			gOut.WriteLine("  {0,-22}{1,-22}{2,-22}", "PauseCombatMs", "0 .. 10000", gGameState.PauseCombatMs);
		}

		public SettingsCommand()
		{
			SortOrder = 400;

			IsSentenceParserEnabled = false;

			IsDarkEnabled = true;

			Uid = 65;

			Name = "SettingsCommand";

			Verb = "settings";

			Type = CommandType.Miscellaneous;
		}
	}
}
