
// GuestRoomData.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework.Primitive.Enums;

namespace TheWayfarersInn.Framework.Primitive.Classes
{
	public class GuestRoomData
	{
		public virtual string FurnitureSetDesc { get; set; }

		public virtual ContainerType FoundArtifactContainerType { get; set; }

		public virtual long FoundArtifactUid { get; set; }
	}
}
