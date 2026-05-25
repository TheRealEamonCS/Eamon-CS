
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

		/// <summary></summary>
		long MinRange { get; set; }

		/// <summary></summary>
		long OptimalMin { get; set; }

		/// <summary></summary>
		long OptimalMax { get; set; }

		/// <summary></summary>
		long MaxRange { get; set; }
		
		/// <summary></summary>
		long CloseOddsModifier { get; set; }

		/// <summary></summary>
		long CloseDmgMultiplier { get; set; }

		/// <summary></summary>
		long FarOddsModifier { get; set; }

		/// <summary></summary>
		long FarDmgMultiplier { get; set; }
	}
}
