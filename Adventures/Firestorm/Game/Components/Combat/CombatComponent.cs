
// CombatComponent.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using Eamon.Framework;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using EamonRT.Framework.Components;
using static Firestorm.Game.Plugin.Globals;

namespace Firestorm.Game.Components
{
	[ClassMappings]
	public class CombatComponent : EamonRT.Game.Components.CombatComponent, ICombatComponent
	{
		public override void PrintHealthStatus(IRoom room, IMonster actorMonster, IMonster dobjMonster, bool blastSpell, bool nonCombat)
		{
			Debug.Assert(room != null && dobjMonster != null);

			var monsterDies = dobjMonster.IsDead();

			if (!monsterDies)
			{
				var dobjMonsterName = room.EvalViewability(nonCombat ? "The entity" : dobjMonster == actorMonster ? "The offender" : "The defender", dobjMonster.GetTheName(true, true, false, false, true));

				gEngine.Buf.SetFormat("{0} health is now {1}%.", 
					dobjMonster.IsCharacterMonster() ? "Your" :
					dobjMonsterName.AddPossessiveSuffix(), 
					(long)Math.Round((double)(dobjMonster.Hardiness - dobjMonster.DmgTaken) / (double)dobjMonster.Hardiness * 100));

				gOut.Print("{0}", gEngine.Buf);
			}
			else
			{
				base.PrintHealthStatus(room, actorMonster, dobjMonster, blastSpell, nonCombat);
			}
		}

		public override void PrintCriticalHit()
		{
			gOut.Write("Well struck!");
		}

		public override void PrintBlowTurned(IMonster monster, bool omitBboaPadding)
		{
			Debug.Assert(monster != null);

			if (monster.Armor > 0)
			{
				var armorDesc = monster.GetArmorDescString();

				gOut.Write("{0}{1}Blow glances off {2}!", Environment.NewLine, omitBboaPadding ? "" : "  ", armorDesc);
			}
			else
			{
				base.PrintBlowTurned(monster, omitBboaPadding);
			}
		}
	}
}
