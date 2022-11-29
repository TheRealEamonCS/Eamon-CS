
// UseCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using System.Linq;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static RiddlesOfTheDuergarKingdom.Game.Plugin.Globals;

namespace RiddlesOfTheDuergarKingdom.Game.Commands
{
	[ClassMappings]
	public class UseCommand : EamonRT.Game.Commands.UseCommand, IUseCommand
	{
		public override void ProcessEvents(EventType eventType)
		{
			base.ProcessEvents(eventType);

			if (eventType == EventType.BeforeUseArtifact)
			{
				switch (DobjArtifact.Uid)
				{
					case 6:
					{
						// Brass Bell

						gEngine.PrintEffectDesc(39);

						if (!gGameState.VolcanoErupting)
						{
							var rl = gEngine.RollDice(1, 4, 3);

							// Guard opens iron gate in 4-7 turns (including bell ring)

							gGameState.BeforePrintPlayerRoomEventHeap.Insert(gGameState.CurrTurn + rl, "GuardOpensIronGate", (k, v) => v.EventName == "GuardOpensIronGate");
						}

						GotoCleanup = true;

						break;
					}

					case 15:

						// Iron lever

						gGameState.IronLeverDisabled = !gGameState.IronLeverDisabled;

						gOut.Print("You grab the handle of the iron lever with both hands and {0} you.  A metallic clanking sound echoes through the mine shaft as the safety mechanism {1}.{2}",
							gGameState.IronLeverDisabled ? "push it away from" : "pull it towards",
							gGameState.IronLeverDisabled ? "disengages" : "engages",
							gGameState.IronLeverDisabled && gGameState.WinchCounter > 0 ? "  The wooden cart shudders momentarily then sways gently as your weight shifts." : "");

						GotoCleanup = true;

						break;

					case 18:

						// Hand winch

						if (gGameState.IronLeverDisabled)
						{
							gOut.Print("Using both hands, you turn the handle of the winch {0}.  The wooden cart slowly {1} into the mine shaft.{2}", 
								gGameState.WoodenCartAscending ? "clockwise" : "counter-clockwise",
								gGameState.WoodenCartAscending ? "ascends" : "descends",
								gGameState.WoodenCartAscending && gGameState.WinchCounter == 1 ? "  Finally, you arrive at the top of the mine shaft, which opens up into a tunnel." : 
								!gGameState.WoodenCartAscending && gGameState.WinchCounter == 1 ? "  Finally, you arrive at the bottom of the mine shaft, which opens up into a dark chamber." :
								"  You are at the midway point, suspended high above the ground.");

							if (gGameState.WoodenCartAscending)
							{
								gGameState.WinchCounter++;

								if (gGameState.WinchCounter == 2)
								{
									gGameState.WoodenCartAscending = false;
								}
							}
							else
							{
								gGameState.WinchCounter--;

								if (gGameState.WinchCounter == 0)
								{
									gGameState.WoodenCartAscending = true;
								}
							}

							gGameState.R2 = ActorRoom.Uid;

							NextState = gEngine.CreateInstance<IAfterPlayerMoveState>();
						}
						else
						{
							gOut.Print("You grasp the handle of the winch with both hands and attempt to turn it.  Unfortunately, the winch seems to be stuck, refusing to move.");
						}

						GotoCleanup = true;

						break;

					case 41:

						// Fountain pen

						if (!gGameState.WaiverSigned)
						{
							gEngine.PrintEffectDesc(55);

							var gradStudentCompanionMonster = gMDB[gGameState.GradStudentCompanionUid];

							Debug.Assert(gradStudentCompanionMonster != null);

							gradStudentCompanionMonster.SetInRoom(ActorRoom);

							gradStudentCompanionMonster.Friendliness = Friendliness.Friend;

							gradStudentCompanionMonster.Reaction = Friendliness.Friend;

							gGameState.WaiverSigned = true;
						}
						else
						{
							gOut.Print("You've already signed the necessary paperwork!");

							NextState = gEngine.CreateInstance<IStartState>();
						}

						GotoCleanup = true;

						break;

					case 42:
					{
						// Latrine

						gOut.Print("Really?!  You're just going to have to hold it!");

						GotoCleanup = true;

						break;
					}

					case 26:
					{
						// Hand crank #1

						var sluiceGateArtifact = gADB[25];

						Debug.Assert(sluiceGateArtifact != null);

						gOut.Print("You turn the hand crank {0}.  As you do so, outside, to the southeast, you hear a loud clanking sound{1}.", 
							sluiceGateArtifact.DoorGate.IsOpen() ? "clockwise" : "counter-clockwise",
							sluiceGateArtifact.DoorGate.IsOpen() ? ", and the violent churn of water abates" : " and the violent churn of water");

						sluiceGateArtifact.DoorGate.SetOpen(!sluiceGateArtifact.DoorGate.IsOpen());

						if (!sluiceGateArtifact.DoorGate.IsOpen())
						{
							gEngine.SteamTurbineStopsRunning();
						}

						GotoCleanup = true;

						break;
					}

					case 60:
					{
						// Hand crank #2

						var sluiceGateArtifact = gADB[25];

						Debug.Assert(sluiceGateArtifact != null);

						var waterDiversionGateArtifact = gADB[61];

						Debug.Assert(waterDiversionGateArtifact != null);

						gOut.Print("You turn the hand crank {0}.  As you do so, to the northwest, you hear a loud clanking sound{1}.", 
							waterDiversionGateArtifact.DoorGate.IsOpen() ? "clockwise" : "counter-clockwise",
							sluiceGateArtifact.DoorGate.IsOpen() ? " and the violent churn of water" : "");

						waterDiversionGateArtifact.DoorGate.SetOpen(!waterDiversionGateArtifact.DoorGate.IsOpen());

						if (!waterDiversionGateArtifact.DoorGate.IsOpen())
						{
							gEngine.SteamTurbineStopsRunning();
						}

						GotoCleanup = true;

						break;
					}

					case 65:
					{
						// Gold pan

						var roomUids = new long[] { 36, 37, 38, 39 };
							
						if (roomUids.Contains(gGameState.Ro))
						{
							var goldNuggetsArtifact = gADB[66];

							Debug.Assert(goldNuggetsArtifact != null);

							if (goldNuggetsArtifact.Value < 40)
							{
								var goldNuggetCount = goldNuggetsArtifact.IsInLimbo() ? gEngine.RollDice(1, 2, 1) : gEngine.RollDice(1, 3, 0);

								gOut.Print("You pan for gold where the fine beach sand meets the lake water and, in due time, discover {0} glittering gold nugget{1}!",
									gEngine.GetStringFromNumber(goldNuggetCount, false, gEngine.Buf),
									goldNuggetCount != 1 ? "s" : "");

								goldNuggetsArtifact.Value += (goldNuggetCount * 2);

								goldNuggetsArtifact.Weight += (goldNuggetCount * 2);

								if (!goldNuggetsArtifact.IsCarriedByCharacter() /* || goldNuggetsArtifact too heavy to carry */)      // TODO: implement
								{
									goldNuggetsArtifact.SetInRoom(ActorRoom);
								}
							}
							else
							{
								gOut.Print("Your attempts to pan for more gold are unsuccessful.");
							}
						}
						else
						{
							gOut.Print("This isn't a good place to try your luck at prospecting.");

							NextState = gEngine.CreateInstance<IStartState>();
						}

						GotoCleanup = true;

						break;
					}

					case 69:
					{
						var mesquiteTreeArtifact = gADB[70];

						Debug.Assert(mesquiteTreeArtifact != null);

						// Rope

						if (gGameState.Ro == 37 || DobjArtifact.IsCarriedByContainer(mesquiteTreeArtifact))
						{
							gOut.Print("You're putting it to pretty good use right now!");
						}
						else if (gGameState.Ro == 42)
						{
							gOut.Print("Try to be a little more specific.");
						}
						else
						{
							gOut.Print("That doesn't do anything right now.");
						}

						GotoCleanup = true;

						break;
					}

					case 71:
					{
						var containerArtifact = DobjArtifact.GetCarriedByContainer();

						Debug.Assert(containerArtifact != null);
													
						// Lever

						if (containerArtifact.Uid == 29)
						{
							var gearsArtifact = gADB[30];

							Debug.Assert(gearsArtifact != null);

							var waterfallArtifact = gADB[59];

							Debug.Assert(waterfallArtifact != null);

							// Iron steam turbine

							if (gGameState.SteamTurbineRunning)
							{
								gEngine.PrintEffectDesc(73);

								gEngine.SteamTurbineStopsRunning();
							}
							else if (!containerArtifact.InContainer.IsOpen() && gearsArtifact.IsCarriedByContainer(containerArtifact) && waterfallArtifact.IsInRoom(ActorRoom))
							{
								gEngine.PrintEffectDesc(71);

								containerArtifact.StateDesc = " emitting a loud whirring sound";

								gGameState.SteamTurbineRunning = true;
							}
							else
							{
								gOut.Print("You pull the lever, but {0} remains silent.", containerArtifact.GetTheName());
							}
						}
						else if (!gGameState.SteamTurbineRunning)
						{
							gOut.Print("You pull the lever, but {0} remains silent.", containerArtifact.GetTheName());
						}

						// Rock crusher

						else if (containerArtifact.Uid == 48)
						{
							if (gGameState.RockCrusherRunning)
							{
								gOut.Print("You pull the lever, and {0} abruptly falls silent.", containerArtifact.GetTheName());

								containerArtifact.StateDesc = "";

								gGameState.RockCrusherRunning = false;
							}
							else
							{
								gEngine.PrintEffectDesc(74);

								gEngine.RockCrusherDestroysContents(ActorRoom);

								containerArtifact.StateDesc = " emitting a loud clanking sound";

								gGameState.RockCrusherRunning = true;
							}
						}

						// Rock grinder

						else if (containerArtifact.Uid == 49)
						{
							if (gGameState.RockGrinderRunning)
							{
								gOut.Print("You pull the lever, and {0} abruptly falls silent.", containerArtifact.GetTheName());

								containerArtifact.StateDesc = "";

								gGameState.RockGrinderRunning = false;
							}
							else
							{
								gEngine.PrintEffectDesc(75);

								gEngine.RockGrinderDestroysContents(ActorRoom);

								containerArtifact.StateDesc = " emitting a loud scraping sound";

								gGameState.RockGrinderRunning = true;
							}
						}

						// Debris Sifter

						else
						{
							if (gGameState.DebrisSifterRunning)
							{
								gOut.Print("You pull the lever, and {0} abruptly falls silent.", containerArtifact.GetTheName());

								containerArtifact.StateDesc = "";

								gGameState.DebrisSifterRunning = false;
							}
							else
							{
								gEngine.PrintEffectDesc(76);

								gEngine.DebrisSifterVibratesContents(ActorRoom);

								containerArtifact.StateDesc = " emitting a loud clattering sound";

								gGameState.DebrisSifterRunning = true;
							}
						}

						GotoCleanup = true;

						break;
					}
				}
			}
		}
	}
}
