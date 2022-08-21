
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
using static EamonRT.Game.Plugin.PluginContext;
using static EamonRT.Game.Plugin.PluginContextStack;

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
		public virtual Type ConstantsType { get; set; } = typeof(Game.Plugin.PluginConstants);

		/// <summary></summary>
		public virtual Type ClassMappingsType { get; set; } = typeof(Game.Plugin.PluginClassMappings);

		/// <summary></summary>
		public virtual Type GlobalsType { get; set; } = typeof(Game.Plugin.PluginGlobals);

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

			Globals.Config.Uid = 1;

			Globals.Config.ShowDesc = true;

			Globals.Config.GenerateUids = true;

			Globals.Config.FieldDesc = FieldDesc.Full;

			Globals.Config.WordWrapMargin = Constants.RightMargin;

			// change window title bar and size

			gOut.SetWindowTitle(ProgramName);

			try
			{
				gOut.SetWindowSize(Math.Min(Constants.WindowWidth, gOut.GetLargestWindowWidth()),
													Math.Min(Math.Max(Constants.WindowHeight, gOut.GetWindowHeight()), (long)(gOut.GetLargestWindowHeight() * 0.95)));

				gOut.SetBufferSize(Constants.BufferWidth, Constants.BufferHeight);
			}
			catch (Exception)
			{
				// do nothing
			}

			// make announcements

			gOut.Write("{0}Eamon CS Dungeon Designer ({1}) {2}.", Environment.NewLine, ProgramName, Constants.DdProgVersion);

			gOut.Write("{0}Copyright (c) 2014+ by Michael Penner.  All rights reserved.", Environment.NewLine);

			gOut.Print("This MIT Licensed free software has ABSOLUTELY NO WARRANTY.");

			// copy and store command line args

			Globals.Argv = new string[args.Length];

			for (i = 0; i < args.Length; i++)
			{
				Globals.Argv[i] = Globals.CloneInstance(args[i]);
			}

			// process command line args

			gEngine.DdProcessArgv(false, ref _ddfnFlag, ref _nlFlag);

			// initialize Config record

			Globals.Config.DdFilesetFileName = "FILESETS.DAT";

			Globals.Config.DdCharacterFileName = "CHARACTERS.DAT";

			Globals.Config.DdModuleFileName = "MODULE.DAT";

			Globals.Config.DdRoomFileName = "ROOMS.DAT";

			Globals.Config.DdArtifactFileName = "ARTIFACTS.DAT";

			Globals.Config.DdEffectFileName = "EFFECTS.DAT";

			Globals.Config.DdMonsterFileName = "MONSTERS.DAT";

			Globals.Config.DdHintFileName = "HINTS.DAT";

			Globals.Config.DdTriggerFileName = "TRIGGERS.DAT";

			Globals.Config.DdScriptFileName = "SCRIPTS.DAT";

			if (Globals.WorkDir.Length > 0)
			{
				// if working directory does not exist

				if (!Globals.Directory.Exists(Globals.WorkDir))
				{
					gOut.Print("{0}", Globals.LineSep);

					gOut.Print("The working directory [{0}] does not exist.", Globals.WorkDir);

					gOut.Write("{0}Would you like to create it (Y/N) [N]: ", Environment.NewLine);

					Globals.Buf.Clear();

					rc = Globals.In.ReadField(Globals.Buf, Constants.BufSize02, null, ' ', '\0', true, "N", gEngine.ModifyCharToUpper, gEngine.IsCharYOrN, gEngine.IsCharYOrN);

					Debug.Assert(gEngine.IsSuccess(rc));

					Globals.Thread.Sleep(150);

					if (Globals.Buf[0] != 'Y')
					{
						_nlFlag = false;

						goto Cleanup;
					}

					// create working directory

					Globals.Directory.CreateDirectory(Globals.WorkDir);
				}

				// change to working directory

				Globals.Directory.SetCurrentDirectory(Globals.WorkDir);
			}

			// load the config datafile

			if (Globals.ConfigFileName.Length > 0)
			{
				gOut.Print("{0}", Globals.LineSep);

				rc = Globals.Database.LoadConfigs(Globals.ConfigFileName);

				if (gEngine.IsFailure(rc))
				{
					Globals.Error.Write("Error: LoadConfigs function call failed.");

					goto Cleanup;
				}

				config = gEngine.GetConfig();

				if (config != null)
				{
					if (config.DdFilesetFileName.Length == 0)
					{
						config.DdFilesetFileName = Globals.Config.DdFilesetFileName;

						Globals.ConfigsModified = true;
					}

					if (config.DdCharacterFileName.Length == 0)
					{
						config.DdCharacterFileName = Globals.Config.DdCharacterFileName;

						Globals.ConfigsModified = true;
					}

					if (config.DdModuleFileName.Length == 0)
					{
						config.DdModuleFileName = Globals.Config.DdModuleFileName;

						Globals.ConfigsModified = true;
					}

					if (config.DdRoomFileName.Length == 0)
					{
						config.DdRoomFileName = Globals.Config.DdRoomFileName;

						Globals.ConfigsModified = true;
					}

					if (config.DdArtifactFileName.Length == 0)
					{
						config.DdArtifactFileName = Globals.Config.DdArtifactFileName;

						Globals.ConfigsModified = true;
					}

					if (config.DdEffectFileName.Length == 0)
					{
						config.DdEffectFileName = Globals.Config.DdEffectFileName;

						Globals.ConfigsModified = true;
					}

					if (config.DdMonsterFileName.Length == 0)
					{
						config.DdMonsterFileName = Globals.Config.DdMonsterFileName;

						Globals.ConfigsModified = true;
					}

					if (config.DdHintFileName.Length == 0)
					{
						config.DdHintFileName = Globals.Config.DdHintFileName;

						Globals.ConfigsModified = true;
					}

					if (config.DdTriggerFileName.Length == 0)
					{
						config.DdTriggerFileName = Globals.Config.DdTriggerFileName;

						Globals.ConfigsModified = true;
					}

					if (config.DdScriptFileName.Length == 0)
					{
						config.DdScriptFileName = Globals.Config.DdScriptFileName;

						Globals.ConfigsModified = true;
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

						config.DdEditingTriggers = false;

						config.DdEditingScripts = false;

						Globals.ConfigsModified = true;
					}
				}
				else
				{
					Globals.Config.Uid = Globals.Database.GetConfigUid();

					Globals.Config.IsUidRecycled = true;

					rc = Globals.Database.AddConfig(Globals.Config);

					if (gEngine.IsFailure(rc))
					{
						// PrintError

						goto Cleanup;
					}

					Globals.ConfigsModified = true;

					config = Globals.Config;
				}

				Globals.Config = config;

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

			if (Globals.Config.DdEditingFilesets || Globals.Config.DdEditingCharacters || Globals.Config.DdEditingModules || Globals.Config.DdEditingRooms || Globals.Config.DdEditingArtifacts || Globals.Config.DdEditingEffects || Globals.Config.DdEditingMonsters || Globals.Config.DdEditingHints || Globals.Config.DdEditingTriggers || Globals.Config.DdEditingScripts)
			{
				gOut.Print("{0}", Globals.LineSep);
			}

			if (Globals.Config.DdEditingFilesets)
			{
				rc = Globals.Database.LoadFilesets(Globals.Config.DdFilesetFileName);

				if (gEngine.IsFailure(rc))
				{
					Globals.Error.Write("Error: LoadFilesets function call failed.");

					goto Cleanup;
				}
			}

			if (Globals.Config.DdEditingCharacters)
			{
				rc = Globals.Database.LoadCharacters(Globals.Config.DdCharacterFileName);

				if (gEngine.IsFailure(rc))
				{
					Globals.Error.Write("Error: LoadCharacters function call failed.");

					goto Cleanup;
				}
			}

			if (Globals.Config.DdEditingModules)
			{
				rc = Globals.Database.LoadModules(Globals.Config.DdModuleFileName);

				if (gEngine.IsFailure(rc))
				{
					Globals.Error.Write("Error: LoadModules function call failed.");

					goto Cleanup;
				}
			}

			if (Globals.Config.DdEditingRooms)
			{
				rc = Globals.Database.LoadRooms(Globals.Config.DdRoomFileName);

				if (gEngine.IsFailure(rc))
				{
					Globals.Error.Write("Error: LoadRooms function call failed.");

					goto Cleanup;
				}
			}

			if (Globals.Config.DdEditingArtifacts)
			{
				rc = Globals.Database.LoadArtifacts(Globals.Config.DdArtifactFileName);

				if (gEngine.IsFailure(rc))
				{
					Globals.Error.Write("Error: LoadArtifacts function call failed.");

					goto Cleanup;
				}
			}

			if (Globals.Config.DdEditingEffects)
			{
				rc = Globals.Database.LoadEffects(Globals.Config.DdEffectFileName);

				if (gEngine.IsFailure(rc))
				{
					Globals.Error.Write("Error: LoadEffects function call failed.");

					goto Cleanup;
				}
			}

			if (Globals.Config.DdEditingMonsters)
			{
				rc = Globals.Database.LoadMonsters(Globals.Config.DdMonsterFileName);

				if (gEngine.IsFailure(rc))
				{
					Globals.Error.Write("Error: LoadMonsters function call failed.");

					goto Cleanup;
				}
			}

			if (Globals.Config.DdEditingHints)
			{
				rc = Globals.Database.LoadHints(Globals.Config.DdHintFileName);

				if (gEngine.IsFailure(rc))
				{
					Globals.Error.Write("Error: LoadHints function call failed.");

					goto Cleanup;
				}
			}

			if (Globals.Config.DdEditingTriggers)
			{
				rc = Globals.Database.LoadTriggers(Globals.Config.DdTriggerFileName);

				if (gEngine.IsFailure(rc))
				{
					Globals.Error.Write("Error: LoadTriggers function call failed.");

					goto Cleanup;
				}
			}

			if (Globals.Config.DdEditingScripts)
			{
				rc = Globals.Database.LoadScripts(Globals.Config.DdScriptFileName);

				if (gEngine.IsFailure(rc))
				{
					Globals.Error.Write("Error: LoadScripts function call failed.");

					goto Cleanup;
				}
			}

			if (Globals.Config.DdEditingModules)
			{
				// find the first Module record

				Globals.Module = gEngine.GetModule();

				if (Globals.Module != null)
				{
					if (Globals.Config.DdEditingRooms)
					{
						if (Globals.Module.NumDirs == 6)
						{
							var lastDv = EnumUtil.GetLastValue<Direction>();

							foreach (var room in Globals.Database.RoomTable.Records)
							{
								for (var dv = Direction.Northeast; dv <= lastDv; dv++)
								{
									i = (long)dv;

									if (room.GetDirs(i) != 0)
									{
										room.SetDirs(i, 0);

										Globals.RoomsModified = true;
									}
								}
							}
						}

						if (Globals.Module.NumRooms != Globals.Database.GetRoomsCount())
						{
							Globals.Module.NumRooms = Globals.Database.GetRoomsCount();

							Globals.ModulesModified = true;
						}
					}

					if (Globals.Config.DdEditingArtifacts && Globals.Module.NumArtifacts != Globals.Database.GetArtifactsCount())
					{
						Globals.Module.NumArtifacts = Globals.Database.GetArtifactsCount();

						Globals.ModulesModified = true;
					}

					if (Globals.Config.DdEditingEffects && Globals.Module.NumEffects != Globals.Database.GetEffectsCount())
					{
						Globals.Module.NumEffects = Globals.Database.GetEffectsCount();

						Globals.ModulesModified = true;
					}

					if (Globals.Config.DdEditingMonsters && Globals.Module.NumMonsters != Globals.Database.GetMonstersCount())
					{
						Globals.Module.NumMonsters = Globals.Database.GetMonstersCount();

						Globals.ModulesModified = true;
					}

					if (Globals.Config.DdEditingHints && Globals.Module.NumHints != Globals.Database.GetHintsCount())
					{
						Globals.Module.NumHints = Globals.Database.GetHintsCount();

						Globals.ModulesModified = true;
					}

					if (Globals.Config.DdEditingTriggers && Globals.Module.NumTriggers != Globals.Database.GetTriggersCount())
					{
						Globals.Module.NumTriggers = Globals.Database.GetTriggersCount();

						Globals.ModulesModified = true;
					}

					if (Globals.Config.DdEditingScripts && Globals.Module.NumScripts != Globals.Database.GetScriptsCount())
					{
						Globals.Module.NumScripts = Globals.Database.GetScriptsCount();

						Globals.ModulesModified = true;
					}
				}
			}

			if (Globals.ConfigFileName.Length > 0 || Globals.Config.DdEditingFilesets || Globals.Config.DdEditingCharacters || Globals.Config.DdEditingModules || Globals.Config.DdEditingRooms || Globals.Config.DdEditingArtifacts || Globals.Config.DdEditingEffects || Globals.Config.DdEditingMonsters || Globals.Config.DdEditingHints || Globals.Config.DdEditingTriggers || Globals.Config.DdEditingScripts)
			{
				gOut.WriteLine();
			}

			// create main menu

			Globals.Menu = Globals.CreateInstance<IMainMenu>();

			// call main menu

			Globals.Menu.Execute();

			// update module last modified time if necessary

			if (Globals.ModulesModified || Globals.RoomsModified || Globals.ArtifactsModified || Globals.EffectsModified || Globals.MonstersModified || Globals.HintsModified || Globals.TriggersModified || Globals.ScriptsModified)
			{
				if (Globals.Module != null)
				{
					Globals.Module.LastMod = DateTime.Now;

					Globals.ModulesModified = true;
				}
			}

			// prompt user to save datafiles, if any modifications were made

			if ((Globals.ConfigFileName.Length > 0 && Globals.ConfigsModified) || Globals.FilesetsModified || Globals.CharactersModified || Globals.ModulesModified || Globals.RoomsModified || Globals.ArtifactsModified || Globals.EffectsModified || Globals.MonstersModified || Globals.HintsModified || Globals.TriggersModified || Globals.ScriptsModified)
			{
				gOut.Print("{0}", Globals.LineSep);

				gOut.Print("You have made changes to the in-memory contents of one or more datafiles.");

				gOut.Write("{0}Would you like to save these modifications (Y/N): ", Environment.NewLine);

				Globals.Buf.Clear();

				rc = Globals.In.ReadField(Globals.Buf, Constants.BufSize02, null, ' ', '\0', false, null, gEngine.ModifyCharToUpper, gEngine.IsCharYOrN, gEngine.IsCharYOrN);

				Debug.Assert(gEngine.IsSuccess(rc));

				Globals.Thread.Sleep(150);

				if (Globals.Buf.Length > 0 && Globals.Buf[0] == 'N')
				{
					_nlFlag = false;

					goto Cleanup;
				}

				gOut.Print("{0}", Globals.LineSep);

				// save the datafiles

				if (Globals.ScriptsModified)
				{
					rc = Globals.Database.SaveScripts(Globals.Config.DdScriptFileName);

					if (gEngine.IsFailure(rc))
					{
						Globals.Error.Write("Error: SaveScripts function call failed.");

						rc = RetCode.Success;

						// goto Cleanup omitted
					}
				}

				if (Globals.TriggersModified)
				{
					rc = Globals.Database.SaveTriggers(Globals.Config.DdTriggerFileName);

					if (gEngine.IsFailure(rc))
					{
						Globals.Error.Write("Error: SaveTriggers function call failed.");

						rc = RetCode.Success;

						// goto Cleanup omitted
					}
				}

				if (Globals.HintsModified)
				{
					rc = Globals.Database.SaveHints(Globals.Config.DdHintFileName);

					if (gEngine.IsFailure(rc))
					{
						Globals.Error.Write("Error: SaveHints function call failed.");

						rc = RetCode.Success;

						// goto Cleanup omitted
					}
				}

				if (Globals.MonstersModified)
				{
					rc = Globals.Database.SaveMonsters(Globals.Config.DdMonsterFileName);

					if (gEngine.IsFailure(rc))
					{
						Globals.Error.Write("Error: SaveMonsters function call failed.");

						rc = RetCode.Success;

						// goto Cleanup omitted
					}
				}

				if (Globals.EffectsModified)
				{
					rc = Globals.Database.SaveEffects(Globals.Config.DdEffectFileName);

					if (gEngine.IsFailure(rc))
					{
						Globals.Error.Write("Error: SaveEffects function call failed.");

						rc = RetCode.Success;

						// goto Cleanup omitted
					}
				}

				if (Globals.ArtifactsModified)
				{
					rc = Globals.Database.SaveArtifacts(Globals.Config.DdArtifactFileName);

					if (gEngine.IsFailure(rc))
					{
						Globals.Error.Write("Error: SaveArtifacts function call failed.");

						rc = RetCode.Success;

						// goto Cleanup omitted
					}
				}

				if (Globals.RoomsModified)
				{
					rc = Globals.Database.SaveRooms(Globals.Config.DdRoomFileName);

					if (gEngine.IsFailure(rc))
					{
						Globals.Error.Write("Error: SaveRooms function call failed.");

						rc = RetCode.Success;

						// goto Cleanup omitted
					}
				}

				if (Globals.ModulesModified)
				{
					rc = Globals.Database.SaveModules(Globals.Config.DdModuleFileName);

					if (gEngine.IsFailure(rc))
					{
						Globals.Error.Write("Error: SaveModules function call failed.");

						rc = RetCode.Success;

						// goto Cleanup omitted
					}
				}

				if (Globals.CharactersModified)
				{
					rc = Globals.Database.SaveCharacters(Globals.Config.DdCharacterFileName);

					if (gEngine.IsFailure(rc))
					{
						Globals.Error.Write("Error: SaveCharacters function call failed.");

						rc = RetCode.Success;

						// goto Cleanup omitted
					}
				}

				if (Globals.FilesetsModified)
				{
					rc = Globals.Database.SaveFilesets(Globals.Config.DdFilesetFileName);

					if (gEngine.IsFailure(rc))
					{
						Globals.Error.Write("Error: SaveFilesets function call failed.");

						rc = RetCode.Success;

						// goto Cleanup omitted
					}
				}

				if (Globals.ConfigFileName.Length > 0 && Globals.ConfigsModified)
				{
					rc = Globals.Database.SaveConfigs(Globals.ConfigFileName);

					if (gEngine.IsFailure(rc))
					{
						Globals.Error.Write("Error: SaveConfigs function call failed.");

						rc = RetCode.Success;

						// goto Cleanup omitted
					}
				}

				Globals.Thread.Sleep(150);
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

			Globals.Config.Uid = 1;

			Globals.Config.ShowDesc = true;

			Globals.Config.GenerateUids = true;

			Globals.Config.FieldDesc = FieldDesc.Full;

			Globals.Config.WordWrapMargin = Constants.RightMargin;

			// change window title bar and size

			gOut.SetWindowTitle(ProgramName);

			try
			{
				gOut.SetWindowSize(Math.Min(Constants.WindowWidth, gOut.GetLargestWindowWidth()),
													Math.Min(Math.Max(Constants.WindowHeight, gOut.GetWindowHeight()), (long)(gOut.GetLargestWindowHeight() * 0.95)));

				gOut.SetBufferSize(Constants.BufferWidth, Constants.BufferHeight);
			}
			catch (Exception)
			{
				// do nothing
			}

			// set punctuation space code

			SetPunctSpaceCode();

			// make announcements

			gOut.Write("{0}Eamon CS Runtime ({1}) {2}.", Environment.NewLine, ProgramName, Constants.RtProgVersion);

			gOut.Write("{0}Copyright (c) 2014+ by Michael Penner.  All rights reserved.", Environment.NewLine);

			gOut.Print("This MIT Licensed free software has ABSOLUTELY NO WARRANTY.");

			// copy and store command line args

			Globals.Argv = new string[args.Length];

			for (i = 0; i < args.Length; i++)
			{
				Globals.Argv[i] = Globals.CloneInstance(args[i]);
			}

			// process command line args

			gEngine.RtProcessArgv(false, ref _nlFlag);

			// assign default work directory, if necessary

			if (Globals.WorkDir.Length == 0)
			{
				Globals.WorkDir = Constants.DefaultWorkDir;
			}

			if (Globals.ConfigFileName.Length == 0)
			{
				Globals.ConfigFileName = "EAMONCFG.DAT";
			}

			// initialize Config record

			Globals.Config.RtFilesetFileName = "SAVEGAME.DAT";

			Globals.Config.RtCharacterFileName = "FRESHMEAT.DAT";

			Globals.Config.RtModuleFileName = "MODULE.DAT";

			Globals.Config.RtRoomFileName = "ROOMS.DAT";

			Globals.Config.RtArtifactFileName = "ARTIFACTS.DAT";

			Globals.Config.RtEffectFileName = "EFFECTS.DAT";

			Globals.Config.RtMonsterFileName = "MONSTERS.DAT";

			Globals.Config.RtHintFileName = "HINTS.DAT";

			Globals.Config.RtTriggerFileName = "TRIGGERS.DAT";

			Globals.Config.RtScriptFileName = "SCRIPTS.DAT";

			Globals.Config.RtGameStateFileName = "GAMESTATE.DAT";

			if (Globals.WorkDir.Length > 0)
			{
				// if working directory does not exist

				if (!Globals.Directory.Exists(Globals.WorkDir))
				{
					gOut.Print("{0}", Globals.LineSep);

					gOut.Print("The working directory [{0}] does not exist.", Globals.WorkDir);

					gOut.Write("{0}Would you like to create it (Y/N) [N]: ", Environment.NewLine);

					Globals.Buf.Clear();

					rc = Globals.In.ReadField(Globals.Buf, Constants.BufSize02, null, ' ', '\0', true, "N", gEngine.ModifyCharToUpper, gEngine.IsCharYOrN, gEngine.IsCharYOrN);

					Debug.Assert(gEngine.IsSuccess(rc));

					Globals.Thread.Sleep(150);

					if (Globals.Buf[0] != 'Y')
					{
						_nlFlag = false;

						goto Cleanup;
					}

					// create working directory

					Globals.Directory.CreateDirectory(Globals.WorkDir);
				}

				// change to working directory

				Globals.Directory.SetCurrentDirectory(Globals.WorkDir);
			}

			// load the config datafile

			if (Globals.ConfigFileName.Length > 0)
			{
				gOut.Print("{0}", Globals.LineSep);

				rc = Globals.Database.LoadConfigs(Globals.ConfigFileName);

				if (gEngine.IsFailure(rc))
				{
					Globals.Error.Write("Error: LoadConfigs function call failed.");

					goto Cleanup;
				}

				config = gEngine.GetConfig();

				if (config != null)
				{
					config = Globals.Database.RemoveConfig(config.Uid);

					Debug.Assert(config != null);

					var config01 = Globals.CreateInstance<IConfig>();

					Debug.Assert(config01 != null);

					rc = config01.CopyProperties(config);

					Debug.Assert(gEngine.IsSuccess(rc));

					// config.Dispose() omitted (Uid still in use)

					config = config01;

					rc = Globals.Database.AddConfig(config);

					Debug.Assert(gEngine.IsSuccess(rc));

					if (config.RtFilesetFileName.Length == 0)
					{
						config.RtFilesetFileName = Globals.Config.RtFilesetFileName;

						Globals.ConfigsModified = true;
					}

					if (config.RtCharacterFileName.Length == 0)
					{
						config.RtCharacterFileName = Globals.Config.RtCharacterFileName;

						Globals.ConfigsModified = true;
					}

					if (config.RtModuleFileName.Length == 0)
					{
						config.RtModuleFileName = Globals.Config.RtModuleFileName;

						Globals.ConfigsModified = true;
					}

					if (config.RtRoomFileName.Length == 0)
					{
						config.RtRoomFileName = Globals.Config.RtRoomFileName;

						Globals.ConfigsModified = true;
					}

					if (config.RtArtifactFileName.Length == 0)
					{
						config.RtArtifactFileName = Globals.Config.RtArtifactFileName;

						Globals.ConfigsModified = true;
					}

					if (config.RtEffectFileName.Length == 0)
					{
						config.RtEffectFileName = Globals.Config.RtEffectFileName;

						Globals.ConfigsModified = true;
					}

					if (config.RtMonsterFileName.Length == 0)
					{
						config.RtMonsterFileName = Globals.Config.RtMonsterFileName;

						Globals.ConfigsModified = true;
					}

					if (config.RtHintFileName.Length == 0)
					{
						config.RtHintFileName = Globals.Config.RtHintFileName;

						Globals.ConfigsModified = true;
					}

					if (config.RtTriggerFileName.Length == 0)
					{
						config.RtTriggerFileName = Globals.Config.RtTriggerFileName;

						Globals.ConfigsModified = true;
					}

					if (config.RtScriptFileName.Length == 0)
					{
						config.RtScriptFileName = Globals.Config.RtScriptFileName;

						Globals.ConfigsModified = true;
					}

					if (config.RtGameStateFileName.Length == 0)
					{
						config.RtGameStateFileName = Globals.Config.RtGameStateFileName;

						Globals.ConfigsModified = true;
					}
				}
				else
				{
					Globals.Config.Uid = Globals.Database.GetConfigUid();

					Globals.Config.IsUidRecycled = true;

					rc = Globals.Database.AddConfig(Globals.Config);

					if (gEngine.IsFailure(rc))
					{
						// PrintError

						goto Cleanup;
					}

					Globals.ConfigsModified = true;

					config = Globals.Config;
				}

				Globals.Config = config;

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

			gOut.Print("{0}", Globals.LineSep);

			rc = Globals.Config.LoadGameDatabase();

			if (gEngine.IsFailure(rc))
			{
				Globals.Error.Write("Error: LoadGameDatabase function call failed.");

				goto Cleanup;
			}

			if (!Globals.DeleteGameStateFromMainHall)
			{
				gOut.WriteLine();

				character = Globals.Database.CharacterTable.Records.FirstOrDefault();

				if (character != null)
				{
					character = Globals.Database.RemoveCharacter(character.Uid);

					Debug.Assert(character != null);

					var character01 = Globals.CreateInstance<ICharacter>();

					Debug.Assert(character01 != null);

					rc = character01.CopyProperties(character);

					Debug.Assert(gEngine.IsSuccess(rc));

					// character.Dispose() omitted (Uid still in use)

					character = character01;

					rc = Globals.Database.AddCharacter(character);

					Debug.Assert(gEngine.IsSuccess(rc));
				}

				Globals.Character = character;

				if (gCharacter == null || gCharacter.Uid <= 0 || gCharacter.Status != Status.Adventuring || string.IsNullOrWhiteSpace(gCharacter.Name) || gCharacter.Name.Equals("NONE", StringComparison.OrdinalIgnoreCase))
				{
					rc = RetCode.InvalidObj;

					Globals.Error.Write(gCharacter == null ? "{0}Error: {1}." : "{0}Error: Assertion failed [{1}].",
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

				Globals.Module = gEngine.GetModule();

				if (Globals.Module == null || Globals.Module.Uid <= 0)
				{
					rc = RetCode.InvalidObj;

					Globals.Error.Write(Globals.Module == null ? "{0}Error: {1}." : "{0}Error: Assertion failed [{1}].",
						Environment.NewLine,
						Globals.Module == null ? "Use EamonDD to define a Module record for this adventure" :
						"Globals.Module.Uid > 0");

					if (Globals.Module == null)
					{
						gEngine.UnlinkOnFailure();
					}

					goto Cleanup;
				}

				Globals.GameState = gEngine.GetGameState();

				if (gGameState == null || gGameState.Uid <= 0)
				{
					Globals.GameState = Globals.CreateInstance<IGameState>(x =>
					{
						x.Uid = Globals.Database.GetGameStateUid();
					});

					Debug.Assert(gGameState != null);

					rc = Globals.Database.AddGameState(gGameState);

					Debug.Assert(gEngine.IsSuccess(rc));
				}

				var room = gRDB[gEngine.StartRoom];

				if (room == null)
				{
					rc = RetCode.InvalidObj;

					Globals.Error.Write("{0}Error: {1}.",
						Environment.NewLine,
						"Use EamonDD to define a start Room record for this adventure");

					gEngine.UnlinkOnFailure();

					goto Cleanup;
				}

				rc = gEngine.ValidateRecordsAfterDatabaseLoaded();

				if (gEngine.IsFailure(rc))
				{
					Globals.Error.Write("Error: ValidateRecordsAfterDatabaseLoaded function call failed.");

					gEngine.UnlinkOnFailure();

					goto Cleanup;
				}

				var printIntroOutput = Globals.IntroStory.ShouldPrintOutput;

				gOut.WriteLine("{0}{1}{0}", Environment.NewLine, Globals.LineSep);

				gEngine.PrintTitle(Globals.Module.Name, false);

				gOut.WriteLine();

				Globals.Buf.SetFormat("By {0}", Globals.Module.Author);

				gEngine.PrintTitle(Globals.Buf.ToString(), false);

				if (printIntroOutput)
				{
					Globals.IntroStory.PrintOutput();
				}

				Globals.In.KeyPress(Globals.Buf);

				if (Globals.MainLoop.ShouldStartup)
				{
					Globals.MainLoop.Startup();
				}

				if (Globals.MainLoop.ShouldExecute)
				{
					Globals.MainLoop.Execute();
				}

				if (Globals.MainLoop.ShouldShutdown)
				{
					Globals.MainLoop.Shutdown();
				}

				if (Globals.ErrorExit)
				{
					rc = RetCode.Failure;

					goto Cleanup;
				}

				if (Globals.DeleteGameStateAfterLoop)
				{
					rc = Globals.Config.DeleteGameState(Globals.ConfigFileName, Globals.StartOver);

					Debug.Assert(gEngine.IsSuccess(rc));
				}

				if (Globals.ExportCharacterGoToMainHall || Globals.DeleteCharacter)
				{
					Globals.Directory.SetCurrentDirectory(Globals.Config.MhWorkDir);

					rc = Globals.PushDatabase();

					Debug.Assert(gEngine.IsSuccess(rc));

					rc = Globals.Database.LoadCharacters(Globals.Config.MhCharacterFileName, printOutput: false);

					Debug.Assert(gEngine.IsSuccess(rc));

					character = Globals.CHRDB[gCharacter.Uid];

					if (character != null && gCharacter.Name.Equals(character.Name, StringComparison.OrdinalIgnoreCase))
					{
						if (Globals.DeleteCharacter)
						{
							Globals.Database.RemoveCharacter(character.Uid);

							character.Dispose();
						}
						else
						{
							if (Globals.ExportCharacter)
							{
								rc = character.CopyProperties(gCharacter);

								Debug.Assert(gEngine.IsSuccess(rc));
							}

							character.Status = (gGameState.Die != 1 ? Status.Alive : Status.Dead);
						}

						rc = Globals.Database.SaveCharacters(Globals.Config.MhCharacterFileName, false);

						Debug.Assert(gEngine.IsSuccess(rc));

						rc = Globals.PopDatabase();

						Debug.Assert(gEngine.IsSuccess(rc));

						character = null;

						if (!Globals.DeleteCharacter)
						{
							gOut.Print("{0}", Globals.LineSep);

							if (gGameState.Die != 1)
							{
								Globals.TransferProtocol.SendCharacterToMainHall(Globals.FilePrefix, Globals.Config.MhFilesetFileName, Globals.Config.MhCharacterFileName, Globals.Config.MhEffectFileName, gCharacter.Name);
							}
							else
							{
								gEngine.PrintMemorialService();

								Globals.In.KeyPress(Globals.Buf);
							}
						}
					}
					else
					{
						rc = Globals.PopDatabase();

						Debug.Assert(gEngine.IsSuccess(rc));
					}
				}
				else if (Globals.StartOver)
				{
					gOut.Print("{0}", Globals.LineSep);

					gEngine.PrintSavedGamesDeleted();

					gEngine.PrintRestartGameUsingResume();

					Globals.In.KeyPress(Globals.Buf);
				}

				_nlFlag = false;
			}
			else
			{
				rc = Globals.Config.DeleteGameState(Globals.ConfigFileName, false);

				if (gEngine.IsFailure(rc))
				{
					Globals.Error.Write("Error: DeleteGameState function call failed.");

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

				PushConstants(ConstantsType);

				PushClassMappings(ClassMappingsType);

				ClassMappings.EnableStdio = EnableStdio;

				ClassMappings.ConvertDatafileToMscorlib = ConvertDatafileToMscorlib;

				ClassMappings.LoadPortabilityClassMappings = LoadPortabilityClassMappings;

				// resolve portability class mappings

				ClassMappings.ResolvePortabilityClassMappings();

				// process command line args

				ClassMappings.ProcessArgv(args);

				// load plugin class mappings

				rc = ClassMappings.LoadPluginClassMappings();

				if (rc != RetCode.Success)
				{
					_nlFlag = false;

					goto Cleanup;
				}

				try
				{
					PushGlobals(GlobalsType);

					// initialize system

					Globals.InitSystem();

					Globals.LineWrapUserInput = LineWrapUserInput;

					// call appropriate program

					rc = Globals.RunGameEditor ? DdMain(args) : RtMain(args);
				}
				catch (Exception)
				{
					throw;
				}
				finally
				{
					// de-initialize system

					Globals.DeinitSystem();

					PopGlobals();
				}

			Cleanup:

				if (_nlFlag)
				{
					if (rc == RetCode.Success)
					{
						ClassMappings.Out.WriteLine();
					}
					else
					{
						ClassMappings.Error.WriteLine();
					}

					_nlFlag = false;
				}

				if (!ClassMappings.DeleteGameStateFromMainHall && rc != RetCode.Success)
				{
					ClassMappings.Error.WriteLine("{0}{1}", Environment.NewLine, new string('-', (int)Constants.RightMargin));

					ClassMappings.Error.Write("{0}Press any key to continue: ", Environment.NewLine);

					ClassMappings.In.ReadKey(true);

					ClassMappings.Error.WriteLine();

					ClassMappings.Thread.Sleep(150);
				}

				return;
			}
			catch (Exception)
			{
				throw;
			}
			finally
			{
				PopClassMappings();

				PopConstants();
			}
		}
	}
}
