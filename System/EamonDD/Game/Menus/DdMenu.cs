
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

			if (!Globals.BortCommand && Globals.Config.DdEditingFilesets)
			{
				gOut.Write("{0}Filesets: {1}", "  ", Globals.Database.GetFilesetCount());
			}

			if (!Globals.BortCommand && Globals.Config.DdEditingCharacters)
			{
				gOut.Write("{0}Characters: {1}", "  ", Globals.Database.GetCharacterCount());
			}

			if (Globals.Config.DdEditingModules)
			{
				gOut.Write("{0}Modules: {1}", "  ", Globals.Database.GetModuleCount());
			}

			if (Globals.Config.DdEditingRooms)
			{
				gOut.Write("{0}Rooms: {1}", "  ", Globals.Database.GetRoomCount());
			}

			if (Globals.Config.DdEditingArtifacts)
			{
				gOut.Write("{0}Artifacts: {1}", "  ", Globals.Database.GetArtifactCount());
			}

			gOut.WriteLine();

			i = 0;

			if (Globals.Config.DdEditingEffects || Globals.Config.DdEditingMonsters || Globals.Config.DdEditingHints)
			{
				gOut.WriteLine();

				if (Globals.Config.DdEditingEffects)
				{
					gOut.Write("Effects: {0}", Globals.Database.GetEffectCount());

					i++;
				}

				if (Globals.Config.DdEditingMonsters)
				{
					gOut.Write("{0}Monsters: {1}", i > 0 ? "  " : "", Globals.Database.GetMonsterCount());

					i++;
				}

				if (Globals.Config.DdEditingHints)
				{
					gOut.Write("{0}Hints: {1}", i > 0 ? "  " : "", Globals.Database.GetHintCount());

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
			gOut.Print("Filesets: {0}", Globals.Database.GetFilesetCount());
		}

		public virtual void PrintCharacterMenuSubtitle()
		{
			gOut.Print("Characters: {0}", Globals.Database.GetCharacterCount());
		}

		public virtual void PrintModuleMenuSubtitle()
		{
			if (gEngine.IsAdventureFilesetLoaded())
			{
				gOut.Print("Editing: {0}",
					Globals.Module != null ? Globals.Module.Name : gEngine.UnknownName);
			}

			gOut.Print("Modules: {0}", Globals.Database.GetModuleCount());
		}

		public virtual void PrintRoomMenuSubtitle()
		{
			if (gEngine.IsAdventureFilesetLoaded())
			{
				gOut.Print("Editing: {0}",
					Globals.Module != null ? Globals.Module.Name : gEngine.UnknownName);
			}

			gOut.Print("Rooms: {0}", Globals.Database.GetRoomCount());
		}

		public virtual void PrintArtifactMenuSubtitle()
		{
			if (gEngine.IsAdventureFilesetLoaded())
			{
				gOut.Print("Editing: {0}",
					Globals.Module != null ? Globals.Module.Name : gEngine.UnknownName);
			}

			gOut.Print("Artifacts: {0}", Globals.Database.GetArtifactCount());
		}

		public virtual void PrintEffectMenuSubtitle()
		{
			if (gEngine.IsAdventureFilesetLoaded())
			{
				gOut.Print("Editing: {0}",
					Globals.Module != null ? Globals.Module.Name : gEngine.UnknownName);
			}

			gOut.Print("Effects: {0}", Globals.Database.GetEffectCount());
		}

		public virtual void PrintMonsterMenuSubtitle()
		{
			if (gEngine.IsAdventureFilesetLoaded())
			{
				gOut.Print("Editing: {0}",
					Globals.Module != null ? Globals.Module.Name : gEngine.UnknownName);
			}

			gOut.Print("Monsters: {0}", Globals.Database.GetMonsterCount());
		}

		public virtual void PrintHintMenuSubtitle()
		{
			if (gEngine.IsAdventureFilesetLoaded())
			{
				gOut.Print("Editing: {0}",
					Globals.Module != null ? Globals.Module.Name : gEngine.UnknownName);
			}

			gOut.Print("Hints: {0}", Globals.Database.GetHintCount());
		}
	}
}
