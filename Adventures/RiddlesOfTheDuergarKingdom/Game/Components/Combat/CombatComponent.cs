
// CombatComponent.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using Eamon.Framework;
using Eamon.Game.Attributes;
using EamonRT.Framework.Components;
using EamonRT.Framework.Primitive.Enums;
using static RiddlesOfTheDuergarKingdom.Game.Plugin.Globals;

namespace RiddlesOfTheDuergarKingdom.Game.Components
{
	[ClassMappings]
	public class CombatComponent : EamonRT.Game.Components.CombatComponent, ICombatComponent
	{
		public override void ExecuteAttack()
		{
			// Adjust black spider stats

			if (ActorMonster?.Uid == 16 && DobjMonster != null)
			{
				ActorMonster.NwDice = DobjMonster.Hardiness - DobjMonster.DmgTaken;

				OmitArmor = true;

				FixedResult = gEngine.RollDice(1, 100, 0) >= 5 ? AttackResult.Hit : AttackResult.CriticalHit;
			}

			// Player always hits black spider

			if (ActorMonster?.Uid == gGameState.Cm && DobjMonster?.Uid == 16)
			{
				FixedResult = gEngine.RollDice(1, 100, 0) >= 5 ? AttackResult.Hit : AttackResult.CriticalHit;
			}

			base.ExecuteAttack();

			// Adjust black spider stats

			if (ActorMonster?.Uid == 16)
			{
				ActorMonster.NwDice = 1;
			}
		}

		public override void PrintHealthStatus(IRoom room, IMonster actorMonster, IMonster dobjMonster, bool blastSpell, bool nonCombat)
		{
			Debug.Assert(room != null && dobjMonster != null);

			var monsterDies = dobjMonster != null ? dobjMonster.IsDead() : false;

			// Black spider kills player

			if (actorMonster?.Uid == 16 && dobjMonster?.Uid == gGameState.Cm && monsterDies)
			{
				gOut.WriteLine();

				gEngine.PrintEffectDesc(58);
			}

			base.PrintHealthStatus(room, actorMonster, dobjMonster, blastSpell, nonCombat);

			// Player kills black spider

			if (actorMonster?.Uid == gGameState.Cm && dobjMonster?.Uid == 16 && monsterDies)		// TODO: if dead bodies used should be dead black spider's Desc
			{
				gOut.WriteLine();

				gEngine.PrintEffectDesc(59);
			}
		}

		public override void CheckMonsterStatus()
		{
			var room = DobjMonster.GetInRoom();

			Debug.Assert(room != null);

			var rl = gEngine.RollDice(1, 100, 0);

			// Apply special attacks

			if (ActorMonster?.Uid == 1 && rl > 50)
			{
				long poisonDice;

				string furtherString;

				if (gGameState.PoisonedTargets.TryGetValue(DobjMonster.Uid, out poisonDice))
				{
					gGameState.PoisonedTargets[DobjMonster.Uid] = ++poisonDice;

					furtherString = "further ";
				}
				else
				{
					poisonDice = 1;

					gGameState.PoisonedTargets.Add(DobjMonster.Uid, poisonDice);

					furtherString = "";

					gGameState.AfterPrintPlayerRoomEventHeap.Insert02(gGameState.CurrTurn + gEngine.PoisonInjuryTurns, "PoisonSickensMonsterEvent", DobjMonster.Uid);
				}

				if (DobjMonster.IsCharacterMonster() || room.IsViewable())
				{
					gOut.Write("{0}{1}{2}", Environment.NewLine, OmitBboaPadding ? "" : "  ", DobjMonster.IsCharacterMonster() ? "You are " + furtherString + "poisoned!" : DobjMonster.GetTheName(true, true, false, false, true) + " is " + furtherString + "poisoned!");
				}
			}

			base.CheckMonsterStatus();
		}
	}
}
