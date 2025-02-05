
// AfterPlayerMoveState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static OrbOfMyLife.Game.Plugin.Globals;

namespace OrbOfMyLife.Game.States
{
	[ClassMappings]
	public class AfterPlayerMoveState : EamonRT.Game.States.AfterPlayerMoveState, IAfterPlayerMoveState
	{
		public override void ProcessEvents(EventType eventType)
		{
			base.ProcessEvents(eventType);

			if (eventType == EventType.AfterMoveMonsters)
			{
				var room = gRDB[gGameState.Ro];

				Debug.Assert(room != null);

				// Worst nightmare (using EDX logic)

				if (gGameState.Ro == 57 && gGameState.R2 == 57 && !gEngine.RestoreGame)
				{
					var roomUid = room.GetDir(1);

					for (var i = 1; i < 10; i++)
					{
						room.SetDir(i, room.GetDir(i + 1));
					}
					
					room.SetDir(10, roomUid);
				}

				// Lost in forest

				if (gGameState.Ro == 3 && !gEngine.RestoreGame)
				{
					var monster = gMDB[17];

					Debug.Assert(monster != null);

					var deadBodyArtifact = gADB[monster.DeadBody];

					Debug.Assert(deadBodyArtifact != null);

					deadBodyArtifact.SetInLimbo();

					gEngine.BuildRandomRoomExits(room);

					var rl = gEngine.RollDice(1, 100, 0);

					if (rl >= 90 && monster.IsInLimbo())
					{
						gEngine.BuildRandomMonster(room, monster, deadBodyArtifact);
					}
				}
			}
			else if (eventType == EventType.AfterExtinguishLightSourceCheck)
			{
				var room33 = gRDB[33];

				Debug.Assert(room33 != null);

				var gateOfLightArtifact = gADB[13];

				Debug.Assert(gateOfLightArtifact != null);

				// Exit Royal Wizard's chamber

				if (gGameState.Ro == 29 && gGameState.R3 == 1)
				{
					gEngine.PrintEffectDesc(23);
				}

				// Pass through Gate of Light

				if (gGameState.Ro == 36 && gGameState.R3 == 33)
				{
					room33.SetDir(Direction.Northeast, 36);

					gateOfLightArtifact.SetInLimbo();
				}
			}
		}

		public override void Execute()
		{
			base.Execute();

			gEngine.RestoreGame = false;
		}
	}
}
