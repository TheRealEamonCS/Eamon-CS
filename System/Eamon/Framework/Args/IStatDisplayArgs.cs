
// IStatDisplayArgs.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework.Primitive.Enums;

namespace Eamon.Framework.Args
{
	/// <summary></summary>
	public interface IStatDisplayArgs
	{
		#region Properties

		/// <summary></summary>
		IMonster Monster { get; set; }

		/// <summary></summary>
		string ArmorString { get; set; }

		/// <summary></summary>
		long[] SpellAbilities { get; set; }

		/// <summary></summary>
		long Speed { get; set; }

		/// <summary></summary>
		long CharmMon { get; set; }

		/// <summary></summary>
		long Weight { get; set; }

		#endregion

		#region Methods

		/// <summary></summary>
		/// <param name="index"></param>
		/// <returns></returns>
		long GetSpellAbilities(long index);

		/// <summary></summary>
		/// <param name="spell"></param>
		/// <returns></returns>
		long GetSpellAbilities(Spell spell);

		#endregion
	}
}
