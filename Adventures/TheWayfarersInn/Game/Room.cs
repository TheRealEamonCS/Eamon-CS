
// Room.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Eamon.Framework;
using Eamon.Game.Attributes;
using static TheWayfarersInn.Game.Plugin.Globals;

namespace TheWayfarersInn.Game
{
	[ClassMappings(typeof(IRoom))]
	public class Room : Eamon.Game.Room, Framework.IRoom
	{
		public override long GetDir(long index)
		{
			if (gEngine.EnableMutateProperties)
			{
				if (Uid == 10)
				{
					return gGameState.WoodenBridgeUseCounter > 2 && gGameState.WallMapRead && index == 2 ? gEngine.DirectionExit : base.GetDir(index);
				}
				else
				{
					return base.GetDir(index);
				}
			}
			else
			{
				return base.GetDir(index);
			}
		}

		public override bool IsDirectionInObviousExitsList(long index)
		{
			// Suppress out in Temple and Small Graveyard

			return (Uid != 17 && Uid != 18) || index != 12 ? base.IsDirectionInObviousExitsList(index) : false;
		}

		public override bool IsArtifactListedInRoom(IArtifact artifact)
		{
			if (artifact != null && artifact.IsInRoom(this))
			{
				// Windows

				if (artifact.Uid != 153 && artifact.IsListed == false)
				{
					artifact.Seen = true;
				}

				return artifact.IsListed;
			}
			else
			{
				return false;
			}
		}

		public virtual bool IsForestRoom()
		{
			var roomUids = new long[] { 1, 2, 3, 8, 14, 15 };

			return roomUids.Contains(Uid);
		}

		public virtual bool IsRiverRoom()
		{
			return Uid > 3 && Uid < 8;
		}

		public virtual bool IsWayfarersInnClearingRoom()
		{
			return Uid > 8 && Uid < 13;
		}

		public virtual bool IsWayfarersInnRoom()
		{
			return /* Uid == 13 || */ (Uid > 22 && Uid < 67);
		}
	}
}
