
// IGameState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

namespace StrongholdOfKahrDur.Framework
{
	/// <summary></summary>
	public interface IGameState : Eamon.Framework.IGameState
	{
		/// <summary></summary>
		long LichState { get; set; }

		/// <summary></summary>
		bool UsedCauldron { get; set; }
	}
}
