
// ReadCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using Eamon.Framework;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static TheSubAquanLaboratory.Game.Plugin.Globals;

namespace TheSubAquanLaboratory.Game.Commands
{
	[ClassMappings]
	public class ReadCommand : EamonRT.Game.Commands.ReadCommand, IReadCommand
	{
		public override void PrintCantVerbObj(IGameBase obj)
		{
			Debug.Assert(obj != null);

			gOut.Print("You stare at {0}, but you don't see any secret messages forming.", obj.GetTheName());
		}

		public override void ExecuteForPlayer()
		{
			var rl = 0L;

			Debug.Assert(DobjArtifact != null);

			switch (DobjArtifact.Uid)
			{
				case 9:

					// Bronze plaque

					if (!gGameState.ReadPlaque)
					{
						gGameState.QuestValue += 250;

						gGameState.ReadPlaque = true;
					}

					base.ExecuteForPlayer();

					break;

				case 48:

					// Display screen

					rl = gEngine.RollDice(1, 100, 0);

					if (rl < 34)
					{
						var ls = new string[]
						{
							"",
							"SINCLAIR INLET     ",
							"VLADIVOSTOK     ",
							"BAJA PENINSULA     ",
							"YUKATAN PENINSULA     ",
							"GOLD COAST     ",
							"UPPER RHINE VALLEY     ",
							"LHASA     ",
							"VANCOUVER     ",
							"TRIPOLI     ",
							"HANOI     "
						};

						var d = new long[]
						{
							0L,
							gEngine.RollDice(1, 9, 0),
							gEngine.RollDice(1, 99, 0),
							gEngine.RollDice(1, 30, 0),
							gEngine.RollDice(1, 100, 0),
							gEngine.RollDice(1, 2, 0),
							gEngine.RollDice(1, 2, -1) + 3,
							gEngine.RollDice(1, 90, 0),
							gEngine.RollDice(1, 180, 0),
							gEngine.RollDice(1, 59, 0),
							gEngine.RollDice(1, 59, 0),
							gEngine.RollDice(1, 59, 0),
							gEngine.RollDice(1, 59, 0),
							gEngine.RollDice(1, 10, 0),
							gEngine.RollDice(1, 20, 0),
							gEngine.RollDice(1, 100, 0)
						};

						var nsd = d[5] == 1 ? "North" : "South";

						var ewd = d[6] == 3 ? "East" : "West";

						gEngine.PrintEffectDesc(51);

						gOut.Print("{0}{1}.{2} GMT", ls[d[13]], d[14], d[15]);

						gOut.Write("{0}Magnitude.....{1}.{2}", Environment.NewLine, d[1], d[2]);

						gOut.Write("{0}Duration......{1}.{2} seconds", Environment.NewLine, d[3], d[4]);

						gOut.Write("{0}Epicenter.....{1} Latitude {2} degrees {3} minutes {4} seconds", Environment.NewLine, nsd, d[7], d[9], d[11]);

						gOut.Print("{0,14}{1} Longitude {2} degrees {3} minutes {4} seconds", "", ewd, d[8], d[10], d[12]);
						
						if (!gGameState.ReadDisplayScreen)
						{
							gGameState.QuestValue += 300;

							gGameState.ReadDisplayScreen = true;
						}
					}
					else
					{
						gOut.Print("The monitor screen remains blank.");
					}

					NextState = gEngine.CreateInstance<IMonsterStartState>();

					break;

				case 50:

					// Terminals

					rl = gEngine.RollDice(1, 100, 0);

					if (rl < 51)
					{
						gEngine.PrintEffectDesc(52);

						if (!gGameState.ReadTerminals)
						{
							gGameState.QuestValue += 350;

							gGameState.ReadTerminals = true;
						}
					}
					else
					{
						rl = gEngine.RollDice(1, 100, 0);

						gOut.Print("As you watch, the terminal screen prints:");

						gOut.Print("  Error #{0}", rl);

						gOut.Print("Uploading execution impossible - attempting to abort!");
					}

					NextState = gEngine.CreateInstance<IMonsterStartState>();

					break;

				default:

					base.ExecuteForPlayer();

					break;
			}
		}
	}
}
