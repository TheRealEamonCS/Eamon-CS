
// Spell.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework.Primitive.Classes;
using Eamon.Game.Attributes;

namespace Eamon.Game.Primitive.Classes
{
	[ClassMappings]
	public class Spell : ISpell
	{
		public virtual string Name { get; set; }

		public virtual string HokasName { get; set; }

		public virtual long HokasPrice { get; set; }

		public virtual long MinValue { get; set; }

		public virtual long MaxValue { get; set; }
	}
}
