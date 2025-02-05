
// PrintPlayerRoomState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static OrbOfMyLife.Game.Plugin.Globals;

namespace OrbOfMyLife.Game.States
{
	[ClassMappings]
	public class PrintPlayerRoomState : EamonRT.Game.States.PrintPlayerRoomState, IPrintPlayerRoomState
	{
		public override void ProcessEvents(EventType eventType)
		{
			if (gGameState.Die > 0)
			{
				goto Cleanup;
			}

			base.ProcessEvents(eventType);

			Debug.Assert(gCharMonster != null);

			Debug.Assert(gCharRoom != null);

			if (eventType == EventType.BeforePrintPlayerRoom && gEngine.ShouldPreTurnProcess)
			{
				// Witch's scream

				if (gGameState.SCR > 0)
				{
					gOut.Print("The witch's scream is driving you crazy!");

					if (--gGameState.SCR <= 0)
					{
						gOut.Print("Your ears (and brain) overload! You are dead!");

						gGameState.Die = 1;

						NextState = gEngine.CreateInstance<IPlayerDeadState>(x =>
						{
							x.PrintLineSep = true;
						});

						GotoCleanup = true;

						goto Cleanup;
					}
				}

				// Dream Dimension sensory overload

				if (gCharRoom.IsDreamDimensionRoom() && !gGameState.IC)         // TODO: might be (gGameState.Ro == 27 || gGameState.Ro > 49)
				{
					gOut.Print("The sights and sensations are driving you insane!");

					if (--gGameState.IS <= 0)
					{
						gOut.Print("Your brain overloads! You are dead!");

						gGameState.Die = 1;

						NextState = gEngine.CreateInstance<IPlayerDeadState>(x =>
						{
							x.PrintLineSep = true;
						});

						GotoCleanup = true;

						goto Cleanup;
					}
				}
			}
			else if (eventType == EventType.AfterPrintPlayerRoom && gEngine.ShouldPreTurnProcess)
			{
				var darrkNessMonster = gMDB[1];

				Debug.Assert(darrkNessMonster != null);

				var blindPriestMonster = gMDB[3];

				Debug.Assert(blindPriestMonster != null);

				var sirenWitchMonster = gMDB[5];

				Debug.Assert(sirenWitchMonster != null);

				var orbOfLifeArtifact = gADB[9];			// Forcefield

				Debug.Assert(orbOfLifeArtifact != null);

				var woodenBoxArtifact = gADB[10];

				Debug.Assert(woodenBoxArtifact != null);

				var gateOfLightArtifact = gADB[13];

				Debug.Assert(gateOfLightArtifact != null);

				var cloakOfLevitationArtifact = gADB[17];

				Debug.Assert(cloakOfLevitationArtifact != null);

				var orbOfLifeArtifact02 = gADB[23];			// No forcefield

				Debug.Assert(orbOfLifeArtifact02 != null);

				// Woods of Certain Death

				if (gCharRoom.Uid == 34 && !gGameState.IC)
				{
					gGameState.Die = 1;

					NextState = gEngine.CreateInstance<IPlayerDeadState>(x =>
					{
						x.PrintLineSep = true;
					});

					GotoCleanup = true;

					goto Cleanup;
				}

				// Bridge of Death

				if (gCharRoom.Uid == 51 && !cloakOfLevitationArtifact.IsWornByMonster(gCharMonster))
				{
					var totalWeight = 0L;

					var rc = gCharMonster.GetFullInventoryWeight(ref totalWeight, recurse: true);

					Debug.Assert(gEngine.IsSuccess(rc));

					var takeableList = gCharRoom.GetTakeableList();

					Debug.Assert(takeableList != null);

					foreach (var artifact in takeableList)
					{
						totalWeight += artifact.RecursiveWeight;
					}

					if (totalWeight > 10)
					{
						gEngine.PrintEffectDesc(5);

						gGameState.Die = 1;

						NextState = gEngine.CreateInstance<IPlayerDeadState>(x =>
						{
							x.PrintLineSep = true;
						});

						GotoCleanup = true;

						goto Cleanup;
					}
				}

				// Blind priest walks through Gate of Light

				if (gateOfLightArtifact.IsInRoom(gCharRoom) && blindPriestMonster.IsInRoom(gCharRoom) && ++gGameState.GOL > 3)
				{
					gEngine.PrintEffectDesc(13);

					blindPriestMonster.SetInRoomUid(36);
				}

				// Siren Witch knocks out Darrk Ness with scream

				if (darrkNessMonster.IsInRoom(gCharRoom) && darrkNessMonster.StateDesc.Length <= 0 && sirenWitchMonster.IsInRoom(gCharRoom) && woodenBoxArtifact.IsCarriedByMonster(sirenWitchMonster) && ++gGameState.TC > 2)
				{
					gEngine.PrintEffectDesc(33);

					darrkNessMonster.StateDesc = " (lying unconscious)";

					darrkNessMonster.Friendliness = Friendliness.Neutral;

					darrkNessMonster.Reaction = Friendliness.Neutral;
				}

				// Orb of Life forcefield disappears

				if (orbOfLifeArtifact.IsInRoom(gCharRoom) && gGameState.FL)
				{
					gOut.Print("The force field around the orb vanishes!");

					orbOfLifeArtifact.SetInLimbo();

					orbOfLifeArtifact02.SetInRoom(gCharRoom);

					gGameState.FL = false;
				}
			}

		Cleanup:

			;
		}
	}
}
