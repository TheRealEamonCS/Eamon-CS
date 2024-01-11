
// MonsterLoopIncrementState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework;
using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.Globals;

namespace EamonRT.Game.States
{
	[ClassMappings]
	public class MonsterLoopIncrementState : State, IMonsterLoopIncrementState
	{
		/// <summary></summary>
		public virtual IMonster FailedMoveMonster { get; set; }

		/// <summary></summary>
		public virtual IMonster LoopMonster { get; set; }

		public override void Execute()
		{
			if (gEngine.LoopMonsterUid > 0)
			{
				FailedMoveMonster = gMDB[gEngine.LoopMonsterUid];

				Debug.Assert(FailedMoveMonster != null);

				Debug.Assert(gEngine.LoopFailedMoveMemberCount >= 0);

				FailedMoveMonster.CurrGroupCount += gEngine.LoopFailedMoveMemberCount;

				gEngine.LoopFailedMoveMemberCount = 0;
			}

			while (true)
			{
				gEngine.LoopMonsterUid = gEngine.LoopMonsterUidListIndex < gEngine.LoopMonsterUidList.Count ? gEngine.LoopMonsterUidList[(int)(gEngine.LoopMonsterUidListIndex++)] : gDatabase.GetMonsterUid(false) + 1;

				LoopMonster = gMDB[gEngine.LoopMonsterUid];

				if (LoopMonster != null)
				{
					if (LoopMonster.ShouldProcessInGameLoop())
					{
						NextState = gEngine.CreateInstance<IMonsterActionState>();

						goto Cleanup;
					}
				}
				else
				{
					goto Cleanup;
				}
			}

		Cleanup:

			if (NextState == null)
			{
				NextState = gEngine.CreateInstance<IEndOfRoundState>();
			}

			gEngine.NextState = NextState;
		}

		public MonsterLoopIncrementState()
		{
			Name = "MonsterLoopIncrementState";
		}
	}
}
