
// RecordNameListArgs.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework.Args;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;

namespace Eamon.Game.Args
{
	[ClassMappings]
	public class RecordNameListArgs : IRecordNameListArgs
	{
		public virtual ArticleType ArticleType { get; set; }

		public virtual bool ShowCharOwned { get; set; }

		public virtual StateDescDisplayCode StateDescCode { get; set; }

		public virtual bool ShowContents { get; set; }

		public virtual bool GroupCountOne { get; set; }

		public RecordNameListArgs()
		{

		}
	}
}
