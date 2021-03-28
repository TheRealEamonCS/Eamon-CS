
// MonsterStartState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.States
{
	[ClassMappings]
	public class MonsterStartState : State, IMonsterStartState
	{
		public override void Execute()
		{
			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IMonsterLoopInitializeState>();
			}

			Globals.NextState = NextState;
		}

		public MonsterStartState()
		{
			Uid = 17;

			Name = "MonsterStartState";
		}
	}
}
