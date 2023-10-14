
// UseCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using System.Linq;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Components;
using EamonRT.Framework.States;
using TheWayfarersInn.Framework.Primitive.Enums;
using static TheWayfarersInn.Game.Plugin.Globals;

namespace TheWayfarersInn.Game.Commands
{
	[ClassMappings]
	public class UseCommand : EamonRT.Game.Commands.UseCommand, IUseCommand
	{
		public override void ExecuteForPlayer()
		{
			Debug.Assert(DobjArtifact != null);

			var nolanMonster = gMDB[24];

			Debug.Assert(nolanMonster != null);

			// Digging with shovel

			if (DobjArtifact.Uid == 13)
			{
				if (gGameState.GetNBTL(Friendliness.Enemy) <= 0)
				{
					// Digging in small graveyard

					if (ActorRoom.Uid == 18)
					{
						var childsApparitionMonster = gMDB[4];

						Debug.Assert(childsApparitionMonster != null);

						var crumblingBrickWallArtifact = gADB[53];

						Debug.Assert(crumblingBrickWallArtifact != null);

						var childsSkeletonArtifact = gADB[54];

						Debug.Assert(childsSkeletonArtifact != null);

						var teddyBearArtifact = gADB[55];

						Debug.Assert(teddyBearArtifact != null);

						var freshlyDugHoleArtifact = gADB[58];

						Debug.Assert(freshlyDugHoleArtifact != null);

						var moundOfDirtArtifact = gADB[59];

						Debug.Assert(moundOfDirtArtifact != null);

						if (!gGameState.CharlotteRestInPeace)
						{ 
							if (freshlyDugHoleArtifact.IsInLimbo())
							{
								gEngine.PrintEffectDesc(12);

								freshlyDugHoleArtifact.SetInRoom(ActorRoom);

								moundOfDirtArtifact.SetInRoom(ActorRoom);
							}
							else
							{
								gEngine.PrintEffectDesc(13);

								if (childsSkeletonArtifact.IsCarriedByContainer(freshlyDugHoleArtifact))
								{
									if (gGameState.CharlotteBonesBlessed && teddyBearArtifact.IsCarriedByContainer(freshlyDugHoleArtifact))
									{
										gEngine.PrintEffectDesc(62);

										childsApparitionMonster.SetInLimbo();

										gGameState.CharlotteRestInPeace = true;
									}
									else
									{
										gOut.Print("While Charlotte's bones are in a far better place, you sense her spirit may still linger.");
									}
								}

								freshlyDugHoleArtifact.SetInLimbo();

								moundOfDirtArtifact.SetInLimbo();
							}
						}
						else
						{
							gEngine.PrintEffectDesc(5);
						}
					}
					else
					{
						var digResult = ActorRoom.EvalRoomType("This isn't a suitable place for digging!", "You dig for a while but find nothing of interest.");

						gOut.Print(digResult);
					}

					NextState = gEngine.CreateInstance<IMonsterStartState>();
				}
				else
				{
					PrintEnemiesNearby();

					NextState = gEngine.CreateInstance<IStartState>();
				}
			}
	
			// Using leather sectionals

			else if (DobjArtifact.Uid == 36)
			{
				gOut.Print("There's no time to lounge about with mysteries to solve!");

				NextState = gEngine.CreateInstance<IMonsterStartState>();
			}

			// Using courtyard benches

			else if (DobjArtifact.Uid == 40)
			{
				if (gGameState.GetNBTL(Friendliness.Enemy) <= 0)
				{
					gEngine.PrintEffectDesc(112);

					NextState = gEngine.CreateInstance<IMonsterStartState>();
				}
				else
				{
					PrintEnemiesNearby();

					NextState = gEngine.CreateInstance<IStartState>();
				}
			}

			// Using mop / broom

			else if (DobjArtifact.Uid == 46 || DobjArtifact.Uid == 111)
			{
				var actionType = DobjArtifact.Uid == 46 ? "mop" : "sweep";

				gOut.Print(ActorRoom.EvalRoomType($"You {actionType} the floor for a while, which looks a little bit cleaner.", $"You want to {actionType}... the ground?"));

				DobjArtifact.Moved = true;

				NextState = gEngine.CreateInstance<IMonsterStartState>();
			}

			// Using bucket with water well or purification pool

			else if (DobjArtifact.Uid == 47)
			{
				var waterWellArtifact = gADB[17];

				Debug.Assert(waterWellArtifact != null);

				var purificationPoolArtifact = gADB[24];

				Debug.Assert(purificationPoolArtifact != null);

				var waterArtifact = gADB[60];

				Debug.Assert(waterArtifact != null);

				if (waterWellArtifact.IsInRoom(ActorRoom) || waterWellArtifact.IsEmbeddedInRoom(ActorRoom))
				{
					gEngine.RevealEmbeddedArtifact(ActorRoom, waterWellArtifact);

					if (waterArtifact.IsInLimbo())
					{
						gOut.Print("You fill {0} with fresh water from {1}.", DobjArtifact.GetTheName(), waterWellArtifact.GetTheName());

						waterArtifact.SetCarriedByContainer(DobjArtifact);
						
						if (!waterArtifact.Seen)
						{
							PrintFullDesc(waterArtifact, false, false);

							waterArtifact.Seen = true;
						}
					}
					else
					{
						PrintDontNeedTo();

						NextState = gEngine.CreateInstance<IStartState>();
					}
				}
				else if (purificationPoolArtifact.IsInRoom(ActorRoom) || purificationPoolArtifact.IsEmbeddedInRoom(ActorRoom))
				{
					gEngine.RevealEmbeddedArtifact(ActorRoom, purificationPoolArtifact);

					if (waterArtifact.IsCarriedByContainer(DobjArtifact))
					{
						gOut.Print("You pour {0} of water into {1}.", DobjArtifact.GetTheName(), purificationPoolArtifact.GetTheName());

						waterArtifact.SetCarriedByContainer(purificationPoolArtifact);
					}
					else
					{
						PrintDontNeedTo();

						NextState = gEngine.CreateInstance<IStartState>();
					}
				}
				else
				{
					gOut.Print("That doesn't do anything right now.");
				}

				if (NextState == null)
				{
					NextState = gEngine.CreateInstance<IMonsterStartState>();
				}
			}

			// Using water well or purification pool with bucket

			else if (DobjArtifact.Uid == 17 || DobjArtifact.Uid == 24)
			{
				var bucketArtifact = gADB[47];

				Debug.Assert(bucketArtifact != null);

				if (bucketArtifact.IsInRoom(ActorRoom) || bucketArtifact.IsCarriedByMonster(ActorMonster))
				{
					var command = gEngine.CreateInstance<IUseCommand>();

					CopyCommandData(command);

					command.Dobj = bucketArtifact;

					NextState = command;
				}
				else
				{
					gOut.Print("That doesn't do anything right now.");

					NextState = gEngine.CreateInstance<IMonsterStartState>();
				}
			}

			// Using pews in temple

			else if (DobjArtifact.Uid == 22)
			{
				var childsSkeletonArtifact = gADB[54];

				Debug.Assert(childsSkeletonArtifact != null);

				var altarArtifact = gADB[23];

				Debug.Assert(altarArtifact != null);

				var purificationPoolArtifact = gADB[24];

				Debug.Assert(purificationPoolArtifact != null);

				var waterArtifact = gADB[60];

				Debug.Assert(waterArtifact != null);

				gOut.Print("You sit in {0}, bow your head in reverence, then pray to a higher power.", DobjArtifact.GetTheName());

				if (childsSkeletonArtifact.IsCarriedByContainer(purificationPoolArtifact) && waterArtifact.IsCarriedByContainer(purificationPoolArtifact) && !gGameState.CharlotteBonesPurified)
				{
					gEngine.PrintEffectDesc(14);

					gGameState.CharlotteBonesPurified = true;
				}
				else if (childsSkeletonArtifact.IsCarriedByContainer(altarArtifact) && gGameState.CharlotteBonesPurified && !gGameState.CharlotteBonesBlessed)
				{
					gEngine.PrintEffectDesc(16);

					gGameState.CharlotteBonesBlessed = true;
				}
				else
				{
					PrintNothingHappens();
				}

				NextState = gEngine.CreateInstance<IMonsterStartState>();
			}

			// Using feather bed, get lucid nightmare of Charlotte's fate

			else if (DobjArtifact.Uid == 79)
			{
				if (gGameState.GetNBTL(Friendliness.Enemy) <= 0)
				{
					if (!gGameState.CharlotteDeathSeen)
					{
						var unseenApparitionMonster = gMDB[2];

						Debug.Assert(unseenApparitionMonster != null);

						var childsApparitionMonster = gMDB[4];

						Debug.Assert(childsApparitionMonster != null);

						var northDoorArtifact = gADB[73];

						Debug.Assert(northDoorArtifact != null);

						gEngine.PrintEffectDesc(51);

						gEngine.In.KeyPress(gEngine.Buf);

						gOut.Print("{0}", gEngine.LineSep);

						gEngine.PrintEffectDesc(52);

						gEngine.In.KeyPress(gEngine.Buf);

						gOut.Print("{0}", gEngine.LineSep);

						gEngine.PrintEffectDesc(55);

						var combatComponent = gEngine.CreateInstance<ICombatComponent>(x =>
						{
							x.SetNextStateFunc = s => NextState = s;

							x.ActorRoom = ActorRoom;

							x.Dobj = ActorMonster;

							x.OmitArmor = true;
						});

						combatComponent.ExecuteCalculateDamage(1, 4);

						if (gGameState.Die > 0)
						{
							goto Cleanup;
						}

						gGameState.CharlotteDeathSeen = true;

						unseenApparitionMonster.SetInLimbo();

						childsApparitionMonster.SetInRoomUid(37);

						gGameState.SetEventState(EventState.ChildsApparition, 1);

						gGameState.AfterPrintPlayerRoomEventHeap.Insert02(gGameState.CurrTurn + 1, "ChildsApparition", 0, null);

						gEngine.PrintEffectDesc(60);

						northDoorArtifact.DoorGate.SetOpen(true);
					}
					else
					{
						gOut.Print("You can't bring yourself to use {0} ever again!", DobjArtifact.GetTheName());
					}

					NextState = gEngine.CreateInstance<IMonsterStartState>();
				}
				else
				{
					PrintEnemiesNearby();

					NextState = gEngine.CreateInstance<IStartState>();
				}
			}

			// Using four-poster bed and various furniture set beds

			else if (DobjArtifact.Uid == 82 || (DobjArtifact.Uid == 154 && DobjArtifact.ParserMatchName.Equals("bed", StringComparison.OrdinalIgnoreCase)))
			{
				PrintDontNeedTo();

				NextState = gEngine.CreateInstance<IStartState>();
			}

			// Using ladder in small barn

			else if (DobjArtifact.Uid == 112)
			{
				if (!gGameState.LadderUsed)
				{
					if (ActorRoom.Uid == 20 || ActorRoom.Uid == 67)
					{
						gEngine.PrintEffectDesc(ActorRoom.Uid == 20 ? 48 : 49);

						if (!DobjArtifact.IsInRoom(ActorRoom))
						{
							DobjArtifact.SetInRoom(ActorRoom);
						}

						gGameState.LadderUsed = true;
					}
					else
					{
						gOut.Print("That doesn't really help you here.");
					}
				}
				else
				{
					gOut.Print("You're already putting it to good use!");
				}

				NextState = gEngine.CreateInstance<IMonsterStartState>();
			}

			// Using bell

			else if (DobjArtifact.Uid == 120)
			{
				gOut.Print(gActorRoom(this).IsWayfarersInnRoom() ? "Most unwise - whatever arrives will likely give you very poor service." : "Ding!");

				NextState = gEngine.CreateInstance<IMonsterStartState>();
			}

			// Using pool table

			else if (DobjArtifact.Uid == 138)
			{
				if (gGameState.GetNBTL(Friendliness.Enemy) <= 0)
				{
					gOut.Print("{0} has long since seen its last game of pool.", DobjArtifact.GetTheName(true));

					NextState = gEngine.CreateInstance<IMonsterStartState>();
				}
				else
				{
					PrintEnemiesNearby();

					NextState = gEngine.CreateInstance<IStartState>();
				}
			}

			// Using dartboard

			else if (DobjArtifact.Uid == 139)
			{
				if (gGameState.GetNBTL(Friendliness.Enemy) <= 0)
				{
					if (ActorRoom.Uid == 27 && DobjArtifact.IsInRoom(ActorRoom))
					{
						if (!gGameState.DartboardCreepsOut)
						{
							gEngine.PrintEffectDesc(131);

							gGameState.DartboardCreepsOut = true;
						}

						// TODO: possible future mini-game ???

						var rl = gEngine.RollDice(1, 100, 0);

						gOut.Print("You{0} play a quick game of darts{1}",
							nolanMonster.IsInRoom(ActorRoom) ? " and Nolan" : "", 
							nolanMonster.IsInRoom(ActorRoom) ? string.Format(" and {0} handily!", rl > 50 ? "you beat him" : "he beats you") : ".");
					}
					else
					{
						gOut.Print("There's no time for fun and games right now!");
					}

					NextState = gEngine.CreateInstance<IMonsterStartState>();
				}
				else
				{
					PrintEnemiesNearby();

					NextState = gEngine.CreateInstance<IStartState>();
				}
			}

			// Using wooden chess pieces

			else if (DobjArtifact.Uid == 140)
			{
				if (gGameState.GetNBTL(Friendliness.Enemy) <= 0)
				{
					gEngine.PrintEffectDesc(125);

					NextState = gEngine.CreateInstance<IMonsterStartState>();
				}
				else
				{
					PrintEnemiesNearby();

					NextState = gEngine.CreateInstance<IStartState>();
				}
			}

			// Using deck of cards

			else if (DobjArtifact.Uid == 141)
			{
				if (gGameState.GetNBTL(Friendliness.Enemy) <= 0)
				{
					var rl = gEngine.RollDice(1, 100, 0);

					gOut.Print("You{0} play a quick game of cards{1}",
						nolanMonster.IsInRoom(ActorRoom) ? " and Nolan" : "",
						nolanMonster.IsInRoom(ActorRoom) ? string.Format(" and {0} handily!", rl > 50 ? "you beat him" : "he beats you") : ".");

					// Using deck of cards damages its value

					if (DobjArtifact.Value > 1)
					{
						gEngine.PrintEffectDesc(130);

						rl = gEngine.RollDice(1, 5, 0);		// TODO: verify value loss amount

						DobjArtifact.Value = Math.Max(DobjArtifact.Value - rl, 1);
					}

					NextState = gEngine.CreateInstance<IMonsterStartState>();
				}
				else
				{
					PrintEnemiesNearby();

					NextState = gEngine.CreateInstance<IStartState>();
				}
			}

			// Using board game

			else if (DobjArtifact.Uid == 142)
			{
				if (gGameState.GetNBTL(Friendliness.Enemy) <= 0)
				{
					gEngine.PrintEffectDesc(129);

					NextState = gEngine.CreateInstance<IMonsterStartState>();
				}
				else
				{
					PrintEnemiesNearby();

					NextState = gEngine.CreateInstance<IStartState>();
				}
			}

			// Using anvil / hammer

			else if (DobjArtifact.Uid == 161 || (DobjArtifact.Uid == 164 && DobjArtifact.GetInRoomUid(true) == 65))
			{
				if (gGameState.GetNBTL(Friendliness.Enemy) <= 0)
				{
					var hammerArtifact = gADB[164];

					Debug.Assert(hammerArtifact != null);

					if (hammerArtifact.IsCarriedByMonster(ActorMonster))
					{
						var forgedArtifact = gEngine.GetArtifactList(a => a.IsCarriedByContainerUid(161) && a.GetCarriedByContainerContainerType() == ContainerType.On).FirstOrDefault();

						if (forgedArtifact != null)
						{
							if (forgedArtifact.GeneralWeapon != null)
							{
								if (!gGameState.ForgedArtifactUids.Contains(forgedArtifact.Uid))
								{
									var rl = gEngine.RollDice(1, 5, 0);

									if (gGameState.ForgecraftCodexRead)
									{
										var improvementDescs = new string[] { "", "dubious", "fair", "good", "excellent", "amazing" };

										gOut.Print("You get to work on {0}, given your newfound knowledge. The improvements to the weapon{1} are {2}.", forgedArtifact.GetTheName(), forgedArtifact.EvalPlural("", "s"), improvementDescs[rl]);

										forgedArtifact.GeneralWeapon.Field1 = Math.Min(forgedArtifact.GeneralWeapon.Field1 + rl, 30);
									}
									else
									{
										var damageDescs = new string[] { "", "light", "limited", "bad", "severe", "catastrophic" };

										gOut.Print("You get to work on {0} despite your complete lack of knowledge. The damage to the weapon{1} is {2}.", forgedArtifact.GetTheName(), forgedArtifact.EvalPlural("", "s"), damageDescs[rl]);

										forgedArtifact.GeneralWeapon.Field1 = Math.Max(forgedArtifact.GeneralWeapon.Field1 - rl, -30);
									}

									gGameState.ForgedArtifactUids.Add(forgedArtifact.Uid);
								}
								else
								{
									gOut.Print("There isn't anything more you can do with {0}.", forgedArtifact.GetTheName());
								}
							}
							else
							{
								gOut.Print("You can only attempt to forge weapons!");
							}
						}
						else
						{
							gOut.Print("It's not clear what you want to attempt to forge.");
						}

						NextState = gEngine.CreateInstance<IMonsterStartState>();
					}
					else
					{
						gOut.Print("You aren't carrying {0}.", DobjArtifact.Uid == 161 ? hammerArtifact.GetArticleName() : "it");

						NextState = gEngine.CreateInstance<IStartState>();
					}
				}
				else
				{
					PrintEnemiesNearby();

					NextState = gEngine.CreateInstance<IStartState>();
				}
			}

			// Using adventuring supplies

			else if (DobjArtifact.Uid == 187)
			{
				gOut.Print("You quickly realize this is just a collection of useless junk.");

				NextState = gEngine.CreateInstance<IMonsterStartState>();
			}
			else
			{
				base.ExecuteForPlayer();
			}

		Cleanup:

			;
		}
	}
}
