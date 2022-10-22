
// Monster.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Linq;
using Eamon.Framework;
using Eamon.Game.Attributes;
using static AlternateBeginnersCave.Game.Plugin.Globals;

namespace AlternateBeginnersCave.Game
{
	[ClassMappings]
	public class Monster : Eamon.Game.Monster, IMonster
	{
		public override bool HasWornInventory()
		{
			var monsterUids = new long[] { 1, 3, 4, 6, 7, 8 };

			// Only humanoids have a worn inventory list

			return monsterUids.Contains(Uid) ? false : base.HasWornInventory();
		}

		public override bool HasCarriedInventory()
		{
			var monsterUids = new long[] { 1, 3, 4, 6, 7, 8 };

			// Only humanoids have a carried inventory list

			return monsterUids.Contains(Uid) ? false : base.HasCarriedInventory();
		}
	}
}
