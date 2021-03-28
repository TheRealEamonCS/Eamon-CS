
// DeleteRoomRecordMenu.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework;
using Eamon.Framework.Helpers;
using Eamon.Game.Attributes;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.PluginContext;

namespace EamonDD.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class DeleteRoomRecordMenu : DeleteRecordMenu<IRoom, IRoomHelper>, IDeleteRoomRecordMenu
	{
		public override void UpdateGlobals()
		{
			Globals.RoomsModified = true;

			if (Globals.Module != null)
			{
				Globals.Module.NumRooms--;

				Globals.ModulesModified = true;
			}
		}

		public DeleteRoomRecordMenu()
		{
			Title = "DELETE ROOM RECORD";

			RecordTable = Globals.Database.RoomTable;

			RecordTypeName = "Room";
		}
	}
}
