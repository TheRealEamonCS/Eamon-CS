
// Armor.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework.Primitive.Classes;
using Eamon.Game.Attributes;

namespace Eamon.Game.Primitive.Classes
{
	[ClassMappings]
	public class Armor : IArmor
	{
		public virtual string Name { get; set; }

		public virtual string MarcosName { get; set; }

		public virtual long MarcosPrice { get; set; }

		public virtual long MarcosNum { get; set; }

		public virtual long ArtifactValue { get; set; }
	}
}
