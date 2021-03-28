
// GameState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework;
using Eamon.Game.Attributes;

namespace BeginnersForest.Game
{
	[ClassMappings(typeof(IGameState))]
	public class GameState : Eamon.Game.GameState, Framework.IGameState
	{
		public virtual long QueenGiftEffectUid { get; set; }

		public virtual long QueenGiftArtifactUid { get; set; }

		public virtual long SpookCounter { get; set; }

		public GameState()
		{
			// Queen's gift

			QueenGiftEffectUid = 5;

			QueenGiftArtifactUid = 7;
		}
	}
}
