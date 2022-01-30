
// GameState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework;
using Eamon.Game.Attributes;
using static TheTempleOfNgurct.Game.Plugin.PluginContext;

namespace TheTempleOfNgurct.Game
{
	[ClassMappings(typeof(IGameState))]
	public class GameState : Eamon.Game.GameState, Framework.IGameState
	{
		[FieldName(1121)]
		public virtual long WanderingMonster { get; set; }

		[FieldName(1122)]
		public virtual long DwLoopCounter { get; set; }

		[FieldName(1123)]
		public virtual long WandCharges { get; set; }

		[FieldName(1124)]
		public virtual long Regenerate { get; set; }

		[FieldName(1125)]
		public virtual long KeyRingRoomUid { get; set; }

		public virtual bool AlkandaKilled { get; set; }

		public virtual bool AlignmentConflict { get; set; }

		public virtual bool CobraAppeared { get; set; }

		public GameState()
		{
			// Sets up wandering monsters and fireball wand charges

			WanderingMonster = gEngine.RollDice(1, 14, 11);

			WandCharges = gEngine.RollDice(1, 4, 1);
		}
	}
}
