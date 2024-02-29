
// Engine.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon;
using Eamon.Framework;
using Eamon.Framework.Menus;
using Eamon.Framework.Primitive.Enums;
using EamonDD.Framework.Menus;
using EamonDD.Framework.Plugin;
using System;
using System.Reflection;
//using static EamonDD.Game.Plugin.Globals;

namespace EamonDD.Game.Plugin
{
	public class Engine : Eamon.Game.Plugin.Engine, IEngine
	{
		#region Public Properties

		public virtual string[] Argv { get; set; }

		public virtual long WordWrapCurrColumn { get; set; }

		public virtual char WordWrapLastChar { get; set; }

		public virtual string ConfigFileName { get; set; }

		public virtual IDdMenu DdMenu { get; set; }

		public virtual IMenu Menu { get; set; }

		public virtual IModule Module { get; set; }

		public virtual IConfig Config { get; set; }

		public virtual bool BortCommand { get; set; }

		public virtual bool ConfigsModified { get; set; }

		public virtual bool FilesetsModified { get; set; }

		public virtual bool CharactersModified { get; set; }

		public virtual bool ModulesModified { get; set; }

		public virtual bool RoomsModified { get; set; }

		public virtual bool ArtifactsModified { get; set; }

		public virtual bool EffectsModified { get; set; }

		public virtual bool MonstersModified { get; set; }

		public virtual bool HintsModified { get; set; }

		#endregion

		#region Public Methods

		public override RetCode LoadPluginClassMappings()
		{
			RetCode rc;

			rc = base.LoadPluginClassMappings();

			if (rc != RetCode.Success)
			{
				goto Cleanup;
			}

#if !DEBUG
			if (RunGameEditor)
			{
				rc = LoadPluginClassMappings01(Assembly.GetExecutingAssembly());
			}
#else
			rc = LoadPluginClassMappings01(Assembly.GetExecutingAssembly());
#endif

		Cleanup:

			return rc;
		}

		public override void InitSystem()
		{
			base.InitSystem();

			ConfigFileName = "";

			if (RunGameEditor)
			{
				DdMenu = CreateInstance<IDdMenu>();
			}

			Config = CreateInstance<IConfig>();
		}

		public override void ResetProperties(PropertyResetCode resetCode)
		{
			base.ResetProperties(resetCode);
		}

		public virtual bool IsAdventureFilesetLoaded()
		{
			if (Config != null)
			{
				return Config.DdEditingModules && Config.DdEditingRooms && Config.DdEditingArtifacts && Config.DdEditingEffects && Config.DdEditingMonsters && Config.DdEditingHints;
			}
			else
			{
				return false;
			}
		}

