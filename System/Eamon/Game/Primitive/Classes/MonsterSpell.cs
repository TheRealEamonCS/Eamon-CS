
// MonsterSpell.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Polenter.Serialization;
using Eamon.Framework;
using Eamon.Framework.Primitive.Classes;
using Eamon.Game.Attributes;
using Enums = Eamon.Framework.Primitive.Enums;

namespace Eamon.Game.Primitive.Classes
{
	[ClassMappings]
	public class MonsterSpell : IMonsterSpell
	{
		[ExcludeFromSerialization]
		public virtual IMonster Parent { get; set; }

		public virtual Enums.Spell Spell { get; set; }

		public virtual long Field1 { get; set; }

		public virtual long Field2 { get; set; }
	}
}
