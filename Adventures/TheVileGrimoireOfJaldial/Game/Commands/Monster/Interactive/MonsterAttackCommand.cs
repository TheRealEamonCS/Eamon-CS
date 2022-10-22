
// MonsterAttackCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using static TheVileGrimoireOfJaldial.Game.Plugin.Globals;

namespace TheVileGrimoireOfJaldial.Game.Commands
{
	[ClassMappings]
	public class MonsterAttackCommand : EamonRT.Game.Commands.MonsterAttackCommand, IMonsterAttackCommand
	{
		public override void Execute()
		{
			Debug.Assert(ActorMonster != null && DobjMonster != null);

			// efreeti has special attack behavior when water weird is present

			if (ActorMonster.Uid == 50 && AttackNumber == 1)
			{
				var waterWeirdMonster = gMDB[38];

				Debug.Assert(waterWeirdMonster != null);

				ActorMonster.AttackCount = DobjMonster?.Uid == 38 ? 1 : waterWeirdMonster.IsInRoom(ActorRoom) ? 3 : -3;
			}

			while (true)
			{
				// Monster selects its attack modality

				gActorMonster(this).SetAttackModality();

				// Beholder's clumsiness spells only work on non-group monsters

				if (ActorMonster.Uid == 36 && gActorMonster(this).AttackDesc.Equals("cast{0} a clumsiness spell on", StringComparison.OrdinalIgnoreCase) && DobjMonster.GroupCount > 1)
				{
					gGameState.ClumsySpells--;
				}
				else
				{
					break;
				}
			}

			base.Execute();
		}
	}
}
