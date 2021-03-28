
// AnalyseMonsterRecordInterdependenciesMenu01.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework;
using Eamon.Game.Attributes;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.PluginContext;

namespace EamonDD.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class AnalyseMonsterRecordInterdependenciesMenu01 : AnalyseRecordInterdependenciesMenu01<IMonster>, IAnalyseMonsterRecordInterdependenciesMenu01
	{
		public AnalyseMonsterRecordInterdependenciesMenu01()
		{
			AnalyseMenu = Globals.CreateInstance<IAnalyseMonsterRecordInterdependenciesMenu>();
		}
	}
}
