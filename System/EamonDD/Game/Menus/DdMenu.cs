
// DdMenu.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using Eamon.Game.Attributes;
using EamonDD.Framework.Menus;
using static EamonDD.Game.Plugin.PluginContext;

namespace EamonDD.Game.Menus
{
	[ClassMappings]
	public class DdMenu : IDdMenu
	{
		public virtual void PrintMainMenuSubtitle()
		{
			long i;

			if (gEngine.IsAdventureFilesetLoaded())
			{
				gOut.Print("Editing: {0}",
					Globals.Module != null ? Globals.Module.Name : gEngine.UnknownName);
			}

			gOut.Write("{0}Configs: 1", Environment.NewLine);

			if (Globals.Config.DdEditingFilesets)
			{
				gOut.Write("{0}Filesets: {1}", "  ", Globals.Database.GetFilesetsCount());
			}

			if (Globals.Config.DdEditingCharacters)
			{
				gOut.Write("{0}Characters: {1}", "  ", Globals.Database.GetCharactersCount());
			}

			if (Globals.Config.DdEditingModules)
			{
				gOut.Write("{0}Modules: {1}", "  ", Globals.Database.GetModulesCount());
			}

			if (Globals.Config.DdEditingRooms)
			{
				gOut.Write("{0}Rooms: {1}", "  ", Globals.Database.GetRoomsCount());
			}

			if (Globals.Config.DdEditingArtifacts)
			{
				gOut.Write("{0}Artifacts: {1}", "  ", Globals.Database.GetArtifactsCount());
			}

			gOut.WriteLine();

			i = 0;

			if (Globals.Config.DdEditingEffects || Globals.Config.DdEditingMonsters || Globals.Config.DdEditingHints || Globals.Config.DdEditingTriggers || Globals.Config.DdEditingScripts)
			{
				gOut.WriteLine();

				if (Globals.Config.DdEditingEffects)
				{
					gOut.Write("Effects: {0}", Globals.Database.GetEffectsCount());

					i++;
				}

				if (Globals.Config.DdEditingMonsters)
				{
					gOut.Write("{0}Monsters: {1}", i > 0 ? "  " : "", Globals.Database.GetMonstersCount());

					i++;
				}

				if (Globals.Config.DdEditingHints)
				{
					gOut.Write("{0}Hints: {1}", i > 0 ? "  " : "", Globals.Database.GetHintsCount());

					i++;
				}

				if (Globals.Config.DdEditingTriggers)
				{
					gOut.Write("{0}Triggers: {1}", i > 0 ? "  " : "", Globals.Database.GetTriggersCount());

					i++;
				}

				if (Globals.Config.DdEditingScripts)
				{
					gOut.Write("{0}Scripts: {1}", i > 0 ? "  " : "", Globals.Database.GetScriptsCount());

					i++;
				}

				gOut.WriteLine();
			}
		}

		public virtual void PrintConfigMenuSubtitle()
		{
			gOut.Print("Configs: 1");
		}

		public virtual void PrintFilesetMenuSubtitle()
		{
			gOut.Print("Filesets: {0}", Globals.Database.GetFilesetsCount());
		}

		public virtual void PrintCharacterMenuSubtitle()
		{
			gOut.Print("Characters: {0}", Globals.Database.GetCharactersCount());
		}

		public virtual void PrintModuleMenuSubtitle()
		{
			if (gEngine.IsAdventureFilesetLoaded())
			{
				gOut.Print("Editing: {0}",
					Globals.Module != null ? Globals.Module.Name : gEngine.UnknownName);
			}

			gOut.Print("Modules: {0}", Globals.Database.GetModulesCount());
		}

		public virtual void PrintRoomMenuSubtitle()
		{
			if (gEngine.IsAdventureFilesetLoaded())
			{
				gOut.Print("Editing: {0}",
					Globals.Module != null ? Globals.Module.Name : gEngine.UnknownName);
			}

			gOut.Print("Rooms: {0}", Globals.Database.GetRoomsCount());
		}

		public virtual void PrintArtifactMenuSubtitle()
		{
			if (gEngine.IsAdventureFilesetLoaded())
			{
				gOut.Print("Editing: {0}",
					Globals.Module != null ? Globals.Module.Name : gEngine.UnknownName);
			}

			gOut.Print("Artifacts: {0}", Globals.Database.GetArtifactsCount());
		}

		public virtual void PrintEffectMenuSubtitle()
		{
			if (gEngine.IsAdventureFilesetLoaded())
			{
				gOut.Print("Editing: {0}",
					Globals.Module != null ? Globals.Module.Name : gEngine.UnknownName);
			}

			gOut.Print("Effects: {0}", Globals.Database.GetEffectsCount());
		}

		public virtual void PrintMonsterMenuSubtitle()
		{
			if (gEngine.IsAdventureFilesetLoaded())
			{
				gOut.Print("Editing: {0}",
					Globals.Module != null ? Globals.Module.Name : gEngine.UnknownName);
			}

			gOut.Print("Monsters: {0}", Globals.Database.GetMonstersCount());
		}

		public virtual void PrintHintMenuSubtitle()
		{
			if (gEngine.IsAdventureFilesetLoaded())
			{
				gOut.Print("Editing: {0}",
					Globals.Module != null ? Globals.Module.Name : gEngine.UnknownName);
			}

			gOut.Print("Hints: {0}", Globals.Database.GetHintsCount());
		}

		public virtual void PrintTriggerMenuSubtitle()
		{
			if (gEngine.IsAdventureFilesetLoaded())
			{
				gOut.Print("Editing: {0}",
					Globals.Module != null ? Globals.Module.Name : gEngine.UnknownName);
			}

			gOut.Print("Triggers: {0}", Globals.Database.GetTriggersCount());
		}

		public virtual void PrintScriptMenuSubtitle()
		{
			if (gEngine.IsAdventureFilesetLoaded())
			{
				gOut.Print("Editing: {0}",
					Globals.Module != null ? Globals.Module.Name : gEngine.UnknownName);
			}

			gOut.Print("Scripts: {0}", Globals.Database.GetScriptsCount());
		}
	}
}
