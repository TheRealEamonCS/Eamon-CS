
// PlayerMoveCheckState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using Eamon;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static StrongholdOfKahrDur.Game.Plugin.PluginContext;

namespace StrongholdOfKahrDur.Game.States
{
	[ClassMappings]
	public class PlayerMoveCheckState : EamonRT.Game.States.PlayerMoveCheckState, IPlayerMoveCheckState
	{
		public override void PrintRideOffIntoSunset()
		{
			gOut.Print("You ride off into the sunset.");
		}

		public override void ProcessEvents(EventType eventType)
		{
			RetCode rc;

			if (eventType == EventType.BeforeCanMoveToRoomCheck)
			{
				var amuletArtifact = gADB[18];

				Debug.Assert(amuletArtifact != null);

				// Cannot enter forest if not wearing magical amulet

				if (gGameState.Ro == 92 && gGameState.R2 == 65 && !amuletArtifact.IsWornByCharacter())
				{
					gEngine.PrintEffectDesc(45);

					GotoCleanup = true;

					goto Cleanup;
				}

				var bootsArtifact = gADB[14];

				Debug.Assert(bootsArtifact != null);

				if (gGameState.Ro == 84 && gGameState.R2 == 94)
				{
					// If descend pit w/ mgk boots, write effect

					if (bootsArtifact.IsWornByCharacter())
					{
						gEngine.PrintEffectDesc(47);
					}

					// If descend pit w/out mgk boots, fall to death

					else
					{
						gOut.Write("{0}It looks dangerous, try to climb down anyway (Y/N): ", Environment.NewLine);

						Globals.Buf.Clear();

						rc = Globals.In.ReadField(Globals.Buf, Constants.BufSize02, null, ' ', '\0', false, null, gEngine.ModifyCharToUpper, gEngine.IsCharYOrN, null);

						Debug.Assert(gEngine.IsSuccess(rc));

						if (Globals.Buf.Length > 0 && Globals.Buf[0] == 'Y')
						{
							gEngine.PrintEffectDesc(46);

							gGameState.Die = 1;

							NextState = Globals.CreateInstance<IPlayerDeadState>(x =>
							{
								x.PrintLineSep = true;
							});
						}

						GotoCleanup = true;
					}

					goto Cleanup;
				}

				if (gGameState.Ro == 94 && gGameState.R2 == 84)
				{
					// If ascend pit w/ mgk boots, write effect

					if (bootsArtifact.IsWornByCharacter())
					{
						gEngine.PrintEffectDesc(48);
					}

					// Cannot go up the pit if not wearing mgk boots

					else
					{
						gOut.Print("The ceiling is too high to climb back up!");

						GotoCleanup = true;
					}

					goto Cleanup;
				}
			}
			else if (eventType == EventType.AfterBlockingArtifactCheck)
			{
				if (gGameState.R2 == Constants.DirectionExit)
				{
					// Successful adventure means the necromancer (22) is dead and Lady Mirabelle (26) is alive and exiting with player

					var necromancerMonster = gMDB[22];

					Debug.Assert(necromancerMonster != null);

					var vanquished = necromancerMonster.IsInLimbo();

					var mirabelleMonster = gMDB[26];

					Debug.Assert(mirabelleMonster != null);

					var rescued = mirabelleMonster.Location == gGameState.Ro;

					if (!vanquished || !rescued)
					{
						gOut.Print("You have not succeeded in your quest!");

						if (!vanquished)
						{
							gOut.Print(" * The evil force here has not been vanquished");
						}

						if (!rescued)
						{
							gOut.Print(" * Lady Mirabelle has not been rescued");
						}
					}
					else
					{
						gOut.Print("YOU HAVE SUCCEEDED IN YOUR QUEST!  CONGRATULATIONS!");
					}

					gOut.Write("{0}Leave this adventure (Y/N): ", Environment.NewLine);

					Globals.Buf.Clear();

					rc = Globals.In.ReadField(Globals.Buf, Constants.BufSize02, null, ' ', '\0', false, null, gEngine.ModifyCharToUpper, gEngine.IsCharYOrN, null);

					Debug.Assert(gEngine.IsSuccess(rc));

					if (Globals.Buf.Length > 0 && Globals.Buf[0] == 'Y')
					{
						PrintRideOffIntoSunset();

						gGameState.Die = 0;

						Globals.ExitType = ExitType.FinishAdventure;

						Globals.MainLoop.ShouldShutdown = true;
					}

					GotoCleanup = true;
				}
				else
				{
					base.ProcessEvents(eventType);
				}
			}
			else
			{
				base.ProcessEvents(eventType);
			}

		Cleanup:

			;
		}
	}
}
