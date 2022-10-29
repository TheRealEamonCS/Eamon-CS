
// DdMenu.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using Eamon.Game.Attributes;
using EamonDD.Framework.Menus;
using static EamonDD.Game.Plugin.Globals;

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
					gEngine.Module != null ? gEngine.Module.Name : gEngine.UnknownName);
			}

			gOut.Write("{0}Configs: 1", Environment.NewLine);

			if (!gEngine.BortCommand && gEngine.Config.DdEditingFilesets)
			{
				gOut.Write("{0}Filesets: {1}", "  ", gEngine.Database.GetFilesetCount());
			}

			if (!gEngine.BortCommand && gEngine.Config.DdEditingCharacters)
			{
				gOut.Write("{0}Characters: {1}", "  ", gEngine.Database.GetCharacterCount());
			}

			if (gEngine.Config.DdEditingModules)
			{
				gOut.Write("{0}Modules: {1}", "  ", gEngine.Database.GetModuleCount());
			}

			if (gEngine.Config.DdEditingRooms)
			{
				gOut.Write("{0}Rooms: {1}", "  ", gEngine.Database.GetRoomCount());
			}

			if (gEngine.Config.DdEditingArtifacts)
			{
				gOut.Write("{0}Artifacts: {1}", "  ", gEngine.Database.GetArtifactCount());
			}

			gOut.WriteLine();

			i = 0;

			if (gEngine.Config.DdEditingEffects || gEngine.Config.DdEditingMonsters || gEngine.Config.DdEditingHints)
			{
				gOut.WriteLine();

				if (gEngine.Config.DdEditingEffects)
				{
					gOut.Write("Effects: {0}", gEngine.Database.GetEffectCount());

					i++;
				}

				if (gEngine.Config.DdEditingMonsters)
				{
					gOut.Write("{0}Monsters: {1}", i > 0 ? "  " : "", gEngine.Database.GetMonsterCount());

					i++;
				}

				if (gEngine.Config.DdEditingHints)
				{
					gOut.Write("{0}Hints: {1}", i > 0 ? "  " : "", gEngine.Database.GetHintCount());

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
			gOut.Print("Filesets: {0}", gEngine.Database.GetFilesetCount());
		}

		public virtual void PrintCharacterMenuSubtitle()
		{
			gOut.Print("Characters: {0}", gEngine.Database.GetCharacterCount());
		}

		public virtual void PrintModuleMenuSubtitle()
		{
			if (gEngine.IsAdventureFilesetLoaded())
			{
				gOut.Print("Editing: {0}",
					gEngine.Module != null ? gEngine.Module.Name : gEngine.UnknownName);
			}

			gOut.Print("Modules: {0}", gEngine.Database.GetModuleCount());
		}

		public virtual void PrintRoomMenuSubtitle()
		{
			if (gEngine.IsAdventureFilesetLoaded())
			{
				gOut.Print("Editing: {0}",
					gEngine.Module != null ? gEngine.Module.Name : gEngine.UnknownName);
			}

			gOut.Print("Rooms: {0}", gEngine.Database.GetRoomCount());
		}

		public virtual void PrintArtifactMenuSubtitle()
		{
			if (gEngine.IsAdventureFilesetLoaded())
			{
				gOut.Print("Editing: {0}",
					gEngine.Module != null ? gEngine.Module.Name : gEngine.UnknownName);
			}

			gOut.Print("Artifacts: {0}", gEngine.Database.GetArtifactCount());
		}

		public virtual void PrintEffectMenuSubtitle()
		{
			if (gEngine.IsAdventureFilesetLoaded())
			{
				gOut.Print("Editing: {0}",
					gEngine.Module != null ? gEngine.Module.Name : gEngine.UnknownName);
			}

			gOut.Print("Effects: {0}", gEngine.Database.GetEffectCount());
		}

		public virtual void PrintMonsterMenuSubtitle()
		{
			if (gEngine.IsAdventureFilesetLoaded())
			{
				gOut.Print("Editing: {0}",
					gEngine.Module != null ? gEngine.Module.Name : gEngine.UnknownName);
			}

			gOut.Print("Monsters: {0}", gEngine.Database.GetMonsterCount());
		}

		public virtual void PrintHintMenuSubtitle()
		{
			if (gEngine.IsAdventureFilesetLoaded())
			{
				gOut.Print("Editing: {0}",
					gEngine.Module != null ? gEngine.Module.Name : gEngine.UnknownName);
			}

			gOut.Print("Hints: {0}", gEngine.Database.GetHintCount());
		}
	}
}
