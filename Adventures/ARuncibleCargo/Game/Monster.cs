
// Monster.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using System.Text;
using Eamon;
using Eamon.Framework;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using static ARuncibleCargo.Game.Plugin.PluginContext;

namespace ARuncibleCargo.Game
{
	[ClassMappings]
	public class Monster : Eamon.Game.Monster, IMonster
	{
		public override RetCode BuildPrintedFullDesc(StringBuilder buf, bool showName)
		{
			RetCode rc;

			rc = base.BuildPrintedFullDesc(buf, showName);

			// Pookas

			if (gEngine.IsSuccess(rc) && Uid < 4 && !gGameState.GetPookaMet(Uid - 1))
			{
				var effect = gEDB[3 + Uid];

				Debug.Assert(effect != null);

				buf.AppendPrint("{0}", effect.Desc);

				gGameState.SetPookaMet(Uid - 1, true);
			}

			return rc;
		}

		public override bool HasHumanNaturalAttackDescs()
		{
			// Use appropriate natural attack descriptions for humans

			return (Uid > 3 && Uid < 13) || Uid == 15 || (Uid > 21 && Uid < 24) || (Uid > 24 && Uid < 40);
		}
		
		public override bool CanMoveToRoomUid(long roomUid, bool fleeing)
		{
			var room = gRDB[roomUid] as Framework.IRoom;

			Debug.Assert(room != null);

			// Nobody can flee into a water room

			if (fleeing && room.IsWaterRoom())
			{
				return false;
			}

			// Putrid rats and enormous alligator can't leave sewer

			else if (Uid == 13 || Uid == 14)
			{
				return roomUid != 13 && roomUid != 29 && roomUid != 50 ? base.CanMoveToRoomUid(roomUid, fleeing) : false;
			}

			// Gelatinous ghoul can't leave hotel basement

			else if (Uid == 19)
			{
				return roomUid != 39 ? base.CanMoveToRoomUid(roomUid, fleeing) : false;
			}
			else
			{
				return base.CanMoveToRoomUid(roomUid, fleeing);
			}
		}

		public override bool ShouldRefuseToAcceptGift(IArtifact artifact)
		{
			return false;
		}
	}
}
