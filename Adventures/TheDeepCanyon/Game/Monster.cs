
// Monster.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework;
using Eamon.Game.Attributes;
using static TheDeepCanyon.Game.Plugin.PluginContext;

namespace TheDeepCanyon.Game
{
	[ClassMappings]
	public class Monster : Eamon.Game.Monster, IMonster
	{
		public override bool CanMoveToRoom(bool fleeing)
		{
			// Fido can't flee or follow

			return Uid != 11 ? base.CanMoveToRoom(fleeing) : false;
		}

		public override bool ShouldProcessInGameLoop()
		{
			// Fido is always active

			return (Uid == 11 && !IsInLimbo()) || base.ShouldProcessInGameLoop();
		}
	}
}
