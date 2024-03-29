﻿
// EditMonsterRecordOneFieldMenu.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework;
using Eamon.Framework.Helpers;
using Eamon.Game.Attributes;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.Globals;

namespace EamonDD.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class EditMonsterRecordOneFieldMenu : EditRecordOneFieldMenu<IMonster, IMonsterHelper>, IEditMonsterRecordOneFieldMenu
	{
		public override void UpdateGlobals()
		{
			gEngine.MonstersModified = true;
		}

		public EditMonsterRecordOneFieldMenu()
		{
			Title = "EDIT MONSTER RECORD FIELD";

			RecordTable = gDatabase.MonsterTable;

			RecordTypeName = "Monster";
		}
	}
}
