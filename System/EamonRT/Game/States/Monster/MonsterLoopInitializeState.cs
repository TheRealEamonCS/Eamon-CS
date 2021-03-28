
// MonsterLoopInitializeState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.States
{
	[ClassMappings]
	public class MonsterLoopInitializeState : State, IMonsterLoopInitializeState
	{
		public override void Execute()
		{
			Globals.LoopMonsterUid = 0;

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IMonsterLoopIncrementState>();
			}

			Globals.NextState = NextState;
		}

		public MonsterLoopInitializeState()
		{
			Uid = 16;

			Name = "MonsterLoopInitializeState";
		}
	}
}
