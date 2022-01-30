
// OpenCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Primitive.Enums;
using static StrongholdOfKahrDur.Game.Plugin.PluginContext;

namespace StrongholdOfKahrDur.Game.Commands
{
	[ClassMappings]
	public class OpenCommand : EamonRT.Game.Commands.OpenCommand, IOpenCommand
	{
		public override void ProcessEvents(EventType eventType)
		{
			base.ProcessEvents(eventType);

			var eyeglassesArtifact = gADB[2];

			Debug.Assert(eyeglassesArtifact != null);

			// If armoire opened and player is wearing eyeglasses reveal secret door

			if (eventType == EventType.AfterOpenArtifact && DobjArtifact.Uid == 3 && eyeglassesArtifact.IsWornByCharacter())
			{
				var secretDoorArtifact = gADB[4];

				Debug.Assert(secretDoorArtifact != null);

				var ac = secretDoorArtifact.DoorGate;

				Debug.Assert(ac != null);

				secretDoorArtifact.SetInRoom(ActorRoom);

				ac.SetOpen(true);

				ac.Field4 = 0;
			}
		}
	}
}
