
// Direction.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework.Primitive.Classes;
using Eamon.Game.Attributes;
using Enums = Eamon.Framework.Primitive.Enums;

namespace Eamon.Game.Primitive.Classes
{
	[ClassMappings]
	public class Direction : IDirection
	{
		public virtual string Name { get; set; }

		public virtual string PrintedName { get; set; }

		public virtual string Abbr { get; set; }

		public virtual Enums.Direction EnterDir { get; set; }
	}
}
