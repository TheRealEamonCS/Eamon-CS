
// GameState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework;
using Eamon.Game.Attributes;
using static TheDeepCanyon.Game.Plugin.Globals;

namespace TheDeepCanyon.Game
{
	[ClassMappings(typeof(IGameState))]
	public class GameState : Eamon.Game.GameState, Framework.IGameState
	{
		public virtual long FidoSleepCounter { get; set; }

		public virtual bool Peanuts { get; set; }

		public virtual bool ElephantStatue { get; set; }

		public virtual bool Diamonds { get; set; }

		public virtual bool SquirrelRing { get; set; }

		public virtual bool BlueDragon { get; set; }

		public virtual bool TrapSet { get; set; }
	}
}
