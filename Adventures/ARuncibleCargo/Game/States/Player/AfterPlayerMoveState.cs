
// AfterPlayerMoveState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static ARuncibleCargo.Game.Plugin.PluginContext;

namespace ARuncibleCargo.Game.States
{
	[ClassMappings]
	public class AfterPlayerMoveState : EamonRT.Game.States.AfterPlayerMoveState, IAfterPlayerMoveState
	{
		public override void ProcessEvents(EventType eventType)
		{
			if (eventType == EventType.AfterExtinguishLightSourceCheck)
			{
				// Boat stuff (an Eamon tradition)

				if ((gGameState.Ro == 97 && gGameState.R3 == 31) || (gGameState.Ro == 99 && gGameState.R3 == 32) || (gGameState.Ro == 101 && gGameState.R3 == 67))
				{
					gEngine.PrintEffectDesc(113);
				}

				if ((gGameState.Ro == 31 && gGameState.R3 == 97) || (gGameState.Ro == 32 && gGameState.R3 == 99) || (gGameState.Ro == 67 && gGameState.R3 == 101))
				{
					gEngine.PrintEffectDesc(114);
				}

				// Sync contents of water rooms

				var oldRoom = gRDB[gGameState.R3] as Framework.IRoom;

				var newRoom = gRDB[gGameState.Ro] as Framework.IRoom;

				if (oldRoom != null && oldRoom.IsWaterRoom() && newRoom != null)
				{
					if (!newRoom.IsWaterRoom())
					{
						var artifactList = gEngine.GetArtifactList(a => a.IsInRoom(oldRoom));

						if (artifactList.Count > 0)
						{
							gOut.Print("You also move the contents of the boat onto the shore.");
						}
					}

					gEngine.TransportRoomContentsBetweenRooms(oldRoom, newRoom, false);
				}

				// Out the Window of Ill Repute

				if (gGameState.Ro == 21 && gGameState.R3 == 50)
				{
					gEngine.PrintEffectDesc(49);
				}
			}

			base.ProcessEvents(eventType);
		}
	}
}
