
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
using static EamonRT.Game.Plugin.Globals;

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
			
			try
			{
				gEngine.MutatePropertyCounter--;

				gEngine.RevealContentCounter--;

				OrigCurrState = gEngine.CurrState;

				SaveFilesetsCount = gEngine.Database.GetFilesetCount();

				Debug.Assert(SaveFilesetsCount <= gEngine.NumSaveSlots);

				Debug.Assert(SaveSlot >= 1 && SaveSlot <= SaveFilesetsCount);

				SaveFilesetList = gEngine.Database.FilesetTable.Records.OrderBy(f => f.Uid).ToList();

				SaveFileset = SaveFilesetList[(int)SaveSlot - 1];

				SaveConfig = gEngine.CreateInstance<IConfig>();

				rc = gEngine.Database.LoadConfigs(SaveFileset.ConfigFileName, printOutput: false);

				if (gEngine.IsFailure(rc))
				{
					gEngine.Error.Write("Error: LoadConfigs function call failed.");

					gEngine.ExitType = ExitType.Error;

					gEngine.MainLoop.ShouldShutdown = false;

					goto Cleanup;
				}

				gEngine.Config = gEngine.GetConfig();

				if (gEngine.Config == null || gEngine.Config.Uid <= 0)
				{
					gEngine.Error.Write("{0}Error: Assertion failed [{1}].", Environment.NewLine, gEngine.Config == null ? "gEngine.Config != null" : "gEngine.Config.Uid > 0");

					gEngine.ExitType = ExitType.Error;

					gEngine.MainLoop.ShouldShutdown = false;

					goto Cleanup;
				}

				gEngine.ResetProperties(PropertyResetCode.RestoreGame);

				gEngine.ResetMonsterStats(ActorMonster);

				SaveConfig.RtFilesetFileName = gEngine.CloneInstance(gEngine.Config.RtFilesetFileName);

				SaveConfig.RtCharacterFileName = gEngine.CloneInstance(SaveFileset.CharacterFileName);

				SaveConfig.RtModuleFileName = gEngine.CloneInstance(SaveFileset.ModuleFileName);

				SaveConfig.RtRoomFileName = gEngine.CloneInstance(SaveFileset.RoomFileName);

				SaveConfig.RtArtifactFileName = gEngine.CloneInstance(SaveFileset.ArtifactFileName);

				SaveConfig.RtEffectFileName = gEngine.CloneInstance(SaveFileset.EffectFileName);

				SaveConfig.RtMonsterFileName = gEngine.CloneInstance(SaveFileset.MonsterFileName);

				SaveConfig.RtHintFileName = gEngine.CloneInstance(SaveFileset.HintFileName);

				SaveConfig.RtGameStateFileName = gEngine.CloneInstance(SaveFileset.GameStateFileName);

				rc = SaveConfig.LoadGameDatabase(printOutput: false);

				if (gEngine.IsFailure(rc))
				{
					gEngine.Error.Write("Error: LoadGameDatabase function call failed.");

					gEngine.ExitType = ExitType.Error;

					gEngine.MainLoop.ShouldShutdown = false;

					goto Cleanup;
				}

				// fileset is now invalid

				gEngine.Character = gEngine.Database.CharacterTable.Records.FirstOrDefault();

				if (gCharacter == null || gCharacter.Uid <= 0 || gCharacter.Status != Status.Adventuring || string.IsNullOrWhiteSpace(gCharacter.Name) || gCharacter.Name.Equals("NONE", StringComparison.OrdinalIgnoreCase))
				{
					gEngine.Error.Write("{0}Error: Assertion failed [{1}].",
						Environment.NewLine,
						gCharacter == null ? "gCharacter != null" :
						gCharacter.Uid <= 0 ? "gCharacter.Uid > 0" :
						gCharacter.Status != Status.Adventuring ? "gCharacter.Status == Status.Adventuring" :
						string.IsNullOrWhiteSpace(gCharacter.Name) ? "!string.IsNullOrWhiteSpace(gCharacter.Name)" :
						"!gCharacter.Name.Equals(\"NONE\", StringComparison.OrdinalIgnoreCase)");

					gEngine.ExitType = ExitType.Error;

					gEngine.MainLoop.ShouldShutdown = false;

					goto Cleanup;
				}

				gEngine.Module = gEngine.GetModule();

				if (gEngine.Module == null || gEngine.Module.Uid <= 0)
				{
					gEngine.Error.Write("{0}Error: Assertion failed [{1}].", Environment.NewLine, gEngine.Module == null ? "gEngine.Module != null" : "gEngine.Module.Uid > 0");

					gEngine.ExitType = ExitType.Error;

					gEngine.MainLoop.ShouldShutdown = false;

					goto Cleanup;
				}

				gEngine.GameState = gEngine.GetGameState();

				if (gGameState == null || gGameState.Uid <= 0)
				{
					gEngine.Error.Write("{0}Error: Assertion failed [{1}].", Environment.NewLine, gGameState == null ? "gGameState != null" : "gGameState.Uid > 0");

					gEngine.ExitType = ExitType.Error;

					gEngine.MainLoop.ShouldShutdown = false;

					goto Cleanup;
				}

				CharacterRoom = gRDB[gGameState.Ro];

				if (CharacterRoom == null)
				{
					gEngine.Error.Write("{0}Error: Assertion failed [{1}].", Environment.NewLine, "CharacterRoom != null");

					gEngine.ExitType = ExitType.Error;

					gEngine.MainLoop.ShouldShutdown = false;

					goto Cleanup;
				}

				FullArtifactList = gEngine.Database.ArtifactTable.Records.ToList();

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

				FullMonsterList = gEngine.Database.MonsterTable.Records.ToList();

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
					gEngine.Error.Write("Error: ValidateRecordsAfterDatabaseLoaded function call failed.");

					gEngine.ExitType = ExitType.Error;

					gEngine.MainLoop.ShouldShutdown = false;

					goto Cleanup;
				}

				PrintGameRestored();

				gGameState.R2 = gGameState.Ro;

				gEngine.CreateInitialState(true);

				NextState = gEngine.CurrState;

				gEngine.CurrState = OrigCurrState;

		Cleanup:

				;
			}
			finally
			{
				if (SaveConfig != null)
				{
					SaveConfig.Dispose();
				}

				gEngine.RevealContentCounter++;

				gEngine.MutatePropertyCounter++;
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
