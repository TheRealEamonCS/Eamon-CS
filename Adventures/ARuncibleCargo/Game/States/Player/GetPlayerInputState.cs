
// GetPlayerInputState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using System.Linq;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Components;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static ARuncibleCargo.Game.Plugin.Globals;

namespace ARuncibleCargo.Game.States
{
	[ClassMappings]
	public class GetPlayerInputState : EamonRT.Game.States.GetPlayerInputState, IGetPlayerInputState
	{
		public override void ProcessEvents(EventType eventType)
		{
			base.ProcessEvents(eventType);

			if (eventType == EventType.BeforePrintCommandPrompt && gEngine.ShouldPreTurnProcess)
			{
				Debug.Assert(gCharMonster != null);

				var room = gCharMonster.GetInRoom();

				Debug.Assert(room != null);

				//   Special events part 2
				//  -----------------------

				// Dream effects

				if (room.Uid < 7)
				{
					if (gGameState.DreamCounter == 4)
					{
						gEngine.PrintEffectDesc(1);
					}

					if (gGameState.DreamCounter == 8)
					{
						gEngine.PrintEffectDesc(2);
					}

					if (gGameState.DreamCounter == 12)
					{
						gEngine.PrintEffectDesc(3);
					}

					gGameState.DreamCounter++;
				}

				var fortunetellerArtifact = gADB[101];

				Debug.Assert(fortunetellerArtifact != null);

				// Swarmy

				if (room.Uid == 66 && fortunetellerArtifact.IsInRoom(room))
				{
					if (gGameState.SwarmyCounter == 1)
					{
						gEngine.PrintEffectDesc(91);
					}

					gGameState.SwarmyCounter++;

					if (gGameState.SwarmyCounter > 3)
					{
						gGameState.SwarmyCounter = 1;
					}
				}

				// In the burning room

				if (room.Uid == 7)
				{
					gEngine.PrintEffectDesc(13);

					var combatComponent = gEngine.CreateInstance<ICombatComponent>(x =>
					{
						x.SetNextStateFunc = s => NextState = s;

						x.ActorRoom = gCharMonster.GetInRoom();

						x.Dobj = gCharMonster;

						x.OmitArmor = true;
					});

					combatComponent.ExecuteCalculateDamage(1, 1);

					if (gGameState.Die > 0)
					{
						GotoCleanup = true;

						goto Cleanup;
					}
				}

				var hokasTokasMonster = gMDB[4];

				Debug.Assert(hokasTokasMonster != null);

				// Hokas scolds you

				if (room.Uid == 8 && gGameState.R3 == 9 && hokasTokasMonster.IsInRoom(room))
				{
					hokasTokasMonster.SetInRoomUid(1);

					gEngine.PrintEffectDesc(11);

					gEngine.PrintPlayerRoom(room);
				}

				var ovenArtifact = gADB[80];

				Debug.Assert(ovenArtifact != null);

				var ac = ovenArtifact.InContainer;

				Debug.Assert(ac != null);

				var billMonster = gMDB[23];

				Debug.Assert(billMonster != null);

				// Bill in oven

				if (room.Uid == 55 && !ac.IsOpen() && !billMonster.Seen)
				{
					gEngine.PrintEffectDesc(52);
				}

				// Bandit camp

				if (room.Uid == 59 && gGameState.R3 == 58)
				{
					gGameState.R3 = 59;

					gEngine.PrintEffectDesc(71 + gGameState.CargoInRoom);
				}

				var westernDoorArtifact = gADB[112];

				Debug.Assert(westernDoorArtifact != null);

				var easternDoorArtifact = gADB[113];

				Debug.Assert(easternDoorArtifact != null);

				ac = easternDoorArtifact.DoorGate;

				Debug.Assert(ac != null);

				// University doors

				if (room.Uid == 74 && billMonster.IsInRoom(room) && ac.GetKeyUid() == -1)
				{
					ac.SetKeyUid(0);

					ac.SetOpen(true);

					gEngine.PrintEffectDesc(86);
				}

				if (room.Uid == 74 && (gGameState.R3 == 85 || gGameState.R3 == 79))
				{
					var doorArtifact = easternDoorArtifact;

					var effectUid = 87L;

					if (gGameState.R3 == 79)
					{
						doorArtifact = westernDoorArtifact;

						effectUid = 152;
					}

					gGameState.R3 = room.Uid;

					Debug.Assert(doorArtifact != null);

					ac = doorArtifact.DoorGate;

					Debug.Assert(ac != null);

					if (!ac.IsOpen())
					{
						ac.SetKeyUid(0);

						ac.SetOpen(true);

						gEngine.PrintEffectDesc(effectUid);
					}
				}

				var jailCellArtifact = gADB[41];

				Debug.Assert(jailCellArtifact != null);

				ac = jailCellArtifact.InContainer;

				Debug.Assert(ac != null);

				var lilMonster = gMDB[37];

				Debug.Assert(lilMonster != null);

				// Lil in jail

				if (room.Uid == 102 && !ac.IsOpen() && !lilMonster.Seen)
				{
					gEngine.PrintEffectDesc(122);
				}

				var amazonMonster = gMDB[22];

				Debug.Assert(amazonMonster != null);

				// Amazon and Bill

				if (amazonMonster.IsInRoom(room) && billMonster.IsInRoom(room) && !gGameState.BillAndAmazonMeet)
				{
					gEngine.PrintEffectDesc(53);

					gGameState.BillAndAmazonMeet = true;
				}

				var princeAndGuardsDead = false;

				var commanderMonster = gMDB[27];

				Debug.Assert(commanderMonster != null);

				var soldiersMonster = gMDB[28];

				Debug.Assert(soldiersMonster != null);

				// Bandit Commander and all soldiers are dead!

				var commanderAndSoldiersDead = commanderMonster.IsInLimbo() && soldiersMonster.IsInLimbo();

				if (!commanderAndSoldiersDead)
				{
					var princeMonster = gMDB[38];

					Debug.Assert(princeMonster != null);

					var guardsMonster = gMDB[39];

					Debug.Assert(guardsMonster != null);

					// Bandit Prince and all Praetorian Guards are dead!

					princeAndGuardsDead = princeMonster.IsInLimbo() && guardsMonster.IsInLimbo();
				}

				if (commanderAndSoldiersDead || princeAndGuardsDead)
				{
					var effectUid = commanderAndSoldiersDead ? 60L : 142L;

					gOut.Print("{0}", gEngine.LineSep);

					gEngine.PrintEffectDesc(effectUid);

					gEngine.In.KeyPress(gEngine.Buf);

					gGameState.Die = 0;

					gEngine.ExitType = ExitType.FinishAdventure;

					gEngine.MainLoop.ShouldShutdown = true;

					NextState = gEngine.CreateInstance<IStartState>();

					GotoCleanup = true;

					goto Cleanup;
				}

				// Amazon and Bill warn about Lil

				if (lilMonster.IsInRoom(room))       // && (room.Uid == 18 || room.Uid == 21 || room.Uid == 38 || room.Uid == 53)
				{
					if (!gGameState.AmazonLilWarning && amazonMonster.IsInRoom(room) && amazonMonster.Reaction == Friendliness.Friend && room.Uid != 50 && room.IsViewable())
					{
						gEngine.PrintEffectDesc(117);

						gGameState.AmazonLilWarning = true;
					}

					if (!gGameState.BillLilWarning && billMonster.IsInRoom(room) && billMonster.Reaction == Friendliness.Friend && room.Uid != 55 && room.IsViewable())
					{
						gEngine.PrintEffectDesc(118);

						gGameState.BillLilWarning = true;
					}
				}

				var cargoArtifact = gADB[129];

				Debug.Assert(cargoArtifact != null);

				var explosiveDeviceArtifact = gADB[43];

				Debug.Assert(explosiveDeviceArtifact != null);

				var remoteDetonatorArtifact = gADB[45];

				Debug.Assert(remoteDetonatorArtifact != null);

				// Bill (or Amazon) hint at using explosives

				if (room.Uid == 92 && (cargoArtifact.IsInRoom(room) || cargoArtifact.IsCarriedByMonster(gCharMonster)) && (explosiveDeviceArtifact.IsInRoom(room) || explosiveDeviceArtifact.IsCarriedByMonster(gCharMonster)) && (remoteDetonatorArtifact.IsInRoom(room) || remoteDetonatorArtifact.IsCarriedByMonster(gCharMonster)))
				{
					var effectUid = 0L;

					if (amazonMonster.IsInRoom(room) && amazonMonster.Reaction == Friendliness.Friend)
					{
						effectUid = 124;
					}

					if (billMonster.IsInRoom(room) && billMonster.Reaction == Friendliness.Friend)
					{
						effectUid = 123;
					}

					if (effectUid > 0 && !gGameState.Explosive)
					{
						gEngine.PrintEffectDesc(effectUid);

						gGameState.Explosive = true;
					}
				}

				Eamon.Framework.Primitive.Classes.IArtifactCategory ac01 = null;

				// Maintenance grate, sewer grate, and (Barney) Rubble

				var doubleDoorList = gEngine.DoubleDoorList.Where(dd => dd.RoomUid == room.Uid).ToList();

				foreach (var dd in doubleDoorList)
				{
					var doorArtifact = gADB[dd.ArtifactUid1];

					Debug.Assert(doorArtifact != null);

					var doorArtifact01 = gADB[dd.ArtifactUid2];

					Debug.Assert(doorArtifact01 != null);

					ac = doorArtifact.DoorGate;

					Debug.Assert(ac != null);

					ac01 = doorArtifact01.DoorGate;

					Debug.Assert(ac01 != null);

					doorArtifact01.Seen = doorArtifact.Seen;

					doorArtifact01.StateDesc = gEngine.CloneInstance(doorArtifact.StateDesc);

					ac01.Field2 = ac.Field2;

					ac01.Field3 = ac.Field3;
				}
			}
			
		Cleanup:

			;
		}
	}
}
