
// PrintPlayerRoomState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static TheDeepCanyon.Game.Plugin.PluginContext;

namespace TheDeepCanyon.Game.States
{
	[ClassMappings]
	public class PrintPlayerRoomState : EamonRT.Game.States.PrintPlayerRoomState, IPrintPlayerRoomState
	{
		public override void ProcessEvents(EventType eventType)
		{
			if (eventType == EventType.BeforePrintPlayerRoom && ShouldPreTurnProcess())
			{
				Debug.Assert(gCharMonster != null);

				var room = gCharMonster.GetInRoom();

				Debug.Assert(room != null);




				var ringArtifact = gADB[22];

				Debug.Assert(ringArtifact != null);

				// Squirrel gives ring

				if (room.Uid == 2 && room.Seen && !gGameState.SquirrelRing)
				{
					gEngine.PrintEffectDesc(2);

					ringArtifact.SetCarriedByCharacter();           // TODO: put in room if too heavy to carry

					gGameState.SquirrelRing = true;
				}

				var rl = gEngine.RollDice(1, 100, 0);

				// Blue Dragon appears

				if (room.Uid < 12 && rl >= 90 && !gGameState.BlueDragon)
				{
					gEngine.PrintEffectDesc(6);

					gGameState.BlueDragon = true;
				}

				var fidoMonster = gMDB[11];

				Debug.Assert(fidoMonster != null);

				// Fido sleeping

				if (gGameState.FidoSleepCounter > 0)
				{
					rl = gEngine.RollDice(1, 15, 0);

					gGameState.FidoSleepCounter -= rl;

					if (gGameState.FidoSleepCounter <= 0)
					{
						if (room.Uid == 26)
						{
							gOut.Print("{0} wakes up!", room.IsLit() ? "Fido" : "Something");
						}

						gGameState.FidoSleepCounter = 0;

						fidoMonster.StateDesc = "";

						fidoMonster.Reaction = Friendliness.Enemy;
					}
				}
			}

			base.ProcessEvents(eventType);
		}
	}
}
