
// PrintPlayerRoomState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static Dharmaquest.Game.Plugin.Globals;

namespace Dharmaquest.Game.States
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

				Debug.Assert(gCharRoom != null);

				var patroclosMonster = gMDB[6];

				Debug.Assert(patroclosMonster != null);

				var achillesMonster = gMDB[7];

				Debug.Assert(achillesMonster != null);

				var neoptolemusMonster = gMDB[8];

				Debug.Assert(neoptolemusMonster != null);

				var sacredBullMonster = gMDB[16];

				Debug.Assert(sacredBullMonster != null);

				var pythonMonster = gMDB[20];

				Debug.Assert(pythonMonster != null);

				var patroclosStatueArtifact = gADB[12];

				Debug.Assert(patroclosStatueArtifact != null);

				var goldenCupArtifact = gADB[18];

				Debug.Assert(goldenCupArtifact != null);

				var crownArtifact = gADB[19];

				Debug.Assert(crownArtifact != null);

				var patroclosStatueArtifact02 = gADB[50];

				Debug.Assert(patroclosStatueArtifact02 != null);

				var achillesStatueArtifact = gADB[51];

				Debug.Assert(achillesStatueArtifact != null);

				var bonesArtifact = gADB[59];

				Debug.Assert(bonesArtifact != null);

				var deadBullArtifact = gADB[60];

				Debug.Assert(deadBullArtifact != null);

				var deadPythonArtifact = gADB[64];

				Debug.Assert(deadPythonArtifact != null);

				// Happenings

				// Patroclos killed

				if (patroclosStatueArtifact02.IsInRoom(gCharRoom) && !gGameState.AchillesMet)
				{
					gGameState.AchillesMet = true;

					achillesMonster.SetInRoom(gCharRoom);
				}

				// Achilles killed

				if (achillesStatueArtifact.IsInRoom(gCharRoom) && !gGameState.NeoptolemusMet)
				{
					gGameState.NeoptolemusMet = true;

					neoptolemusMonster.SetInRoom(gCharRoom);
				}

				// Python killed

				if (deadPythonArtifact.IsInRoom(gCharRoom) && !gGameState.ApolloCurses)
				{
					gGameState.ApolloCurses = true;

					gEngine.ApolloCursesPlayer();
				}

				// Sacred bull killed

				if (deadBullArtifact.IsInRoom(gCharRoom) && !gGameState.PoseidonCurses)
				{
					gGameState.PoseidonCurses = true;

					gEngine.PoseidonCursesPlayer(this);
				}

				// Take Patroclos' small statue - Patroclos appears

				if (patroclosStatueArtifact.IsCarriedByMonster(gCharMonster))
				{
					patroclosStatueArtifact.SetInLimbo();

					patroclosMonster.SetInRoom(gCharRoom);
				}

				// Take Poseidon's golden cup - sacred bull appears

				if (goldenCupArtifact.IsCarriedByMonster(gCharMonster) && !gGameState.BullMet)
				{
					gGameState.BullMet = true;

					sacredBullMonster.SetInRoom(gCharRoom);
				}

				// Take / wear Apollo's crown - python appears

				if ((crownArtifact.IsCarriedByMonster(gCharMonster) || crownArtifact.IsWornByMonster(gCharMonster)) && !gGameState.PythonMet)
				{
					gGameState.PythonMet = true;

					pythonMonster.SetInRoom(gCharRoom);
				}

				// Kill Black Wizard - learn his name

				if (bonesArtifact.IsInRoom(gCharRoom) && !gGameState.BlackWizardNameRevealed)
				{
					gGameState.BlackWizardNameRevealed = true;

					gOut.Print("The Black Wizard's name was {0}.", gGameState.BlackWizardName);
				}
			}
		}
	}
}
