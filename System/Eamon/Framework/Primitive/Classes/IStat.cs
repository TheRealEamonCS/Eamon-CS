
// IStat.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

namespace Eamon.Framework.Primitive.Classes
{
	/// <summary></summary>
	public interface IStat
	{
		/// <summary></summary>
		string Name { get; set; }

		/// <summary></summary>
		string Abbr { get; set; }

		/// <summary></summary>
		string EmptyVal { get; set; }

		/// <summary></summary>
		long MinValue { get; set; }

		/// <summary></summary>
		long MaxValue { get; set; }
	}
}
