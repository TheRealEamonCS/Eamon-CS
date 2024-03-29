﻿
// SearchCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using System.Linq;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static TheVileGrimoireOfJaldial.Game.Plugin.Globals;

namespace TheVileGrimoireOfJaldial.Game.Commands
{
	[ClassMappings]
	public class SearchCommand : EamonRT.Game.Commands.Command, Framework.Commands.ISearchCommand
	{
		public override void ExecuteForPlayer()
		{
			gOut.Print("Okay, you're doing a thorough search now.");

			gGameState.Minute += 15;

			if (gGameState.WeatherDuration > 0)
			{
				gGameState.WeatherDuration -= 15;

				if (gGameState.WeatherDuration < 0)
				{
					gGameState.WeatherDuration = 0;
				}
			}

			if (gGameState.Ls > 0)
			{
				// TODO: decide if further burn down of light source is warranted
			}

			if (gGameState.Speed > 1)
			{
				gGameState.Speed = 1;
			}

			var notFoundDesc = "You find nothing new.";

			var foundDesc = "You find something!";

			var found = false;

			var fountainSecretDoorFound = false;

			var saved = gEngine.SaveThrow(Stat.Intellect);

			var saved02 = gEngine.SaveThrow(Stat.Intellect);

			var waterArtifact = gADB[40];

			Debug.Assert(waterArtifact != null);

			if (DobjArtifact != null)
			{
				var crystalGobletArtifact = gADB[12];

				Debug.Assert(crystalGobletArtifact != null);

				var crimsonCloakArtifact = gADB[19];

				Debug.Assert(crimsonCloakArtifact != null);

				var goldPiecesArtifact = gADB[20];

				Debug.Assert(goldPiecesArtifact != null);

				var pouchOfStonesArtifact = gADB[21];

				Debug.Assert(pouchOfStonesArtifact != null);

				var griffinEggArtifact = gADB[22];

				Debug.Assert(griffinEggArtifact != null);

				var grimoireArtifact = gADB[27];

				Debug.Assert(grimoireArtifact != null);

				// Dragon's treasure hoard

				if (DobjArtifact.Uid == 11)
				{
					if (saved && crystalGobletArtifact.IsInLimbo())
					{
						foundDesc = "You find an interesting item!";

						crystalGobletArtifact.SetInRoom(ActorRoom);

						found = true;
					}
				}

				// Beholder's treasure hoard

				else if (DobjArtifact.Uid == 18)
				{
					if (saved && crimsonCloakArtifact.IsInLimbo())
					{
						foundDesc = "After tearing the hoard apart, you find some interesting items!";

						crimsonCloakArtifact.SetInRoom(ActorRoom);

						goldPiecesArtifact.SetInRoom(ActorRoom);

						pouchOfStonesArtifact.SetInRoom(ActorRoom);

						found = true;
					}
				}

				// Large nest

				else if (DobjArtifact.Uid == 23)
				{
					if (saved && griffinEggArtifact.IsInLimbo())
					{
						foundDesc = "You find a secret compartment, in which something is hidden!";

						griffinEggArtifact.SetCarriedByContainer(DobjArtifact);

						found = true;
					}
				}

				// Large fountain

				else if (DobjArtifact.Uid == 24)
				{
					if (waterArtifact.IsInLimbo())
					{
						if (saved && !gGameState.GetSecretDoor(12))
						{
							foundDesc = "You find a secret door at the bottom of the fountain!";

							gGameState.SetSecretDoor(12, true);

							found = true;

							fountainSecretDoorFound = true;
						}
					}
					else
					{
						notFoundDesc = "There is a large pool of water in this fountain.  You can't see the murky bottom.";
					}
				}

				// Wooden throne

				else if (DobjArtifact.Uid == 26)
				{
					if (saved && grimoireArtifact.IsInLimbo())
					{
						foundDesc = "You find a secret compartment in the tree stump!";

						grimoireArtifact.SetInRoom(ActorRoom);

						found = true;
					}
				}
			}
			else
			{
				var tapestryArtifact = gADB[2];

				Debug.Assert(tapestryArtifact != null);

				var lanternArtifact = gADB[39];

				Debug.Assert(lanternArtifact != null);

				if (ActorRoom.Uid == 36)
				{
					if (saved && lanternArtifact.IsInLimbo())
					{
						foundDesc = "After scouring the area, you find something of interest!";

						lanternArtifact.SetInRoom(ActorRoom);

						found = true;
					}
				}
				else if (ActorRoom.Uid == 54)
				{
					if (saved && !gGameState.GetSecretDoor(1))
					{
						gGameState.SetSecretDoor(1, true);

						found = true;
					}
					else if (saved02 && !gGameState.GetSecretDoor(2))
					{
						gGameState.SetSecretDoor(2, true);

						found = true;
					}
				}
				else if (ActorRoom.Uid == 55)
				{
					if (saved && !gGameState.GetSecretDoor(1))
					{
						gGameState.SetSecretDoor(1, true);

						found = true;
					}
				}
				else if (ActorRoom.Uid == 56)
				{
					if (saved && !gGameState.GetSecretDoor(2))
					{
						gGameState.SetSecretDoor(2, true);

						found = true;
					}
					else if (saved02 && !gGameState.GetSecretDoor(4))
					{
						gGameState.SetSecretDoor(4, true);

						found = true;
					}
				}
				else if (ActorRoom.Uid == 58)
				{
					if (saved && !gGameState.GetSecretDoor(3))
					{
						gGameState.SetSecretDoor(3, true);

						found = true;
					}
				}
				else if (ActorRoom.Uid == 63)
				{
					if (saved && !gGameState.GetSecretDoor(3))
					{
						gGameState.SetSecretDoor(3, true);

						found = true;
					}
				}
				else if (ActorRoom.Uid == 68)
				{
					if (saved && !gGameState.GetSecretDoor(4))
					{
						gGameState.SetSecretDoor(4, true);

						found = true;
					}
				}
				else if (ActorRoom.Uid == 74)
				{
					if (saved && !gGameState.GetSecretDoor(5))
					{
						gGameState.SetSecretDoor(5, true);

						found = true;
					}
					else if (saved02 && !gGameState.GetSecretDoor(6))
					{
						gGameState.SetSecretDoor(6, true);

						found = true;
					}
				}
				else if (ActorRoom.Uid == 87)
				{
					if (saved && !gGameState.GetSecretDoor(7))
					{
						gGameState.SetSecretDoor(7, true);

						found = true;
					}
				}
				else if (ActorRoom.Uid == 89)
				{
					if (saved && tapestryArtifact.IsInLimbo())
					{
						foundDesc = "You find an interesting tapestry!";

						tapestryArtifact.SetInRoom(ActorRoom);

						found = true;
					}
				}
				else if (ActorRoom.Uid == 100)
				{
					if (saved && !gGameState.GetSecretDoor(9))
					{
						gGameState.SetSecretDoor(9, true);

						found = true;
					}
				}
				else if (ActorRoom.Uid == 101)
				{
					if (saved && !gGameState.GetSecretDoor(8))
					{
						gGameState.SetSecretDoor(8, true);

						found = true;
					}
				}
				else if (ActorRoom.Uid == 102)
				{
					if (saved && !gGameState.GetSecretDoor(11))
					{
						gGameState.SetSecretDoor(11, true);

						found = true;
					}
				}
				else if (ActorRoom.Uid == 115)
				{
					if (saved && !gGameState.GetSecretDoor(10))
					{
						gGameState.SetSecretDoor(10, true);

						found = true;
					}
				}
				else if (ActorRoom.Uid == 116)
				{
					if (waterArtifact.IsInLimbo())
					{
						if (saved && !gGameState.GetSecretDoor(12))
						{
							gGameState.SetSecretDoor(12, true);

							found = true;

							fountainSecretDoorFound = true;
						}
					}
				}
			}

			if (found)
			{
				gOut.Print(foundDesc);

				NextState = gEngine.CreateInstance<IStartState>();
			}
			else
			{
				gOut.Print(notFoundDesc);
			}

			// Large fountain becomes a DoorGate

			if (fountainSecretDoorFound)
			{
				var largeFountainArtifact = gADB[24];

				Debug.Assert(largeFountainArtifact != null);

				// Fountain cannot be both an InContainer and a DoorGate, an illegal combination - must empty contents

				var artifactList = largeFountainArtifact.GetContainedList();

				foreach (var a in artifactList)
				{
					a.SetInRoom(ActorRoom);
				}

				largeFountainArtifact.Type = ArtifactType.DoorGate;

				largeFountainArtifact.Field1 = 117;

				largeFountainArtifact.Field2 = -1;

				largeFountainArtifact.Field3 = 0;

				largeFountainArtifact.Field4 = 0;

				largeFountainArtifact.Field5 = 0;

				largeFountainArtifact.Synonyms = largeFountainArtifact.Synonyms.Concat(new string[] { "secret door", "secret panel", "door", "panel" }).ToArray();
			}

			if (NextState == null)
			{
				NextState = gEngine.CreateInstance<IMonsterStartState>();
			}
		}

		public SearchCommand()
		{
			SortOrder = 440;

			IsNew = true;

			Name = "SearchCommand";

			Verb = "search";

			Type = CommandType.Interactive;
		}
	}
}
