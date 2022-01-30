
// Engine.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Eamon;
using Eamon.Framework;
using Eamon.Framework.Args;
using Eamon.Framework.Primitive.Classes;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using Eamon.Game.Utilities;
using static AlternateBeginnersCave.Game.Plugin.PluginContext;

namespace AlternateBeginnersCave.Game
{
	[ClassMappings(typeof(IEngine))]
	public class Engine : EamonRT.Game.Engine, EamonRT.Framework.IEngine
	{
		/// <summary></summary>
		public virtual long HeldWpnIdx { get; set; }

		public override RetCode StatDisplay(IStatDisplayArgs args)
		{
			StringBuilder buf01, buf02, buf03;
			RetCode rc;
			long i, j;

			IWeapon weapon;
			ISpell spell;

			if (args == null || args.Character == null || args.Monster == null || args.ArmorString == null || args.SpellAbilities == null)
			{
				rc = RetCode.InvalidArg;

				// PrintError

				goto Cleanup;
			}

			rc = RetCode.Success;

			var origPunctSpaceCode = gOut.PunctSpaceCode;

			gOut.PunctSpaceCode = PunctSpaceCode.None;

			buf01 = new StringBuilder(Constants.BufSize);

			buf02 = new StringBuilder(Constants.BufSize);

			buf03 = new StringBuilder(Constants.BufSize);

			var omitSkillStats = Globals.IsRulesetVersion(15, 25) && GetGameState() != null;

			gOut.Print("{0,-36}Gender: {1,-9}Damage Taken: {2}/{3}",
				args.Monster.Name.ToUpper(),
				args.Character.EvalGender("Male", "Female", "Neutral"),
				args.Monster.DmgTaken,
				args.Monster.Hardiness);

			var ibp = GetIntellectBonusPct(args.Character.GetStats(Stat.Intellect));

			buf01.AppendFormat("{0}{1}{2}%)",
				"(Learning: ",
				ibp > 0 ? "+" : "",
				ibp);

			buf02.AppendFormat("{0}{1}",
				args.Speed > 0 ? args.Monster.Agility / 2 : args.Monster.Agility,
				args.Speed > 0 ? "x2" : "");

			buf03.AppendFormat("{0}{1}",
				gGameState.MagicDaggerCounter > 0 ? args.Monster.Hardiness - 5 : args.Monster.Hardiness,
				gGameState.MagicDaggerCounter > 0 ? "+5" : "  ");

			gOut.WriteLine("{0}{1}{2,-2}{3,20}{4,15}{5}{0}{6}{7,-5}{8,32}{9,-2}{10,15}{11}{12}%)",
				Environment.NewLine,
				"Intellect:  ", args.Character.GetStats(Stat.Intellect),
				buf01.ToString(),
				"Agility :  ", buf02.ToString(),
				"Hardiness:  ", buf03.ToString(),
				"Charisma:  ", args.Character.GetStats(Stat.Charisma),
				"(Charm Mon: ",
				args.CharmMon > 0 ? "+" : "",
				args.CharmMon);

			if (!omitSkillStats)
			{
				gOut.Write("{0}{1}{2,39}",
					Environment.NewLine,
					"Weapon Abilities:",
					"Spell Abilities:");

				var weaponValues = EnumUtil.GetValues<Weapon>();

				var spellValues = EnumUtil.GetValues<Spell>();

				i = Math.Min((long)weaponValues[0], (long)spellValues[0]);

				j = Math.Max((long)weaponValues[weaponValues.Count - 1], (long)spellValues[spellValues.Count - 1]);

				while (i <= j)
				{
					gOut.WriteLine();

					if (Enum.IsDefined(typeof(Weapon), i))
					{
						weapon = GetWeapons((Weapon)i);

						Debug.Assert(weapon != null);

						gOut.Write(" {0,-5}: {1,3}%",
							weapon.Name,
							args.Character.GetWeaponAbilities(i));
					}
					else
					{
						gOut.Write("{0,12}", "");
					}

					if (Enum.IsDefined(typeof(Spell), i))
					{
						spell = GetSpells((Spell)i);

						Debug.Assert(spell != null);

						gOut.Write("{0,29}{1,-5}: {2,3}% / {3}%",
							"",
							spell.Name,
							args.GetSpellAbilities(i),
							args.Character.GetSpellAbilities(i));
					}

					i++;
				}
			}

			gOut.WriteLine("{0}{0}{1}{2,-30}{3}{4,-6}",
				Environment.NewLine,
				"Gold: ",
				args.Character.HeldGold,
				"In bank: ",
				args.Character.BankGold);

			gOut.Print("Armor:  {0}{1}",
				args.ArmorString.PadTRight(28, ' '),
				!omitSkillStats ? string.Format(" Armor Expertise:  {0}%", args.Character.ArmorExpertise) : "");

			var wcg = GetWeightCarryableGronds(args.Monster.Hardiness);

			gOut.Print("Weight carried: {0}/{1} Gronds (One Grond = Ten DOS)",
				args.Weight,
				wcg);

			gOut.PunctSpaceCode = origPunctSpaceCode;

		Cleanup:

			return rc;
		}

