
// Program.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

/*

Originally based upon Eamon Deluxe's MAINPGM.BAS:

'ÉÍÍÍÍÍÍÍÍÍÍÍ THE MARVELOUS WORLD OF EAMON DELUXE v5.0 ÍÍÍÍÍÍÍÍÍÍÍ»
'º                  by Frank Black Productions                    º
'º                                                                º
'º      Originally based upon the classic Eamon                   º
'º              computerized fantasy role-playing system          º
'º                                                                º
'º      BASE ADVENTURE PROGRAM 5.0 (Build 6618 - 12 MAY 2014)     º
'º                                                                º
'º  [Adventure name]                        Revision: 29 FEB 2012 º
'º    [by Author]                             Update: 12 MAY 2014 º
'º                                                                º
'ÈÍÍÍÍÍÍÍÍÍÍ ALL NON-COMMERCIAL DISTRIBUTION ENCOURAGED ÍÍÍÍÍÍÍÍÍÍ¼

*/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Eamon;
using Eamon.Framework;
using Eamon.Framework.Portability;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Extensions;
using Eamon.Game.Utilities;
using EamonDD.Framework.Menus.HierarchicalMenus;
using static EamonRT.Game.Plugin.ContextStack;
using static EamonRT.Game.Plugin.Globals;

namespace EamonRT
{
	public class Program : IProgram
	{
		/// <summary></summary>
		public bool _ddfnFlag;

		/// <summary></summary>
		public bool _nlFlag;

		public virtual bool EnableStdio { get; set; }

		public virtual bool LineWrapUserInput { get; set; }

		public virtual bool ConvertDatafileToMscorlib { get; set; }

		public virtual Action<IDictionary<Type, Type>> LoadPortabilityClassMappings { get; set; }

		/// <summary></summary>
		public virtual string ProgramName { get; set; } = "EamonRT";

		/// <summary></summary>
		public virtual Type EngineType { get; set; } = typeof(Game.Plugin.Engine);

		public virtual void SetPunctSpaceCode()
		{
			gOut.PunctSpaceCode = PunctSpaceCode.Single;
		}

