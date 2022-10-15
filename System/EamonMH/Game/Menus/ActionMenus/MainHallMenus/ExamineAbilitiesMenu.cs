
// ExamineAbilitiesMenu.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using Eamon;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using Eamon.Game.Menus;
using Eamon.Game.Utilities;
using EamonMH.Framework.Menus.ActionMenus;
using static EamonMH.Game.Plugin.PluginContext;

namespace EamonMH.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class ExamineAbilitiesMenu : Menu, IExamineAbilitiesMenu
	{
		public override void Execute()
		{
			RetCode rc;

			gOut.Print("{0}", Globals.LineSep);

			gOut.Print("You are the {0} {1}", gCharacter.EvalGender("mighty", "fair", "androgynous"), gCharacter.Name);

			Buf.SetFormat("{0}{1}{2}%)",
				"(Learning: ",
				gCharacter.GetIntellectBonusPct() > 0 ? "+" : "",
				gCharacter.GetIntellectBonusPct());

			gOut.WriteLine("{0}{1}{2,-2}{3,20}{4,15}{5,-2}{0}{6}{7,-3}{8,34}{9,-2}{10,15}{11}{12}%)",
				Environment.NewLine,
				"Intellect:  ", gCharacter.GetStat(Stat.Intellect),
				Buf,
				"Agility :  ", gCharacter.GetStat(Stat.Agility),
				"Hardiness:  ", gCharacter.GetStat(Stat.Hardiness),
				"Charisma:  ", gCharacter.GetStat(Stat.Charisma),
				"(Charm Mon: ",
				gCharacter.GetCharmMonsterPct() > 0 ? "+" : "",
				gCharacter.GetCharmMonsterPct());

			gOut.Write("{0}{1}{2,39}",
				Environment.NewLine,
				"Weapon Abilities:",
				"Spell Abilities:");

			var weaponValues = EnumUtil.GetValues<Weapon>();

			var spellValues = EnumUtil.GetValues<Spell>();

			var i = Math.Min((long)weaponValues[0], (long)spellValues[0]);

			var j = Math.Max((long)weaponValues[weaponValues.Count - 1], (long)spellValues[spellValues.Count - 1]);

			while (i <= j)
			{
				gOut.WriteLine();

				if (Enum.IsDefined(typeof(Weapon), i))
				{
					var weapon = gEngine.GetWeapon((Weapon)i);

					Debug.Assert(weapon != null);

					gOut.Write(" {0,-5}: {1,3}%",
						weapon.Name,
						gCharacter.GetWeaponAbility(i));
				}
				else
				{
					gOut.Write("{0,12}", "");
				}

				if (Enum.IsDefined(typeof(Spell), i))
				{
					var spell = gEngine.GetSpell((Spell)i);

					Debug.Assert(spell != null);

					if (gCharacter.GetSpellAbility(i) > 0)
					{
						gOut.Write("{0,29}{1,-5}: {2,3}%",
						"",
						spell.Name,
						gCharacter.GetSpellAbility(i));
					}
					else
					{
						gOut.Write("{0,29}{1,-5}: {2}",
							"",
							spell.Name,
							"None");
					}
				}

				i++;
			}

			gOut.WriteLine("{0}{0}{1}{2,-26}{3}{4,-6}",
				Environment.NewLine,
				"Gold: ",
				gCharacter.HeldGold,
				"In bank: ",
				gCharacter.BankGold);

			var armor = gEngine.GetArmor(gCharacter.ArmorClass);

			Debug.Assert(armor != null);

			gOut.Print("{0}{1,-25}{2}{3,3}%",
				"Armor:  ",
				armor.Name,
				"Armor Expertise: ",
				gCharacter.ArmorExpertise);

			gOut.Print("{0}{1}{2}{3}{4}",
				"Weight Carryable: ",
				gCharacter.GetWeightCarryableGronds(),
				" Gronds  (",
				gCharacter.GetWeightCarryableDos(),
				" Dos)");

			gOut.Write("{0}{1}{2,25}{3,10}{4,10}{5,19}",
				Environment.NewLine,
				"Weapon Names:",
				"Complexity:",
				"Damage:",
				"# Hands:",
				"Base Odds to hit:");

			long odds = 0;

			if (gCharacter.IsWeaponActive(0))
			{
				for (i = 0; i < gCharacter.Weapons.Length; i++)
				{
					if (gCharacter.IsWeaponActive(i))
					{
						rc = gCharacter.GetBaseOddsToHit(i, ref odds);

						Debug.Assert(gEngine.IsSuccess(rc));

						gOut.Write("{0} {1} {2,3}%{3,9}D{4,-9}{5,-12}{6,3}%",
							Environment.NewLine,
							gEngine.Capitalize(gCharacter.GetWeapon(i).Name.PadTRight(29, ' ')),
							gCharacter.GetWeapon(i).Field1,
							gCharacter.GetWeapon(i).Field3,
							gCharacter.GetWeapon(i).Field4,
							gCharacter.GetWeapon(i).Field5,
							odds);
					}
					else
					{
						break;
					}
				}
			}
			else
			{
				gOut.Write("{0}{0}{1,42}",
					Environment.NewLine,
					"No Weapons");
			}

			gOut.WriteLine();

			Globals.In.KeyPress(Buf);
		}

		public ExamineAbilitiesMenu()
		{
			Buf = Globals.Buf;
		}
	}
}
