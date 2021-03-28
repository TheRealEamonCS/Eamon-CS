
// Artifact.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using static ARuncibleCargo.Game.Plugin.PluginContext;

namespace ARuncibleCargo.Game
{
	[ClassMappings]
	public class Artifact : Eamon.Game.Artifact, IArtifact
	{
		public override bool IsDisguisedMonster()
		{
			// Bill in oven, Lil in cell

			return Uid != 82 && Uid != 135 && base.IsDisguisedMonster();
		}

		public override bool ShouldExposeContentsToRoom(ContainerType containerType = ContainerType.In)
		{
			// Dodge Hotel basement shelves don't expose contents

			return Uid != 58 && base.ShouldExposeContentsToRoom(containerType);
		}

		public override bool ShouldShowContentsWhenOpened()
		{
			// Skip Cargo contents if empty

			if (Uid == 129)
			{
				var artifactList = GetContainedList();

				return artifactList.Count > 0;
			}
			else
			{
				return base.ShouldShowContentsWhenOpened();
			}
		}
	}
}
