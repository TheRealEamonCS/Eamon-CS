
// AdventureSupportMenu.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Eamon.Framework.Menus;
using Eamon.Game.Attributes;
using Eamon.Game.Menus;
using EamonDD.Framework.Menus.ActionMenus;
using EamonDD.Framework.Menus.HierarchicalMenus;
using static EamonDD.Game.Plugin.PluginContext;

namespace EamonDD.Game.Menus.HierarchicalMenus
{
	[ClassMappings]
	public class AdventureSupportMenu : Menu, IAdventureSupportMenu
	{
		public AdventureSupportMenu()
		{
			Title = "ADVENTURE SUPPORT MENU";

			Buf = Globals.Buf;

			MenuItemList = new List<IMenuItem>();

			Debug.Assert(!gEngine.IsAdventureFilesetLoaded());

			MenuItemList.Add(Globals.CreateInstance<IMenuItem>(x =>
			{
				x.SelectChar = (char)('1' + MenuItemList.Count);
				x.LineText = string.Format("{0}{1}. Add a standard adventure.", Environment.NewLine, MenuItemList.Count + 1);
				x.SubMenu = Globals.CreateInstance<IAddStandardAdventureMenu>();
			}));

			if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) || RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
			{
				MenuItemList.Add(Globals.CreateInstance<IMenuItem>(x =>
				{
					x.SelectChar = (char)('1' + MenuItemList.Count);
					x.LineText = string.Format("{0}{1}. Add a custom adventure.", Environment.NewLine, MenuItemList.Count + 1);
					x.SubMenu = Globals.CreateInstance<IAddCustomAdventureMenu>();
				}));

				MenuItemList.Add(Globals.CreateInstance<IMenuItem>(x =>
				{
					x.SelectChar = (char)('1' + MenuItemList.Count);
					x.LineText = string.Format("{0}{1}. Add custom adventure classes.", Environment.NewLine, MenuItemList.Count + 1);
					x.SubMenu = Globals.CreateInstance<IAddCustomAdventureClassesMenu>();
				}));
			}

			MenuItemList.Add(Globals.CreateInstance<IMenuItem>(x =>
			{
				x.SelectChar = (char)('1' + MenuItemList.Count);
				x.LineText = string.Format("{0}{1}. Delete an adventure.", Environment.NewLine, MenuItemList.Count + 1);
				x.SubMenu = Globals.CreateInstance<IDeleteAdventureMenu>();
			}));

			if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) || RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
			{
				MenuItemList.Add(Globals.CreateInstance<IMenuItem>(x =>
				{
					x.SelectChar = (char)('1' + MenuItemList.Count);
					x.LineText = string.Format("{0}{1}. Delete custom adventure classes.", Environment.NewLine, MenuItemList.Count + 1);
					x.SubMenu = Globals.CreateInstance<IDeleteCustomAdventureClassesMenu>();
				}));
			}

			MenuItemList.Add(Globals.CreateInstance<IMenuItem>(x =>
			{
				x.SelectChar = 'X';
				x.LineText = string.Format("{0}X. Exit.{0}", Environment.NewLine);
				x.SubMenu = null;
			}));
		}
	}
}
