
// GoOnAdventureMenu.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
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
		public override void Execute()
		{
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

			var j = gEngine.Database.GetFilesetCount();

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

				var filesets = gEngine.Database.FilesetTable.Records;

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

						gEngine.Thread.Sleep(150);

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

				gEngine.Thread.Sleep(150);

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
					// do nothing
				}

				if (fileset != null)
				{
					if (!gEngine.Directory.Exists(fileset.WorkDir))
					{
						var errorMessage = string.Format("Attempted to access a path [{0}] that is not on the disk.", fileset.WorkDir);

						throw new Exception(errorMessage);
					}

					rc = gEngine.PushDatabase();

					Debug.Assert(gEngine.IsSuccess(rc));

					if (!string.IsNullOrWhiteSpace(fileset.FilesetFileName) && !fileset.FilesetFileName.Equals("NONE", StringComparison.OrdinalIgnoreCase))
					{
						var fsfn = gEngine.Path.Combine(fileset.WorkDir, fileset.FilesetFileName);

						rc = gEngine.Database.LoadFilesets(fsfn, printOutput: false);

						Debug.Assert(gEngine.IsSuccess(rc));

						SelectAdventure(index + 1);
					}
					else
					{
						gOut.Print("{0}", gEngine.LineSep);

						var chrfn = gEngine.Path.Combine(fileset.WorkDir, "FRESHMEAT.DAT");

						rc = gEngine.Database.LoadCharacters(chrfn, printOutput: false);

						Debug.Assert(gEngine.IsSuccess(rc));

						var character = gEngine.Database.CharacterTable.Records.FirstOrDefault();

						if (character != null && character.Uid > 0 && !string.IsNullOrWhiteSpace(character.Name) && !character.Name.Equals("NONE", StringComparison.OrdinalIgnoreCase))
						{
							gOut.Print("{0} is already adventuring there!", character.Name);

							goto Cleanup01;
						}

						gEngine.Database.FreeCharacters();

						gCharacter.Status = Status.Adventuring;

						gEngine.CharactersModified = true;

						character = gEngine.CloneInstance(gCharacter);

						rc = gEngine.Database.AddCharacter(character);

						Debug.Assert(gEngine.IsSuccess(rc));

						rc = gEngine.Database.SaveCharacters(chrfn, false);

						Debug.Assert(gEngine.IsSuccess(rc));

						var fsfn = gEngine.Path.Combine(fileset.WorkDir, "SAVEGAME.DAT");

						rc = gEngine.Database.LoadFilesets(fsfn, printOutput: false);

						Debug.Assert(gEngine.IsSuccess(rc));

						filesets = gEngine.Database.FilesetTable.Records;

						foreach (var fileset01 in filesets)
						{
							fileset01.DeleteFiles(null);
						}

						gEngine.Database.FreeFilesets();

						rc = gEngine.Database.SaveFilesets(fsfn, false);

						Debug.Assert(gEngine.IsSuccess(rc));

						var cfgfn = gEngine.Path.Combine(fileset.WorkDir, "EAMONCFG.DAT");

						rc = gEngine.Database.LoadConfigs(cfgfn, printOutput: false);

						Debug.Assert(gEngine.IsSuccess(rc));

						gEngine.Database.FreeConfigs();

						var config = gEngine.CloneInstance(gEngine.Config);

						config.Uid = gEngine.Database.GetConfigUid();

						config.IsUidRecycled = true;

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

						config.RtEffectFileName = gEngine.CloneInstance(fileset.EffectFileName);

						config.DdEffectFileName = gEngine.CloneInstance(config.RtEffectFileName);

						config.RtMonsterFileName = gEngine.CloneInstance(fileset.MonsterFileName);

						config.DdMonsterFileName = gEngine.CloneInstance(config.RtMonsterFileName);

						config.RtHintFileName = gEngine.CloneInstance(fileset.HintFileName);

						config.DdHintFileName = gEngine.CloneInstance(config.RtHintFileName);

						config.RtGameStateFileName = "GAMESTATE.DAT";

						config.DdEditingFilesets = true;

						config.DdEditingCharacters = true;

						rc = gEngine.Database.AddConfig(config);

						Debug.Assert(gEngine.IsSuccess(rc));

						rc = gEngine.Database.SaveConfigs(cfgfn, false);

						Debug.Assert(gEngine.IsSuccess(rc));

						gOut.Print("You are about to adventure in {0}{1}", fileset.Name, fileset.Name.Length > 0 && Char.IsPunctuation(fileset.Name[fileset.Name.Length - 1]) ? "" : "!");

						IDatabase database = null;

						rc = gEngine.GetDatabase(0, ref database);

						Debug.Assert(gEngine.IsSuccess(rc));

						rc = gEngine.PushDatabase(database);

						Debug.Assert(gEngine.IsSuccess(rc));

						// silently sync characters file with newly created files above

						rc = gEngine.Database.SaveCharacters(gEngine.Config.MhCharacterFileName, false);

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
