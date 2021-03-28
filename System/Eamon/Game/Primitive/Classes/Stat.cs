
// Stat.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework.Primitive.Classes;
using Eamon.Game.Attributes;

namespace Eamon.Game.Primitive.Classes
{
	[ClassMappings]
	public class Stat : IStat
	{
		public virtual string Name { get; set; }

		public virtual string Abbr { get; set; }

		public virtual string EmptyVal { get; set; }

		public virtual long MinValue { get; set; }

		public virtual long MaxValue { get; set; }
	}
}
