
// AfterPrintPlayerRoomEventState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Components;
using EamonRT.Framework.States;
using static RiddlesOfTheDuergarKingdom.Game.Plugin.Globals;

namespace RiddlesOfTheDuergarKingdom.Game.States
{
	[ClassMappings]
	public class AfterPrintPlayerRoomEventState : EamonRT.Game.States.AfterPrintPlayerRoomEventState, IAfterPrintPlayerRoomEventState
	{
		public virtual void PoisonSickensMonsterEvent(object obj)
		{
			Debug.Assert(obj != null);

			long monsterUid;

			if (long.TryParse(obj.ToString(), out monsterUid) == false)
			{
				monsterUid = 0;
			}

			var monster = gMDB[monsterUid];

			var monsterRoom = monster != null ? monster.GetInRoom() : null;

			long poisonDice;
			
			if (gGameState.PoisonedTargets.TryGetValue(monsterUid, out poisonDice) == false)
			{
				poisonDice = 0;
			}

			if (monsterRoom != null && poisonDice > 0)
			{
				var currGroupCount = monster.CurrGroupCount;

				var scheduleEvent = false;

				var printMessage = monster.IsCharacterMonster() || (monsterRoom.Uid == gGameState.Ro && monsterRoom.IsLit());

				if (printMessage)
				{
					gOut.Print("The poison spreading through {0} causes further sickness!", monster.IsCharacterMonster() ? "you" : monster.GetTheName(false, true, false, false, true));
				}

				var combatComponent = gEngine.CreateInstance<ICombatComponent>(x =>
				{
					x.SetNextStateFunc = s => NextState = s;

					x.ActorRoom = monster.GetInRoom();

					x.Dobj = monster;

					x.OmitArmor = true;

					x.OmitMonsterStatus = !printMessage;
				});

				combatComponent.ExecuteCalculateDamage(poisonDice, 1);

				if (monster.IsCharacterMonster())
				{
					if (gGameState.Die <= 0)
					{
						scheduleEvent = true;
					}
					else
					{ 
						GotoCleanup = true;
					}
				}
				else if (!monster.IsInLimbo() && monster.CurrGroupCount == currGroupCount)
				{
					scheduleEvent = true;
				}

				if (scheduleEvent)
				{
					gGameState.AfterPrintPlayerRoomEventHeap.Insert02(gGameState.CurrTurn + gEngine.PoisonInjuryTurns, "PoisonSickensMonsterEvent", monsterUid);
				}
			}
		}
	}
}
