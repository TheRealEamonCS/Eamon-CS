
// Status.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

namespace Eamon.Framework.Primitive.Enums
{
	/// <summary>
	/// An enumeration of (<see cref="ICharacter">Character</see>) Statuses.
	/// </summary>
	/// <remarks>
	/// These represent the possible states that a player character can be in.  Each <see cref="ICharacter">Character</see> has an
	/// associated Status that is set based on various game activities.
	/// </remarks>
	public enum Status : long
	{
		/// <summary>
		/// The state of the <see cref="ICharacter">Character</see> is indeterminate (in practice, this <see cref="Status">Status</see> is unused).
		/// </summary>
		Unknown = 0,

		/// <summary>
		/// The <see cref="ICharacter">Character</see> is available for use; this is the default <see cref="Status">Status</see>.
		/// </summary>
		Alive,
		
		/// <summary>
		/// The <see cref="ICharacter">Character</see> is not available for use; this is trivially reversible, however.
		/// </summary>
		Dead,
		
		/// <summary>
		/// The <see cref="ICharacter">Character</see> is out on an adventure.
		/// </summary>
		Adventuring
	}
}
