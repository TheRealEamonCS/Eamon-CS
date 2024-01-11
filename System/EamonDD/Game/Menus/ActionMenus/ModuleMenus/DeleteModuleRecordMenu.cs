
// DeleteModuleRecordMenu.cs

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
	public class DeleteModuleRecordMenu : Menu, IDeleteModuleRecordMenu
	{
		public override void Execute()
		{
			RetCode rc;

			if (gEngine.Module != null)
			{
				gOut.WriteLine();

				gEngine.PrintTitle("DELETE MODULE RECORD", true);
				
				var helper = gEngine.CreateInstance<IModuleHelper>(x =>
				{
					x.Record = gEngine.Module;
				});
				
				helper.ListRecord(true, true, false, true, false, false);

				gOut.WriteLine();

				gOut.Print("{0}", gEngine.LineSep);

				gOut.Write("{0}Would you like to delete this Module record (Y/N): ", Environment.NewLine);

				Buf.Clear();

				rc = gEngine.In.ReadField(Buf, gEngine.BufSize02, null, ' ', '\0', false, null, gEngine.ModifyCharToUpper, gEngine.IsCharYOrN, gEngine.IsCharYOrN);

				Debug.Assert(gEngine.IsSuccess(rc));

				gEngine.Thread.Sleep(150);

				if (Buf.Length > 0 && Buf[0] == 'N')
				{
					goto Cleanup;
				}

				var module = gDatabase.RemoveModule(gEngine.Module.Uid);

				Debug.Assert(module != null);

				module.Dispose();

				gEngine.ModulesModified = true;

				gEngine.Module = null;
			}

		Cleanup:

			;
		}

		public DeleteModuleRecordMenu()
		{
			Buf = gEngine.Buf;
		}
	}
}
