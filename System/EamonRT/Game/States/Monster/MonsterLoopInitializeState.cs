
// MonsterLoopInitializeState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.Globals;

namespace EamonRT.Game.States
{
	[ClassMappings]
	public class MonsterLoopInitializeState : State, IMonsterLoopInitializeState
	{
		public override void Execute()
		{
			gEngine.LoopMonsterUidList = GetLoopMonsterUidList();

			gEngine.LoopMonsterUidListIndex = 0;

			gEngine.LoopMonsterUid = 0;

			if (NextState == null)
			{
				NextState = gEngine.CreateInstance<IMonsterLoopIncrementState>();
			}

			gEngine.NextState = NextState;
		}

		public MonsterLoopInitializeState()
		{
			Name = "MonsterLoopInitializeState";
		}
	}
}
