
// ModuleRecordUtilitiesMenu.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using Eamon.Framework.Menus;
using Eamon.Game.Attributes;
using Eamon.Game.Menus;
using EamonDD.Framework.Menus.ActionMenus;
using EamonDD.Framework.Menus.HierarchicalMenus;
using static EamonDD.Game.Plugin.Globals;

namespace EamonDD.Game.Menus.HierarchicalMenus
{
	[ClassMappings]
	public class ModuleRecordUtilitiesMenu : Menu, IModuleRecordUtilitiesMenu
	{
		public override void PrintSubtitle()
		{
			if (gEngine.IsAdventureFilesetLoaded())
			{
				gOut.Print("Editing: {0}",
					gEngine.Module != null ? gEngine.Module.Name : gEngine.UnknownName);
			}

			gOut.Print("Modules: {0}", gDatabase.GetModuleCount());
		}

		public ModuleRecordUtilitiesMenu()
		{
			Title = "MODULE RECORD UTILITIES MENU";

			Buf = gEngine.Buf;

			MenuItemList = new List<IMenuItem>();

			if (gEngine.IsAdventureFilesetLoaded())
			{
				MenuItemList.Add(gEngine.CreateInstance<IMenuItem>(x =>
				{
					x.SelectChar = (char)('1' + MenuItemList.Count);
					x.LineText = string.Format("{0}{1}. Analyse Module record interdependencies.", Environment.NewLine, MenuItemList.Count + 1);
					x.SubMenu = gEngine.CreateInstance<IAnalyseModuleRecordInterdependenciesMenu>();
				}));
			}

			MenuItemList.Add(gEngine.CreateInstance<IMenuItem>(x =>
			{
				x.SelectChar = 'X';
				x.LineText = string.Format("{0}X. Exit.{0}", Environment.NewLine);
				x.SubMenu = null;
			}));
		}
	}
}
