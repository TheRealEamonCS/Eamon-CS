
// CombatComponent.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using Eamon.Game.Utilities;
using EamonRT.Framework.Components;
using Enums = Eamon.Framework.Primitive.Enums;
using RTEnums = EamonRT.Framework.Primitive.Enums;
using static TheVileGrimoireOfJaldial.Game.Plugin.Globals;

namespace TheVileGrimoireOfJaldial.Game.Components
{
	[ClassMappings(typeof(ICombatComponent))]
	public class CombatComponent : EamonRT.Game.Components.CombatComponent, Framework.Components.ICombatComponent
	{
		public virtual bool CrossbowTrap { get; set; }

		public virtual bool ScoredCriticalHit { get; set; }

		public override void ExecuteAttack()
		{
			var griffinMonster = gMDB[40];

			Debug.Assert(griffinMonster != null);

			// Attacking baby griffins makes the parent angry

			if (DobjMonster != null && DobjMonster.Uid == 41 && !griffinMonster.IsInLimbo() && !gGameState.GriffinAngered)
			{
				gEngine.MiscEventFuncList02.Add(() =>
				{
					if (griffinMonster.IsInRoom(ActorRoom) && ActorRoom.IsLit())
					{
						gEngine.PrintEffectDesc(82);
					}

					gGameState.GriffinAngered = true;
				});
			}

			base.ExecuteAttack();
		}

		public override void PrintMiss(IMonster monster, IArtifact weapon)
		{
			Debug.Assert(monster != null);

			string missDesc = null;

			var rl = gEngine.RollDice(1, 100, 0);

			// Beholder

			if (ActorMonster?.Uid == 36)
			{
				var beholderMonster = ActorMonster as Framework.IMonster;

				Debug.Assert(beholderMonster != null);

				switch (beholderMonster.AttackDesc)
				{
					case "cast{0} a clumsiness spell on":

						missDesc = "Ineffective";

						break;

					case "cast{0} a fireball at":

						missDesc = rl > 50 ? "Dodged" : "Missed";

						break;

					case "cast{0} a mystic missile at":

						missDesc = "Missed";

						break;
				}
			}

			// Jaldi'al the lich

			else if (ActorMonster?.Uid == 43)
			{
				var jaldialMonster = ActorMonster as Framework.IMonster;

				Debug.Assert(jaldialMonster != null);

				switch (jaldialMonster.AttackDesc)
				{
					case "cast{0} a lightning bolt at":

						missDesc = "Missed";

						break;

					case "cast{0} an ice bolt at":

						missDesc = rl > 50 ? "Dodged" : "Missed";

						break;

					case "mentally blast{0}":

						missDesc = "Ineffective";

						break;
				}
			}

			if (!string.IsNullOrWhiteSpace(missDesc))
			{
				gOut.Write("{0} --- {1}!", Environment.NewLine, missDesc);
			}
			else
			{
				base.PrintMiss(monster, weapon);
			}
		}

		public override void PrintCriticalHit()
		{
			ScoredCriticalHit = true;

			base.PrintCriticalHit();
		}

		public override void RollToHitOrMiss()
		{
			ScoredCriticalHit = false;

			base.RollToHitOrMiss();

			// Bloodnettle always hits when draining blood (and ignores armor)

			if (ActorMonster?.Uid == 20 && DobjMonster.Uid == gGameState.BloodnettleVictimUid)
			{
				if (_rl > _odds)
				{
					_rl = _odds;
				}

				OmitArmor = true;
			}
		}

		public override void CalculateDamage()
		{
			var beholderMonster = gMDB[36] as Framework.IMonster;

			Debug.Assert(beholderMonster != null);

			var waterWeirdMonster = gMDB[38] as Framework.IMonster;

			Debug.Assert(waterWeirdMonster != null);

			// Bypass damage calculation for beholder clumsiness spell and water weird envelopment

			if ((ActorMonster?.Uid == 36 && beholderMonster.AttackDesc.Equals("cast{0} a clumsiness spell on", StringComparison.OrdinalIgnoreCase)) || (ActorMonster?.Uid == 38 && waterWeirdMonster.AttackDesc.Equals("envelop{0}", StringComparison.OrdinalIgnoreCase)))
			{
				CombatState = RTEnums.CombatState.CheckMonsterStatus;
			}
			else
			{
				base.CalculateDamage();
			}
		}

