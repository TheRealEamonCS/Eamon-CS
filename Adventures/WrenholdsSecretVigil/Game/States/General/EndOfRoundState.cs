
// EndOfRoundState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Game.Attributes;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static WrenholdsSecretVigil.Game.Plugin.PluginContext;

namespace WrenholdsSecretVigil.Game.States
{
	[ClassMappings]
	public class EndOfRoundState : EamonRT.Game.States.EndOfRoundState, IEndOfRoundState
	{
		public override void ProcessEvents(EventType eventType)
		{
			// Try to open running device, all flee

			if (eventType == EventType.AfterRoundEnd && Globals.DeviceOpened)
			{
				gOut.Print("Your attempts to open the glowing device are unsuccessful.");

				Globals.DeviceOpened = false;
			}

			base.ProcessEvents(eventType);
		}
	}
}

