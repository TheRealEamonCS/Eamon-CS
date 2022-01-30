
// Direction.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

namespace Eamon.Framework.Primitive.Enums
{
	/// <summary>
	/// An enumeration of (compass) Directions.
	/// </summary>
	/// <remarks>
	/// These represent the possible directional links between <see cref="IRoom">Room</see>s in a game.  Each Room contains an
	/// array that is indexed using these Direction values.  The array will always be created assuming a 12-direction game; for
	/// 6-direction games, the last six (6) elements will be unused.
	/// </remarks>
	public enum Direction : long
	{
		/// <summary></summary>
		North = 1,

		/// <summary></summary>
		South,
		
		/// <summary></summary>
		East,
		
		/// <summary></summary>
		West,
		
		/// <summary></summary>
		Up,
		
		/// <summary></summary>
		Down,
		
		/// <summary></summary>
		Northeast,
		
		/// <summary></summary>
		Northwest,
		
		/// <summary></summary>
		Southeast,
		
		/// <summary></summary>
		Southwest,

		/// <summary></summary>
		In,

		/// <summary></summary>
		Out
	}
}
