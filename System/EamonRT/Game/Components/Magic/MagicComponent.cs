﻿
// MagicComponent.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Eamon.Framework;
using Eamon.Framework.Primitive.Classes;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using Eamon.Game.Utilities;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Components;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.Globals;

namespace EamonRT.Game.Components
{
	[ClassMappings]
	public class MagicComponent : Component, IMagicComponent
	{
		public virtual Func<IArtifact, bool>[] ResurrectWhereClauseFuncs { get; set; }

		public virtual Func<IArtifact, bool>[] VanishWhereClauseFuncs { get; set; }

		public virtual IList<Spell> SpellValueList { get; set; }

		public virtual bool CastSpell { get; set; }

		/// <summary></summary>
		public virtual long DamageHealed { get; set; }

		/// <summary></summary>
		public virtual long SpeedTurns { get; set; }

		/// <summary></summary>
		public virtual long PowerEventRoll { get; set; }

		/// <summary></summary>
		public virtual MagicState MagicState { get; set; }

		public virtual void ExecuteBlastSpell()
		{
			Debug.Assert(SetNextStateFunc != null && CopyCommandDataFunc != null);

			Debug.Assert(DobjArtifact != null || DobjMonster != null);

			MagicState = MagicState.BeginSpellBlast;

			ExecuteStateMachine();
		}

		public virtual void ExecuteHealSpell()
		{
			Debug.Assert(DobjMonster != null);

			MagicState = MagicState.BeginSpellHeal;

			ExecuteStateMachine();
		}

		public virtual void ExecuteSpeedSpell()
		{
			Debug.Assert(ActorMonster != null);

			MagicState = MagicState.BeginSpellSpeed;

			ExecuteStateMachine();
		}

