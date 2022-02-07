
// MagicComponent.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using Eamon.Framework.Primitive.Classes;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Components;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.Components
{
	[ClassMappings]
	public class MagicComponent : Component, IMagicComponent
	{
		public virtual bool CastSpell { get; set; }

		/// <summary></summary>
		public virtual long SpeedTurns { get; set; }

		public virtual void ExecuteBlastSpell()
		{
			// TODO: implement
		}

		public virtual void ExecuteHealSpell()
		{
			// TODO: implement
		}

		public virtual void ExecuteSpeedSpell()
		{
			Debug.Assert(ActorMonster != null);

			if (CastSpell && !CheckPlayerSpellCast(Spell.Speed))
			{
				goto Cleanup;
			}

			if (gGameState.Speed <= 0)
			{
				ActorMonster.Agility *= 2;
			}

			SpeedTurns = Globals.IsRulesetVersion(5, 15, 25) ? gEngine.RollDice(1, 25, 9) : gEngine.RollDice(1, 10, 10);

			gGameState.Speed += (SpeedTurns + 1);

			PrintFeelNewAgility();

		Cleanup:

			;
		}

		public virtual void ExecutePowerSpell()
		{
			// TODO: implement
		}

		/// <summary></summary>
		/// <param name="spellValue"></param>
		/// <returns></returns>
		public virtual bool CheckPlayerSpellCast(Spell spellValue)
		{
			Debug.Assert(Enum.IsDefined(typeof(Spell), spellValue));

			var result = false;

			var rl = 0L;

			var s = spellValue;

			var spell = gEngine.GetSpells(spellValue);

			Debug.Assert(spell != null);

			if (gGameState.GetSa(s) > 0 && gCharacter.GetSpellAbilities(s) > 0)
			{
				rl = gEngine.RollDice(1, 100, 0);
			}

			if (rl == 100)
			{
				PlayerSpellCastBrainOverload(s, spell);

				goto Cleanup;
			}

			if (rl > 0 && rl < 95 && (rl < 5 || rl <= gGameState.GetSa(s)))
			{
				result = true;

				gGameState.SetSa(s, (long)((double)gGameState.GetSa(s) * .5 + 1));

				if (!OmitSkillGains)
				{
					rl = gEngine.RollDice(1, 100, 0);

					rl += gCharacter.GetIntellectBonusPct();

					if (rl > gCharacter.GetSpellAbilities(s))
					{
						Globals.SpellSkillIncreaseFunc = () =>
						{
							if (!Globals.IsRulesetVersion(5, 15, 25))
							{
								PrintSpellAbilityIncreased(s, spell);
							}

							gCharacter.ModSpellAbilities(s, 2);

							if (gCharacter.GetSpellAbilities(s) > spell.MaxValue)
							{
								gCharacter.SetSpellAbilities(s, spell.MaxValue);
							}
						};
					}
				}
			}
			else
			{
				PrintSpellCastFailed(s, spell);

				goto Cleanup;
			}

		Cleanup:

			return result;
		}

		/// <summary></summary>
		/// <param name="s"></param>
		/// <param name="spell"></param>
		public virtual void PlayerSpellCastBrainOverload(Spell s, ISpell spell)
		{
			Debug.Assert(Enum.IsDefined(typeof(Spell), s));

			Debug.Assert(spell != null);

			PrintSpellOverloadsBrain(s, spell);

			gGameState.SetSa(s, 0);

			if (Globals.IsRulesetVersion(5, 15, 25))
			{
				gCharacter.SetSpellAbilities(s, 0);
			}
		}

		public MagicComponent()
		{

		}
	}
}
