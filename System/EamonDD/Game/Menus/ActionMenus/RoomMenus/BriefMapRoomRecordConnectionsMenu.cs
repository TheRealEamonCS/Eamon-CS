
// BriefMapRoomRecordConnectionsMenu.cs

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
using static EamonDD.Game.Plugin.PluginContext;

namespace EamonDD.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class BriefMapRoomRecordConnectionsMenu : Menu, IBriefMapRoomRecordConnectionsMenu
	{
		public override void Execute()
		{
			RetCode rc;

			var showHeader = true;

			var nlFlag = false;

			gOut.WriteLine();

			gEngine.PrintTitle("BRIEF MAP ROOM RECORD CONNECTIONS", true);

			if (gEngine.IsAdventureFilesetLoaded())
			{
				gOut.Print("A map of: {0}",
					Globals.Module != null ? Globals.Module.Name : gEngine.UnknownName);

				gOut.Print("{0}", Globals.LineSep);
			}

			var numDirs = Globals.Module != null ? Globals.Module.NumDirs : 6;

			var directionValues = EnumUtil.GetValues<Direction>();

			var k = Globals.Database.GetRoomsCount();

			var i = 0;

			foreach (var room in Globals.Database.RoomTable.Records)
			{
				if (showHeader)
				{
					if (numDirs == 12)
					{
						Buf.SetFormat("{0}{1}{0}{2,-18}", Environment.NewLine, "Room name:", "");

						for (var j = 0; j < numDirs; j++)
						{
							var direction = gEngine.GetDirections(directionValues[j]);

							Debug.Assert(direction != null);

							Buf.AppendFormat("{0}:{1}", direction.Abbr, direction.Abbr.Length == 2 ? "  " : "   ");
						}
					}
					else
					{
						Buf.SetFormat("{0}{1,-48}", Environment.NewLine, "Room name:");

						for (var j = 0; j < numDirs; j++)
						{
							var direction = gEngine.GetDirections(directionValues[j]);

							Debug.Assert(direction != null);

							Buf.AppendFormat("{0}:   ", direction.Abbr);
						}
					}

					gOut.Write("{0}", Buf);

					gOut.Write("{0}{1}", Environment.NewLine, Globals.LineSep);

					showHeader = false;
				}

				Buf.SetFormat("{0}{1,3}. {2}", Environment.NewLine, room.Uid, room.Name);

				while (Buf.Length < 49)
				{
					Buf.Append('.');
				}

				Buf.Length = 49;

				if (numDirs == 6 && Environment.NewLine.Length == 1)
				{
					Buf.Length--;
				}

				Buf.Append('.');

				if (numDirs == 12)
				{
					Buf.AppendFormat("{0}{1,-18}", Environment.NewLine, "");
				}

				for (var j = 0; j < numDirs; j++)
				{
					Buf.AppendFormat("{0,-4} ", room.GetDirs(directionValues[j]));
				}

				gOut.Write("{0}", Buf);

				nlFlag = true;

				if ((i != 0 && (i % (Constants.NumRows - (numDirs == 12 ? 16 : 10))) == 0) || i == k - 1)
				{
					nlFlag = false;

					gOut.WriteLine();

					gOut.Print("{0}", Globals.LineSep);

					gOut.Write("{0}Press any key to continue or X to exit: ", Environment.NewLine);

					Buf.Clear();

					rc = Globals.In.ReadField(Buf, Constants.BufSize02, null, ' ', '\0', true, null, gEngine.ModifyCharToNullOrX, null, gEngine.IsCharAny);

					Debug.Assert(gEngine.IsSuccess(rc));

					gOut.Print("{0}", Globals.LineSep);

					if (Buf.Length > 0 && Buf[0] == 'X')
					{
						break;
					}

					showHeader = true;
				}

				i++;
			}

			if (nlFlag)
			{
				gOut.WriteLine();
			}

			gOut.Print("Done briefly mapping Room record connections.");
		}

		public BriefMapRoomRecordConnectionsMenu()
		{
			Buf = Globals.Buf;
		}
	}
}
