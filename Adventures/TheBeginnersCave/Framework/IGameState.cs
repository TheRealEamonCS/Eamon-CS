
// IGameState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

namespace TheBeginnersCave.Framework
{
	/// <summary></summary>
	public interface IGameState : Eamon.Framework.IGameState
	{
		#region Properties

		/// <summary>
		/// Gets or sets a value indicating the Trollsfire sword's activation state.
		/// </summary>
		long Trollsfire { get; set; }

		/// <summary></summary>
		long BookWarning { get; set; }

		#endregion
	}
}
