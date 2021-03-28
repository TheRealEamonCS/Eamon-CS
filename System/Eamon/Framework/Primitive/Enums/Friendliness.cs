
// Friendliness.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

namespace Eamon.Framework.Primitive.Enums
{
	/// <summary>
	/// An enumeration of <see cref="IMonster">Monster</see> friendliness values.  
	/// </summary>
	/// <remarks>
	/// Each <see cref="IMonster">Monster</see> in a game has a reaction to the player character, as defined by this
	/// enum.  This is pretty standard to Eamon in general and Eamon CS is no different.
	/// </remarks>
	public enum Friendliness : long
	{
		/// <summary>
		/// The <see cref="IMonster">Monster</see> is hostile to the player character and all <see cref="Friend">Friend</see> monsters.
		/// The Monster attacks on sight and flees the <see cref="IRoom">Room</see> or pursues based on <see cref="IMonster.Courage">Courage</see>.
		/// </summary>
		Enemy = 1,

		/// <summary>
		/// The <see cref="IMonster">Monster</see> ignores all other Monsters and refuses to flee the <see cref="IRoom">Room</see>, or attack
		/// either <see cref="Friend">Friend</see> or <see cref="Enemy">Enemy</see>.
		/// </summary>
		Neutral,
		
		/// <summary>
		/// The <see cref="IMonster">Monster</see> is friendly, follows the player character around and attacks <see cref="Enemy">Enemy</see> Monsters.
		/// </summary>
		Friend
	}
}
