
// MonsterMemberLoopIncrementState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using Eamon.Framework;
using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

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
			LoopMonster = gMDB[Globals.LoopMonsterUid];

			Debug.Assert(LoopMonster != null);

			Debug.Assert(Globals.LoopMemberNumber >= 0 && Globals.LoopMemberNumber <= LoopMonster.CurrGroupCount);

			Globals.LoopMemberNumber++;

			MaxMemberActionCount = Math.Max(0, LoopMonster.GetMaxMemberActionCount());

			if (LoopMonster.IsInLimbo() || Globals.LoopMemberNumber > LoopMonster.CurrGroupCount || Globals.LoopMemberNumber > MaxMemberActionCount)
			{
				NextState = Globals.CreateInstance<IMonsterLoopIncrementState>();

				goto Cleanup;
			}

		Cleanup:

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IMonsterMemberActionState>();
			}

			Globals.NextState = NextState;
		}

		public MonsterMemberLoopIncrementState()
		{
			Name = "MonsterMemberLoopIncrementState";
		}
	}
}
