
// ListConfigRecordMenu.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using Eamon;
using Eamon.Framework.Helpers;
using Eamon.Game.Attributes;
using Eamon.Game.Menus;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.PluginContext;

namespace EamonDD.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class ListConfigRecordMenu : Menu, IListConfigRecordMenu
	{
		public override void Execute()
		{
			RetCode rc;

			gOut.WriteLine();

			gEngine.PrintTitle("LIST CONFIG RECORD DETAILS", true);
			
			var helper = Globals.CreateInstance<IConfigHelper>(x =>
			{
				x.Record = Globals.Config;
			});
			
			helper.ListRecord(true, false, false, false, false, false);

			gOut.WriteLine();

			gOut.Print("{0}", Globals.LineSep);

			gOut.Write("{0}Press any key to continue or X to exit: ", Environment.NewLine);

			Buf.Clear();

			rc = Globals.In.ReadField(Buf, Constants.BufSize02, null, ' ', '\0', true, null, gEngine.ModifyCharToNullOrX, null, gEngine.IsCharAny);

			Debug.Assert(gEngine.IsSuccess(rc));

			if (Buf.Length > 0 && Buf[0] == 'X')
			{
				// do nothing
			}

			gOut.Print("{0}", Globals.LineSep);

			gOut.Print("Done listing Config record details.");
		}

		public ListConfigRecordMenu()
		{
			Buf = Globals.Buf;
		}
	}
}
