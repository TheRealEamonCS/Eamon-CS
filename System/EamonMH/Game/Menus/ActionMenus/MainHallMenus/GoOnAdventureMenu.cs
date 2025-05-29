
// GoOnAdventureMenu.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Eamon;
using Eamon.Framework;
using Eamon.Framework.DataStorage;
using Eamon.Framework.Helpers;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using Eamon.Game.Menus;
using EamonMH.Framework.Menus.ActionMenus;
using static EamonMH.Game.Plugin.Globals;

namespace EamonMH.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class GoOnAdventureMenu : Menu, IGoOnAdventureMenu
	{
		/// <summary></summary>
		public virtual IList<IArtifact> ArtifactList { get; set; }

		public override void Execute()
		{
			ArtifactList = gCharacter.GetContainedList().OrderBy(a => a.Uid).ToList();

			SelectAdventure(0);
		}

		/// <summary></summary>
		/// <param name="index"></param>
		public virtual void SelectAdventure(long index)
		{
			RetCode rc;

			if (index < 0)
			{
				// PrintError

				goto Cleanup;
			}

			var nlFlag = false;

			var j = gDatabase.GetFilesetCount();

			if (index == 0)
			{
				gOut.Print("{0}", gEngine.LineSep);

				gOut.Print("When you inquire with the burly Irishman about the adventures available to you he says, \"Ye cannat wait to put yerself to the test, eh?  {0}\"",
					j > 0 ? "Well, maybe one of these will suit yer fancy." : "Well, I just don't know where ye can venture right now.");
			}

			if (j == 0)
			{
				if (index == 0)
				{
					gEngine.In.KeyPress(Buf);
				}

				goto Cleanup;
			}

			while (true)
			{
				gOut.Print("{0}", gEngine.LineSep);

				var i = 0;

				var helper = gEngine.CreateInstance<IFilesetHelper>();

				helper.RecordTable = gDatabase.FilesetTable;
				
				var filesets = gDatabase.FilesetTable.Records;

				foreach (var fileset01 in filesets)
				{
					helper.Record = fileset01;

					helper.ListRecord(false, false, false, false, false, false);

					nlFlag = true;

					if (i != 0 && (i % (gEngine.NumRows - 8)) == 0)
					{
						nlFlag = false;

						gOut.WriteLine("{0}{0}{1}", Environment.NewLine, gEngine.LineSep);

						gOut.Write("{0}Press any key to continue or X to exit: ", Environment.NewLine);

						Buf.Clear();

						rc = gEngine.In.ReadField(Buf, gEngine.BufSize02, null, ' ', '\0', true, null, gEngine.ModifyCharToNullOrX, null, gEngine.IsCharAny);

						Debug.Assert(gEngine.IsSuccess(rc));

						if (Buf.Length > 0 && Buf[0] == 'X')
						{
							break;
						}

						if (i + 1 < filesets.Count)
						{
							gOut.Print("{0}", gEngine.LineSep);
						}
					}

					i++;
				}

				if (nlFlag)
				{
					gOut.WriteLine();
				}

				gOut.Print("{0}", gEngine.LineSep);

				gOut.Write("{0}Enter the selection or X to exit: ", Environment.NewLine);

				Buf.Clear();

				rc = gEngine.In.ReadField(Buf, gEngine.BufSize01, null, ' ', '\0', false, null, gEngine.ModifyCharToUpper, gEngine.IsCharDigitOrX, null);

				Debug.Assert(gEngine.IsSuccess(rc));

				if (Buf.Length > 0 && Buf[0] == 'X')
				{
					goto Cleanup;
				}

				IFileset fileset = null;

				try
				{
					var filesetUid = Convert.ToInt64(Buf.Trim().ToString());

					fileset = gEngine.FSDB[filesetUid];
				}
				catch (Exception)
				{
					// Do nothing
				}

				if (fileset != null)
				{
					if (!gEngine.Directory.Exists(fileset.WorkDir))
					{
						var errorMessage = string.Format("Attempted to access a path [{0}] that is not on the disk.", fileset.WorkDir.Replace('\\', gEngine.Path.DirectorySeparatorChar));

						throw new Exception(errorMessage);
					}

					rc = gEngine.PushDatabase();

					Debug.Assert(gEngine.IsSuccess(rc));

					gDatabase.PushArtifactTable(ArtifactTableType.CharArt);

					if (!string.IsNullOrWhiteSpace(fileset.FilesetFileName) && !fileset.FilesetFileName.Equals("NONE", StringComparison.OrdinalIgnoreCase))
					{
						var fsfn = gEngine.Path.Combine(fileset.WorkDir, fileset.FilesetFileName);

						rc = gDatabase.LoadFilesets(fsfn, printOutput: false);

						Debug.Assert(gEngine.IsSuccess(rc));

						SelectAdventure(index + 1);
					}
					else
					{
						gOut.Print("{0}", gEngine.LineSep);

						var chrfn = gEngine.Path.Combine(fileset.WorkDir, "FRESHMEAT.DAT");

						rc = gDatabase.LoadCharacters(chrfn, printOutput: false);

						Debug.Assert(gEngine.IsSuccess(rc));

						var character = gDatabase.CharacterTable.Records.FirstOrDefault();

						if (character != null && character.Uid > 0 && !string.IsNullOrWhiteSpace(character.Name) && !character.Name.Equals("NONE", StringComparison.OrdinalIgnoreCase))
						{
							gOut.Print("{0} is already adventuring there!", character.Name);

							goto Cleanup01;
						}

						gDatabase.FreeCharacters();

						gCharacter.Status = Status.Adventuring;

						gEngine.CharactersModified = true;

						character = gEngine.CloneInstance(gCharacter);

						rc = gDatabase.AddCharacter(character);

						Debug.Assert(gEngine.IsSuccess(rc));

						rc = gDatabase.SaveCharacters(chrfn, false);

						Debug.Assert(gEngine.IsSuccess(rc));

						var cafn = gEngine.Path.Combine(fileset.WorkDir, "FRESHGEAR.DAT");

						foreach (var artifact in ArtifactList)
						{
							rc = gDatabase.AddArtifact(artifact, true);

							Debug.Assert(gEngine.IsSuccess(rc));
						}

						rc = gDatabase.SaveArtifacts(cafn, false);

						Debug.Assert(gEngine.IsSuccess(rc));

						var fsfn = gEngine.Path.Combine(fileset.WorkDir, "SAVEGAME.DAT");

						rc = gDatabase.LoadFilesets(fsfn, printOutput: false);

						Debug.Assert(gEngine.IsSuccess(rc));

						filesets = gDatabase.FilesetTable.Records;

						foreach (var fileset01 in filesets)
						{
							fileset01.DeleteFiles(null);
						}

						gDatabase.FreeFilesets();

						rc = gDatabase.SaveFilesets(fsfn, false);

						Debug.Assert(gEngine.IsSuccess(rc));

						var cfgfn = gEngine.Path.Combine(fileset.WorkDir, "EAMONCFG.DAT");

						rc = gDatabase.LoadConfigs(cfgfn, printOutput: false);

						Debug.Assert(gEngine.IsSuccess(rc));

						gDatabase.FreeConfigs();

						var config = gEngine.CloneInstance(gEngine.Config);

						config.Uid = gDatabase.GetConfigUid();

						config.MhWorkDir = @"..\..\System\Bin";         // config.MhWorkDir = Engine.CloneInstance(Engine.WorkDir);

						config.RtFilesetFileName = "SAVEGAME.DAT";

						config.DdFilesetFileName = gEngine.CloneInstance(config.RtFilesetFileName);

						config.RtCharacterFileName = "FRESHMEAT.DAT";

						config.DdCharacterFileName = gEngine.CloneInstance(config.RtCharacterFileName);

						config.RtModuleFileName = gEngine.CloneInstance(fileset.ModuleFileName);

						config.DdModuleFileName = gEngine.CloneInstance(config.RtModuleFileName);

						config.RtRoomFileName = gEngine.CloneInstance(fileset.RoomFileName);

						config.DdRoomFileName = gEngine.CloneInstance(config.RtRoomFileName);

						config.RtArtifactFileName = gEngine.CloneInstance(fileset.ArtifactFileName);

						config.DdArtifactFileName = gEngine.CloneInstance(config.RtArtifactFileName);

						config.RtCharArtFileName = "FRESHGEAR.DAT";

						config.DdCharArtFileName = gEngine.CloneInstance(config.RtCharArtFileName);

						config.RtEffectFileName = gEngine.CloneInstance(fileset.EffectFileName);

						config.DdEffectFileName = gEngine.CloneInstance(config.RtEffectFileName);

						config.RtMonsterFileName = gEngine.CloneInstance(fileset.MonsterFileName);

						config.DdMonsterFileName = gEngine.CloneInstance(config.RtMonsterFileName);

						config.RtHintFileName = gEngine.CloneInstance(fileset.HintFileName);

						config.DdHintFileName = gEngine.CloneInstance(config.RtHintFileName);

						config.RtGameStateFileName = "GAMESTATE.DAT";

						config.DdEditingFilesets = true;

						config.DdEditingCharacters = true;

						config.DdEditingCharArts = true;

						rc = gDatabase.AddConfig(config);

						Debug.Assert(gEngine.IsSuccess(rc));

						rc = gDatabase.SaveConfigs(cfgfn, false);

						Debug.Assert(gEngine.IsSuccess(rc));

						gOut.Print("You are about to adventure in {0}{1}", fileset.Name, fileset.Name.Length > 0 && Char.IsPunctuation(fileset.Name[fileset.Name.Length - 1]) ? "" : "!");

						IDatabase database = null;

						rc = gEngine.GetDatabase(0, ref database);

						Debug.Assert(gEngine.IsSuccess(rc));

						rc = gEngine.PushDatabase(database);

						Debug.Assert(gEngine.IsSuccess(rc));

						// Silently sync characters file with newly created files above

						rc = gDatabase.SaveCharacters(gEngine.Config.MhCharacterFileName, false);

						Debug.Assert(gEngine.IsSuccess(rc));

						rc = gEngine.PopDatabase(false);

						Debug.Assert(gEngine.IsSuccess(rc));

						gEngine.Fileset = gEngine.CloneInstance(fileset);

						gEngine.GoOnAdventure = true;

						gEngine.In.KeyPress(Buf);

					Cleanup01:

						;
					}

					rc = gEngine.PopDatabase();

					Debug.Assert(gEngine.IsSuccess(rc));

					if (gEngine.Fileset != null)
					{
						goto Cleanup;
					}
				}
				else
				{
					goto Cleanup;
				}
			}

		Cleanup:

			;
		}

		public GoOnAdventureMenu()
		{
			Buf = gEngine.Buf;
		}
	}
}
