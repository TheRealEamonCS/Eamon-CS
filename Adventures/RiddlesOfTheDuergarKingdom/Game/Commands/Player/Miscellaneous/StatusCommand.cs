
// StatusCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Primitive.Enums;
using static RiddlesOfTheDuergarKingdom.Game.Plugin.Globals;

namespace RiddlesOfTheDuergarKingdom.Game.Commands
{
	[ClassMappings]
	public class StatusCommand : EamonRT.Game.Commands.StatusCommand, IStatusCommand
	{
		public override void ProcessEvents(EventType eventType)
		{
			base.ProcessEvents(eventType);

			if (eventType == EventType.AfterPrintPlayerStatus)
			{
				if (gGameState.PoisonedTargets.ContainsKey(gGameState.Cm))
				{
					gOut.Print("You are poisoned at this time!");
				}
			}
		}
	}
}
