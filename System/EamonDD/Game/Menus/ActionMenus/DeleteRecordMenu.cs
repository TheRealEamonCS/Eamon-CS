
// DeleteRecordMenu.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using System.Text;
using Eamon;
using Eamon.Framework;
using Eamon.Framework.Helpers.Generic;
using Eamon.Framework.Primitive.Enums;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.PluginContext;

namespace EamonDD.Game.Menus.ActionMenus
{
	public abstract class DeleteRecordMenu<T, U> : RecordMenu<T>, IDeleteRecordMenu<T> where T : class, IGameBase where U : class, IHelper<T>
	{
		public override void Execute()
		{
			RetCode rc;

			gOut.WriteLine();

			gEngine.PrintTitle(Title, true);

			gOut.Write("{0}{1}", Environment.NewLine, gEngine.BuildPrompt(55, '\0', 0, string.Format("Enter the Uid of the {0} record to delete", RecordTypeName), null));

			Buf.Clear();

			rc = Globals.In.ReadField(Buf, Constants.BufSize01, null, '_', '\0', false, null, null, gEngine.IsCharDigit, null);

			Debug.Assert(gEngine.IsSuccess(rc));

			var recordUid = Convert.ToInt64(Buf.Trim().ToString());

			gOut.Print("{0}", Globals.LineSep);

			var record = RecordTable.FindRecord(recordUid);

			if (record == null)
			{
				gOut.Print("{0} record not found.", RecordTypeName);

				goto Cleanup;
			}

			var character = record as ICharacter;

			if (character != null && character.Status != Status.Alive && character.Status != Status.Dead)
			{
				gOut.Print("{0} record Status not marked as Alive or Dead.", RecordTypeName);

				goto Cleanup;
			}

			var helper = Globals.CreateInstance<U>(x =>
			{
				x.Record = record;
			});
			
			helper.ListRecord(true, true, false, true, false, false);

			PrintPostListLineSep();

			Globals.Thread.Sleep(150);

			gOut.Write("{0}Would you like to delete this {1} record (Y/N): ", Environment.NewLine, RecordTypeName);

			Buf.Clear();

			rc = Globals.In.ReadField(Buf, Constants.BufSize02, null, ' ', '\0', false, null, gEngine.ModifyCharToUpper, gEngine.IsCharYOrN, gEngine.IsCharYOrN);

			Debug.Assert(gEngine.IsSuccess(rc));

			Globals.Thread.Sleep(150);

			if (Buf.Length > 0 && Buf[0] == 'N')
			{
				goto Cleanup;
			}

			record = RecordTable.RemoveRecord(recordUid);

			Debug.Assert(record != null);

			record.Dispose();

			UpdateGlobals();

		Cleanup:

			;
		}
	}
}
