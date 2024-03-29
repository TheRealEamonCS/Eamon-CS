﻿
// DrinkBeerMenu.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Game.Attributes;
using Eamon.Game.Menus;
using EamonMH.Framework.Menus.ActionMenus;
using static EamonMH.Game.Plugin.Globals;

namespace EamonMH.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class DrinkBeerMenu : Menu, IDrinkBeerMenu
	{
		public override void Execute()
		{
			gOut.Print("{0}", gEngine.LineSep);

			gOut.Print("As you go over to the men, you feel a sword being thrust through your back and you hear someone say, \"You really must learn to follow directions!\"");

			gEngine.In.KeyPress(Buf);
		}

		public DrinkBeerMenu()
		{
			Buf = gEngine.Buf;
		}
	}
}
