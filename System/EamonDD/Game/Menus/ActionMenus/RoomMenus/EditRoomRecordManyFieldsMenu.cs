
// EditRoomRecordManyFieldsMenu.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework;
using Eamon.Framework.Helpers;
using Eamon.Game.Attributes;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.Globals;

namespace EamonDD.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class EditRoomRecordManyFieldsMenu : EditRecordManyFieldsMenu<IRoom, IRoomHelper>, IEditRoomRecordManyFieldsMenu
	{
		public override void UpdateGlobals()
		{
			gEngine.RoomsModified = true;
		}

		public EditRoomRecordManyFieldsMenu()
		{
			Title = "EDIT ROOM RECORD FIELDS";

			RecordTable = gDatabase.RoomTable;

			RecordTypeName = "Room";
		}
	}
}
