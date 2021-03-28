
// GameState.cs

// Copyright (c) 2014+ by Kenneth Pedersen.  All rights reserved.

using Eamon.Framework;
using Eamon.Game.Attributes;
using static LandOfTheMountainKing.Game.Plugin.PluginContext;

namespace LandOfTheMountainKing.Game
{
	[ClassMappings(typeof(IGameState))]
	public class GameState : Eamon.Game.GameState, Framework.IGameState
	{
		public virtual Framework.ILMKKP1 LMKKP1 { get; set; }

		public GameState()
		{
			LMKKP1 = Globals.CreateInstance<Framework.ILMKKP1>();
		}
	}
}
