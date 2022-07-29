
// ExamineCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using System.Linq;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Components;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static TheVileGrimoireOfJaldial.Game.Plugin.PluginContext;

namespace TheVileGrimoireOfJaldial.Game.Commands
{
	[ClassMappings]
	public class ExamineCommand : EamonRT.Game.Commands.ExamineCommand, IExamineCommand
	{
		public override void ProcessEvents(EventType eventType)
		{
			base.ProcessEvents(eventType);

			if (eventType == EventType.AfterExamineMonsterHealthStatus)
			{
				if (!gActorRoom(this).IsDimLightRoomWithoutGlowingMonsters() || gGameState.Ls > 0)
				{
					if (gGameState.ParalyzedTargets.ContainsKey(DobjMonster.Uid))
					{
						gOut.Print("{0} {1} paralyzed at this time!", DobjMonster.GetTheName(true), DobjMonster.EvalPlural("is", "are"));
					}

					if (gGameState.ClumsyTargets.ContainsKey(DobjMonster.Uid))
					{
						gOut.Print("{0} {1} agility impaired at this time!", DobjMonster.GetTheName(true), DobjMonster.EvalPlural("is", "are"));
					}
				}
			}
		}

		public override void Execute()
		{
			var waterWeirdMonster = gMDB[38];

			Debug.Assert(waterWeirdMonster != null);

			// Large fountain and water weird

			if (DobjArtifact?.Uid == 24 && waterWeirdMonster.IsInLimbo() && !gGameState.WaterWeirdKilled && !Enum.IsDefined(typeof(ContainerType), ContainerType))
			{
				gEngine.PrintEffectDesc(101);

				waterWeirdMonster.SetInRoom(ActorRoom);

				NextState = Globals.CreateInstance<IStartState>();
			}

			// Decoration

			else if (DobjArtifact?.Uid == 41 && DobjArtifact.Field1 > 0 && (!gActorRoom(this).IsDimLightRoomWithoutGlowingMonsters() || gGameState.Ls > 0 || DobjArtifact.Field1 == 61 || DobjArtifact.Field1 == 62) && !Enum.IsDefined(typeof(ContainerType), ContainerType))
			{
				switch (DobjArtifact.Field1)
				{
					case 1:

						gEngine.PrintEffectDesc(18);

						break;

					case 2:

						gEngine.PrintEffectDesc(19);

						break;

					case 3:

						gEngine.PrintEffectDesc(20);

						break;

					case 4:

						gEngine.PrintEffectDesc(21);

						break;

					case 5:

						gEngine.PrintEffectDesc(22);

						break;

					case 6:

						gEngine.PrintEffectDesc(23);

						break;

					case 7:

						gEngine.PrintEffectDesc(24);

						break;

					case 8:

						gEngine.PrintEffectDesc(25);

						break;

					case 9:

						gEngine.PrintEffectDesc(26);

						break;

					case 10:

						gOut.Print("The grave, to your great surprise, has been dug out very recently, probably no more than {0} ago.  Hmm... it looks to be about your size.",
							gGameState.Day > 0 ? string.Format("{0} day{1}", gEngine.GetStringFromNumber(gGameState.Day, false, Globals.Buf), gGameState.Day != 1 ? "s" : "") :
							gGameState.Hour > 0 ? string.Format("{0} hour{1}", gEngine.GetStringFromNumber(gGameState.Hour, false, Globals.Buf), gGameState.Hour != 1 ? "s" : "") :
							string.Format("{0} minute{1}", gEngine.GetStringFromNumber(gGameState.Minute, false, Globals.Buf), gGameState.Minute != 1 ? "s" : ""));

						break;

					case 11:

						gEngine.PrintEffectDesc(27);

						break;

					case 12:

						gEngine.PrintEffectDesc(28);

						break;

					case 13:

						gEngine.PrintEffectDesc(29);

						break;

					case 14:

						gEngine.PrintEffectDesc(30);

						break;

					case 15:

						gEngine.PrintEffectDesc(31);

						break;

					case 16:

						gEngine.PrintEffectDesc(32);

						break;

					case 17:

						gEngine.PrintEffectDesc(33);

						break;

					case 18:
					{
						var rl = gEngine.RollDice(1, 100, 0);

						if (rl > 60)
						{
							var rl02 = 0L;

							do
							{
								rl02 = gEngine.RollDice(1, 4, 0);
							}
							while (rl02 == 2 && gGameState.GetNBTL(Friendliness.Enemy) > 0);

							gOut.Print("The rune you pick to examine turns out to be a glyph of {0}!", rl02 == 1 ? "death" : rl02 == 2 ? "sleep" : rl02 == 3 ? "teleportation" : "warding");

							if (rl02 == 1)
							{
								var saved = gEngine.SaveThrow(0);

								if (!saved)
								{
									gEngine.PrintEffectDesc(34);

									gGameState.Die = 1;

									NextState = Globals.CreateInstance<IPlayerDeadState>(x =>
									{
										x.PrintLineSep = true;
									});

									goto Cleanup;
								}
								else
								{
									gEngine.PrintEffectDesc(35);
								}
							}
							else if (rl02 == 2)
							{
								gEngine.PrintEffectDesc(36);

								gGameState.Day++;
							}
							else if (rl02 == 3)
							{
								gEngine.PrintEffectDesc(37);

								var groundsRooms = gRDB.Records.Cast<Framework.IRoom>().Where(r => r.IsGroundsRoom()).ToList();    // TODO: maybe add this crypt's Rooms as well

								var idx = gEngine.RollDice(1, groundsRooms.Count, -1);

								var newRoom = groundsRooms[(int)idx];

								gGameState.R2 = newRoom.Uid;

								NextState = Globals.CreateInstance<IAfterPlayerMoveState>(x =>
								{
									x.MoveMonsters = false;
								});

								goto Cleanup;
							}
							else
							{
								Direction direction = 0;

								gEngine.GetRandomMoveDirection(ActorRoom, ActorMonster, true, ref direction);

								Debug.Assert(Enum.IsDefined(typeof(Direction), direction));

								if (direction == Direction.Up || direction == Direction.Down || direction == Direction.In || direction == Direction.Out)
								{
									Globals.Buf.SetFormat(" {0}ward", direction.ToString().ToLower());
								}
								else
								{
									Globals.Buf.SetFormat(" to the {0}", direction.ToString().ToLower());
								}

								gOut.Print("You flee in terror{0}!", Globals.Buf);

								gGameState.R2 = ActorRoom.GetDirs(direction);

								NextState = Globals.CreateInstance<IPlayerMoveCheckState>(x =>
								{
									x.Direction = direction;

									x.Fleeing = true;
								});

								goto Cleanup;
							}
						}
						else
						{
							gEngine.PrintEffectDesc(38);

							gOut.WriteLine();

							gEngine.PrintTitle("The sands of time pass slowly here,".PadTRight(35, ' '), false);
							gEngine.PrintTitle("For those who've slipped away.".PadTRight(35, ' '), false);
							gEngine.PrintTitle("Yet all will surely rise again,".PadTRight(35, ' '), false);
							gEngine.PrintTitle("To see the break of day.".PadTRight(35, ' '), false);

							gEngine.PrintEffectDesc(39);
						}

						break;
					}

					case 19:

						gEngine.PrintEffectDesc(40);

						break;

					case 20:

						gEngine.PrintEffectDesc(41);

						break;

					case 21:

						gEngine.PrintEffectDesc(42);

						break;

					case 22:

						gEngine.PrintEffectDesc(43);

						break;

					case 23:

						gEngine.PrintEffectDesc(44);

						break;

					case 24:

						gEngine.PrintEffectDesc(45);

						break;

					case 25:

						gEngine.PrintEffectDesc(46);

						break;

					case 26:

						gEngine.PrintEffectDesc(47);

						break;

					case 27:

						gEngine.PrintEffectDesc(48);

						break;

					case 28:

						gEngine.PrintEffectDesc(49);

						break;

					case 29:

						gEngine.PrintEffectDesc(50);

						break;

					case 30:

						gOut.Print("The hole, which is several inches across, is far too small to fit into (and definitely too high up to reach), but when you stand directly under it, you can see {0}.",
							gGameState.IsDayTime() ? "blue skies above you" : "the dark nighttime sky above");

						break;

					case 31:

						gEngine.PrintEffectDesc(51);

						break;

					case 32:

						gEngine.PrintEffectDesc(52);

						break;

					case 33:

						gEngine.PrintEffectDesc(53);

						break;

					case 34:

						gEngine.PrintEffectDesc(54);

						break;

					case 35:

						gEngine.PrintEffectDesc(55);

						break;

					case 36:
					{
						gEngine.PrintEffectDesc(56);

						var rl = gEngine.RollDice(1, 4, 0);

						if (rl == 1)
						{
							gOut.WriteLine();

							gEngine.PrintTitle("...".PadTRight(51, ' '), false);
							gEngine.PrintTitle("River shall overflow, and tree shall bear no fruit.".PadTRight(51, ' '), false);
							gEngine.PrintTitle("...".PadTRight(51, ' '), false);

							gEngine.PrintEffectDesc(57);
						}
						else if (rl == 2)
						{
							gEngine.PrintEffectDesc(58);
						}
						else if (rl == 3)
						{
							gOut.WriteLine();

							gEngine.PrintTitle("How long have we lived in these valleys, dwelt upon these shores?".PadTRight(65, ' '), false);
							gEngine.PrintTitle("When shall we understand those things around us that shape life?".PadTRight(65, ' '), false);
							gEngine.PrintTitle("Our enemy wages war against us as we ponder such.".PadTRight(65, ' '), false);
							gEngine.PrintTitle("This knowledge kept from us, forever veiled in shadow.".PadTRight(65, ' '), false);
						}
						else
						{
							gOut.WriteLine();

							gEngine.PrintTitle("When the waves rise against you, pull you under, where shall you turn?".PadTRight(70, ' '), false);
							gEngine.PrintTitle("What deliverance from an icy grip, when all hope is lost?".PadTRight(70, ' '), false);
							gEngine.PrintTitle("Though the scales may cover your eyes, what's this - a revelation!".PadTRight(70, ' '), false);
							gEngine.PrintTitle("Merely speak the words of truth, and life shall again be yours:".PadTRight(70, ' '), false);

							gOut.WriteLine();

							gEngine.PrintTitle("\"Avarchrom Yarei Uttoximo.\"", false);
						}

						break;
					}

					case 37:

						gEngine.PrintEffectDesc(59);

						break;

					case 38:

						gEngine.PrintEffectDesc(60);

						break;

					case 39:

						gEngine.PrintEffectDesc(61);

						break;

					case 40:

						gEngine.PrintEffectDesc(62);

						break;

					case 41:

						gEngine.PrintEffectDesc(63);

						break;

					case 42:

						gEngine.PrintEffectDesc(64);

						break;

					case 43:

						gOut.Print("The dead goblins have been slain recently; you'd say no more than {0} days ago.  They appear to have been slashed severely, with well-placed strokes - blood lies splattered all over the walls and the floor.  All useful items have been taken from the bodies.",
							gEngine.GetStringFromNumber(gGameState.Day + 2, false, Globals.Buf));

						break;

					case 44:

						gEngine.PrintEffectDesc(65);

						break;

					case 45:

						gEngine.PrintEffectDesc(66);

						break;

					case 46:

						gEngine.PrintEffectDesc(67);

						break;

					case 47:

						gEngine.PrintEffectDesc(68);

						break;

					case 48:

						gEngine.PrintEffectDesc(69);

						break;

					case 49:

						gEngine.PrintEffectDesc(70);

						break;

					case 50:
					{
						gEngine.PrintEffectDesc(71);

						var rl = gEngine.RollDice(1, 100, 0);

						if (rl > 70)
						{
							var saved = gEngine.SaveThrow(0);

							gEngine.PrintEffectDesc(72);

							if (saved)
							{
								gEngine.PrintEffectDesc(73);
							}
							else
							{
								gEngine.PrintEffectDesc(74);
							}

							if (!saved)
							{
								gGameState.Die = 1;

								NextState = Globals.CreateInstance<IPlayerDeadState>(x =>
								{
									x.PrintLineSep = true;
								});

								goto Cleanup;
							}
						}

						break;
					}

					case 51:

						gEngine.PrintEffectDesc(75);

						break;

					case 52:

						gOut.Print("The stains on the eastern wall are obviously dried blood.{0}", gEngine.SaveThrow(Stat.Intellect) ? "  You notice that the stains abruptly end at a fine vertical crack in the wall.  Strange." : "");

						break;

					case 53:

						gEngine.PrintEffectDesc(76);

						break;

					case 54:
					{
						var saved = gEngine.SaveThrow(Stat.Intellect);

						var target = gEngine.GetRandomMonsterList(1, m => m.IsInRoom(ActorRoom)).FirstOrDefault();

						Debug.Assert(target != null);

						var rl = gEngine.RollDice(1, 100, 0);

						gEngine.PrintEffectDesc(77);

						if (saved)
						{
							gEngine.PrintEffectDesc(78);
						}
						else
						{
							gOut.Print("As you examine it, a crossbow bolt shoots out of the face's mouth, and strikes {0}!", rl > 50 ? (target.IsCharacterMonster() ? "you" : target.GetTheName()) : "the opposite wall");
						}

						if (!saved && rl > 50)
						{
							var combatComponent = Globals.CreateInstance<ICombatComponent>(x =>
							{
								x.Cast<Framework.Components.ICombatComponent>().CrossbowTrap = true;

								x.SetNextStateFunc = s => NextState = s;

								x.ActorRoom = target.GetInRoom();

								x.Dobj = target;
							});

							combatComponent.ExecuteCalculateDamage(2, 6);

							goto Cleanup;
						}

						break;
					}

					case 55:

						gEngine.PrintEffectDesc(79);

						break;

					case 56:
					{
						var giantCrayfishMonster = gMDB[37];

						Debug.Assert(giantCrayfishMonster != null);

						if (!giantCrayfishMonster.IsInLimbo() || gGameState.GiantCrayfishKilled)
						{
							gEngine.PrintEffectDesc(80);
						}
						else
						{
							gEngine.PrintEffectDesc(81);

							giantCrayfishMonster.SetInRoom(ActorRoom);

							var saved = gEngine.SaveThrow(Stat.Agility);

							if (!saved)
							{
								gOut.Print("You've been taken entirely by surprise.");

								Globals.InitiativeMonsterUid = giantCrayfishMonster.Uid;
							}
							else
							{
								NextState = Globals.CreateInstance<IStartState>();

								goto Cleanup;
							}
						}

						break;
					}

					case 57:
					case 58:
					case 59:
					case 60:

						var command = Globals.CreateInstance<IReadCommand>();

						CopyCommandData(command);

						NextState = command;

						break;

					case 61:

						var rainDescs = new string[]
						{
							"",
							string.Format("A sprinkling of rain falls on the cemetery from a patchwork of{0} clouds.", gGameState.IsDayTime() ? " light grey" : ""),
							string.Format("The rain falls lightly{0}, covering the ground with small puddles and coating any tombstones or monuments present.", gGameState.IsDayTime() ? " from slate-grey clouds" : ""),
							"A driving rain falls from ominous black clouds; you try to keep dry but quickly realize the effort is futile.  Large puddles form, and streams of water cut through the path, seeking lower ground.  Your footing is treacherous.",
							"The fury of nature is on full display as you blindly stumble through a deluge.  Large sections of ground turn into pools, and the path disappears altogether.  You are soaked, chilled to the bone, feeling quite miserable, and must carefully navigate to avoid being swept away."
						};

						gOut.Print("{0}", rainDescs[gActorRoom(this).GetWeatherIntensity()]);

						break;

					case 62:

						var fogDescs = new string[]
						{
							"",
							"A light mist hangs in the air, gently swirling as you move along the path, but your vision is otherwise unimpaired.",
							"A fog hangs over the terrain and limits your vision to several dozen paces; familiar landmarks become dimly-lit silhouettes.",
							"As you peer into the dense fog, you realize your vision is restricted to a dozen paces.  The imposing shapes of monuments and trees lurk in the gloom, just beyond your perception; a foreboding silence is broken occasionally by the cry of an unseen bird or rustle of leaves nearby.",
							"The world is lost as you find yourself enveloped in a silent white cocoon, the fog so dense you can barely see your hand, held up at arms' length in front of your face.  Your eyes only occasionally leave the ground as you stumble across monuments or trees, hulking shadows that suddenly appear in the miasma, seemingly from nowhere."
						};

						gOut.Print("{0}", fogDescs[gActorRoom(this).GetWeatherIntensity()]);

						break;
				}

			Cleanup:

				if (NextState == null)
				{
					NextState = Globals.CreateInstance<IMonsterStartState>();
				}
			}
			else
			{
				base.Execute();
			}
		}

		public ExamineCommand()
		{
			CheckContainerTypeInDobjArtName = false;
		}
	}
}
