
// GetPlayerInputState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static WrenholdsSecretVigil.Game.Plugin.Globals;

namespace WrenholdsSecretVigil.Game.States
{
	[ClassMappings]
	public class GetPlayerInputState : EamonRT.Game.States.GetPlayerInputState, IGetPlayerInputState
	{
		public override void ProcessEvents(EventType eventType)
		{
			base.ProcessEvents(eventType);

			if (eventType == EventType.BeforePrintCommandPrompt)
			{
				var lifeOrbArtifact = gADB[4];

				Debug.Assert(lifeOrbArtifact != null);

				var magicCubeArtifact = gADB[5];

				Debug.Assert(magicCubeArtifact != null);

				// Magic cube code

				if (magicCubeArtifact.IsCarriedByCharacter() && gGameState.Ro >= 40 && lifeOrbArtifact.IsCarriedByContainerUid(49) && gSentenceParser.IsInputExhausted)
				{
					var characterRoom = gRDB[gGameState.Ro];

					Debug.Assert(characterRoom != null);

					if (gGameState.Ro == 69)
					{
						gOut.Print("All sides of the magic cube are glowing!");
					}
					else
					{
						var dir = "south";

						if (gGameState.Ro == 67 || gGameState.Ro == 68)
						{
							dir = "west";
						}
						else if (characterRoom.GetDir(Direction.East) > 0 && characterRoom.GetDir(Direction.East) <= gEngine.Module.NumRooms)
						{
							dir = "east";
						}

						gOut.Print("The {0} side of the magic cube is glowing!", dir);
					}
				}
			}
		}
	}
}
