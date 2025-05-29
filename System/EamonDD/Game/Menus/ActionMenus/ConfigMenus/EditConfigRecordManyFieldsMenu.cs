
// EditConfigRecordManyFieldsMenu.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework.Helpers;
using Eamon.Game.Attributes;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.Globals;

namespace EamonDD.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class EditConfigRecordManyFieldsMenu : EditConfigRecordMenu, IEditConfigRecordManyFieldsMenu
	{
		public override void Execute()
		{
			gOut.WriteLine();

			gEngine.PrintTitle("EDIT CONFIG RECORD FIELDS", true);

			if (EditRecord == null)
			{
				EditRecord = gEngine.Config;
			}

			var editConfig01 = gEngine.CloneInstance(EditRecord);

			Debug.Assert(editConfig01 != null);

			var helper = gEngine.CreateInstance<IConfigHelper>(x =>
			{
				x.RecordTable = gDatabase.ConfigTable;
				
				x.Record = editConfig01;
			});

			helper.InputRecord(true, gEngine.Config.FieldDesc);

			CompareAndSave(editConfig01);

			EditRecord = null;
		}

		public EditConfigRecordManyFieldsMenu()
		{
			Buf = gEngine.Buf;
		}
	}
}
