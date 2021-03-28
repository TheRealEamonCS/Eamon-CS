
// EditFilesetRecordManyFieldsMenu.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework;
using Eamon.Framework.Helpers;
using Eamon.Game.Attributes;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.PluginContext;

namespace EamonDD.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class EditFilesetRecordManyFieldsMenu : EditRecordManyFieldsMenu<IFileset, IFilesetHelper>, IEditFilesetRecordManyFieldsMenu
	{
		public override void UpdateGlobals()
		{
			Globals.FilesetsModified = true;
		}

		public EditFilesetRecordManyFieldsMenu()
		{
			Title = "EDIT FILESET RECORD FIELDS";

			RecordTable = Globals.Database.FilesetTable;

			RecordTypeName = "Fileset";
		}
	}
}
