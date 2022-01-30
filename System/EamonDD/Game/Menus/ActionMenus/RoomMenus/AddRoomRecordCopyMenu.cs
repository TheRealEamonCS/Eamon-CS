
// AddRoomRecordCopyMenu.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework;
using Eamon.Framework.Helpers;
using Eamon.Game.Attributes;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.PluginContext;

namespace EamonDD.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class AddRoomRecordCopyMenu : AddRecordCopyMenu<IRoom, IRoomHelper>, IAddRoomRecordCopyMenu
	{
		public override void UpdateGlobals()
		{
			Globals.RoomsModified = true;

			if (Globals.Module != null)
			{
				Globals.Module.NumRooms++;

				Globals.ModulesModified = true;
			}
		}

		public AddRoomRecordCopyMenu()
		{
			Title = "COPY ROOM RECORD";

			RecordTable = Globals.Database.RoomTable;

			RecordTypeName = "Room";
		}
	}
}
