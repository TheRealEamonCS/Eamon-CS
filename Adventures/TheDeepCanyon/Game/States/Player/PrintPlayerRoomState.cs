
// PrintPlayerRoomState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static TheDeepCanyon.Game.Plugin.PluginContext;

namespace TheDeepCanyon.Game.States
{
	[ClassMappings]
	public class PrintPlayerRoomState : EamonRT.Game.States.PrintPlayerRoomState, IPrintPlayerRoomState
	{
		public override void ProcessEvents(EventType eventType)
		{
			base.ProcessEvents(eventType);

			if (eventType == EventType.BeforePrintPlayerRoom && Globals.ShouldPreTurnProcess)
			{
				Debug.Assert(gCharMonster != null);

				var room = gCharMonster.GetInRoom();

				Debug.Assert(room != null);

				var rl = gEngine.RollDice(1, 100, 0);

				// Resurrect monster

				if (Globals.ResurrectMonsterUid > 0)
				{
					var resurrectMonster = gMDB[Globals.ResurrectMonsterUid];

					Debug.Assert(resurrectMonster != null);

					gOut.Print("{0}{1} comes back to life!", Environment.NewLine, room.IsLit() ? resurrectMonster.GetTheName(true) : "Something");

					resurrectMonster.DmgTaken = 0;

					gEngine.MagicRingLowersMonsterStats(resurrectMonster);

					var weaponArtifact = resurrectMonster.Weapon > 0 ? gADB[resurrectMonster.Weapon] : null;

					if (weaponArtifact != null)
					{
						weaponArtifact.SetInRoom(room);

						weaponArtifact.RemoveStateDesc(weaponArtifact.GetReadyWeaponDesc());

						resurrectMonster.Weapon = -weaponArtifact.Uid - 1;
					}

					resurrectMonster.SetInRoom(room);

					Globals.ResurrectMonsterUid = 0;
				}

				var trapArtifact = gADB[17];

				Debug.Assert(trapArtifact != null);

				var mouseArtifact = gADB[19];

				Debug.Assert(mouseArtifact != null);

				var elephantsMonster = gMDB[24];

				Debug.Assert(elephantsMonster != null);

				// Mouse trap catches mouse

				if (mouseArtifact.IsInLimbo() && trapArtifact.IsInRoomUid(80) && rl >= 85 && gGameState.TrapSet)
				{
					if (room.Uid == 80)
					{
						gOut.Print(room.IsLit() ? "You see a mouse scrurry around in the shadows and run back into a hole." : "You hear something scrurry around in the darkness.");
					}
					else
					{
						if (room.Uid == 23 || room.Uid == 20)
						{
							gOut.Print("You hear something scurrying around and a bell ring!");
						}

						gGameState.TrapSet = false;

						trapArtifact.InContainer.SetOpen(false);

						mouseArtifact.SetCarriedByContainer(trapArtifact);
					}
				}

				var mouseRoom = mouseArtifact.GetInRoom();

				// Mouse scares off elephants

				if (mouseRoom != null && elephantsMonster.IsInRoom(mouseRoom))
				{
					if (mouseArtifact.IsInRoom(room))
					{
						gOut.Print(room.IsLit() ? "As you release the mouse it darts to the elephants. The elephants run away in fright." : "You hear a brief panicked stampede and then silence.");
					}

					elephantsMonster.SetInLimbo();
				}

				rl = gEngine.RollDice(1, 100, 0);

				// Mouse escapes

				if (mouseRoom != null && rl >= 50)
				{
					if (mouseArtifact.IsInRoom(room))
					{
						if (room.IsLit())
						{
							gOut.Print("The mouse scurries off {0}.", room.EvalRoomType("into the shadows", "into the underbrush"));
						}
						else
						{
							gOut.Print("You hear something scrurry around in the darkness.");
						}
					}

					mouseArtifact.SetInLimbo();
				}

				var ringArtifact = gADB[22];

				Debug.Assert(ringArtifact != null);

				// Squirrel gives ring

				if (room.Uid == 2 && Globals.LastCommand != null && Globals.LastCommand.Type != CommandType.Movement && !gGameState.SquirrelRing)
				{
					gEngine.PrintEffectDesc(2);

					ringArtifact.SetCarriedByCharacter();           // TODO: put in room if too heavy to carry

					gGameState.SquirrelRing = true;
				}

				rl = gEngine.RollDice(1, 100, 0);

				// Blue Dragon appears

				if (room.Uid < 12 && rl >= 90 && !gGameState.BlueDragon)
				{
					gEngine.PrintEffectDesc(6);

					gGameState.BlueDragon = true;
				}

				var fidoMonster = gMDB[11];

				Debug.Assert(fidoMonster != null);

				// Fido sleeping

				if (gGameState.FidoSleepCounter > 0)
				{
					rl = gEngine.RollDice(1, 15, 0);

					gGameState.FidoSleepCounter -= rl;

					if (gGameState.FidoSleepCounter <= 0)
					{
						if (room.Uid == 26)
						{
							gOut.Print("{0} wakes up!", room.IsLit() ? "Fido" : "Something");
						}

						gGameState.FidoSleepCounter = 0;

						fidoMonster.StateDesc = "";

						fidoMonster.Reaction = Friendliness.Enemy;
					}
				}
			}
		}
	}
}
