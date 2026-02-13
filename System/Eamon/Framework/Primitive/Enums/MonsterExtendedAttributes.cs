
// MonsterExtendedAttributes.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

namespace Eamon.Framework.Primitive.Enums
{
	public enum MonsterExtendedAttributes : ulong
	{
		/// <summary></summary>
		HasWornInventory = 1,

		/// <summary></summary>
		HasCarriedInventory = 2,

		/// <summary></summary>
		HasHumanNaturalAttackDescs = 4,

		/// <summary></summary>
		IsAttackable = 8,

		/// <summary></summary>
		CanAttackWithMultipleWeapons = 16,

		/// <summary></summary>
		ShouldReadyWeapon = 32,

		/// <summary></summary>
		ShouldShowContentsWhenExamined = 64,

		/// <summary></summary>
		ShouldShowHealthStatusWhenExamined = 128,

		/// <summary></summary>
		ShouldShowHealthStatusWhenInventoried = 256,

		/// <summary></summary>
		ShouldShowVerboseNameStateDesc = 512,

		/// <summary></summary>
		ShouldRefuseToAcceptGold = 1024,

		/// <summary></summary>
		ShouldRefuseToAcceptDeadBody = 2048,

		/// <summary></summary>
		ShouldPreferNaturalWeaponsToWeakerWeapon = 4096,
	}
}