		/// <summary></summary>
		/// <param name="args"></param>
		/// <returns></returns>
		public virtual RetCode DdMain(string[] args)
		{
			IConfig config;
			RetCode rc;
			long i;

			rc = RetCode.Success;

			ProgramName = "EamonDD";

			// disable resolution of uid macros

			gOut.ResolveUidMacros = false;

			// disable extraneous newline suppression

			gOut.SuppressNewLines = false;

			// make the cursor disappear

			gOut.CursorVisible = false;

			// initialize Config record

			gEngine.Config.Uid = 1;

			gEngine.Config.ShowDesc = true;

			gEngine.Config.GenerateUids = true;

			gEngine.Config.FieldDesc = FieldDesc.Full;

			gEngine.Config.WordWrapMargin = gEngine.RightMargin;

			// change window title bar and size

			gOut.SetWindowTitle(ProgramName);

			try
			{
				gOut.SetWindowSize(Math.Min(gEngine.WindowWidth, gOut.GetLargestWindowWidth()),
													Math.Min(Math.Max(gEngine.WindowHeight, gOut.GetWindowHeight()), (long)(gOut.GetLargestWindowHeight() * 0.95)));

				gOut.SetBufferSize(gEngine.BufferWidth, gEngine.BufferHeight);
			}
			catch (Exception)
			{
				// do nothing
			}

			// make announcements

			gOut.Write("{0}Eamon CS Dungeon Designer ({1}) {2}.", Environment.NewLine, ProgramName, gEngine.DdProgVersion);

			gOut.Write("{0}Copyright (c) 2014+ by Michael Penner.  All rights reserved.", Environment.NewLine);

			gOut.Print("This MIT Licensed free software has ABSOLUTELY NO WARRANTY.");

			// copy and store command line args

			gEngine.Argv = new string[args.Length];

			for (i = 0; i < args.Length; i++)
			{
				gEngine.Argv[i] = gEngine.CloneInstance(args[i]);
			}

			// process command line args

			gEngine.DdProcessArgv(false, ref _ddfnFlag, ref _nlFlag);

			// initialize Config record

			gEngine.Config.DdFilesetFileName = "FILESETS.DAT";

			gEngine.Config.DdCharacterFileName = "CHARACTERS.DAT";

			gEngine.Config.DdModuleFileName = "MODULE.DAT";

			gEngine.Config.DdRoomFileName = "ROOMS.DAT";

			gEngine.Config.DdArtifactFileName = "ARTIFACTS.DAT";

			gEngine.Config.DdEffectFileName = "EFFECTS.DAT";

			gEngine.Config.DdMonsterFileName = "MONSTERS.DAT";

			gEngine.Config.DdHintFileName = "HINTS.DAT";

			if (gEngine.WorkDir.Length > 0)
			{
				// if working directory does not exist

				if (!gEngine.Directory.Exists(gEngine.WorkDir))
				{
					gOut.Print("{0}", gEngine.LineSep);

					gOut.Print("The working directory [{0}] does not exist.", gEngine.WorkDir);

					gOut.Write("{0}Would you like to create it (Y/N) [N]: ", Environment.NewLine);

					gEngine.Buf.Clear();

					rc = gEngine.In.ReadField(gEngine.Buf, gEngine.BufSize02, null, ' ', '\0', true, "N", gEngine.ModifyCharToUpper, gEngine.IsCharYOrN, gEngine.IsCharYOrN);

					Debug.Assert(gEngine.IsSuccess(rc));

					gEngine.Thread.Sleep(150);

					if (gEngine.Buf[0] != 'Y')
					{
						_nlFlag = false;

						goto Cleanup;
					}

					// create working directory

					gEngine.Directory.CreateDirectory(gEngine.WorkDir);
				}

				// change to working directory

				gEngine.Directory.SetCurrentDirectory(gEngine.WorkDir);
			}

			// load the config datafile

			if (gEngine.ConfigFileName.Length > 0)
			{
				gOut.Print("{0}", gEngine.LineSep);

				rc = gEngine.Database.LoadConfigs(gEngine.ConfigFileName);

				if (gEngine.IsFailure(rc))
				{
					gEngine.Error.Write("Error: LoadConfigs function call failed.");

					goto Cleanup;
				}

				config = gEngine.GetConfig();

				if (config != null)
				{
					if (config.DdFilesetFileName.Length == 0)
					{
						config.DdFilesetFileName = gEngine.Config.DdFilesetFileName;

						gEngine.ConfigsModified = true;
					}

					if (config.DdCharacterFileName.Length == 0)
					{
						config.DdCharacterFileName = gEngine.Config.DdCharacterFileName;

						gEngine.ConfigsModified = true;
					}

					if (config.DdModuleFileName.Length == 0)
					{
						config.DdModuleFileName = gEngine.Config.DdModuleFileName;

						gEngine.ConfigsModified = true;
					}

					if (config.DdRoomFileName.Length == 0)
					{
						config.DdRoomFileName = gEngine.Config.DdRoomFileName;

						gEngine.ConfigsModified = true;
					}

					if (config.DdArtifactFileName.Length == 0)
					{
						config.DdArtifactFileName = gEngine.Config.DdArtifactFileName;

						gEngine.ConfigsModified = true;
					}

					if (config.DdEffectFileName.Length == 0)
					{
						config.DdEffectFileName = gEngine.Config.DdEffectFileName;

						gEngine.ConfigsModified = true;
					}

					if (config.DdMonsterFileName.Length == 0)
					{
						config.DdMonsterFileName = gEngine.Config.DdMonsterFileName;

						gEngine.ConfigsModified = true;
					}

					if (config.DdHintFileName.Length == 0)
					{
						config.DdHintFileName = gEngine.Config.DdHintFileName;

						gEngine.ConfigsModified = true;
					}

					if (_ddfnFlag)
					{
						config.DdEditingFilesets = false;

						config.DdEditingCharacters = false;

						config.DdEditingModules = false;

						config.DdEditingRooms = false;

						config.DdEditingArtifacts = false;

						config.DdEditingEffects = false;

						config.DdEditingMonsters = false;

						config.DdEditingHints = false;

						gEngine.ConfigsModified = true;
					}
				}
				else
				{
					gEngine.Config.Uid = gEngine.Database.GetConfigUid();

					gEngine.Config.IsUidRecycled = true;

					rc = gEngine.Database.AddConfig(gEngine.Config);

					if (gEngine.IsFailure(rc))
					{
						// PrintError

						goto Cleanup;
					}

					gEngine.ConfigsModified = true;

					config = gEngine.Config;
				}

				gEngine.Config = config;

				gOut.WriteLine();
			}

			_nlFlag = false;

			// process command line args

			gEngine.DdProcessArgv(true, ref _ddfnFlag, ref _nlFlag);

			if (_nlFlag)
			{
				gOut.WriteLine();
			}

			_nlFlag = true;

			if (gEngine.Config.DdEditingFilesets || gEngine.Config.DdEditingCharacters || gEngine.Config.DdEditingModules || gEngine.Config.DdEditingRooms || gEngine.Config.DdEditingArtifacts || gEngine.Config.DdEditingEffects || gEngine.Config.DdEditingMonsters || gEngine.Config.DdEditingHints)
			{
				gOut.Print("{0}", gEngine.LineSep);
			}

			if (gEngine.Config.DdEditingFilesets)
			{
				rc = gEngine.Database.LoadFilesets(gEngine.Config.DdFilesetFileName);

				if (gEngine.IsFailure(rc))
				{
					gEngine.Error.Write("Error: LoadFilesets function call failed.");

					goto Cleanup;
				}
			}

			if (gEngine.Config.DdEditingCharacters)
			{
				rc = gEngine.Database.LoadCharacters(gEngine.Config.DdCharacterFileName);

				if (gEngine.IsFailure(rc))
				{
					gEngine.Error.Write("Error: LoadCharacters function call failed.");

					goto Cleanup;
				}
			}

			if (gEngine.Config.DdEditingModules)
			{
				rc = gEngine.Database.LoadModules(gEngine.Config.DdModuleFileName);

				if (gEngine.IsFailure(rc))
				{
					gEngine.Error.Write("Error: LoadModules function call failed.");

					goto Cleanup;
				}
			}

			if (gEngine.Config.DdEditingRooms)
			{
				rc = gEngine.Database.LoadRooms(gEngine.Config.DdRoomFileName);

				if (gEngine.IsFailure(rc))
				{
					gEngine.Error.Write("Error: LoadRooms function call failed.");

					goto Cleanup;
				}
			}

			if (gEngine.Config.DdEditingArtifacts)
			{
				rc = gEngine.Database.LoadArtifacts(gEngine.Config.DdArtifactFileName);

				if (gEngine.IsFailure(rc))
				{
					gEngine.Error.Write("Error: LoadArtifacts function call failed.");

					goto Cleanup;
				}
			}

			if (gEngine.Config.DdEditingEffects)
			{
				rc = gEngine.Database.LoadEffects(gEngine.Config.DdEffectFileName);

				if (gEngine.IsFailure(rc))
				{
					gEngine.Error.Write("Error: LoadEffects function call failed.");

					goto Cleanup;
				}
			}

			if (gEngine.Config.DdEditingMonsters)
			{
				rc = gEngine.Database.LoadMonsters(gEngine.Config.DdMonsterFileName);

				if (gEngine.IsFailure(rc))
				{
					gEngine.Error.Write("Error: LoadMonsters function call failed.");

					goto Cleanup;
				}
			}

			if (gEngine.Config.DdEditingHints)
			{
				rc = gEngine.Database.LoadHints(gEngine.Config.DdHintFileName);

				if (gEngine.IsFailure(rc))
				{
					gEngine.Error.Write("Error: LoadHints function call failed.");

					goto Cleanup;
				}
			}

			if (gEngine.Config.DdEditingModules)
			{
				// find the first Module record

				gEngine.Module = gEngine.GetModule();

				if (gEngine.Module != null)
				{
					if (gEngine.Config.DdEditingRooms)
					{
						if (gEngine.Module.NumDirs == 6)
						{
							var lastDv = EnumUtil.GetLastValue<Direction>();

							foreach (var room in gEngine.Database.RoomTable.Records)
							{
								for (var dv = Direction.Northeast; dv <= lastDv; dv++)
								{
									i = (long)dv;

									if (room.GetDir(i) != 0)
									{
										room.SetDir(i, 0);

										gEngine.RoomsModified = true;
									}
								}
							}
						}

						if (gEngine.Module.NumRooms != gEngine.Database.GetRoomCount())
						{
							gEngine.Module.NumRooms = gEngine.Database.GetRoomCount();

							gEngine.ModulesModified = true;
						}
					}

					if (gEngine.Config.DdEditingArtifacts && gEngine.Module.NumArtifacts != gEngine.Database.GetArtifactCount())
					{
						gEngine.Module.NumArtifacts = gEngine.Database.GetArtifactCount();

						gEngine.ModulesModified = true;
					}

					if (gEngine.Config.DdEditingEffects && gEngine.Module.NumEffects != gEngine.Database.GetEffectCount())
					{
						gEngine.Module.NumEffects = gEngine.Database.GetEffectCount();

						gEngine.ModulesModified = true;
					}

					if (gEngine.Config.DdEditingMonsters && gEngine.Module.NumMonsters != gEngine.Database.GetMonsterCount())
					{
						gEngine.Module.NumMonsters = gEngine.Database.GetMonsterCount();

						gEngine.ModulesModified = true;
					}

					if (gEngine.Config.DdEditingHints && gEngine.Module.NumHints != gEngine.Database.GetHintCount())
					{
						gEngine.Module.NumHints = gEngine.Database.GetHintCount();

						gEngine.ModulesModified = true;
					}
				}
			}

			if (gEngine.ConfigFileName.Length > 0 || gEngine.Config.DdEditingFilesets || gEngine.Config.DdEditingCharacters || gEngine.Config.DdEditingModules || gEngine.Config.DdEditingRooms || gEngine.Config.DdEditingArtifacts || gEngine.Config.DdEditingEffects || gEngine.Config.DdEditingMonsters || gEngine.Config.DdEditingHints)
			{
				gOut.WriteLine();
			}

			// create main menu

			gEngine.Menu = gEngine.CreateInstance<IMainMenu>();

			// call main menu

			gEngine.Menu.Execute();

			// update module last modified time if necessary

			if (gEngine.ModulesModified || gEngine.RoomsModified || gEngine.ArtifactsModified || gEngine.EffectsModified || gEngine.MonstersModified || gEngine.HintsModified)
			{
				if (gEngine.Module != null)
				{
					gEngine.Module.LastMod = DateTime.Now;

					gEngine.ModulesModified = true;
				}
			}

			// prompt user to save datafiles, if any modifications were made

			if ((gEngine.ConfigFileName.Length > 0 && gEngine.ConfigsModified) || gEngine.FilesetsModified || gEngine.CharactersModified || gEngine.ModulesModified || gEngine.RoomsModified || gEngine.ArtifactsModified || gEngine.EffectsModified || gEngine.MonstersModified || gEngine.HintsModified)
			{
				gOut.Print("{0}", gEngine.LineSep);

				gOut.Print("You have made changes to the in-memory contents of one or more datafiles.");

				gOut.Write("{0}Would you like to save these modifications (Y/N): ", Environment.NewLine);

				gEngine.Buf.Clear();

				rc = gEngine.In.ReadField(gEngine.Buf, gEngine.BufSize02, null, ' ', '\0', false, null, gEngine.ModifyCharToUpper, gEngine.IsCharYOrN, gEngine.IsCharYOrN);

				Debug.Assert(gEngine.IsSuccess(rc));

				gEngine.Thread.Sleep(150);

				if (gEngine.Buf.Length > 0 && gEngine.Buf[0] == 'N')
				{
					_nlFlag = false;

					goto Cleanup;
				}

				gOut.Print("{0}", gEngine.LineSep);

				// save the datafiles

				if (gEngine.HintsModified)
				{
					rc = gEngine.Database.SaveHints(gEngine.Config.DdHintFileName);

					if (gEngine.IsFailure(rc))
					{
						gEngine.Error.Write("Error: SaveHints function call failed.");

						rc = RetCode.Success;

						// goto Cleanup omitted
					}
				}

				if (gEngine.MonstersModified)
				{
					rc = gEngine.Database.SaveMonsters(gEngine.Config.DdMonsterFileName);

					if (gEngine.IsFailure(rc))
					{
						gEngine.Error.Write("Error: SaveMonsters function call failed.");

						rc = RetCode.Success;

						// goto Cleanup omitted
					}
				}

				if (gEngine.EffectsModified)
				{
					rc = gEngine.Database.SaveEffects(gEngine.Config.DdEffectFileName);

					if (gEngine.IsFailure(rc))
					{
						gEngine.Error.Write("Error: SaveEffects function call failed.");

						rc = RetCode.Success;

						// goto Cleanup omitted
					}
				}

				if (gEngine.ArtifactsModified)
				{
					rc = gEngine.Database.SaveArtifacts(gEngine.Config.DdArtifactFileName);

					if (gEngine.IsFailure(rc))
					{
						gEngine.Error.Write("Error: SaveArtifacts function call failed.");

						rc = RetCode.Success;

						// goto Cleanup omitted
					}
				}

				if (gEngine.RoomsModified)
				{
					rc = gEngine.Database.SaveRooms(gEngine.Config.DdRoomFileName);

					if (gEngine.IsFailure(rc))
					{
						gEngine.Error.Write("Error: SaveRooms function call failed.");

						rc = RetCode.Success;

						// goto Cleanup omitted
					}
				}

				if (gEngine.ModulesModified)
				{
					rc = gEngine.Database.SaveModules(gEngine.Config.DdModuleFileName);

					if (gEngine.IsFailure(rc))
					{
						gEngine.Error.Write("Error: SaveModules function call failed.");

						rc = RetCode.Success;

						// goto Cleanup omitted
					}
				}

				if (gEngine.CharactersModified)
				{
					rc = gEngine.Database.SaveCharacters(gEngine.Config.DdCharacterFileName);

					if (gEngine.IsFailure(rc))
					{
						gEngine.Error.Write("Error: SaveCharacters function call failed.");

						rc = RetCode.Success;

						// goto Cleanup omitted
					}
				}

				if (gEngine.FilesetsModified)
				{
					rc = gEngine.Database.SaveFilesets(gEngine.Config.DdFilesetFileName);

					if (gEngine.IsFailure(rc))
					{
						gEngine.Error.Write("Error: SaveFilesets function call failed.");

						rc = RetCode.Success;

						// goto Cleanup omitted
					}
				}

				if (gEngine.ConfigFileName.Length > 0 && gEngine.ConfigsModified)
				{
					rc = gEngine.Database.SaveConfigs(gEngine.ConfigFileName);

					if (gEngine.IsFailure(rc))
					{
						gEngine.Error.Write("Error: SaveConfigs function call failed.");

						rc = RetCode.Success;

						// goto Cleanup omitted
					}
				}

				gEngine.Thread.Sleep(150);
			}
			else
			{
				_nlFlag = false;
			}

		Cleanup:

			return rc;
		}

