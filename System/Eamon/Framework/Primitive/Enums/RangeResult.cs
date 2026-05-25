
// RangeResult.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

namespace Eamon.Framework.Primitive.Enums
{
	/// <summary>
	/// Defines the result of a weapon range check relative to a target during combat.
	/// </summary>
	/// <remarks>
	/// Returned by CheckWeaponRange to classify the attacker's distance from the target against
	/// the weapon's range properties. Results of <see cref="TooClose"/> and <see cref="OutOfRange"/>
	/// block the attack entirely. <see cref="SubOptimalClose"/> and <see cref="SubOptimalFar"/> allow
	/// the attack to proceed but apply odds and damage penalties. <see cref="InRange"/> indicates
	/// full effectiveness with no penalties. Note this applies to both Artifact and Natural weapons.
	/// </remarks>
	public enum RangeResult : long
	{
		/// <summary>
		/// The target is within the weapon's optimal range. No penalties apply.
		/// </summary>
		InRange = 0,

		/// <summary>
		/// The target is closer than the weapon's minimum range. Attack is blocked.
		/// </summary>
		TooClose,

		/// <summary>
		/// The target is between the weapon's minimum range and optimal minimum. Attack proceeds with penalties.
		/// </summary>
		SubOptimalClose,

		/// <summary>
		/// The target is between the weapon's optimal maximum and maximum range. Attack proceeds with penalties.
		/// </summary>
		SubOptimalFar,

		/// <summary>
		/// The target is beyond the weapon's maximum range. Attack is blocked.
		/// </summary>
		OutOfRange
	}
}
