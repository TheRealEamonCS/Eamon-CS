
// IWeapon.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework.Primitive.Enums;

namespace Eamon.Framework.Primitive.Classes
{
	/// <summary></summary>
	public interface IWeapon
	{
		/// <summary></summary>
		string Name { get; set; }

		/// <summary></summary>
		string EmptyVal { get; set; }

		/// <summary></summary>
		string MarcosName { get; set; }

		/// <summary></summary>
		bool MarcosIsPlural { get; set; }

		/// <summary></summary>
		Enums.PluralType MarcosPluralType { get; set; }

		/// <summary></summary>
		Enums.ArticleType MarcosArticleType { get; set; }

		/// <summary></summary>
		long MarcosPrice { get; set; }

		/// <summary></summary>
		long MarcosDice { get; set; }

		/// <summary></summary>
		long MarcosSides { get; set; }

		/// <summary></summary>
		long MarcosNumHands { get; set; }

		/// <summary></summary>
		long MinValue { get; set; }

		/// <summary></summary>
		long MaxValue { get; set; }

		/// <summary></summary>
		AmmoType AmmoType { get; set; }

		/// <summary></summary>
		long AmmoCount { get; set; }

		/// <summary></summary>
		long MaxAmmoCount { get; set; }

		/// <summary></summary>
		long AmmoRecoveryOdds { get; set; }

		/// <summary></summary>
		AmmoRefillCode AmmoRefillCode { get; set; }

		/// <summary></summary>
		long MinRange { get; set; }

		/// <summary></summary>
		long OptimalMin { get; set; }

		/// <summary></summary>
		long OptimalMax { get; set; }

		/// <summary></summary>
		long MaxRange { get; set; }
		
		/// <summary></summary>
		long CloseOddsModifier { get; set; }

		/// <summary></summary>
		long CloseDmgMultiplier { get; set; }

		/// <summary></summary>
		long FarOddsModifier { get; set; }

		/// <summary></summary>
		long FarDmgMultiplier { get; set; }
	}
}