		/// <summary></summary>
		/// <param name="args"></param>
		/// <returns></returns>
		public virtual RetCode RtMain(string[] args)
		{
			ICharacter character;
			IConfig config;
			RetCode rc;
			long i;

			rc = RetCode.Success;

			// make the cursor disappear

			gOut.CursorVisible = false;

			// initialize Config record

			gEngine.Config.Uid = 1;

			gEngine.Config.ShowDesc = true;

			gEngine.Config.GenerateUids = true;

			gEngine.Config.FieldDesc = FieldDesc.Full;

			gEngine.Config.WordWrapMargin = gEngine.RightMargin;

			// change window title bar and size

			gOut.SetWindowTitle(ProgramName);

			try
			{
				gOut.SetWindowSize(Math.Min(gEngine.WindowWidth, gOut.GetLargestWindowWidth()),
													Math.Min(Math.Max(gEngine.WindowHeight, gOut.GetWindowHeight()), (long)(gOut.GetLargestWindowHeight() * 0.95)));

				gOut.SetBufferSize(gEngine.BufferWidth, gEngine.BufferHeight);
			}
			catch (Exception)
			{
				// do nothing
			}

			// set punctuation space code

			SetPunctSpaceCode();

			// make announcements

			gOut.Write("{0}Eamon CS Runtime ({1}) {2}.", Environment.NewLine, ProgramName, gEngine.RtProgVersion);

			gOut.Write("{0}Copyright (c) 2014+ by Michael Penner.  All rights reserved.", Environment.NewLine);

			gOut.Print("This MIT Licensed free software has ABSOLUTELY NO WARRANTY.");

			// copy and store command line args

			gEngine.Argv = new string[args.Length];

			for (i = 0; i < args.Length; i++)
			{
				gEngine.Argv[i] = gEngine.CloneInstance(args[i]);
			}

			// process command line args

			gEngine.RtProcessArgv(false, ref _nlFlag);

			// assign default work directory, if necessary

			if (gEngine.WorkDir.Length == 0)
			{
				gEngine.WorkDir = gEngine.DefaultWorkDir;
			}

			if (gEngine.ConfigFileName.Length == 0)
			{
				gEngine.ConfigFileName = "EAMONCFG.DAT";
			}

			// initialize Config record

			gEngine.Config.RtFilesetFileName = "SAVEGAME.DAT";

			gEngine.Config.RtCharacterFileName = "FRESHMEAT.DAT";

			gEngine.Config.RtModuleFileName = "MODULE.DAT";

			gEngine.Config.RtRoomFileName = "ROOMS.DAT";

			gEngine.Config.RtArtifactFileName = "ARTIFACTS.DAT";

			gEngine.Config.RtEffectFileName = "EFFECTS.DAT";

			gEngine.Config.RtMonsterFileName = "MONSTERS.DAT";

			gEngine.Config.RtHintFileName = "HINTS.DAT";

			gEngine.Config.RtGameStateFileName = "GAMESTATE.DAT";

			if (gEngine.WorkDir.Length > 0)
			{
				// if working directory does not exist

				if (!gEngine.Directory.Exists(gEngine.WorkDir))
				{
					gOut.Print("{0}", gEngine.LineSep);

					gOut.Print("The working directory [{0}] does not exist.", gEngine.WorkDir);

					gOut.Write("{0}Would you like to create it (Y/N) [N]: ", Environment.NewLine);

					gEngine.Buf.Clear();

					rc = gEngine.In.ReadField(gEngine.Buf, gEngine.BufSize02, null, ' ', '\0', true, "N", gEngine.ModifyCharToUpper, gEngine.IsCharYOrN, gEngine.IsCharYOrN);

					Debug.Assert(gEngine.IsSuccess(rc));

					gEngine.Thread.Sleep(150);

					if (gEngine.Buf[0] != 'Y')
					{
						_nlFlag = false;

						goto Cleanup;
					}

					// create working directory

					gEngine.Directory.CreateDirectory(gEngine.WorkDir);
				}

				// change to working directory

				gEngine.Directory.SetCurrentDirectory(gEngine.WorkDir);
			}

			// load the config datafile

			if (gEngine.ConfigFileName.Length > 0)
			{
				gOut.Print("{0}", gEngine.LineSep);

				rc = gEngine.Database.LoadConfigs(gEngine.ConfigFileName);

				if (gEngine.IsFailure(rc))
				{
					gEngine.Error.Write("Error: LoadConfigs function call failed.");

					goto Cleanup;
				}

				config = gEngine.GetConfig();

				if (config != null)
				{
					config = gEngine.Database.RemoveConfig(config.Uid);

					Debug.Assert(config != null);

					var config01 = gEngine.CreateInstance<IConfig>();

					Debug.Assert(config01 != null);

					rc = config01.CopyProperties(config);

					Debug.Assert(gEngine.IsSuccess(rc));

					// config.Dispose() omitted (Uid still in use)

					config = config01;

					rc = gEngine.Database.AddConfig(config);

					Debug.Assert(gEngine.IsSuccess(rc));

					if (config.RtFilesetFileName.Length == 0)
					{
						config.RtFilesetFileName = gEngine.Config.RtFilesetFileName;

						gEngine.ConfigsModified = true;
					}

					if (config.RtCharacterFileName.Length == 0)
					{
						config.RtCharacterFileName = gEngine.Config.RtCharacterFileName;

						gEngine.ConfigsModified = true;
					}

					if (config.RtModuleFileName.Length == 0)
					{
						config.RtModuleFileName = gEngine.Config.RtModuleFileName;

						gEngine.ConfigsModified = true;
					}

					if (config.RtRoomFileName.Length == 0)
					{
						config.RtRoomFileName = gEngine.Config.RtRoomFileName;

						gEngine.ConfigsModified = true;
					}

					if (config.RtArtifactFileName.Length == 0)
					{
						config.RtArtifactFileName = gEngine.Config.RtArtifactFileName;

						gEngine.ConfigsModified = true;
					}

					if (config.RtEffectFileName.Length == 0)
					{
						config.RtEffectFileName = gEngine.Config.RtEffectFileName;

						gEngine.ConfigsModified = true;
					}

					if (config.RtMonsterFileName.Length == 0)
					{
						config.RtMonsterFileName = gEngine.Config.RtMonsterFileName;

						gEngine.ConfigsModified = true;
					}

					if (config.RtHintFileName.Length == 0)
					{
						config.RtHintFileName = gEngine.Config.RtHintFileName;

						gEngine.ConfigsModified = true;
					}

					if (config.RtGameStateFileName.Length == 0)
					{
						config.RtGameStateFileName = gEngine.Config.RtGameStateFileName;

						gEngine.ConfigsModified = true;
					}
				}
				else
				{
					gEngine.Config.Uid = gEngine.Database.GetConfigUid();

					gEngine.Config.IsUidRecycled = true;

					rc = gEngine.Database.AddConfig(gEngine.Config);

					if (gEngine.IsFailure(rc))
					{
						// PrintError

						goto Cleanup;
					}

					gEngine.ConfigsModified = true;

					config = gEngine.Config;
				}

				gEngine.Config = config;

				gOut.WriteLine();
			}

			_nlFlag = false;

			// process command line args

			gEngine.RtProcessArgv(true, ref _nlFlag);

			if (_nlFlag)
			{
				gOut.WriteLine();
			}

			_nlFlag = true;

			gOut.Print("{0}", gEngine.LineSep);

			rc = gEngine.Config.LoadGameDatabase();

			if (gEngine.IsFailure(rc))
			{
				gEngine.Error.Write("Error: LoadGameDatabase function call failed.");

				goto Cleanup;
			}

			if (!gEngine.DeleteGameStateFromMainHall)
			{
				gOut.WriteLine();

				character = gEngine.Database.CharacterTable.Records.FirstOrDefault();

				if (character != null)
				{
					character = gEngine.Database.RemoveCharacter(character.Uid);

					Debug.Assert(character != null);

					var character01 = gEngine.CreateInstance<ICharacter>();

					Debug.Assert(character01 != null);

					rc = character01.CopyProperties(character);

					Debug.Assert(gEngine.IsSuccess(rc));

					// character.Dispose() omitted (Uid still in use)

					character = character01;

					rc = gEngine.Database.AddCharacter(character);

					Debug.Assert(gEngine.IsSuccess(rc));
				}

				gEngine.Character = character;

				if (gCharacter == null || gCharacter.Uid <= 0 || gCharacter.Status != Status.Adventuring || string.IsNullOrWhiteSpace(gCharacter.Name) || gCharacter.Name.Equals("NONE", StringComparison.OrdinalIgnoreCase))
				{
					rc = RetCode.InvalidObj;

					gEngine.Error.Write(gCharacter == null ? "{0}Error: {1}." : "{0}Error: Assertion failed [{1}].",
						Environment.NewLine,
						gCharacter == null ? "Use EamonMH to send a character on this adventure" :
						gCharacter.Uid <= 0 ? "gCharacter.Uid > 0" :
						gCharacter.Status != Status.Adventuring ? "gCharacter.Status == Status.Adventuring" :
						string.IsNullOrWhiteSpace(gCharacter.Name) ? "!string.IsNullOrWhiteSpace(gCharacter.Name)" :
						"!gCharacter.Name.Equals(\"NONE\", StringComparison.OrdinalIgnoreCase)");

					if (gCharacter == null)
					{
						gEngine.UnlinkOnFailure();
					}

					goto Cleanup;
				}

				gEngine.Module = gEngine.GetModule();

				if (gEngine.Module == null || gEngine.Module.Uid <= 0)
				{
					rc = RetCode.InvalidObj;

					gEngine.Error.Write(gEngine.Module == null ? "{0}Error: {1}." : "{0}Error: Assertion failed [{1}].",
						Environment.NewLine,
						gEngine.Module == null ? "Use EamonDD to define a Module record for this adventure" :
						"Engine.Module.Uid > 0");

					if (gEngine.Module == null)
					{
						gEngine.UnlinkOnFailure();
					}

					goto Cleanup;
				}

				gEngine.GameState = gEngine.GetGameState();

				if (gGameState == null || gGameState.Uid <= 0)
				{
					gEngine.GameState = gEngine.CreateInstance<IGameState>(x =>
					{
						x.Uid = gEngine.Database.GetGameStateUid();
					});

					Debug.Assert(gGameState != null);

					rc = gEngine.Database.AddGameState(gGameState);

					Debug.Assert(gEngine.IsSuccess(rc));
				}

				var room = gRDB[gEngine.StartRoom];

				if (room == null)
				{
					rc = RetCode.InvalidObj;

					gEngine.Error.Write("{0}Error: {1}.",
						Environment.NewLine,
						"Use EamonDD to define a start Room record for this adventure");

					gEngine.UnlinkOnFailure();

					goto Cleanup;
				}

				rc = gEngine.ValidateRecordsAfterDatabaseLoaded();

				if (gEngine.IsFailure(rc))
				{
					gEngine.Error.Write("Error: ValidateRecordsAfterDatabaseLoaded function call failed.");

					gEngine.UnlinkOnFailure();

					goto Cleanup;
				}

				var printIntroOutput = gEngine.IntroStory.ShouldPrintOutput;

				gOut.WriteLine("{0}{1}{0}", Environment.NewLine, gEngine.LineSep);

				gEngine.PrintTitle(gEngine.Module.Name, false);

				gOut.WriteLine();

				gEngine.Buf.SetFormat("By {0}", gEngine.Module.Author);

				gEngine.PrintTitle(gEngine.Buf.ToString(), false);

				if (printIntroOutput)
				{
					gEngine.IntroStory.PrintOutput();
				}

				gEngine.In.KeyPress(gEngine.Buf);

				if (gEngine.MainLoop.ShouldStartup)
				{
					gEngine.MainLoop.Startup();
				}

				if (gEngine.MainLoop.ShouldExecute)
				{
					gEngine.MainLoop.Execute();
				}

				if (gEngine.MainLoop.ShouldShutdown)
				{
					gEngine.MainLoop.Shutdown();
				}

				if (gEngine.ErrorExit)
				{
					rc = RetCode.Failure;

					goto Cleanup;
				}

				if (gEngine.DeleteGameStateAfterLoop)
				{
					rc = gEngine.Config.DeleteGameState(gEngine.ConfigFileName, gEngine.StartOver);

					Debug.Assert(gEngine.IsSuccess(rc));
				}

				if (gEngine.ExportCharacterGoToMainHall || gEngine.DeleteCharacter)
				{
					gEngine.Directory.SetCurrentDirectory(gEngine.Config.MhWorkDir);

					rc = gEngine.PushDatabase();

					Debug.Assert(gEngine.IsSuccess(rc));

					rc = gEngine.Database.LoadCharacters(gEngine.Config.MhCharacterFileName, printOutput: false);

					Debug.Assert(gEngine.IsSuccess(rc));

					character = gEngine.CHRDB[gCharacter.Uid];

					if (character != null && gCharacter.Name.Equals(character.Name, StringComparison.OrdinalIgnoreCase))
					{
						if (gEngine.DeleteCharacter)
						{
							gEngine.Database.RemoveCharacter(character.Uid);

							character.Dispose();
						}
						else
						{
							if (gEngine.ExportCharacter)
							{
								rc = character.CopyProperties(gCharacter);

								Debug.Assert(gEngine.IsSuccess(rc));
							}

							character.Status = (gGameState.Die != 1 ? Status.Alive : Status.Dead);
						}

						rc = gEngine.Database.SaveCharacters(gEngine.Config.MhCharacterFileName, false);

						Debug.Assert(gEngine.IsSuccess(rc));

						rc = gEngine.PopDatabase();

						Debug.Assert(gEngine.IsSuccess(rc));

						character = null;

						if (!gEngine.DeleteCharacter)
						{
							gOut.Print("{0}", gEngine.LineSep);

							if (gGameState.Die != 1)
							{
								gEngine.TransferProtocol.SendCharacterToMainHall(gEngine.FilePrefix, gEngine.Config.MhFilesetFileName, gEngine.Config.MhCharacterFileName, gEngine.Config.MhEffectFileName, gCharacter.Name);
							}
							else
							{
								gEngine.PrintMemorialService();

								gEngine.In.KeyPress(gEngine.Buf);
							}
						}
					}
					else
					{
						rc = gEngine.PopDatabase();

						Debug.Assert(gEngine.IsSuccess(rc));
					}
				}
				else if (gEngine.StartOver)
				{
					gOut.Print("{0}", gEngine.LineSep);

					gEngine.PrintSavedGamesDeleted();

					gEngine.PrintRestartGameUsingResume();

					gEngine.In.KeyPress(gEngine.Buf);
				}

				_nlFlag = false;
			}
			else
			{
				rc = gEngine.Config.DeleteGameState(gEngine.ConfigFileName, false);

				if (gEngine.IsFailure(rc))
				{
					gEngine.Error.Write("Error: DeleteGameState function call failed.");

					goto Cleanup;
				}
			}

		Cleanup:

			return rc;
		}

