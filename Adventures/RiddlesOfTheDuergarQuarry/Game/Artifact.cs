
// Artifact.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using static RiddlesOfTheDuergarQuarry.Game.Plugin.PluginContext;

namespace RiddlesOfTheDuergarQuarry.Game
{
	[ClassMappings]
	public class Artifact : Eamon.Game.Artifact, IArtifact
	{
		public override bool ShouldExposeContentsToRoom(ContainerType containerType = ContainerType.In)
		{
			// Pack animals exposed in corral, even when it's closed

			return Uid != 72 || containerType != ContainerType.In ? base.ShouldExposeContentsToRoom(containerType) : true;
		}

		public override bool ShouldExposeInContentsWhenClosed()
		{
			// Pack animals exposed in corral, even when it's closed

			return Uid != 72 ? base.ShouldExposeInContentsWhenClosed() : true;
		}
	}
}