		public override void CheckArmor()
		{
			var beholderMonster = gMDB[36] as Framework.IMonster;

			Debug.Assert(beholderMonster != null);

			var waterWeirdMonster = gMDB[38] as Framework.IMonster;

			Debug.Assert(waterWeirdMonster != null);

			// Bypass armor check for beholder clumsiness spell and water weird envelopment

			if ((ActorMonster?.Uid == 36 && beholderMonster.AttackDesc.Equals("cast{0} a clumsiness spell on", StringComparison.OrdinalIgnoreCase)) || (ActorMonster?.Uid == 38 && waterWeirdMonster.AttackDesc.Equals("envelop{0}", StringComparison.OrdinalIgnoreCase)))
			{
				CombatState = RTEnums.CombatState.CheckMonsterStatus;
			}
			else
			{
				var room = gRDB[gGameState.Ro];

				Debug.Assert(room != null);

				var artTypes = new Enums.ArtifactType[] { Enums.ArtifactType.Weapon, Enums.ArtifactType.MagicWeapon };

				var immuneMonsterUids = new long[] { 8, 9, 14, 15, 16, 17 };

				var ac = ActorWeapon != null ? ActorWeapon.GetArtifactCategory(artTypes) : null;

				// Apply special defenses

				if (ActorMonster?.Uid != 50 && !BlastSpell)
				{
					// Some monsters are immune to non-magical weapons

					if (immuneMonsterUids.Contains(DobjMonster.Uid))
					{
						if (ac == null || ac.Field1 < 20)
						{
							if (DobjMonster.IsInRoom(room))
							{
								gOut.Write("{0}{1}{2} seems unaffected{3}!", Environment.NewLine, OmitBboaPadding ? "" : "  ", room.EvalLightLevel("The defender", DobjMonster.GetTheName(true)), ActorWeapon != null ? " by the weapon" : "");
							}

							CombatState = RTEnums.CombatState.EndAttack;

							goto Cleanup;
						}
					}

					// Skeleton and crimson amoeba are resistant to non-club weapons (half damage)

					else if (DobjMonster.Uid == 3 || DobjMonster.Uid == 25)
					{
						if (ac == null || ac.Field2 != (long)Enums.Weapon.Club)
						{
							_d2 = (long)Math.Round((double)_d2 / 2.0);
						}
					}

					// Water weird is extremely resistant to non-club weapons (minimum damage)

					else if (DobjMonster.Uid == 38)
					{
						if (ac == null || ac.Field2 != (long)Enums.Weapon.Club)
						{
							if (_d2 > 1)
							{
								_d2 = 1;
							}
						}
					}
				}

				// Bloodnettle always injures when draining blood

				if (ActorMonster?.Uid == 20 && DobjMonster.Uid == gGameState.BloodnettleVictimUid && _d2 < 1)
				{
					_d2 = 1;
				}

				base.CheckArmor();

			Cleanup:

				;
			}
		}

