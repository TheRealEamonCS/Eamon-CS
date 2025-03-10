﻿
// SettingsCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.Globals;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class SettingsCommand : Command, ISettingsCommand
	{
		public virtual bool? VerboseRooms { get; set; } = null;

		public virtual bool? VerboseMonsters { get; set; } = null;

		public virtual bool? VerboseArtifacts { get; set; } = null;

		public virtual bool? VerboseNames { get; set; } = null;

		public virtual bool? MatureContent { get; set; } = null;

		public virtual bool? InteractiveFiction { get; set; } = null;

		public virtual bool? EnhancedCombat { get; set; } = null;

		public virtual bool? EnhancedParser { get; set; } = null;

		public virtual bool? IobjPronounAffinity { get; set; } = null;

		public virtual bool? ShowPronounChanges { get; set; } = null;

		public virtual bool? ShowFulfillMessages { get; set; } = null;

		public virtual long? PauseCombatMs { get; set; } = null;

		public virtual long? PauseCombatActions { get; set; } = null;

		/// <summary></summary>
		public virtual IList<ICommand> EnhancedCombatCommandList { get; set; }

		/// <summary></summary>
		public virtual IList<IMonster> ResetMonsterList { get; set; }

		public override void ExecuteForPlayer()
		{
			Debug.Assert(VerboseRooms != null || VerboseMonsters != null || VerboseArtifacts != null || VerboseNames != null || MatureContent != null || InteractiveFiction != null || EnhancedCombat != null || EnhancedParser != null || IobjPronounAffinity != null || ShowPronounChanges != null || ShowFulfillMessages != null || PauseCombatMs != null || PauseCombatActions != null);

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

			if (VerboseNames != null)
			{
				gGameState.Vn = (bool)VerboseNames;
			}

			if (MatureContent != null)
			{
				gGameState.MatureContent = (bool)MatureContent;
			}

			if (InteractiveFiction != null)
			{
				gGameState.InteractiveFiction = (bool)InteractiveFiction;
			}

			if (gEngine.EnableEnhancedCombat && EnhancedCombat != null)
			{
				gGameState.EnhancedCombat = (bool)EnhancedCombat;

				EnhancedCombatCommandList = gEngine.CommandList.Where(c => c is IParryCommand).ToList();

				foreach (var command in EnhancedCombatCommandList)
				{
					command.IsPlayerEnabled = gGameState.EnhancedCombat;

					command.IsMonsterEnabled = gGameState.EnhancedCombat;
				}

				ResetMonsterList = gDatabase.MonsterTable.Records.ToList();

				foreach (var monster in ResetMonsterList)
				{
					monster.Parry = monster.InitParry;
				}
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

					gGameState.IobjPronounAffinity = false;

					gGameState.ShowPronounChanges = false;

					gGameState.ShowFulfillMessages = false;
				}
			}

			if (IobjPronounAffinity != null)
			{
				gGameState.IobjPronounAffinity = (bool)IobjPronounAffinity;
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

			if (PauseCombatActions != null)
			{
				Debug.Assert(PauseCombatActions >= 0 && PauseCombatActions <= 25);

				gGameState.PauseCombatActions = (long)PauseCombatActions;
			}

			PrintSettingsChanged();

			if (NextState == null)
			{
				NextState = gEngine.CreateInstance<IStartState>();
			}
		}

		public SettingsCommand()
		{
			SortOrder = 400;

			IsSentenceParserEnabled = false;

			IsDarkEnabled = true;

			Name = "SettingsCommand";

			Verb = "settings";

			Type = CommandType.Miscellaneous;
		}
	}
}
