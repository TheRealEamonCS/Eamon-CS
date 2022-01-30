
// IMonsterSpell.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

namespace Eamon.Framework.Primitive.Classes
{
	/// <summary></summary>
	public interface IMonsterSpell
	{
		/// <summary></summary>
		IMonster Parent { get; set; }

		/// <summary></summary>
		Enums.Spell Spell { get; set; }

		/// <summary></summary>
		long Field1 { get; set; }

		/// <summary></summary>
		long Field2 { get; set; }
	}
}
