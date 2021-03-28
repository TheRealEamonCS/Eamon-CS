
// GetPlayerInputState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static BeginnersForest.Game.Plugin.PluginContext;

namespace BeginnersForest.Game.States
{
	[ClassMappings]
	public class GetPlayerInputState : EamonRT.Game.States.GetPlayerInputState, IGetPlayerInputState
	{
		public override void ProcessEvents(EventType eventType)
		{
			if (eventType == EventType.BeforeCommandPromptPrint && ShouldPreTurnProcess())
			{
				var fairyQueenMonster = gMDB[8];

				Debug.Assert(fairyQueenMonster != null);

				// Meet Fairy Queen

				if (fairyQueenMonster.Location == gGameState.Ro && fairyQueenMonster.Field1 == 0)
				{
					gEngine.PrintEffectDesc(gGameState.QueenGiftEffectUid);

					var crownArtifact = gADB[gGameState.QueenGiftArtifactUid];

					Debug.Assert(crownArtifact != null);

					crownArtifact.SetInRoomUid(gGameState.Ro);

					var grassBladeArtifact = gADB[6];

					Debug.Assert(grassBladeArtifact != null);

					// Grass blade turns into Greenblade

					if (grassBladeArtifact.IsCarriedByCharacter() || grassBladeArtifact.IsInRoomUid(gGameState.Ro))
					{
						gEngine.PrintEffectDesc(20);

						grassBladeArtifact.SetInLimbo();

						var greenBladeArtifact = gADB[8];

						Debug.Assert(greenBladeArtifact != null);

						greenBladeArtifact.SetInRoomUid(gGameState.Ro);
					}

					fairyQueenMonster.Field1 = 1;
				}
			}

			base.ProcessEvents(eventType);
		}
	}
}
