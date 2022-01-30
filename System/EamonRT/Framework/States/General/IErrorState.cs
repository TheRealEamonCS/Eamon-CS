
// IErrorState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

namespace EamonRT.Framework.States
{
	/// <summary></summary>
	public interface IErrorState : IState
	{
		/// <summary></summary>
		long ErrorCode { get; set; }

		/// <summary></summary>
		string ErrorMessage { get; set; }
	}
}
