
// AfterPlayerMoveState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Game.Attributes;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static RiddlesOfTheDuergarQuarry.Game.Plugin.PluginContext;

namespace RiddlesOfTheDuergarQuarry.Game.States
{
	[ClassMappings]
	public class AfterPlayerMoveState : EamonRT.Game.States.AfterPlayerMoveState, IAfterPlayerMoveState
	{
		public override void ProcessEvents(EventType eventType)
		{
			if (eventType == EventType.AfterExtinguishLightSourceCheck)
			{
				// Climb up the giant boulder

				if (gGameState.Ro == 53 && gGameState.R3 == 46)
				{
					gEngine.PrintEffectDesc(37);
				}

				// Climb down the giant boulder

				if (gGameState.Ro == 46 && gGameState.R3 == 53)
				{
					gEngine.PrintEffectDesc(38);
				}
			}

			base.ProcessEvents(eventType);
		}
	}
}
