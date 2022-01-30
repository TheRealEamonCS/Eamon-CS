
// ISpell.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

namespace Eamon.Framework.Primitive.Classes
{
	/// <summary></summary>
	public interface ISpell
	{
		/// <summary></summary>
		string Name { get; set; }

		/// <summary></summary>
		string HokasName { get; set; }

		/// <summary></summary>
		long HokasPrice { get; set; }

		/// <summary></summary>
		long MinValue { get; set; }

		/// <summary></summary>
		long MaxValue { get; set; }
	}
}
