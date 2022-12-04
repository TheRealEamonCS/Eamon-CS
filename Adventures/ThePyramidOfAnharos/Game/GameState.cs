
// GameState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework;
using Eamon.Game.Attributes;
using static ThePyramidOfAnharos.Game.Plugin.Globals;

namespace ThePyramidOfAnharos.Game
{
	[ClassMappings(typeof(IGameState))]
	public class GameState : Eamon.Game.GameState, Framework.IGameState
	{
		public virtual long GU { get; set; }

		public virtual long KE { get; set; }

		public virtual long KF { get; set; }

		public virtual long KG { get; set; }

		public virtual long KH { get; set; }

		public virtual long KL { get; set; }

		public virtual long KN { get; set; }

		public virtual long KO { get; set; }

		public virtual long KP { get; set; }

		public virtual long KQ { get; set; }

		public virtual long KR { get; set; }

		public virtual long KS { get; set; }

		public virtual long KT { get; set; }

		public virtual long KV { get; set; }

		public virtual long KW { get; set; }

		public GameState()
		{
			KW = 200;
		}
	}
}
