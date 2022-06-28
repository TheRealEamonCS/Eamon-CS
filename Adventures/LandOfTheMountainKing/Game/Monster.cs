﻿
// Monster.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using static LandOfTheMountainKing.Game.Plugin.PluginContext;

namespace LandOfTheMountainKing.Game
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

			return !Globals.IsRulesetVersion(5, 25) && (Reaction == Friendliness.Enemy || (Reaction == Friendliness.Neutral && artifact.Value < 3000));
		}
	}
}
