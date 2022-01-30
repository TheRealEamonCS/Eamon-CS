
// ArtifactType.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

namespace Eamon.Framework.Primitive.Enums
{
	/// <summary>
	/// An enumeration of Artifact Types.
	/// </summary>
	/// <remarks>
	/// These artifact types parallel those found in Eamon Deluxe, but Eamon CS supports a multiple
	/// artifact type paradigm.  You can take a look through the documentation for EDX or wait for
	/// the Eamon CS Dungeon Designer's Manual, which will have more details.
	/// </remarks>
	public enum ArtifactType : long
	{
		/// <summary></summary>
		None = -1,

		/// <summary></summary>
		Gold,
		
		/// <summary></summary>
		Treasure,
		
		/// <summary></summary>
		Weapon,
		
		/// <summary></summary>
		MagicWeapon,
		
		/// <summary></summary>
		InContainer,

		/// <summary></summary>
		OnContainer,

		/// <summary></summary>
		UnderContainer,
		
		/// <summary></summary>
		BehindContainer,

		/// <summary></summary>
		LightSource,
		
		/// <summary></summary>
		Drinkable,
		
		/// <summary></summary>
		Readable,
		
		/// <summary></summary>
		DoorGate,
		
		/// <summary></summary>
		Edible,
		
		/// <summary></summary>
		BoundMonster,
		
		/// <summary></summary>
		Wearable,
		
		/// <summary></summary>
		DisguisedMonster,
		
		/// <summary></summary>
		DeadBody,
		
		/// <summary></summary>
		User1,
		
		/// <summary></summary>
		User2,
		
		/// <summary></summary>
		User3
	}
}
