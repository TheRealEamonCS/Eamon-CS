﻿
// AddFilesetRecordManualMenu.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework;
using Eamon.Framework.Helpers;
using Eamon.Game.Attributes;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.Globals;

namespace EamonDD.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class AddFilesetRecordManualMenu : AddRecordManualMenu<IFileset, IFilesetHelper>, IAddFilesetRecordManualMenu
	{
		public override void UpdateGlobals()
		{
			gEngine.FilesetsModified = true;
		}

		public AddFilesetRecordManualMenu()
		{
			Title = "ADD FILESET RECORD";

			RecordTable = gEngine.Database.FilesetTable;

			RecordTypeName = "Fileset";
		}
	}
}
