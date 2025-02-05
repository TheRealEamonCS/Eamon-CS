
// PlayerMoveCheckState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Args;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static ThePyramidOfAnharos.Game.Plugin.Globals;

namespace ThePyramidOfAnharos.Game.States
{
	[ClassMappings]
	public class PlayerMoveCheckState : EamonRT.Game.States.PlayerMoveCheckState, IPlayerMoveCheckState
	{
		public override void ProcessEvents(EventType eventType)
		{
			base.ProcessEvents(eventType);

			Debug.Assert(gCharMonster != null);

			var room = gCharMonster.GetInRoom();

			Debug.Assert(room != null);

			if (eventType == EventType.BeforeCanMoveToRoomCheck)
			{
				var omarMonster = gMDB[1];

				Debug.Assert(omarMonster != null);

				var aliMonster = gMDB[2];

				Debug.Assert(aliMonster != null);

				var amuletArtifact = gADB[15];

				Debug.Assert(amuletArtifact != null);

				var tunicArtifact = gADB[45];

				Debug.Assert(tunicArtifact != null);

				var gotoCleanup = false;

				// Wall of flames

				if ((gGameState.Ro == 27 && gGameState.R2 == 28) || (gGameState.Ro == 28 && gGameState.R2 == 27))
				{
					if (!tunicArtifact.IsWornByMonster(gCharMonster))
					{
						var injureAndDamageArgs = gEngine.CreateInstance<IInjureAndDamageArgs>(x =>
						{
							x.Room = room;

							x.EffectUid = 19;

							x.DeadBodyRoomUid = gGameState.R2;

							x.EquipmentDamageAmount = 1;

							x.InjuryMultiplier = 0.1;

							x.SetNextStateFunc = s => NextState = s;
						});

						gEngine.InjurePartyAndDamageEquipment(injureAndDamageArgs, ref gotoCleanup);

						if (gotoCleanup)
						{
							GotoCleanup = true;

							goto Cleanup;
						}
					}
					else
					{
						gEngine.PrintEffectDesc(17);
					}
				}

				// Dark cloud

				else if ((gGameState.Ro == 28 && gGameState.R2 == 29) || (gGameState.Ro == 29 && gGameState.R2 == 28))
				{
					if (!amuletArtifact.IsWornByMonster(gCharMonster))
					{
						var injureAndDamageArgs = gEngine.CreateInstance<IInjureAndDamageArgs>(x =>
						{
							x.Room = room;

							x.EffectUid = 49;

							x.DeadBodyRoomUid = gGameState.R2;

							x.EquipmentDamageAmount = 1;

							x.InjuryMultiplier = 0.1;

							x.SetNextStateFunc = s => NextState = s;
						});
					
						gEngine.InjurePartyAndDamageEquipment(injureAndDamageArgs, ref gotoCleanup);

						if (gotoCleanup)
						{
							GotoCleanup = true;

							goto Cleanup;
						}
					}
					else
					{
						gEngine.PrintEffectDesc(50);
					}
				}

				// Wander into desert

				else if (gGameState.R2 < -89 && gGameState.R2 > -95)
				{
					gGameState.KL = Math.Abs(gGameState.R2) - 89;

					gGameState.R2 = 67;
				}

				// Lost in desert

				else if (gGameState.R2 == -95)
				{
					var km = 25 + (omarMonster.IsInRoom(room) && omarMonster.Reaction == Friendliness.Friend ? 50 : aliMonster.IsInRoom(room) && aliMonster.Reaction == Friendliness.Friend ? 25 : 0);

					var rl = gEngine.RollDice(1, 100, 0);        // TODO: should modifier be -1 ???

					if (km - rl >= 0)
					{
						switch (gGameState.KL)
						{
							case 1:

								gGameState.R2 = gEngine.RollDice(1, 4, 0);

								break;

							case 2:

								gGameState.R2 = gEngine.RollDice(1, 8, 5);

								break;

							case 3:

								gGameState.R2 = gEngine.RollDice(1, 3, 43);

								break;

							case 4:

								gGameState.R2 = gEngine.RollDice(1, 3, 49);

								break;

							case 5:

								gGameState.R2 = gEngine.RollDice(1, 3, 63);

								break;

							default:

								Debug.Assert(1 == 0);

								break;
						}
					}
					else
					{
						gGameState.R2 = 67;
					}
				}
			}
			else if (eventType == EventType.AfterBlockingArtifactCheck)
			{
				// Falling stone death trap

				if (gGameState.R2 == -98)
				{
					if (room.IsViewable())
					{
						gEngine.PrintEffectDesc(1);
					}
					else
					{
						gOut.Print("As you move east, you feel the floor sink slightly under your feet. A noise above directs your gaze upward; moments later, something heavy crushes you. Splat! A most effective way of discouraging grave robbers or other snoops.");
					}

					gGameState.Die = 1;

					NextState = gEngine.CreateInstance<IPlayerDeadState>(x =>
					{
						x.PrintLineSep = true;
					});

					GotoCleanup = true;

					goto Cleanup;
				}

				// Abyss death trap

				else if (gGameState.R2 == -97)
				{
					for (var i = 7; i <= 8; i++)
					{
						gEngine.PrintEffectDesc(i);
					}

					gGameState.Die = 1;

					NextState = gEngine.CreateInstance<IPlayerDeadState>(x =>
					{
						x.PrintLineSep = true;
					});

					GotoCleanup = true;

					goto Cleanup;
				}
			}

		Cleanup:

			;
		}
	}
}
