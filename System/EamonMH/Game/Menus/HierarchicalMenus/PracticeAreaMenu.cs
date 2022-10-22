
// PracticeAreaMenu.cs

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
	public class PracticeAreaMenu : Menu, IPracticeAreaMenu
	{
		public override void PrintSubtitle()
		{
			gEngine.MhMenu.PrintPracticeAreaMenuSubtitle();
		}

		public PracticeAreaMenu()
		{
			Title = "PRACTICE AREA";

			Buf = gEngine.Buf;

			MenuItemList = new List<IMenuItem>();

			MenuItemList.Add(gEngine.CreateInstance<IMenuItem>(x =>
			{
				x.SelectChar = (char)('1' + MenuItemList.Count);
				x.LineText = string.Format("{0}{1}. Practice your spells with a Licensed Wizard.", Environment.NewLine, MenuItemList.Count + 1);
				x.SubMenu = gEngine.CreateInstance<ILicensedWizardMenu>();
			}));

			MenuItemList.Add(gEngine.CreateInstance<IMenuItem>(x =>
			{
				x.SelectChar = (char)('1' + MenuItemList.Count);
				x.LineText = string.Format("{0}{1}. Weapon Wielding 101 with Don Diego.", Environment.NewLine, MenuItemList.Count + 1);
				x.SubMenu = gEngine.CreateInstance<IDonDiegoMenu>();
			}));

			MenuItemList.Add(gEngine.CreateInstance<IMenuItem>(x =>
			{
				x.SelectChar = (char)('1' + MenuItemList.Count);
				x.LineText = string.Format("{0}{1}. Master the art of Wearing Your Clothes Properly.", Environment.NewLine, MenuItemList.Count + 1);
				x.SubMenu = gEngine.CreateInstance<IMasterArmorMenu>();
			}));

			MenuItemList.Add(gEngine.CreateInstance<IMenuItem>(x =>
			{
				x.SelectChar = (char)('1' + MenuItemList.Count);
				x.LineText = string.Format("{0}{1}. Talk to the man in the Info Booth.", Environment.NewLine, MenuItemList.Count + 1);
				x.SubMenu = gEngine.CreateInstance<IInfoBoothMenu>();
			}));

			MenuItemList.Add(gEngine.CreateInstance<IMenuItem>(x =>
			{
				x.SelectChar = (char)('1' + MenuItemList.Count);
				x.LineText = string.Format("{0}{1}. Examine your abilities.", Environment.NewLine, MenuItemList.Count + 1);
				x.SubMenu = gEngine.CreateInstance<IExamineAbilitiesMenu>();
			}));

			MenuItemList.Add(gEngine.CreateInstance<IMenuItem>(x =>
			{
				x.SelectChar = 'X';
				x.LineText = string.Format("{0}X. Head north into the Village.{0}", Environment.NewLine);
				x.SubMenu = null;
			}));
		}
	}
}
