﻿
// PrintPlayerRoomState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using System.Linq;
using Eamon;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static ARuncibleCargo.Game.Plugin.Globals;

namespace ARuncibleCargo.Game.States
{
	[ClassMappings]
	public class PrintPlayerRoomState : EamonRT.Game.States.PrintPlayerRoomState, IPrintPlayerRoomState
	{
		public override void ProcessEvents(EventType eventType)
		{
			RetCode rc;

			base.ProcessEvents(eventType);

			var gameState = gEngine.GameState as Framework.IGameState;

			Debug.Assert(gameState != null);

			if (eventType == EventType.BeforePrintPlayerRoom && gEngine.ShouldPreTurnProcess)
			{
				var charMonster = gMDB[gameState.Cm];

				Debug.Assert(charMonster != null);

				var room = charMonster.GetInRoom();

				Debug.Assert(room != null);

				//   Special events part 1
				//  -----------------------

				// Move from dream to fire

				if (room.Uid < 7 && gameState.DreamCounter >= 13)
				{
					var lookCommand = gEngine.LastCommand as ILookCommand;

					if (lookCommand != null)
					{
						gEngine.PrintPlayerRoom(room);
					}

					// gEngine.SentenceParser.PrintDiscardingCommands() not called for this abrupt reality shift

					gEngine.ResetProperties(PropertyResetCode.SwitchContext);

					// Nothing in the dream affects the real world; revert game state now that player is awake

					var filesetTable = gEngine.CloneInstance(gDatabase.FilesetTable);

					Debug.Assert(filesetTable != null);

					var gameState01 = gEngine.CloneInstance(gameState);

					Debug.Assert(gameState01 != null);

					rc = gEngine.PopDatabase();

					Debug.Assert(gEngine.IsSuccess(rc));

					rc = gEngine.RestoreDatabase(gEngine.SnapshotFileName);

					Debug.Assert(gEngine.IsSuccess(rc));

					gDatabase.FilesetTable = filesetTable;

					gEngine.Config = gEngine.GetConfig();

					gEngine.Character = gDatabase.CharacterTable.Records.FirstOrDefault();

					gEngine.Module = gEngine.GetModule();

					gEngine.GameState = gEngine.GetGameState();

					gameState = gEngine.GameState as Framework.IGameState;

					gameState.PookaMet = gEngine.CloneInstance(gameState01.PookaMet);

					gameState.Ro = 7;

					gameState.R2 = 7;

					gameState.R3 = 7;

					gameState.Vr = gameState01.Vr;

					gameState.Vm = gameState01.Vm;

					gameState.Va = gameState01.Va;

					gameState.Vn = gameState01.Vn;

					gameState.MatureContent = gameState01.MatureContent;

					gameState.InteractiveFiction = gameState01.InteractiveFiction;

					gameState.EnhancedCombat = gameState01.EnhancedCombat;

					gameState.EnhancedParser = gameState01.EnhancedParser;

					gameState.IobjPronounAffinity = gameState01.IobjPronounAffinity;

					gameState.ShowPronounChanges = gameState01.ShowPronounChanges;

					gameState.ShowFulfillMessages = gameState01.ShowFulfillMessages;

					gameState.PauseCombatMs = gameState01.PauseCombatMs;

					gameState.PauseCombatActions = gameState01.PauseCombatActions;

					charMonster = gEngine.MDB[gameState.Cm];

					room = gEngine.RDB[gameState.Ro];

					charMonster.SetInRoom(room);			// TODO: determine if AfterPlayerMoveState is needed

					gEngine.PrintEffectDesc(7);
				}

				// Out the burning window

				if (room.Uid == 8 && !gameState.FireEscaped)
				{
					gEngine.PrintEffectDesc(8);

					gameState.FireEscaped = true;
				}

				Eamon.Framework.Primitive.Classes.IArtifactCategory ac = null;

				// Shop doors

				if (gameState.R3 == 19 && (room.Uid == 12 || room.Uid == 20))
				{
					gameState.R3 = 12;

					var shopDoorArtifact = gADB[room.Uid == 20 ? 136 : 17];

					Debug.Assert(shopDoorArtifact != null);

					ac = shopDoorArtifact.DoorGate;

					Debug.Assert(ac != null);

					ac.SetOpen(false);

					gEngine.PrintEffectDesc(48);
				}

				// Out the Window of Ill Repute

				if (gameState.R3 == 18 && room.Uid == 50 && !gameState.AmazonMet)
				{
					gEngine.PrintEffectDesc(47);

					gameState.AmazonMet = true;
				}

				// Bandit camp

				if (room.Uid == 59 && !gameState.CampEntered)
				{
					gameState.R3 *= gameState.CargoInRoom;

					gEngine.PrintEffectDesc(68);

					gameState.CampEntered = true;
				}

				var larkspurMonster = gMDB[36];

				Debug.Assert(larkspurMonster != null);

				// Meet Larkspur

				if (room.Uid == 88 && larkspurMonster.IsInRoom(room) && !larkspurMonster.Seen)
				{
					gEngine.PrintEffectDesc(92);
				}

				var lilMonster = gMDB[37];

				Debug.Assert(lilMonster != null);

				var cargoArtifact = gADB[129];

				Debug.Assert(cargoArtifact != null);

				// Lil steals Runcible Cargo

				if (room.Uid != 102 && room.Uid != 43 && (room.Uid < 86 || room.Uid > 88))
				{
					if ((cargoArtifact.IsInRoom(room) || cargoArtifact.IsCarriedByMonster(gCharMonster)) && lilMonster.IsInRoom(room))
					{
						cargoArtifact.SetCarriedByMonster(lilMonster);

						gEngine.PrintEffectDesc(119);

						lilMonster.Friendliness = (Friendliness)100;

						lilMonster.Reaction = Friendliness.Enemy;
					}
				}

				var princeMonster = gMDB[38];

				Debug.Assert(princeMonster != null);

				// The Prince would like the Cargo, please

				if (room.Uid == 96 && princeMonster.IsInRoom(room) && !gameState.PrinceMet)
				{
					gameState.R3 = 96;

					gEngine.PrintEffectDesc(125);

					gameState.PrinceMet = true;
				}

				if (room.Uid == 96 && gameState.R3 == 95 && princeMonster.IsInRoom(room) && princeMonster.Reaction > Friendliness.Enemy && !cargoArtifact.IsCarriedByMonster(princeMonster) && gameState.PrinceMet)
				{
					gameState.R3 = 96;

					gEngine.PrintEffectDesc(127);
				}

				var guardsMonster = gMDB[39];

				Debug.Assert(guardsMonster != null);

				var gatesArtifact = gADB[137];

				Debug.Assert(gatesArtifact != null);

				// Gates of Frukendorf slam shut

				if (room.Uid == 93 && cargoArtifact.IsCarriedByMonster(princeMonster) && princeMonster.Reaction > Friendliness.Enemy && !guardsMonster.IsInLimbo())
				{
					ac = gatesArtifact.DoorGate;

					Debug.Assert(ac != null);

					if (ac.IsOpen())
					{
						ac.SetOpen(false);

						gEngine.PrintEffectDesc(140);
					}
				}

				ac = cargoArtifact.InContainer;

				Debug.Assert(ac != null);

				// Cargo open counter

				if ((cargoArtifact.IsInRoom(room) || cargoArtifact.IsCarriedByMonster(gCharMonster)) && ac.IsOpen())
				{
					gameState.CargoOpenCounter++;

					if (gameState.CargoOpenCounter == 3)
					{
						ac.SetOpen(false);

						gEngine.PrintEffectDesc(130);
					}
				}
			}
		}
	}
}
