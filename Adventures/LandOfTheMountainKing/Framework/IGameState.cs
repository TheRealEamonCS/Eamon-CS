﻿
// IGameState.cs

// Copyright (c) 2014+ by Kenneth Pedersen.  All rights reserved.

using static LandOfTheMountainKing.Game.Plugin.Globals;

namespace LandOfTheMountainKing.Framework
{
	/// <inheritdoc />
	public interface IGameState : Eamon.Framework.IGameState
	{
		ILMKKP1 LMKKP1 { get; set; }
	}
}
