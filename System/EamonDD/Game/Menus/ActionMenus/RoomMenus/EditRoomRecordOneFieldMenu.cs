
// EditRoomRecordOneFieldMenu.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework;
using Eamon.Framework.Helpers;
using Eamon.Game.Attributes;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.Globals;

namespace EamonDD.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class EditRoomRecordOneFieldMenu : EditRecordOneFieldMenu<IRoom, IRoomHelper>, IEditRoomRecordOneFieldMenu
	{
		public override void UpdateGlobals()
		{
			gEngine.RoomsModified = true;
		}

		public EditRoomRecordOneFieldMenu()
		{
			Title = "EDIT ROOM RECORD FIELD";

			RecordTable = gEngine.Database.RoomTable;

			RecordTypeName = "Room";
		}
	}
}
