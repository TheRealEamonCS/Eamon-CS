
// Engine.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using Eamon.Game.Attributes;
using EamonDD.Framework;
using static EamonDD.Game.Plugin.PluginContext;

namespace EamonDD.Game
{
	[ClassMappings(typeof(Eamon.Framework.IEngine))]
	public class Engine : Eamon.Game.Engine, IEngine
	{
		public virtual bool IsAdventureFilesetLoaded()
		{
			if (Globals.Config != null)
			{
				return Globals.Config.DdEditingModules && Globals.Config.DdEditingRooms && Globals.Config.DdEditingArtifacts && Globals.Config.DdEditingEffects && Globals.Config.DdEditingMonsters && Globals.Config.DdEditingHints && Globals.Config.DdEditingTriggers && Globals.Config.DdEditingScripts;
			}
			else
			{
				return false;
			}
		}

		public virtual void DdProcessArgv(bool secondPass, ref bool ddfnFlag, ref bool nlFlag)
		{
			long i;

			for (i = 0; i < Globals.Argv.Length; i++)
			{
				if (Globals.Argv[i].Equals("--workingDirectory", StringComparison.OrdinalIgnoreCase) || Globals.Argv[i].Equals("-wd", StringComparison.OrdinalIgnoreCase))
				{
					if (++i < Globals.Argv.Length)
					{
						// do nothing
					}
				}
				else if (Globals.Argv[i].Equals("--filePrefix", StringComparison.OrdinalIgnoreCase) || Globals.Argv[i].Equals("-fp", StringComparison.OrdinalIgnoreCase))
				{
					if (++i < Globals.Argv.Length)
					{
						// do nothing
					}
				}
				else if (Globals.Argv[i].Equals("--ignoreMutex", StringComparison.OrdinalIgnoreCase) || Globals.Argv[i].Equals("-im", StringComparison.OrdinalIgnoreCase))
				{
					// do nothing
				}
				else if (Globals.Argv[i].Equals("--runGameEditor", StringComparison.OrdinalIgnoreCase) || Globals.Argv[i].Equals("-rge", StringComparison.OrdinalIgnoreCase))
				{
					// do nothing
				}
				else if (Globals.Argv[i].Equals("--configFileName", StringComparison.OrdinalIgnoreCase) || Globals.Argv[i].Equals("-cfgfn", StringComparison.OrdinalIgnoreCase))
				{
					if (++i < Globals.Argv.Length)
					{
						if (!secondPass)
						{
							Globals.ConfigFileName = Globals.Argv[i].Trim();
						}
					}
				}
				else if (Globals.Argv[i].Equals("--filesetFileName", StringComparison.OrdinalIgnoreCase) || Globals.Argv[i].Equals("-fsfn", StringComparison.OrdinalIgnoreCase))
				{
					if (++i < Globals.Argv.Length)
					{
						if (secondPass)
						{
							Globals.Config.DdFilesetFileName = Globals.Argv[i].Trim();

							Globals.Config.DdEditingFilesets = true;

							Globals.ConfigsModified = true;
						}
						else
						{
							ddfnFlag = true;
						}
					}
				}
				else if (Globals.Argv[i].Equals("--characterFileName", StringComparison.OrdinalIgnoreCase) || Globals.Argv[i].Equals("-chrfn", StringComparison.OrdinalIgnoreCase))
				{
					if (++i < Globals.Argv.Length)
					{
						if (secondPass)
						{
							Globals.Config.DdCharacterFileName = Globals.Argv[i].Trim();

							Globals.Config.DdEditingCharacters = true;

							Globals.ConfigsModified = true;
						}
						else
						{
							ddfnFlag = true;
						}
					}
				}
				else if (Globals.Argv[i].Equals("--moduleFileName", StringComparison.OrdinalIgnoreCase) || Globals.Argv[i].Equals("-modfn", StringComparison.OrdinalIgnoreCase))
				{
					if (++i < Globals.Argv.Length)
					{
						if (secondPass)
						{
							Globals.Config.DdModuleFileName = Globals.Argv[i].Trim();

							Globals.Config.DdEditingModules = true;

							Globals.ConfigsModified = true;
						}
						else
						{
							ddfnFlag = true;
						}
					}
				}
				else if (Globals.Argv[i].Equals("--roomFileName", StringComparison.OrdinalIgnoreCase) || Globals.Argv[i].Equals("-rfn", StringComparison.OrdinalIgnoreCase))
				{
					if (++i < Globals.Argv.Length)
					{
						if (secondPass)
						{
							Globals.Config.DdRoomFileName = Globals.Argv[i].Trim();

							Globals.Config.DdEditingRooms = true;

							Globals.ConfigsModified = true;
						}
						else
						{
							ddfnFlag = true;
						}
					}
				}
				else if (Globals.Argv[i].Equals("--artifactFileName", StringComparison.OrdinalIgnoreCase) || Globals.Argv[i].Equals("-afn", StringComparison.OrdinalIgnoreCase))
				{
					if (++i < Globals.Argv.Length)
					{
						if (secondPass)
						{
							Globals.Config.DdArtifactFileName = Globals.Argv[i].Trim();

							Globals.Config.DdEditingArtifacts = true;

							Globals.ConfigsModified = true;
						}
						else
						{
							ddfnFlag = true;
						}
					}
				}
				else if (Globals.Argv[i].Equals("--effectFileName", StringComparison.OrdinalIgnoreCase) || Globals.Argv[i].Equals("-efn", StringComparison.OrdinalIgnoreCase))
				{
					if (++i < Globals.Argv.Length)
					{
						if (secondPass)
						{
							Globals.Config.DdEffectFileName = Globals.Argv[i].Trim();

							Globals.Config.DdEditingEffects = true;

							Globals.ConfigsModified = true;
						}
						else
						{
							ddfnFlag = true;
						}
					}
				}
				else if (Globals.Argv[i].Equals("--monsterFileName", StringComparison.OrdinalIgnoreCase) || Globals.Argv[i].Equals("-monfn", StringComparison.OrdinalIgnoreCase))
				{
					if (++i < Globals.Argv.Length)
					{
						if (secondPass)
						{
							Globals.Config.DdMonsterFileName = Globals.Argv[i].Trim();

							Globals.Config.DdEditingMonsters = true;

							Globals.ConfigsModified = true;
						}
						else
						{
							ddfnFlag = true;
						}
					}
				}
				else if (Globals.Argv[i].Equals("--hintFileName", StringComparison.OrdinalIgnoreCase) || Globals.Argv[i].Equals("-hfn", StringComparison.OrdinalIgnoreCase))
				{
					if (++i < Globals.Argv.Length)
					{
						if (secondPass)
						{
							Globals.Config.DdHintFileName = Globals.Argv[i].Trim();

							Globals.Config.DdEditingHints = true;

							Globals.ConfigsModified = true;
						}
						else
						{
							ddfnFlag = true;
						}
					}
				}
				else if (Globals.Argv[i].Equals("--triggerFileName", StringComparison.OrdinalIgnoreCase) || Globals.Argv[i].Equals("-trgfn", StringComparison.OrdinalIgnoreCase))
				{
					if (++i < Globals.Argv.Length)
					{
						if (secondPass)
						{
							Globals.Config.DdTriggerFileName = Globals.Argv[i].Trim();

							Globals.Config.DdEditingTriggers = true;

							Globals.ConfigsModified = true;
						}
						else
						{
							ddfnFlag = true;
						}
					}
				}
				else if (Globals.Argv[i].Equals("--scriptFileName", StringComparison.OrdinalIgnoreCase) || Globals.Argv[i].Equals("-sfn", StringComparison.OrdinalIgnoreCase))
				{
					if (++i < Globals.Argv.Length)
					{
						if (secondPass)
						{
							Globals.Config.DdScriptFileName = Globals.Argv[i].Trim();

							Globals.Config.DdEditingScripts = true;

							Globals.ConfigsModified = true;
						}
						else
						{
							ddfnFlag = true;
						}
					}
				}
				else if (Globals.Argv[i].Equals("--loadAdventure", StringComparison.OrdinalIgnoreCase) || Globals.Argv[i].Equals("-la", StringComparison.OrdinalIgnoreCase))
				{
					if (secondPass)
					{
						Globals.Config.DdEditingModules = true;

						Globals.Config.DdEditingRooms = true;

						Globals.Config.DdEditingArtifacts = true;

						Globals.Config.DdEditingEffects = true;

						Globals.Config.DdEditingMonsters = true;

						Globals.Config.DdEditingHints = true;

						Globals.Config.DdEditingTriggers = true;

						Globals.Config.DdEditingScripts = true;

						Globals.ConfigsModified = true;
					}
					else
					{
						ddfnFlag = true;
					}
				}
				else if (secondPass)
				{
					if (!nlFlag)
					{
						gOut.Print("{0}", Globals.LineSep);
					}

					gOut.Write("{0}Unrecognized command line argument: [{1}]", Environment.NewLine, Globals.Argv[i]);

					nlFlag = true;
				}
			}
		}
	}
}
