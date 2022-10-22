
// GetPlayerInputState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using TheVileGrimoireOfJaldial.Framework.Primitive.Enums;
using static TheVileGrimoireOfJaldial.Game.Plugin.Globals;

namespace TheVileGrimoireOfJaldial.Game.States
{
	[ClassMappings]
	public class GetPlayerInputState : EamonRT.Game.States.GetPlayerInputState, IGetPlayerInputState
	{
		public override void ProcessEvents(EventType eventType)
		{
			base.ProcessEvents(eventType);

			if (eventType == EventType.BeforePrintCommandPrompt)
			{
				Debug.Assert(gCharMonster != null);

				var room = gCharMonster.GetInRoom() as Framework.IRoom;

				Debug.Assert(room != null);

				// Describe secret doors

				if (room.IsLit() && gSentenceParser.IsInputExhausted)
				{
					if (room.Uid == 54 && (gGameState.GetSecretDoor(1) || gGameState.GetSecretDoor(2)))
					{
						gOut.Print(gGameState.GetSecretDoor(1) && gGameState.GetSecretDoor(2) ? "You've found secret doors leading north and south." : gGameState.GetSecretDoor(1) ? "You've found a secret door leading north." : "You've found a secret door leading south.");
					}
					else if (room.Uid == 55 && gGameState.GetSecretDoor(1))
					{
						gOut.Print("You've found a door leading south.");
					}
					else if (room.Uid == 56 && (gGameState.GetSecretDoor(2) || gGameState.GetSecretDoor(4)))
					{
						gOut.Print(gGameState.GetSecretDoor(2) && gGameState.GetSecretDoor(4) ? "You've found secret doors leading north and east." : gGameState.GetSecretDoor(2) ? "You've found a secret door leading north." : "You've found a secret door leading east.");
					}
					else if (room.Uid == 58 && gGameState.GetSecretDoor(3))
					{
						gOut.Print("You've found a secret door leading east.");
					}
					else if (room.Uid == 63 && gGameState.GetSecretDoor(3))
					{
						gOut.Print("You've found a secret door leading west.");
					}
					else if (room.Uid == 68 && gGameState.GetSecretDoor(4))
					{
						gOut.Print("You've found a secret door leading west.");
					}
					else if (room.Uid == 74 && (gGameState.GetSecretDoor(5) || gGameState.GetSecretDoor(6)))
					{
						gOut.Print(gGameState.GetSecretDoor(5) && gGameState.GetSecretDoor(6) ? "You've discovered secret doors leading north and south." : gGameState.GetSecretDoor(5) ? "You've discovered a secret door leading north." : "You've discovered a secret door leading south.");
					}
					else if (room.Uid == 87 && gGameState.GetSecretDoor(7))
					{
						gOut.Print("You've discovered a trapdoor leading down into darkness.");
					}
					else if (room.Uid == 100 && gGameState.GetSecretDoor(9))
					{
						gOut.Print("You've discovered a secret door leading north.");
					}
					else if (room.Uid == 101 && gGameState.GetSecretDoor(8))
					{
						gOut.Print("You've discovered a secret door leading west.");
					}
					else if (room.Uid == 102 && gGameState.GetSecretDoor(11))
					{
						gOut.Print("You've found a secret door leading east.");
					}
					else if (room.Uid == 115 && gGameState.GetSecretDoor(10))
					{
						gOut.Print("You've found a secret door leading north.");
					}
					else if (room.Uid == 116 && gGameState.GetSecretDoor(12))
					{
						gOut.Print("You've found a secret panel in the fountain bottom - you may go down.");
					}
				}

				// Describe weather conditions

				if ((room.IsRainyRoom() || room.IsFoggyRoom()) && gSentenceParser.IsInputExhausted)
				{
					var rainyWeathers = new string[] { "", "a drizzle", "light rain", "heavy rain", "a downpour" };

					var foggyWeathers = new string[] { "", "a light mist", "a light fog bank", "a heavy fog bank", "an impenetrable fog bank" };

					var weatherDesc = room.IsRainyRoom() ? rainyWeathers[(int)room.GetWeatherIntensity()] : foggyWeathers[(int)room.GetWeatherIntensity()];

					gOut.Print(room.IsRainyRoom() ? "You are caught in {0}." : "The area is shrouded in {0}.", weatherDesc);
				}

				if (gEngine.ShouldPreTurnProcess)
				{
					var shadowMonster = gMDB[9];

					Debug.Assert(shadowMonster != null);

					// Shadow regenerates wounds

					if (!shadowMonster.IsInLimbo() && shadowMonster.DmgTaken > 0)
					{
						if (shadowMonster.IsInRoom(room) && room.IsLit())
						{
							gOut.Print("{0} regenerates some of its wounds!", shadowMonster.GetTheName(true));
						}

						shadowMonster.DmgTaken -= gEngine.RollDice(1, 4, 0);

						if (shadowMonster.DmgTaken < 0)
						{
							shadowMonster.DmgTaken = 0;
						}
					}

					var willOTheWispMonster = gMDB[10];

					Debug.Assert(willOTheWispMonster != null);

					// Will-o-the-wisp regenerates wounds

					if (!willOTheWispMonster.IsInLimbo() && willOTheWispMonster.DmgTaken > 0)
					{
						if (willOTheWispMonster.IsInRoom(room) && room.IsLit())
						{
							gOut.Print("{0} regenerates!", willOTheWispMonster.GetTheName(true));
						}

						willOTheWispMonster.DmgTaken -= gEngine.RollDice(1, 2, 0);

						if (willOTheWispMonster.DmgTaken < 0)
						{
							willOTheWispMonster.DmgTaken = 0;
						}
					}

					var specterMonster = gMDB[14];

					Debug.Assert(specterMonster != null);

					// Specter regenerates wounds

					if (!specterMonster.IsInLimbo() && specterMonster.DmgTaken > 0)
					{
						if (specterMonster.IsInRoom(room) && room.IsLit())
						{
							gOut.Print("{0} regenerates!", specterMonster.GetTheName(true));
						}

						specterMonster.DmgTaken -= gEngine.RollDice(1, 6, 0);

						if (specterMonster.DmgTaken < 0)
						{
							specterMonster.DmgTaken = 0;
						}
					}

					var wraithMonster = gMDB[16];

					Debug.Assert(wraithMonster != null);

					// Wraith regenerates wounds

					if (!wraithMonster.IsInLimbo() && wraithMonster.DmgTaken > 0)
					{
						if (wraithMonster.IsInRoom(room) && room.IsLit())
						{
							gOut.Print("{0} regenerates!", wraithMonster.GetTheName(true));
						}

						wraithMonster.DmgTaken -= gEngine.RollDice(1, 3, 0);

						if (wraithMonster.DmgTaken < 0)
						{
							wraithMonster.DmgTaken = 0;
						}
					}

					var waterWeirdMonster = gMDB[38];

					Debug.Assert(waterWeirdMonster != null);

					// Water weird regenerates wounds

					if (!waterWeirdMonster.IsInLimbo() && waterWeirdMonster.DmgTaken > 0)
					{
						if (waterWeirdMonster.IsInRoom(room) && room.IsLit())
						{
							gOut.Print("{0} regenerates all of its wounds!", waterWeirdMonster.GetTheName(true));
						}

						waterWeirdMonster.DmgTaken = 0;
					}

					// Burn down paralysis

					var paralyzedMonsterList = gEngine.GetMonsterList(m => gGameState.ParalyzedTargets.ContainsKey(m.Uid));

					foreach (var monster in paralyzedMonsterList)
					{
						var rl = gEngine.RollDice(1, 100, 0);

						var saved = monster.IsCharacterMonster() ? gEngine.SaveThrow(Stat.Hardiness) : rl > 80;

						if (saved || --gGameState.ParalyzedTargets[monster.Uid] <= 0)
						{
							gGameState.ParalyzedTargets.Remove(monster.Uid);

							if (monster.IsInRoom(room) && (monster.IsCharacterMonster() || room.IsLit()))
							{
								var monsterName = string.Empty;

								if (monster.IsCharacterMonster())
								{
									monsterName = "Your";
								}
								else
								{
									monsterName = monster.GetTheName(true).AddPossessiveSuffix();
								}

								gOut.Print("{0} paralysis has worn off{1}.", monsterName, saved ? " prematurely" : "");
							}
						}
					}

					// Burn down beholder's clumsiness spells

					var clumsyMonsterList = gEngine.GetMonsterList(m => gGameState.ClumsyTargets.ContainsKey(m.Uid));

					foreach (var monster in clumsyMonsterList)
					{
						var roundsList = gGameState.ClumsyTargets[monster.Uid];

						Debug.Assert(roundsList != null && roundsList.Count > 0);

						var expiredSpells = 0;

						var i = 0;

						while (i < roundsList.Count)
						{
							if (--roundsList[i] <= 0)
							{
								roundsList.RemoveAt(i);

								expiredSpells++;
							}
							else
							{
								i++;
							}
						}

						if (roundsList.Count == 0)
						{
							gGameState.ClumsyTargets.Remove(monster.Uid);

							if (monster.IsInRoom(room) && (monster.IsCharacterMonster() || room.IsLit()))
							{
								var monsterName = string.Empty;

								if (monster.IsCharacterMonster())
								{
									monsterName = "Your";
								}
								else
								{
									monsterName = monster.GetTheName(true).AddPossessiveSuffix();
								}

								gOut.Print("{0} agility has returned.", monsterName);
							}
						}
						else if (expiredSpells > 0)
						{
							if (monster.IsInRoom(room) && (monster.IsCharacterMonster() || room.IsLit()))
							{
								gOut.Print("{0} a little more agile.", monster.IsCharacterMonster() ? "You feel" : monster.GetTheName(true) + " looks");
							}
						}
					}

					var efreetiMonster = gMDB[50];

					Debug.Assert(efreetiMonster != null);

					var efreetiRoom = efreetiMonster.GetInRoom() as Framework.IRoom;

					var efreetiInRainyRoom = efreetiRoom != null && efreetiRoom.IsRainyRoom() && efreetiRoom.GetWeatherIntensity() >= 2;

					// Efreeti goes poof

					if (!efreetiMonster.IsInLimbo() && (efreetiInRainyRoom || !efreetiMonster.IsInRoom(room) || !efreetiMonster.CheckNBTLHostility()) && (efreetiInRainyRoom || gEngine.RollDice(1, 100, 0) <= 50))
					{
						if (efreetiMonster.IsInRoom(room) && room.IsLit())
						{
							gOut.Print("{0}{1} vanishes into thin air.", efreetiMonster.GetTheName(true), efreetiInRainyRoom ? ", seeing that it's raining," : efreetiMonster.Reaction == Friendliness.Friend ? ", seeing that you aren't in any immediate danger," : "");
						}

						efreetiMonster.SetInLimbo();
					}

					var cloakAndCowlArtifact = gADB[45];

					Debug.Assert(cloakAndCowlArtifact != null);

					// Dark hood and small glade

					if (cloakAndCowlArtifact.IsInLimbo())
					{
						var darkHoodMonster = gMDB[21];

						Debug.Assert(darkHoodMonster != null);

						if (!darkHoodMonster.IsInLimbo())
						{
							var darkHoodInPlayerRoom = darkHoodMonster.IsInRoom(room);

							var darkHoodVanishes = false;

							if (gGameState.IsDayTime())
							{
								darkHoodMonster.SetInLimbo();

								darkHoodVanishes = true;
							}
							else if (room.Uid != 23 && !darkHoodMonster.IsInRoomUid(23))
							{
								darkHoodMonster.SetInRoomUid(23);

								darkHoodVanishes = true;
							}

							if (darkHoodInPlayerRoom && darkHoodVanishes)
							{
								gOut.Print("{0} suddenly vanishes, seemingly into thin air.", darkHoodMonster.GetTheName(true));
							}
						}
					}

					// Flavor effects

					if (gEngine.EventRoll <= 3 && gEngine.FrequencyRoll <= gGameState.FlavorFreqPct)
					{
						var idx = gEngine.RollDice(1, 8, -1);

						if (room.IsGroundsRoom())
						{
							var rl = gEngine.RollDice(1, 9, 0);

							if (rl <= 5)
							{
								if (rl >= 3 || (gGameState.IsDayTime() && !room.IsRainyRoom() && (!room.IsFoggyRoom() || room.GetWeatherIntensity() <= 2)))
								{
									var effect = gEDB[rl];

									Debug.Assert(effect != null);

									if (rl == 3)
									{
										gOut.Print("{0}", gGameState.IsNightTime() ? "You notice the stars are blotted out by the dark clouds overhead." : room.IsRainyRoom() ? "You notice the storm clouds swiftly rolling by overhead." : room.IsFoggyRoom() ? "You notice once in a while the sky is visible through a break in the fog." : effect.Desc);
									}
									else
									{
										gOut.Print("{0}", effect.Desc);
									}
								}
							}

							// Distant graveyard sounds

							else if (rl == 6)
							{
								var distantSounds = new string[]
								{
									"what seems to be loud footfalls.", "a shrill scream - possibly human, but probably not.", "the sounds of wildlife - crickets, and bullfrogs.",
									"the muffled beat of a drum.", "the whisper of wind through the trees.", "a loud crashing sound.", "the sounds of battle!", "a peal of thunder."
								};

								gOut.Print("You hear, in the distance, {0}", distantSounds[(int)idx]);
							}

							// Nearby graveyard sounds

							else if (rl == 7)
							{
								var nearbySounds = new string[]
								{
									"what sounds like footsteps.", "a strange hissing sound.", "the faint sounds of sobbing.", "the rustling of leaves.", "the chirping of birds.",
									"quiet laughter.", "a faint humming sound.", "a faint clicking sound."
								};

								gOut.Print("You hear, very close by, {0}", nearbySounds[(int)idx]);
							}

							// Graveyard aromas

							else if (rl == 8)
							{
								var aromas = new string[]
								{
									"meat frying on an open skillet.", "the putrid odor of decaying flesh.", "the aroma of vanilla.", "the odor of offal.", "a refreshing pine scent, carried by the wind.",
									"what can only be described as ancient death.", "a cloying sweet aroma, that of flowers.", "the reeking odor of swamp methane."
								};

								gOut.Print("You smell {0}", aromas[(int)idx]);
							}

							// Graveyard sightings

							else
							{
								var sightings = new string[]
								{
									"a stark figure, who disappears behind a tombstone.", 
									string.Format(gGameState.IsNightTime() || room.IsRainyRoom() ? "a bolt of lightning far to the {0}." : room.IsFoggyRoom() ? "movement through the fog in the distance to the {0}." : "a plume of smoke rising far to the {0}.", gEngine.GetRandomElement(new string[] { "north", "south", "east", "west" })),
									"ephemeral wisps of steam rising from the damp earth.", 
									string.Format("a {0}, which piques your interest for a fleeting moment.", gEngine.GetRandomElement(new string[] { "bush", "tree", "rock", "tombstone" })),
									gGameState.IsDayTime() ? string.Format("a rare species of {0}, found only here.", gEngine.GetRandomElement(new string[] { "warbler", "gecko", "lichen", "mandrake root" })) : string.Format("the glow of {0} in the darkness nearby.", gEngine.GetRandomElement(new string[] { "fireflies", "mushrooms", "fungi", "corpse candles" })), 
									"an unintelligible symbol scratched into the ground.",
									gGameState.IsDayTime() || gGameState.Ls > 0 ? "a tombstone with an oddly familiar name on it." : "a crumbling statue of a weeping mourner, face in hands.",
									gGameState.IsDayTime() && (!room.IsFoggyRoom() || room.GetWeatherIntensity() <= 1) ? string.Format("a {0} flying high overhead.", gEngine.GetRandomElement(new string[] { "crow", "vulture", "falcon", "dragon" })) : string.Format("a {0} shadow in the sky from something flying overhead.", gEngine.GetRandomElement(new string[] { "tiny", "small", "middling", "large" }))
								};

								gOut.Print("You see {0}", sightings[(int)idx]);
							}
						}
						else if (room.IsCryptRoom())
						{
							var rl = gEngine.RollDice(1, 3, 0);

							// Distant underground sounds

							if (rl == 1)
							{
								var distantSounds = new string[]
								{
									"a hollow booming sound that echoes down the passage.", "the quiet thud of footsteps.", "a soft creaking sound.", "a strange whirring sound.",
									"the whistle of wind down a passageway.", "the quiet rattling of chains.", "a faint clicking sound that echoes down the passage.",
									"a blood-curdling scream!"
								};

								gOut.Print("You hear, in the distance, {0}", distantSounds[(int)idx]);
							}

							// Nearby underground sounds

							else if (rl == 2)
							{
								var nearbySounds = new string[]
								{
									"a quiet conversation.", "a coughing sound, which quickly dies away.", "a wheezing sound.", "footfalls.", "cackling laughter.",
									"the sound of water dripping from the ceiling.", "a faint moaning which echoes down the corridors.", "a faint thud."
								};

								gOut.Print("You hear, very close by, {0}", nearbySounds[(int)idx]);
							}

							// Underground aromas

							else
							{
								var aromas = new string[]
								{
									"a fresh breeze that blows down the hallway.", "a putrid odor, probably unseen offal somewhere.", "the air, which grows increasingly stale.",
									"a rotting odor, quite unpleasant.", "yourself; you've been sweating again!", "the strong aroma of vanilla.", "the reek of decaying flesh.",
									"a cloying aroma, probably plant life somewhere nearby."
								};

								gOut.Print("You smell {0}", aromas[(int)idx]);
							}
						}
					}
				}

				// Reset decorations; they remain in limbo unless the normal Artifact resolution process fails

				var decoration41Artifact = gADB[41];

				Debug.Assert(decoration41Artifact != null);

				decoration41Artifact.Name = "DECORATION41";

				decoration41Artifact.Location = 0;

				decoration41Artifact.Field1 = 0;

				decoration41Artifact.Field2 = 0;

				var decoration42Artifact = gADB[42];

				Debug.Assert(decoration42Artifact != null);

				decoration42Artifact.Name = "DECORATION42";

				decoration42Artifact.Location = 0;

				decoration42Artifact.Field1 = 0;

				decoration42Artifact.Field2 = 0;
			}
		}
	}
}
