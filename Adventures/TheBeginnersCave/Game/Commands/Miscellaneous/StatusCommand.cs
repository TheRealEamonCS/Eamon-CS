
// StatusCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Primitive.Enums;
using static TheBeginnersCave.Game.Plugin.Globals;

namespace TheBeginnersCave.Game.Commands
{
	[ClassMappings]
	public class StatusCommand : EamonRT.Game.Commands.StatusCommand, IStatusCommand
	{
		public override void ProcessEvents(EventType eventType)
		{
			base.ProcessEvents(eventType);

			if (eventType == EventType.AfterPrintPlayerStatus)
			{
				var trollsfireArtifact = gADB[10];

				Debug.Assert(trollsfireArtifact != null);

				if (trollsfireArtifact.IsCarriedByMonster(ActorMonster) && gGameState.Trollsfire == 1)
				{
					gOut.Print("Trollsfire is alight!");
				}
			}
		}
	}
}
