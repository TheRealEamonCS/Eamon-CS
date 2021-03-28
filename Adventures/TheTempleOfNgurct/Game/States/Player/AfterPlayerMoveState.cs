
// AfterPlayerMoveState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static TheTempleOfNgurct.Game.Plugin.PluginContext;

namespace TheTempleOfNgurct.Game.States
{
	[ClassMappings]
	public class AfterPlayerMoveState : EamonRT.Game.States.AfterPlayerMoveState, IAfterPlayerMoveState
	{
		public override void ProcessEvents(EventType eventType)
		{
			if (eventType == EventType.AfterExtinguishLightSourceCheck)
			{
				var rl = gEngine.RollDice(1, 100, 0);

				// Spear trap

				if (gGameState.Ro == 5 && rl < 20)
				{
					gEngine.PrintEffectDesc(19);

					var monsterList = gEngine.GetTrapMonsterList(1, gGameState.Ro);

					foreach (var m in monsterList)
					{
						gEngine.ApplyTrapDamage(s => NextState = s, m, 1, 6, false);

						if (gGameState.Die == 1)
						{
							GotoCleanup = true;

							goto Cleanup;
						}
					}
				}

				// Loose rocks

				if (gGameState.Ro == 11 && rl < 33)
				{
					gEngine.PrintEffectDesc(20);

					var monsterList = gEngine.GetTrapMonsterList(1, gGameState.Ro);

					foreach (var m in monsterList)
					{
						gEngine.ApplyTrapDamage(s => NextState = s, m, 1, 4, false);

						if (gGameState.Die == 1)
						{
							GotoCleanup = true;

							goto Cleanup;
						}
					}
				}

				// Gas trap

				if (gGameState.Ro == 16 && rl < 15)
				{
					gEngine.PrintEffectDesc(21);

					var monsterList = gEngine.GetTrapMonsterList(3, gGameState.Ro);

					foreach (var m in monsterList)
					{
						gEngine.ApplyTrapDamage(s => NextState = s, m, 2, 6, true);

						if (gGameState.Die == 1)
						{
							GotoCleanup = true;

							goto Cleanup;
						}
					}
				}

				// Crossbow trap

				if (gGameState.Ro == 32 && rl < 51)
				{
					gEngine.PrintEffectDesc(22);

					var monsterList = gEngine.GetTrapMonsterList(1, gGameState.Ro);

					foreach (var m in monsterList)
					{
						gEngine.ApplyTrapDamage(s => NextState = s, m, 1, 8, false);

						if (gGameState.Die == 1)
						{
							GotoCleanup = true;

							goto Cleanup;
						}
					}
				}

				// Scything blade trap

				if (gGameState.Ro == 57 && rl < 20)
				{
					gEngine.PrintEffectDesc(23);

					var monsterList = gEngine.GetTrapMonsterList(1, gGameState.Ro);

					foreach (var m in monsterList)
					{
						gEngine.ApplyTrapDamage(s => NextState = s, m, 2, 6, false);

						if (gGameState.Die == 1)
						{
							GotoCleanup = true;

							goto Cleanup;
						}
					}
				}

				// Reveal secret doors

				var room1 = gRDB[24];

				Debug.Assert(room1 != null);

				var secretDoorArtifact1 = gADB[83];

				Debug.Assert(secretDoorArtifact1 != null);

				if (gGameState.Ro == 24 && gGameState.R3 == 41 && secretDoorArtifact1.IsInLimbo())
				{
					secretDoorArtifact1.SetInRoomUid(24);

					secretDoorArtifact1.DoorGate.SetOpen(true);

					room1.SetDirectionDoor(Direction.North, secretDoorArtifact1);
				}

				var room2 = gRDB[48];

				Debug.Assert(room2 != null);

				var secretDoorArtifact2 = gADB[84];

				Debug.Assert(secretDoorArtifact2 != null);

				if (gGameState.Ro == 48 && gGameState.R3 == 49 && secretDoorArtifact2.IsInLimbo())
				{
					secretDoorArtifact2.SetInRoomUid(48);

					secretDoorArtifact2.DoorGate.SetOpen(true);

					room2.SetDirectionDoor(Direction.South, secretDoorArtifact2);
				}
			}

			base.ProcessEvents(eventType);

		Cleanup:

			;
		}
	}
}
