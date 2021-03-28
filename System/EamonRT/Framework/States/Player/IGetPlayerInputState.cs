
// IGetPlayerInputState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

namespace EamonRT.Framework.States
{
	/// <summary></summary>
	public interface IGetPlayerInputState : IState
	{
		/// <summary></summary>
		bool RestartCommand { get; set; }
	}
}
