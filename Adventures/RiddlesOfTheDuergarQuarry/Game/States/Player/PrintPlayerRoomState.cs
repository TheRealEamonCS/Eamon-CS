
// PrintPlayerRoomState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static RiddlesOfTheDuergarQuarry.Game.Plugin.PluginContext;

namespace RiddlesOfTheDuergarQuarry.Game.States
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

				var rl = gEngine.RollDice(1, 100, 0);

				var viperMonster = gMDB[18];

				Debug.Assert(viperMonster != null);

				// Viper doesn't like company

				if (viperMonster.IsInRoom(room) && viperMonster.Seen && viperMonster.Reaction > Friendliness.Enemy && rl <= 25)
				{
					viperMonster.Reaction--;

					gOut.Print("{0} is aggravated by your presence!", viperMonster.GetTheName(true));
				}
			}

			base.ProcessEvents(eventType);
		}
	}
}
