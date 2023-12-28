﻿
// PrintPlayerRoomState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using System.Linq;
using Eamon.Game.Attributes;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static TheWayfarersInn.Game.Plugin.Globals;

namespace TheWayfarersInn.Game.States
{
	[ClassMappings]
	public class PrintPlayerRoomState : EamonRT.Game.States.PrintPlayerRoomState, IPrintPlayerRoomState
	{
		public override void ProcessEvents(EventType eventType)
		{
			base.ProcessEvents(eventType);

			Debug.Assert(gCharRoom != null);

			var unseenApparitionMonster = gMDB[2];

			Debug.Assert(unseenApparitionMonster != null);

			var childsApparitionMonster = gMDB[4];

			Debug.Assert(childsApparitionMonster != null);

			if (eventType == EventType.BeforePrintPlayerRoom)
			{
				// Unseen apparition becomes aggravated

				if (gCharRoom.Uid == 38 && unseenApparitionMonster.IsInRoom(gCharRoom) && gEngine.ShouldPreTurnProcess)
				{
					gGameState.BedroomTurnCounter++;

					if (gGameState.BedroomTurnCounter == 10)
					{
						gOut.Print("You sense {0} is becoming aggravated.", unseenApparitionMonster.GetTheName());
					}
					else if (gGameState.BedroomTurnCounter > 15)
					{
						gEngine.UnseenApparitionAttacks = 2;

						gEngine.PrintEffectDesc(23);

						gEngine.UnseenApparitionAttacks = 0;

						gGameState.Die = 1;

						NextState = gEngine.CreateInstance<IPlayerDeadState>(x =>
						{
							x.PrintLineSep = true;
						});

						GotoCleanup = true;
					}
				}
			}
			else if (eventType == EventType.AfterPrintPlayerRoom)
			{
				//var treelineGapArtifact = gADB[16];

				//Debug.Assert(treelineGapArtifact != null);

				var majesticOakTreeArtifact = gADB[41];

				Debug.Assert(majesticOakTreeArtifact != null);

				var barArtifact = gADB[132];

				Debug.Assert(barArtifact != null);

				var hauntingArtifact = gADB[151];

				Debug.Assert(hauntingArtifact != null);

				// Bottle of bourbon / folded note noticed by player

				if (gGameState.BourbonAppeared && !gGameState.BourbonNoticed && barArtifact.IsInRoom(gCharRoom) && gCharRoom.IsLit() && gEngine.ShouldPreTurnProcess && gSentenceParser.IsInputExhausted)
				{
					gEngine.PrintEffectDesc(133);

					gGameState.BourbonNoticed = true;
				}

				// Child's apparition peeks from behind the majestic oak tree

				if (gCharRoom.Uid == 31 && majesticOakTreeArtifact.Seen && !unseenApparitionMonster.IsInLimbo() && !unseenApparitionMonster.IsInRoom(gCharRoom) && !gGameState.CharlottePeeks && gEngine.ShouldPreTurnProcess && gSentenceParser.IsInputExhausted)
				{
					gEngine.PrintEffectDesc(128);

					gGameState.CharlottePeeks = true;
				}

				var rl = gEngine.RollDice(1, 100, 0);

				var hauntingArtifactVanished = false;

				// Unseen apparition moves between rooms

				if (!unseenApparitionMonster.IsInLimbo() && rl < 10 && !gEngine.InnkeepersQuartersRoomUids.Contains(gCharRoom.Uid) && gEngine.ShouldPreTurnProcess)
				{
					if (unseenApparitionMonster.IsInRoom(gCharRoom) && !gEngine.PlayerMoved)
					{
						gOut.Print("You sense {0} is no longer present.", unseenApparitionMonster.GetTheName());
					}

					var room = gEngine.GetRandomWayfarersInnRoom(new long[] { unseenApparitionMonster.Location, 42 });

					unseenApparitionMonster.SetInRoom(room);

					if (hauntingArtifact.IsInRoom(gCharRoom))
					{
						if (gCharRoom.IsWayfarersInnClearingRoom())
						{
							gOut.Print("{0} suddenly {1}!", hauntingArtifact.GetTheName(true), hauntingArtifact.EvalPlural("vanishes", "vanish"));
						}
						else
						{
							// TODO: implement
						}

						hauntingArtifactVanished = true;
					}

					hauntingArtifact.SetInLimbo();
				}

				// Unseen apparition causes haunting artifact to appear

				if (hauntingArtifact.IsInLimbo() && !hauntingArtifactVanished)
				{
					if (gCharRoom.IsWayfarersInnClearingRoom())
					{
						var stateDesc = "";

						gEngine.GetOutdoorsHauntingData(gCharRoom.Uid, unseenApparitionMonster.GetInRoomUid(), ref stateDesc);

						rl = gEngine.RollDice(1, 100, 0);

						if (!string.IsNullOrWhiteSpace(stateDesc) && (rl < 50 || !gGameState.OutdoorsHauntingSeen))
						{
							var name = gEngine.GetRandomElement(new string[] { "ghostly face", "ghostly figure", "ghostly form", "ghostly light" });

							var synonyms = new string[] { name.Split(' ')[1] };

							hauntingArtifact.Field1 = gEngine.RollDice(1, 4, -1);

							gEngine.BuildDecorationArtifact(151, 56, name, synonyms, stateDesc);

							gGameState.OutdoorsHauntingSeen = true;
						}
					}
					else
					{
						// TODO: implement
					}
				}

				// Unseen apparition present

				if (unseenApparitionMonster.IsInRoom(gCharRoom) && gSentenceParser.IsInputExhausted)
				{
					if (!gGameState.UnseenApparitionMet)
					{
						gEngine.PrintFullDesc(unseenApparitionMonster, false, false);

						gGameState.UnseenApparitionMet = true;
					}
					else
					{
						gOut.Print("You sense {0} lurking nearby.", unseenApparitionMonster.GetArticleName());
					}
				}

				// Hint at treeline gap

				// TODO: implement if needed
			}
		}
	}
}
