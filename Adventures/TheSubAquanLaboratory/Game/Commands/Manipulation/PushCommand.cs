
// PushCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static TheSubAquanLaboratory.Game.Plugin.Globals;

namespace TheSubAquanLaboratory.Game.Commands
{
	[ClassMappings]
	public class PushCommand : EamonRT.Game.Commands.Command, Framework.Commands.IPushCommand
	{
		public override void ExecuteForPlayer()
		{
			Debug.Assert(DobjArtifact != null);

			switch (DobjArtifact.Uid)
			{
				case 3:

					// Elevator up button

					if (gGameState.GetNBTL(Friendliness.Enemy) <= 0)
					{
						if (ActorRoom.Uid != 17)
						{
							gOut.Print("Up button pushed.");

							var newRoom = gRDB[17];

							Debug.Assert(newRoom != null);

							var effect = gEDB[2];

							Debug.Assert(effect != null);

							gEngine.TransportPlayerBetweenRooms(ActorRoom, newRoom, effect);

							NextState = gEngine.CreateInstance<IAfterPlayerMoveState>();
						}
						else
						{
							gEngine.PrintEffectDesc(16);
						}
					}
					else
					{
						PrintEnemiesNearby();

						NextState = gEngine.CreateInstance<IStartState>();
					}

					goto Cleanup;

				case 4:

					// Elevator down button

					if (gGameState.GetNBTL(Friendliness.Enemy) <= 0)
					{
						if (ActorRoom.Uid != 18)
						{
							gOut.Print("Down button pushed.");

							var newRoom = gRDB[18];

							Debug.Assert(newRoom != null);

							var effect = gEDB[3];

							Debug.Assert(effect != null);

							gEngine.TransportPlayerBetweenRooms(ActorRoom, newRoom, effect);

							NextState = gEngine.CreateInstance<IAfterPlayerMoveState>();
						}
						else
						{
							gEngine.PrintEffectDesc(17);
						}
					}
					else
					{
						PrintEnemiesNearby();

						NextState = gEngine.CreateInstance<IStartState>();
					}

					goto Cleanup;

				case 19:
				case 20:
				case 21:

					// Black/Yellow/Red buttons (Make mystery food)

					gOut.Print("{0} pushed.", DobjArtifact.GetNoneName(true, false));

					var foodArtifact = gADB[22];

					Debug.Assert(foodArtifact != null);

					if (!foodArtifact.IsInLimbo() || ++gGameState.FoodButtonPushes < 3)
					{
						gEngine.PrintEffectDesc(26);

						goto Cleanup;
					}

					gGameState.FoodButtonPushes = 0;

					gEngine.PrintEffectDesc(27);

					foodArtifact.SetInRoom(ActorRoom);

					foodArtifact.Seen = false;

					foodArtifact.GetCategory(0).Field2 = 5;

					var gruel = new string[] { "",
						"piping hot", "warm", "tepid", "cool", "ice cold", "frozen",
						"red", "yellow", "blue", "purple", "orange", "green",
						"watery", "smooth and thick", "chunky, soup-like", "solid", "flaky", "granular",
						"pours", "pours", "plops", "thumps", "pours", "pours"
					};

					var d = new long[4];

					var rc = gEngine.RollDice(d.Length, 6, ref d);

					Debug.Assert(gEngine.IsSuccess(rc));

					if (d[1] > 5 && d[2] < 4)
					{
						d[2] += 3;
					}

					gOut.Print("{0} {1} {2} {3} substance {4} into the deposit area at the bottom of the machine.", 
						d[1] == 5 ? "An" : "A",
						gruel[d[1]],
						gruel[d[3] + 6],
						gruel[d[2] + 12],
						gruel[d[2] + 18]);

					NextState = gEngine.CreateInstance<IStartState>();

					goto Cleanup;

				case 27:
				case 46:
				case 55:
				case 56:
				case 59:
				case 60:
				case 66:
				case 67:
				case 68:
				case 69:
				case 70:

					gOut.Print("{0} pushed.", DobjArtifact.GetNoneName(true, false));

					switch (DobjArtifact.Uid)
					{
						case 27:

							// Sterilize

							var glassWallsArtifact = gADB[84];

							Debug.Assert(glassWallsArtifact != null);

							if (!glassWallsArtifact.IsInLimbo() && !gGameState.Sterilize)
							{
								gEngine.PrintEffectDesc(6);

								gGameState.Sterilize = true;
							}

							goto Cleanup;

						case 46:

							// Nada

							gEngine.PrintEffectDesc(7);

							goto Cleanup;

						case 55:

							// Flood in

							if (gGameState.Flood != 1)
							{
								gEngine.PrintEffectDesc(8);

								gGameState.Flood = 1;
							}

							goto Cleanup;

						case 56:

							// Flood out

							if (gGameState.Flood == 1 /* && gGameState.FloodLevel > 1 */)
							{
								gEngine.PrintEffectDesc(9);

								gGameState.Flood = 2;
							}

							goto Cleanup;

						case 66:

							// Turret up

							if (gGameState.Elevation < 4)
							{
								gEngine.PrintEffectDesc(10);

								gGameState.Elevation++;
							}
							else
							{
								gEngine.PrintEffectDesc(28);
							}

							goto Cleanup;

						case 67:

							// Turret down

							if (gGameState.Elevation > 0)
							{
								gEngine.PrintEffectDesc(11);

								gGameState.Elevation--;
							}
							else
							{
								gEngine.PrintEffectDesc(29);
							}

							goto Cleanup;

						case 68:

							// Turret rotate

							gEngine.PrintEffectDesc(12);

							goto Cleanup;

						case 69:

							// Energize

							if (!gGameState.Energize)
							{
								gEngine.PrintEffectDesc(13);

								gGameState.Energize = true;
							}
							else
							{
								gEngine.PrintEffectDesc(30);
							}

							goto Cleanup;

						case 70:

							// Blue laser

							if (gGameState.Energize)
							{
								gEngine.PrintEffectDesc(14);

								gGameState.Energize = false;
							}
							else
							{
								gEngine.PrintEffectDesc(30);
							}

							goto Cleanup;

						default:

							goto Cleanup;
					}

				default:

					PrintCantVerbObj(DobjArtifact);

					NextState = gEngine.CreateInstance<IStartState>();

					goto Cleanup;
			}

		Cleanup:

			if (NextState == null)
			{
				NextState = gEngine.CreateInstance<IMonsterStartState>();
			}
		}

		public PushCommand()
		{
			SortOrder = 440;

			IsNew = true;

			Name = "PushCommand";

			Verb = "push";

			Type = CommandType.Manipulation;
		}
	}
}
