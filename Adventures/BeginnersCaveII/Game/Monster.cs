
// Monster.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Linq;
using Eamon.Framework;
using Eamon.Game.Attributes;
using static BeginnersCaveII.Game.Plugin.PluginContext;

namespace BeginnersCaveII.Game
{
	[ClassMappings]
	public class Monster : Eamon.Game.Monster, IMonster
	{
		public override bool HasWornInventory()
		{
			var monsterUids = new long[] { 3, 5, 6, 10, 14, 16 };

			// Only humanoids have a worn inventory list

			return monsterUids.Contains(Uid) ? false : base.HasWornInventory();
		}

		public override bool HasCarriedInventory()
		{
			var monsterUids = new long[] { 3, 5, 6, 10, 14, 16 };

			// Only humanoids have a carried inventory list

			return monsterUids.Contains(Uid) ? false : base.HasCarriedInventory();
		}
	}
}