		public virtual void Main(string[] args)
		{
			RetCode rc;

			try
			{
				_ddfnFlag = false;

				_nlFlag = true;

				rc = RetCode.Success;

				PushEngine(EngineType);

				gEngine.EnableStdio = EnableStdio;

				gEngine.ConvertDatafileToMscorlib = ConvertDatafileToMscorlib;

				gEngine.LoadPortabilityClassMappings = LoadPortabilityClassMappings;

				// resolve portability class mappings

				gEngine.ResolvePortabilityClassMappings();

				// process command line args

				gEngine.ProcessArgv(args);

				// load plugin class mappings

				rc = gEngine.LoadPluginClassMappings();

				if (rc != RetCode.Success)
				{
					_nlFlag = false;

					goto Cleanup;
				}

				try
				{
					// initialize system

					gEngine.InitSystem();

					gEngine.LineWrapUserInput = LineWrapUserInput;

					// call appropriate program

					rc = gEngine.RunGameEditor ? DdMain(args) : RtMain(args);
				}
				catch (Exception)
				{
					throw;
				}
				finally
				{
					// de-initialize system

					gEngine.DeinitSystem();
				}

			Cleanup:

				if (_nlFlag)
				{
					if (rc == RetCode.Success)
					{
						gEngine.Out.WriteLine();
					}
					else
					{
						gEngine.Error.WriteLine();
					}

					_nlFlag = false;
				}

				if (!gEngine.DeleteGameStateFromMainHall && rc != RetCode.Success)
				{
					gEngine.Error.WriteLine("{0}{1}", Environment.NewLine, new string('-', (int)gEngine.RightMargin));

					gEngine.Error.Write("{0}Press any key to continue: ", Environment.NewLine);

					gEngine.In.ReadKey(true);

					gEngine.Error.WriteLine();

					gEngine.Thread.Sleep(150);
				}

				return;
			}
			catch (Exception)
			{
				throw;
			}
			finally
			{
				PopEngine();
			}
		}
	}
}
