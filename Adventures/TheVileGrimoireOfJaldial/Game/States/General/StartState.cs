
// StartState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static TheVileGrimoireOfJaldial.Game.Plugin.PluginContext;

namespace TheVileGrimoireOfJaldial.Game.States
{
	[ClassMappings]
	public class StartState : EamonRT.Game.States.StartState, IStartState
	{
		public override void ProcessEvents(EventType eventType)
		{
			if (eventType == EventType.BeforeRoundStart)
			{
				if (ShouldPreTurnProcess())
				{

				}

				// See if bloodnettle and its victim are still in the same room

				if (gGameState.BloodnettleVictimUid != 0)
				{
					var bloodnettleMonster = gMDB[20];

					Debug.Assert(bloodnettleMonster != null);

					var victimMonster = gMDB[gGameState.BloodnettleVictimUid];

					Debug.Assert(victimMonster != null);

					if (bloodnettleMonster.IsInLimbo() || !bloodnettleMonster.IsInRoomUid(victimMonster.GetInRoomUid()))
					{
						gGameState.BloodnettleVictimUid = 0;
					}
				}
			}

			base.ProcessEvents(eventType);
		}
	}
}
