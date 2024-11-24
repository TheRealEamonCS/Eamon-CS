
// SayCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Eamon.Framework;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static Dharmaquest.Game.Plugin.Globals;

namespace Dharmaquest.Game.Commands
{
	[ClassMappings]
	public class SayCommand : EamonRT.Game.Commands.SayCommand, ISayCommand
	{
		public override void ProcessEvents(EventType eventType)
		{
			base.ProcessEvents(eventType);

			if (eventType == EventType.BeforePrintSayText)
			{
				var blackWizardMonster = gMDB[15];

				Debug.Assert(blackWizardMonster != null);

				var bronzeRingArtifact = gADB[4];

				Debug.Assert(bronzeRingArtifact != null);

				var goldRingArtifact = gADB[5];

				Debug.Assert(goldRingArtifact != null);

				var silverRingArtifact = gADB[6];

				Debug.Assert(silverRingArtifact != null);

				var blackRingArtifact = gADB[7];

				Debug.Assert(blackRingArtifact != null);

				var goldBoxArtifact = gADB[26];

				Debug.Assert(goldBoxArtifact != null);

				var silverBoxArtifact = gADB[27];

				Debug.Assert(silverBoxArtifact != null);

				if (ProcessedPhrase.Equals("ilgaard", StringComparison.OrdinalIgnoreCase))
				{
					if (bronzeRingArtifact.IsCarriedByMonster(ActorMonster) || bronzeRingArtifact.IsWornByMonster(ActorMonster))
					{
						gEngine.PrintEffectDesc(8);

						if (ActorRoom.Uid == 17)
						{
							gEngine.PrintEffectDesc(15);

							gGameState.R2 = 23;

							NextState = gEngine.CreateInstance<IAfterPlayerMoveState>();
						}
						else if (ActorRoom.Uid == 70 && blackWizardMonster.IsInLimbo() && !gGameState.BlackWizardMet)
						{
							gGameState.BlackWizardMet = true;

							blackWizardMonster.SetInRoom(ActorRoom);

							NextState = gEngine.CreateInstance<IStartState>();
						}
						else if ((goldBoxArtifact.IsCarriedByMonster(ActorMonster) || goldBoxArtifact.IsInRoom(ActorRoom)) && blackRingArtifact.IsInLimbo())
						{
							goldBoxArtifact.InContainer.SetKeyUid(0);

							goldBoxArtifact.InContainer.SetOpen(true);

							blackRingArtifact.SetInRoom(ActorRoom);
						}
						else if ((silverBoxArtifact.IsCarriedByMonster(ActorMonster) || silverBoxArtifact.IsInRoom(ActorRoom)) && silverRingArtifact.IsInLimbo())
						{
							silverBoxArtifact.InContainer.SetKeyUid(0);

							silverBoxArtifact.InContainer.SetOpen(true);

							silverRingArtifact.SetInRoom(ActorRoom);
						}
						else
						{
							gOut.Print("The Ring of Ilgaard does not work here.");
						}
					}
					else
					{
						gOut.Print("You do not possess the Ring of Ilgaard.");

						NextState = gEngine.CreateInstance<IStartState>();
					}

					GotoCleanup = true;

					goto Cleanup;
				}

				if (ProcessedPhrase.Equals("xantec", StringComparison.OrdinalIgnoreCase))
				{
					if (goldRingArtifact.IsCarriedByMonster(ActorMonster) || goldRingArtifact.IsWornByMonster(ActorMonster))
					{
						gEngine.PrintEffectDesc(9);

						if (gGameState.Ro == 4)
						{
							gOut.Print("There is a secret cave to the east.");
						}
						else if (gGameState.Ro == 17)
						{
							gOut.Print("Use the Ring of Ilgaard.");
						}
						else if ((gGameState.Ro > 24 && gGameState.Ro < 29) || gGameState.Ro == 68)
						{
							gOut.Print("Try prayer.");
						}
						else if (gGameState.Ro == 36 || gGameState.Ro == 38 || gGameState.Ro == 69)
						{
							gOut.Print("There is a secret passage down.");
						}
						else if (gGameState.Ro == 43 || gGameState.Ro == 49 || gGameState.Ro == 50 || gGameState.Ro == 67)
						{
							gOut.Print("There is a secret passage behind the mosaic.");
						}
						else if (gGameState.Ro == 22)
						{
							gOut.Print("To use any of the Rings of Power you must possess the ring and say the name of the wizard who made the ring.");
						}
						else
						{
							gOut.Print("You must think for yourself sometimes.");
						}
					}
					else
					{
						gOut.Print("You do not possess the Ring of Xantec.");

						NextState = gEngine.CreateInstance<IStartState>();
					}

					GotoCleanup = true;

					goto Cleanup;
				}

				if (ProcessedPhrase.Equals("twalrep", StringComparison.OrdinalIgnoreCase))
				{
					if (silverRingArtifact.IsCarriedByMonster(ActorMonster) || silverRingArtifact.IsWornByMonster(ActorMonster))
					{
						gEngine.PrintEffectDesc(10);

						gOut.Print("You feel better.");

						gOut.Print("Some of your wounds are healed.");

						ActorMonster.DmgTaken -= 5;

						if (ActorMonster.DmgTaken < 0)
						{
							ActorMonster.DmgTaken = 1;      // TODO: is this a bug, should it be 0 ???

							gOut.Print("You feel totally rejuvenated!");
						}
					}
					else
					{
						gOut.Print("You do not have Twalrep's ring.");

						NextState = gEngine.CreateInstance<IStartState>();
					}

					GotoCleanup = true;

					goto Cleanup;
				}

				if (ProcessedPhrase.Equals("asroth", StringComparison.OrdinalIgnoreCase))
				{
					if (blackRingArtifact.IsCarriedByMonster(ActorMonster) || blackRingArtifact.IsWornByMonster(ActorMonster))
					{
						gEngine.PrintEffectDesc(11);

						// Monsters in room disappear

						gOut.Print("Who do you wish destroyed, my master?");

						gOut.Write("{0}Answer: ", Environment.NewLine);

						var buf = new StringBuilder(gEngine.BufSize);

						buf.SetFormat("{0}", gEngine.In.ReadLine());

						if (string.IsNullOrWhiteSpace(buf.ToString()))
						{
							buf.SetFormat("{0}", gEngine.UnknownName);
						}

						var monsterList = gEngine.GetMonsterList(m => !m.IsCharacterMonster() && m.IsInRoom(ActorRoom));

						var filteredList = gEngine.FilterRecordList(monsterList.Cast<IGameBase>().ToList(), buf.ToString());

						if (filteredList.Count > 1)
						{
							PrintDoYouMeanObj1OrObj2(filteredList[0], filteredList[1]);

							NextState = gEngine.CreateInstance<IStartState>();
						}
						else if (filteredList.Count < 1)
						{
							PrintNobodyHereByThatName();
						}
						else
						{
							var monster = filteredList[0] as IMonster;

							Debug.Assert(monster != null);

							monster.SetInLimbo();

							gOut.Print("{0} is destroyed!", monster.GetTheName(true));
						}
					}
					else
					{
						gOut.Print("You don't have the Ring of Asroth.");

						NextState = gEngine.CreateInstance<IStartState>();
					}

					GotoCleanup = true;

					goto Cleanup;
				}

				if (ProcessedPhrase.Equals(gGameState.BlackWizardName, StringComparison.OrdinalIgnoreCase))
				{
					if ((bronzeRingArtifact.IsCarriedByMonster(ActorMonster) || bronzeRingArtifact.IsWornByMonster(ActorMonster)) &&
						(goldRingArtifact.IsCarriedByMonster(ActorMonster) || goldRingArtifact.IsWornByMonster(ActorMonster)) &&
						(silverRingArtifact.IsCarriedByMonster(ActorMonster) || silverRingArtifact.IsWornByMonster(ActorMonster)) &&
						(blackRingArtifact.IsCarriedByMonster(ActorMonster) || blackRingArtifact.IsWornByMonster(ActorMonster)))
					{
						if (gGameState.Karma >= 100)
						{
							gEngine.PrintEffectDesc(15);

							gGameState.R2 = 68;

							NextState = gEngine.CreateInstance<IAfterPlayerMoveState>();
						}
						else
						{
							gOut.Print("You are not worthy! Your karma level is only {0}%.", gGameState.Karma);
						}
					}
					else
					{
						gOut.Print("You do not have all four Rings of Power.");

						NextState = gEngine.CreateInstance<IStartState>();
					}

					GotoCleanup = true;

					goto Cleanup;
				}

				if (ProcessedPhrase.Equals("friend", StringComparison.OrdinalIgnoreCase) && ActorRoom.Uid == 62)
				{
					gEngine.PrintEffectDesc(15);

					gGameState.R2 = 9;

					NextState = gEngine.CreateInstance<IAfterPlayerMoveState>();

					GotoCleanup = true;

					goto Cleanup;
				}

				if (((ProcessedPhrase.Equals("agricola", StringComparison.OrdinalIgnoreCase) && gGameState.SphinxKilled) || (ProcessedPhrase.Equals("nauta", StringComparison.OrdinalIgnoreCase) && gGameState.RiddleSolved)) && ActorRoom.Uid == 65)
				{
					gEngine.PrintEffectDesc(15);

					gGameState.R2 = 63;

					NextState = gEngine.CreateInstance<IAfterPlayerMoveState>();

					GotoCleanup = true;

					goto Cleanup;
				}
			}

		Cleanup:

			;
		}
	}
}
