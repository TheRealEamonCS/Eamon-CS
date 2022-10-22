
// VillageMenu.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using Eamon.Framework.Menus;
using Eamon.Game.Attributes;
using Eamon.Game.Menus;
using EamonMH.Framework.Menus.ActionMenus;
using EamonMH.Framework.Menus.HierarchicalMenus;
using static EamonMH.Game.Plugin.Globals;

namespace EamonMH.Game.Menus.HierarchicalMenus
{
	[ClassMappings]
	public class VillageMenu : Menu, IVillageMenu
	{
		public override void PrintSubtitle()
		{
			gEngine.MhMenu.PrintVillageMenuSubtitle();
		}

		public VillageMenu()
		{
			Title = "VILLAGE";

			Buf = gEngine.Buf;

			MenuItemList = new List<IMenuItem>();

			MenuItemList.Add(gEngine.CreateInstance<IMenuItem>(x =>
			{
				x.SelectChar = (char)('1' + MenuItemList.Count);
				x.LineText = string.Format("{0}{1}. Try your luck at the Casino Schmitt.", Environment.NewLine, MenuItemList.Count + 1);
				x.SubMenu = gEngine.CreateInstance<ICasinoSchmittMenu>();
			}));

			MenuItemList.Add(gEngine.CreateInstance<IMenuItem>(x =>
			{
				x.SelectChar = (char)('1' + MenuItemList.Count);
				x.LineText = string.Format("{0}{1}. Visit the Good Witch's Emporium.", Environment.NewLine, MenuItemList.Count + 1);
				x.SubMenu = gEngine.CreateInstance<IGoodWitchMenu>();
			}));

			MenuItemList.Add(gEngine.CreateInstance<IMenuItem>(x =>
			{
				x.SelectChar = (char)('1' + MenuItemList.Count);
				x.LineText = string.Format("{0}{1}. Browse exotic weapons at Grendel's Smithy.", Environment.NewLine, MenuItemList.Count + 1);
				x.SubMenu = gEngine.CreateInstance<IGrendelSmithyMenu>();
			}));

			MenuItemList.Add(gEngine.CreateInstance<IMenuItem>(x =>
			{
				x.SelectChar = (char)('1' + MenuItemList.Count);
				x.LineText = string.Format("{0}{1}. Check out the statue in the Village Square.", Environment.NewLine, MenuItemList.Count + 1);
				x.SubMenu = gEngine.CreateInstance<IVillageSquareMenu>();
			}));

			MenuItemList.Add(gEngine.CreateInstance<IMenuItem>(x =>
			{
				x.SelectChar = (char)('1' + MenuItemList.Count);
				x.LineText = string.Format("{0}{1}. Examine your abilities.", Environment.NewLine, MenuItemList.Count + 1);
				x.SubMenu = gEngine.CreateInstance<IExamineAbilitiesMenu>();
			}));

			MenuItemList.Add(gEngine.CreateInstance<IMenuItem>(x =>
			{
				x.SelectChar = (char)('1' + MenuItemList.Count);
				x.LineText = string.Format("{0}{1}. Wander south into the Practice Area.", Environment.NewLine, MenuItemList.Count + 1);
				x.SubMenu = gEngine.CreateInstance<IPracticeAreaMenu>();
			}));

			MenuItemList.Add(gEngine.CreateInstance<IMenuItem>(x =>
			{
				x.SelectChar = 'X';
				x.LineText = string.Format("{0}X. Head north into the Main Hall.{0}", Environment.NewLine);
				x.SubMenu = null;
			}));
		}
	}
}
