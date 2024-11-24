
// Monster.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework;
using Eamon.Game.Attributes;
using System.Diagnostics;
using System.Linq;
using static Dharmaquest.Game.Plugin.Globals;

namespace Dharmaquest.Game
{
	[ClassMappings]
	public class Monster : Eamon.Game.Monster, IMonster
	{
		public override bool HasWornInventory()
		{
			var monsterUids = new long[] { 16, 20, 30, 31 };

			// Sacred bull / Python / Leopard / Sphinx have no worn inventory

			return !monsterUids.Contains(Uid) ? base.HasWornInventory() : false;
		}

		public override bool HasCarriedInventory()
		{
			var monsterUids = new long[] { 16, 20, 30, 31 };

			// Sacred bull / Python / Leopard / Sphinx have no carried inventory

			return !monsterUids.Contains(Uid) ? base.HasCarriedInventory() : false;
		}

		public override bool CanMoveToRoomUid(long roomUid, bool fleeing)
		{
			// Patroclos / Achilles / Neoptolemus can't flee or follow

			return Uid != 6 && Uid != 7 && Uid != 8 ? base.CanMoveToRoomUid(roomUid, fleeing) : false;
		}

		public override bool ShouldRefuseToAcceptGift(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			// Sacred bull accepts salt / Python accepts hamster

			return !((Uid == 16 && artifact.Uid == 20) || (Uid == 20 && artifact.Uid == 21)) ? base.ShouldRefuseToAcceptGift(artifact) : false;
		}

		public override string[] GetNaturalAttackDescs()
		{
			// Sacred bull

			return Uid == 16 ? new string[] { "charge{0} at", "bite{0} at", "gore{0} at" } :

				// Python

				Uid == 20 ? new string[] { "constrict{0} around", "bite{0} at" } :

				// Leopard

				Uid == 30 ? new string[] { "lunge{0} at", "bite{0} at", "claw{0} at", "pounce{0} on" } :

				base.GetNaturalAttackDescs();
		}

		public override string GetArmorDescString()
		{
			var armorDesc = base.GetArmorDescString();

			if (IsInRoomLit())
			{
				// Sacred bull

				if (Uid == 16)
				{
					armorDesc = "its tough hide";
				}

				// Python

				else if (Uid == 20)
				{
					armorDesc = "its slick scales";
				}

				// Leopard

				else if (Uid == 30)
				{
					armorDesc = "its smooth fur";
				}
			}

			return armorDesc;
		}
	}
}
