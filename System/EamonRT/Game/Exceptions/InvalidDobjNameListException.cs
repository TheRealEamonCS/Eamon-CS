
// InvalidDobjNameListException.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;

namespace EamonRT.Game.Exceptions
{
	public class InvalidDobjNameListException : Exception
	{
		public virtual string DobjNameStr { get; set; }

		public InvalidDobjNameListException(string dobjNameStr)
		{
			DobjNameStr = dobjNameStr;
		}
	}
}
