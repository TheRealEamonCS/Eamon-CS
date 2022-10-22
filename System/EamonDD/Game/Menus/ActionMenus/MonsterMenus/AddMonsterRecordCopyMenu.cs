
// AddMonsterRecordCopyMenu.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework;
using Eamon.Framework.Helpers;
using Eamon.Game.Attributes;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.Globals;

namespace EamonDD.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class AddMonsterRecordCopyMenu : AddRecordCopyMenu<IMonster, IMonsterHelper>, IAddMonsterRecordCopyMenu
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

		public AddMonsterRecordCopyMenu()
		{
			Title = "COPY MONSTER RECORD";

			RecordTable = gEngine.Database.MonsterTable;

			RecordTypeName = "Monster";
		}
	}
}
