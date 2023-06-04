
// EditConfigRecordMenu.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using Eamon;
using Eamon.Framework;
using Eamon.Game.Menus;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.Globals;

namespace EamonDD.Game.Menus.ActionMenus
{
	public abstract class EditConfigRecordMenu : Menu, IEditConfigRecordMenu
	{
		public virtual IConfig EditRecord { get; set; }

		public virtual void CompareAndSave(IConfig editConfig01)
		{
			RetCode rc;

			Debug.Assert(editConfig01 != null);

			if (!gEngine.CompareInstances(EditRecord, editConfig01))
			{
				gOut.Write("{0}Would you like to save this updated Config record (Y/N): ", Environment.NewLine);

				Buf.Clear();

				rc = gEngine.In.ReadField(Buf, gEngine.BufSize02, null, ' ', '\0', false, null, gEngine.ModifyCharToUpper, gEngine.IsCharYOrN, gEngine.IsCharYOrN);

				Debug.Assert(gEngine.IsSuccess(rc));

				if (Buf.Length > 0 && Buf[0] == 'N')
				{
					goto Cleanup;
				}

				var config = gEngine.Database.RemoveConfig(EditRecord.Uid);

				if (config != null)
				{
					rc = gEngine.Database.AddConfig(editConfig01);

					Debug.Assert(gEngine.IsSuccess(rc));
				}

				if (gEngine.Config == EditRecord)
				{
					gEngine.Config = editConfig01;
				}

				gEngine.ConfigsModified = true;
			}
			else
			{
				gOut.Print("Config record not modified.");
			}

		Cleanup:

			;
		}
	}
}
