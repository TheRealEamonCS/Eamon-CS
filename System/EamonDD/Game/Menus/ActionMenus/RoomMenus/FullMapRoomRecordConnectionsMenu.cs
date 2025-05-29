
// FullMapRoomRecordConnectionsMenu.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using Eamon;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using Eamon.Game.Menus;
using Eamon.Game.Utilities;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.Globals;

namespace EamonDD.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class FullMapRoomRecordConnectionsMenu : Menu, IFullMapRoomRecordConnectionsMenu
	{
		public override void Execute()
		{
			RetCode rc;

			gOut.WriteLine();

			gEngine.PrintTitle("FULL MAP ROOM RECORD CONNECTIONS", true);

			if (gEngine.IsAdventureFilesetLoaded())
			{
				gOut.Print("A map of: {0}",
					gEngine.Module != null ? gEngine.Module.Name : gEngine.UnknownName);

				gOut.Print("{0}", gEngine.LineSep);
			}

			var numDirs = gEngine.Module != null ? gEngine.Module.NumDirs : 6;

			var directionValues = EnumUtil.GetValues<Direction>();

			gEngine.DdSuppressPostInputSleep = true;

			foreach (var room in gDatabase.RoomTable.Records)
			{
				Buf.SetFormat("{0}\tRoom {1}: {2}", Environment.NewLine, room.Uid, room.Name);

				Buf.AppendFormat("{0}{0}{1}{0}", Environment.NewLine, room.Desc);

				for (var i = 0; i < numDirs; i++)
				{
					var direction = gEngine.GetDirection(directionValues[i]);

					Debug.Assert(direction != null);

					Buf.AppendFormat("{0}{1,-2}: {2,-6}", (directionValues[i] == Direction.North || directionValues[i] == Direction.Up || directionValues[i] == Direction.Southeast) ? Environment.NewLine : "\t", direction.Abbr, room.GetDir(directionValues[i]));
				}

				gOut.WriteLine("{0}", Buf);

				gOut.Print("{0}", gEngine.LineSep);

				gOut.Write("{0}Press any key to continue or X to exit: ", Environment.NewLine);

				Buf.Clear();

				rc = gEngine.In.ReadField(Buf, gEngine.BufSize02, null, ' ', '\0', true, null, gEngine.ModifyCharToNullOrX, null, gEngine.IsCharAny);

				Debug.Assert(gEngine.IsSuccess(rc));

				gOut.Print("{0}", gEngine.LineSep);

				if (Buf.Length > 0 && Buf[0] == 'X')
				{
					break;
				}
			}

			gEngine.DdSuppressPostInputSleep = false;

			gOut.Print("Done fully mapping Room record connections.");
		}

		public FullMapRoomRecordConnectionsMenu()
		{
			Buf = gEngine.Buf;
		}
	}
}
