
// PlayerMoveCheckState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Game.Attributes;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static ARuncibleCargo.Game.Plugin.Globals;

namespace ARuncibleCargo.Game.States
{
	[ClassMappings]
	public class PlayerMoveCheckState : EamonRT.Game.States.PlayerMoveCheckState, IPlayerMoveCheckState
	{
		public override void PrintRideOffIntoSunset()
		{
			gOut.Print("You ride off into the moonlight.");
		}

		public override void ProcessEvents(EventType eventType)
		{
			base.ProcessEvents(eventType);

			if (eventType == EventType.AfterBlockingArtifactCheck && gGameState.R2 == -34)
			{
				gOut.Print("The broken opening is too small to fit through.");

				GotoCleanup = true;
			}
		}
	}
}
