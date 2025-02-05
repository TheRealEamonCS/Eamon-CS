
// SayCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using System.Linq;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using Eamon.Game.Utilities;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static OrbOfMyLife.Game.Plugin.Globals;

namespace OrbOfMyLife.Game.Commands
{
	[ClassMappings]
	public class SayCommand : EamonRT.Game.Commands.SayCommand, ISayCommand
	{
		public virtual string RetrievedFormatString { get; set; }

		public override void ProcessEvents(EventType eventType)
		{
			base.ProcessEvents(eventType);

			if (eventType == EventType.BeforePrintSayText)
			{
				var heroMonster = gMDB[2];

				Debug.Assert(heroMonster != null);

				var princeOfDarknessMonster = gMDB[9];

				Debug.Assert(princeOfDarknessMonster != null);

				var orbOfLifeArtifact = gADB[9];		// forcefield

				Debug.Assert(orbOfLifeArtifact != null);

				var woodenBoxArtifact = gADB[10];

				Debug.Assert(woodenBoxArtifact != null);

				var cloakOfDarknessArtifact = gADB[11];

				Debug.Assert(cloakOfDarknessArtifact != null);

				var staffOfLightArtifact = gADB[12];

				Debug.Assert(staffOfLightArtifact != null);

				var jewelOfReomeArtifact = gADB[14];

				Debug.Assert(jewelOfReomeArtifact != null);

				var jadedTalismanArtifact = gADB[15];

				Debug.Assert(jadedTalismanArtifact != null);

				var orbOfLifeArtifact02 = gADB[23];		// no forcefield

				Debug.Assert(orbOfLifeArtifact02 != null);

				// Enter Dream Dimension

				if (ProcessedPhrase.Equals(gGameState.MW, StringComparison.OrdinalIgnoreCase) && staffOfLightArtifact.IsCarriedByMonster(ActorMonster) && jadedTalismanArtifact.IsWornByMonster(ActorMonster) && gGameState.IC)
				{
					if (ActorRoom.Uid == 50)
					{
						gEngine.PrintEffectDesc(11);

						gGameState.R2 = 49;

						NextState = gEngine.CreateInstance<IAfterPlayerMoveState>();

						GotoCleanup = true;
					}
					else if (ActorRoom.Uid == 49)
					{
						gEngine.PrintEffectDesc(11);

						gGameState.R2 = 50;

						gGameState.IS = 10;

						NextState = gEngine.CreateInstance<IAfterPlayerMoveState>();

						GotoCleanup = true;
					}
				}

				// Visit strange vortex

				if (ProcessedPhrase.Equals(gGameState.TW, StringComparison.OrdinalIgnoreCase) && orbOfLifeArtifact02.IsCarriedByMonster(ActorMonster))
				{
					gEngine.PrintEffectDesc(1);

					gGameState.R2 = 45;

					NextState = gEngine.CreateInstance<IAfterPlayerMoveState>();

					GotoCleanup = true;
				}

				// Strange vortex activities

				if (ProcessedPhrase.Equals(gGameState.CW, StringComparison.OrdinalIgnoreCase) && ActorRoom.Uid == 45)
				{
					if (cloakOfDarknessArtifact.IsInRoom(ActorRoom))
					{
						gEngine.PrintEffectDesc(22);

						gGameState.RC = true;

						cloakOfDarknessArtifact.SetInLimbo();

						GotoCleanup = true;
					}
					else if (gGameState.IV)
					{
						gOut.Print("You flash visible and invisible as the cloak spins around your neck!");

						gEngine.MonstersGetUnnerved();

						GotoCleanup = true;
					}
				}

				// Jewel of Reome

				if (ProcessedPhrase.Equals("reome", StringComparison.OrdinalIgnoreCase) && jewelOfReomeArtifact.IsCarriedByMonster(ActorMonster) && ActorRoom.GetDir(Direction.North) != gEngine.DirectionExit)
				{
					gOut.Print("Magic exits open in all directions!");

					var directionValues = EnumUtil.GetValues<Direction>().Where(dv => dv != Direction.In && dv != Direction.Out).ToList();

					foreach (var dv in directionValues)
					{
						ActorRoom.SetDir(dv, gEngine.DirectionExit);
					}

					GotoCleanup = true;
				}

				// Hero fetches stuff

				if (ProcessedPhrase.Equals("rovatha", StringComparison.OrdinalIgnoreCase) && heroMonster.IsInRoom(ActorRoom))
				{
					if (gGameState.SR > 0)
					{
						var retrievedArtifactList = gEngine.GetArtifactList(a => a.IsInRoomUid(gGameState.SR) && !a.IsUnmovable() && heroMonster.CanCarryArtifactWeight(a));

						foreach (var artifact in retrievedArtifactList)
						{
							artifact.SetInRoom(ActorRoom);
						}

						gGameState.SR = 0;

						if (retrievedArtifactList.Count > 0)
						{
							gOut.Print(RetrievedFormatString, heroMonster.GetTheName(true), "your stuff", "it", "at your feet");
						}
						else
						{
							PrintNothingHappens();
						}
					}
					else
					{
						IArtifact retrievedArtifact = null;

						if (woodenBoxArtifact.IsInRoom(ActorRoom))
						{
							retrievedArtifact = woodenBoxArtifact;
						}
						else if (orbOfLifeArtifact.IsInRoom(ActorRoom))         //NextState = gEngine.CreateInstance<IStartState>();
						{
							gEngine.PrintEffectDesc(20);

							heroMonster.SetInLimbo();
						}
						else if (orbOfLifeArtifact02.IsInRoom(ActorRoom))
						{
							retrievedArtifact = orbOfLifeArtifact02;
						}
						else if (princeOfDarknessMonster.IsInRoom(ActorRoom) && cloakOfDarknessArtifact.IsCarriedByMonster(princeOfDarknessMonster))
						{
							retrievedArtifact = cloakOfDarknessArtifact;
						}
						else													//NextState = gEngine.CreateInstance<IStartState>();
						{
							PrintNothingHappens();
						}

						if (retrievedArtifact != null)
						{
							var retrievedToWhereString = "";

							if (ActorMonster.CanCarryArtifactWeight(retrievedArtifact))
							{
								retrievedToWhereString = "into your hands";

								retrievedArtifact.SetCarriedByMonster(ActorMonster);
							}
							else
							{
								retrievedToWhereString = "at your feet";

								retrievedArtifact.SetInRoom(ActorRoom);
							}

							gOut.Print(RetrievedFormatString, heroMonster.GetTheName(true), retrievedArtifact.GetTheName(), retrievedArtifact.EvalPlural("it", "them"), retrievedToWhereString);
						}
					}

					GotoCleanup = true;
				}
			}
			else if (eventType == EventType.AfterPrintSayText)
			{
				// Disembodied voices

				if (gGameState.IV)
				{
					gEngine.MonstersGetUnnerved();
				}
			}
		}

		public SayCommand()
		{
			RetrievedFormatString = "{0} circles the room, picks up {1} and drops {2} {3}!";
		}
	}
}
