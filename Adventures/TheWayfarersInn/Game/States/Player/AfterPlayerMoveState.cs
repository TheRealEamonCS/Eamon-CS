
// AfterPlayerMoveState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using System.Linq;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using TheWayfarersInn.Framework.Primitive.Classes;
using TheWayfarersInn.Framework.Primitive.Enums;
using static TheWayfarersInn.Game.Plugin.Globals;

namespace TheWayfarersInn.Game.States
{
	[ClassMappings]
	public class AfterPlayerMoveState : EamonRT.Game.States.AfterPlayerMoveState, IAfterPlayerMoveState
	{
		public override void ProcessEvents(EventType eventType)
		{
			base.ProcessEvents(eventType);

			if (eventType == EventType.AfterExtinguishLightSourceCheck)
			{
				var unseenApparitionMonster = gMDB[2];

				Debug.Assert(unseenApparitionMonster != null);

				var childsApparitionMonster = gMDB[4];

				Debug.Assert(childsApparitionMonster != null);

				var direWolvesMonster = gMDB[7];

				Debug.Assert(direWolvesMonster != null);

				var nolanMonster = gMDB[24];

				Debug.Assert(nolanMonster != null);

				var woodenBridgeArtifact = gADB[1];

				Debug.Assert(woodenBridgeArtifact != null);

				var mirrorArtifact = gADB[71];

				Debug.Assert(mirrorArtifact != null);

				var northDoorArtifact = gADB[73];

				Debug.Assert(northDoorArtifact != null);

				var direWolfPupsArtifact = gADB[117];

				Debug.Assert(direWolfPupsArtifact != null);

				var bottleOfBourbonArtifact = gADB[130];

				Debug.Assert(bottleOfBourbonArtifact != null);

				var foldedNoteArtifact = gADB[131];

				Debug.Assert(foldedNoteArtifact != null);

				var barArtifact = gADB[132];

				Debug.Assert(barArtifact != null);

				var wildlifeArtifact = gADB[150];

				Debug.Assert(wildlifeArtifact != null);

				var furnitureSetArtifact = gADB[154];

				Debug.Assert(furnitureSetArtifact != null);

				// Child's apparition / Charlotte arrives

				if (gCharRoom.IsWayfarersInnRoom() && gCharRoom.IsLit() && gCharRoom.Uid != 38 && gCharRoom.Uid != 42 && !childsApparitionMonster.IsInLimbo() && !childsApparitionMonster.IsInRoom(gCharRoom))
				{
					childsApparitionMonster.SetInRoom(gCharRoom);
				}

				// Go across the wooden bridge

				if ((gGameState.Ro == 7 && gGameState.R3 == 6) || (gGameState.Ro == 6 && gGameState.R3 == 7))
				{
					gEngine.PrintEffectDesc(6);

					// Too many uses makes wooden bridge a death trap

					gGameState.WoodenBridgeUseCounter++;
				}

				// Go to Wayfarers Inn front door

				if ((gGameState.Ro == 13 && gGameState.R3 == 9) || (gGameState.Ro == 13 && gGameState.R3 == 10) || (gGameState.Ro == 13 && gGameState.R3 == 11) || (gGameState.Ro == 13 && gGameState.R3 == 12))
				{
					gEngine.PrintEffectDesc(8);
				}

				// Go down into boiler room

				if (gGameState.Ro == 34 && gGameState.R3 == 32 && gCharRoom.IsLit())
				{
					gEngine.PrintEffectDesc(10);
				}

				// Go through the magic mirror

				if (gGameState.Ro == 66 && gGameState.R3 == 36)
				{
					mirrorArtifact.Type = ArtifactType.Treasure;

					mirrorArtifact.Field1 = 0;

					mirrorArtifact.Field2 = 0;

					mirrorArtifact.Field3 = 0;

					mirrorArtifact.Field4 = 0;

					mirrorArtifact.Field5 = 0;

					gGameState.MirrorPassphraseSpoken = false;
				}

				// Go up the narrow staircase

				if (gGameState.Ro == 37 && gGameState.R3 == 66 && gCharRoom.IsLit())
				{
					gEngine.PrintEffectDesc(66);
				}

				// Go into bedroom before lucid nightmare

				if (gGameState.Ro == 38 && gGameState.R3 == 37 && !gGameState.CharlotteDeathSeen)
				{
					gEngine.PrintEffectDesc(59);

					northDoorArtifact.DoorGate.SetOpen(false);
				}

				// Go where Charlotte can't follow (to Entryway or Clearing)

				if (((gGameState.Ro == 13 && gGameState.R3 == 23) || (gGameState.Ro == 9 && gGameState.R3 == 45) || (gGameState.Ro == 10 && gGameState.R3 == 57) || (gGameState.Ro == 12 && gGameState.R3 == 59)) && gGameState.CharlotteMet && childsApparitionMonster.IsInRoomUid(gGameState.R3))
				{
					gEngine.PrintEffectDesc(61);
				}

				// Go back to Charlotte (from Entryway)

				if (gGameState.Ro == 23 && gGameState.R3 == 13 && gGameState.CharlotteMet && childsApparitionMonster.IsInRoomUid(gGameState.Ro))
				{
					gEngine.PrintEffectDesc(63);
				}

				// Go into foyer after Charlotte rests in peace; Caldwell family reunited

				if (gGameState.Ro == 23 && gGameState.R3 == 13 && gGameState.CharlotteRestInPeace && !gGameState.CharlotteReunited)
				{
					gEngine.PrintEffectDesc(57);

					gGameState.CharlotteReunited = true;
				}

				// Go into root cellar; bourbon and folded note appear on bar

				if (gGameState.Ro == 29 && gGameState.R3 == 28 && !gGameState.BourbonAppeared)
				{
					bottleOfBourbonArtifact.SetCarriedByContainer(barArtifact, ContainerType.On);

					foldedNoteArtifact.SetCarriedByContainer(barArtifact, ContainerType.On);

					if (barArtifact.IsEmbeddedInRoom())
					{
						barArtifact.SetInRoom(barArtifact.GetEmbeddedInRoom());
					}

					gGameState.BourbonAppeared = true;
				}

				GuestRoomData guestRoomData = null;

				// Go into guest room; add found artifact to furniture set if necessary

				if (gEngine.GuestRoomUids.Contains(gGameState.Ro) && !gEngine.GuestRoomUids.Contains(gGameState.R3) && gGameState.GuestRoomDictionary.TryGetValue(gGameState.Ro, out guestRoomData))
				{
					if (guestRoomData.FoundArtifactUid != 0)
					{
						var foundArtifact = gADB[guestRoomData.FoundArtifactUid];

						Debug.Assert(foundArtifact != null);

						foundArtifact.SetCarriedByContainer(furnitureSetArtifact, guestRoomData.FoundArtifactContainerType);

						guestRoomData.FoundArtifactUid = 0;
					}
				}

				// Exit guest room; remove found artifact from furniture set if necessary

				if (!gEngine.GuestRoomUids.Contains(gGameState.Ro) && gEngine.GuestRoomUids.Contains(gGameState.R3) && gGameState.GuestRoomDictionary.TryGetValue(gGameState.R3, out guestRoomData))
				{
					var foundArtifact = gEngine.GetArtifactList(a => a.IsCarriedByContainer(furnitureSetArtifact)).FirstOrDefault();

					if (foundArtifact != null)
					{
						guestRoomData.FoundArtifactUid = foundArtifact.Uid;

						foundArtifact.SetInLimbo();
					}
				}

				// Move dire wolves / dire wolf pups back into kennel

				if (gGameState.Ro != gGameState.R3 && direWolvesMonster.IsInRoomUid(gGameState.R3) && direWolfPupsArtifact.IsInRoomUid(gGameState.R3))
				{
					direWolvesMonster.SetInRoomUid(22);

					direWolfPupsArtifact.SetCarriedByContainerUid(15);
				}

				// Make sure some artifacts are in limbo

				if (gGameState.Ro != gGameState.R3)
				{
					wildlifeArtifact.SetInLimbo();
				}
			
				// Spin up event state machines

				if (!unseenApparitionMonster.IsInLimbo() && (gCharRoom.IsWayfarersInnRoom() || gCharRoom.IsWayfarersInnClearingRoom()))
				{
					var eventState = gGameState.GetEventState(EventState.UnseenApparition);

					if (eventState == 0)
					{
						eventState++;

						gGameState.SetEventState(EventState.UnseenApparition, eventState);

						gGameState.AfterPrintPlayerRoomEventHeap.Insert02(gGameState.CurrTurn + 1, "UnseenApparition", 0, null);
					}
				}

				if (!childsApparitionMonster.IsInLimbo() && gCharRoom.IsWayfarersInnRoom())
				{
					var eventState = gGameState.GetEventState(EventState.ChildsApparition);

					if (eventState == 0)
					{
						eventState = 1;

						gGameState.SetEventState(EventState.ChildsApparition, eventState);

						gGameState.AfterPrintPlayerRoomEventHeap.Insert02(gGameState.CurrTurn + 1, "ChildsApparition", 0, null);
					}
				}

				if (!nolanMonster.IsInLimbo())
				{
					var eventState = gGameState.GetEventState(EventState.Nolan);

					if (eventState == 0)
					{
						eventState++;

						gGameState.SetEventState(EventState.Nolan, eventState);

						gGameState.AfterPrintPlayerRoomEventHeap.Insert02(gGameState.CurrTurn + 1, "Nolan", 0, null);
					}
				}

				if (gCharRoom.IsForestRoom())
				{
					var eventState = gGameState.GetEventState(EventState.Forest);

					if (eventState == 0)
					{
						eventState++;

						gGameState.SetEventState(EventState.Forest, eventState);

						gGameState.AfterPrintPlayerRoomEventHeap.Insert02(gGameState.CurrTurn + 1, "Forest", 0, null);
					}
				}

				if (gCharRoom.IsRiverRoom())
				{
					var eventState = gGameState.GetEventState(EventState.River);

					if (eventState == 0)
					{
						eventState++;

						gGameState.SetEventState(EventState.River, eventState);

						gGameState.AfterPrintPlayerRoomEventHeap.Insert02(gGameState.CurrTurn + 1, "River", 0, null);
					}
				}
			}
		}
	}
}
