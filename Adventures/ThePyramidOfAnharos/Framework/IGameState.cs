
// IGameState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using static ThePyramidOfAnharos.Game.Plugin.Globals;

namespace ThePyramidOfAnharos.Framework
{
	public interface IGameState : Eamon.Framework.IGameState
	{
		long GU { get; set; }

		long KF { get; set; }

		long KV { get; set; }

		long KW { get; set; }
	}
}
