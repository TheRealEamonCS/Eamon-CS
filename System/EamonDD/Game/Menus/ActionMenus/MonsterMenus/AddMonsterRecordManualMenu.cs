
// AddMonsterRecordManualMenu.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework;
using Eamon.Framework.Helpers;
using Eamon.Game.Attributes;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.Globals;

namespace EamonDD.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class AddMonsterRecordManualMenu : AddRecordManualMenu<IMonster, IMonsterHelper>, IAddMonsterRecordManualMenu
	{
		public override void UpdateGlobals()
		{
			gEngine.MonstersModified = true;

			if (gEngine.Module != null)
			{
				gEngine.Module.NumMonsters++;

				gEngine.ModulesModified = true;
			}
		}

		public AddMonsterRecordManualMenu()
		{
			Title = "ADD MONSTER RECORD";

			RecordTable = gDatabase.MonsterTable;

			RecordTypeName = "Monster";
		}
	}
}
