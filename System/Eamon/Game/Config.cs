
// Config.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using static Eamon.Game.Plugin.Globals;

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

		[FieldName(670)]
		public virtual bool DeleteCharArts { get; set; }

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

		[FieldName(810)]
		public virtual string DdCharArtFileName { get; set; }

		[FieldName(820)]
		public virtual string DdEffectFileName { get; set; }

		[FieldName(840)]
		public virtual string DdMonsterFileName { get; set; }

		[FieldName(860)]
		public virtual string DdHintFileName { get; set; }

		[FieldName(920)]
		public virtual string MhWorkDir { get; set; }

		[FieldName(940)]
		public virtual string MhFilesetFileName { get; set; }

		[FieldName(960)]
		public virtual string MhCharacterFileName { get; set; }

		[FieldName(970)]
		public virtual string MhCharArtFileName { get; set; }

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

		[FieldName(1090)]
		public virtual string RtCharArtFileName { get; set; }

		[FieldName(1100)]
		public virtual string RtEffectFileName { get; set; }

		[FieldName(1120)]
		public virtual string RtMonsterFileName { get; set; }

		[FieldName(1140)]
		public virtual string RtHintFileName { get; set; }

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

		[FieldName(1310)]
		public virtual bool DdEditingCharArts { get; set; }

		[FieldName(1320)]
		public virtual bool DdEditingEffects { get; set; }

		[FieldName(1340)]
		public virtual bool DdEditingMonsters { get; set; }

		[FieldName(1360)]
		public virtual bool DdEditingHints { get; set; }

		#endregion

		#endregion

		#region Public Methods

		#region Interface IDisposable

		public override void Dispose(bool disposing)
		{
			if (disposing)
			{
				// Get rid of managed resources
			}

			if (Uid > 0)
			{
				gDatabase.FreeConfigUid(Uid);

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

			rc = gDatabase.LoadFilesets(RtFilesetFileName, validate, printOutput);

			if (gEngine.IsFailure(rc))
			{
				gEngine.Error.WriteLine("Error: LoadFilesets function call failed.");

				goto Cleanup;
			}

			rc = gDatabase.LoadCharacters(RtCharacterFileName, validate, printOutput);

			if (gEngine.IsFailure(rc))
			{
				gEngine.Error.WriteLine("Error: LoadCharacters function call failed.");

				goto Cleanup;
			}

			gDatabase.ExecuteOnArtifactTable(ArtifactTableType.CharArt, () =>
			{
				rc = gDatabase.LoadArtifacts(RtCharArtFileName, validate, printOutput);
			});

			if (gEngine.IsFailure(rc))
			{
				gEngine.Error.WriteLine("Error: LoadArtifacts function call failed.");

				goto Cleanup;
			}

			rc = gDatabase.LoadModules(RtModuleFileName, validate, printOutput);

			if (gEngine.IsFailure(rc))
			{
				gEngine.Error.WriteLine("Error: LoadModules function call failed.");

				goto Cleanup;
			}

			rc = gDatabase.LoadRooms(RtRoomFileName, validate, printOutput);

			if (gEngine.IsFailure(rc))
			{
				gEngine.Error.WriteLine("Error: LoadRooms function call failed.");

				goto Cleanup;
			}

			rc = gDatabase.LoadArtifacts(RtArtifactFileName, validate, printOutput);

			if (gEngine.IsFailure(rc))
			{
				gEngine.Error.WriteLine("Error: LoadArtifacts function call failed.");

				goto Cleanup;
			}

			rc = gDatabase.LoadEffects(RtEffectFileName, validate, printOutput);

			if (gEngine.IsFailure(rc))
			{
				gEngine.Error.WriteLine("Error: LoadEffects function call failed.");

				goto Cleanup;
			}

			rc = gDatabase.LoadMonsters(RtMonsterFileName, validate, printOutput);

			if (gEngine.IsFailure(rc))
			{
				gEngine.Error.WriteLine("Error: LoadMonsters function call failed.");

				goto Cleanup;
			}

			rc = gDatabase.LoadHints(RtHintFileName, validate, printOutput);

			if (gEngine.IsFailure(rc))
			{
				gEngine.Error.WriteLine("Error: LoadHints function call failed.");

				goto Cleanup;
			}

			rc = gDatabase.LoadGameStates(RtGameStateFileName, validate, printOutput);

			if (gEngine.IsFailure(rc))
			{
				gEngine.Error.WriteLine("Error: LoadGameStates function call failed.");

				goto Cleanup;
			}

		Cleanup:

			return rc;
		}

		public virtual RetCode SaveGameDatabase(bool printOutput = true)
		{
			RetCode rc;

			rc = gDatabase.SaveGameStates(RtGameStateFileName, printOutput);

			if (gEngine.IsFailure(rc))
			{
				gEngine.Error.WriteLine("Error: SaveGameStates function call failed.");

				goto Cleanup;
			}

			rc = gDatabase.SaveHints(RtHintFileName, printOutput);

			if (gEngine.IsFailure(rc))
			{
				gEngine.Error.WriteLine("Error: SaveHints function call failed.");

				goto Cleanup;
			}

			rc = gDatabase.SaveMonsters(RtMonsterFileName, printOutput);

			if (gEngine.IsFailure(rc))
			{
				gEngine.Error.WriteLine("Error: SaveMonsters function call failed.");

				goto Cleanup;
			}

			rc = gDatabase.SaveEffects(RtEffectFileName, printOutput);

			if (gEngine.IsFailure(rc))
			{
				gEngine.Error.WriteLine("Error: SaveEffects function call failed.");

				goto Cleanup;
			}

			rc = gDatabase.SaveArtifacts(RtArtifactFileName, printOutput);

			if (gEngine.IsFailure(rc))
			{
				gEngine.Error.WriteLine("Error: SaveArtifacts function call failed.");

				goto Cleanup;
			}

			rc = gDatabase.SaveRooms(RtRoomFileName, printOutput);

			if (gEngine.IsFailure(rc))
			{
				gEngine.Error.WriteLine("Error: SaveRooms function call failed.");

				goto Cleanup;
			}

			rc = gDatabase.SaveModules(RtModuleFileName, printOutput);

			if (gEngine.IsFailure(rc))
			{
				gEngine.Error.WriteLine("Error: SaveModules function call failed.");

				goto Cleanup;
			}

			gDatabase.ExecuteOnArtifactTable(ArtifactTableType.CharArt, () =>
			{
				rc = gDatabase.SaveArtifacts(RtCharArtFileName, printOutput);
			});

			if (gEngine.IsFailure(rc))
			{
				gEngine.Error.WriteLine("Error: SaveArtifacts function call failed.");

				goto Cleanup;
			}

			rc = gDatabase.SaveCharacters(RtCharacterFileName, printOutput);

			if (gEngine.IsFailure(rc))
			{
				gEngine.Error.WriteLine("Error: SaveCharacters function call failed.");

				goto Cleanup;
			}

			rc = gDatabase.SaveFilesets(RtFilesetFileName, printOutput);

			if (gEngine.IsFailure(rc))
			{
				gEngine.Error.WriteLine("Error: SaveFilesets function call failed.");

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

			foreach (var fs in gDatabase.FilesetTable.Records)
			{
				rc = fs.DeleteFiles(null);

				Debug.Assert(gEngine.IsSuccess(rc));
			}

			if (startOver)
			{
				rc = gDatabase.FreeFilesets();

				Debug.Assert(gEngine.IsSuccess(rc));

				rc = gDatabase.SaveFilesets(RtFilesetFileName, false);

				Debug.Assert(gEngine.IsSuccess(rc));
			}
			else
			{
				try
				{
					gEngine.File.Delete(configFileName);
				}
				catch (Exception ex)
				{
					if (ex != null)
					{
						// Do nothing
					}
				}

				try
				{
					gEngine.File.Delete(RtFilesetFileName);
				}
				catch (Exception ex)
				{
					if (ex != null)
					{
						// Do nothing
					}
				}

				try
				{
					gEngine.File.Delete(RtCharArtFileName);
				}
				catch (Exception ex)
				{
					if (ex != null)
					{
						// Do nothing
					}
				}

				try
				{
					gEngine.File.Delete(RtCharacterFileName);
				}
				catch (Exception ex)
				{
					if (ex != null)
					{
						// Do nothing
					}
				}
			}

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

			DdCharArtFileName = "";

			DdEffectFileName = "";

			DdMonsterFileName = "";

			DdHintFileName = "";

			MhWorkDir = "";

			MhFilesetFileName = "";

			MhCharacterFileName = "";

			MhCharArtFileName = "";

			MhEffectFileName = "";

			RtFilesetFileName = "";

			RtCharacterFileName = "";

			RtModuleFileName = "";

			RtRoomFileName = "";

			RtArtifactFileName = "";

			RtCharArtFileName = "";

			RtEffectFileName = "";

			RtMonsterFileName = "";

			RtHintFileName = "";

			RtGameStateFileName = "";
		}

		#endregion

		#endregion
	}
}
