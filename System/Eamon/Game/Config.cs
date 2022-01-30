
// Config.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using static Eamon.Game.Plugin.PluginContext;

namespace Eamon.Game
{
	[ClassMappings]
	public class Config : GameBase, IConfig
	{
		#region Public Properties

		#region Interface IConfig

		[FieldName(620)]
		public virtual bool ShowDesc { get; set; }

		[FieldName(640)]
		public virtual bool ResolveEffects { get; set; }

		[FieldName(660)]
		public virtual bool GenerateUids { get; set; }

		[FieldName(680)]
		public virtual FieldDesc FieldDesc { get; set; }

		[FieldName(700)]
		public virtual long WordWrapMargin { get; set; }

		[FieldName(720)]
		public virtual string DdFilesetFileName { get; set; }

		[FieldName(740)]
		public virtual string DdCharacterFileName { get; set; }

		[FieldName(760)]
		public virtual string DdModuleFileName { get; set; }

		[FieldName(780)]
		public virtual string DdRoomFileName { get; set; }

		[FieldName(800)]
		public virtual string DdArtifactFileName { get; set; }

		[FieldName(820)]
		public virtual string DdEffectFileName { get; set; }

		[FieldName(840)]
		public virtual string DdMonsterFileName { get; set; }

		[FieldName(860)]
		public virtual string DdHintFileName { get; set; }

		//[FieldName(880)]
		public virtual string DdTriggerFileName { get; set; }

		//[FieldName(900)]
		public virtual string DdScriptFileName { get; set; }

		[FieldName(920)]
		public virtual string MhWorkDir { get; set; }

		[FieldName(940)]
		public virtual string MhFilesetFileName { get; set; }

		[FieldName(960)]
		public virtual string MhCharacterFileName { get; set; }

		[FieldName(980)]
		public virtual string MhEffectFileName { get; set; }

		[FieldName(1000)]
		public virtual string RtFilesetFileName { get; set; }

		[FieldName(1020)]
		public virtual string RtCharacterFileName { get; set; }

		[FieldName(1040)]
		public virtual string RtModuleFileName { get; set; }

		[FieldName(1060)]
		public virtual string RtRoomFileName { get; set; }

		[FieldName(1080)]
		public virtual string RtArtifactFileName { get; set; }

		[FieldName(1100)]
		public virtual string RtEffectFileName { get; set; }

		[FieldName(1120)]
		public virtual string RtMonsterFileName { get; set; }

		[FieldName(1140)]
		public virtual string RtHintFileName { get; set; }

		//[FieldName(1160)]
		public virtual string RtTriggerFileName { get; set; }

		//[FieldName(1180)]
		public virtual string RtScriptFileName { get; set; }

		[FieldName(1200)]
		public virtual string RtGameStateFileName { get; set; }

		[FieldName(1220)]
		public virtual bool DdEditingFilesets { get; set; }

		[FieldName(1240)]
		public virtual bool DdEditingCharacters { get; set; }

		[FieldName(1260)]
		public virtual bool DdEditingModules { get; set; }

		[FieldName(1280)]
		public virtual bool DdEditingRooms { get; set; }

		[FieldName(1300)]
		public virtual bool DdEditingArtifacts { get; set; }

		[FieldName(1320)]
		public virtual bool DdEditingEffects { get; set; }

		[FieldName(1340)]
		public virtual bool DdEditingMonsters { get; set; }

		[FieldName(1360)]
		public virtual bool DdEditingHints { get; set; }

		//[FieldName(1380)]
		public virtual bool DdEditingTriggers { get; set; }

		//[FieldName(1400)]
		public virtual bool DdEditingScripts { get; set; }

		#endregion

		#endregion

		#region Public Methods

		#region Interface IDisposable

		public override void Dispose(bool disposing)
		{
			if (disposing)
			{
				// get rid of managed resources
			}

			if (IsUidRecycled && Uid > 0)
			{
				Globals.Database.FreeConfigUid(Uid);

				Uid = 0;
			}
		}

		#endregion

		#region Interface IComparable

