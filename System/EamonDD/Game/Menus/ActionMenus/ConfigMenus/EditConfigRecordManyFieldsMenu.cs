
// EditConfigRecordManyFieldsMenu.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework.Helpers;
using Eamon.Game.Attributes;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.PluginContext;

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
				EditRecord = Globals.Config;
			}

			var editConfig01 = Globals.CloneInstance(EditRecord);

			Debug.Assert(editConfig01 != null);

			var helper = Globals.CreateInstance<IConfigHelper>(x =>
			{
				x.Record = editConfig01;
			});

			helper.InputRecord(true, Globals.Config.FieldDesc);

			CompareAndSave(editConfig01);

			EditRecord = null;
		}

		public EditConfigRecordManyFieldsMenu()
		{
			Buf = Globals.Buf;
		}
	}
}
