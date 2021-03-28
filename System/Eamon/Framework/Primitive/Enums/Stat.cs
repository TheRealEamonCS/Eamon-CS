
// Stat.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

namespace Eamon.Framework.Primitive.Enums
{
	/// <summary>
	/// An enumeration of <see cref="ICharacter">Character</see> Stats.  
	/// </summary>
	/// <remarks>
	/// The last three should be familiar to anyone who has been involved with Eamon before; <see cref="Stat.Intellect">Intellect</see>
	/// is unique to Eamon CS, and represents the character's mental capacity, wisdom and/or intelligence.
	/// </remarks>
	public enum Stat : long
	{
		/// <summary></summary>
		Intellect = 1,

		/// <summary></summary>
		Hardiness,
		
		/// <summary></summary>
		Agility,
		
		/// <summary></summary>
		Charisma
	}
}
