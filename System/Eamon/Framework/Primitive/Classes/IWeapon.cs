
// IWeapon.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

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
	}
}
