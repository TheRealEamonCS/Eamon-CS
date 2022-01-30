
// SaveCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using Eamon;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class SaveCommand : Command, ISaveCommand
	{
		public string _saveFilePath;

		public string _saveFileName;

		public string _saveFileExtension;

		public virtual long SaveSlot { get; set; }

		public virtual string SaveName { get; set; }

		/// <summary></summary>
		public virtual IList<IArtifact> FullArtifactList { get; set; }

		/// <summary></summary>
		public virtual IList<IFileset> SaveFilesetList { get; set; }

		/// <summary></summary>
		public virtual IFileset SaveFileset { get; set; }

		/// <summary></summary>
		public virtual IConfig SaveConfig { get; set; }

		/// <summary></summary>
		public virtual string SaveSlotString { get; set; }

		/// <summary></summary>
		public virtual string SaveFilePath
		{
			get
			{
				return _saveFilePath;
			}

			set
			{
				_saveFilePath = value;
			}
		}

		/// <summary></summary>
		public virtual string SaveFileName
		{
			get
			{
				return _saveFileName;
			}

			set
			{
				_saveFileName = value;
			}
		}

		/// <summary></summary>
		public virtual string SaveFileExtension
		{
			get
			{
				return _saveFileExtension;
			}

			set
			{
				_saveFileExtension = value;
			}
		}

		/// <summary></summary>
		public virtual long SaveFilesetsCount { get; set; }

		/// <summary></summary>
		public virtual long SaveFileNameIndex { get; set; }

		/// <summary></summary>
		public virtual bool GameSaved { get; set; }

		public override void Execute()
		{
			RetCode rc;

			Globals.RevealContentCounter--;

			SaveFilesetsCount = Globals.Database.GetFilesetsCount();

			Debug.Assert(SaveFilesetsCount <= gEngine.NumSaveSlots);

			Debug.Assert(SaveSlot >= 1 && SaveSlot <= Math.Min(SaveFilesetsCount + 1, gEngine.NumSaveSlots));

			Debug.Assert(SaveName != null);

			if (SaveSlot == SaveFilesetsCount + 1)
			{
				SaveFileset = Globals.CreateInstance<IFileset>(x =>
				{
					x.Uid = Globals.Database.GetFilesetUid();
					x.Name = "(none)";
				});

				rc = Globals.Database.AddFileset(SaveFileset);

				Debug.Assert(gEngine.IsSuccess(rc));
			}

			SaveFilesetList = Globals.Database.FilesetTable.Records.OrderBy(f => f.Uid).ToList();

			SaveFileset = SaveFilesetList[(int)SaveSlot - 1];

			if (SaveName.Length == 0)
			{
				if (!SaveFileset.Name.Equals("(none)", StringComparison.OrdinalIgnoreCase))
				{
					PrintChangeSaveName();

					Globals.Buf.Clear();

					rc = Globals.In.ReadField(Globals.Buf, Constants.BufSize02, null, ' ', '\0', false, null, gEngine.ModifyCharToUpper, gEngine.IsCharYOrN, null);

					Debug.Assert(gEngine.IsSuccess(rc));

					if (Globals.Buf.Length > 0 && Globals.Buf[0] == 'Y')
					{
						SaveFileset.Name = "(none)";
					}
				}

				while (SaveFileset.Name.Equals("(none)", StringComparison.OrdinalIgnoreCase))
				{
					PrintEnterSaveName();

					Globals.Buf.Clear();

					rc = Globals.In.ReadField(Globals.Buf, Constants.FsNameLen, null, ' ', '\0', false, null, null, null, null);

					Debug.Assert(gEngine.IsSuccess(rc));

					Globals.Buf.SetFormat("{0}", Regex.Replace(Globals.Buf.ToString(), @"\s+", " ").Trim());

					SaveFileset.Name = gEngine.Capitalize(Globals.Buf.ToString());

					if (SaveFileset.Name.Length == 0)
					{
						SaveFileset.Name = "(none)";
					}
				}
			}
			else
			{
				if (!SaveFileset.Name.Equals("(none)", StringComparison.OrdinalIgnoreCase) && SaveName.Equals("Quick Saved Game", StringComparison.OrdinalIgnoreCase))
				{
					SaveName = Globals.CloneInstance(SaveFileset.Name);
				}

				SaveName = gEngine.Capitalize(SaveName);

				SaveFileset.Name = Globals.CloneInstance(SaveName);

				gEngine.PrintQuickSave(SaveSlot, SaveName);
			}

			SaveConfig = Globals.CreateInstance<IConfig>();

			SaveSlotString = SaveSlot.ToString("D3");

			SaveFileset.WorkDir = "NONE";

			SaveFileset.PluginFileName = "NONE";

			SaveFilePath = "";

			SaveFileName = "";

			SaveFileExtension = "";

			rc = gEngine.SplitPath(Globals.ConfigFileName, ref _saveFilePath, ref _saveFileName, ref _saveFileExtension);

			Debug.Assert(gEngine.IsSuccess(rc));

			SaveFileNameIndex = SaveFileName.IndexOf('_');

			if (SaveFileNameIndex >= 0)
			{
				SaveFileName = SaveFileName.Substring(0, (int)SaveFileNameIndex);
			}

			Globals.Buf.SetFormat("{0}{1}_{2}{3}", SaveFilePath, SaveFileName, SaveSlotString, SaveFileExtension);

			SaveFileset.ConfigFileName = Globals.Buf.ToString().Truncate(Constants.FsFileNameLen);

			SaveFileset.FilesetFileName = "NONE";

			rc = gEngine.SplitPath(Globals.Config.RtCharacterFileName, ref _saveFilePath, ref _saveFileName, ref _saveFileExtension);

			Debug.Assert(gEngine.IsSuccess(rc));

			Globals.Buf.SetFormat("{0}{1}_{2}{3}", SaveFilePath, SaveFileName, SaveSlotString, SaveFileExtension);

			SaveFileset.CharacterFileName = Globals.Buf.ToString().Truncate(Constants.FsFileNameLen);

			rc = gEngine.SplitPath(Globals.Config.RtModuleFileName, ref _saveFilePath, ref _saveFileName, ref _saveFileExtension);

			Debug.Assert(gEngine.IsSuccess(rc));

			Globals.Buf.SetFormat("{0}{1}_{2}{3}", SaveFilePath, SaveFileName, SaveSlotString, SaveFileExtension);

			SaveFileset.ModuleFileName = Globals.Buf.ToString().Truncate(Constants.FsFileNameLen);

			rc = gEngine.SplitPath(Globals.Config.RtRoomFileName, ref _saveFilePath, ref _saveFileName, ref _saveFileExtension);

			Debug.Assert(gEngine.IsSuccess(rc));

			Globals.Buf.SetFormat("{0}{1}_{2}{3}", SaveFilePath, SaveFileName, SaveSlotString, SaveFileExtension);

			SaveFileset.RoomFileName = Globals.Buf.ToString().Truncate(Constants.FsFileNameLen);

			rc = gEngine.SplitPath(Globals.Config.RtArtifactFileName, ref _saveFilePath, ref _saveFileName, ref _saveFileExtension);

			Debug.Assert(gEngine.IsSuccess(rc));

			Globals.Buf.SetFormat("{0}{1}_{2}{3}", SaveFilePath, SaveFileName, SaveSlotString, SaveFileExtension);

			SaveFileset.ArtifactFileName = Globals.Buf.ToString().Truncate(Constants.FsFileNameLen);

			rc = gEngine.SplitPath(Globals.Config.RtEffectFileName, ref _saveFilePath, ref _saveFileName, ref _saveFileExtension);

			Debug.Assert(gEngine.IsSuccess(rc));

			Globals.Buf.SetFormat("{0}{1}_{2}{3}", SaveFilePath, SaveFileName, SaveSlotString, SaveFileExtension);

			SaveFileset.EffectFileName = Globals.Buf.ToString().Truncate(Constants.FsFileNameLen);

			rc = gEngine.SplitPath(Globals.Config.RtMonsterFileName, ref _saveFilePath, ref _saveFileName, ref _saveFileExtension);

			Debug.Assert(gEngine.IsSuccess(rc));

			Globals.Buf.SetFormat("{0}{1}_{2}{3}", SaveFilePath, SaveFileName, SaveSlotString, SaveFileExtension);

			SaveFileset.MonsterFileName = Globals.Buf.ToString().Truncate(Constants.FsFileNameLen);

			rc = gEngine.SplitPath(Globals.Config.RtHintFileName, ref _saveFilePath, ref _saveFileName, ref _saveFileExtension);

			Debug.Assert(gEngine.IsSuccess(rc));

			Globals.Buf.SetFormat("{0}{1}_{2}{3}", SaveFilePath, SaveFileName, SaveSlotString, SaveFileExtension);

			SaveFileset.HintFileName = Globals.Buf.ToString().Truncate(Constants.FsFileNameLen);

			if (!string.IsNullOrWhiteSpace(Globals.Config.RtTriggerFileName))          // TODO: remove this check at some point
			{
				rc = gEngine.SplitPath(Globals.Config.RtTriggerFileName, ref _saveFilePath, ref _saveFileName, ref _saveFileExtension);

				Debug.Assert(gEngine.IsSuccess(rc));

				Globals.Buf.SetFormat("{0}{1}_{2}{3}", SaveFilePath, SaveFileName, SaveSlotString, SaveFileExtension);

				SaveFileset.TriggerFileName = Globals.Buf.ToString().Truncate(Constants.FsFileNameLen);
			}

			if (!string.IsNullOrWhiteSpace(Globals.Config.RtScriptFileName))          // TODO: remove this check at some point
			{
				rc = gEngine.SplitPath(Globals.Config.RtScriptFileName, ref _saveFilePath, ref _saveFileName, ref _saveFileExtension);

				Debug.Assert(gEngine.IsSuccess(rc));

				Globals.Buf.SetFormat("{0}{1}_{2}{3}", SaveFilePath, SaveFileName, SaveSlotString, SaveFileExtension);

				SaveFileset.ScriptFileName = Globals.Buf.ToString().Truncate(Constants.FsFileNameLen);
			}

			rc = gEngine.SplitPath(Globals.Config.RtGameStateFileName, ref _saveFilePath, ref _saveFileName, ref _saveFileExtension);

			Debug.Assert(gEngine.IsSuccess(rc));

			Globals.Buf.SetFormat("{0}{1}_{2}{3}", SaveFilePath, SaveFileName, SaveSlotString, SaveFileExtension);

			SaveFileset.GameStateFileName = Globals.Buf.ToString().Truncate(Constants.FsFileNameLen);

			SaveConfig.RtFilesetFileName = Globals.CloneInstance(Globals.Config.RtFilesetFileName);

			SaveConfig.RtCharacterFileName = Globals.CloneInstance(SaveFileset.CharacterFileName);

			SaveConfig.RtModuleFileName = Globals.CloneInstance(SaveFileset.ModuleFileName);

			SaveConfig.RtRoomFileName = Globals.CloneInstance(SaveFileset.RoomFileName);

			SaveConfig.RtArtifactFileName = Globals.CloneInstance(SaveFileset.ArtifactFileName);

			SaveConfig.RtEffectFileName = Globals.CloneInstance(SaveFileset.EffectFileName);

			SaveConfig.RtMonsterFileName = Globals.CloneInstance(SaveFileset.MonsterFileName);

			SaveConfig.RtHintFileName = Globals.CloneInstance(SaveFileset.HintFileName);

			SaveConfig.RtTriggerFileName = Globals.CloneInstance(SaveFileset.TriggerFileName);

			SaveConfig.RtScriptFileName = Globals.CloneInstance(SaveFileset.ScriptFileName);

			SaveConfig.RtGameStateFileName = Globals.CloneInstance(SaveFileset.GameStateFileName);
			
			Globals.Config.DdFilesetFileName = SaveConfig.RtFilesetFileName;

			Globals.Config.DdCharacterFileName = SaveConfig.RtCharacterFileName;

			Globals.Config.DdModuleFileName = SaveConfig.RtModuleFileName;

			Globals.Config.DdRoomFileName = SaveConfig.RtRoomFileName;

			Globals.Config.DdArtifactFileName = SaveConfig.RtArtifactFileName;

			Globals.Config.DdEffectFileName = SaveConfig.RtEffectFileName;

			Globals.Config.DdMonsterFileName = SaveConfig.RtMonsterFileName;

			Globals.Config.DdHintFileName = SaveConfig.RtHintFileName;

			Globals.Config.DdTriggerFileName = SaveConfig.RtTriggerFileName;

			Globals.Config.DdScriptFileName = SaveConfig.RtScriptFileName;

			Globals.Config.DdEditingFilesets = true;

			Globals.Config.DdEditingCharacters = true;

			Globals.Config.DdEditingModules = true;

			Globals.Config.DdEditingRooms = true;

			Globals.Config.DdEditingArtifacts = true;

			Globals.Config.DdEditingEffects = true;

			Globals.Config.DdEditingMonsters = true;

			Globals.Config.DdEditingHints = true;

			Globals.Config.DdEditingTriggers = true;

			Globals.Config.DdEditingScripts = true;

			FullArtifactList = Globals.Database.ArtifactTable.Records.ToList();

			foreach (var artifact in FullArtifactList)
			{
				if (artifact.IsCarriedByCharacter())
				{
					artifact.SetCarriedByMonsterUid(gGameState.Cm);
				}
				else if (artifact.IsWornByCharacter())
				{
					artifact.SetWornByMonsterUid(gGameState.Cm);
				}
			}

			GameSaved = true;

			rc = SaveConfig.SaveGameDatabase(false);

			if (gEngine.IsFailure(rc))
			{
				Globals.Error.WriteLine("Error: SaveGameDatabase function call failed");

				GameSaved = false;
			}

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

			rc = Globals.Database.SaveConfigs(SaveFileset.ConfigFileName, false);

			if (gEngine.IsFailure(rc))
			{
				Globals.Error.WriteLine("Error: SaveConfigs function call failed");

				GameSaved = false;
			}

			SaveConfig.Dispose();

			if (GameSaved)
			{
				PrintGameSaved();
			}
			else
			{
				PrintGameNotSaved();
			}

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IStartState>();
			}

			Globals.RevealContentCounter++;
		}

		public override bool ShouldPreTurnProcess()
		{
			return false;
		}

		public SaveCommand()
		{
			SortOrder = 410;

			IsSentenceParserEnabled = false;

			IsDarkEnabled = true;

			Uid = 63;

			Name = "SaveCommand";

			Verb = "save";

			Type = CommandType.Miscellaneous;

			SaveName = "";
		}
	}
}
