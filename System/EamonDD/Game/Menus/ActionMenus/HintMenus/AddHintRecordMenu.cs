
// AddHintRecordMenu.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework;
using Eamon.Framework.Helpers;
using Eamon.Game.Attributes;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.Globals;

namespace EamonDD.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class AddHintRecordMenu : AddRecordManualMenu<IHint, IHintHelper>, IAddHintRecordMenu
	{
		public override void UpdateGlobals()
		{
			gEngine.HintsModified = true;

			if (gEngine.Module != null)
			{
				gEngine.Module.NumHints++;

				gEngine.ModulesModified = true;
			}
		}

		public AddHintRecordMenu()
		{
			Title = "ADD HINT RECORD";

			RecordTable = gDatabase.HintTable;

			RecordTypeName = "Hint";
		}
	}
}
