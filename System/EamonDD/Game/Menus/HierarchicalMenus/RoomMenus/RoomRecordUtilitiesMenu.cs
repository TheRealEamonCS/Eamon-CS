﻿
// RoomRecordUtilitiesMenu.cs

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
	public class RoomRecordUtilitiesMenu : Menu, IRoomRecordUtilitiesMenu
	{
		public override void PrintSubtitle()
		{
			gEngine.DdMenu.PrintRoomMenuSubtitle();
		}

		public RoomRecordUtilitiesMenu()
		{
			Title = "ROOM RECORD UTILITIES MENU";

			Buf = gEngine.Buf;

			MenuItemList = new List<IMenuItem>();

			MenuItemList.Add(gEngine.CreateInstance<IMenuItem>(x =>
			{
				x.SelectChar = (char)('1' + MenuItemList.Count);
				x.LineText = string.Format("{0}{1}. Brief map of Room record connections.", Environment.NewLine, MenuItemList.Count + 1);
				x.SubMenu = gEngine.CreateInstance<IBriefMapRoomRecordConnectionsMenu>();
			}));

			MenuItemList.Add(gEngine.CreateInstance<IMenuItem>(x =>
			{
				x.SelectChar = (char)('1' + MenuItemList.Count);
				x.LineText = string.Format("{0}{1}. Full map of Room record connections.", Environment.NewLine, MenuItemList.Count + 1);
				x.SubMenu = gEngine.CreateInstance<IFullMapRoomRecordConnectionsMenu>();
			}));

			if (gEngine.IsAdventureFilesetLoaded())
			{
				MenuItemList.Add(gEngine.CreateInstance<IMenuItem>(x =>
				{
					x.SelectChar = (char)('1' + MenuItemList.Count);
					x.LineText = string.Format("{0}{1}. Analyse Room record interdependencies.", Environment.NewLine, MenuItemList.Count + 1);
					x.SubMenu = gEngine.CreateInstance<IAnalyseRoomRecordInterdependenciesMenu>();
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
