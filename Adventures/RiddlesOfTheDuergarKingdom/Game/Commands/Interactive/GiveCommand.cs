
// GiveCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using System.Linq;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Primitive.Enums;
using static RiddlesOfTheDuergarKingdom.Game.Plugin.Globals;

namespace RiddlesOfTheDuergarKingdom.Game.Commands
{
	[ClassMappings]
	public class GiveCommand : EamonRT.Game.Commands.GiveCommand, IGiveCommand
	{
		public override void ProcessEvents(EventType eventType)
		{
			base.ProcessEvents(eventType);

			if (eventType == EventType.AfterEnforceMonsterWeightLimitsCheck)
			{
				var artifactUids = new long[] { 36, 37, 38, 73, 74 };

				// Give Professor Berescroft the fountain pen

				if (IobjMonster.Uid == 2 && DobjArtifact.Uid == 41)
				{
					if (gGameState.WaiverSigned)
					{
						DobjArtifact.SetInLimbo();

						gOut.Print("You give the fountain pen to Professor Berescroft.");
					}
					else
					{
						gOut.Print("He politely requests you indemnify Eamon University by signing the document.");
					}

					GotoCleanup = true;
				}

				// Give highland ibex the vegetables

				else if (IobjMonster.Uid == 21 && artifactUids.Contains(DobjArtifact.Uid))
				{
					var idx = Array.FindIndex(gGameState.IbexVegetableUids, x => x == DobjArtifact.Uid);

					if (idx < 0)
					{
						idx = Array.FindIndex(gGameState.IbexVegetableUids, x => x == 0);

						if (idx >= 0)
						{
							gGameState.IbexVegetableUids[idx] = DobjArtifact.Uid;
						}
					}
				}

				// Give highland ibex the leather panniers

				else if (IobjMonster.Uid == 21 && IobjMonster.Reaction == Friendliness.Friend && DobjArtifact.Uid == 93)
				{
					// do nothing
				}

				// Further disable bribing

				else if (IobjMonster.Uid == 21 || IobjMonster.Reaction < Friendliness.Friend)
				{
					gEngine.PrintMonsterEmotes(IobjMonster);

					gOut.WriteLine();

					GotoCleanup = true;
				}
			}
			else if (eventType == EventType.BeforeTakePlayerGold)
			{
				// Disable bribing

				if (IobjMonster.Uid == 21 || IobjMonster.Reaction < Friendliness.Friend)
				{
					gEngine.PrintMonsterEmotes(IobjMonster);

					gOut.WriteLine();

					GotoCleanup = true;
				}
			}
		}

		public override void ExecuteForPlayer()
		{
			base.ExecuteForPlayer();

			// Highland ibex becomes friendly

			if (IobjMonster != null && IobjMonster.Uid == 21 && Array.FindIndex(gGameState.IbexVegetableUids, x => x == 0) < 0)
			{
				if (IobjMonster.Reaction < Friendliness.Friend)
				{ 
					gEngine.PrintEffectDesc(22);

					IobjMonster.Reaction = Friendliness.Friend;
				}

				Array.Clear(gGameState.IbexVegetableUids, 0, gGameState.IbexVegetableUids.Length);

				gGameState.BeforePrintPlayerRoomEventHeap.Remove((k, v) => v.EventName == "HighlandIbexAbandonsPlayerEvent");

				gGameState.BeforePrintPlayerRoomEventHeap.Insert(gGameState.CurrTurn + gEngine.IbexAbandonTurns, "HighlandIbexAbandonsPlayerEvent");
			}

			var leatherPanniersArtifact = gADB[93];

			Debug.Assert(leatherPanniersArtifact != null);

			// Highland ibex wears leather panniers

			if (leatherPanniersArtifact.IsCarriedByMonsterUid(21))
			{
				leatherPanniersArtifact.SetWornByMonsterUid(21);
			}
		}

		public override void PrintOpensConsumesAndHandsBack(IArtifact artifact, IMonster monster, bool objOpened, bool objEdible)
		{
			Debug.Assert(artifact != null && monster != null);

			// Highland ibex

			if (monster.Uid == 21)
			{
				gOut.Print("{0} takes a {1}.", monster.GetTheName(true), objEdible ? "bite" : "drink");
			}
			else
			{
				base.PrintOpensConsumesAndHandsBack(artifact, monster, objOpened, objEdible);
			}
		}
	}
}
