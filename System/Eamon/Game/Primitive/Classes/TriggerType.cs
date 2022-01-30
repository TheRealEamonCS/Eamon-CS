
// TriggerType.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework.Primitive.Classes;
using Eamon.Game.Attributes;

namespace Eamon.Game.Primitive.Classes
{
	[ClassMappings]
	public class TriggerType : ITriggerType
	{
		public virtual string Name { get; set; }

		public virtual string Field1Name { get; set; }

		public virtual string Field1EmptyVal { get; set; }

		public virtual string Field2Name { get; set; }

		public virtual string Field2EmptyVal { get; set; }

		public virtual string Field3Name { get; set; }

		public virtual string Field3EmptyVal { get; set; }

		public virtual string Field4Name { get; set; }

		public virtual string Field4EmptyVal { get; set; }

		public virtual string Field5Name { get; set; }

		public virtual string Field5EmptyVal { get; set; }
	}
}
