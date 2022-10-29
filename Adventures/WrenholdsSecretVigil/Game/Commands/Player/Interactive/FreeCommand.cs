
// FreeCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Primitive.Enums;
using static WrenholdsSecretVigil.Game.Plugin.Globals;

namespace WrenholdsSecretVigil.Game.Commands
{
	[ClassMappings]
	public class FreeCommand : EamonRT.Game.Commands.FreeCommand, IFreeCommand
	{
		public override void ProcessEvents(EventType eventType)
		{
			base.ProcessEvents(eventType);

			var deviceArtifact = gADB[44];

			Debug.Assert(deviceArtifact != null);

			// Free caged animals

			if (eventType == EventType.BeforeGuardMonsterCheck && DobjArtifact.Uid == 46 && (deviceArtifact.IsInRoom(ActorRoom) || deviceArtifact.IsEmbeddedInRoom(ActorRoom)))
			{
				gOut.Print("The glowing cages won't open!");

				GotoCleanup = true;
			}
		}
	}
}
