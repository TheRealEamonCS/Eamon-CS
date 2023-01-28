
// Monster.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using static TheDeepCanyon.Game.Plugin.Globals;

namespace TheDeepCanyon.Game
{
	[ClassMappings]
	public class Monster : Eamon.Game.Monster, IMonster
	{
		public override bool HasWornInventory()
		{
			// Only humanoids have a worn inventory list

			return Uid == 3 || Uid == 4 || Uid == 5 || Uid == 12 || (gGameState != null && Uid == gGameState.Cm);
		}

		public override bool HasCarriedInventory()
		{
			// Only humanoids have a carried inventory list

			return Uid == 3 || Uid == 4 || Uid == 5 || Uid == 12 || Uid == 20 || (gGameState != null && Uid == gGameState.Cm);
		}

		public override bool CanMoveToRoom(bool fleeing)
		{
			// Fido can't flee or follow

			return Uid != 11 ? base.CanMoveToRoom(fleeing) : false;
		}

		public override bool ShouldRefuseToAcceptGold()
		{
			return false;
		}

		public override bool ShouldRefuseToAcceptGift(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			// Fido and elephants need special handling

			return Uid != 11 && Uid != 24 && (!gEngine.IsRulesetVersion(5, 62) && (Reaction == Friendliness.Enemy || (Reaction == Friendliness.Neutral && artifact.Value < 3000)));
		}

		public override bool ShouldRefuseToAcceptDeadBody(IArtifact artifact)
		{
			// Original game behavior

			return false;
		}

		public override void CalculateGiftFriendliness(long value, bool isArtifactValue)
		{
			Debug.Assert(gEngine.IsRulesetVersion(5, 62));

			long f = (long)(Friendliness - 100);

			f = (long)((double)f * (1 + (double)value / 200.0));

			if (f < 0)
			{
				f = 0;
			}
			else if (f > 100)
			{
				f = 100;
			}

			Friendliness = (Friendliness)(f + 100);
		}

		public override string[] GetWeaponAttackDescs(IArtifact artifact)
		{
			// Falcon

			return artifact != null && artifact.Uid == 5 ? new string[] { "attack{0}" } : base.GetWeaponAttackDescs(artifact);
		}
	}
}
