
// EamonMH.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Eamon;
using Eamon.Framework;
using Eamon.Framework.Portability;
using Eamon.Framework.Primitive.Enums;
using EamonMH.Framework.Menus.ActionMenus;
using EamonMH.Framework.Menus.HierarchicalMenus;
using static EamonMH.Game.Plugin.ContextStack;
using static EamonMH.Game.Plugin.Globals;

namespace EamonMH
{
	public class Program : IProgram
	{
		public virtual bool EnableStdio { get; set; }

		public virtual bool LineWrapUserInput { get; set; }

		public virtual bool ConvertDatafileToMscorlib { get; set; }

		public virtual Action<IDictionary<Type, Type>> LoadPortabilityClassMappings { get; set; }

		/// <summary></summary>
		public virtual string ProgramName { get; set; } = "EamonMH";

		/// <summary></summary>
		public virtual Type EngineType { get; set; } = typeof(Game.Plugin.Engine);

		public virtual void Main(string[] args)
		{
			IConfig config;
			bool nlFlag;
			RetCode rc;
			long i;

			try
			{
				nlFlag = true;

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
					nlFlag = false;

					goto Cleanup;
				}

				try
				{
					// initialize system

					gEngine.InitSystem();

					gEngine.LineWrapUserInput = LineWrapUserInput;

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
						gOut.SetWindowSize
						(
							Math.Min(gEngine.WindowWidth, gOut.GetLargestWindowWidth()),
							Math.Min(Math.Max(gEngine.WindowHeight, gOut.GetWindowHeight()), (long)(gOut.GetLargestWindowHeight() * 0.95))
						);

						gOut.SetBufferSize(gEngine.BufferWidth, gEngine.BufferHeight);
					}
					catch (Exception)
					{
						// do nothing
					}

					if (gEngine.EnableScreenReaderMode)
					{
						gEngine.Thread.Sleep(1000);
					}

					// make announcements

					gOut.Write("{0}Eamon CS Main Hall ({1}) {2}.", Environment.NewLine, ProgramName, gEngine.ProgVersion);

					gOut.Write("{0}Copyright (c) 2014+ by Michael Penner.  All rights reserved.", Environment.NewLine);

					gOut.Print("This MIT Licensed free software has ABSOLUTELY NO WARRANTY.");

					// copy and store command line args

					gEngine.Argv = new string[args.Length];

					for (i = 0; i < args.Length; i++)
					{
						gEngine.Argv[i] = gEngine.CloneInstance(args[i]);
					}

					// process command line args

					gEngine.MhProcessArgv(false, ref nlFlag);

					// assign default work directory, if necessary

					if (gEngine.WorkDir.Length == 0)
					{
						gEngine.WorkDir = gEngine.DefaultWorkDir;
					}

					// initialize Config record

					gEngine.Config.MhFilesetFileName = "ADVENTURES.DAT";

					gEngine.Config.MhCharacterFileName = "CHARACTERS.DAT";

					gEngine.Config.MhEffectFileName = "SNAPPY.DAT";

					if (gEngine.WorkDir.Length > 0)
					{
						// if working directory does not exist

						if (!gEngine.Directory.Exists(gEngine.WorkDir))
						{
							gOut.Print("{0}", gEngine.LineSep);

							gOut.Print("The working directory [{0}] does not exist.", gEngine.WorkDir);

							gOut.Write("{0}Would you like to create it (Y/N) [{1}N]: ", Environment.NewLine, gEngine.EnableScreenReaderMode ? "Default " : "");

							gEngine.Buf.Clear();

							rc = gEngine.In.ReadField(gEngine.Buf, gEngine.BufSize02, null, ' ', '\0', true, "N", gEngine.ModifyCharToUpper, gEngine.IsCharYOrN, gEngine.IsCharYOrN);

							Debug.Assert(gEngine.IsSuccess(rc));

							gEngine.Thread.Sleep(150);

							if (gEngine.Buf[0] != 'Y')
							{
								nlFlag = false;

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
							if (config.MhFilesetFileName.Length == 0)
							{
								config.MhFilesetFileName = gEngine.Config.MhFilesetFileName;

								gEngine.ConfigsModified = true;
							}

							if (config.MhCharacterFileName.Length == 0)
							{
								config.MhCharacterFileName = gEngine.Config.MhCharacterFileName;

								gEngine.ConfigsModified = true;
							}

							if (config.MhEffectFileName.Length == 0)
							{
								config.MhEffectFileName = gEngine.Config.MhEffectFileName;

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

					nlFlag = false;

					// process command line args

					gEngine.MhProcessArgv(true, ref nlFlag);

					if (nlFlag)
					{
						gOut.WriteLine();
					}

					nlFlag = true;

					gOut.Print("{0}", gEngine.LineSep);

					rc = gEngine.Database.LoadFilesets(gEngine.Config.MhFilesetFileName);

					if (gEngine.IsFailure(rc))
					{
						gEngine.Error.Write("Error: LoadFilesets function call failed.");

						goto Cleanup;
					}

					rc = gEngine.Database.LoadCharacters(gEngine.Config.MhCharacterFileName);

					if (gEngine.IsFailure(rc))
					{
						gEngine.Error.Write("Error: LoadCharacters function call failed.");

						goto Cleanup;
					}

					rc = gEngine.Database.LoadEffects(gEngine.Config.MhEffectFileName);

					if (gEngine.IsFailure(rc))
					{
						gEngine.Error.Write("Error: LoadEffects function call failed.");

						goto Cleanup;
					}

					gOut.WriteLine();

					// auto load character if necessary

					if (gEngine.CharacterName.Length > 0 && !gEngine.CharacterName.Equals("NONE", StringComparison.OrdinalIgnoreCase))
					{
						gEngine.Character = gEngine.Database.CharacterTable.Records.FirstOrDefault(c => c.Name.Equals(gEngine.CharacterName, StringComparison.OrdinalIgnoreCase));

						if (gCharacter == null || gCharacter.Uid <= 0 || gCharacter.Status != Status.Alive)
						{
							gEngine.CharacterName = "";

							gEngine.Character = null;
						}
					}

					// create appropriate menu

					if (gCharacter != null)
					{
						gEngine.Menu = gEngine.CreateInstance<IMainHallMenu>();
					}
					else
					{
						gEngine.Menu = gEngine.CreateInstance<IOuterChamberMenu>();
					}

					// call appropriate menu

					gEngine.Menu.Execute();

					var saveDataFiles = (gEngine.ConfigFileName.Length > 0 && gEngine.ConfigsModified) || gEngine.FilesetsModified || gEngine.CharactersModified || gEngine.EffectsModified;

					// save datafiles, if any modifications were made

					if (saveDataFiles)
					{
						gOut.Print("{0}", gEngine.LineSep);

						// save the datafiles

						if (gEngine.EffectsModified)
						{
							rc = gEngine.Database.SaveEffects(gEngine.Config.MhEffectFileName);

							if (gEngine.IsFailure(rc))
							{
								gEngine.Error.Write("Error: SaveEffects function call failed.");

								rc = RetCode.Success;

								// goto Cleanup omitted
							}
						}

						if (gEngine.CharactersModified)
						{
							rc = gEngine.Database.SaveCharacters(gEngine.Config.MhCharacterFileName);

							if (gEngine.IsFailure(rc))
							{
								gEngine.Error.Write("Error: SaveCharacters function call failed.");

								rc = RetCode.Success;

								// goto Cleanup omitted
							}
						}

						if (gEngine.FilesetsModified)
						{
							rc = gEngine.Database.SaveFilesets(gEngine.Config.MhFilesetFileName);

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

						gOut.WriteLine();
					}

					// send character on adventure if necessary

					if (gEngine.GoOnAdventure)
					{
						Debug.Assert(gCharacter != null);

						Debug.Assert(gEngine.Fileset != null);

						gOut.Print("{0}", gEngine.LineSep);

						gEngine.TransferProtocol.SendCharacterOnAdventure(gEngine.Fileset.WorkDir, gEngine.FilePrefix, gEngine.Fileset.PluginFileName);
					}
					else if (saveDataFiles)
					{
						gEngine.Thread.Sleep(150);
					}

					nlFlag = false;
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

				if (nlFlag)
				{
					if (rc == RetCode.Success)
					{
						gEngine.Out.WriteLine();
					}
					else
					{
						gEngine.Error.WriteLine();
					}

					nlFlag = false;
				}

				if (rc != RetCode.Success)
				{
					gEngine.Error.WriteLine("{0}{1}", Environment.NewLine, gEngine.EnableScreenReaderMode ? "" : new string('-', (int)gEngine.RightMargin));

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
