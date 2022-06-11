
// Monster.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework;
using Eamon.Game.Attributes;
using static AlternateBeginnersCave.Game.Plugin.PluginContext;

namespace AlternateBeginnersCave.Game
{
	[ClassMappings]
	public class Monster : Eamon.Game.Monster, IMonster
	{
		public override bool HasWornInventory()
		{
			// Only humanoids have a worn inventory list

			return Uid != 6 && Uid != 7;
		}

		public override bool HasCarriedInventory()
		{
			// Only humanoids have a carried inventory list

			return Uid != 6 && Uid != 7;
		}
	}
}
