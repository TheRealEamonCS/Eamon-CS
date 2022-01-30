
// GameState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework;
using Eamon.Game.Attributes;

namespace BeginnersForest.Game
{
	[ClassMappings(typeof(IGameState))]
	public class GameState : Eamon.Game.GameState, Framework.IGameState
	{
		[FieldName(1121)]
		public virtual long QueenGiftEffectUid { get; set; }

		[FieldName(1122)]
		public virtual long QueenGiftArtifactUid { get; set; }

		[FieldName(1123)]
		public virtual long SpookCounter { get; set; }

		public GameState()
		{
			// Queen's gift

			QueenGiftEffectUid = 5;

			QueenGiftArtifactUid = 7;
		}
	}
}
