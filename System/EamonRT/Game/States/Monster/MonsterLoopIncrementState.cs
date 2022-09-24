
// MonsterLoopIncrementState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework;
using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

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
			if (Globals.LoopMonsterUid > 0)
			{
				FailedMoveMonster = gMDB[Globals.LoopMonsterUid];

				Debug.Assert(FailedMoveMonster != null);

				Debug.Assert(Globals.LoopFailedMoveMemberCount >= 0);

				FailedMoveMonster.CurrGroupCount += Globals.LoopFailedMoveMemberCount;

				Globals.LoopFailedMoveMemberCount = 0;
			}

			while (true)
			{
				Globals.LoopMonsterUid = Globals.LoopMonsterUidListIndex < Globals.LoopMonsterUidList.Count ? Globals.LoopMonsterUidList[(int)(Globals.LoopMonsterUidListIndex++)] : Globals.Database.GetMonsterUid(false) + 1;

				LoopMonster = gMDB[Globals.LoopMonsterUid];

				if (LoopMonster != null)
				{
					if (LoopMonster.ShouldProcessInGameLoop())
					{
						NextState = Globals.CreateInstance<IMonsterActionState>();

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
				NextState = Globals.CreateInstance<IEndOfRoundState>();
			}

			Globals.NextState = NextState;
		}

		public MonsterLoopIncrementState()
		{
			Name = "MonsterLoopIncrementState";
		}
	}
}
