
// AfterPlayerMoveState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static ThePyramidOfAnharos.Game.Plugin.Globals;

namespace ThePyramidOfAnharos.Game.States
{
	[ClassMappings]
	public class AfterPlayerMoveState : EamonRT.Game.States.AfterPlayerMoveState, IAfterPlayerMoveState
	{
		public override void ProcessEvents(EventType eventType)
		{
			base.ProcessEvents(eventType);

			if (eventType == EventType.AfterMoveMonsters)
			{
				var crazedHermitMonster = gMDB[5];

				Debug.Assert(crazedHermitMonster != null);

				var statueArtifact = gADB[63];

				Debug.Assert(statueArtifact != null);

				// Snuff torch in Black room

				if (gGameState.Ro == 39 && gGameState.Ls > 0)
				{
					var lsArtifact = gADB[gGameState.Ls];

					Debug.Assert(lsArtifact != null);

					gEngine.PrintEffectDesc(9);

					lsArtifact.RemoveStateDesc(lsArtifact.GetProvidingLightDesc());

					gGameState.Ls = 0;
				}

				// Move statue back into Chamber of Alaxar

				if (!statueArtifact.IsInLimbo() && statueArtifact.GetInRoomUid() != 30)
				{
					statueArtifact.SetInRoomUid(30);
				}

				// Reset crazed hermit's reaction

				crazedHermitMonster.ResolveReaction(gCharacter);
			}
			else if (eventType == EventType.AfterExtinguishLightSourceCheck)
			{
				var ropeArtifact = gADB[13];

				Debug.Assert(ropeArtifact != null);

				// Rope

				if ((gGameState.Ro == 22 && gGameState.R3 == 25) || (gGameState.Ro == 25 && gGameState.R3 == 22))
				{
					ropeArtifact.SetInRoomUid(gGameState.Ro);
				}
			}
		}
	}
}
