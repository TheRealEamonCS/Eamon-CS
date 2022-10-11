
// RestoreCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Eamon;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class RestoreCommand : Command, IRestoreCommand
	{
		public virtual long SaveSlot { get; set; }

		/// <summary></summary>
		public virtual IList<IArtifact> FullArtifactList { get; set; }

		/// <summary></summary>
		public virtual IList<IMonster> FullMonsterList { get; set; }

		/// <summary></summary>
		public virtual IList<IFileset> SaveFilesetList { get; set; }

		/// <summary></summary>
		public virtual IFileset SaveFileset { get; set; }

		/// <summary></summary>
		public virtual IConfig SaveConfig { get; set; }

		/// <summary></summary>
		public virtual IRoom CharacterRoom { get; set; }

		/// <summary></summary>
		public virtual IState OrigCurrState { get; set; }

		/// <summary></summary>
		public virtual long SaveFilesetsCount { get; set; }

		public override void Execute()
		{
			RetCode rc;
			
			Globals.ShouldPreTurnProcess = false;

			try
			{
				Globals.MutatePropertyCounter--;

				Globals.RevealContentCounter--;

				OrigCurrState = Globals.CurrState;

				SaveFilesetsCount = Globals.Database.GetFilesetsCount();

				Debug.Assert(SaveFilesetsCount <= gEngine.NumSaveSlots);

				Debug.Assert(SaveSlot >= 1 && SaveSlot <= SaveFilesetsCount);

				SaveFilesetList = Globals.Database.FilesetTable.Records.OrderBy(f => f.Uid).ToList();

				SaveFileset = SaveFilesetList[(int)SaveSlot - 1];

				SaveConfig = Globals.CreateInstance<IConfig>();

				rc = Globals.Database.LoadConfigs(SaveFileset.ConfigFileName, printOutput: false);

				if (gEngine.IsFailure(rc))
				{
					Globals.Error.Write("Error: LoadConfigs function call failed.");

					Globals.ExitType = ExitType.Error;

					Globals.MainLoop.ShouldShutdown = false;

					goto Cleanup;
				}

				Globals.Config = gEngine.GetConfig();

				if (Globals.Config == null || Globals.Config.Uid <= 0)
				{
					Globals.Error.Write("{0}Error: Assertion failed [{1}].", Environment.NewLine, Globals.Config == null ? "Globals.Config != null" : "Globals.Config.Uid > 0");

					Globals.ExitType = ExitType.Error;

					Globals.MainLoop.ShouldShutdown = false;

					goto Cleanup;
				}

				SaveConfig.RtFilesetFileName = Globals.CloneInstance(Globals.Config.RtFilesetFileName);

				SaveConfig.RtCharacterFileName = Globals.CloneInstance(SaveFileset.CharacterFileName);

				SaveConfig.RtModuleFileName = Globals.CloneInstance(SaveFileset.ModuleFileName);

				SaveConfig.RtRoomFileName = Globals.CloneInstance(SaveFileset.RoomFileName);

				SaveConfig.RtArtifactFileName = Globals.CloneInstance(SaveFileset.ArtifactFileName);

				SaveConfig.RtEffectFileName = Globals.CloneInstance(SaveFileset.EffectFileName);

				SaveConfig.RtMonsterFileName = Globals.CloneInstance(SaveFileset.MonsterFileName);

				SaveConfig.RtHintFileName = Globals.CloneInstance(SaveFileset.HintFileName);

				SaveConfig.RtGameStateFileName = Globals.CloneInstance(SaveFileset.GameStateFileName);

				rc = SaveConfig.LoadGameDatabase(printOutput: false);

				if (gEngine.IsFailure(rc))
				{
					Globals.Error.Write("Error: LoadGameDatabase function call failed.");

					Globals.ExitType = ExitType.Error;

					Globals.MainLoop.ShouldShutdown = false;

					goto Cleanup;
				}

				// fileset is now invalid

				Globals.Character = Globals.Database.CharacterTable.Records.FirstOrDefault();

				if (gCharacter == null || gCharacter.Uid <= 0 || gCharacter.Status != Status.Adventuring || string.IsNullOrWhiteSpace(gCharacter.Name) || gCharacter.Name.Equals("NONE", StringComparison.OrdinalIgnoreCase))
				{
					Globals.Error.Write("{0}Error: Assertion failed [{1}].",
						Environment.NewLine,
						gCharacter == null ? "gCharacter != null" :
						gCharacter.Uid <= 0 ? "gCharacter.Uid > 0" :
						gCharacter.Status != Status.Adventuring ? "gCharacter.Status == Status.Adventuring" :
						string.IsNullOrWhiteSpace(gCharacter.Name) ? "!string.IsNullOrWhiteSpace(gCharacter.Name)" :
						"!gCharacter.Name.Equals(\"NONE\", StringComparison.OrdinalIgnoreCase)");

					Globals.ExitType = ExitType.Error;

					Globals.MainLoop.ShouldShutdown = false;

					goto Cleanup;
				}

				Globals.Module = gEngine.GetModule();

				if (Globals.Module == null || Globals.Module.Uid <= 0)
				{
					Globals.Error.Write("{0}Error: Assertion failed [{1}].", Environment.NewLine, Globals.Module == null ? "Globals.Module != null" : "Globals.Module.Uid > 0");

					Globals.ExitType = ExitType.Error;

					Globals.MainLoop.ShouldShutdown = false;

					goto Cleanup;
				}

				Globals.GameState = gEngine.GetGameState();

				if (gGameState == null || gGameState.Uid <= 0)
				{
					Globals.Error.Write("{0}Error: Assertion failed [{1}].", Environment.NewLine, gGameState == null ? "gGameState != null" : "gGameState.Uid > 0");

					Globals.ExitType = ExitType.Error;

					Globals.MainLoop.ShouldShutdown = false;

					goto Cleanup;
				}

				CharacterRoom = gRDB[gGameState.Ro];

				if (CharacterRoom == null)
				{
					Globals.Error.Write("{0}Error: Assertion failed [{1}].", Environment.NewLine, "room != null");

					Globals.ExitType = ExitType.Error;

					Globals.MainLoop.ShouldShutdown = false;

					goto Cleanup;
				}

				FullArtifactList = Globals.Database.ArtifactTable.Records.ToList();

				foreach (var artifact in FullArtifactList)
				{
					if (artifact.IsCarriedByMonsterUid(gGameState.Cm))
					{
						artifact.SetCarriedByCharacter();
					}
					else if (artifact.IsWornByMonsterUid(gGameState.Cm))
					{
						artifact.SetWornByCharacter();
					}
				}

				FullMonsterList = Globals.Database.MonsterTable.Records.ToList();

				foreach (var monster in FullMonsterList)
				{
					monster.InitGroupCount = monster.CurrGroupCount;

					if (!Enum.IsDefined(typeof(Friendliness), monster.Reaction))
					{
						monster.ResolveReaction(gCharacter);
					}
				}

				rc = gEngine.ValidateRecordsAfterDatabaseLoaded();

				if (gEngine.IsFailure(rc))
				{
					Globals.Error.Write("Error: ValidateRecordsAfterDatabaseLoaded function call failed.");

					Globals.ExitType = ExitType.Error;

					Globals.MainLoop.ShouldShutdown = false;

					goto Cleanup;
				}

				gEngine.ClearActionLists();

				gSentenceParser.LastInputStr = "";

				gSentenceParser.Clear();

				gCommandParser.LastInputStr = "";

				gCommandParser.LastHimNameStr = "";

				gCommandParser.LastHerNameStr = "";

				gCommandParser.LastItNameStr = "";

				gCommandParser.LastThemNameStr = "";

				gGameState.R2 = gGameState.Ro;

				PrintGameRestored();

				gEngine.CreateInitialState(true);

				NextState = Globals.CurrState;

				Globals.CurrState = OrigCurrState;

		Cleanup:

				;
			}
			finally
			{
				if (SaveConfig != null)
				{
					SaveConfig.Dispose();
				}

				Globals.RevealContentCounter++;

				Globals.MutatePropertyCounter++;
			}
		}

		public RestoreCommand()
		{
			SortOrder = 420;

			IsSentenceParserEnabled = false;

			IsDarkEnabled = true;

			Name = "RestoreCommand";

			Verb = "restore";

			Type = CommandType.Miscellaneous;
		}
	}
}
