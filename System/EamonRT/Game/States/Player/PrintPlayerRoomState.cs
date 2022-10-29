
// PrintPlayerRoomState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Game.Attributes;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.Globals;

namespace EamonRT.Game.States
{
	[ClassMappings]
	public class PrintPlayerRoomState : State, IPrintPlayerRoomState
	{
		public override void Execute()
		{
			ProcessEvents(EventType.BeforePrintPlayerRoom);

			if (GotoCleanup)
			{
				goto Cleanup;
			}

			// If we've run out of player input print player Room

			if (gSentenceParser.IsInputExhausted)
			{
				gEngine.PrintPlayerRoom();
			}

		Cleanup:

			if (NextState == null)
			{
				NextState = gEngine.CreateInstance<IAfterPrintPlayerRoomEventState>();
			}

			gEngine.NextState = NextState;
		}

		public PrintPlayerRoomState()
		{
			Name = "PrintPlayerRoomState";
		}
	}
}
