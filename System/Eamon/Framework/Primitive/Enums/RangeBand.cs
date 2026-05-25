
// RangeBand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

namespace Eamon.Framework.Primitive.Enums
{
	/// <summary>
	/// Defines the named distance bands used to classify proximity in the range system.
	/// </summary>
	/// <remarks>
	/// Range bands provide a human-readable description of distance that is used in Room descriptions,
	/// the Range command, and the Map command. Each band corresponds to a range of varn values.
	/// </remarks>
	public enum RangeBand : long
	{
		/// <summary>
		/// The object is at the same position as the context Monster (range 0).
		/// </summary>
		Here = 0,

		/// <summary>
		/// The object is close by (range 1-3).
		/// </summary>
		CloseBy,

		/// <summary>
		/// The object is nearby (range 4-15).
		/// </summary>
		Nearby,

		/// <summary>
		/// The object is far away (range 16-40).
		/// </summary>
		FarAway,

		/// <summary>
		/// The object is very far away (range 41+).
		/// </summary>
		VeryFarAway
	}
}
