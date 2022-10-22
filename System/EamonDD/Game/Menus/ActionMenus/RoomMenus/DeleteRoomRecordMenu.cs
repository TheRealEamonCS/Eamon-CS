
// DeleteRoomRecordMenu.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework;
using Eamon.Framework.Helpers;
using Eamon.Game.Attributes;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.Globals;

namespace EamonDD.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class DeleteRoomRecordMenu : DeleteRecordMenu<IRoom, IRoomHelper>, IDeleteRoomRecordMenu
	{
		public override void UpdateGlobals()
		{
			gEngine.RoomsModified = true;

			if (gEngine.Module != null)
			{
				gEngine.Module.NumRooms--;

				gEngine.ModulesModified = true;
			}
		}

		public DeleteRoomRecordMenu()
		{
			Title = "DELETE ROOM RECORD";

			RecordTable = gEngine.Database.RoomTable;

			RecordTypeName = "Room";
		}
	}
}
