
// AddArtifactRecordManualMenu.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework;
using Eamon.Framework.Helpers;
using Eamon.Game.Attributes;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.Globals;

namespace EamonDD.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class AddArtifactRecordManualMenu : AddRecordManualMenu<IArtifact, IArtifactHelper>, IAddArtifactRecordManualMenu
	{
		public override void UpdateGlobals()
		{
			gEngine.ArtifactsModified = true;

			if (gEngine.Module != null)
			{
				gEngine.Module.NumArtifacts++;

				gEngine.ModulesModified = true;
			}
		}

		public AddArtifactRecordManualMenu()
		{
			Title = "ADD ARTIFACT RECORD";

			RecordTable = gDatabase.ArtifactTable;

			RecordTypeName = "Artifact";
		}
	}
}
