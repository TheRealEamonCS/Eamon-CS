
// ConvertEamonDeluxeAdventureMenu.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Game.Attributes;
using Eamon.Game.Menus;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.PluginContext;

namespace EamonDD.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class ConvertEamonDeluxeAdventureMenu : Menu, IConvertEamonDeluxeAdventureMenu
	{
		public override void Execute()
		{
			gOut.WriteLine();

			gEngine.PrintTitle("CONVERT EAMON DELUXE ADVENTURE", true);

			Debug.Assert(gEngine.IsAdventureFilesetLoaded());
		}

		public ConvertEamonDeluxeAdventureMenu()
		{

		}
	}
}
