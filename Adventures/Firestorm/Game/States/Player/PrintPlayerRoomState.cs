
// PrintPlayerRoomState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using System.Linq;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static Firestorm.Game.Plugin.Globals;

namespace Firestorm.Game.States
{
	[ClassMappings]
	public class PrintPlayerRoomState : EamonRT.Game.States.PrintPlayerRoomState, IPrintPlayerRoomState
	{
		public override void ProcessEvents(EventType eventType)
		{
			base.ProcessEvents(eventType);

			Debug.Assert(gGameState != null);

			Debug.Assert(gCharMonster != null);

			Debug.Assert(gCharRoom != null);

			var thorakJuniorMonster = gMDB[39];

			Debug.Assert(thorakJuniorMonster != null);

			var gruntsMonster = gMDB[40];

			Debug.Assert(gruntsMonster != null);

			var reldakMonster = gMDB[44];

			Debug.Assert(reldakMonster != null);

			var bagOfGoldArtifact = gADB[15];

			Debug.Assert(bagOfGoldArtifact != null);

			var magicPlateMailArtifact = gADB[24];

			Debug.Assert(magicPlateMailArtifact != null);

			var uniformArtifact = gADB[33];

			Debug.Assert(uniformArtifact != null);

			var broadTippedArrowsArtifact = gADB[61];

			Debug.Assert(broadTippedArrowsArtifact != null);

			if (eventType == EventType.BeforePrintPlayerRoom)
			{
				if (gEngine.ShouldPreTurnProcess)
				{
					// Rec room

					if (gCharRoom.Uid == 82 && gGameState.GetPG(6) == 0)
					{
						gEngine.PrintEffectDesc(37);

						gGameState.SetPG(6, 1);
					}

					// Thorak vision #1 (Ghost town)

					if (gCharRoom.Uid == 34 && gGameState.GetPG(1) == 0)
					{
						for (var effectUid = 46; effectUid <= 48; effectUid++)
						{
							gEngine.PrintEffectDesc(effectUid);
						}

						gGameState.SetPG(1, 1);
					}

					// Bar

					if (gCharRoom.Uid == 42 && gGameState.GetPG(2) == 0)
					{
						for (var effectUid = 44; effectUid <= 45; effectUid++)
						{
							gEngine.PrintEffectDesc(effectUid);
						}

						gGameState.SetPG(2, 1);
					}

					// Thorak vision #2

					if (gCharRoom.Uid == 66 && gGameState.GetPG(3) == 0)
					{
						for (var effectUid = 51; effectUid <= 52; effectUid++)
						{
							gEngine.PrintEffectDesc(effectUid);
						}

						gGameState.SetPG(3, 1);
					}

					// Thorak vision #3

					if (gCharRoom.Uid == 81 && gGameState.GetPG(4) == 0)
					{
						gEngine.PrintEffectDesc(53);

						gGameState.SetPG(4, 1);
					}

					// Guard's quarters

					if (gCharRoom.Uid == 80 && broadTippedArrowsArtifact.IsEmbeddedInRoomUid(80) && gGameState.GetPG(7) == 0)
					{
						gOut.Print("The guards' gear is stacked neatly on the shelves.");

						gGameState.SetPG(7, 1);
					}

					// Steal bag of gold

					if (gCharRoom.Uid == 23 && !bagOfGoldArtifact.IsInRoomUid(49) && gGameState.GetPG(8) == 0)
					{
						gEngine.PrintEffectDesc(61);

						reldakMonster.SetInRoom(gCharRoom);

						gGameState.SetPG(8, 1);
					}

					// Random fortress guards

					if (gCharRoom.Uid > 50 && gCharRoom.Uid < 81 && gGameState.NF == 0)
					{
						var zz = gEngine.RollDice(1, 10, 0);

						if (zz > gEngine.GetGU(gCharRoom.Uid - 50))
						{
							var guardCount = gEngine.GetMonsterList(m => m.Uid >= 23 && m.Uid <= 26 && !m.IsInLimbo()).Count();

							if (guardCount < 4)
							{
								var rl = gEngine.RollDice(1, 100, 0);

								if (!uniformArtifact.IsWornByMonster(gCharMonster) || rl < 63)
								{
									var guardsArrive = false;

									var xx = gEngine.RollDice(1, 4, 0);

									var rr = new long[5];

									for (var rx = 1; rx <= xx; rx++)
									{
										rr[rx] = gEngine.RollDice(1, 8, 0);
									}

									for (var m = 1; m <= xx; m++)
									{
										var m2 = 22 + m;

										var guardMonster = gMDB[m2];

										Debug.Assert(guardMonster != null);

										if (guardMonster.IsInLimbo())
										{
											var effect = gEDB[gEngine.GetRN(rr[m])];

											Debug.Assert(effect != null);

											guardMonster.SetInRoom(gCharRoom);

											guardMonster.DmgTaken = 0;

											guardMonster.Weapon = 0;

											guardMonster.Seen = false;

											guardMonster.Name = gEngine.CloneInstance(gEngine.GetRM(rr[m]));

											guardMonster.Desc = gEngine.CloneInstance(effect.Desc);

											guardMonster.Gender = rr[m] == 6 ? Gender.Female : rr[m] == 1 || rr[m] == 2 ? Gender.Neutral : Gender.Male;

											guardMonster.ArticleType = rr[m] == 5 ? ArticleType.An : ArticleType.A;

											guardMonster.PluralType = rr[m] == 7 ? PluralType.YIes : PluralType.S;

											guardsArrive = true;
										}
									}

									if (guardsArrive)
									{
										gOut.Print("Guards on routine patrol {0} you and call for help!", gCharRoom.EvalLightLevel("bump into", "find"));
									}
								}
								else
								{
									gOut.Print("{0} some passing guards.", gCharRoom.EvalLightLevel("The darkness hides you from", "Your uniform fools"));
								}
							}
						}
						else
						{
							if (!uniformArtifact.IsWornByMonster(gCharMonster) && gGameState.GetPG(5) == 0)
							{
								gOut.Print("You are {0} as an intruder. Guards are on their way.", gCharRoom.EvalLightLevel("detected", "spotted"));

								gGameState.SetPG(5, 1);
							}

							if (thorakJuniorMonster.Location == -98)
							{
								thorakJuniorMonster.SetInRoomUid(82);

								gruntsMonster.SetInRoomUid(82);

								gruntsMonster.Weapon = 0;
							}
						}

						gGameState.NF = 1;
					}

					// Poisoned

					if (gGameState.PZ == 1)
					{
						gCharMonster.DmgTaken += 1;

						if (gCharMonster.IsDead())
						{
							gEngine.PrintEffectDesc(158);

							gGameState.Die = 1;

							NextState = gEngine.CreateInstance<IPlayerDeadState>(x =>
							{
								x.PrintLineSep = true;
							});

							GotoCleanup = true;

							goto Cleanup;
						}
					}
				}

				// Poisoned

				if (gGameState.PZ == 1 && gCharMonster.Hardiness - gCharMonster.DmgTaken < 4)
				{
					gOut.Print("*** You are very close to death.");
				}
			}
			else if (eventType == EventType.AfterPrintPlayerRoom)
			{
				if (gEngine.ShouldPreTurnProcess)
				{
					if (gCharRoom.Uid == 38 && !gGameState.MPEnabled)
					{
						gEngine.PrintEffectDesc(22);

						gGameState.MPEnabled = true;
					}

					if (gGameState.MPEnabled && gGameState.CurrTurn % 4 == 0)
					{
						gGameState.MP += 1;
					}
				}

				if (gCharRoom.Uid == 45 && magicPlateMailArtifact.Location == -98)
				{
					gOut.Print("There is a suit of armor hanging from a branch of a tree.");
				}
			}

		Cleanup:

			;
		}
	}
}