		public virtual void ExecutePowerSpell()
		{
			Debug.Assert(SetNextStateFunc != null && ActorMonster != null);

			MagicState = MagicState.BeginSpellPower;

			ExecuteStateMachine();
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

			var spell = gEngine.GetSpell(spellValue);

			Debug.Assert(spell != null);

			if (gGameState.GetSa(s) > 0 && gCharacter.GetSpellAbility(s) > 0)
			{
				rl = gGameState.InteractiveFiction ? 1 : gEngine.RollDice(1, 100, 0);
			}

			if (rl == 100)
			{
				PlayerSpellCastBrainOverload(s, spell);

				goto Cleanup;
			}

			if (rl > 0 && rl < 95 && (rl < 5 || rl <= gGameState.GetSa(s)))
			{
				result = true;

				if (!gGameState.InteractiveFiction)
				{
					gGameState.SetSa(s, (long)((double)gGameState.GetSa(s) * .5 + 1));
				}

				if (!OmitSkillGains)
				{
					rl = gEngine.RollDice(1, 100, 0);

					rl += gCharacter.GetIntellectBonusPct();

					if (rl > gCharacter.GetSpellAbility(s) && !gGameState.InteractiveFiction)
					{
						gEngine.SkillIncreaseFuncList.Add(() =>
						{
							if (!gEngine.IsRulesetVersion(5, 62))
							{
								PrintSpellAbilityIncreases(s, spell);
							}

							gCharacter.ModSpellAbility(s, 2);

							if (gCharacter.GetSpellAbility(s) > spell.MaxValue)
							{
								gCharacter.SetSpellAbility(s, spell.MaxValue);
							}
						});
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

			if (gEngine.IsRulesetVersion(5, 62))
			{
				gCharacter.SetSpellAbility(s, 0);
			}
		}

		/// <summary></summary>
		public virtual bool ShouldShowBlastSpellAttack()
		{
			return DobjMonster != null || DobjArtifact.DisguisedMonster == null;
		}

		/// <summary></summary>
		public virtual void BeginSpellBlast()
		{
			if (CastSpell && !CheckPlayerSpellCast(Spell.Blast))
			{
				if (DobjMonster != null)
				{
					gEngine.PauseCombat();
				}

				MagicState = MagicState.EndMagic;

				goto Cleanup;
			}

			MagicState = MagicState.CheckAfterCastBlast;

		Cleanup:

			;
		}

		/// <summary></summary>
		public virtual void CheckAfterCastBlast()
		{
			MagicState = DobjMonster != null ? MagicState.AggravateMonster : MagicState.AttackDobj;
		}

		/// <summary></summary>
		public virtual void AggravateMonster()
		{
			if (DobjMonster.Reaction != Friendliness.Enemy)
			{
				gEngine.MonsterGetsAggravated(DobjMonster);
			}

			MagicState = MagicState.CheckAfterAggravateMonster;
		}

		/// <summary></summary>
		public virtual void CheckAfterAggravateMonster()
		{
			MagicState = MagicState.AttackDobj;
		}

		/// <summary></summary>
		public virtual void AttackDobj()
		{
			if (ShouldShowBlastSpellAttack())
			{
				PrintZapDirectHit();
			}
			else
			{
				OmitFinalNewLine = true;
			}

			RedirectCommand = gEngine.CreateInstance<IAttackCommand>(x =>
			{
				x.BlastSpell = true;
			});

			CopyCommandDataFunc(RedirectCommand);

			SetNextStateFunc(RedirectCommand);

			MagicState = MagicState.EndMagic;
		}

		/// <summary></summary>
		public virtual void BeginSpellHeal()
		{
			if (CastSpell && !CheckPlayerSpellCast(Spell.Heal))
			{
				MagicState = MagicState.EndMagic;

				goto Cleanup;
			}

			MagicState = MagicState.CheckAfterCastHeal;

		Cleanup:

			;
		}

		/// <summary></summary>
		public virtual void CheckAfterCastHeal()
		{
			MagicState = MagicState.HealInjury;
		}

		/// <summary></summary>
		public virtual void HealInjury()
		{
			if (DobjMonster.DmgTaken > 0)
			{
				PrintHealthImproves(DobjMonster);

				DamageHealed = gEngine.RollDice(1, gEngine.IsRulesetVersion(5, 62) ? 10 : 12, 0);

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
		public virtual void BeginSpellSpeed()
		{
			if (CastSpell && !CheckPlayerSpellCast(Spell.Speed))
			{
				MagicState = MagicState.EndMagic;

				goto Cleanup;
			}

			MagicState = MagicState.CheckAfterCastSpeed;

		Cleanup:

			;
		}

		/// <summary></summary>
		public virtual void CheckAfterCastSpeed()
		{
			MagicState = MagicState.BoostAgility;
		}

		/// <summary></summary>
		public virtual void BoostAgility()
		{
			if (gGameState.Speed <= 0)
			{
				ActorMonster.Agility *= 2;
			}

			MagicState = MagicState.CalculateSpeedTurns;
		}

		/// <summary></summary>
		public virtual void CalculateSpeedTurns()
		{
			SpeedTurns = gEngine.IsRulesetVersion(5, 62) ? gEngine.RollDice(1, 25, 9) : gEngine.RollDice(1, 10, 10);

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
		public virtual void BeginSpellPower()
		{
			if (CastSpell && !CheckPlayerSpellCast(Spell.Power))
			{
				MagicState = MagicState.EndMagic;

				goto Cleanup;
			}

			MagicState = MagicState.CheckAfterCastPower;

		Cleanup:

			;
		}

		/// <summary></summary>
		public virtual void CheckAfterCastPower()
		{
			PowerEventRoll = gEngine.RollDice(1, 100, 0);

			if (gEngine.IsRulesetVersion(5))
			{
				MagicState = MagicState.RaiseDeadVanishArtifacts;
			}
			else if (gEngine.IsRulesetVersion(62))
			{
				MagicState = MagicState.TeleportToRoom;
			}
			else
			{
				MagicState = MagicState.SonicBoomFortuneCookie;
			}
		}

		/// <summary></summary>
		public virtual void SonicBoomFortuneCookie()
		{
			// 50% chance of boom

			if (PowerEventRoll > 50)
			{
				PrintSonicBoom(ActorRoom);
			}

			// 50% chance of fortune cookie

			else
			{
				PrintFortuneCookie();
			}

			MagicState = MagicState.EndMagic;
		}

		/// <summary></summary>
		public virtual void RaiseDead()
		{
			// 25% chance of raise the dead

			if (PowerEventRoll < 80)
			{
				if (gEngine.ResurrectDeadBodies(ActorRoom, ResurrectWhereClauseFuncs))
				{
					MagicState = MagicState.EndMagic;

					goto Cleanup;
				}
			}

			// 21% chance of SPEED spell

			CastSpell = false;

			MagicState = MagicState.BeginSpellSpeed;

		Cleanup:

			;
		}

		/// <summary></summary>
		public virtual void RaiseDeadVanishArtifacts()
		{
			// Raise the dead / Make stuff vanish

			if (gEngine.ResurrectDeadBodies(ActorRoom, ResurrectWhereClauseFuncs) || gEngine.MakeArtifactsVanish(ActorRoom, VanishWhereClauseFuncs))
			{
				MagicState = MagicState.EndMagic;

				goto Cleanup;
			}

			MagicState = MagicState.TunnelCollapses;

		Cleanup:

			;
		}

		/// <summary></summary>
		public virtual void TunnelCollapses()
		{
			// 10% chance of death trap

			if (PowerEventRoll < 11)
			{
				PrintTunnelCollapses(ActorRoom);

				gGameState.Die = 1;

				SetNextStateFunc(gEngine.CreateInstance<IPlayerDeadState>(x =>
				{
					x.PrintLineSep = true;
				}));

				MagicState = MagicState.EndMagic;

				goto Cleanup;
			}

			MagicState = MagicState.SonicBoom;

		Cleanup:

			;
		}

		/// <summary></summary>
		public virtual void SonicBoom()
		{
			// 20%/75% chance of boom

			if ((gEngine.IsRulesetVersion(62) && PowerEventRoll < 55) || (!gEngine.IsRulesetVersion(62) && PowerEventRoll < 86))
			{
				PrintSonicBoom(ActorRoom);

				MagicState = MagicState.EndMagic;

				goto Cleanup;
			}

			if (gEngine.IsRulesetVersion(62))
			{
				MagicState = MagicState.RaiseDead;
			}
			else
			{
				MagicState = MagicState.AllWoundsHealed;
			}

		Cleanup:

			;
		}

		/// <summary></summary>
		public virtual void AllWoundsHealed()
		{
			// 10%/5% chance of full heal

			if ((gEngine.IsRulesetVersion(62) && PowerEventRoll < 35 && ActorMonster.DmgTaken > 0) || (!gEngine.IsRulesetVersion(62) && PowerEventRoll > 95))
			{
				PrintAllWoundsHealed();

				ActorMonster.DmgTaken = 0;

				MagicState = MagicState.EndMagic;

				goto Cleanup;
			}

			if (gEngine.IsRulesetVersion(62))
			{
				MagicState = MagicState.MagicSkillsIncrease;
			}
			else
			{
				// 10% chance of SPEED spell

				CastSpell = false;

				MagicState = MagicState.BeginSpellSpeed;
			}

		Cleanup:

			;
		}

		/// <summary></summary>
		public virtual void TeleportToRoom()
		{
			// 9% chance of teleport

			if (PowerEventRoll < 10)
			{
				PrintTeleportToRoom();

				gGameState.R2 = gEngine.RollDice(1, gEngine.Module.NumRooms, 0);

				SetNextStateFunc(gEngine.CreateInstance<IAfterPlayerMoveState>(x =>
				{
					x.MoveMonsters = false;
				}));

				MagicState = MagicState.EndMagic;

				goto Cleanup;
			}

			MagicState = MagicState.ArmorThickens;

		Cleanup:

			;
		}

		/// <summary></summary>
		public virtual void ArmorThickens()
		{
			// 15% chance of armor thickening

			if (PowerEventRoll < 25)
			{
				var rl = gEngine.RollDice(1, 8, -1);      // TODO: verify 8

				if (gGameState.Ar <= 0 || rl < ActorMonster.Armor)
				{
					PowerEventRoll = 1;

					MagicState = MagicState.TeleportToRoom;

					goto Cleanup;
				}

				PrintArmorThickens();

				ActorMonster.Armor += 2;

				MagicState = MagicState.EndMagic;

				goto Cleanup;
			}

			MagicState = MagicState.AllWoundsHealed;

		Cleanup:

			;
		}

		/// <summary></summary>
		public virtual void MagicSkillsIncrease()
		{
			// 10% chance of magic skills increasing

			if (PowerEventRoll < 35)
			{
				PrintMagicSkillsIncrease();

				SpellValueList = EnumUtil.GetValues<Spell>();

				foreach (var sv in SpellValueList)
				{
					var spell = gEngine.GetSpell(sv);

					Debug.Assert(spell != null);

					gGameState.SetSa(sv, gCharacter.GetSpellAbility(sv) * 2);

					if (gGameState.GetSa(sv) > spell.MaxValue)
					{
						gGameState.SetSa(sv, spell.MaxValue);
					}
				}

				MagicState = MagicState.EndMagic;

				goto Cleanup;
			}

			MagicState = MagicState.SonicBoom;

		Cleanup:

			;
		}

		/// <summary></summary>
		public virtual void ExecuteStateMachine()
		{
			Debug.Assert(MagicState == MagicState.BeginSpellBlast || MagicState == MagicState.BeginSpellHeal || MagicState == MagicState.BeginSpellSpeed || MagicState == MagicState.BeginSpellPower);

			while (true)
			{
				switch (MagicState)
				{
					case MagicState.BeginSpellBlast:

						BeginSpellBlast();

						break;

					case MagicState.CheckAfterCastBlast:

						CheckAfterCastBlast();

						break;

					case MagicState.AggravateMonster:

						AggravateMonster();

						break;

					case MagicState.CheckAfterAggravateMonster:

						CheckAfterAggravateMonster();

						break;

					case MagicState.AttackDobj:

						AttackDobj();

						break;

					case MagicState.BeginSpellHeal:

						BeginSpellHeal();

						break;

					case MagicState.CheckAfterCastHeal:

						CheckAfterCastHeal();

						break;

					case MagicState.HealInjury:

						HealInjury();

						break;

					case MagicState.ShowHealthStatus:

						ShowHealthStatus();

						break;

					case MagicState.BeginSpellSpeed:

						BeginSpellSpeed();

						break;

					case MagicState.CheckAfterCastSpeed:

						CheckAfterCastSpeed();

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

					case MagicState.BeginSpellPower:

						BeginSpellPower();

						break;

					case MagicState.CheckAfterCastPower:

						CheckAfterCastPower();

						break;

					case MagicState.SonicBoomFortuneCookie:

						SonicBoomFortuneCookie();

						break;

					case MagicState.RaiseDead:

						RaiseDead();

						break;

					case MagicState.RaiseDeadVanishArtifacts:

						RaiseDeadVanishArtifacts();

						break;

					case MagicState.TunnelCollapses:

						TunnelCollapses();

						break;

					case MagicState.SonicBoom:

						SonicBoom();

						break;

					case MagicState.AllWoundsHealed:

						AllWoundsHealed();

						break;

					case MagicState.TeleportToRoom:

						TeleportToRoom();

						break;

					case MagicState.ArmorThickens:

						ArmorThickens();

						break;

					case MagicState.MagicSkillsIncrease:

						MagicSkillsIncrease();

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
