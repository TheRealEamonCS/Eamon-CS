
// LightLevel.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

namespace Eamon.Framework.Primitive.Enums
{
	/// <summary>
	/// An enumeration of <see cref="IRoom">Room</see> Light Levels.
	/// </summary>
	/// <remarks>
	/// These represent the possible ambient light levels found in <see cref="IRoom">Room</see>s in a game.  Each Room has its
	/// own light level value which can be manipulated during gameplay if desired.
	/// </remarks>
	public enum LightLevel : long
	{
		/// <summary>
		/// Dark <see cref="IRoom">Room</see>s are very restrictive on the kind of activities allowed.
		/// </summary>
		Dark = 0,

		/// <summary>
		/// Lit <see cref="IRoom">Room</see>s allow the full range of Commands.
		/// </summary>
		Light
	}
}
