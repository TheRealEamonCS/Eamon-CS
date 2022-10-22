
// EditFilesetRecordManyFieldsMenu.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework;
using Eamon.Framework.Helpers;
using Eamon.Game.Attributes;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.Globals;

namespace EamonDD.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class EditFilesetRecordManyFieldsMenu : EditRecordManyFieldsMenu<IFileset, IFilesetHelper>, IEditFilesetRecordManyFieldsMenu
	{
		public override void UpdateGlobals()
		{
			gEngine.FilesetsModified = true;
		}

		public EditFilesetRecordManyFieldsMenu()
		{
			Title = "EDIT FILESET RECORD FIELDS";

			RecordTable = gEngine.Database.FilesetTable;

			RecordTypeName = "Fileset";
		}
	}
}
