
// ScriptRecordMenu.cs

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
	public class ScriptRecordMenu : Menu, IScriptRecordMenu
	{
		public override void PrintSubtitle()
		{
			Globals.DdMenu.PrintScriptMenuSubtitle();
		}

		public ScriptRecordMenu()
		{
			Title = "SCRIPT RECORD MENU";

			Buf = Globals.Buf;

			MenuItemList = new List<IMenuItem>();

			MenuItemList.Add(Globals.CreateInstance<IMenuItem>(x =>
			{
				x.SelectChar = (char)('1' + MenuItemList.Count);
				x.LineText = string.Format("{0}{1}. Script record utilities.", Environment.NewLine, MenuItemList.Count + 1);
				x.SubMenu = Globals.CreateInstance<IScriptRecordUtilitiesMenu>();
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
