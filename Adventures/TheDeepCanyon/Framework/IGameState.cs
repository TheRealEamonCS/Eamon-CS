
// IGameState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using static TheDeepCanyon.Game.Plugin.PluginContext;

namespace TheDeepCanyon.Framework
{
	public interface IGameState : Eamon.Framework.IGameState
	{
		long FidoSleepCounter { get; set; }

		bool Peanuts { get; set; }

		bool ElephantStatue { get; set; }

		bool Diamonds { get; set; }

		bool SquirrelRing { get; set; }

		bool BlueDragon { get; set; }

		bool TrapSet { get; set; }
	}
}
