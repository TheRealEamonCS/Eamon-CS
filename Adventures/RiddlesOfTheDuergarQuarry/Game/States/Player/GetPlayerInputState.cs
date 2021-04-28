
// GetPlayerInputState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static RiddlesOfTheDuergarQuarry.Game.Plugin.PluginContext;

namespace RiddlesOfTheDuergarQuarry.Game.States
{
	[ClassMappings]
	public class GetPlayerInputState : EamonRT.Game.States.GetPlayerInputState, IGetPlayerInputState
	{
		public override void ProcessEvents(EventType eventType)
		{
			if (eventType == EventType.BeforePrintCommandPrompt && ShouldPreTurnProcess())
			{
				Debug.Assert(gCharMonster != null);

				var room = gCharMonster.GetInRoom();

				Debug.Assert(room != null);


			}

			base.ProcessEvents(eventType);
		}
	}
}
