﻿
// EditConfigRecordOneFieldMenu.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using System.Text;
using Eamon;
using Eamon.Framework.Helpers;
using Eamon.Game.Attributes;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.Globals;

namespace EamonDD.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class EditConfigRecordOneFieldMenu : EditConfigRecordMenu, IEditConfigRecordOneFieldMenu
	{
		public virtual string EditFieldName { get; set; }

		public override void Execute()
		{
			RetCode rc;

			gOut.WriteLine();

			gEngine.PrintTitle("EDIT CONFIG RECORD FIELD", true);

			if (EditRecord == null)
			{
				EditRecord = gEngine.Config;
			}

			var editConfig01 = gEngine.CloneInstance(EditRecord);

			Debug.Assert(editConfig01 != null);
			
			var helper = gEngine.CreateInstance<IConfigHelper>(x =>
			{
				x.Record = editConfig01;
			});
			
			string editFieldName01 = null;

			if (string.IsNullOrWhiteSpace(EditFieldName))
			{
				helper.ListRecord(true, true, false, true, true, true);

				gOut.WriteLine();

				gOut.Print("{0}", gEngine.LineSep);

				gOut.Write("{0}{1}", Environment.NewLine, gEngine.BuildPrompt(47, '\0', 0, "Enter the number of the field to edit", "0"));

				Buf.Clear();

				rc = gEngine.In.ReadField(Buf, gEngine.BufSize01, null, '_', '\0', true, "0", null, gEngine.IsCharDigit, null);

				Debug.Assert(gEngine.IsSuccess(rc));

				var fieldNum = Convert.ToInt64(Buf.Trim().ToString());

				editFieldName01 = helper.GetFieldName(fieldNum);

				if (string.IsNullOrWhiteSpace(editFieldName01))
				{
					goto Cleanup;
				}

				gOut.Print("{0}", gEngine.LineSep);
			}
			else
			{
				editFieldName01 = EditFieldName;
			}

			helper.EditRec = true;
			helper.EditField = true;
			helper.FieldDesc = gEngine.Config.FieldDesc;

			helper.InputField(editFieldName01);

			CompareAndSave(editConfig01);

		Cleanup:

			EditRecord = null;

			EditFieldName = null;
		}

		public EditConfigRecordOneFieldMenu()
		{
			Buf = gEngine.Buf;
		}
	}
}
