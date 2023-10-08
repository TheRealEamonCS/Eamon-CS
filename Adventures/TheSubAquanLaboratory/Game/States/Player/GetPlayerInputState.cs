
// GetPlayerInputState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Components;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static TheSubAquanLaboratory.Game.Plugin.Globals;

namespace TheSubAquanLaboratory.Game.States
{
	[ClassMappings]
	public class GetPlayerInputState : EamonRT.Game.States.GetPlayerInputState, IGetPlayerInputState
	{
		public override void ProcessEvents(EventType eventType)
		{
			base.ProcessEvents(eventType);

			if (eventType == EventType.BeforePrintCommandPrompt && gEngine.ShouldPreTurnProcess)
			{
				var room = gRDB[gGameState.Ro];

				Debug.Assert(room != null);

				// Electrified floor

				var electrifiedFloorArtifact = gADB[85];

				Debug.Assert(electrifiedFloorArtifact != null);

				if (electrifiedFloorArtifact.IsInRoom(room))
				{
					gOut.Print("The electrified floor zaps everyone in the chamber!");

					var monsterList = gEngine.GetMonsterList(m => m.IsCharacterMonster(), m => m.IsInRoom(room) && !m.IsCharacterMonster());
					
					foreach (var monster in monsterList)
					{
						var combatComponent = gEngine.CreateInstance<ICombatComponent>(x =>
						{
							x.SetNextStateFunc = s => NextState = s;

							x.ActorRoom = room;

							x.Dobj = monster;

							x.OmitArmor = true;
						});

						combatComponent.ExecuteCalculateDamage(1, 4);

						if (gGameState.Die > 0)
						{
							GotoCleanup = true;

							goto Cleanup;
						}
					}
				}

				// Flood water

				if (room.Uid == 10 && gGameState.Flood > 0)
				{
					if (gGameState.Flood == 1)
					{
						if (gGameState.FloodLevel == 11)
						{
							var scubaGearArtifact = gADB[52];

							Debug.Assert(scubaGearArtifact != null);

							if (scubaGearArtifact.IsWornByMonster(gCharMonster))
							{
								gOut.Print("The chamber has entirely flooded!");

								var monsterList = gEngine.GetMonsterList(m => m.IsInRoom(room) && !m.IsCharacterMonster());

								foreach (var monster in monsterList)
								{
									monster.DmgTaken = monster.Hardiness;

									var combatComponent = gEngine.CreateInstance<ICombatComponent>(x =>
									{
										x.SetNextStateFunc = s => NextState = s;

										x.ActorRoom = room;

										x.Dobj = monster;
									});

									combatComponent.ExecuteCheckMonsterStatus();
								}
							}
							else
							{
								gEngine.PrintEffectDesc(33);

								gGameState.Die = 1;

								NextState = gEngine.CreateInstance<IPlayerDeadState>(x =>
								{
									x.PrintLineSep = true;
								});

								GotoCleanup = true;

								goto Cleanup;
							}
						}
						else
						{
							if (++gGameState.FloodLevel % 3 == 0)
							{
								gOut.Print("The water has risen to the {0} meter mark.", gGameState.FloodLevel / 3);
							}
							else
							{
								gOut.Print("The water continues to pour into the chamber.");
							}
						}
					}
					else if (gGameState.Flood == 2)
					{
						if (gGameState.FloodLevel % 3 == 0)
						{
							gOut.Print("The water has receded to the {0} meter mark.", gGameState.FloodLevel / 3);
						}
						else
						{
							gOut.Print("The water continues to drain from the chamber.");
						}

						if (--gGameState.FloodLevel < 0)
						{
							gGameState.Flood = 0;

							gGameState.FloodLevel = 0;
						}
					}
				}
			}

		Cleanup:

			;
		}
	}
}
