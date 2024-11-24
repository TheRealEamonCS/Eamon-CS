
// PlayerMoveCheckState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static Dharmaquest.Game.Plugin.Globals;

namespace Dharmaquest.Game.States
{
	[ClassMappings]
	public class PlayerMoveCheckState : EamonRT.Game.States.PlayerMoveCheckState, IPlayerMoveCheckState
	{
		public override void ProcessEvents(EventType eventType)
		{
			base.ProcessEvents(eventType);

			if (eventType == EventType.AfterBlockingArtifactCheck)
			{
				// Mermaid death trap / Whirlpool death trap

				if (gGameState.R2 == -70 || gGameState.R2 == -71)
				{
					gOut.Print(gGameState.R2 == -70 ? 
						"Mermaids drag you beneath the surface. Your lungs fill with water and you drown!" :
						"You feel a sudden surge in the water! A giant whirlpool forms and sucks you beneath the surface!");

					gGameState.Die = 1;

					NextState = gEngine.CreateInstance<IPlayerDeadState>(x =>
					{
						x.PrintLineSep = true;
					});

					GotoCleanup = true;

					goto Cleanup;
				}
			}

		Cleanup:

			;
		}
	}
}
