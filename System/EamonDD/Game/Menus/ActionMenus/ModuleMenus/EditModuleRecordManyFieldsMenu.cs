
// EditModuleRecordManyFieldsMenu.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework.Helpers;
using Eamon.Game.Attributes;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.Globals;

namespace EamonDD.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class EditModuleRecordManyFieldsMenu : EditModuleRecordMenu, IEditModuleRecordManyFieldsMenu
	{
		public override void Execute()
		{
			if (EditRecord != null || gEngine.Module != null)
			{
				gOut.WriteLine();

				gEngine.PrintTitle("EDIT MODULE RECORD FIELDS", true);

				if (EditRecord == null)
				{
					EditRecord = gEngine.Module;
				}

				var editModule01 = gEngine.CloneInstance(EditRecord);

				Debug.Assert(editModule01 != null);

				var helper = gEngine.CreateInstance<IModuleHelper>(x =>
				{
					x.RecordTable = gDatabase.ModuleTable;
					
					x.Record = editModule01;
				});

				helper.InputRecord(true, gEngine.Config.FieldDesc);

				CompareAndSave(editModule01);
			}

			EditRecord = null;
		}

		public EditModuleRecordManyFieldsMenu()
		{
			Buf = gEngine.Buf;
		}
	}
}
