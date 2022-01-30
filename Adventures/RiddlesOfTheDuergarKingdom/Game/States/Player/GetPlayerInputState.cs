
// GetPlayerInputState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using System.Linq;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Components;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static RiddlesOfTheDuergarKingdom.Game.Plugin.PluginContext;

namespace RiddlesOfTheDuergarKingdom.Game.States
{
	[ClassMappings]
	public class GetPlayerInputState : EamonRT.Game.States.GetPlayerInputState, IGetPlayerInputState
	{
		public override void ProcessEvents(EventType eventType)
		{
			base.ProcessEvents(eventType);

			if (eventType == EventType.BeforePrintCommandPrompt)
			{
				Debug.Assert(gCharMonster != null);

				var room = gCharMonster.GetInRoom();

				Debug.Assert(room != null);

				// Player pushing ore cart

				if (gGameState.PushingOreCart && gSentenceParser.IsInputExhausted)
				{
					gOut.Print("You are pushing an ore cart.");
				}

				// Archaeology Department's shambolic retreat from excavation site

				if (gGameState.VolcanoErupting && gEngine.ArchaeologyDepartmentAbandonedRoomUids.Contains(room.Uid) && room.IsLit() && gSentenceParser.IsInputExhausted)
				{
					gEngine.PrintEffectDesc(13);
				}

				var waterfallArtifact = gADB[59];

				Debug.Assert(waterfallArtifact != null);

				var mistRoomUids = new long[] { 55, 56, 57, 58, 59, 60 };

				// White mist in magma chamber

				if (waterfallArtifact.IsInRoomUid(57) && mistRoomUids.Contains(room.Uid) && gSentenceParser.IsInputExhausted)
				{
					gOut.Print("The area is shrouded in a{0} white mist.", room.Uid == 57 ? " thick" : room.Uid == 55 || room.Uid == 59 ? " thin" : "");
				}

				if (ShouldPreTurnProcess())
				{
					var gradStudentCompanionMonster = gMDB[gGameState.GradStudentCompanionUid];

					Debug.Assert(gradStudentCompanionMonster != null);

					// Grad student companion hides behind player for protection

					if (!gGameState.GradStudentRetreats && gradStudentCompanionMonster.IsInRoom(room) && gradStudentCompanionMonster.Reaction == Friendliness.Friend && gGameState.GetNBTL(Friendliness.Enemy) > 0 && gSentenceParser.IsInputExhausted)
					{
						if (room.IsLit())
						{
							gEngine.PrintEffectDesc(32);
						}

						gGameState.GradStudentRetreats = true;
					}

					// People try not to retch from the stench

					if (gGameState.SewagePitVisited && room.IsLit() && gEngine.RollDice(1, 100, 0) > 90 && gSentenceParser.IsInputExhausted)
					{
						var monsterList = gEngine.GetMonsterList(m => m.IsInRoom(room) && gEngine.ArchaeologyDepartmentMonsterUids.Contains(m.Uid));

						if (monsterList.Count > 0)
						{
							gEngine.PrintEffectDesc(81);
						}
					}

					var blacksmithSmockArtifact = gADB[67];

					Debug.Assert(blacksmithSmockArtifact != null);

					var blacksmithSmockArtifact2 = gADB[68];

					Debug.Assert(blacksmithSmockArtifact2 != null);

					// In the broiling rooms

					if (gEngine.BroilingRoomUids.Contains(gGameState.Ro) && !blacksmithSmockArtifact.IsWornByCharacter() && !blacksmithSmockArtifact2.IsWornByCharacter())
					{
						gEngine.PrintEffectDesc(41);

						var combatComponent = Globals.CreateInstance<ICombatComponent>(x =>
						{
							x.SetNextStateFunc = s => NextState = s;

							x.ActorRoom = gCharMonster.GetInRoom();

							x.Dobj = gCharMonster;

							x.OmitArmor = true;
						});

						var dice = gGameState.Ro >= 77 ? 1 : !gEngine.LavaRiverRoomUids.Contains(gGameState.Ro) ? 2 : 3;

						combatComponent.ExecuteCalculateDamage(dice, 1);

						if (gGameState.Die > 0)
						{
							GotoCleanup = true;

							goto Cleanup;
						}
					}

					// In the sewage pit (carrying lit lantern)

					if (gGameState.Ro == 141 && room.IsLit())
					{
						gEngine.PrintEffectDesc(80);

						gGameState.Die = 1;

						NextState = Globals.CreateInstance<IPlayerDeadState>(x =>
						{
							x.PrintLineSep = true;
						});

						GotoCleanup = true;

						goto Cleanup;
					}

					var armorArtifact = gGameState.Ar > 0 ? gADB[gGameState.Ar] : null;

					// In the placid lake (wearing more than leather armor)

					if (gGameState.Ro == 40 && armorArtifact != null && armorArtifact.Wearable.Field1 > 2)
					{
						gEngine.PrintEffectDesc(82);

						gGameState.Die = 1;

						NextState = Globals.CreateInstance<IPlayerDeadState>(x =>
						{
							x.PrintLineSep = true;
						});

						GotoCleanup = true;

						goto Cleanup;
					}
				}
			}

		Cleanup:

			;
		}
	}
}
