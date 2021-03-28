
// AnalyseMonsterRecordInterdependenciesMenu.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework;
using Eamon.Framework.Helpers;
using Eamon.Game.Attributes;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.PluginContext;

namespace EamonDD.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class AnalyseMonsterRecordInterdependenciesMenu : AnalyseRecordInterdependenciesMenu<IMonster, IMonsterHelper>, IAnalyseMonsterRecordInterdependenciesMenu
	{
		public AnalyseMonsterRecordInterdependenciesMenu()
		{
			Title = "ANALYSE MONSTER RECORDS";

			RecordTable = Globals.Database.MonsterTable;

			RecordTypeName = "Monster";
		}
	}
}
