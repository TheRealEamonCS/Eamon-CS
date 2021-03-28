
// DeleteModuleRecordMenu.cs

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
	public class DeleteModuleRecordMenu : Menu, IDeleteModuleRecordMenu
	{
		public override void Execute()
		{
			RetCode rc;

			if (Globals.Module != null)
			{
				gOut.WriteLine();

				gEngine.PrintTitle("DELETE MODULE RECORD", true);
				
				var helper = Globals.CreateInstance<IModuleHelper>(x =>
				{
					x.Record = Globals.Module;
				});
				
				helper.ListRecord(true, true, false, true, false, false);

				gOut.WriteLine();

				gOut.Print("{0}", Globals.LineSep);

				gOut.Write("{0}Would you like to delete this Module record (Y/N): ", Environment.NewLine);

				Buf.Clear();

				rc = Globals.In.ReadField(Buf, Constants.BufSize02, null, ' ', '\0', false, null, gEngine.ModifyCharToUpper, gEngine.IsCharYOrN, gEngine.IsCharYOrN);

				Debug.Assert(gEngine.IsSuccess(rc));

				Globals.Thread.Sleep(150);

				if (Buf.Length > 0 && Buf[0] == 'N')
				{
					goto Cleanup;
				}

				var module = Globals.Database.RemoveModule(Globals.Module.Uid);

				Debug.Assert(module != null);

				module.Dispose();

				Globals.ModulesModified = true;

				Globals.Module = null;
			}

		Cleanup:

			;
		}

		public DeleteModuleRecordMenu()
		{
			Buf = Globals.Buf;
		}
	}
}
