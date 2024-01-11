
// EditMonsterRecordManyFieldsMenu.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework;
using Eamon.Framework.Helpers;
using Eamon.Game.Attributes;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.Globals;

namespace EamonDD.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class EditMonsterRecordManyFieldsMenu : EditRecordManyFieldsMenu<IMonster, IMonsterHelper>, IEditMonsterRecordManyFieldsMenu
	{
		public override void UpdateGlobals()
		{
			gEngine.MonstersModified = true;
		}

		public EditMonsterRecordManyFieldsMenu()
		{
			Title = "EDIT MONSTER RECORD FIELDS";

			RecordTable = gDatabase.MonsterTable;

			RecordTypeName = "Monster";
		}
	}
}
