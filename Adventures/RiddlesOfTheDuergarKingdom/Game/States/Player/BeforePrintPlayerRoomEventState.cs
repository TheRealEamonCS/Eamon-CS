
// BeforePrintPlayerRoomEventState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static RiddlesOfTheDuergarKingdom.Game.Plugin.PluginContext;

namespace RiddlesOfTheDuergarKingdom.Game.States
{
	[ClassMappings]
	public class BeforePrintPlayerRoomEventState : EamonRT.Game.States.BeforePrintPlayerRoomEventState, IBeforePrintPlayerRoomEventState
	{
		public virtual void GuardOpensIronGateEvent(object obj)
		{
			Debug.Assert(gCharMonster != null);

			var room = gCharMonster.GetInRoom();

			// Guard opens iron gate, if necessary

			if (room != null && (gGameState.Ro == 9 || gGameState.Ro == 10))
			{
				gEngine.PrintEffectDesc(53);

				var gradStudentCompanionMonster = gMDB[gGameState.GradStudentCompanionUid];

				Debug.Assert(gradStudentCompanionMonster != null);

				// Grad student companion won't go through the iron gate

				if (room.IsLit() && gradStudentCompanionMonster.IsInRoom(room) && gradStudentCompanionMonster.Reaction == Friendliness.Friend)
				{
					gEngine.PrintEffectDesc(54);
				}

				var highlandIbexMonster = gMDB[21];

				Debug.Assert(highlandIbexMonster != null);

				// Highland ibex won't go through the iron gate

				if (room.IsLit() && highlandIbexMonster.IsInRoom(room) && highlandIbexMonster.Reaction == Friendliness.Friend)
				{
					gEngine.PrintEffectDesc(33);
				}

				gGameState.R2 = gGameState.Ro == 9 ? 10 : 9;

				NextState = Globals.CreateInstance<IAfterPlayerMoveState>(x =>
				{
					x.MoveMonsters = false;
				});
			}
		}

		public virtual void HighlandIbexAbandonsPlayerEvent(object obj)
		{
			Debug.Assert(gCharMonster != null);

			var room = gCharMonster.GetInRoom();

			var highlandIbexMonster = gMDB[21];

			Debug.Assert(highlandIbexMonster != null);

			var sluiceGateArtifact = gADB[25];

			Debug.Assert(sluiceGateArtifact != null);

			var leatherPanniersArtifact = gADB[93];

			Debug.Assert(leatherPanniersArtifact != null);

			// Highland ibex abandons player

			if (room != null && room.IsLit() && highlandIbexMonster.IsInRoom(room) && sluiceGateArtifact.DoorGate.IsOpen() && !gGameState.SewagePitVisited)
			{
				gEngine.PrintEffectDesc(91);

				highlandIbexMonster.Reaction = Friendliness.Neutral;

				if (leatherPanniersArtifact.IsWornByMonster(highlandIbexMonster))
				{
					gEngine.PrintEffectDesc(92);

					leatherPanniersArtifact.SetInRoom(room);
				}
			}
			else
			{
				gGameState.BeforePrintPlayerRoomEventHeap.Insert(gGameState.CurrTurn + Constants.IbexAbandonTurns, "HighlandIbexAbandonsPlayerEvent");
			}
		}
	}
}
