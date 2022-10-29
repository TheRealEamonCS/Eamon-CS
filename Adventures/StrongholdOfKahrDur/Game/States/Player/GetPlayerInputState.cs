
// GetPlayerInputState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static StrongholdOfKahrDur.Game.Plugin.Globals;

namespace StrongholdOfKahrDur.Game.States
{
	[ClassMappings]
	public class GetPlayerInputState : EamonRT.Game.States.GetPlayerInputState, IGetPlayerInputState
	{
		public override void ProcessEvents(EventType eventType)
		{
			base.ProcessEvents(eventType);

			if (eventType == EventType.BeforePrintCommandPrompt && gEngine.ShouldPreTurnProcess)
			{
				var room = gRDB[84];

				Debug.Assert(room != null);

				var necromancerMonster = gMDB[22];

				Debug.Assert(necromancerMonster != null);

				// Flavor effects

				var rl = gEngine.RollDice(1, 100, 0);

				var r = 5 + 5 * (!room.Seen ? 1 : 0);

				if (rl < r && !necromancerMonster.IsInLimbo())
				{
					rl = gEngine.RollDice(1, 5, 64);

					gEngine.PrintEffectDesc(rl);
				}
			}
		}
	}
}