		public virtual int CompareTo(IConfig config)
		{
			return this.Uid.CompareTo(config.Uid);
		}

		#endregion

		#region Interface IConfig

		public virtual RetCode LoadGameDatabase(bool validate = true, bool printOutput = true)
		{
			RetCode rc;

			Globals.ResetRevealContentProperties();

			rc = Globals.Database.LoadFilesets(RtFilesetFileName, validate, printOutput);

			if (gEngine.IsFailure(rc))
			{
				Globals.Error.WriteLine("Error: LoadFilesets function call failed");

				goto Cleanup;
			}

			rc = Globals.Database.LoadCharacters(RtCharacterFileName, validate, printOutput);

			if (gEngine.IsFailure(rc))
			{
				Globals.Error.WriteLine("Error: LoadCharacters function call failed");

				goto Cleanup;
			}

			rc = Globals.Database.LoadModules(RtModuleFileName, validate, printOutput);

			if (gEngine.IsFailure(rc))
			{
				Globals.Error.WriteLine("Error: LoadModules function call failed");

				goto Cleanup;
			}

			rc = Globals.Database.LoadRooms(RtRoomFileName, validate, printOutput);

			if (gEngine.IsFailure(rc))
			{
				Globals.Error.WriteLine("Error: LoadRooms function call failed");

				goto Cleanup;
			}

			rc = Globals.Database.LoadArtifacts(RtArtifactFileName, validate, printOutput);

			if (gEngine.IsFailure(rc))
			{
				Globals.Error.WriteLine("Error: LoadArtifacts function call failed");

				goto Cleanup;
			}

			rc = Globals.Database.LoadEffects(RtEffectFileName, validate, printOutput);

			if (gEngine.IsFailure(rc))
			{
				Globals.Error.WriteLine("Error: LoadEffects function call failed");

				goto Cleanup;
			}

			rc = Globals.Database.LoadMonsters(RtMonsterFileName, validate, printOutput);

			if (gEngine.IsFailure(rc))
			{
				Globals.Error.WriteLine("Error: LoadMonsters function call failed");

				goto Cleanup;
			}

			rc = Globals.Database.LoadHints(RtHintFileName, validate, printOutput);

			if (gEngine.IsFailure(rc))
			{
				Globals.Error.WriteLine("Error: LoadHints function call failed");

				goto Cleanup;
			}

			if (!string.IsNullOrWhiteSpace(RtTriggerFileName))				// TODO: remove this check at some point
			{
				rc = Globals.Database.LoadTriggers(RtTriggerFileName, validate, printOutput);

				if (gEngine.IsFailure(rc))
				{
					Globals.Error.WriteLine("Error: LoadTriggers function call failed");

					goto Cleanup;
				}
			}

			if (!string.IsNullOrWhiteSpace(RtScriptFileName))          // TODO: remove this check at some point
			{
				rc = Globals.Database.LoadScripts(RtScriptFileName, validate, printOutput);

				if (gEngine.IsFailure(rc))
				{
					Globals.Error.WriteLine("Error: LoadScripts function call failed");

					goto Cleanup;
				}
			}

			rc = Globals.Database.LoadGameStates(RtGameStateFileName, validate, printOutput);

			if (gEngine.IsFailure(rc))
			{
				Globals.Error.WriteLine("Error: LoadGameStates function call failed");

				goto Cleanup;
			}

		Cleanup:

			return rc;
		}

