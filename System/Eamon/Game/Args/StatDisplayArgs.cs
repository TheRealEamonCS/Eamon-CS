
// StatDisplayArgs.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework;
using Eamon.Framework.Args;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;

namespace Eamon.Game.Args
{
	[ClassMappings]
	public class StatDisplayArgs : IStatDisplayArgs
	{
		public virtual IMonster Monster { get; set; }

		public virtual string ArmorString { get; set; }

		public virtual long[] SpellAbilities { get; set; }

		public virtual long Speed { get; set; }

		public virtual long CharmMon { get; set; }

		public virtual long Weight { get; set; }

		public virtual long GetSpellAbilities(long index)
		{
			return SpellAbilities[index];
		}

		public virtual long GetSpellAbilities(Spell spell)
		{
			return GetSpellAbilities((long)spell);
		}
	}
}
