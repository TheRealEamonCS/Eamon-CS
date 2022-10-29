
// GameState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework;
using Eamon.Game.Attributes;
using static AlternateBeginnersCave.Game.Plugin.Globals;

namespace AlternateBeginnersCave.Game
{
	[ClassMappings(typeof(IGameState))]
	public class GameState : Eamon.Game.GameState, Framework.IGameState
	{
		/// <summary></summary>
		[FieldName(1121)]
		public virtual long MagicDaggerCounter { get; set; }

		/// <summary></summary>
		public virtual bool OpenedBox { get; set; }

		/// <summary></summary>
		public virtual bool DrankVial { get; set; }
	}
}
