
// RetCode.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

namespace Eamon
{
	/// <summary></summary>
	public enum RetCode : long
	{
		/// <summary></summary>
		CycleFound = -21,

		/// <summary></summary>
		InvalidObj,

		/// <summary></summary>
		Expired,

		/// <summary></summary>
		NotAllocated,

		/// <summary></summary>
		Unsupported,

		/// <summary></summary>
		AlreadyExists,

		/// <summary></summary>
		Unimplemented,

		/// <summary></summary>
		InvalidFmt,

		/// <summary></summary>
		IsFull,

		/// <summary></summary>
		IsEmpty,

		/// <summary></summary>
		NotFound02,

		/// <summary></summary>
		NotFound01,

		/// <summary></summary>
		NotFound,

		/// <summary></summary>
		OutOfMemory,

		/// <summary></summary>
		InvalidArg,

		/// <summary></summary>
		TimeOut,

		/// <summary></summary>
		Failure04,

		/// <summary></summary>
		Failure03,

		/// <summary></summary>
		Failure02,

		/// <summary></summary>
		Failure01,

		/// <summary></summary>
		Failure,

		/// <summary></summary>
		Success,

		/// <summary></summary>
		Aborted_S
	}
}
