
// BeforePrintPlayerRoomEventState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using TheWayfarersInn.Framework.Primitive.Enums;
using static TheWayfarersInn.Game.Plugin.Globals;

namespace TheWayfarersInn.Game.States
{
	[ClassMappings]
	public class BeforePrintPlayerRoomEventState : EamonRT.Game.States.BeforePrintPlayerRoomEventState, IBeforePrintPlayerRoomEventState
	{
		public virtual void DropArtisanBodyPartArtifactEvent(object obj)
		{
			Debug.Assert(obj != null);

			long artifactUid;

			if (long.TryParse(obj.ToString(), out artifactUid) == false)
			{
				artifactUid = 0;
			}

			var artifact = gADB[artifactUid];

			if (artifact != null && artifact.IsCarriedByMonster(gCharMonster))
			{
				gOut.Print("You decide to discard {0}, finding {1} repulsive.", artifact.GetTheName(), artifact.EvalPlural("it", "them"));

				var dropCommand = gEngine.CreateInstance<IDropCommand>(x =>
				{
					x.ActorMonster = gCharMonster;

					x.ActorRoom = gCharRoom;

					x.Dobj = artifact;
				});

				dropCommand.Execute();
			}
		}

		public virtual void BlueBandedCentipedesEvent(object obj)
		{
			long rl;

			var blueBandedCentipedesMonster = gMDB[3];

			Debug.Assert(blueBandedCentipedesMonster != null);

			var gapingHoleArtifact = gADB[42];

			Debug.Assert(gapingHoleArtifact != null);

			var eventState = gGameState.GetEventState(EventState.BlueBandedCentipedes);

			if (blueBandedCentipedesMonster.IsInLimbo() && eventState > 9)
			{
				eventState = 0;
			}

			switch (eventState)
			{
				case 9:

					rl = Math.Min(gEngine.RollDice(1, 3, 2), gGameState.TotalCentipedeCounter);

					gGameState.TotalCentipedeCounter -= rl;

					gGameState.AttackingCentipedeCounter = 0;

					blueBandedCentipedesMonster.SetInRoomUid(27);

					blueBandedCentipedesMonster.GroupCount = rl;

					blueBandedCentipedesMonster.InitGroupCount = rl;

					blueBandedCentipedesMonster.CurrGroupCount = rl;

					if (gCharRoom.Uid == 27)
					{
						gOut.Print("{0} suddenly crawl{1} out of {2}!", blueBandedCentipedesMonster.GetArticleName(true), rl > 1 ? "" : "s", gapingHoleArtifact.GetArticleName());
					}

					eventState++;

					break;

				case 10:

					rl = gEngine.RollDice(1, 100, 0);

					if (rl < 75)
					{
						do
						{
							rl = gEngine.RollDice(1, 100, 0);
						}
						while (rl > 29 && gCharRoom.Uid != 27);

						if (rl < 15)        // Add group member(s)
						{
							eventState = 13;
						}
						else if (rl < 30)        // Remove group member(s)
						{
							eventState = 14;
						}
						else if (rl < 75)       // Add attacker(s)
						{
							eventState = 11;
						}
						else                    // Remove attacker(s)
						{
							eventState = 12;
						}
					}

					break;

				case 13:

					if (gGameState.TotalCentipedeCounter > 0)
					{
						rl = Math.Min(gEngine.RollDice(1, 3, 2), gGameState.TotalCentipedeCounter);

						gGameState.TotalCentipedeCounter -= rl;

						blueBandedCentipedesMonster.GroupCount += rl;

						blueBandedCentipedesMonster.InitGroupCount += rl;

						blueBandedCentipedesMonster.CurrGroupCount += rl;

						if (gCharRoom.Uid == 27)
						{
							var origCurrGroupCount = blueBandedCentipedesMonster.CurrGroupCount;

							blueBandedCentipedesMonster.CurrGroupCount = rl;

							gOut.Print("{0} crawl{1} out of {2}!", blueBandedCentipedesMonster.GetArticleName(true), rl > 1 ? "" : "s", gapingHoleArtifact.GetArticleName());

							blueBandedCentipedesMonster.CurrGroupCount = origCurrGroupCount;
						}
					}

					eventState = 10;

					break;

				case 14:

					if (blueBandedCentipedesMonster.CurrGroupCount > 0 && gGameState.AttackingCentipedeCounter < blueBandedCentipedesMonster.CurrGroupCount)
					{
						rl = Math.Min(gEngine.RollDice(1, 3, 2), blueBandedCentipedesMonster.CurrGroupCount - gGameState.AttackingCentipedeCounter);

						gGameState.TotalCentipedeCounter += rl;

						blueBandedCentipedesMonster.GroupCount -= rl;

						blueBandedCentipedesMonster.InitGroupCount -= rl;

						blueBandedCentipedesMonster.CurrGroupCount -= rl;

						if (gGameState.AttackingCentipedeCounter > blueBandedCentipedesMonster.CurrGroupCount)
						{
							gGameState.AttackingCentipedeCounter = blueBandedCentipedesMonster.CurrGroupCount;
						}

						if (blueBandedCentipedesMonster.CurrGroupCount <= 0)
						{
							blueBandedCentipedesMonster.SetInLimbo();
						}

						if (gCharRoom.Uid == 27)
						{
							var origCurrGroupCount = blueBandedCentipedesMonster.CurrGroupCount;

							blueBandedCentipedesMonster.CurrGroupCount = rl;

							gOut.Print("{0} crawl{1} into {2}!", blueBandedCentipedesMonster.GetArticleName(true), rl > 1 ? "" : "s", gapingHoleArtifact.GetArticleName());

							blueBandedCentipedesMonster.CurrGroupCount = origCurrGroupCount;
						}
					}

					eventState = blueBandedCentipedesMonster.IsInLimbo() ? 0 : 10;

					break;
			}

			gGameState.SetEventState(EventState.BlueBandedCentipedes, eventState);

			if (eventState == 9 || eventState == 10 || eventState == 13 || eventState == 14)
			{
				gGameState.BeforePrintPlayerRoomEventHeap.Insert02(gGameState.CurrTurn + 1, "BlueBandedCentipedes", 0, null);
			}
			else if (eventState > 0)
			{
				gGameState.AfterPrintPlayerRoomEventHeap.Insert02(gGameState.CurrTurn + 1, "BlueBandedCentipedes", 0, null);
			}
		}
	}
}
