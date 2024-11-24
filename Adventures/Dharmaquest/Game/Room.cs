
// Room.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Linq;
using Eamon.Framework;
using Eamon.Game.Attributes;
using static Dharmaquest.Game.Plugin.Globals;

namespace Dharmaquest.Game
{
	[ClassMappings]
	public class Room : Eamon.Game.Room, IRoom
	{
		public override bool IsDirectionInObviousExitsList(long index)
		{
			var northRoomUids = new long[] { 43, 67 };

			var eastRoomUids = new long[] { 4 };

			var downRoomUids = new long[] { 36, 38, 49, 50, 69 };

			// Suppress various "invisible" exits

			return !((northRoomUids.Contains(Uid) && index == 1) || (eastRoomUids.Contains(Uid) && index == 3) || (downRoomUids.Contains(Uid) && index == 6)) ? base.IsDirectionInObviousExitsList(index) : false;
		}
	}
}
