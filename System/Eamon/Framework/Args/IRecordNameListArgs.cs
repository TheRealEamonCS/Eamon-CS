
// IRecordNameListArgs.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework.Primitive.Enums;

namespace Eamon.Framework.Args
{
	/// <summary></summary>
	public interface IRecordNameListArgs
	{
		/// <summary></summary>
		ArticleType ArticleType { get; set; }

		/// <summary></summary>
		bool ShowCharOwned { get; set; }

		/// <summary></summary>
		StateDescDisplayCode StateDescCode { get; set; }

		/// <summary></summary>
		bool ShowContents { get; set; }

		/// <summary></summary>
		bool GroupCountOne { get; set; }
	}
}
