
// MonsterActionState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static WrenholdsSecretVigil.Game.Plugin.Globals;

namespace WrenholdsSecretVigil.Game.States
{
	[ClassMappings]
	public class MonsterActionState : EamonRT.Game.States.MonsterActionState, IMonsterActionState
	{
		public override void Execute()
		{
			LoopMonster = gMDB[gEngine.LoopMonsterUid];

			Debug.Assert(LoopMonster != null);

			LoopMonsterRoom = LoopMonster.GetInRoom();

			Debug.Assert(LoopMonsterRoom != null);

			// Try to open running device, all flee

			if (LoopMonster.CanMoveToRoom(true) && gEngine.DeviceOpened)
			{
				ActionCommand = gEngine.CreateInstance<IFleeCommand>(x =>
				{
					x.ActorMonster = LoopMonster;

					x.ActorRoom = LoopMonsterRoom;
				});

				ActionCommand.Execute();

				NextState = gEngine.CreateInstance<IMonsterLoopIncrementState>();

				gEngine.NextState = NextState;
			}
			else
			{
				base.Execute();
			}
		}
	}
}

