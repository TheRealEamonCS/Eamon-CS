
// AmmoType.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

namespace Eamon.Framework.Primitive.Enums
{
	/// <summary>
	/// Defines the type of ammunition consumed by a ranged weapon.
	/// </summary>
	/// <remarks>
	/// Determines the out-of-ammo message displayed when a weapon's ammo count reaches zero.
	/// Melee weapons use <see cref="None"/>. The ammo type of a weapon and any (currently
	/// hypothetical) loaded ammunition Artifact must match.
	/// </remarks>
	public enum AmmoType : long
	{
		/// <summary></summary>
		None = 0,

		/// <summary>
		/// Ammunition for bows.
		/// </summary>
		Arrow,

		/// <summary>
		/// Ammunition for crossbows.
		/// </summary>
		Bolt,

		/// <summary>
		/// Ammunition for firearms.
		/// </summary>
		Bullet,

		/// <summary>
		/// Ammunition for magic wands, staves, energy weapons.
		/// </summary>
		Charge,

		/// <summary>
		/// Ammunition for shotguns, cannons.
		/// </summary>
		Shell,

		/// <summary>
		/// Ammunition for slings, catapults.
		/// </summary>
		Stone,

		/// <summary>
		/// Ammunition for rocket launchers, bazookas.
		/// </summary>
		Rocket,

		/// <summary>
		/// Ammunition for blowguns, dart guns.
		/// </summary>
		Dart,

		/// <summary>
		/// Ammunition for needle guns, tranquilizer weapons.
		/// </summary>
		Needle,

		/// <summary>
		/// Ammunition for air guns, BB guns.
		/// </summary>
		Pellet,

		/// <summary>
		/// Ammunition for grenade launchers.
		/// </summary>
		Grenade,

		/// <summary>
		/// Ammunition for fantasy/alien energy weapons.
		/// </summary>
		Bolazar,

		/// <summary>
		/// Ammunition for crystal-powered magic weapons.
		/// </summary>
		Crystal,

		/// <summary>
		/// Ammunition for runic weapons that consume inscribed runes.
		/// </summary>
		Rune,
	}
}
