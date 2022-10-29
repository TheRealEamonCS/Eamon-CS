
// ListMonsterRecordDetailMenu.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework;
using Eamon.Framework.Helpers;
using Eamon.Game.Attributes;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.Globals;

namespace EamonDD.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class ListMonsterRecordDetailMenu : ListRecordDetailMenu<IMonster, IMonsterHelper>, IListMonsterRecordDetailMenu
	{
		public ListMonsterRecordDetailMenu()
		{
			Title = "LIST MONSTER RECORD DETAILS";

			RecordTable = gEngine.Database.MonsterTable;

			RecordTypeName = "Monster";
		}
	}
}
