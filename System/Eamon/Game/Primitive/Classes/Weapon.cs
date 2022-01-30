
// Weapon.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework.Primitive.Classes;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;

namespace Eamon.Game.Primitive.Classes
{
	[ClassMappings]
	public class Weapon : IWeapon
	{
		public virtual string Name { get; set; }

		public virtual string EmptyVal { get; set; }

		public virtual string MarcosName { get; set; }

		public virtual bool MarcosIsPlural { get; set; }

		public virtual PluralType MarcosPluralType { get; set; }

		public virtual ArticleType MarcosArticleType { get; set; }

		public virtual long MarcosPrice { get; set; }

		public virtual long MarcosDice { get; set; }

		public virtual long MarcosSides { get; set; }

		public virtual long MarcosNumHands { get; set; }

		public virtual long MinValue { get; set; }

		public virtual long MaxValue { get; set; }
	}
}
