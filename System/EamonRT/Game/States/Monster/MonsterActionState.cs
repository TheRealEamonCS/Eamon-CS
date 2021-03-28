
// MonsterActionState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.States
{
	[ClassMappings]
	public class MonsterActionState : State, IMonsterActionState
	{
		/// <summary></summary>
		public virtual IMonster LoopMonster { get; set; }

		/// <summary></summary>
		public virtual IRoom LoopMonsterRoom { get; set; }

		/// <summary></summary>
		public virtual ICommand ActionCommand { get; set; }

		public override void Execute()
		{
			LoopMonster = gMDB[Globals.LoopMonsterUid];

			Debug.Assert(LoopMonster != null);

			LoopMonsterRoom = LoopMonster.GetInRoom();

			Debug.Assert(LoopMonsterRoom != null);

			Globals.LoopGroupCount = LoopMonster.CurrGroupCount;

			MonsterFleesCheck();

			if (GotoCleanup)
			{
				goto Cleanup;
			}

			NextState = Globals.CreateInstance<IMonsterMemberLoopInitializeState>();

		Cleanup:

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IMonsterLoopIncrementState>();
			}

			Globals.NextState = NextState;
		}

		public virtual void MonsterFleesCheck()
		{
			if (LoopMonster.CheckNBTLHostility() && LoopMonster.CanMoveToRoom(true) && !LoopMonster.CheckCourage())
			{
				Debug.Assert(LoopMonster.Reaction != Friendliness.Neutral);

				ActionCommand = Globals.CreateInstance<IMonsterFleeCommand>(x =>
				{
					x.ActorMonster = LoopMonster;

					x.ActorRoom = LoopMonsterRoom;
				});

				ActionCommand.Execute();

				if (LoopMonster.CurrGroupCount >= Globals.LoopGroupCount)
				{
					GotoCleanup = true;
				}
			}
		}

		public MonsterActionState()
		{
			Uid = 14;

			Name = "MonsterActionState";
		}
	}
}
