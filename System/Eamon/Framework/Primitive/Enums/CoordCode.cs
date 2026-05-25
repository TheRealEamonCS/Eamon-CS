
// CoordCode.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

namespace Eamon.Framework.Primitive.Enums
{
	/// <summary>
	/// Defines how the initial coordinate of a Monster or Artifact is determined when placed in a Room.
	/// </summary>
	/// <remarks>
	/// Used during Room initialization to set the starting position of Monsters and Artifacts
	/// within the range system. Designers can use this to control encounter positioning — for
	/// example, placing archers at the far end of a room or ambushers at melee range.
	/// </remarks>
	public enum CoordCode : long
	{
		/// <summary>
		/// Use the explicitly set Coord value as InitCoord.
		/// Designer has manually specified the position.
		/// </summary>
		Specified = 0,
		
		/// <summary>
		/// Set InitCoord to a random value within [0, Room.MaxCoord].
		/// Used for unpredictable encounters.
		/// </summary>
		Random,
		
		/// <summary>
		/// Set InitCoord to 0 (melee range, at player's starting position).
		/// Used for ambushers, melee fighters.
		/// </summary>
		Zero,
		
		/// <summary>
		/// Set InitCoord to Room.MaxCoord / 2 (middle of room).
		/// Used for neutral starting positions.
		/// </summary>
		Mid,
		
		/// <summary>
		/// Set InitCoord to Room.MaxCoord (far end of room).
		/// Used for archers, distant encounters.
		/// </summary>
		Max
	}
}
