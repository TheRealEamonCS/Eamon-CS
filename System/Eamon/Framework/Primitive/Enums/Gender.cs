
// Gender.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

namespace Eamon.Framework.Primitive.Enums
{
	/// <summary>
	/// An enumeration of <see cref="ICharacter">Character</see>/<see cref="IMonster">Monster</see> genders.
	/// </summary>
	/// <remarks>
	/// Each <see cref="ICharacter">Character</see> or <see cref="IMonster">Monster</see> in a game is assigned
	/// a gender value.  For player characters this is of course set during creation in the Main Hall.  As with
	/// all other properties, the gender value can be manipulated during gameplay (and, in at least one ECS
	/// adventure, actually is!)
	/// </remarks>
	public enum Gender : long
	{
		/// <summary>
		/// The <see cref="ICharacter">Character</see> or <see cref="IMonster">Monster</see> is male.
		/// </summary>
		Male = 0,

		/// <summary>
		/// The <see cref="ICharacter">Character</see> or <see cref="IMonster">Monster</see> is female.
		/// </summary>
		Female,
		
		/// <summary>
		/// The <see cref="IMonster">Monster</see> is neutral/indeterminate.  Not available to player characters.
		/// </summary>
		Neutral
	}
}
