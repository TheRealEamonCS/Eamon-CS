
// BeforePlayerMoveState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static OrbOfMyLife.Game.Plugin.Globals;

namespace OrbOfMyLife.Game.States
{
	[ClassMappings]
	public class BeforePlayerMoveState : EamonRT.Game.States.BeforePlayerMoveState, IBeforePlayerMoveState
	{
		public override bool ShouldEnemiesNearbyPreventMovement()
		{
			// Invisible

			return !gGameState.IV ? base.ShouldEnemiesNearbyPreventMovement() : false;
		}
	}
}
