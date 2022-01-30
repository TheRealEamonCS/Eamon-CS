
// CloseCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Primitive.Enums;
using static TheDeepCanyon.Game.Plugin.PluginContext;

namespace TheDeepCanyon.Game.Commands
{
	[ClassMappings]
	public class CloseCommand : EamonRT.Game.Commands.CloseCommand, ICloseCommand
	{
		public override void ProcessEvents(EventType eventType)
		{
			base.ProcessEvents(eventType);

			// If the trap is closed deactivate it

			if (eventType == EventType.AfterCloseArtifact && DobjArtifact.Uid == 17 && gGameState.TrapSet)
			{
				gOut.Print("The trap is no longer set.");

				gGameState.TrapSet = false;
			}
		}
	}
}
