
// ProcessPlayerInputState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Game.Attributes;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

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
				Globals.LastCommandList.Clear();

				ProcessEvents(EventType.AfterLastCommandListClear);

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
				NextState = Globals.CreateInstance<IUnrecognizedCommandState>();
			}

			Globals.NextState = NextState;
		}

		public ProcessPlayerInputState()
		{
			Uid = 33;

			Name = "ProcessPlayerInputState";
		}
	}
}
