
// MonsterMemberLoopIncrementState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using Eamon.Framework;
using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.Globals;

namespace EamonRT.Game.States
{
	[ClassMappings]
	public class MonsterMemberLoopIncrementState : State, IMonsterMemberLoopIncrementState
	{
		/// <summary></summary>
		public virtual IMonster LoopMonster { get; set; }

		/// <summary></summary>
		public virtual long MaxMemberActionCount { get; set; }

		public override void Execute()
		{
			LoopMonster = gMDB[gEngine.LoopMonsterUid];

			Debug.Assert(LoopMonster != null);

			Debug.Assert(gEngine.LoopMemberNumber >= 0 && gEngine.LoopMemberNumber <= LoopMonster.CurrGroupCount);

			gEngine.LoopMemberNumber++;

			MaxMemberActionCount = Math.Max(0, LoopMonster.GetMaxMemberActionCount());

			if (LoopMonster.IsInLimbo() || gEngine.LoopMemberNumber > LoopMonster.CurrGroupCount || gEngine.LoopMemberNumber > MaxMemberActionCount)
			{
				NextState = gEngine.CreateInstance<IMonsterLoopIncrementState>();

				goto Cleanup;
			}

		Cleanup:

			if (NextState == null)
			{
				NextState = gEngine.CreateInstance<IMonsterMemberActionState>();
			}

			gEngine.NextState = NextState;
		}

		public MonsterMemberLoopIncrementState()
		{
			Name = "MonsterMemberLoopIncrementState";
		}
	}
}
