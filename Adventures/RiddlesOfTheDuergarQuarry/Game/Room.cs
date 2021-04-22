
// Room.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Linq;
using Eamon.Framework;
using Eamon.Game.Attributes;
using static RiddlesOfTheDuergarQuarry.Game.Plugin.PluginContext;

namespace RiddlesOfTheDuergarQuarry.Game
{
	[ClassMappings]
	public class Room : Eamon.Game.Room, IRoom
	{
		public override bool IsDirectionInObviousExitsList(long index)
		{
			var roomUids = new long[] 
			{ 
				49, 50, 51, 52, 
			};

			// Suppress up for all rooms in roomUids; suppress down for all except room Uid 49

			return base.IsDirectionInObviousExitsList(index) && (roomUids.Contains(Uid) ? index != 5 && (Uid == 49 || index != 6) : true);
		}
	}
}
