
// Monster.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Linq;
using Eamon.Framework;
using Eamon.Game.Attributes;
using static RiddlesOfTheDuergarKingdom.Game.Plugin.PluginContext;

namespace RiddlesOfTheDuergarKingdom.Game
{
	[ClassMappings]
	public class Monster : Eamon.Game.Monster, IMonster
	{
		public override bool HasCarriedInventory()
		{
			// Highland ibex has no carried inventory list

			return Uid != 21 ? base.HasCarriedInventory() : false;
		}

		public override bool IsAttackable(IMonster monster)
		{
			var monsterUids = new long[] { 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 21 };

			// Various monsters can't be attacked

			return !monsterUids.Contains(Uid) ? base.IsAttackable(monster) : false;
		}

		public override bool CanMoveToRoomUid(long roomUid, bool fleeing)
		{
			var roomUids = new long[] { 37, 137, 138, 149 };

			// Highland ibex can't move to various rooms

			if (Uid == 21)
			{
				return !roomUids.Contains(roomUid) ? base.CanMoveToRoomUid(roomUid, fleeing) : false;
			}
			else
			{
				return base.CanMoveToRoomUid(roomUid, fleeing);
			}
		}

		public override bool ShouldProcessInGameLoop()
		{
			// Suppress black spider's initial attack

			if (Uid == 16 && Globals.BlackSpiderJumps)
			{
				Globals.BlackSpiderJumps = false;

				return false;
			}
			else
			{
				return base.ShouldProcessInGameLoop();
			}
		}

		public override bool ShouldRefuseToAcceptGold()
		{
			return false;
		}

		public override bool ShouldRefuseToAcceptGift(IArtifact artifact)
		{
			return false;
		}

		public override string[] GetHumanAttackDescs()
		{
			var attackDescs = new string[] { "smash{0} at" };

			return Globals.PlayerAttacksBlackSpider ? attackDescs : base.GetHumanAttackDescs();
		}
	}
}
