
// ArtifactLinkage.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework.Primitive.Classes;
using Eamon.Game.Attributes;

namespace Eamon.Game.Primitive.Classes
{
	[ClassMappings]
	public class ArtifactLinkage : IArtifactLinkage
	{
		public virtual long RoomUid { get; set; }

		public virtual long ArtifactUid1 { get; set; }

		public virtual long ArtifactUid2 { get; set; }
	}
}
