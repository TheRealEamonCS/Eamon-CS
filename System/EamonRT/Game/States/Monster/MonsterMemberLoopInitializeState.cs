
// MonsterMemberLoopInitializeState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework;
using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.Globals;

namespace EamonRT.Game.States
{
	[ClassMappings]
	public class MonsterMemberLoopInitializeState : State, IMonsterMemberLoopInitializeState
	{
		/// <summary></summary>
		public virtual IMonster LoopMonster { get; set; }

		public override void Execute()
		{
			LoopMonster = gMDB[gEngine.LoopMonsterUid];

			Debug.Assert(LoopMonster != null);

			gEngine.LoopMemberNumber = 0;

			if (NextState == null)
			{
				NextState = gEngine.CreateInstance<IMonsterMemberLoopIncrementState>();
			}

			gEngine.NextState = NextState;
		}

		public MonsterMemberLoopInitializeState()
		{
			Name = "MonsterMemberLoopInitializeState";
		}
	}
}
