
// Monster.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using static TheTrainingGround.Game.Plugin.Globals;

namespace TheTrainingGround.Game
{
	[ClassMappings]
	public class Monster : Eamon.Game.Monster, IMonster
	{
		public override bool ShouldRefuseToAcceptGold()
		{
			return false;
		}

		public override bool ShouldRefuseToAcceptGift(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			return ((Uid == 5 && artifact.Uid == 8) || (Uid == 14 && artifact.Uid == 51)) ? false : (!gEngine.IsRulesetVersion(5, 62) && (Reaction == Friendliness.Enemy || (Reaction == Friendliness.Neutral && artifact.Value < 3000)));
		}
	}
}