		public virtual RetCode SaveGameDatabase(bool printOutput = true)
		{
			RetCode rc;

			rc = Globals.Database.SaveGameStates(RtGameStateFileName, printOutput);

			if (gEngine.IsFailure(rc))
			{
				Globals.Error.WriteLine("Error: SaveGameStates function call failed");

				goto Cleanup;
			}

			if (!string.IsNullOrWhiteSpace(RtScriptFileName))          // TODO: remove this check at some point
			{
				rc = Globals.Database.SaveScripts(RtScriptFileName, printOutput);

				if (gEngine.IsFailure(rc))
				{
					Globals.Error.WriteLine("Error: SaveScripts function call failed");

					goto Cleanup;
				}
			}

			if (!string.IsNullOrWhiteSpace(RtTriggerFileName))          // TODO: remove this check at some point
			{
				rc = Globals.Database.SaveTriggers(RtTriggerFileName, printOutput);

				if (gEngine.IsFailure(rc))
				{
					Globals.Error.WriteLine("Error: SaveTriggers function call failed");

					goto Cleanup;
				}
			}

			rc = Globals.Database.SaveHints(RtHintFileName, printOutput);

			if (gEngine.IsFailure(rc))
			{
				Globals.Error.WriteLine("Error: SaveHints function call failed");

				goto Cleanup;
			}

			rc = Globals.Database.SaveMonsters(RtMonsterFileName, printOutput);

			if (gEngine.IsFailure(rc))
			{
				Globals.Error.WriteLine("Error: SaveMonsters function call failed");

				goto Cleanup;
			}

			rc = Globals.Database.SaveEffects(RtEffectFileName, printOutput);

			if (gEngine.IsFailure(rc))
			{
				Globals.Error.WriteLine("Error: SaveEffects function call failed");

				goto Cleanup;
			}

			rc = Globals.Database.SaveArtifacts(RtArtifactFileName, printOutput);

			if (gEngine.IsFailure(rc))
			{
				Globals.Error.WriteLine("Error: SaveArtifacts function call failed");

				goto Cleanup;
			}

			rc = Globals.Database.SaveRooms(RtRoomFileName, printOutput);

			if (gEngine.IsFailure(rc))
			{
				Globals.Error.WriteLine("Error: SaveRooms function call failed");

				goto Cleanup;
			}

			rc = Globals.Database.SaveModules(RtModuleFileName, printOutput);

			if (gEngine.IsFailure(rc))
			{
				Globals.Error.WriteLine("Error: SaveModules function call failed");

				goto Cleanup;
			}

			rc = Globals.Database.SaveCharacters(RtCharacterFileName, printOutput);

			if (gEngine.IsFailure(rc))
			{
				Globals.Error.WriteLine("Error: SaveCharacters function call failed");

				goto Cleanup;
			}

			rc = Globals.Database.SaveFilesets(RtFilesetFileName, printOutput);

			if (gEngine.IsFailure(rc))
			{
				Globals.Error.WriteLine("Error: SaveFilesets function call failed");

				goto Cleanup;
			}

		Cleanup:

			return rc;
		}

		public virtual RetCode DeleteGameState(string configFileName, bool startOver)
		{
			RetCode rc;

			if (configFileName == null)
			{
				rc = RetCode.InvalidArg;

				// PrintError

				goto Cleanup;
			}

			rc = RetCode.Success;

			foreach (var fs in Globals.Database.FilesetTable.Records)
			{
				rc = fs.DeleteFiles(null);

				Debug.Assert(gEngine.IsSuccess(rc));
			}

			if (startOver)
			{
				rc = Globals.Database.FreeFilesets();

				Debug.Assert(gEngine.IsSuccess(rc));

				rc = Globals.Database.SaveFilesets(RtFilesetFileName, false);

				Debug.Assert(gEngine.IsSuccess(rc));
			}
			else
			{
				try
				{
					Globals.File.Delete(configFileName);
				}
				catch (Exception ex)
				{
					if (ex != null)
					{
						// do nothing
					}
				}

				try
				{
					Globals.File.Delete(RtCharacterFileName);
				}
				catch (Exception ex)
				{
					if (ex != null)
					{
						// do nothing
					}
				}

				try
				{
					Globals.File.Delete(RtFilesetFileName);
				}
				catch (Exception ex)
				{
					if (ex != null)
					{
						// do nothing
					}
				}
			}

		Cleanup:

			return rc;
		}

