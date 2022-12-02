
// PrintPlayerRoomState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework;
using Eamon.Game.Attributes;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.Globals;

namespace EamonRT.Game.States
{
	[ClassMappings]
	public class PrintPlayerRoomState : State, IPrintPlayerRoomState
	{
		public virtual IRoom CurrRoom { get; set; }

		public override void Execute()
		{
			Debug.Assert(gCharMonster != null);

			ProcessEvents(EventType.BeforePrintPlayerRoom);

			if (GotoCleanup)
			{
				goto Cleanup;
			}

			CurrRoom = gCharMonster.GetInRoom();

			Debug.Assert(CurrRoom != null);

			// If room is dark or we've run out of player input print player Room

			if ((!CurrRoom.IsLit() && (gEngine.LastCommand == null || !gEngine.LastCommand.IsDarkEnabled)) || gSentenceParser.IsInputExhausted)
			{
				gEngine.PrintPlayerRoom(CurrRoom);
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
