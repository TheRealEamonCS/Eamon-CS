
// MagicComponent.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using Eamon.Framework.Primitive.Classes;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Components;
using EamonRT.Framework.Primitive.Enums;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.Components
{
	[ClassMappings]
	public class MagicComponent : Component, IMagicComponent
	{
		public virtual bool CastSpell { get; set; }

		/// <summary></summary>
		public virtual long DamageHealed { get; set; }

		/// <summary></summary>
		public virtual long SpeedTurns { get; set; }

		/// <summary></summary>
		public virtual MagicState MagicState { get; set; }

		public virtual void ExecuteBlastSpell()
		{
			// TODO: implement
		}

		public virtual void ExecuteHealSpell()
		{
			Debug.Assert(DobjMonster != null);

			MagicState = MagicState.BeginHealSpell;

			ExecuteStateMachine();
		}

		public virtual void ExecuteSpeedSpell()
		{
			MagicState = MagicState.BeginSpeedSpell;

			ExecuteStateMachine();
		}

		public virtual void ExecutePowerSpell()
		{
			// TODO: implement
		}

		/// <summary></summary>
		public virtual void BeginHealSpell()
		{
			if (CastSpell && !CheckPlayerSpellCast(Spell.Heal))
			{
				MagicState = MagicState.EndMagic;

				goto Cleanup;
			}

			MagicState = MagicState.HealInjury;

		Cleanup:

			;
		}

		/// <summary></summary>
		public virtual void HealInjury()
		{
			Debug.Assert(DobjMonster != null);

			if (DobjMonster.DmgTaken > 0)
			{
				PrintHealthImproves(DobjMonster);

				DamageHealed = gEngine.RollDice(1, Globals.IsRulesetVersion(5, 15, 25) ? 10 : 12, 0);

				DobjMonster.DmgTaken -= DamageHealed;
			}

			if (DobjMonster.DmgTaken < 0)
			{
				DobjMonster.DmgTaken = 0;
			}

			MagicState = MagicState.ShowHealthStatus;
		}

		/// <summary></summary>
		public virtual void ShowHealthStatus()
		{
			PrintHealthStatus(DobjMonster, false);

			MagicState = MagicState.EndMagic;
		}

		/// <summary></summary>
		public virtual void BeginSpeedSpell()
		{
			if (CastSpell && !CheckPlayerSpellCast(Spell.Speed))
			{
				MagicState = MagicState.EndMagic;

				goto Cleanup;
			}

			MagicState = MagicState.BoostAgility;

		Cleanup:

			;
		}

		/// <summary></summary>
		public virtual void BoostAgility()
		{
			Debug.Assert(ActorMonster != null);

			if (gGameState.Speed <= 0)
			{
				ActorMonster.Agility *= 2;
			}

			MagicState = MagicState.CalculateSpeedTurns;
		}

		/// <summary></summary>
		public virtual void CalculateSpeedTurns()
		{
			SpeedTurns = Globals.IsRulesetVersion(5, 15, 25) ? gEngine.RollDice(1, 25, 9) : gEngine.RollDice(1, 10, 10);

			gGameState.Speed += (SpeedTurns + 1);

			MagicState = MagicState.FeelEnergized;
		}

		/// <summary></summary>
		public virtual void FeelEnergized()
		{
			PrintFeelNewAgility();

			MagicState = MagicState.EndMagic;
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

		/// <summary></summary>
		public virtual void ExecuteStateMachine()
		{
			Debug.Assert(MagicState == MagicState.BeginSpeedSpell);

			while (true)
			{
				switch (MagicState)
				{
					case MagicState.BeginHealSpell:

						BeginHealSpell();

						break;

					case MagicState.HealInjury:

						HealInjury();

						break;

					case MagicState.ShowHealthStatus:

						ShowHealthStatus();

						break;

					case MagicState.BeginSpeedSpell:

						BeginSpeedSpell();

						break;

					case MagicState.BoostAgility:

						BoostAgility();

						break;

					case MagicState.CalculateSpeedTurns:

						CalculateSpeedTurns();

						break;

					case MagicState.FeelEnergized:

						FeelEnergized();

						break;

					case MagicState.EndMagic:

						goto Cleanup;

					default:

						Debug.Assert(false, "Invalid MagicState");

						break;
				}
			}

		Cleanup:

			if (!OmitFinalNewLine)
			{
				gOut.WriteLine();
			}
		}

		public MagicComponent()
		{

		}
	}
}