		public override void CheckMonsterStatus()
		{
			var room = DobjMonster.GetInRoom();

			Debug.Assert(room != null);

			var rl = gEngine.RollDice(1, 100, 0);

			// Apply special attacks

			if (ActorMonster?.Uid == 9)
			{
				if (DobjMonster.Uid > 17 && rl > 50)
				{
					if (DobjMonster.IsCharacterMonster() || room.IsLit())
					{
						gOut.Write("{0}{1}{2}", Environment.NewLine, OmitBboaPadding ? "" : "  ", DobjMonster.IsCharacterMonster() ? "You suddenly feel weaker!" : DobjMonster.GetTheName(true) + " suddenly looks weaker!");
					}

					var stat = gEngine.GetStat(Stat.Hardiness);

					Debug.Assert(stat != null);

					DobjMonster.Hardiness--;

					if (DobjMonster.Hardiness < stat.MinValue)
					{
						DobjMonster.Hardiness = stat.MinValue;
					}
					else if (DobjMonster.IsCharacterMonster())
					{
						gGameState.PlayerHardinessPointsDrained++;
					}

					if (DobjMonster.DmgTaken > DobjMonster.Hardiness)
					{
						DobjMonster.DmgTaken = DobjMonster.Hardiness;
					}
				}
			}
			else if (ActorMonster?.Uid == 11)
			{
				var carrionCrawlerMonster = ActorMonster as Framework.IMonster;

				Debug.Assert(carrionCrawlerMonster != null);

				if (DobjMonster.Uid != 50 && carrionCrawlerMonster.AttackDesc.Equals("flail{0} at", StringComparison.OrdinalIgnoreCase) && rl > 50)
				{
					var saved = DobjMonster.IsCharacterMonster() ? gEngine.SaveThrow(Stat.Hardiness) : rl > 80;

					if (!saved)
					{
						var rl02 = gEngine.RollDice(2, 2, 1);

						if (ScoredCriticalHit)
						{
							rl02 *= 2;
						}

						var firstParalyzed = !gGameState.ParalyzedTargets.ContainsKey(DobjMonster.Uid);

						if (firstParalyzed)
						{
							gGameState.ParalyzedTargets[DobjMonster.Uid] = 0;
						}

						if (DobjMonster.IsCharacterMonster())
						{
							gOut.Write("{0}{1}You are{2} paralyzed!", Environment.NewLine, OmitBboaPadding ? "" : "  ", !firstParalyzed ? " further" : "");
						}
						else if (room.IsLit())
						{
							gOut.Write("{0}{1}{2} is{3} paralyzed!", Environment.NewLine, OmitBboaPadding ? "" : "  ", DobjMonster.GetTheName(true), !firstParalyzed ? " further" : "");
						}
						else if (firstParalyzed)
						{
							gOut.Write("{0}{1}The defender falls silent!", Environment.NewLine, OmitBboaPadding ? "" : "  ");
						}

						gGameState.ParalyzedTargets[DobjMonster.Uid] += rl02;
					}
				}
			}
			else if (ActorMonster?.Uid == 14 || ActorMonster?.Uid == 16)
			{
				if (DobjMonster.Uid > 17 && rl > 60)
				{
					if (DobjMonster.IsCharacterMonster() || room.IsLit())
					{
						gOut.Write("{0}{1}{2}", Environment.NewLine, OmitBboaPadding ? "" : "  ", DobjMonster.IsCharacterMonster() ? "You suddenly feel less skillful!" : DobjMonster.GetTheName(true) + " suddenly looks less skillful!");
					}

					// Only apply skill loss to the player character

					if (DobjMonster.IsCharacterMonster())
					{
						var weaponValues = EnumUtil.GetValues<Weapon>();

						foreach (var wv in weaponValues)
						{
							var weapon = gEngine.GetWeapon(wv);

							Debug.Assert(weapon != null);

							gCharacter.ModWeaponAbility(wv, -gEngine.RollDice(1, ActorMonster?.Uid == 14 ? 4 : 2, 0));

							if (gCharacter.GetWeaponAbility(wv) < weapon.MinValue)
							{
								gCharacter.SetWeaponAbility(wv, weapon.MinValue);
							}
						}
					}
				}
			}
			else if (ActorMonster?.Uid == 36)
			{
				var beholderMonster = ActorMonster as Framework.IMonster;

				Debug.Assert(beholderMonster != null);

				if (beholderMonster.AttackDesc.Equals("cast{0} a clumsiness spell on", StringComparison.OrdinalIgnoreCase))
				{
					var saved = DobjMonster.IsCharacterMonster() ? gEngine.SaveThrow(Stat.Intellect) : rl > 50;

					if (!saved)
					{
						var rl02 = gEngine.RollDice(1, 5, 2);

						if (ScoredCriticalHit)
						{
							rl02 *= 2;
						}

						IList<long> roundsList = null;

						if (gGameState.ClumsyTargets.ContainsKey(DobjMonster.Uid))
						{
							roundsList = gGameState.ClumsyTargets[DobjMonster.Uid];
						}
						else
						{
							roundsList = new List<long>();

							gGameState.ClumsyTargets[DobjMonster.Uid] = roundsList;
						}

						roundsList.Add(rl02);

						if (DobjMonster.IsCharacterMonster())
						{
							gOut.Write("{0}{1}You suddenly feel {2}less agile!", Environment.NewLine, OmitBboaPadding ? "" : "  ", ScoredCriticalHit ? "far " : "");
						}
						else
						{
							gOut.Write("{0}{1}{2} suddenly {3} {4}less agile!", Environment.NewLine, OmitBboaPadding ? "" : "  ", room.EvalLightLevel("The defender", DobjMonster.GetTheName(true)), room.EvalLightLevel("sounds", "looks"), ScoredCriticalHit ? "far " : "");
						}
					}
					else
					{
						if (DobjMonster.IsCharacterMonster() || room.IsLit())
						{
							gOut.Write("{0}{1}Spell resisted!", Environment.NewLine, OmitBboaPadding ? "" : "  ");
						}
					}

					CombatState = RTEnums.CombatState.EndAttack;

					goto Cleanup;
				}
			}
			else if (ActorMonster?.Uid == 38)
			{
				var waterWeirdMonster = ActorMonster as Framework.IMonster;

				Debug.Assert(waterWeirdMonster != null);

				if (waterWeirdMonster.AttackDesc.Equals("envelop{0}", StringComparison.OrdinalIgnoreCase))
				{
					var saved = DobjMonster.IsCharacterMonster() ? gEngine.SaveThrow(Stat.Hardiness) : rl > 40;

					if (!saved)
					{
						if (DobjMonster.IsCharacterMonster())
						{
							gOut.Write("{0}{1}You are held down by {2}, and drown immediately!", Environment.NewLine, OmitBboaPadding ? "" : "  ", room.EvalLightLevel("the offender", ActorMonster?.GetTheName()));
						}
						else
						{
							gOut.Write("{0}{1}{2} is held down by {3}, and drowns immediately!", Environment.NewLine, OmitBboaPadding ? "" : "  ", room.EvalLightLevel("The defender", DobjMonster.GetTheName(true)), room.EvalLightLevel("the offender", ActorMonster?.GetTheName()));
						}

						DobjMonster.DmgTaken = DobjMonster.Hardiness;
					}
					else
					{
						if (DobjMonster.IsCharacterMonster())
						{
							gOut.Write("{0}{1}You break free of {2}'s grip!", Environment.NewLine, OmitBboaPadding ? "" : "  ", room.EvalLightLevel("the offender", ActorMonster?.GetTheName()));
						}
						else
						{
							gOut.Write("{0}{1}{2} breaks free of {3}'s grip!", Environment.NewLine, OmitBboaPadding ? "" : "  ", room.EvalLightLevel("The defender", DobjMonster.GetTheName(true)), room.EvalLightLevel("the offender", ActorMonster?.GetTheName()));
						}

						CombatState = RTEnums.CombatState.EndAttack;

						goto Cleanup;
					}
				}
			}

			if (_d2 > 0 && gGameState.ShowCombatDamage && room.IsLit())
			{
				gOut.Write("{0}{1}Blow does {2} point{3} of damage.{4}", Environment.NewLine, OmitBboaPadding ? "" : "  ", _d2, _d2 != 1 ? "s" : "", BlastSpell || CrossbowTrap ? Environment.NewLine : "");
			}

			base.CheckMonsterStatus();

			// Bloodnettle selects its next victim

			if (ActorMonster?.Uid == 20 && !DobjMonster.IsInLimbo() && gGameState.BloodnettleVictimUid == 0)
			{
				gGameState.BloodnettleVictimUid = DobjMonster.Uid;
			}

		Cleanup:

			;
		}
	}
}
