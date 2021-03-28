
// GetPlayerInputState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static WrenholdsSecretVigil.Game.Plugin.PluginContext;

namespace WrenholdsSecretVigil.Game.States
{
	[ClassMappings]
	public class GetPlayerInputState : EamonRT.Game.States.GetPlayerInputState, IGetPlayerInputState
	{
		public override void ProcessEvents(EventType eventType)
		{
			if (eventType == EventType.BeforeCommandPromptPrint)
			{
				var lifeOrbArtifact = gADB[4];

				Debug.Assert(lifeOrbArtifact != null);

				var magicCubeArtifact = gADB[5];

				Debug.Assert(magicCubeArtifact != null);

				// Magic cube code

				if (magicCubeArtifact.IsCarriedByCharacter() && gGameState.Ro >= 40 && lifeOrbArtifact.IsCarriedByContainerUid(49) && string.IsNullOrWhiteSpace(gSentenceParser.ParserInputStr))
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
						else if (characterRoom.GetDirs(Direction.East) > 0 && characterRoom.GetDirs(Direction.East) <= Globals.Module.NumRooms)
						{
							dir = "east";
						}

						gOut.Print("The {0} side of the magic cube is glowing!", dir);
					}
				}
			}

			base.ProcessEvents(eventType);
		}
	}
}
