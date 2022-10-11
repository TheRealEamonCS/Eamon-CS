
// PrintPlayerRoomState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static BeginnersForest.Game.Plugin.PluginContext;

namespace BeginnersForest.Game.States
{
	[ClassMappings]
	public class PrintPlayerRoomState : EamonRT.Game.States.PrintPlayerRoomState, IPrintPlayerRoomState
	{
		public override void ProcessEvents(EventType eventType)
		{
			base.ProcessEvents(eventType);

			if (eventType == EventType.BeforePrintPlayerRoom && Globals.ShouldPreTurnProcess)
			{
				var spookMonster = gMDB[9];

				Debug.Assert(spookMonster != null);

				// Random Annoying Spooks (4 Spook limit)

				if (spookMonster.IsInLimbo() || spookMonster.IsInRoomUid(gGameState.Ro))
				{
					if (gGameState.Ro > 1 && gGameState.Ro < 5 && gGameState.SpookCounter < 10 && spookMonster.CurrGroupCount < 4)
					{
						var rl = gEngine.RollDice(1, 100, 0);

						if (rl < 35)
						{
							spookMonster.Seen = false;

							spookMonster.GroupCount++;

							spookMonster.InitGroupCount++;

							spookMonster.CurrGroupCount++;

							spookMonster.Location = gGameState.Ro;

							gGameState.SpookCounter++;
						}
					}
				}
			}
		}
	}
}
