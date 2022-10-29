
// ListModuleRecordMenu.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using Eamon;
using Eamon.Framework.Helpers;
using Eamon.Game.Attributes;
using Eamon.Game.Menus;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.Globals;

namespace EamonDD.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class ListModuleRecordMenu : Menu, IListModuleRecordMenu
	{
		public override void Execute()
		{
			RetCode rc;

			if (gEngine.Module != null)
			{
				gOut.WriteLine();

				gEngine.PrintTitle("LIST MODULE RECORD DETAILS", true);
				
				var helper = gEngine.CreateInstance<IModuleHelper>(x =>
				{
					x.Record = gEngine.Module;
				});
				
				helper.ListRecord(true, gEngine.Config.ShowDesc, gEngine.Config.ResolveEffects, true, false, false);

				gOut.WriteLine();

				gOut.Print("{0}", gEngine.LineSep);

				gOut.Write("{0}Press any key to continue or X to exit: ", Environment.NewLine);

				Buf.Clear();

				rc = gEngine.In.ReadField(Buf, gEngine.BufSize02, null, ' ', '\0', true, null, gEngine.ModifyCharToNullOrX, null, gEngine.IsCharAny);

				Debug.Assert(gEngine.IsSuccess(rc));

				if (Buf.Length > 0 && Buf[0] == 'X')
				{
					// do nothing
				}

				gOut.Print("{0}", gEngine.LineSep);

				gOut.Print("Done listing Module record details.");
			}
		}

		public ListModuleRecordMenu()
		{
			Buf = gEngine.Buf;
		}
	}
}
