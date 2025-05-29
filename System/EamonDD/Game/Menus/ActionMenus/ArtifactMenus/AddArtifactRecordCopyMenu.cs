
// AddArtifactRecordCopyMenu.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework;
using Eamon.Framework.Helpers;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.Globals;

namespace EamonDD.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class AddArtifactRecordCopyMenu : AddRecordCopyMenu<IArtifact, IArtifactHelper>, IAddArtifactRecordCopyMenu
	{
		public override void UpdateGlobals()
		{
			if (gDatabase.ArtifactTableType == ArtifactTableType.CharArt)
			{
				gEngine.CharArtsModified = true;
			}
			else
			{
				gEngine.ArtifactsModified = true;

				if (gEngine.Module != null)
				{
					gEngine.Module.NumArtifacts++;

					gEngine.ModulesModified = true;
				}
			}
		}

		public AddArtifactRecordCopyMenu()
		{
			Title = "COPY ARTIFACT RECORD";

			RecordTable = gDatabase.ArtifactTable;

			RecordTypeName = "Artifact";
		}
	}
}
