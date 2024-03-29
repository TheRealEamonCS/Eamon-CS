﻿
// ArtifactRecordUtilitiesMenu.cs

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
	public class ArtifactRecordUtilitiesMenu : Menu, IArtifactRecordUtilitiesMenu
	{
		public override void PrintSubtitle()
		{
			gEngine.DdMenu.PrintArtifactMenuSubtitle();
		}

		public ArtifactRecordUtilitiesMenu()
		{
			Title = "ARTIFACT RECORD UTILITIES MENU";

			Buf = gEngine.Buf;

			MenuItemList = new List<IMenuItem>();

			MenuItemList.Add(gEngine.CreateInstance<IMenuItem>(x =>
			{
				x.SelectChar = (char)('1' + MenuItemList.Count);
				x.LineText = string.Format("{0}{1}. Generate dummy Artifact records.", Environment.NewLine, MenuItemList.Count + 1);
				x.SubMenu = gEngine.CreateInstance<IGenerateDummyArtifactRecordsMenu>();
			}));

			MenuItemList.Add(gEngine.CreateInstance<IMenuItem>(x =>
			{
				x.SelectChar = (char)('1' + MenuItemList.Count);
				x.LineText = string.Format("{0}{1}. Generate dead body Artifact records.", Environment.NewLine, MenuItemList.Count + 1);
				x.SubMenu = gEngine.CreateInstance<IGenerateDeadBodyArtifactRecordsMenu>();
			}));

			if (gEngine.IsAdventureFilesetLoaded())
			{
				MenuItemList.Add(gEngine.CreateInstance<IMenuItem>(x =>
				{
					x.SelectChar = (char)('1' + MenuItemList.Count);
					x.LineText = string.Format("{0}{1}. Analyse Artifact record interdependencies.", Environment.NewLine, MenuItemList.Count + 1);
					x.SubMenu = gEngine.CreateInstance<IAnalyseArtifactRecordInterdependenciesMenu>();
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
