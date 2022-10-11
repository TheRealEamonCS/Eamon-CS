
// StartState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.States
{
	[ClassMappings]
	public class StartState : State, IStartState
	{
		public override void Execute()
		{
			ProcessEvents(EventType.BeforeStartRound);

			if (Globals.CommandPromptSeen && !Globals.ShouldPreTurnProcess)
			{
				goto Cleanup;
			}

			gGameState.CurrTurn++;

			Debug.Assert(gGameState.CurrTurn > 0);

		Cleanup:

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IBurnDownLightSourceState>();
			}

			Globals.NextState = NextState;
		}

		public StartState()
		{
			Name = "StartState";
		}
	}
}
