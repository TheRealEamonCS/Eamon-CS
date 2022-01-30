﻿
// MonsterMemberActionState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static TheDeepCanyon.Game.Plugin.PluginContext;

namespace TheDeepCanyon.Game.States
{
	[ClassMappings]
	public class MonsterMemberActionState : EamonRT.Game.States.MonsterMemberActionState, IMonsterMemberActionState
	{
		public override void Execute()
		{
			LoopMonster = gMDB[Globals.LoopMonsterUid];

			Debug.Assert(LoopMonster != null);

			LoopMonsterRoom = LoopMonster.GetInRoom();

			Debug.Assert(LoopMonsterRoom != null);

			// Fido eats dead bodies

			if (LoopMonster.Uid == 11 && gGameState.FidoSleepCounter <= 0)
			{
				var artifactList = gEngine.GetArtifactList(a => a.DeadBody != null && a.IsInRoom(LoopMonsterRoom));

				if (artifactList.Count > 0)
				{
					var mealArtifact = artifactList[0];

					Debug.Assert(mealArtifact != null);

					// Munch, munch....

					if (LoopMonsterRoom.Uid == gGameState.Ro)
					{
						gOut.Print("{0} eats{1}, then goes to sleep for a while.", LoopMonsterRoom.IsLit() ? "Fido" : "Something", LoopMonsterRoom.IsLit() ? " " + mealArtifact.GetTheName() : "");
					}

					gGameState.FidoSleepCounter += mealArtifact.Weight;

					mealArtifact.SetInLimbo();

					LoopMonster.StateDesc = "(sound asleep)";

					LoopMonster.Reaction = Friendliness.Neutral;

					NextState = Globals.CreateInstance<IMonsterMemberLoopIncrementState>();

					Globals.NextState = NextState;
				}
				else
				{
					base.Execute();
				}
			}
			else
			{
				base.Execute();
			}
		}
	}
}
