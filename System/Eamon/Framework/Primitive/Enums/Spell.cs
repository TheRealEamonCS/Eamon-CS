
// Spell.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

namespace Eamon.Framework.Primitive.Enums
{
	/// <summary>
	/// An enumeration of <see cref="ICharacter">Character</see> and <see cref="IMonster">Monster</see> Spells.
	/// </summary>
	/// <remarks>
	/// These spells should be familiar to anyone who has been involved with Eamon before.  The Eamon CS
	/// Dungeon Designer's Manual will have more details.
	/// </remarks>
	public enum Spell : long
	{
		/// <summary></summary>
		Blast = 1,

		/// <summary></summary>
		Heal,
		
		/// <summary></summary>
		Speed,
		
		/// <summary></summary>
		Power
	}
}
