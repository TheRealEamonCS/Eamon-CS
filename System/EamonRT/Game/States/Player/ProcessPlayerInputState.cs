﻿
// ProcessPlayerInputState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Game.Attributes;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.Globals;

namespace EamonRT.Game.States
{
	[ClassMappings]
	public class ProcessPlayerInputState : State, IProcessPlayerInputState
	{
		public override void Execute()
		{
			gCommandParser.Execute();

			if (gCommandParser.NextState == null || !(gCommandParser.NextState is IGetPlayerInputState gpis) || !gpis.RestartCommand)
			{
				gEngine.LastCommandList.Clear();

				ProcessEvents(EventType.AfterClearLastCommandList);

				if (GotoCleanup)
				{
					goto Cleanup;
				}
			}

			NextState = gCommandParser.NextState;

			gCommandParser.Clear();

		Cleanup:

			if (NextState == null)
			{
				NextState = gEngine.CreateInstance<IUnrecognizedCommandState>();
			}

			gEngine.NextState = NextState;
		}

		public ProcessPlayerInputState()
		{
			Name = "ProcessPlayerInputState";
		}
	}
}
