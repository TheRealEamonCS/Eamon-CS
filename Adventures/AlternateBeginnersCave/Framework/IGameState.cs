
// IGameState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using static AlternateBeginnersCave.Game.Plugin.Globals;

namespace AlternateBeginnersCave.Framework
{
	public interface IGameState : Eamon.Framework.IGameState
	{
		#region Properties

		/// <summary></summary>
		long MagicDaggerCounter { get; set; }

		/// <summary></summary>
		bool OpenedBox { get; set; }

		/// <summary></summary>
		bool DrankVial { get; set; }

		#endregion
	}
}
