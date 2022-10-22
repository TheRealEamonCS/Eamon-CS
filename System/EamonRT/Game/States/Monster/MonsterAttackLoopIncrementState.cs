
// MonsterAttackLoopIncrementState.cs

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
	public class MonsterAttackLoopIncrementState : State, IMonsterAttackLoopIncrementState
	{
		/// <summary></summary>
		public virtual IMonster LoopMonster { get; set; }

		/// <summary></summary>
		public virtual long MaxMemberAttackCount { get; set; }

		public override void Execute()
		{
			LoopMonster = gMDB[gEngine.LoopMonsterUid];

			Debug.Assert(LoopMonster != null);

			gEngine.LoopAttackNumber++;

			MaxMemberAttackCount = Math.Max(0, LoopMonster.GetMaxMemberAttackCount());

			if (LoopMonster.CurrGroupCount < gEngine.LoopGroupCount)
			{
				gEngine.LoopMemberNumber--;
			}

			if (LoopMonster.IsInLimbo() || LoopMonster.CurrGroupCount < gEngine.LoopGroupCount || LoopMonster.Weapon < 0 || gEngine.LoopAttackNumber > Math.Abs(LoopMonster.AttackCount) || gEngine.LoopAttackNumber > MaxMemberAttackCount)
			{
				NextState = gEngine.CreateInstance<IMonsterMemberLoopIncrementState>();

				goto Cleanup;
			}

		Cleanup:

			if (NextState == null)
			{
				NextState = gEngine.CreateInstance<IMonsterAttackActionState>();
			}

			gEngine.NextState = NextState;
		}

		public MonsterAttackLoopIncrementState()
		{
			Name = "MonsterAttackLoopIncrementState";
		}
	}
}
