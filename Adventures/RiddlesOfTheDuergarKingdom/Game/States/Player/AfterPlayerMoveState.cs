
// AfterPlayerMoveState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using System.Linq;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using RiddlesOfTheDuergarKingdom.Framework.Commands;
using static RiddlesOfTheDuergarKingdom.Game.Plugin.Globals;

namespace RiddlesOfTheDuergarKingdom.Game.States
{
	[ClassMappings]
	public class AfterPlayerMoveState : EamonRT.Game.States.AfterPlayerMoveState, IAfterPlayerMoveState
	{
		public override void ProcessEvents(EventType eventType)
		{
			base.ProcessEvents(eventType);

			if (eventType == EventType.AfterExtinguishLightSourceCheck)
			{
				Debug.Assert(gCharMonster != null);

				var room = gCharMonster.GetInRoom();

				Debug.Assert(room != null);

				var ropeArtifact = gADB[69];

				Debug.Assert(ropeArtifact != null);

				// Climb up the caldera cliff

				if (gGameState.Ro == 42 && gGameState.R3 == 37)
				{
					// gEngine.PrintEffectDesc(???);			// TODO: implement

					ropeArtifact.Weight = 0;					// TODO: fix

					ropeArtifact.Field1 = 37;
				}

				// Climb down the caldera cliff

				if (gGameState.Ro == 37 && gGameState.R3 == 42)
				{
					// gEngine.PrintEffectDesc(???);			// TODO: implement

					ropeArtifact.Weight = -999;

					ropeArtifact.Field1 = 42;
				}

				// Enter the placid lake

				if (gGameState.Ro == 40 && (gEngine.BeachRoomUids.Contains(gGameState.R3) || gGameState.R3 == 41))
				{
					var lsArtifact = gGameState.Ls > 0 ? gADB[gGameState.Ls] : null;

					if (lsArtifact != null && lsArtifact.IsCarriedByMonster(gCharMonster))
					{
						gEngine.LightOut(lsArtifact);
					}

					// gEngine.PrintEffectDesc(???);			// TODO: implement

					if (gGameState.SewagePitVisited)
					{
						// gEngine.PrintEffectDesc(???);			// TODO: implement

						gGameState.SewagePitVisited = false;
					}
				}

				// Enter the latrine

				if (gGameState.Ro == 141 && gGameState.R3 == 138)
				{
					// gEngine.PrintEffectDesc(???);			// TODO: implement

					gGameState.SewagePitVisited = true;
				}

				var gradStudentCompanionFollows = false;

				// Climb up the large boulder

				if (gGameState.Ro == 135 && gGameState.R3 == 4)
				{
					gEngine.PrintEffectDesc(37);

					gradStudentCompanionFollows = true;
				}

				// Climb down the large boulder

				if (gGameState.Ro == 4 && gGameState.R3 == 135)
				{
					gEngine.PrintEffectDesc(38);

					gradStudentCompanionFollows = true;
				}

				// Climb up the wooden ladder

				if (gGameState.Ro == 23 && gGameState.R3 == 35)
				{
					gEngine.PrintEffectDesc(44);

					gradStudentCompanionFollows = true;
				}

				// Climb down the wooden ladder

				if (gGameState.Ro == 35 && gGameState.R3 == 23)
				{
					gEngine.PrintEffectDesc(45);

					gradStudentCompanionFollows = true;
				}

				// Climb into the wooden cart

				if (gGameState.Ro == 136 && (gGameState.R3 == 31 || gGameState.R3 == 84))
				{
					gEngine.PrintEffectDesc(46);

					gradStudentCompanionFollows = true;
				}

				// Climb out of the wooden cart

				if (gGameState.Ro == 31 && gGameState.R3 == 136 && room.IsLit())
				{
					gEngine.PrintEffectDesc(47);

					gradStudentCompanionFollows = true;
				}

				// Climb out of the wooden cart

				if (gGameState.Ro == 84 && gGameState.R3 == 136 && room.IsLit())
				{
					gEngine.PrintEffectDesc(48);

					gradStudentCompanionFollows = true;
				}

				// Enter the canvas tents

				if ((gGameState.Ro == 129 && gGameState.R3 == 14) || 
					(gGameState.Ro == 133 && gGameState.R3 == 15) || 
					(gGameState.Ro == 131 && gGameState.R3 == 16) || 
					(gGameState.Ro == 130 && gGameState.R3 == 18) ||
					(gGameState.Ro == 134 && gGameState.R3 == 19) ||
					(gGameState.Ro == 132 && gGameState.R3 == 20) ||
					(gGameState.Ro == 155 && gGameState.R3 == 22))
				{
					gEngine.PrintEffectDesc(49);

					gradStudentCompanionFollows = true;
				}

				// Exit the canvas tents

				if ((gGameState.Ro == 14 && gGameState.R3 == 129) ||
					(gGameState.Ro == 15 && gGameState.R3 == 133) ||
					(gGameState.Ro == 16 && gGameState.R3 == 131) ||
					(gGameState.Ro == 18 && gGameState.R3 == 130) ||
					(gGameState.Ro == 19 && gGameState.R3 == 134) ||
					(gGameState.Ro == 20 && gGameState.R3 == 132) ||
					(gGameState.Ro == 22 && gGameState.R3 == 155))
				{
					gEngine.PrintEffectDesc(50);

					gradStudentCompanionFollows = true;
				}

				var gradStudentCompanionMonster = gMDB[gGameState.GradStudentCompanionUid];

				Debug.Assert(gradStudentCompanionMonster != null);

				// Grad student companion follows close behind

				if (gradStudentCompanionFollows && room.IsLit() && gradStudentCompanionMonster.IsInRoom(room) && gradStudentCompanionMonster.Reaction == Friendliness.Friend)
				{
					gEngine.PrintEffectDesc(52);
				}

				var tracksArtifact = gADB[44];

				Debug.Assert(tracksArtifact != null);

				var oreCartTracksArtifact = gADB[45];

				Debug.Assert(oreCartTracksArtifact != null);

				var oreCartArtifact = gADB[46];

				Debug.Assert(oreCartArtifact != null);

				// Push ore cart on tracks

				if (gEngine.LastCommand != null && gEngine.LastCommand.Type == CommandType.Movement)
				{
					var lastCommandGoFleeClimb = gEngine.LastCommand is IGoCommand || gEngine.LastCommand is IFleeCommand || gEngine.LastCommand is IClimbCommand;

					if (!lastCommandGoFleeClimb && !gEngine.EnemyInExitedDarkRoom && tracksArtifact.IsInRoom(room) && oreCartArtifact.IsCarriedByContainer() && gGameState.PushingOreCart)
					{
						gEngine.PrintEffectDesc(65);

						oreCartTracksArtifact.SetInRoom(room);

						gGameState.OreCartTracksRoomUid = room.Uid;
					}
					else
					{
						gGameState.PushingOreCart = false;
					}
				}
			}
		}

		public override void Execute()
		{
			var room = gRDB[gGameState.Ro];

			gEngine.EnemyInExitedDarkRoom = room != null && !room.IsLit() && gGameState.GetNBTL(Friendliness.Enemy) > 0;

			base.Execute();

			gGameState.GradStudentRetreats = false;
		}
	}
}
