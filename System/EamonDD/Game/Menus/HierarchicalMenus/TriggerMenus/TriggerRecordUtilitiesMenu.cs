
// TriggerRecordUtilitiesMenu.cs

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
	public class TriggerRecordUtilitiesMenu : Menu, ITriggerRecordUtilitiesMenu
	{
		public override void PrintSubtitle()
		{
			Globals.DdMenu.PrintTriggerMenuSubtitle();
		}

		public TriggerRecordUtilitiesMenu()
		{
			Title = "TRIGGER RECORD UTILITIES MENU";

			Buf = Globals.Buf;

			MenuItemList = new List<IMenuItem>();

			if (gEngine.IsAdventureFilesetLoaded())
			{

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
