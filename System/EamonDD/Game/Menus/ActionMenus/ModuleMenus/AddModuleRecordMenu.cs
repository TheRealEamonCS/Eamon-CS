
// AddModuleRecordMenu.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using System.Text;
using Eamon;
using Eamon.Framework;
using Eamon.Framework.Helpers;
using Eamon.Game.Attributes;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.PluginContext;

namespace EamonDD.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class AddModuleRecordMenu : AddRecordManualMenu<IModule, IModuleHelper>, IAddModuleRecordMenu
	{
		public override void Execute()
		{
			IModule module;
			RetCode rc;

			if (Globals.Module == null)
			{
				gOut.WriteLine();

				gEngine.PrintTitle("ADD MODULE RECORD", true);

				if (!Globals.Config.GenerateUids && NewRecordUid == 0)
				{
					gOut.Write("{0}{1}", Environment.NewLine, gEngine.BuildPrompt(55, '\0', 0, "Enter the Uid of the Module record to add", null));

					Buf.Clear();

					rc = Globals.In.ReadField(Buf, Constants.BufSize01, null, '_', '\0', false, null, null, gEngine.IsCharDigit, null);

					Debug.Assert(gEngine.IsSuccess(rc));

					NewRecordUid = Convert.ToInt64(Buf.Trim().ToString());

					gOut.Print("{0}", Globals.LineSep);

					if (NewRecordUid > 0)
					{
						module = Globals.MODDB[NewRecordUid];

						if (module != null)
						{
							gOut.Print("Module record already exists.");

							goto Cleanup;
						}

						Globals.Database.ModuleTable.FreeUids.Remove(NewRecordUid);
					}
				}

				module = Globals.CreateInstance<IModule>(x =>
				{
					x.Uid = NewRecordUid;
				});
				
				var helper = Globals.CreateInstance<IModuleHelper>(x =>
				{
					x.Record = module;
				});
				
				helper.InputRecord(false, Globals.Config.FieldDesc);

				Globals.Thread.Sleep(150);

				gOut.Write("{0}Would you like to save this Module record (Y/N): ", Environment.NewLine);

				Buf.Clear();

				rc = Globals.In.ReadField(Buf, Constants.BufSize02, null, ' ', '\0', false, null, gEngine.ModifyCharToUpper, gEngine.IsCharYOrN, gEngine.IsCharYOrN);

				Debug.Assert(gEngine.IsSuccess(rc));

				Globals.Thread.Sleep(150);

				if (Buf.Length > 0 && Buf[0] == 'N')
				{
					module.Dispose();

					goto Cleanup;
				}

				rc = Globals.Database.AddModule(module);

				Debug.Assert(gEngine.IsSuccess(rc));

				Globals.ModulesModified = true;

				Globals.Module = module;
			}

		Cleanup:

			NewRecordUid = 0;
		}
	}
}
