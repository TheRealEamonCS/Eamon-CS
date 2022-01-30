
// MonsterAttackLoopInitializeState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.States
{
	[ClassMappings]
	public class MonsterAttackLoopInitializeState : State, IMonsterAttackLoopInitializeState
	{
		/// <summary></summary>
		public virtual IMonster LoopMonster { get; set; }

		public override void Execute()
		{
			LoopMonster = gMDB[Globals.LoopMonsterUid];

			Debug.Assert(LoopMonster != null && LoopMonster.CombatCode != CombatCode.NeverFights && LoopMonster.Reaction != Friendliness.Neutral);

			Globals.LoopAttackNumber = 0;

			Globals.LoopGroupCount = LoopMonster.CurrGroupCount;

			Globals.LoopLastDobjMonster = null;

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IMonsterAttackLoopIncrementState>();
			}

			Globals.NextState = NextState;
		}

		public MonsterAttackLoopInitializeState()
		{
			Uid = 10;

			Name = "MonsterAttackLoopInitializeState";
		}
	}
}
