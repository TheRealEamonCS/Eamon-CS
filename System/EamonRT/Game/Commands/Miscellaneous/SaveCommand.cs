
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
using static EamonRT.Game.Plugin.Globals;

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
		public virtual long OrigCm { get; set; }

		/// <summary></summary>
		public virtual bool GameSaved { get; set; }

		public override void ExecuteForPlayer()
		{
			RetCode rc;
			
			try
			{
				gEngine.MutatePropertyCounter--;

				gEngine.RevealContentCounter--;

				SaveFilesetsCount = gEngine.Database.GetFilesetCount();

				Debug.Assert(SaveFilesetsCount <= gEngine.NumSaveSlots);

				Debug.Assert(SaveSlot >= 1 && SaveSlot <= Math.Min(SaveFilesetsCount + 1, gEngine.NumSaveSlots));

				Debug.Assert(SaveName != null);

				if (SaveSlot == SaveFilesetsCount + 1)
				{
					SaveFileset = gEngine.CreateInstance<IFileset>(x =>
					{
						x.Uid = gEngine.Database.GetFilesetUid();
						x.Name = "(none)";
					});

					rc = gEngine.Database.AddFileset(SaveFileset);

					Debug.Assert(gEngine.IsSuccess(rc));
				}

				SaveFilesetList = gEngine.Database.FilesetTable.Records.OrderBy(f => f.Uid).ToList();

				SaveFileset = SaveFilesetList[(int)SaveSlot - 1];

				if (SaveName.Length == 0)
				{
					if (!SaveFileset.Name.Equals("(none)", StringComparison.OrdinalIgnoreCase))
					{
						PrintChangeSaveName();

						gEngine.Buf.Clear();

						rc = gEngine.In.ReadField(gEngine.Buf, gEngine.BufSize02, null, ' ', '\0', false, null, gEngine.ModifyCharToUpper, gEngine.IsCharYOrN, null);

						Debug.Assert(gEngine.IsSuccess(rc));

						if (gEngine.Buf.Length > 0 && gEngine.Buf[0] == 'Y')
						{
							SaveFileset.Name = "(none)";
						}
					}

					while (SaveFileset.Name.Equals("(none)", StringComparison.OrdinalIgnoreCase))
					{
						PrintEnterSaveName();

						gEngine.Buf.Clear();

						rc = gEngine.In.ReadField(gEngine.Buf, gEngine.FsNameLen, null, ' ', '\0', false, null, null, null, null);

						Debug.Assert(gEngine.IsSuccess(rc));

						gEngine.Buf.SetFormat("{0}", Regex.Replace(gEngine.Buf.ToString(), @"\s+", " ").Trim());

						SaveFileset.Name = gEngine.Capitalize(gEngine.Buf.ToString());

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
						SaveName = gEngine.CloneInstance(SaveFileset.Name);
					}

					SaveName = gEngine.Capitalize(SaveName);

					SaveFileset.Name = gEngine.CloneInstance(SaveName);

					gEngine.PrintQuickSave(SaveSlot, SaveName);
				}

				SaveConfig = gEngine.CreateInstance<IConfig>();

				SaveSlotString = SaveSlot.ToString("D3");

				SaveFileset.WorkDir = "NONE";

				SaveFileset.PluginFileName = "NONE";

				SaveFilePath = "";

				SaveFileName = "";

				SaveFileExtension = "";

				rc = gEngine.SplitPath(gEngine.ConfigFileName, ref _saveFilePath, ref _saveFileName, ref _saveFileExtension);

				Debug.Assert(gEngine.IsSuccess(rc));

				SaveFileNameIndex = SaveFileName.IndexOf('_');

				if (SaveFileNameIndex >= 0)
				{
					SaveFileName = SaveFileName.Substring(0, (int)SaveFileNameIndex);
				}

				gEngine.Buf.SetFormat("{0}{1}_{2}{3}", SaveFilePath, SaveFileName, SaveSlotString, SaveFileExtension);

				SaveFileset.ConfigFileName = gEngine.Buf.ToString().Truncate(gEngine.FsFileNameLen);

				SaveFileset.FilesetFileName = "NONE";

				rc = gEngine.SplitPath(gEngine.Config.RtCharacterFileName, ref _saveFilePath, ref _saveFileName, ref _saveFileExtension);

				Debug.Assert(gEngine.IsSuccess(rc));

				gEngine.Buf.SetFormat("{0}{1}_{2}{3}", SaveFilePath, SaveFileName, SaveSlotString, SaveFileExtension);

				SaveFileset.CharacterFileName = gEngine.Buf.ToString().Truncate(gEngine.FsFileNameLen);

				rc = gEngine.SplitPath(gEngine.Config.RtModuleFileName, ref _saveFilePath, ref _saveFileName, ref _saveFileExtension);

				Debug.Assert(gEngine.IsSuccess(rc));

				gEngine.Buf.SetFormat("{0}{1}_{2}{3}", SaveFilePath, SaveFileName, SaveSlotString, SaveFileExtension);

				SaveFileset.ModuleFileName = gEngine.Buf.ToString().Truncate(gEngine.FsFileNameLen);

				rc = gEngine.SplitPath(gEngine.Config.RtRoomFileName, ref _saveFilePath, ref _saveFileName, ref _saveFileExtension);

				Debug.Assert(gEngine.IsSuccess(rc));

				gEngine.Buf.SetFormat("{0}{1}_{2}{3}", SaveFilePath, SaveFileName, SaveSlotString, SaveFileExtension);

				SaveFileset.RoomFileName = gEngine.Buf.ToString().Truncate(gEngine.FsFileNameLen);

				rc = gEngine.SplitPath(gEngine.Config.RtArtifactFileName, ref _saveFilePath, ref _saveFileName, ref _saveFileExtension);

				Debug.Assert(gEngine.IsSuccess(rc));

				gEngine.Buf.SetFormat("{0}{1}_{2}{3}", SaveFilePath, SaveFileName, SaveSlotString, SaveFileExtension);

				SaveFileset.ArtifactFileName = gEngine.Buf.ToString().Truncate(gEngine.FsFileNameLen);

				rc = gEngine.SplitPath(gEngine.Config.RtEffectFileName, ref _saveFilePath, ref _saveFileName, ref _saveFileExtension);

				Debug.Assert(gEngine.IsSuccess(rc));

				gEngine.Buf.SetFormat("{0}{1}_{2}{3}", SaveFilePath, SaveFileName, SaveSlotString, SaveFileExtension);

				SaveFileset.EffectFileName = gEngine.Buf.ToString().Truncate(gEngine.FsFileNameLen);

				rc = gEngine.SplitPath(gEngine.Config.RtMonsterFileName, ref _saveFilePath, ref _saveFileName, ref _saveFileExtension);

				Debug.Assert(gEngine.IsSuccess(rc));

				gEngine.Buf.SetFormat("{0}{1}_{2}{3}", SaveFilePath, SaveFileName, SaveSlotString, SaveFileExtension);

				SaveFileset.MonsterFileName = gEngine.Buf.ToString().Truncate(gEngine.FsFileNameLen);

				rc = gEngine.SplitPath(gEngine.Config.RtHintFileName, ref _saveFilePath, ref _saveFileName, ref _saveFileExtension);

				Debug.Assert(gEngine.IsSuccess(rc));

				gEngine.Buf.SetFormat("{0}{1}_{2}{3}", SaveFilePath, SaveFileName, SaveSlotString, SaveFileExtension);

				SaveFileset.HintFileName = gEngine.Buf.ToString().Truncate(gEngine.FsFileNameLen);

				rc = gEngine.SplitPath(gEngine.Config.RtGameStateFileName, ref _saveFilePath, ref _saveFileName, ref _saveFileExtension);

				Debug.Assert(gEngine.IsSuccess(rc));

				gEngine.Buf.SetFormat("{0}{1}_{2}{3}", SaveFilePath, SaveFileName, SaveSlotString, SaveFileExtension);

				SaveFileset.GameStateFileName = gEngine.Buf.ToString().Truncate(gEngine.FsFileNameLen);

				SaveConfig.RtFilesetFileName = gEngine.CloneInstance(gEngine.Config.RtFilesetFileName);

				SaveConfig.RtCharacterFileName = gEngine.CloneInstance(SaveFileset.CharacterFileName);

				SaveConfig.RtModuleFileName = gEngine.CloneInstance(SaveFileset.ModuleFileName);

				SaveConfig.RtRoomFileName = gEngine.CloneInstance(SaveFileset.RoomFileName);

				SaveConfig.RtArtifactFileName = gEngine.CloneInstance(SaveFileset.ArtifactFileName);

				SaveConfig.RtEffectFileName = gEngine.CloneInstance(SaveFileset.EffectFileName);

				SaveConfig.RtMonsterFileName = gEngine.CloneInstance(SaveFileset.MonsterFileName);

				SaveConfig.RtHintFileName = gEngine.CloneInstance(SaveFileset.HintFileName);

				SaveConfig.RtGameStateFileName = gEngine.CloneInstance(SaveFileset.GameStateFileName);

				gEngine.Config.DdFilesetFileName = SaveConfig.RtFilesetFileName;

				gEngine.Config.DdCharacterFileName = SaveConfig.RtCharacterFileName;

				gEngine.Config.DdModuleFileName = SaveConfig.RtModuleFileName;

				gEngine.Config.DdRoomFileName = SaveConfig.RtRoomFileName;

				gEngine.Config.DdArtifactFileName = SaveConfig.RtArtifactFileName;

				gEngine.Config.DdEffectFileName = SaveConfig.RtEffectFileName;

				gEngine.Config.DdMonsterFileName = SaveConfig.RtMonsterFileName;

				gEngine.Config.DdHintFileName = SaveConfig.RtHintFileName;

				gEngine.Config.DdEditingFilesets = true;

				gEngine.Config.DdEditingCharacters = true;

				gEngine.Config.DdEditingModules = true;

				gEngine.Config.DdEditingRooms = true;

				gEngine.Config.DdEditingArtifacts = true;

				gEngine.Config.DdEditingEffects = true;

				gEngine.Config.DdEditingMonsters = true;

				gEngine.Config.DdEditingHints = true;

				OrigCm = gGameState.Cm;

				FullArtifactList = gEngine.Database.ArtifactTable.Records.ToList();

				foreach (var artifact in FullArtifactList)
				{
					if (artifact.IsCarriedByMonster(gCharMonster))
					{
						gGameState.Cm = 0;

						artifact.SetCarriedByMonsterUid(OrigCm);

						gGameState.Cm = OrigCm;
					}
					else if (artifact.IsWornByMonster(gCharMonster))
					{
						gGameState.Cm = 0;

						artifact.SetWornByMonsterUid(OrigCm);

						gGameState.Cm = OrigCm;
					}
				}

				GameSaved = true;

				rc = SaveConfig.SaveGameDatabase(false);

				if (gEngine.IsFailure(rc))
				{
					gEngine.Error.WriteLine("Error: SaveGameDatabase function call failed.");

					GameSaved = false;
				}

				gGameState.Cm = 0;

				foreach (var artifact in FullArtifactList)
				{
					if (artifact.IsCarriedByMonsterUid(OrigCm))
					{
						artifact.SetCarriedByMonster(gCharMonster);
					}
					else if (artifact.IsWornByMonsterUid(OrigCm))
					{
						artifact.SetWornByMonster(gCharMonster);
					}
				}

				gGameState.Cm = OrigCm;

				rc = gEngine.Database.SaveConfigs(SaveFileset.ConfigFileName, false);

				if (gEngine.IsFailure(rc))
				{
					gEngine.Error.WriteLine("Error: SaveConfigs function call failed.");

					GameSaved = false;
				}

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
					NextState = gEngine.CreateInstance<IStartState>();
				}
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

		public SaveCommand()
		{
			SortOrder = 410;

			IsSentenceParserEnabled = false;

			IsDarkEnabled = true;

			Name = "SaveCommand";

			Verb = "save";

			Type = CommandType.Miscellaneous;

			SaveName = "";
		}
	}
}
