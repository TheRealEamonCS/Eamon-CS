
// MonsterAttackActionState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Collections.Generic;
using System.Diagnostics;
using Eamon.Framework;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.Globals;

namespace EamonRT.Game.States
{
	[ClassMappings]
	public class MonsterAttackActionState : State, IMonsterAttackActionState
	{
		/// <summary></summary>
		public virtual IList<IMonster> HostileMonsterList { get; set; }

		/// <summary></summary>
		public virtual IMonster LoopMonster { get; set; }

		/// <summary></summary>
		public virtual IRoom LoopMonsterRoom { get; set; }

		/// <summary></summary>
		public virtual ICommand ActionCommand { get; set; }

		/// <summary></summary>
		public virtual long HostileMonsterListIndex { get; set; }

		public override void Execute()
		{
			LoopMonster = gMDB[gEngine.LoopMonsterUid];

			Debug.Assert(LoopMonster != null);

			LoopMonsterRoom = LoopMonster.GetInRoom();

			Debug.Assert(LoopMonsterRoom != null);

			MonsterAttackGetHostileMonsterList();

			if (GotoCleanup)
			{
				goto Cleanup;
			}

			MonsterAttackDefenderMissingCheck();

			if (GotoCleanup)
			{
				goto Cleanup;
			}

			MonsterAttackDefenderSelected();

			if (GotoCleanup)
			{
				goto Cleanup;
			}

			MonsterAttackExecuted();

			if (GotoCleanup)
			{
				goto Cleanup;
			}

		Cleanup:

			if (NextState == null)
			{
				NextState = gEngine.CreateInstance<IMonsterAttackLoopIncrementState>();
			}

			gEngine.NextState = NextState;
		}

		public virtual void MonsterAttackGetHostileMonsterList()
		{
			HostileMonsterList = gEngine.GetHostileMonsterList(LoopMonster);

			Debug.Assert(HostileMonsterList != null);

			if (HostileMonsterList.Count < 1)
			{
				NextState = gEngine.CreateInstance<IMonsterMemberLoopIncrementState>();

				GotoCleanup = true;
			}
		}

		public virtual void MonsterAttackDefenderMissingCheck()
		{
			if (LoopMonster.AttackCount > 1 && gEngine.LoopLastDobjMonster != null && !HostileMonsterList.Contains(gEngine.LoopLastDobjMonster))
			{
				NextState = gEngine.CreateInstance<IMonsterMemberLoopIncrementState>();

				GotoCleanup = true;
			}
		}

		public virtual void MonsterAttackDefenderSelected()
		{
			HostileMonsterListIndex = gEngine.RollDice(1, HostileMonsterList.Count, -1);

			ActionCommand = gEngine.CreateInstance<IMonsterAttackCommand>(x =>
			{
				x.NextState = gEngine.CreateInstance<IMonsterAttackLoopIncrementState>();

				x.ActorMonster = LoopMonster;

				x.ActorRoom = LoopMonsterRoom;

				x.MemberNumber = gEngine.LoopMemberNumber;

				x.AttackNumber = gEngine.LoopAttackNumber;

				x.Dobj = LoopMonster.AttackCount > 1 && gEngine.LoopLastDobjMonster != null ? gEngine.LoopLastDobjMonster : HostileMonsterList[(int)HostileMonsterListIndex];
			});

			gEngine.LoopLastDobjMonster = ActionCommand.Dobj as IMonster;
		}

		public virtual void MonsterAttackExecuted()
		{
			ActionCommand.Execute();

			NextState = ActionCommand.NextState;
		}

		public MonsterAttackActionState()
		{
			Name = "MonsterAttackActionState";
		}
	}
}
