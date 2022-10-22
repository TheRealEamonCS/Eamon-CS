
// MonsterAttackLoopInitializeState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.Globals;

namespace EamonRT.Game.States
{
	[ClassMappings]
	public class MonsterAttackLoopInitializeState : State, IMonsterAttackLoopInitializeState
	{
		/// <summary></summary>
		public virtual IMonster LoopMonster { get; set; }

		public override void Execute()
		{
			LoopMonster = gMDB[gEngine.LoopMonsterUid];

			Debug.Assert(LoopMonster != null && LoopMonster.CombatCode != CombatCode.NeverFights && LoopMonster.Reaction != Friendliness.Neutral);

			gEngine.LoopAttackNumber = 0;

			gEngine.LoopGroupCount = LoopMonster.CurrGroupCount;

			gEngine.LoopLastDobjMonster = null;

			if (NextState == null)
			{
				NextState = gEngine.CreateInstance<IMonsterAttackLoopIncrementState>();
			}

			gEngine.NextState = NextState;
		}

		public MonsterAttackLoopInitializeState()
		{
			Name = "MonsterAttackLoopInitializeState";
		}
	}
}
