
// AnalyseRoomRecordInterdependenciesMenu.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework;
using Eamon.Framework.Helpers;
using Eamon.Game.Attributes;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.PluginContext;

namespace EamonDD.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class AnalyseRoomRecordInterdependenciesMenu : AnalyseRecordInterdependenciesMenu<IRoom, IRoomHelper>, IAnalyseRoomRecordInterdependenciesMenu
	{
		public AnalyseRoomRecordInterdependenciesMenu()
		{
			Title = "ANALYSE ROOM RECORDS";

			RecordTable = Globals.Database.RoomTable;

			RecordTypeName = "Room";
		}
	}
}
