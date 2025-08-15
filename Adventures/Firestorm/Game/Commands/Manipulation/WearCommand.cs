
// WearCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Primitive.Enums;
using static Firestorm.Game.Plugin.Globals;

namespace Firestorm.Game.Commands
{
	[ClassMappings]
	public class WearCommand : EamonRT.Game.Commands.WearCommand, IWearCommand
	{
		public override void ProcessEvents(EventType eventType)
		{
			base.ProcessEvents(eventType);

			if (eventType == EventType.BeforeWearArtifact)
			{
				// Magic plate mail

				if (DobjArtifact.Uid == 24 && gGameState.Ar <= 0)
				{
					gOut.Print("{0} doesn't fit you.", DobjArtifact.GetTheName(true));

					GotoCleanup = true;
				}
			}
		}
	}
}
