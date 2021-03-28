
// MonsterAttackLoopIncrementState.cs

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
	public class MonsterAttackLoopIncrementState : State, IMonsterAttackLoopIncrementState
	{
		/// <summary></summary>
		public virtual IMonster LoopMonster { get; set; }

		/// <summary></summary>
		public virtual long MaxMemberAttackCount { get; set; }

		public override void Execute()
		{
			LoopMonster = gMDB[Globals.LoopMonsterUid];

			Debug.Assert(LoopMonster != null);

			Globals.LoopAttackNumber++;

			MaxMemberAttackCount = Math.Max(1, LoopMonster.GetMaxMemberAttackCount());

			if (LoopMonster.CurrGroupCount < Globals.LoopGroupCount)
			{
				Globals.LoopMemberNumber--;
			}

			if (LoopMonster.IsInLimbo() || LoopMonster.CurrGroupCount < Globals.LoopGroupCount || LoopMonster.Weapon < 0 || Globals.LoopAttackNumber > Math.Abs(LoopMonster.AttackCount) || Globals.LoopAttackNumber > MaxMemberAttackCount)
			{
				NextState = Globals.CreateInstance<IMonsterMemberLoopIncrementState>();

				goto Cleanup;
			}

		Cleanup:

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IMonsterAttackActionState>();
			}

			Globals.NextState = NextState;
		}

		public MonsterAttackLoopIncrementState()
		{
			Uid = 9;

			Name = "MonsterAttackLoopIncrementState";
		}
	}
}
