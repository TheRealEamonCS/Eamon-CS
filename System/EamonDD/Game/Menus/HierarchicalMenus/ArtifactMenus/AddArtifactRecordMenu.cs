
// AddArtifactRecordMenu.cs

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
	public class AddArtifactRecordMenu : Menu, IAddArtifactRecordMenu
	{
		public override void PrintSubtitle()
		{
			Globals.DdMenu.PrintArtifactMenuSubtitle();
		}

		public AddArtifactRecordMenu()
		{
			Title = "ADD ARTIFACT RECORD MENU";

			Buf = Globals.Buf;

			MenuItemList = new List<IMenuItem>();

			MenuItemList.Add(Globals.CreateInstance<IMenuItem>(x =>
			{
				x.SelectChar = (char)('1' + MenuItemList.Count);
				x.LineText = string.Format("{0}{1}. Add an Artifact record by entering data manually.", Environment.NewLine, MenuItemList.Count + 1);
				x.SubMenu = Globals.CreateInstance<IAddArtifactRecordManualMenu>();
			}));

			MenuItemList.Add(Globals.CreateInstance<IMenuItem>(x =>
			{
				x.SelectChar = (char)('1' + MenuItemList.Count);
				x.LineText = string.Format("{0}{1}. Add an Artifact record by copying an old Artifact.", Environment.NewLine, MenuItemList.Count + 1);
				x.SubMenu = Globals.CreateInstance<IAddArtifactRecordCopyMenu>();
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