		public virtual void DdProcessArgv(bool secondPass, ref bool ddfnFlag, ref bool nlFlag)
		{
			long i;

			for (i = 0; i < Argv.Length; i++)
			{
				if (Argv[i].Equals("--workingDirectory", StringComparison.OrdinalIgnoreCase) || Argv[i].Equals("-wd", StringComparison.OrdinalIgnoreCase))
				{
					if (++i < Argv.Length)
					{
						// do nothing
					}
				}
				else if (Argv[i].Equals("--filePrefix", StringComparison.OrdinalIgnoreCase) || Argv[i].Equals("-fp", StringComparison.OrdinalIgnoreCase))
				{
					if (++i < Argv.Length)
					{
						// do nothing
					}
				}
				else if (Argv[i].Equals("--configFileName", StringComparison.OrdinalIgnoreCase) || Argv[i].Equals("-cfgfn", StringComparison.OrdinalIgnoreCase))
				{
					if (++i < Argv.Length)
					{
						if (!secondPass)
						{
							ConfigFileName = Argv[i].Trim();
						}
					}
				}
				else if (Argv[i].Equals("--filesetFileName", StringComparison.OrdinalIgnoreCase) || Argv[i].Equals("-fsfn", StringComparison.OrdinalIgnoreCase))
				{
					if (++i < Argv.Length)
					{
						if (secondPass)
						{
							Config.DdFilesetFileName = Argv[i].Trim();

							Config.DdEditingFilesets = true;

							ConfigsModified = true;
						}
						else
						{
							ddfnFlag = true;
						}
					}
				}
				else if (Argv[i].Equals("--characterFileName", StringComparison.OrdinalIgnoreCase) || Argv[i].Equals("-chrfn", StringComparison.OrdinalIgnoreCase))
				{
					if (++i < Argv.Length)
					{
						if (secondPass)
						{
							Config.DdCharacterFileName = Argv[i].Trim();

							Config.DdEditingCharacters = true;

							ConfigsModified = true;
						}
						else
						{
							ddfnFlag = true;
						}
					}
				}
				else if (Argv[i].Equals("--moduleFileName", StringComparison.OrdinalIgnoreCase) || Argv[i].Equals("-modfn", StringComparison.OrdinalIgnoreCase))
				{
					if (++i < Argv.Length)
					{
						if (secondPass)
						{
							Config.DdModuleFileName = Argv[i].Trim();

							Config.DdEditingModules = true;

							ConfigsModified = true;
						}
						else
						{
							ddfnFlag = true;
						}
					}
				}
				else if (Argv[i].Equals("--roomFileName", StringComparison.OrdinalIgnoreCase) || Argv[i].Equals("-rfn", StringComparison.OrdinalIgnoreCase))
				{
					if (++i < Argv.Length)
					{
						if (secondPass)
						{
							Config.DdRoomFileName = Argv[i].Trim();

							Config.DdEditingRooms = true;

							ConfigsModified = true;
						}
						else
						{
							ddfnFlag = true;
						}
					}
				}
				else if (Argv[i].Equals("--artifactFileName", StringComparison.OrdinalIgnoreCase) || Argv[i].Equals("-afn", StringComparison.OrdinalIgnoreCase))
				{
					if (++i < Argv.Length)
					{
						if (secondPass)
						{
							Config.DdArtifactFileName = Argv[i].Trim();

							Config.DdEditingArtifacts = true;

							ConfigsModified = true;
						}
						else
						{
							ddfnFlag = true;
						}
					}
				}
				else if (Argv[i].Equals("--effectFileName", StringComparison.OrdinalIgnoreCase) || Argv[i].Equals("-efn", StringComparison.OrdinalIgnoreCase))
				{
					if (++i < Argv.Length)
					{
						if (secondPass)
						{
							Config.DdEffectFileName = Argv[i].Trim();

							Config.DdEditingEffects = true;

							ConfigsModified = true;
						}
						else
						{
							ddfnFlag = true;
						}
					}
				}
				else if (Argv[i].Equals("--monsterFileName", StringComparison.OrdinalIgnoreCase) || Argv[i].Equals("-monfn", StringComparison.OrdinalIgnoreCase))
				{
					if (++i < Argv.Length)
					{
						if (secondPass)
						{
							Config.DdMonsterFileName = Argv[i].Trim();

							Config.DdEditingMonsters = true;

							ConfigsModified = true;
						}
						else
						{
							ddfnFlag = true;
						}
					}
				}
				else if (Argv[i].Equals("--hintFileName", StringComparison.OrdinalIgnoreCase) || Argv[i].Equals("-hfn", StringComparison.OrdinalIgnoreCase))
				{
					if (++i < Argv.Length)
					{
						if (secondPass)
						{
							Config.DdHintFileName = Argv[i].Trim();

							Config.DdEditingHints = true;

							ConfigsModified = true;
						}
						else
						{
							ddfnFlag = true;
						}
					}
				}
				else if (Argv[i].Equals("--loadAdventure", StringComparison.OrdinalIgnoreCase) || Argv[i].Equals("-la", StringComparison.OrdinalIgnoreCase))
				{
					if (secondPass)
					{
						Config.DdEditingModules = true;

						Config.DdEditingRooms = true;

						Config.DdEditingArtifacts = true;

						Config.DdEditingEffects = true;

						Config.DdEditingMonsters = true;

						Config.DdEditingHints = true;

						ConfigsModified = true;
					}
					else
					{
						ddfnFlag = true;
					}
				}
				else if (Argv[i].Equals("--enableScreenReaderMode", StringComparison.OrdinalIgnoreCase) || Argv[i].Equals("-esrm", StringComparison.OrdinalIgnoreCase))
				{
					// do nothing
				}
				else if (Argv[i].Equals("--ignoreMutex", StringComparison.OrdinalIgnoreCase) || Argv[i].Equals("-im", StringComparison.OrdinalIgnoreCase))
				{
					// do nothing
				}
				else if (Argv[i].Equals("--disableValidation", StringComparison.OrdinalIgnoreCase) || Argv[i].Equals("-dv", StringComparison.OrdinalIgnoreCase))
				{
					// do nothing
				}
				else if (Argv[i].Equals("--repaintWindow", StringComparison.OrdinalIgnoreCase) || Argv[i].Equals("-rw", StringComparison.OrdinalIgnoreCase))
				{
					// do nothing
				}
				else if (Argv[i].Equals("--runGameEditor", StringComparison.OrdinalIgnoreCase) || Argv[i].Equals("-rge", StringComparison.OrdinalIgnoreCase))
				{
					// do nothing
				}
				else if (secondPass)
				{
					if (!nlFlag)
					{
						Out.Print("{0}", LineSep);
					}

					Out.Write("{0}Unrecognized command line argument: [{1}]", Environment.NewLine, Argv[i]);

					nlFlag = true;
				}
			}
		}

		#endregion

		public Engine()
		{

		}
	}
}
