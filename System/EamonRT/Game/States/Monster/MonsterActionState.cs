﻿
// MonsterActionState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.Globals;

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

		/// <summary></summary>
		public virtual long OldParry { get; set; }

		/// <summary></summary>
		public virtual long NewParry { get; set; }

		public override void Execute()
		{
			LoopMonster = gMDB[gEngine.LoopMonsterUid];

			Debug.Assert(LoopMonster != null);

			LoopMonsterRoom = LoopMonster.GetInRoom();

			Debug.Assert(LoopMonsterRoom != null);

			gEngine.LoopGroupCount = LoopMonster.CurrGroupCount;

			MonsterFleesCheck();

			if (GotoCleanup)
			{
				goto Cleanup;
			}

			MonsterAdjustsParryCheck();

			if (GotoCleanup)
			{
				goto Cleanup;
			}

			NextState = gEngine.CreateInstance<IMonsterMemberLoopInitializeState>();

		Cleanup:

			if (NextState == null)
			{
				NextState = gEngine.CreateInstance<IMonsterLoopIncrementState>();
			}

			gEngine.NextState = NextState;
		}

		public virtual void MonsterFleesCheck()
		{
			if (LoopMonster.CheckNBTLHostility() && LoopMonster.CanMoveToRoomUid(0, true) && !LoopMonster.CheckCourage())
			{
				Debug.Assert(LoopMonster.Reaction != Friendliness.Neutral);

				ActionCommand = gEngine.CreateInstance<IFleeCommand>(x =>
				{
					x.ActorMonster = LoopMonster;

					x.ActorRoom = LoopMonsterRoom;
				});

				ActionCommand.Execute();

				if (LoopMonster.CurrGroupCount >= gEngine.LoopGroupCount)
				{
					GotoCleanup = true;
				}
			}
		}

		public virtual void MonsterAdjustsParryCheck()
		{
			if (LoopMonster.CheckParryAdjustment())
			{
				OldParry = LoopMonster.Parry;

				NewParry = LoopMonster.GetParryAdjustment();

				if (OldParry != NewParry)
				{
					ActionCommand = gEngine.CreateInstance<IParryCommand>(x =>
					{
						x.ActorMonster = LoopMonster;

						x.ActorRoom = LoopMonsterRoom;

						x.Parry = NewParry;

						x.PrintCombatStanceChanged = LoopMonster.ShouldPrintCombatStanceChanged(OldParry, NewParry);
					});

					ActionCommand.Execute();

					if (LoopMonster.ShouldCombatStanceChangedConsumeTurn(OldParry, NewParry))
					{
						if (LoopMonster.CheckNBTLHostility())
						{
							gEngine.PauseCombat();
						}

						GotoCleanup = true;
					}
				}
			}
		}

		public MonsterActionState()
		{
			Name = "MonsterActionState";
		}
	}
}
