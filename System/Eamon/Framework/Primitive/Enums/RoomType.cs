
// RoomType.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

namespace Eamon.Framework.Primitive.Enums
{
	/// <summary>
	/// An enumeration of Room Types.
	/// </summary>
	/// <remarks>
	/// These represent the possible types of <see cref="IRoom">Room</see>s found in a game.  Each Room has its own room type value
	/// which can be manipulated during gameplay if desired.
	/// </remarks>
	public enum RoomType : long
	{
		/// <summary>
		/// The <see cref="IRoom">Room</see> is considered indoors; the adjacent Rooms list is prefixed with "Obvious exits".
		/// </summary>
		Indoors = 0,

		/// <summary>
		/// The <see cref="IRoom">Room</see> is considered outdoors; the adjacent Rooms list is prefixed with "Obvious paths".
		/// </summary>
		Outdoors
	}
}
