
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
using static EamonMH.Game.Plugin.PluginContext;
using static EamonMH.Game.Plugin.PluginContextStack;

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
		public virtual Type ConstantsType { get; set; } = typeof(Game.Plugin.PluginConstants);

		/// <summary></summary>
		public virtual Type ClassMappingsType { get; set; } = typeof(Game.Plugin.PluginClassMappings);

		/// <summary></summary>
		public virtual Type GlobalsType { get; set; } = typeof(Game.Plugin.PluginGlobals);

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
					nlFlag = false;

					goto Cleanup;
				}

				try
				{
					PushGlobals(GlobalsType);

					// initialize system

					Globals.InitSystem();

					Globals.LineWrapUserInput = LineWrapUserInput;

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

					gOut.Write("{0}Eamon CS Main Hall ({1}) {2}", Environment.NewLine, ProgramName, Constants.ProgVersion);

					gOut.Write("{0}Copyright (c) 2014+ by Michael Penner.  All rights reserved.", Environment.NewLine);

					gOut.Print("This MIT Licensed free software has ABSOLUTELY NO WARRANTY.");

					// copy and store command line args

					Globals.Argv = new string[args.Length];

					for (i = 0; i < args.Length; i++)
					{
						Globals.Argv[i] = Globals.CloneInstance(args[i]);
					}

					// process command line args

					gEngine.MhProcessArgv(false, ref nlFlag);

					// assign default work directory, if necessary

					if (Globals.WorkDir.Length == 0)
					{
						Globals.WorkDir = Constants.DefaultWorkDir;
					}

					// initialize Config record

					Globals.Config.MhFilesetFileName = "ADVENTURES.DAT";

					Globals.Config.MhCharacterFileName = "CHARACTERS.DAT";

					Globals.Config.MhEffectFileName = "SNAPPY.DAT";

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
								nlFlag = false;

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
							Globals.Error.Write("Error: LoadConfigs function call failed");

							goto Cleanup;
						}

						config = gEngine.GetConfig();

						if (config != null)
						{
							if (config.MhFilesetFileName.Length == 0)
							{
								config.MhFilesetFileName = Globals.Config.MhFilesetFileName;

								Globals.ConfigsModified = true;
							}

							if (config.MhCharacterFileName.Length == 0)
							{
								config.MhCharacterFileName = Globals.Config.MhCharacterFileName;

								Globals.ConfigsModified = true;
							}

							if (config.MhEffectFileName.Length == 0)
							{
								config.MhEffectFileName = Globals.Config.MhEffectFileName;

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

					nlFlag = false;

					// process command line args

					gEngine.MhProcessArgv(true, ref nlFlag);

					if (nlFlag)
					{
						gOut.WriteLine();
					}

					nlFlag = true;

					gOut.Print("{0}", Globals.LineSep);

					rc = Globals.Database.LoadFilesets(Globals.Config.MhFilesetFileName);

					if (gEngine.IsFailure(rc))
					{
						Globals.Error.Write("Error: LoadFilesets function call failed");

						goto Cleanup;
					}

					rc = Globals.Database.LoadCharacters(Globals.Config.MhCharacterFileName);

					if (gEngine.IsFailure(rc))
					{
						Globals.Error.Write("Error: LoadCharacters function call failed");

						goto Cleanup;
					}

					rc = Globals.Database.LoadEffects(Globals.Config.MhEffectFileName);

					if (gEngine.IsFailure(rc))
					{
						Globals.Error.Write("Error: LoadEffects function call failed");

						goto Cleanup;
					}

					gOut.WriteLine();

					// auto load character if necessary

					if (Globals.CharacterName.Length > 0 && !Globals.CharacterName.Equals("NONE", StringComparison.OrdinalIgnoreCase))
					{
						Globals.Character = Globals.Database.CharacterTable.Records.FirstOrDefault(c => c.Name.Equals(Globals.CharacterName, StringComparison.OrdinalIgnoreCase));

						if (gCharacter == null || gCharacter.Uid <= 0 || gCharacter.Status != Status.Alive)
						{
							Globals.CharacterName = "";

							Globals.Character = null;
						}
					}

					// create appropriate menu

					if (gCharacter != null)
					{
						Globals.Menu = Globals.CreateInstance<IMainHallMenu>();
					}
					else
					{
						Globals.Menu = Globals.CreateInstance<IOuterChamberMenu>();
					}

					// call appropriate menu

					Globals.Menu.Execute();

					var saveDataFiles = (Globals.ConfigFileName.Length > 0 && Globals.ConfigsModified) || Globals.FilesetsModified || Globals.CharactersModified || Globals.EffectsModified;

					// save datafiles, if any modifications were made

					if (saveDataFiles)
					{
						gOut.Print("{0}", Globals.LineSep);

						// save the datafiles

						if (Globals.EffectsModified)
						{
							rc = Globals.Database.SaveEffects(Globals.Config.MhEffectFileName);

							if (gEngine.IsFailure(rc))
							{
								Globals.Error.Write("Error: SaveEffects function call failed");

								rc = RetCode.Success;

								// goto Cleanup omitted
							}
						}

						if (Globals.CharactersModified)
						{
							rc = Globals.Database.SaveCharacters(Globals.Config.MhCharacterFileName);

							if (gEngine.IsFailure(rc))
							{
								Globals.Error.Write("Error: SaveCharacters function call failed");

								rc = RetCode.Success;

								// goto Cleanup omitted
							}
						}

						if (Globals.FilesetsModified)
						{
							rc = Globals.Database.SaveFilesets(Globals.Config.MhFilesetFileName);

							if (gEngine.IsFailure(rc))
							{
								Globals.Error.Write("Error: SaveFilesets function call failed");

								rc = RetCode.Success;

								// goto Cleanup omitted
							}
						}

						if (Globals.ConfigFileName.Length > 0 && Globals.ConfigsModified)
						{
							rc = Globals.Database.SaveConfigs(Globals.ConfigFileName);

							if (gEngine.IsFailure(rc))
							{
								Globals.Error.Write("Error: SaveConfigs function call failed");

								rc = RetCode.Success;

								// goto Cleanup omitted
							}
						}

						gOut.WriteLine();
					}

					// send character on adventure if necessary

					if (Globals.GoOnAdventure)
					{
						Debug.Assert(gCharacter != null);

						Debug.Assert(Globals.Fileset != null);

						gOut.Print("{0}", Globals.LineSep);

						Globals.TransferProtocol.SendCharacterOnAdventure(Globals.Fileset.WorkDir, Globals.FilePrefix, Globals.Fileset.PluginFileName);
					}
					else if (saveDataFiles)
					{
						Globals.Thread.Sleep(150);
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

					Globals.DeinitSystem();

					PopGlobals();
				}

			Cleanup:

				if (nlFlag)
				{
					if (rc == RetCode.Success)
					{
						ClassMappings.Out.WriteLine();
					}
					else
					{
						ClassMappings.Error.WriteLine();
					}

					nlFlag = false;
				}

				if (rc != RetCode.Success)
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
