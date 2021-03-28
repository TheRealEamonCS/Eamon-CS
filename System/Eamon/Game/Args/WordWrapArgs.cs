
// WordWrapArgs.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework.Args;
using Eamon.Game.Attributes;

namespace Eamon.Game.Args
{
	[ClassMappings]
	public class WordWrapArgs : IWordWrapArgs
	{
		public virtual long CurrColumn { get; set; }

		public virtual char LastChar { get; set; }

		public WordWrapArgs()
		{
			LastChar = '\0';
		}
	}
}