		public virtual RetCode CopyProperties(IConfig config)
		{
			RetCode rc;

			if (config == null)
			{
				rc = RetCode.InvalidArg;

				// PrintError

				goto Cleanup;
			}

			rc = RetCode.Success;

			Uid = config.Uid;

			IsUidRecycled = config.IsUidRecycled;

			ShowDesc = config.ShowDesc;

			ResolveEffects = config.ResolveEffects;

			GenerateUids = config.GenerateUids;

			FieldDesc = config.FieldDesc;

			WordWrapMargin = config.WordWrapMargin;

			DdFilesetFileName = Globals.CloneInstance(config.DdFilesetFileName);

			DdCharacterFileName = Globals.CloneInstance(config.DdCharacterFileName);

			DdModuleFileName = Globals.CloneInstance(config.DdModuleFileName);

			DdRoomFileName = Globals.CloneInstance(config.DdRoomFileName);

			DdArtifactFileName = Globals.CloneInstance(config.DdArtifactFileName);

			DdEffectFileName = Globals.CloneInstance(config.DdEffectFileName);

			DdMonsterFileName = Globals.CloneInstance(config.DdMonsterFileName);

			DdHintFileName = Globals.CloneInstance(config.DdHintFileName);

			DdTriggerFileName = Globals.CloneInstance(config.DdTriggerFileName);

			DdScriptFileName = Globals.CloneInstance(config.DdScriptFileName);

			MhWorkDir = Globals.CloneInstance(config.MhWorkDir);

			MhFilesetFileName = Globals.CloneInstance(config.MhFilesetFileName);

			MhCharacterFileName = Globals.CloneInstance(config.MhCharacterFileName);

			MhEffectFileName = Globals.CloneInstance(config.MhEffectFileName);

			RtFilesetFileName = Globals.CloneInstance(config.RtFilesetFileName);

			RtCharacterFileName = Globals.CloneInstance(config.RtCharacterFileName);

			RtModuleFileName = Globals.CloneInstance(config.RtModuleFileName);

			RtRoomFileName = Globals.CloneInstance(config.RtRoomFileName);

			RtArtifactFileName = Globals.CloneInstance(config.RtArtifactFileName);

			RtEffectFileName = Globals.CloneInstance(config.RtEffectFileName);

			RtMonsterFileName = Globals.CloneInstance(config.RtMonsterFileName);

			RtHintFileName = Globals.CloneInstance(config.RtHintFileName);

			RtTriggerFileName = Globals.CloneInstance(config.RtTriggerFileName);

			RtScriptFileName = Globals.CloneInstance(config.RtScriptFileName);

			RtGameStateFileName = Globals.CloneInstance(config.RtGameStateFileName);

			DdEditingFilesets = config.DdEditingFilesets;

			DdEditingCharacters = config.DdEditingCharacters;

			DdEditingModules = config.DdEditingModules;

			DdEditingRooms = config.DdEditingRooms;

			DdEditingArtifacts = config.DdEditingArtifacts;

			DdEditingEffects = config.DdEditingEffects;

			DdEditingMonsters = config.DdEditingMonsters;

			DdEditingHints = config.DdEditingHints;

			DdEditingTriggers = config.DdEditingTriggers;

			DdEditingScripts = config.DdEditingScripts;

		Cleanup:

			return rc;
		}

		#endregion

		#region Class Config

		public Config()
		{
			DdFilesetFileName = "";

			DdCharacterFileName = "";

			DdModuleFileName = "";

			DdRoomFileName = "";

			DdArtifactFileName = "";

			DdEffectFileName = "";

			DdMonsterFileName = "";

			DdHintFileName = "";

			DdTriggerFileName = "";

			DdScriptFileName = "";

			MhWorkDir = "";

			MhFilesetFileName = "";

			MhCharacterFileName = "";

			MhEffectFileName = "";

			RtFilesetFileName = "";

			RtCharacterFileName = "";

			RtModuleFileName = "";

			RtRoomFileName = "";

			RtArtifactFileName = "";

			RtEffectFileName = "";

			RtMonsterFileName = "";

			RtHintFileName = "";

			RtTriggerFileName = "";

			RtScriptFileName = "";

			RtGameStateFileName = "";
		}

		#endregion

		#endregion
	}
}
