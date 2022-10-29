
// PrintPlayerRoomState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using TheVileGrimoireOfJaldial.Framework.Primitive.Enums;
using static TheVileGrimoireOfJaldial.Game.Plugin.Globals;

namespace TheVileGrimoireOfJaldial.Game.States
{
	[ClassMappings]
	public class PrintPlayerRoomState : EamonRT.Game.States.PrintPlayerRoomState, IPrintPlayerRoomState
	{
		public override void ProcessEvents(EventType eventType)
		{
			if (!gEngine.EncounterSurprises)
			{
				base.ProcessEvents(eventType);

				if (eventType == EventType.BeforePrintPlayerRoom && gEngine.ShouldPreTurnProcess)
				{
					Debug.Assert(gCharMonster != null);

					var room = gCharMonster.GetInRoom() as Framework.IRoom;

					Debug.Assert(room != null);

					var cloakAndCowlArtifact = gADB[45];

					Debug.Assert(cloakAndCowlArtifact != null);

					// Dark hood and small glade

					if (cloakAndCowlArtifact.IsInLimbo())
					{
						var darkHoodMonster = gMDB[21];

						Debug.Assert(darkHoodMonster != null);

						if (darkHoodMonster.IsInLimbo() && gGameState.IsNightTime())
						{
							darkHoodMonster.SetInRoomUid(23);

							if (darkHoodMonster.IsInRoom(room))
							{
								gEngine.PrintEffectDesc(110);
							}
						}
					}

					// Day/night cycle logic

					gGameState.Minute += 5;

					if (gGameState.Minute >= 60)
					{
						gGameState.Hour++;

						gGameState.Minute = 0;
					}

					if (gGameState.Hour >= 24)
					{
						gGameState.Day++;

						gGameState.Hour = 0;

						// New spells for a new day

						gGameState.LightningBolts = 0;

						gGameState.IceBolts = 0;

						gGameState.MentalBlasts = 0;

						gGameState.MysticMissiles = 0;

						gGameState.FireBalls = 0;

						gGameState.ClumsySpells = 0;
					}

					if (room.IsGroundsRoom())
					{
						if (gGameState.Hour == 6 && gGameState.Minute == 0)
						{
							gEngine.PrintEffectDesc(111);
						}
						else if (gGameState.Hour == 7 && gGameState.Minute == 0)
						{
							gEngine.PrintEffectDesc(room.GetWeatherIntensity() > 1 ? 112 : 113);
						}
						else if (gGameState.Hour == 8 && gGameState.Minute == 0)
						{
							gEngine.PrintEffectDesc(room.GetWeatherIntensity() > 1 ? 114 : 115);
						}
						else if (gGameState.Hour == 12 && gGameState.Minute == 0)
						{
							gEngine.PrintEffectDesc(room.GetWeatherIntensity() > 1 ? 116 : 117);
						}
						else if (gGameState.Hour == 18 && gGameState.Minute == 0)
						{
							gEngine.PrintEffectDesc(room.GetWeatherIntensity() > 1 ? 118 : 119);
						}
						else if (gGameState.Hour == 19 && gGameState.Minute == 0)
						{
							gEngine.PrintEffectDesc(room.GetWeatherIntensity() > 1 ? 120 : 121);
						}
						else if (gGameState.Hour == 0 && gGameState.Minute == 0)
						{
							gEngine.PrintEffectDesc(room.GetWeatherIntensity() > 1 ? 122 : 123);
						}
					}

					// Weather cycle logic

					var expiredWeather = false;

					if (gGameState.WeatherDuration > 0)
					{
						gGameState.WeatherDuration -= 5;

						if (gGameState.WeatherDuration < 0)
						{
							gGameState.WeatherDuration = 0;
						}
					}

					if (gGameState.WeatherDuration <= 0 && gGameState.WeatherType != WeatherType.None)
					{
						var rl = gEngine.RollDice(1, 99, 0);

						if (rl >= 1 && rl <= 33)
						{
							if (--gGameState.WeatherIntensity <= 0)
							{
								if (room.IsRainyRoom())
								{
									gEngine.PrintEffectDesc(124);
								}
								else if (room.IsFoggyRoom())
								{
									gEngine.PrintEffectDesc(125);
								}

								gGameState.FoggyRoomWeatherIntensity = 0;

								gGameState.WeatherType = WeatherType.None;

								expiredWeather = true;
							}
							else
							{
								if (room.IsRainyRoom())
								{
									gEngine.PrintEffectDesc(126);
								}
								else if (room.IsFoggyRoom() && gGameState.FoggyRoomWeatherIntensity >= 2)
								{
									gEngine.PrintEffectDesc(127);

									gGameState.FoggyRoomWeatherIntensity--;
								}

								gGameState.WeatherDuration += gEngine.RollDice(1, 100, 0);
							}
						}
						else if (rl >= 34 && rl <= 75)
						{
							gGameState.WeatherDuration += gEngine.RollDice(1, 100, 0);
						}
						else
						{
							if (gGameState.WeatherIntensity < 4)
							{
								if (room.IsRainyRoom())
								{
									gEngine.PrintEffectDesc(128);
								}
								else if (room.IsFoggyRoom())
								{
									gEngine.PrintEffectDesc(129);

									gGameState.FoggyRoomWeatherIntensity++;
								}

								gGameState.WeatherIntensity++;
							}

							gGameState.WeatherDuration += gEngine.RollDice(1, 100, 0);
						}
					}

					// Random events

					gEngine.EventRoll = gEngine.RollDice(1, 100, 0);

					gEngine.FrequencyRoll = gEngine.RollDice(1, 100, 0);

					// Weather pattern starters

					if (!expiredWeather && gEngine.EventRoll >= 4 && gEngine.EventRoll <= 9 && gEngine.FrequencyRoll <= gGameState.WeatherFreqPct)
					{
						if (gGameState.WeatherType == WeatherType.None)
						{
							var rl = gEngine.RollDice(1, 2, 0);

							if (rl == 1 || !gGameState.IsFoggyHours())
							{
								if (room.IsGroundsRoom())
								{
									gEngine.PrintEffectDesc(130);
								}

								gGameState.WeatherType = WeatherType.Rain;

								gGameState.WeatherDuration = gEngine.RollDice(1, 100, 0);

								gGameState.WeatherIntensity = 1;
							}
							else
							{
								if (room.IsGroundsRoom())
								{
									gEngine.PrintEffectDesc(131);
								}

								gGameState.WeatherType = WeatherType.Fog;

								gGameState.WeatherDuration = gEngine.RollDice(1, 60, 0);

								gGameState.WeatherIntensity = 1;

								gGameState.SetFoggyRoomWeatherIntensity(room);
							}
						}
					}

					// Encounters

					else if (((room.IsGroundsRoom() && gGameState.IsDayTime() && gEngine.EventRoll >= 10 && gEngine.EventRoll <= 12) || (((room.IsGroundsRoom() && gGameState.IsNightTime()) || room.IsCryptRoom()) && gEngine.EventRoll >= 10 && gEngine.EventRoll <= 14)) && gEngine.FrequencyRoll <= gGameState.EncounterFreqPct)
					{
						var enemyEncounter = gEngine.GetMonsterList(m => m.Uid <= 17 && m.IsInRoom(room) && m.Reaction == Friendliness.Enemy).FirstOrDefault();

						if (enemyEncounter == null)
						{
							IList<IMonster> encounterList = null;

							if (room.IsGroundsRoom())
							{
								encounterList = gEngine.GetMonsterList(m => m.Uid <= 17 && m.IsInLimbo() && (gGameState.IsNightTime() || m.Uid <= 6));
							}
							else
							{
								var invalidMonsterUids = new long[] { 2, 5, 10, 12 };

								encounterList = gEngine.GetMonsterList(m => m.Uid <= 17 && m.IsInLimbo() && !invalidMonsterUids.Contains(m.Uid));
							}

							if (encounterList.Count > 0)
							{
								var idx = gEngine.RollDice(1, encounterList.Count, -1);

								var monster = encounterList[(int)idx];

								// Wandering Monster appears to be fixed encounter when last Command type is Movement

								if (!gEngine.PlayerMoved)
								{
									if (gGameState.GetNBTL(Friendliness.Enemy) > 0)
									{
										gOut.Print("{0} hears the sounds of battle, and comes wandering by.", room.EvalLightLevel("Something", monster.GetArticleName(true)));
									}
									else if (room.IsGroundsRoom() && gGameState.IsNightTime())
									{
										gOut.Print("{0} suddenly materializes out of the darkness.", monster.GetArticleName(true));
									}
									else if (room.IsFoggyRoom())
									{
										gOut.Print("{0} suddenly materializes out of the fog.", monster.GetArticleName(true));
									}
									else if (room.IsRainyRoom() && room.GetWeatherIntensity() > 2)
									{
										gOut.Print("{0} suddenly materializes out of the rain.", monster.GetArticleName(true));
									}
									else if (room.IsGroundsRoom())
									{
										gOut.Print("{0} wanders into the area!", monster.GetArticleName(true));
									}
									else
									{
										gOut.Print("{0} wanders into the room!", room.EvalLightLevel("Something", monster.GetArticleName(true)));
									}
								}

								monster.InitGroupCount = monster.GroupCount;

								monster.CurrGroupCount = monster.GroupCount;

								monster.DmgTaken = 0;

								monster.SetInRoom(room);

								if (!gEngine.PlayerMoved)
								{
									var saved = gEngine.SaveThrow(Stat.Agility);

									if (!saved)
									{
										gOut.Print("You have been taken by surprise!");

										gEngine.InitiativeMonsterUid = monster.Uid;

										gEngine.EncounterSurprises = true;

										NextState = gEngine.CreateInstance<IMonsterStartState>();

										GotoCleanup = true;
									}
								}
							}
						}
					}

					// Random encounters not in player Room go poof

					var monsterList = gEngine.GetMonsterList(m => m.Uid <= 17 && !m.IsInLimbo() && !m.IsInRoom(room));

					foreach (var monster in monsterList)
					{
						var rl = gEngine.RollDice(1, 100, 0);

						if (rl > 50)
						{
							monster.SetInLimbo();
						}
					}
				}
			}
			else
			{
				gEngine.EncounterSurprises = false;
			}
		}
	}
}
