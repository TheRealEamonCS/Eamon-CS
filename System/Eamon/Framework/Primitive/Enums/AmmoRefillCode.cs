
// AmmoRefillCode.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

namespace Eamon.Framework.Primitive.Enums
{
	/// <summary>
	/// Defines how a weapon's ammo count is initialized at the start of a new game.
	/// </summary>
	/// <remarks>
	/// Applied during game initialization for new games only — not on save/restore. Has no effect
	/// when a weapon's MaxAmmoCount is zero (i.e. melee weapons). The <see cref="Recovery"/> value
	/// simulates pre-combat arrow recovery by rolling against AmmoRecoveryOdds for each potential
	/// round of ammunition.
	/// </remarks>
	public enum AmmoRefillCode : long
	{
		/// <summary>
		/// AmmoCount unchanged on game startup.
		/// </summary>
		None = 0,

		/// <summary>
		/// AmmoCount = MaxAmmoCount.
		/// </summary>
		Full,

		/// <summary>
		/// AmmoCount = MaxAmmoCount / 2 (rounded down).
		/// </summary>
		Half,

		/// <summary>
		/// AmmoCount = MaxAmmoCount / 4 (rounded down).
		/// </summary>
		Quarter,

		/// <summary>
		/// AmmoCount = random number between 1..MaxAmmoCount.
		/// </summary>
		Random,

		/// <summary>
		/// Loop 1..MaxAmmoCount, increment AmmoCount if roll &lt;= AmmoRecoveryOdds.
		/// </summary>
		Recovery,
	}
}
