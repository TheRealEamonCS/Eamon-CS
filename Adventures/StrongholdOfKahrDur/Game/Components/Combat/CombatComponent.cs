
// CombatComponent.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using Eamon.Framework;
using Eamon.Game.Attributes;
using EamonRT.Framework.Components;
using EamonRT.Framework.Primitive.Enums;
using static StrongholdOfKahrDur.Game.Plugin.PluginContext;

namespace StrongholdOfKahrDur.Game.Components
{
	[ClassMappings]
	public class CombatComponent : EamonRT.Game.Components.CombatComponent, ICombatComponent
	{
		public override void ExecuteAttack()
		{
			if (DobjMonster != null && BlastSpell)
			{
				var helmArtifact = gADB[25];

				Debug.Assert(helmArtifact != null);

				// If player is wearing Wizard's Helm (25), blast spell is more potent

				ExecuteCalculateDamage(2, helmArtifact.IsWornByCharacter() ? 12 : 5);

				Globals.PauseCombatAfterSkillGains = Globals.SpellSkillIncreaseFunc != null || Globals.WeaponSkillIncreaseFunc != null || Globals.ArmorSkillIncreaseFunc != null;
				
				if (!Globals.PauseCombatAfterSkillGains)
				{ 
					Globals.Thread.Sleep(gGameState.PauseCombatMs);
				}
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

		public override void BeginAttack()
		{
			// Necromancer (22) is impervious to weapon attacks

			if (DobjMonster != null && DobjMonster.Uid == 22)
			{
				OmitSkillGains = true;
			}

			base.BeginAttack();
		}

		public override void AttackHit()
		{
			// Necromancer (22) is impervious to weapon attacks

			if (DobjMonster.Uid == 22)
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
