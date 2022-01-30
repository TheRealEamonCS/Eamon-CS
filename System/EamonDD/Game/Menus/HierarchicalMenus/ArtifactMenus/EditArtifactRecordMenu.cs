
// EditArtifactRecordMenu.cs

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
	public class EditArtifactRecordMenu : Menu, IEditArtifactRecordMenu
	{
		public override void PrintSubtitle()
		{
			Globals.DdMenu.PrintArtifactMenuSubtitle();
		}

		public EditArtifactRecordMenu()
		{
			Title = "EDIT ARTIFACT RECORD MENU";

			Buf = Globals.Buf;

			MenuItemList = new List<IMenuItem>();

			MenuItemList.Add(Globals.CreateInstance<IMenuItem>(x =>
			{
				x.SelectChar = (char)('1' + MenuItemList.Count);
				x.LineText = string.Format("{0}{1}. Edit many fields of an Artifact record.", Environment.NewLine, MenuItemList.Count + 1);
				x.SubMenu = Globals.CreateInstance<IEditArtifactRecordManyFieldsMenu>();
			}));

			MenuItemList.Add(Globals.CreateInstance<IMenuItem>(x =>
			{
				x.SelectChar = (char)('1' + MenuItemList.Count);
				x.LineText = string.Format("{0}{1}. Edit one field of an Artifact record.", Environment.NewLine, MenuItemList.Count + 1);
				x.SubMenu = Globals.CreateInstance<IEditArtifactRecordOneFieldMenu>();
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
