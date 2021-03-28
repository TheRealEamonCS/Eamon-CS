
// ListRecordNameMenu.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using Eamon;
using Eamon.Framework;
using Eamon.Framework.Helpers.Generic;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.PluginContext;

namespace EamonDD.Game.Menus.ActionMenus
{
	public abstract class ListRecordNameMenu<T, U> : RecordMenu<T>, IListRecordNameMenu<T> where T : class, IGameBase where U : class, IHelper<T>
	{
		public override void Execute()
		{
			RetCode rc;

			var nlFlag = false;

			gOut.WriteLine();

			gEngine.PrintTitle(Title, true);

			var helper = Globals.CreateInstance<U>();

			var j = RecordTable.GetRecordsCount();

			var i = 0;

			foreach (var record in RecordTable.Records)
			{
				helper.Record = record;

				helper.ListRecord(false, false, false, false, false, false);

				nlFlag = true;

				if ((i != 0 && (i % (Constants.NumRows - 8)) == 0) || i == j - 1)
				{
					nlFlag = false;

					PrintPostListLineSep();

					gOut.Write("{0}Press any key to continue or X to exit: ", Environment.NewLine);

					Buf.Clear();

					rc = Globals.In.ReadField(Buf, Constants.BufSize02, null, ' ', '\0', true, null, gEngine.ModifyCharToNullOrX, null, gEngine.IsCharAny);

					Debug.Assert(gEngine.IsSuccess(rc));

					gOut.Print("{0}", Globals.LineSep);

					if (Buf.Length > 0 && Buf[0] == 'X')
					{
						break;
					}
				}

				i++;
			}

			if (nlFlag)
			{
				gOut.WriteLine();
			}

			gOut.Print("Done listing {0} record names.", RecordTypeName);
		}
	}
}
