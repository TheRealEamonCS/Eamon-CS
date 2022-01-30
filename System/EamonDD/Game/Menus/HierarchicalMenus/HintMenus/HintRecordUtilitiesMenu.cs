
// HintRecordUtilitiesMenu.cs

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
	public class HintRecordUtilitiesMenu : Menu, IHintRecordUtilitiesMenu
	{
		public override void PrintSubtitle()
		{
			Globals.DdMenu.PrintHintMenuSubtitle();
		}

		public HintRecordUtilitiesMenu()
		{
			Title = "HINT RECORD UTILITIES MENU";

			Buf = Globals.Buf;

			MenuItemList = new List<IMenuItem>();

			if (gEngine.IsAdventureFilesetLoaded())
			{
				MenuItemList.Add(Globals.CreateInstance<IMenuItem>(x =>
				{
					x.SelectChar = (char)('1' + MenuItemList.Count);
					x.LineText = string.Format("{0}{1}. Analyse Hint record interdependencies.", Environment.NewLine, MenuItemList.Count + 1);
					x.SubMenu = Globals.CreateInstance<IAnalyseHintRecordInterdependenciesMenu>();
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
