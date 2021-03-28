
// Artifact.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework;
using Eamon.Game.Attributes;

namespace TheTempleOfNgurct.Game
{
	[ClassMappings]
	public class Artifact : Eamon.Game.Artifact, IArtifact
	{
		public override bool IsReadyableByMonsterUid(long monsterUid)
		{
			// Only player can wield fireball wand

			return Uid != 63 && base.IsReadyableByMonsterUid(monsterUid);
		}
	}
}
