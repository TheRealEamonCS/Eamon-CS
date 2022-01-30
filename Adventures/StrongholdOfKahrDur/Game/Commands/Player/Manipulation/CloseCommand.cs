
// CloseCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Primitive.Enums;
using static StrongholdOfKahrDur.Game.Plugin.PluginContext;

namespace StrongholdOfKahrDur.Game.Commands
{
	[ClassMappings]
	public class CloseCommand : EamonRT.Game.Commands.CloseCommand, ICloseCommand
	{
		public override void ProcessEvents(EventType eventType)
		{
			base.ProcessEvents(eventType);

			// If the armoire is closed then hide the secret passage

			if (eventType == EventType.AfterCloseArtifact && DobjArtifact.Uid == 3)
			{
				var secretDoorArtifact = gADB[4];

				Debug.Assert(secretDoorArtifact != null);

				var ac = secretDoorArtifact.DoorGate;

				Debug.Assert(ac != null);

				secretDoorArtifact.SetInLimbo();

				ac.SetOpen(false);

				ac.Field4 = 1;
			}
		}
	}
}
