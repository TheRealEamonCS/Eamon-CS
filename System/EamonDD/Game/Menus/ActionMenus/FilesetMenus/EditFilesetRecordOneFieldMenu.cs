
// EditFilesetRecordOneFieldMenu.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework;
using Eamon.Framework.Helpers;
using Eamon.Game.Attributes;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.Globals;

namespace EamonDD.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class EditFilesetRecordOneFieldMenu : EditRecordOneFieldMenu<IFileset, IFilesetHelper>, IEditFilesetRecordOneFieldMenu
	{
		public override void UpdateGlobals()
		{
			gEngine.FilesetsModified = true;
		}

		public EditFilesetRecordOneFieldMenu()
		{
			Title = "EDIT FILESET RECORD FIELD";

			RecordTable = gDatabase.FilesetTable;

			RecordTypeName = "Fileset";
		}
	}
}
