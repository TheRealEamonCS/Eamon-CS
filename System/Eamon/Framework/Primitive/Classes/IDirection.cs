
// IDirection.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework.Primitive.Enums;

namespace Eamon.Framework.Primitive.Classes
{
	/// <summary></summary>
	public interface IDirection
	{
		/// <summary></summary>
		string Name { get; set; }

		/// <summary></summary>
		string PrintedName { get; set; }

		/// <summary></summary>
		string Abbr { get; set; }

		/// <summary></summary>
		Direction EnterDir { get; set; }
	}
}
