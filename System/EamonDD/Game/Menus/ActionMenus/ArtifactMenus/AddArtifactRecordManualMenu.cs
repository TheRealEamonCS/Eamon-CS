
// AddArtifactRecordManualMenu.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework;
using Eamon.Framework.Helpers;
using Eamon.Game.Attributes;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.PluginContext;

namespace EamonDD.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class AddArtifactRecordManualMenu : AddRecordManualMenu<IArtifact, IArtifactHelper>, IAddArtifactRecordManualMenu
	{
		public override void UpdateGlobals()
		{
			Globals.ArtifactsModified = true;

			if (Globals.Module != null)
			{
				Globals.Module.NumArtifacts++;

				Globals.ModulesModified = true;
			}
		}

		public AddArtifactRecordManualMenu()
		{
			Title = "ADD ARTIFACT RECORD";

			RecordTable = Globals.Database.ArtifactTable;

			RecordTypeName = "Artifact";
		}
	}
}
