
// OuterChamberMenu.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using Eamon;
using Eamon.Framework.Menus;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using Eamon.Game.Menus;
using EamonMH.Framework.Menus.ActionMenus;
using static EamonMH.Game.Plugin.PluginContext;

namespace EamonMH.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class OuterChamberMenu : Menu, IOuterChamberMenu
	{
		public override void Execute()
		{
			RetCode rc;
			IMenu menu;

			gOut.WriteLine("{0}{1}{0}", Environment.NewLine, Globals.LineSep);

			gEngine.PrintTitle("WELCOME TO THE", false);

			gOut.WriteLine();

			gEngine.PrintTitle("EAMON CS COMPUTERIZED FANTASY GAMING SYSTEM", false);

			Buf.SetFormat("{0}{0}You are in the outer chamber of the hall of the Guild of Free Adventurers.  Many men and women are guzzling beer and there is loud singing and laughter.{0}", Environment.NewLine);

			Buf.AppendPrint("On the north side of the chamber is a cubbyhole with a desk.  Over the desk is a sign which says:  REGISTER HERE OR ELSE!");

			Buf.AppendPrint("Do you go over to the desk or join the men drinking beer?");

			gOut.Write("{0}", Buf);

			gOut.Print("{0}", Globals.LineSep);

			gOut.Write("{0}Press D for desk or M for men: ", Environment.NewLine);

			Buf.Clear();

			rc = Globals.In.ReadField(Buf, Constants.BufSize02, null, ' ', '\0', false, null, gEngine.ModifyCharToUpper, gEngine.IsCharDOrM, gEngine.IsCharDOrM);

			Debug.Assert(gEngine.IsSuccess(rc));

			Globals.Thread.Sleep(150);

			if (Buf.Length > 0 && Buf[0] == 'D')
			{
				menu = Globals.CreateInstance<IRegistrationDeskMenu>();
			}
			else
			{
				menu = Globals.CreateInstance<IDrinkBeerMenu>();
			}

			menu.Execute();
		}

		public OuterChamberMenu()
		{
			Buf = Globals.Buf;
		}
	}
}