		public override void PrintWakingUpMonsters()
		{
			gOut.Print("(Please wait while I stir things up...)");
		}

		public override void AddUniqueCharsToArtifactAndMonsterNames()
		{
			base.AddUniqueCharsToArtifactAndMonsterNames();

			var signArtifact = gADB[23];     // Sign #2

			Debug.Assert(signArtifact != null);

			signArtifact.Name = signArtifact.Name.TrimEnd('#');
		}

		public override void InitArtifacts()
		{
			base.InitArtifacts();

			var synonyms = new Dictionary<long, string[]>()
			{
				{ 5, new string[] { "label" } },
				{ 8, new string[] { "ornate knife", "knife" } },
				{ 9, new string[] { "scrap of parchment", "scrap" } },
				{ 20, new string[] { "vulture" } },
				{ 21, new string[] { "odd-looking torch", "odd looking torch", "odd torch", "torch" } },
				{ 22, new string[] { "massive inset ring", "inset ring", "ring" } },
				{ 37, new string[] { "shimmering blank wall", "shimmering wall", "blank wall", "east wall", "blank", "wall" } },
			};

			foreach (var synonym in synonyms)
			{
				CreateArtifactSynonyms(synonym.Key, synonym.Value);
			}
		}

		public override IArtifact ConvertWeaponToArtifact(ICharacterArtifact weapon)
		{
			var artifact = base.ConvertWeaponToArtifact(weapon);

			var i = Array.FindIndex(gCharacter.Weapons, x => x == weapon);

			if (i != gGameState.UsedWpnIdx)
			{
				artifact.SetInLimbo();

				gGameState.SetHeldWpnUids(HeldWpnIdx++, artifact.Uid);
			}

			return artifact;
		}

		public override void ResetMonsterStats(IMonster monster)
		{
			Debug.Assert(monster != null && monster.IsCharacterMonster());

			base.ResetMonsterStats(monster);

			// Eighty-six the weird magic dagger-related Hardiness increase upon exit (omitting the in-game death check for simplicity)

			if (gGameState.MagicDaggerCounter > 0)
			{
				monster.Hardiness -= 5;

				gGameState.MagicDaggerCounter = -1;
			}
		}

		public override void ConvertToCarriedInventory(IList<IArtifact> weaponList)
		{
			for (var i = 0; i < gGameState.HeldWpnUids.Length; i++)
			{
				if (gGameState.GetHeldWpnUids(i) > 0)
				{
					var artifact = gADB[gGameState.GetHeldWpnUids(i)];

					Debug.Assert(artifact != null);

					artifact.SetCarriedByCharacter();
				}
			}

			base.ConvertToCarriedInventory(weaponList);
		}
	}
}
