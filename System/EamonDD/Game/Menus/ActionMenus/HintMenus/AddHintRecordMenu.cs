
// AddHintRecordMenu.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework;
using Eamon.Framework.Helpers;
using Eamon.Game.Attributes;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.PluginContext;

namespace EamonDD.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class AddHintRecordMenu : AddRecordManualMenu<IHint, IHintHelper>, IAddHintRecordMenu
	{
		public override void UpdateGlobals()
		{
			Globals.HintsModified = true;

			if (Globals.Module != null)
			{
				Globals.Module.NumHints++;

				Globals.ModulesModified = true;
			}
		}

		public AddHintRecordMenu()
		{
			Title = "ADD HINT RECORD";

			RecordTable = Globals.Database.HintTable;

			RecordTypeName = "Hint";
		}
	}
}
