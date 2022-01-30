
// ICharacterArtifact.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

namespace Eamon.Framework.Primitive.Classes
{
	/// <summary></summary>
	public interface ICharacterArtifact
	{
		/// <summary></summary>
		ICharacter Parent { get; set; }

		/// <summary></summary>
		string Name { get; set; }

		/// <summary></summary>
		string Desc { get; set; }

		/// <summary></summary>
		bool IsPlural { get; set; }

		/// <summary></summary>
		Enums.PluralType PluralType { get; set; }

		/// <summary></summary>
		Enums.ArticleType ArticleType { get; set; }

		/// <summary></summary>
		long Value { get; set; }

		/// <summary></summary>
		long Weight { get; set; }

		/// <summary></summary>
		Enums.ArtifactType Type { get; set; }

		/// <summary></summary>
		long Field1 { get; set; }

		/// <summary></summary>
		long Field2 { get; set; }

		/// <summary></summary>
		long Field3 { get; set; }

		/// <summary></summary>
		long Field4 { get; set; }

		/// <summary></summary>
		long Field5 { get; set; }

		/// <summary></summary>
		/// <returns></returns>
		bool IsActive();

		/// <summary></summary>
		void ClearExtraFields();
	}
}
