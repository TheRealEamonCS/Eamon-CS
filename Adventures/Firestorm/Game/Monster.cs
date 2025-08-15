
// Monster.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Linq;
using Eamon.Framework;
using Eamon.Game.Attributes;
using static Firestorm.Game.Plugin.Globals;

namespace Firestorm.Game
{
	[ClassMappings]
	public class Monster : Eamon.Game.Monster, IMonster
	{
		public override bool HasWornInventory()
		{
			var monsterUids = new long[] { 19, 29, 38, 41, 42 };			// Negative set

			// Some monsters have no worn inventory list

			return !monsterUids.Contains(Uid) ? base.HasWornInventory() : false;
		}

		public override bool HasCarriedInventory()
		{
			var monsterUids = new long[] { 19, 29, 38, 41, 42 };           // Negative set

			// Some monsters have no carried inventory list

			return !monsterUids.Contains(Uid) ? base.HasCarriedInventory() : false;
		}

		public override bool HasHumanNaturalAttackDescs()
		{
			var monsterUids = new long[] { 19, 23, 24, 25, 26, 29, 31, 35, 38, 40, 41, 42, 43 };           // Negative set

			// Use appropriate natural attack descriptions for humans

			return !monsterUids.Contains(Uid) ? true : false;
		}

		public override bool ShouldRefuseToAcceptGift(IArtifact artifact)
		{
			return !HasCarriedInventory();
		}

		public override string[] GetNaturalAttackDescs()
		{
			var randomNames1 = new string[] { "knight", "officer", "barbarian" };

			var randomNames2 = new string[] { "ninja", "spy" };

			return

				// Randoms #1 / Soldiers / Orcs / Grunts

				(Uid > 22 && Uid < 27 && randomNames1.Contains(Name)) || Uid == 31 || Uid == 35 || Uid == 40 ? new string[] { "swing{0} at", "chop{0} at", "stab{0} at", "lunge{0} at", "jab{0} at" } :

				// Randoms #2

				(Uid > 22 && Uid < 27 && randomNames2.Contains(Name)) ? GetHumanAttackDescs() :

				// Rattlesnake / Cobras

				Uid == 41 || Uid == 42 ? new string[] { "strike{0} at", "bite{0} at", "attack{0}" } :

				base.GetNaturalAttackDescs();
		}
	}
}
