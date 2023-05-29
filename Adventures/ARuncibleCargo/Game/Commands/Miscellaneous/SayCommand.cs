
// SayCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static ARuncibleCargo.Game.Plugin.Globals;

namespace ARuncibleCargo.Game.Commands
{
	[ClassMappings]
	public class SayCommand : EamonRT.Game.Commands.SayCommand, ISayCommand
	{
		public override void ProcessEvents(EventType eventType)
		{
			base.ProcessEvents(eventType);

			if (eventType == EventType.BeforePrintSayText)
			{
				var found = false;

				// Fly FBA today and get there faster!

				if (ProcessedPhrase.Equals("*d", StringComparison.OrdinalIgnoreCase))
				{
					PrintedPhrase = "Dodge.";

					ProcessedPhrase = "dodge";

					found = true;
				}

				if (ProcessedPhrase.Equals("*f", StringComparison.OrdinalIgnoreCase))
				{
					PrintedPhrase = "Frukendorf.";

					ProcessedPhrase = "frukendorf";

					found = true;
				}

				if (ProcessedPhrase.Equals("*h", StringComparison.OrdinalIgnoreCase))
				{
					PrintedPhrase = "Main Hall.";

					ProcessedPhrase = "main hall";

					found = true;
				}

				if (ProcessedPhrase.Equals("*m", StringComparison.OrdinalIgnoreCase))
				{
					PrintedPhrase = "Mudville.";

					ProcessedPhrase = "mudville";

					found = true;
				}

				if (found && (gGameState.Ro == 28 || (gGameState.Ro > 88 && gGameState.Ro < 92)))
				{
					gOut.Print("Thank you for flying Frank Black Airlines!");
				}
			}
			else if (eventType == EventType.AfterPrintSayText)
			{
				var princeMonster = gMDB[38];

				Debug.Assert(princeMonster != null);

				var cargoArtifact = gADB[129];

				Debug.Assert(cargoArtifact != null);

				//                     Ye Olde Eamon Railroad
				//                    ------------------------

				// Verify Runcible Cargo before allowing travel to Frukendorf

				if ((ActorRoom.Uid == 28 || ActorRoom.Uid == 89 || ActorRoom.Uid == 90) && ProcessedPhrase.Equals("frukendorf", StringComparison.OrdinalIgnoreCase))
				{
					if (EnemiesInRoom())
					{
						goto Cleanup;
					}

					if (!cargoArtifact.IsInRoom(ActorRoom) && !cargoArtifact.IsCarriedByMonster(ActorMonster))
					{
						gEngine.PrintEffectDesc(107);

						NextState = gEngine.CreateInstance<IStartState>();

						GotoCleanup = true;

						goto Cleanup;
					}

					TravelByTrain(91, 109);

					goto Cleanup;
				}

				// Route 100: Main Hall Station

				if (ActorRoom.Uid == 28)
				{
					if (ProcessedPhrase.Equals("dodge", StringComparison.OrdinalIgnoreCase))
					{
						if (EnemiesInRoom())
						{
							goto Cleanup;
						}

						TravelByTrain(89, 99);

						goto Cleanup;
					}

					if (ProcessedPhrase.Equals("mudville", StringComparison.OrdinalIgnoreCase))
					{
						if (EnemiesInRoom())
						{
							goto Cleanup;
						}

						TravelByTrain(90, 103);

						goto Cleanup;
					}
				}

				// Route 13: Dodge Station

				if (ActorRoom.Uid == 89)
				{
					if (ProcessedPhrase.Equals("main hall", StringComparison.OrdinalIgnoreCase))
					{
						if (EnemiesInRoom())
						{
							goto Cleanup;
						}

						TravelByTrain(28, 100);

						goto Cleanup;
					}

					if (ProcessedPhrase.Equals("mudville", StringComparison.OrdinalIgnoreCase))
					{
						if (EnemiesInRoom())
						{
							goto Cleanup;
						}

						TravelByTrain(90, 101);

						goto Cleanup;
					}
				}

				// Route 0: Mudville Station

				if (ActorRoom.Uid == 90)
				{
					if (ProcessedPhrase.Equals("dodge", StringComparison.OrdinalIgnoreCase))
					{
						if (EnemiesInRoom())
						{
							goto Cleanup;
						}

						TravelByTrain(89, 102);

						goto Cleanup;
					}

					if (ProcessedPhrase.Equals("main hall", StringComparison.OrdinalIgnoreCase))
					{
						if (EnemiesInRoom())
						{
							goto Cleanup;
						}

						TravelByTrain(28, 104);

						goto Cleanup;
					}
				}

				// Route 66: Frukendorf Station

				if (ActorRoom.Uid == 91 && (ProcessedPhrase.Equals("main hall", StringComparison.OrdinalIgnoreCase) || ProcessedPhrase.Equals("dodge", StringComparison.OrdinalIgnoreCase) || ProcessedPhrase.Equals("mudville", StringComparison.OrdinalIgnoreCase)))
				{
					if (!cargoArtifact.IsCarriedByMonster(princeMonster))
					{
						gEngine.PrintEffectDesc(106);

						NextState = gEngine.CreateInstance<IStartState>();

						GotoCleanup = true;

						goto Cleanup;
					}

					if (EnemiesInRoom())
					{
						goto Cleanup;
					}

					if (ProcessedPhrase.Equals("dodge", StringComparison.OrdinalIgnoreCase) || ProcessedPhrase.Equals("mudville", StringComparison.OrdinalIgnoreCase))
					{
						gEngine.PrintEffectDesc(141);

						NextState = gEngine.CreateInstance<IStartState>();

						GotoCleanup = true;

						goto Cleanup;
					}

					// Return to Main Hall after capitulating to the Bandits

					gOut.Print("You begin your journey home...");

					gOut.Print("{0}", gEngine.LineSep);

					gEngine.PrintEffectDesc(145);

					gEngine.In.KeyPress(gEngine.Buf);

					gGameState.Die = 0;

					gEngine.ExitType = ExitType.FinishAdventure;

					gEngine.MainLoop.ShouldShutdown = true;

					NextState = gEngine.CreateInstance<IStartState>();

					GotoCleanup = true;

					goto Cleanup;
				}
			}

		Cleanup:

			;
		}

		public virtual bool EnemiesInRoom()
		{
			var result = false;

			if (gGameState.GetNBTL(Friendliness.Enemy) > 0)
			{
				PrintEnemiesNearby();

				NextState = gEngine.CreateInstance<IStartState>();

				GotoCleanup = true;

				result = true;
			}

			return result;
		}

		public virtual void TravelByTrain(long newRoomUid, long effectUid)
		{
			// Train Routine

			var newRoom = gRDB[newRoomUid];

			Debug.Assert(newRoom != null);

			var effect = gEDB[effectUid];

			Debug.Assert(effect != null);

			gEngine.TransportPlayerBetweenRooms(ActorRoom, newRoom, effect);

			NextState = gEngine.CreateInstance<IAfterPlayerMoveState>();

			GotoCleanup = true;
		}
	}
}
