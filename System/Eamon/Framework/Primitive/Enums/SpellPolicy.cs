
// SpellPolicy.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

namespace Eamon.Framework.Primitive.Enums
{
	/// <summary>
	/// An enumeration used to set a <see cref="IMonster">Monster</see>'s <see cref="Spell">Spell</see> policy.
	/// </summary>
	public enum SpellPolicy : long
	{
		/// <summary></summary>
		None = 0,

		/// <summary>
		/// Target the least injured available <see cref="IMonster">Monster</see>.
		/// </summary>
		LeastInjured,

		/// <summary>
		/// Target the most injured available <see cref="IMonster">Monster</see>.
		/// </summary>
		MostInjured,

		/// <summary>
		/// Target a random available <see cref="IMonster">Monster</see>.
		/// </summary>
		Random
	}
}
