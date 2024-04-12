
// MainHallMenu.cs

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
	public class MainHallMenu : Menu, IMainHallMenu
	{
		public override void PrintSubtitle()
		{
			gEngine.MhMenu.PrintMainHallMenuSubtitle();
		}

		public override bool ShouldBreakMenuLoop()
		{
			return gEngine.GoOnAdventure;
		}

		public override void Shutdown()
		{
			if (!gEngine.GoOnAdventure)
			{
				gOut.Print("{0}", gEngine.LineSep);

				gOut.Print("As you leave the hall, the Irishman comes up to you, slaps you on the back and says, \"Y'all come back real soon, ya heah?\"");

				gEngine.In.KeyPress(Buf);
			}
		}

		public MainHallMenu()
		{
			Title = "MAIN HALL";

			Buf = gEngine.Buf;

			MenuItemList = new List<IMenuItem>();

			MenuItemList.Add(gEngine.CreateInstance<IMenuItem>(x =>
			{
				x.SelectChar = (char)('1' + MenuItemList.Count);
				x.LineText = string.Format("{0}{1}. Go on an adventure.", Environment.NewLine, MenuItemList.Count + 1);
				x.SubMenu = gEngine.CreateInstance<IGoOnAdventureMenu>();
			}));

			MenuItemList.Add(gEngine.CreateInstance<IMenuItem>(x =>
			{
				x.SelectChar = (char)('1' + MenuItemList.Count);
				x.LineText = string.Format("{0}{1}. Visit the Weapons and Armor Shop.", Environment.NewLine, MenuItemList.Count + 1);
				x.SubMenu = gEngine.CreateInstance<IMarcosCavielliMenu>();
			}));

			MenuItemList.Add(gEngine.CreateInstance<IMenuItem>(x =>
			{
				x.SelectChar = (char)('1' + MenuItemList.Count);
				x.LineText = string.Format("{0}{1}. Hire a Wizard to teach you some spells.", Environment.NewLine, MenuItemList.Count + 1);
				x.SubMenu = gEngine.CreateInstance<IHokasTokasMenu>();
			}));

			MenuItemList.Add(gEngine.CreateInstance<IMenuItem>(x =>
			{
				x.SelectChar = (char)('1' + MenuItemList.Count);
				x.LineText = string.Format("{0}{1}. Find the banker to deposit or withdraw some gold.", Environment.NewLine, MenuItemList.Count + 1);
				x.SubMenu = gEngine.CreateInstance<IIanMcFennyMenu>();
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
				x.LineText = string.Format("{0}{1}. Enter the Village.", Environment.NewLine, MenuItemList.Count + 1);
				x.SubMenu = gEngine.CreateInstance<IVillageMenu>();
			}));

			MenuItemList.Add(gEngine.CreateInstance<IMenuItem>(x =>
			{
				x.SelectChar = 'X';
				x.LineText = string.Format("{0}X. Temporarily leave the universe.{0}", Environment.NewLine);
				x.SubMenu = null;
			}));
		}
	}
}
