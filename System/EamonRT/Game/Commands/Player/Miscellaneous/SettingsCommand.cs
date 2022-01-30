﻿
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

			PrintSettingsChanged();

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IStartState>();
			}
		}

		public override bool ShouldPreTurnProcess()
		{
			return false;
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
