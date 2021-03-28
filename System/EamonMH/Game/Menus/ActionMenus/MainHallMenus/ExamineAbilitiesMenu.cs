
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

			gOut.Print("You are the {0} {1}", Globals.Character.EvalGender("mighty", "fair", "androgynous"), Globals.Character.Name);

			Buf.SetFormat("{0}{1}{2}%)",
				"(Learning: ",
				Globals.Character.GetIntellectBonusPct() > 0 ? "+" : "",
				Globals.Character.GetIntellectBonusPct());

			gOut.WriteLine("{0}{1}{2,-2}{3,20}{4,15}{5,-2}{0}{6}{7,-3}{8,34}{9,-2}{10,15}{11}{12}%)",
				Environment.NewLine,
				"Intellect:  ", Globals.Character.GetStats(Stat.Intellect),
				Buf,
				"Agility :  ", Globals.Character.GetStats(Stat.Agility),
				"Hardiness:  ", Globals.Character.GetStats(Stat.Hardiness),
				"Charisma:  ", Globals.Character.GetStats(Stat.Charisma),
				"(Charm Mon: ",
				Globals.Character.GetCharmMonsterPct() > 0 ? "+" : "",
				Globals.Character.GetCharmMonsterPct());

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
					var weapon = gEngine.GetWeapons((Weapon)i);

					Debug.Assert(weapon != null);

					gOut.Write(" {0,-5}: {1,3}%",
						weapon.Name,
						Globals.Character.GetWeaponAbilities(i));
				}
				else
				{
					gOut.Write("{0,12}", "");
				}

				if (Enum.IsDefined(typeof(Spell), i))
				{
					var spell = gEngine.GetSpells((Spell)i);

					Debug.Assert(spell != null);

					if (Globals.Character.GetSpellAbilities(i) > 0)
					{
						gOut.Write("{0,29}{1,-5}: {2,3}%",
						"",
						spell.Name,
						Globals.Character.GetSpellAbilities(i));
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
				Globals.Character.HeldGold,
				"In bank: ",
				Globals.Character.BankGold);

			var armor = gEngine.GetArmors(Globals.Character.ArmorClass);

			Debug.Assert(armor != null);

			gOut.Print("{0}{1,-25}{2}{3,3}%",
				"Armor:  ",
				armor.Name,
				"Armor Expertise: ",
				Globals.Character.ArmorExpertise);

			gOut.Print("{0}{1}{2}{3}{4}",
				"Weight Carryable: ",
				Globals.Character.GetWeightCarryableGronds(),
				" Gronds  (",
				Globals.Character.GetWeightCarryableDos(),
				" Dos)");

			gOut.Write("{0}{1}{2,25}{3,10}{4,10}{5,19}",
				Environment.NewLine,
				"Weapon Names:",
				"Complexity:",
				"Damage:",
				"# Hands:",
				"Base Odds to hit:");

			long odds = 0;

			if (Globals.Character.IsWeaponActive(0))
			{
				for (i = 0; i < Globals.Character.Weapons.Length; i++)
				{
					if (Globals.Character.IsWeaponActive(i))
					{
						rc = Globals.Character.GetBaseOddsToHit(i, ref odds);

						Debug.Assert(gEngine.IsSuccess(rc));

						gOut.Write("{0} {1} {2,3}%{3,9}D{4,-9}{5,-12}{6,3}%",
							Environment.NewLine,
							gEngine.Capitalize(Globals.Character.GetWeapons(i).Name.PadTRight(29, ' ')),
							Globals.Character.GetWeapons(i).Field1,
							Globals.Character.GetWeapons(i).Field3,
							Globals.Character.GetWeapons(i).Field4,
							Globals.Character.GetWeapons(i).Field5,
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
