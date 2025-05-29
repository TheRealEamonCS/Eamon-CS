
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

				gEngine.LoadPortabilityClassMappings = LoadPortabilityClassMappings;

				// Resolve portability class mappings

				gEngine.ResolvePortabilityClassMappings();

				// Process command line args

				gEngine.ProcessArgv(args);

				// Load plugin class mappings

				rc = gEngine.LoadPluginClassMappings();

				if (rc != RetCode.Success)
				{
					nlFlag = false;

					goto Cleanup;
				}

				try
				{
					// Initialize system

					gEngine.InitSystem();

					gEngine.LineWrapUserInput = LineWrapUserInput;

					// Disable resolution of uid macros

					gOut.ResolveUidMacros = false;

					// Disable extraneous newline suppression

					gOut.SuppressNewLines = false;

					// Make the cursor disappear

					gOut.CursorVisible = false;

					// Initialize Config record

					gEngine.Config.Uid = 1;

					gEngine.Config.ShowDesc = true;

					gEngine.Config.GenerateUids = true;

					gEngine.Config.DeleteCharArts = true;

					gEngine.Config.FieldDesc = FieldDesc.Full;

					gEngine.Config.WordWrapMargin = gEngine.RightMargin;

					// Change window title bar and size

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
						// Do nothing
					}

					if (gEngine.EnableScreenReaderMode)
					{
						gEngine.Thread.Sleep(1000);
					}

					// Make announcements

					gOut.Write("{0}Eamon CS Main Hall ({1}) {2}.", Environment.NewLine, ProgramName, gEngine.ProgVersion);

					gOut.Write("{0}Copyright (c) 2014+ by Michael Penner.  All rights reserved.", Environment.NewLine);

					gOut.Print("This MIT Licensed free software has ABSOLUTELY NO WARRANTY.");

					// Copy and store command line args

					gEngine.Argv = new string[args.Length];

					for (i = 0; i < args.Length; i++)
					{
						gEngine.Argv[i] = gEngine.CloneInstance(args[i]);
					}

					// Process command line args

					gEngine.MhProcessArgv(false, ref nlFlag);

					// Assign default work directory, if necessary

					if (gEngine.WorkDir.Length == 0)
					{
						gEngine.WorkDir = gEngine.DefaultWorkDir;
					}

					// Initialize Config record

					gEngine.Config.MhFilesetFileName = "ADVENTURES.DAT";

					gEngine.Config.MhCharacterFileName = "CHARACTERS.DAT";

					gEngine.Config.MhCharArtFileName = "INVENTORY.DAT";

					gEngine.Config.MhEffectFileName = "SNAPPY.DAT";

					if (gEngine.WorkDir.Length > 0)
					{
						// If working directory does not exist

						if (!gEngine.Directory.Exists(gEngine.WorkDir))
						{
							gOut.Print("{0}", gEngine.LineSep);

							gOut.Print("The working directory [{0}] does not exist.", gEngine.WorkDir);

							gOut.Write("{0}Would you like to create it (Y/N) [{1}N]: ", Environment.NewLine, gEngine.EnableScreenReaderMode ? "Default " : "");

							gEngine.Buf.Clear();

							rc = gEngine.In.ReadField(gEngine.Buf, gEngine.BufSize02, null, ' ', '\0', true, "N", gEngine.ModifyCharToUpper, gEngine.IsCharYOrN, gEngine.IsCharYOrN);

							Debug.Assert(gEngine.IsSuccess(rc));

							if (gEngine.Buf[0] != 'Y')
							{
								nlFlag = false;

								goto Cleanup;
							}

							// Create working directory

							gEngine.Directory.CreateDirectory(gEngine.WorkDir);
						}

						// Change to working directory

						gEngine.Directory.SetCurrentDirectory(gEngine.WorkDir);
					}

					// Load the config datafile

					if (gEngine.ConfigFileName.Length > 0)
					{
						gOut.Print("{0}", gEngine.LineSep);

						rc = gDatabase.LoadConfigs(gEngine.ConfigFileName);

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

							if (config.MhCharArtFileName.Length == 0)
							{
								config.MhCharArtFileName = gEngine.Config.MhCharArtFileName;

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
							gEngine.Config.Uid = gDatabase.GetConfigUid();

							rc = gDatabase.AddConfig(gEngine.Config);

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

					// Process command line args

					gEngine.MhProcessArgv(true, ref nlFlag);

					if (nlFlag)
					{
						gOut.WriteLine();
					}

					nlFlag = true;

					gOut.Print("{0}", gEngine.LineSep);

					gDatabase.PushArtifactTable(ArtifactTableType.CharArt);

					rc = gDatabase.LoadFilesets(gEngine.Config.MhFilesetFileName);

					if (gEngine.IsFailure(rc))
					{
						gEngine.Error.Write("Error: LoadFilesets function call failed.");

						goto Cleanup;
					}

					rc = gDatabase.LoadCharacters(gEngine.Config.MhCharacterFileName);

					if (gEngine.IsFailure(rc))
					{
						gEngine.Error.Write("Error: LoadCharacters function call failed.");

						goto Cleanup;
					}

					rc = gDatabase.LoadArtifacts(gEngine.Config.MhCharArtFileName);

					if (gEngine.IsFailure(rc))
					{
						gEngine.Error.Write("Error: LoadArtifacts function call failed.");

						goto Cleanup;
					}

					rc = gDatabase.LoadEffects(gEngine.Config.MhEffectFileName);

					if (gEngine.IsFailure(rc))
					{
						gEngine.Error.Write("Error: LoadEffects function call failed.");

						goto Cleanup;
					}

					gOut.WriteLine();

					// Auto load character if necessary

					if (gEngine.CharacterName.Length > 0 && !gEngine.CharacterName.Equals("NONE", StringComparison.OrdinalIgnoreCase))
					{
						gEngine.Character = gDatabase.CharacterTable.Records.FirstOrDefault(c => c.Name.Equals(gEngine.CharacterName, StringComparison.OrdinalIgnoreCase));

						if (gCharacter == null || gCharacter.Uid <= 0 || gCharacter.Status != Status.Alive)
						{
							gEngine.CharacterName = "";

							gEngine.Character = null;
						}
					}

					// Create appropriate menu

					if (gCharacter != null)
					{
						gEngine.Menu = gEngine.CreateInstance<IMainHallMenu>();
					}
					else
					{
						gEngine.Menu = gEngine.CreateInstance<IOuterChamberMenu>();
					}

					// Call appropriate menu

					gEngine.Menu.Execute();

					var saveDataFiles = (gEngine.ConfigFileName.Length > 0 && gEngine.ConfigsModified) || gEngine.FilesetsModified || gEngine.CharactersModified || gEngine.CharArtsModified || gEngine.EffectsModified;

					// Save datafiles, if any modifications were made

					if (saveDataFiles)
					{
						gOut.Print("{0}", gEngine.LineSep);

						// Save the datafiles

						if (gEngine.EffectsModified)
						{
							rc = gDatabase.SaveEffects(gEngine.Config.MhEffectFileName);

							if (gEngine.IsFailure(rc))
							{
								gEngine.Error.Write("Error: SaveEffects function call failed.");

								rc = RetCode.Success;

								// goto Cleanup omitted
							}
						}

						if (gEngine.CharArtsModified)
						{
							rc = gDatabase.SaveArtifacts(gEngine.Config.MhCharArtFileName);

							if (gEngine.IsFailure(rc))
							{
								gEngine.Error.Write("Error: SaveArtifacts function call failed.");

								rc = RetCode.Success;

								// goto Cleanup omitted
							}
						}

						if (gEngine.CharactersModified)
						{
							rc = gDatabase.SaveCharacters(gEngine.Config.MhCharacterFileName);

							if (gEngine.IsFailure(rc))
							{
								gEngine.Error.Write("Error: SaveCharacters function call failed.");

								rc = RetCode.Success;

								// goto Cleanup omitted
							}
						}

						if (gEngine.FilesetsModified)
						{
							rc = gDatabase.SaveFilesets(gEngine.Config.MhFilesetFileName);

							if (gEngine.IsFailure(rc))
							{
								gEngine.Error.Write("Error: SaveFilesets function call failed.");

								rc = RetCode.Success;

								// goto Cleanup omitted
							}
						}

						if (gEngine.ConfigFileName.Length > 0 && gEngine.ConfigsModified)
						{
							rc = gDatabase.SaveConfigs(gEngine.ConfigFileName);

							if (gEngine.IsFailure(rc))
							{
								gEngine.Error.Write("Error: SaveConfigs function call failed.");

								rc = RetCode.Success;

								// goto Cleanup omitted
							}
						}

						gOut.WriteLine();
					}

					// Send character on adventure if necessary

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
					// De-initialize system

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
