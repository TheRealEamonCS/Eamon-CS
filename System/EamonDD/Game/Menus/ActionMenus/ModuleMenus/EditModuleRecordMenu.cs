
// EditModuleRecordMenu.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using Eamon;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Menus;
using Eamon.Game.Utilities;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.PluginContext;

namespace EamonDD.Game.Menus.ActionMenus
{
	public abstract class EditModuleRecordMenu : Menu, IEditModuleRecordMenu
	{
		public virtual IModule EditRecord { get; set; }

		public virtual void CompareAndSave(IModule editModule01)
		{
			RetCode rc;

			Debug.Assert(editModule01 != null);

			Globals.Thread.Sleep(150);

			if (!Globals.CompareInstances(EditRecord, editModule01))
			{
				gOut.Write("{0}Would you like to save this updated Module record (Y/N): ", Environment.NewLine);

				Buf.Clear();

				rc = Globals.In.ReadField(Buf, Constants.BufSize02, null, ' ', '\0', false, null, gEngine.ModifyCharToUpper, gEngine.IsCharYOrN, gEngine.IsCharYOrN);

				Debug.Assert(gEngine.IsSuccess(rc));

				Globals.Thread.Sleep(150);

				if (Buf.Length > 0 && Buf[0] == 'N')
				{
					goto Cleanup;
				}

				if (EditRecord.NumDirs == 12 && editModule01.NumDirs == 6)
				{
					var directionValues = EnumUtil.GetValues<Direction>();

					foreach (var room in Globals.Database.RoomTable.Records)
					{
						for (var i = editModule01.NumDirs; i < EditRecord.NumDirs; i++)
						{
							var dv = directionValues[(int)i];

							room.SetDirs(dv, 0);
						}

						Globals.RoomsModified = true;
					}
				}

				var module = Globals.Database.RemoveModule(EditRecord.Uid);

				Debug.Assert(module != null);

				rc = Globals.Database.AddModule(editModule01);

				Debug.Assert(gEngine.IsSuccess(rc));

				if (Globals.Module == EditRecord)
				{
					Globals.Module = editModule01;
				}

				Globals.ModulesModified = true;
			}
			else
			{
				gOut.Print("Module record not modified.");
			}

		Cleanup:

			;
		}
	}
}
