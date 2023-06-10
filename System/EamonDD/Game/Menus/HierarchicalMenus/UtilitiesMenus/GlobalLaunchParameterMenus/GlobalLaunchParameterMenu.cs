
// GlobalLaunchParameterMenu.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Eamon.Framework.Menus;
using Eamon.Game.Attributes;
using Eamon.Game.Menus;
using EamonDD.Framework.Menus.ActionMenus;
using EamonDD.Framework.Menus.HierarchicalMenus;
using static EamonDD.Game.Plugin.Globals;

namespace EamonDD.Game.Menus.HierarchicalMenus
{
	[ClassMappings]
	public class GlobalLaunchParameterMenu : Menu, IGlobalLaunchParameterMenu
	{
		public GlobalLaunchParameterMenu()
		{
			Title = "GLOBAL LAUNCH PARAMETER MENU";

			Buf = gEngine.Buf;

			MenuItemList = new List<IMenuItem>();

			Debug.Assert(!gEngine.IsAdventureFilesetLoaded());

			MenuItemList.Add(gEngine.CreateInstance<IMenuItem>(x =>
			{
				x.SelectChar = (char)('1' + MenuItemList.Count);
				x.LineText = string.Format("{0}{1}. Edit global launch parameters.", Environment.NewLine, MenuItemList.Count + 1);
				x.SubMenu = gEngine.CreateInstance<IEditGlobalLaunchParametersMenu>();
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
