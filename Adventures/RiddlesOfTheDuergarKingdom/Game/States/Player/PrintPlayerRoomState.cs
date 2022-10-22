
// PrintPlayerRoomState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static RiddlesOfTheDuergarKingdom.Game.Plugin.Globals;

namespace RiddlesOfTheDuergarKingdom.Game.States
{
	[ClassMappings]
	public class PrintPlayerRoomState : EamonRT.Game.States.PrintPlayerRoomState, IPrintPlayerRoomState
	{
		public override void ProcessEvents(EventType eventType)
		{
			base.ProcessEvents(eventType);

			if (eventType == EventType.BeforePrintPlayerRoom && gEngine.ShouldPreTurnProcess)
			{
				Debug.Assert(gCharMonster != null);

				var room = gCharMonster.GetInRoom();

				Debug.Assert(room != null);

				var rl = gEngine.RollDice(1, 100, 0);

				var viperMonster = gMDB[1];

				Debug.Assert(viperMonster != null);

				// Viper doesn't like company

				if (viperMonster.IsInRoom(room) && viperMonster.Seen && viperMonster.Reaction > Friendliness.Enemy && rl <= 25)
				{
					viperMonster.Reaction--;

					gOut.Print("{0} is aggravated by your presence!", viperMonster.GetTheName(true));
				}

				var berescroftMonster = gMDB[2];

				Debug.Assert(berescroftMonster != null);

				var fountainPenArtifact = gADB[41];

				Debug.Assert(fountainPenArtifact != null);

				// Meet Professor Berescroft

				if (berescroftMonster.IsInRoom(room) && room.IsLit() && !gGameState.BerescroftMet)
				{
					gOut.Print("You have encountered Professor Berescroft, mentioned by the letter posted in the Main Hall, who is leading the archaeological survey.  A long-winded (but hopefully interesting) monologue follows.  A grad student will accompany you on your journey.  He hands you a fountain pen to sign the presented document, necessary to indemnify Eamon University.");			// TODO: refactor

					fountainPenArtifact.SetCarriedByCharacter();

					gGameState.BerescroftMet = true;
				}

				var fieldMedicMonster = gMDB[6];

				Debug.Assert(fieldMedicMonster != null);

				// Field medic renders assistance

				if (fieldMedicMonster.IsInRoom(room) && (gCharMonster.DmgTaken > 0 || gGameState.PoisonedTargets.ContainsKey(gGameState.Cm)) && gGameState.R3 == 18)
				{
					gGameState.R3 = 130;

					gEngine.PrintEffectDesc(84);

					var printLimitedSupplies = false;

					var suppliesUsed = false;

					if (gGameState.WaiverSigned)
					{
						if (gCharMonster.DmgTaken > 0 && gGameState.MedicHealCounter > 0)
						{
							gEngine.PrintEffectDesc(85);

							gCharMonster.DmgTaken = 0;

							gGameState.MedicHealCounter--;

							suppliesUsed = true;
						}

						if (gGameState.PoisonedTargets.ContainsKey(gGameState.Cm) && gGameState.MedicAntiVenomCounter > 0)
						{
							gEngine.PrintEffectDesc(86);

							gGameState.PoisonedTargets.Remove(gGameState.Cm);

							gGameState.AfterPrintPlayerRoomEventHeap.Remove((k, v) => v.EventName == "PoisonSickensMonsterEvent" && long.Parse(v.EventParam.ToString()) == gGameState.Cm);

							gGameState.MedicAntiVenomCounter--;

							suppliesUsed = true;
						}

						if (!suppliesUsed)
						{
							printLimitedSupplies = true;
						}
					}
					else
					{
						printLimitedSupplies = true;
					}

					if (printLimitedSupplies)
					{
						gEngine.PrintEffectDesc(87);
					}
				}

				var gradStudentCompanionMonster = gMDB[gGameState.GradStudentCompanionUid];

				Debug.Assert(gradStudentCompanionMonster != null);

				// Grad student companion loses suffix

				if (gradStudentCompanionMonster.IsInRoom(room) && gradStudentCompanionMonster.Reaction == Friendliness.Friend && gGameState.GradStudentSuffixCounter > 0 && --gGameState.GradStudentSuffixCounter == 0)
				{
					gradStudentCompanionMonster.Name = gradStudentCompanionMonster.Name.Replace(" the grad student", "");
				}

				// Grad student companion abandons player

				if (gGameState.VolcanoErupting && room.Uid == 31 && gradStudentCompanionMonster.IsInRoom(room))
				{
					if (room.IsLit())
					{
						gEngine.PrintEffectDesc(51);
					}

					gradStudentCompanionMonster.SetInLimbo();
				}
			}
		}

		public override void Execute()
		{
			base.Execute();

			var room = gCharMonster.GetInRoom() as Framework.IRoom;

			Debug.Assert(room != null);

			var gradStudentCompanionMonster = gMDB[gGameState.GradStudentCompanionUid];

			Debug.Assert(gradStudentCompanionMonster != null);

			// Grad student companion also sees room

			if (room.IsLit() && gradStudentCompanionMonster.IsInRoom(room) && gradStudentCompanionMonster.Reaction == Friendliness.Friend)
			{
				room.GradStudentCompanionSeen = true;
			}
		}
	}
}
