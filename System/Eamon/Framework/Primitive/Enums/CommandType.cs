
// CommandType.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

namespace Eamon.Framework.Primitive.Enums
{
	/// <summary></summary>
	public enum CommandType : long
	{
		/// <summary></summary>
		None = 0,

		/// <summary></summary>
		Movement,
		
		/// <summary></summary>
		Manipulation,
		
		/// <summary></summary>
		Interactive,
		
		/// <summary></summary>
		Miscellaneous
	}
}
