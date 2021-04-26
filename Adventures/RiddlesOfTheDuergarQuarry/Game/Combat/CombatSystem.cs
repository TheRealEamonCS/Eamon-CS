
// CombatSystem.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Combat;
using static RiddlesOfTheDuergarQuarry.Game.Plugin.PluginContext;

namespace RiddlesOfTheDuergarQuarry.Game.Combat
{
	[ClassMappings]
	public class CombatSystem : EamonRT.Game.Combat.CombatSystem, ICombatSystem
	{
		public override void CheckMonsterStatus()
		{
			var room = DfMonster.GetInRoom();

			Debug.Assert(room != null);

			// Viper

			if (OfMonster?.Uid == 18 && !gGameState.ViperPoisonVictimUids.Contains(DfMonster.Uid))
			{
				if (DfMonster.IsCharacterMonster())
				{
					gOut.Write("{0}{1}You are poisoned!", Environment.NewLine, OmitBboaPadding ? "" : "  ");
				}
				else if (room.IsLit())
				{
					gOut.Write("{0}{1}{2} is poisoned!", Environment.NewLine, OmitBboaPadding ? "" : "  ", DfMonster.GetTheName(true));
				}

				gGameState.ViperPoisonVictimUids.Add(DfMonster.Uid);
			}

			base.CheckMonsterStatus();
		}
	}
}
