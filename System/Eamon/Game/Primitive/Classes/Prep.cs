
// Prep.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework.Primitive.Classes;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;

namespace Eamon.Game.Primitive.Classes
{
	[ClassMappings]
	public class Prep : IPrep
	{
		public virtual string Name { get; set; }

		public virtual ContainerType ContainerType { get; set; }
	}
}
