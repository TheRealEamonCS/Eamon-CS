
// WearCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Primitive.Enums;
using static OrbOfMyLife.Game.Plugin.Globals;

namespace OrbOfMyLife.Game.Commands
{
	[ClassMappings]
	public class WearCommand : EamonRT.Game.Commands.WearCommand, IWearCommand
	{
		public override void ProcessEvents(EventType eventType)
		{
			base.ProcessEvents(eventType);

			if (eventType == EventType.AfterWearArtifact)
			{
				// Cloak of Darkness

				if (DobjArtifact.Uid == 11)
				{
					gOut.Print("You vanish, but feel weaker!");

					gGameState.VC = 0;

					// NOTE: original sets R3 = RO

					// NOTE: original checks monster reactions
				}

				// Cloak of Levitation

				else if (DobjArtifact.Uid == 17)
				{
					gOut.Print("You feel much lighter!");
				}
			}
		}
	}
}
