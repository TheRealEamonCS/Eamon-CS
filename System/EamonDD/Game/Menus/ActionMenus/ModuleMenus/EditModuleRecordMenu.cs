
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
using static EamonDD.Game.Plugin.Globals;

namespace EamonDD.Game.Menus.ActionMenus
{
	public abstract class EditModuleRecordMenu : Menu, IEditModuleRecordMenu
	{
		public virtual IModule EditRecord { get; set; }

		public virtual void CompareAndSave(IModule editModule01)
		{
			RetCode rc;

			Debug.Assert(editModule01 != null);

			gEngine.Thread.Sleep(150);

			if (!gEngine.CompareInstances(EditRecord, editModule01))
			{
				gOut.Write("{0}Would you like to save this updated Module record (Y/N): ", Environment.NewLine);

				Buf.Clear();

				rc = gEngine.In.ReadField(Buf, gEngine.BufSize02, null, ' ', '\0', false, null, gEngine.ModifyCharToUpper, gEngine.IsCharYOrN, gEngine.IsCharYOrN);

				Debug.Assert(gEngine.IsSuccess(rc));

				gEngine.Thread.Sleep(150);

				if (Buf.Length > 0 && Buf[0] == 'N')
				{
					goto Cleanup;
				}

				if (EditRecord.NumDirs == 12 && editModule01.NumDirs == 6)
				{
					var directionValues = EnumUtil.GetValues<Direction>();

					foreach (var room in gEngine.Database.RoomTable.Records)
					{
						for (var i = editModule01.NumDirs; i < EditRecord.NumDirs; i++)
						{
							var dv = directionValues[(int)i];

							room.SetDir(dv, 0);
						}

						gEngine.RoomsModified = true;
					}
				}

				var module = gEngine.Database.RemoveModule(EditRecord.Uid);

				Debug.Assert(module != null);

				rc = gEngine.Database.AddModule(editModule01);

				Debug.Assert(gEngine.IsSuccess(rc));

				if (gEngine.Module == EditRecord)
				{
					gEngine.Module = editModule01;
				}

				gEngine.ModulesModified = true;
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
