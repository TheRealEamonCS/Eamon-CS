
// EditHintRecordManyFieldsMenu.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework;
using Eamon.Framework.Helpers;
using Eamon.Game.Attributes;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.Globals;

namespace EamonDD.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class EditHintRecordManyFieldsMenu : EditRecordManyFieldsMenu<IHint, IHintHelper>, IEditHintRecordManyFieldsMenu
	{
		public override void UpdateGlobals()
		{
			gEngine.HintsModified = true;
		}

		public EditHintRecordManyFieldsMenu()
		{
			Title = "EDIT HINT RECORD FIELDS";

			RecordTable = gDatabase.HintTable;

			RecordTypeName = "Hint";
		}
	}
}
