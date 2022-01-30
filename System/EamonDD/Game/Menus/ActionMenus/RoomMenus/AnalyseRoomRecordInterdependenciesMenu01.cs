
// AnalyseRoomRecordInterdependenciesMenu01.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework;
using Eamon.Game.Attributes;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.PluginContext;

namespace EamonDD.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class AnalyseRoomRecordInterdependenciesMenu01 : AnalyseRecordInterdependenciesMenu01<IRoom>, IAnalyseRoomRecordInterdependenciesMenu01
	{
		public AnalyseRoomRecordInterdependenciesMenu01()
		{
			AnalyseMenu = Globals.CreateInstance<IAnalyseRoomRecordInterdependenciesMenu>();
		}
	}
}
