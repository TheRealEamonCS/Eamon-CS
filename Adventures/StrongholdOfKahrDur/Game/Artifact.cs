
// Artifact.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework;
using Eamon.Game.Attributes;

namespace StrongholdOfKahrDur.Game
{
	[ClassMappings]
	public class Artifact : Eamon.Game.Artifact, IArtifact
	{
		public override bool IsAttackable()
		{
			return Uid != 3 && Uid != 11 && Uid != 15 ? base.IsAttackable() : false;
		}
	}
}
