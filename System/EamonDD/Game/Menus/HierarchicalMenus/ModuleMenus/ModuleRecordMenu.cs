
// ModuleRecordMenu.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using Eamon.Framework.Menus;
using Eamon.Game.Attributes;
using Eamon.Game.Menus;
using EamonDD.Framework.Menus.ActionMenus;
using EamonDD.Framework.Menus.HierarchicalMenus;
using static EamonDD.Game.Plugin.PluginContext;

namespace EamonDD.Game.Menus.HierarchicalMenus
{
	[ClassMappings]
	public class ModuleRecordMenu : Menu, IModuleRecordMenu
	{
		public override void PrintSubtitle()
		{
			Globals.DdMenu.PrintModuleMenuSubtitle();
		}

		public ModuleRecordMenu()
		{
			Title = "MODULE RECORD MENU";

			Buf = Globals.Buf;

			MenuItemList = new List<IMenuItem>();

			MenuItemList.Add(Globals.CreateInstance<IMenuItem>(x =>
			{
				x.SelectChar = (char)('1' + MenuItemList.Count);
				x.LineText = string.Format("{0}{1}. Add a Module record.", Environment.NewLine, MenuItemList.Count + 1);
				x.SubMenu = Globals.CreateInstance<IAddModuleRecordMenu>();
			}));

			MenuItemList.Add(Globals.CreateInstance<IMenuItem>(x =>
			{
				x.SelectChar = (char)('1' + MenuItemList.Count);
				x.LineText = string.Format("{0}{1}. Edit a Module record.", Environment.NewLine, MenuItemList.Count + 1);
				x.SubMenu = Globals.CreateInstance<Framework.Menus.HierarchicalMenus.IEditModuleRecordMenu>();
			}));

			MenuItemList.Add(Globals.CreateInstance<IMenuItem>(x =>
			{
				x.SelectChar = (char)('1' + MenuItemList.Count);
				x.LineText = string.Format("{0}{1}. Delete a Module record.", Environment.NewLine, MenuItemList.Count + 1);
				x.SubMenu = Globals.CreateInstance<IDeleteModuleRecordMenu>();
			}));

			MenuItemList.Add(Globals.CreateInstance<IMenuItem>(x =>
			{
				x.SelectChar = (char)('1' + MenuItemList.Count);
				x.LineText = string.Format("{0}{1}. List Module records.", Environment.NewLine, MenuItemList.Count + 1);
				x.SubMenu = Globals.CreateInstance<IListModuleRecordMenu>();
			}));

			MenuItemList.Add(Globals.CreateInstance<IMenuItem>(x =>
			{
				x.SelectChar = (char)('1' + MenuItemList.Count);
				x.LineText = string.Format("{0}{1}. Module record utilities.", Environment.NewLine, MenuItemList.Count + 1);
				x.SubMenu = Globals.CreateInstance<IModuleRecordUtilitiesMenu>();
			}));

			MenuItemList.Add(Globals.CreateInstance<IMenuItem>(x =>
			{
				x.SelectChar = 'X';
				x.LineText = string.Format("{0}X. Exit.{0}", Environment.NewLine);
				x.SubMenu = null;
			}));
		}
	}
}
