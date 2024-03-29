﻿
// EditArtifactRecordManyFieldsMenu.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework;
using Eamon.Framework.Helpers;
using Eamon.Game.Attributes;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.Globals;

namespace EamonDD.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class EditArtifactRecordManyFieldsMenu : EditRecordManyFieldsMenu<IArtifact, IArtifactHelper>, IEditArtifactRecordManyFieldsMenu
	{
		public override void UpdateGlobals()
		{
			gEngine.ArtifactsModified = true;
		}

		public EditArtifactRecordManyFieldsMenu()
		{
			Title = "EDIT ARTIFACT RECORD FIELDS";

			RecordTable = gDatabase.ArtifactTable;

			RecordTypeName = "Artifact";
		}
	}
}
