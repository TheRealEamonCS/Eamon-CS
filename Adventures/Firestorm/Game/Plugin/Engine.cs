
// Engine.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using Eamon;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Extensions;
using static Firestorm.Game.Plugin.Globals;

namespace Firestorm.Game.Plugin
{
	public class Engine : EamonRT.Game.Plugin.Engine, Framework.Plugin.IEngine
	{
		StringBuilder Framework.Plugin.IEngine.Buf { get; set; }

		StringBuilder Framework.Plugin.IEngine.Buf01 { get; set; }

		public virtual long[] GU { get; set; }

		public virtual string[] RM { get; set; }

		public virtual long[] RN { get; set; }

		public virtual long SecretBonus { get; } = 1500;

		public virtual bool RestoreGame { get; set; }

		public override RetCode LoadPluginClassMappings()
		{
			RetCode rc;

			rc = base.LoadPluginClassMappings();

			if (rc != RetCode.Success)
			{
				goto Cleanup;
			}

			rc = LoadPluginClassMappings01(Assembly.GetExecutingAssembly());

		Cleanup:

			return rc;
		}

		public override void PrintEnemiesNearby()
		{
			Out.Print("You can't turn your back here!");
		}

		public override void PrintHealthStatus(IMonster monster, bool includeUninjuredGroupMonsters)
		{
			Debug.Assert(monster != null);

			var room = monster.GetInRoom();

			Debug.Assert(room != null);

			var isCharMonster = monster.IsCharacterMonster();

			var isUninjuredGroupMonster = includeUninjuredGroupMonsters && monster.CurrGroupCount > 1 && monster.DmgTaken == 0;

			gEngine.Buf.SetFormat("{0} health is {1}%.",
				isCharMonster ? "Your" :
				isUninjuredGroupMonster ? "Their" :
				room.IsViewable() ? monster.GetTheName(true, true, false, false, true).AddPossessiveSuffix() : "The offender's",
				(long)Math.Round((double)(monster.Hardiness - monster.DmgTaken) / (double)monster.Hardiness * 100));

			Out.Print("{0}", gEngine.Buf);
		}

		public override void PrintGoodsPayment(bool goodsExist, long goldAmount)
		{
			// Adjust for secret bonus

			base.PrintGoodsPayment(goodsExist, goldAmount + SecretBonus);
		}

		public override void InitArtifacts()
		{
			base.InitArtifacts();

			var synonyms = new Dictionary<long, string[]>()
			{
				{ 1, new string[] { "box" } },
				{ 2, new string[] { "copper key", "skeleton key", "key" } },
				{ 3, new string[] { "hollow spear", "iron spear", "spear" } },
				{ 4, new string[] { "gateway" } },
				{ 5, new string[] { "west wall", "wall", "cracks", "rectangle", "passage" } },
				{ 6, new string[] { "broadsword" } },
				{ 8, new string[] { "axe" } },
				{ 9, new string[] { "mace" } },
				{ 11, new string[] { "door" } },
				{ 12, new string[] { "door-in-the-wall", "door" } },
				{ 13, new string[] { "door" } },
				{ 15, new string[] { "bag", "gold" } },
				{ 16, new string[] { "earplug" } },
				{ 18, new string[] { "spell book", "book" } },
				{ 19, new string[] { "chest" } },
				{ 20, new string[] { "robe" } },
				{ 21, new string[] { "torch" } },
				{ 23, new string[] { "heap", "muffled voice", "voice" } },
				{ 24, new string[] { "plate mail", "magic armor", "armor" } },
				{ 26, new string[] { "metal" } },
				{ 27, new string[] { "diamond" } },
				{ 29, new string[] { "sword" } },
				{ 32, new string[] { "stick" } },
				{ 34, new string[] { "pile", "rocks", "stone" } },
				{ 35, new string[] { "scattered pile", "rock pile", "pile" } },
				{ 36, new string[] { "sword" } },
				{ 38, new string[] { "green axe", "battle axe", "axe" } },
				{ 39, new string[] { "crossbow" } },
				{ 41, new string[] { "herbs", "roots" } },
				{ 42, new string[] { "symbols" } },
				{ 43, new string[] { "wand" } },
				{ 44, new string[] { "drawer" } },
				{ 46, new string[] { "coins" } },
				{ 47, new string[] { "note" } },
				{ 48, new string[] { "note" } },
				{ 49, new string[] { "bag" } },
				{ 50, new string[] { "bottle" } },
				{ 53, new string[] { "coin" } },
				{ 54, new string[] { "book" } },
				{ 55, new string[] { "gem" } },
				{ 58, new string[] { "sword" } },
				{ 59, new string[] { "large door", "oak door", "door" } },
				{ 60, new string[] { "ornate key", "brass key", "key" } },
				{ 61, new string[] { "arrows" } },
				{ 62, new string[] { "sign" } },
				{ 63, new string[] { "potion" } },
				{ 64, new string[] { "longsword" } },
				{ 65, new string[] { "draughts" } },
				{ 66, new string[] { "buzzsword", "buzz sword" } },
			};

			foreach (var synonym in synonyms)
			{
				CreateArtifactSynonyms(synonym.Key, synonym.Value);
			}
		}

