
// EndOfRoundState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Game.Attributes;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static WrenholdsSecretVigil.Game.Plugin.Globals;

namespace WrenholdsSecretVigil.Game.States
{
	[ClassMappings]
	public class EndOfRoundState : EamonRT.Game.States.EndOfRoundState, IEndOfRoundState
	{
		public override void ProcessEvents(EventType eventType)
		{
			base.ProcessEvents(eventType);

			// Try to open running device, all flee

			if (eventType == EventType.AfterEndRound && gEngine.DeviceOpened)
			{
				gOut.Print("Your attempts to open the glowing device are unsuccessful.");

				gEngine.DeviceOpened = false;
			}
		}
	}
}

