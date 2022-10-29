
// MonsterStartState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.Globals;

namespace EamonRT.Game.States
{
	[ClassMappings]
	public class MonsterStartState : State, IMonsterStartState
	{
		public override void Execute()
		{
			if (NextState == null)
			{
				NextState = gEngine.CreateInstance<IMonsterLoopInitializeState>();
			}

			gEngine.NextState = NextState;
		}

		public MonsterStartState()
		{
			Name = "MonsterStartState";
		}
	}
}
