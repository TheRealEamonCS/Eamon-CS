
// AddRecordCopyMenu.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using System.Text;
using Eamon;
using Eamon.Framework;
using Eamon.Framework.Helpers.Generic;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.PluginContext;

namespace EamonDD.Game.Menus.ActionMenus
{
	public abstract class AddRecordCopyMenu<T, U> : RecordMenu<T>, IAddRecordCopyMenu<T> where T : class, IGameBase where U : class, IHelper<T>
	{
		public override void Execute()
		{
			RetCode rc;

			gOut.WriteLine();

			gEngine.PrintTitle(Title, true);

			gOut.Write("{0}{1}", Environment.NewLine, gEngine.BuildPrompt(55, '\0', 0, string.Format("Enter the Uid of the {0} record to copy", RecordTypeName), "1"));

			Buf.Clear();

			rc = Globals.In.ReadField(Buf, Constants.BufSize01, null, '_', '\0', true, "1", null, gEngine.IsCharDigit, null);

			Debug.Assert(gEngine.IsSuccess(rc));

			var recordUid = Convert.ToInt64(Buf.Trim().ToString());

			gOut.Print("{0}", Globals.LineSep);

			var record = RecordTable.FindRecord(recordUid);

			if (record == null)
			{
				gOut.Print("{0} record not found.", RecordTypeName);

				goto Cleanup;
			}

			var record01 = Globals.CloneInstance(record);

			Debug.Assert(record01 != null);

			if (!Globals.Config.GenerateUids)
			{
				gOut.Write("{0}{1}", Environment.NewLine, gEngine.BuildPrompt(55, '\0', 0, string.Format("Enter the Uid of the {0} record copy", RecordTypeName), null));

				Buf.Clear();

				rc = Globals.In.ReadField(Buf, Constants.BufSize01, null, '_', '\0', false, null, null, gEngine.IsCharDigit, null);

				Debug.Assert(gEngine.IsSuccess(rc));

				recordUid = Convert.ToInt64(Buf.Trim().ToString());

				gOut.Print("{0}", Globals.LineSep);

				if (recordUid > 0)
				{
					record = RecordTable.FindRecord(recordUid);

					if (record != null)
					{
						gOut.Print("{0} record already exists.", RecordTypeName);

						goto Cleanup;
					}

					RecordTable.FreeUids.Remove(recordUid);

					record01.Uid = recordUid;

					record01.IsUidRecycled = false;
				}
				else
				{
					record01.Uid = RecordTable.GetRecordUid();

					record01.IsUidRecycled = true;
				}
			}
			else
			{
				record01.Uid = RecordTable.GetRecordUid();

				record01.IsUidRecycled = true;
			}
			
			var helper = Globals.CreateInstance<U>(x =>
			{
				x.Record = record01;
			});
			
			helper.ListRecord(true, true, false, true, false, false);

			PrintPostListLineSep();

			gOut.Write("{0}Would you like to save this {1} record (Y/N): ", Environment.NewLine, RecordTypeName);

			Buf.Clear();

			rc = Globals.In.ReadField(Buf, Constants.BufSize02, null, ' ', '\0', false, null, gEngine.ModifyCharToUpper, gEngine.IsCharYOrN, gEngine.IsCharYOrN);

			Debug.Assert(gEngine.IsSuccess(rc));

			Globals.Thread.Sleep(150);

			if (Buf.Length > 0 && Buf[0] == 'N')
			{
				record01.Dispose();

				goto Cleanup;
			}

			rc = RecordTable.AddRecord(record01);

			Debug.Assert(gEngine.IsSuccess(rc));

			UpdateGlobals();

		Cleanup:

			;
		}
	}
}