		public override void InitMonsters()
		{
			base.InitMonsters();

			var synonyms = new Dictionary<long, string[]>()
			{
				{ 31, new string[] { "soldier" } },
				{ 32, new string[] { "soldier" } },
				{ 38, new string[] { "monster" } },
				{ 39, new string[] { "junior" } },
			};

			foreach (var synonym in synonyms)
			{
				CreateMonsterSynonyms(synonym.Key, synonym.Value);
			}
		}

		public override void ConvertCharacterToMonster()
		{
			gGameState.SetOS(1, Character.GetStat(Stat.Hardiness));

			gGameState.SetOS(2, Character.GetStat(Stat.Agility));

			if (Character.GetStat(Stat.Hardiness) > 24 || Character.GetStat(Stat.Agility) > 24)
			{
				Out.Print("{0}", LineSep);

				Out.Print("Hey, hold it! No superheroes allowed in here. Let's reduce your Stats a bit.");

				In.KeyPress(gEngine.Buf);
			}

			if (Character.GetStat(Stat.Hardiness) > 24)
			{
				Character.SetStat(Stat.Hardiness, 24);
			}

			if (Character.GetStat(Stat.Agility) > 24)
			{
				Character.SetStat(Stat.Agility, 24);
			}

			base.ConvertCharacterToMonster();
		}

		public override void ConvertMonsterToCharacter(IMonster monster, IList<IArtifact> weaponList)
		{
			Debug.Assert(monster != null);

			base.ConvertMonsterToCharacter(monster, weaponList);

			Character.SetStat(Stat.Hardiness, gGameState.GetOS(1));

			Character.SetStat(Stat.Agility, gGameState.GetOS(2));
		}

		public override void ConvertToCarriedInventory(IList<IArtifact> weaponList)
		{
			// Convert magic crossbow into artifact type gold

			var magicCrossbowArtifact = ADB[39];

			Debug.Assert(magicCrossbowArtifact != null);

			ConvertWeaponToGoldOrTreasure(magicCrossbowArtifact, true);

			base.ConvertToCarriedInventory(weaponList);
		}

		public override void SellInventoryToMerchant(bool sellInventory = true)
		{
			PushRulesetVersion(62);

			base.SellInventoryToMerchant(sellInventory);

			PopRulesetVersion();
		}

		public override void CheckToExtinguishLightSource()
		{
			// Do nothing
		}

		public override void ConvertCharArtifactsToArtifacts(IMonster monster)
		{
			base.ConvertCharArtifactsToArtifacts(monster);

			if (GameState.HeldWpnUids.Count > 0)
			{
				Out.Print("{0}", LineSep);

				Out.Print("What's with those super-weapons? Say bye-bye to them!");

				In.KeyPress(gEngine.Buf);
			}
		}

		public virtual void PrintPebblesLeft(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			if (artifact.Field1 == 1)
			{
				Out.Print("Only one pebble left!");
			}
			else
			{
				Out.Print("There are {0} pebbles left.", GetStringFromNumber(artifact.Field1, false, gEngine.Buf));
			}
		}

		public virtual void PrintHealingHerbsLeft(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			Out.Print("There's {0} left!", GetStringFromNumber(artifact.Field1, false, gEngine.Buf));
		}

		public virtual long GetGU(long index)
		{
			Debug.Assert(index >= 0 && index < GU.Length);

			return GU[index];
		}

		public virtual string GetRM(long index)
		{
			Debug.Assert(index >= 0 && index < RM.Length);

			return RM[index];
		}

		public virtual long GetRN(long index)
		{
			Debug.Assert(index >= 0 && index < RN.Length);

			return RN[index];
		}

		public Engine()
		{
			((Framework.Plugin.IEngine)this).Buf = new StringBuilder(BufSize);

			((Framework.Plugin.IEngine)this).Buf01 = new StringBuilder(BufSize);

			GU = new long[] { 0, 9, 9, 9, 9, 8, 8, 8, 8, 8, 7, 8, 9, 9, 0, 0, 0, 9, 4, 7, 9, 8, 8, 8, 8, 9, 8, 8, 9, 8, 9 };

			RM = new string[] { "", "berserker", "troll", "freak", "knight", "officer", "ninja", "spy", "barbarian" };

			RN = new long[] { 0, 66, 67, 68, 69, 70, 71, 72, 73 };

			EnableNegativeArtifactValues = true;

			PoundCharPolicy = PoundCharPolicy.None;
		}
	}
}
