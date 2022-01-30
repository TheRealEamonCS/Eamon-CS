
// CombatCode.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

namespace Eamon.Framework.Primitive.Enums
{
	/// <summary>
	/// An enumeration of <see cref="IMonster">Monster</see> Combat Codes.
	/// </summary>
	/// <remarks>
	/// These represent the behavior of <see cref="IMonster">Monster</see>s while in combat.  Their effect on gameplay is intended to parallel
	/// the Combat Code setting found in Eamon Deluxe.
	/// </remarks>
	public enum CombatCode : long
	{
		/// <summary>
		/// The <see cref="IMonster">Monster</see> will never fight.
		/// </summary>
		NeverFights = -2,

		/// <summary>
		/// The <see cref="IMonster">Monster</see> will favor <see cref="IArtifact">Artifact</see> weapons but fall back to natural weapons if necessary.
		/// </summary>
		NaturalWeapons,
		
		/// <summary>
		/// The <see cref="IMonster">Monster</see> will use either <see cref="IArtifact">Artifact</see> weapons or natural weapons (but never both).
		/// </summary>
		Weapons,
		
		/// <summary>
		/// The <see cref="IMonster">Monster</see> will be described as "attacking"; otherwise mirrors the <see cref="Weapons">Weapons</see> setting.
		/// </summary>
		Attacks
	}
}
