
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
				gOut.Write("{0}Filesets: {1}", "  ", gDatabase.GetFilesetCount());
			}

			if (!gEngine.BortCommand && gEngine.Config.DdEditingCharacters)
			{
				gOut.Write("{0}Characters: {1}", "  ", gDatabase.GetCharacterCount());
			}

			if (gEngine.Config.DdEditingModules)
			{
				gOut.Write("{0}Modules: {1}", "  ", gDatabase.GetModuleCount());
			}

			if (gEngine.Config.DdEditingRooms)
			{
				gOut.Write("{0}Rooms: {1}", "  ", gDatabase.GetRoomCount());
			}

			if (gEngine.Config.DdEditingArtifacts || (!gEngine.BortCommand && gEngine.Config.DdEditingCharArts))
			{
				gOut.Write("{0}Artifacts: {1}", "  ", gDatabase.GetArtifactCount());
			}

			gOut.WriteLine();

			i = 0;

			if (gEngine.Config.DdEditingEffects || gEngine.Config.DdEditingMonsters || gEngine.Config.DdEditingHints)
			{
				gOut.WriteLine();

				if (gEngine.Config.DdEditingEffects)
				{
					gOut.Write("Effects: {0}", gDatabase.GetEffectCount());

					i++;
				}

				if (gEngine.Config.DdEditingMonsters)
				{
					gOut.Write("{0}Monsters: {1}", i > 0 ? "  " : "", gDatabase.GetMonsterCount());

					i++;
				}

				if (gEngine.Config.DdEditingHints)
				{
					gOut.Write("{0}Hints: {1}", i > 0 ? "  " : "", gDatabase.GetHintCount());

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
			gOut.Print("Filesets: {0}", gDatabase.GetFilesetCount());
		}

		public virtual void PrintCharacterMenuSubtitle()
		{
			gOut.Print("Characters: {0}", gDatabase.GetCharacterCount());
		}

		public virtual void PrintModuleMenuSubtitle()
		{
			if (gEngine.IsAdventureFilesetLoaded())
			{
				gOut.Print("Editing: {0}",
					gEngine.Module != null ? gEngine.Module.Name : gEngine.UnknownName);
			}

			gOut.Print("Modules: {0}", gDatabase.GetModuleCount());
		}

		public virtual void PrintRoomMenuSubtitle()
		{
			if (gEngine.IsAdventureFilesetLoaded())
			{
				gOut.Print("Editing: {0}",
					gEngine.Module != null ? gEngine.Module.Name : gEngine.UnknownName);
			}

			gOut.Print("Rooms: {0}", gDatabase.GetRoomCount());
		}

		public virtual void PrintArtifactMenuSubtitle()
		{
			if (gEngine.IsAdventureFilesetLoaded())
			{
				gOut.Print("Editing: {0}",
					gEngine.Module != null ? gEngine.Module.Name : gEngine.UnknownName);
			}

			gOut.Print("Artifacts: {0}", gDatabase.GetArtifactCount());
		}

		public virtual void PrintEffectMenuSubtitle()
		{
			if (gEngine.IsAdventureFilesetLoaded())
			{
				gOut.Print("Editing: {0}",
					gEngine.Module != null ? gEngine.Module.Name : gEngine.UnknownName);
			}

			gOut.Print("Effects: {0}", gDatabase.GetEffectCount());
		}

		public virtual void PrintMonsterMenuSubtitle()
		{
			if (gEngine.IsAdventureFilesetLoaded())
			{
				gOut.Print("Editing: {0}",
					gEngine.Module != null ? gEngine.Module.Name : gEngine.UnknownName);
			}

			gOut.Print("Monsters: {0}", gDatabase.GetMonsterCount());
		}

		public virtual void PrintHintMenuSubtitle()
		{
			if (gEngine.IsAdventureFilesetLoaded())
			{
				gOut.Print("Editing: {0}",
					gEngine.Module != null ? gEngine.Module.Name : gEngine.UnknownName);
			}

			gOut.Print("Hints: {0}", gDatabase.GetHintCount());
		}
	}
}
