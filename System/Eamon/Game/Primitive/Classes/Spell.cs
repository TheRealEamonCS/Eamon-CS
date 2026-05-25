
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

		public virtual long MinRange { get; set; }

		public virtual long OptimalMin { get; set; }

		public virtual long OptimalMax { get; set; }

		public virtual long MaxRange { get; set; }
		
		public virtual long CloseOddsModifier { get; set; }

		public virtual long CloseDmgMultiplier { get; set; }

		public virtual long FarOddsModifier { get; set; }

		public virtual long FarDmgMultiplier { get; set; }
	}
}
