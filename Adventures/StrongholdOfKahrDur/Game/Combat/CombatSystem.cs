
// CombatSystem.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Combat;
using EamonRT.Framework.Primitive.Enums;
using static StrongholdOfKahrDur.Game.Plugin.PluginContext;

namespace StrongholdOfKahrDur.Game.Combat
{
	[ClassMappings]
	public class CombatSystem : EamonRT.Game.Combat.CombatSystem, ICombatSystem
	{
		public override void ExecuteAttack()
		{
			if (BlastSpell)
			{
				var helmArtifact = gADB[25];

				Debug.Assert(helmArtifact != null);

				PrintBlast();

				// If player is wearing Wizard's Helm (25), blast spell is more potent

				ExecuteCalculateDamage(2, helmArtifact.IsWornByCharacter() ? 12 : 5);
				
				Globals.Thread.Sleep(gGameState.PauseCombatMs);
			}
			else
			{
				base.ExecuteAttack();
			}
		}

		public override void PrintCriticalHit()
		{
			gOut.Write("Well struck!");
		}

		public override void PrintBlowTurned()
		{
			if (DfMonster.Armor > 0)
			{
				var armorDesc = DfMonster.GetArmorDescString();

				gOut.Write("{0}{1}Blow glances off {2}!", Environment.NewLine, OmitBboaPadding ? "" : "  ", armorDesc);
			}
			else
			{
				base.PrintBlowTurned();
			}
		}

		public override void BeginAttack()
		{
			// Necromancer (22) is impervious to weapon attacks

			if (DfMonster != null && DfMonster.Uid == 22)
			{
				OmitSkillGains = true;
			}

			base.BeginAttack();
		}

		public override void AttackHit()
		{
			// Necromancer (22) is impervious to weapon attacks

			if (DfMonster.Uid == 22)
			{
				var rl = gEngine.RollDice(1, 4, 60);

				gEngine.PrintEffectDesc(rl, false);

				CombatState = CombatState.EndAttack;
			}
			else
			{
				base.AttackHit();
			}
		}
	}
}
