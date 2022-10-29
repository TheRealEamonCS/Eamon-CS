
// MainMenu.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using Eamon.Game.Attributes;
using Eamon.Game.Menus;
using Eamon.Framework.Menus;
using EamonDD.Framework.Menus.HierarchicalMenus;
using static EamonDD.Game.Plugin.Globals;

namespace EamonDD.Game.Menus.HierarchicalMenus
{
	[ClassMappings]
	public class MainMenu : Menu, IMainMenu
	{
		public override void PrintSubtitle()
		{
			gEngine.DdMenu.PrintMainMenuSubtitle();
		}

		public MainMenu()
		{
			Title = "MAIN MENU";

			Buf = gEngine.Buf;

			MenuItemList = new List<IMenuItem>();

			MenuItemList.Add(gEngine.CreateInstance<IMenuItem>(x =>
			{
				x.SelectChar = (char)('1' + MenuItemList.Count);
				x.LineText = string.Format("{0}{1}. Config record maintenance.", Environment.NewLine, MenuItemList.Count + 1);
				x.SubMenu = gEngine.CreateInstance<IConfigRecordMenu>();
			}));

			if (!gEngine.BortCommand && gEngine.Config.DdEditingFilesets)
			{
				MenuItemList.Add(gEngine.CreateInstance<IMenuItem>(x =>
				{
					x.SelectChar = (char)('1' + MenuItemList.Count);
					x.LineText = string.Format("{0}{1}. Fileset record maintenance.", Environment.NewLine, MenuItemList.Count + 1);
					x.SubMenu = gEngine.CreateInstance<IFilesetRecordMenu>();
				}));
			}

			if (!gEngine.BortCommand && gEngine.Config.DdEditingCharacters)
			{
				MenuItemList.Add(gEngine.CreateInstance<IMenuItem>(x =>
				{
					x.SelectChar = (char)('1' + MenuItemList.Count);
					x.LineText = string.Format("{0}{1}. Character record maintenance.", Environment.NewLine, MenuItemList.Count + 1);
					x.SubMenu = gEngine.CreateInstance<ICharacterRecordMenu>();
				}));
			}

			if (gEngine.Config.DdEditingModules)
			{
				MenuItemList.Add(gEngine.CreateInstance<IMenuItem>(x =>
				{
					x.SelectChar = (char)('1' + MenuItemList.Count);
					x.LineText = string.Format("{0}{1}. Module record maintenance.", Environment.NewLine, MenuItemList.Count + 1);
					x.SubMenu = gEngine.CreateInstance<IModuleRecordMenu>();
				}));
			}

			if (gEngine.Config.DdEditingRooms)
			{
				MenuItemList.Add(gEngine.CreateInstance<IMenuItem>(x =>
				{
					x.SelectChar = (char)('1' + MenuItemList.Count);
					x.LineText = string.Format("{0}{1}. Room record maintenance.", Environment.NewLine, MenuItemList.Count + 1);
					x.SubMenu = gEngine.CreateInstance<IRoomRecordMenu>();
				}));
			}

			if (gEngine.Config.DdEditingArtifacts)
			{
				MenuItemList.Add(gEngine.CreateInstance<IMenuItem>(x =>
				{
					x.SelectChar = (char)('1' + MenuItemList.Count);
					x.LineText = string.Format("{0}{1}. Artifact record maintenance.", Environment.NewLine, MenuItemList.Count + 1);
					x.SubMenu = gEngine.CreateInstance<IArtifactRecordMenu>();
				}));
			}

			if (gEngine.Config.DdEditingEffects)
			{
				MenuItemList.Add(gEngine.CreateInstance<IMenuItem>(x =>
				{
					x.SelectChar = (char)('1' + MenuItemList.Count);
					x.LineText = string.Format("{0}{1}. Effect record maintenance.", Environment.NewLine, MenuItemList.Count + 1);
					x.SubMenu = gEngine.CreateInstance<IEffectRecordMenu>();
				}));
			}

			if (gEngine.Config.DdEditingMonsters)
			{
				MenuItemList.Add(gEngine.CreateInstance<IMenuItem>(x =>
				{
					x.SelectChar = (char)('1' + MenuItemList.Count);
					x.LineText = string.Format("{0}{1}. Monster record maintenance.", Environment.NewLine, MenuItemList.Count + 1);
					x.SubMenu = gEngine.CreateInstance<IMonsterRecordMenu>();
				}));
			}

			if (gEngine.Config.DdEditingHints)
			{
				MenuItemList.Add(gEngine.CreateInstance<IMenuItem>(x =>
				{
					x.SelectChar = (char)('1' + MenuItemList.Count);
					x.LineText = string.Format("{0}{1}. Hint record maintenance.", Environment.NewLine, MenuItemList.Count + 1);
					x.SubMenu = gEngine.CreateInstance<IHintRecordMenu>();
				}));
			}

			MenuItemList.Add(gEngine.CreateInstance<IMenuItem>(x =>
			{
				x.SelectChar = MenuItemList.Count + 1 > 9 ? 'U' : (char)('1' + MenuItemList.Count);
				x.LineText = string.Format("{0}{1}. Utilities.", Environment.NewLine, MenuItemList.Count + 1 > 9 ? "U" : (MenuItemList.Count + 1).ToString());
				x.SubMenu = gEngine.CreateInstance<IUtilitiesMenu>();
			}));

			MenuItemList.Add(gEngine.CreateInstance<IMenuItem>(x =>
			{
				x.SelectChar = 'X';
				x.LineText = string.Format("{0}X. Exit.{0}", Environment.NewLine);
				x.SubMenu = null;
			}));
		}
	}
}
