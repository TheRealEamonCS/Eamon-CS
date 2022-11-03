
// Engine.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using Eamon;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using static WrenholdsSecretVigil.Game.Plugin.Globals;

namespace WrenholdsSecretVigil.Game.Plugin
{
	public class Engine : EamonRT.Game.Plugin.Engine, Framework.Plugin.IEngine
	{
		public virtual bool MonsterCurses { get; set; }

		public virtual bool DeviceOpened { get; set; }

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

		public override void PrintMonsterEmotes(IMonster monster, bool friendSmile = true)
		{
			Debug.Assert(monster != null);

			// Large death dog

			if (monster.Uid == 1 && monster.Reaction == Friendliness.Friend)
			{
				Out.Write("{0}{1} wags its tail.", Environment.NewLine, monster.GetTheName(true));
			}
			else
			{
				base.PrintMonsterEmotes(monster, friendSmile);
			}
		}

		public override void AddUniqueCharsToArtifactAndMonsterNames()
		{
			base.AddUniqueCharsToArtifactAndMonsterNames();

			var slimeArtifact = ADB[25];     // Slime #2

			Debug.Assert(slimeArtifact != null);

			slimeArtifact.Name = slimeArtifact.Name.TrimEnd('#');

			var deviceArtifact = ADB[49];         // Large Green Device #2

			Debug.Assert(deviceArtifact != null);

			deviceArtifact.Name = deviceArtifact.Name.TrimEnd('#');

			var deadGuardArtifact = ADB[74];         // Dead Drow Guard #2

			Debug.Assert(deadGuardArtifact != null);

			deadGuardArtifact.Name = deadGuardArtifact.Name.TrimEnd('#');
		}

		public override void InitArtifacts()
		{
			base.InitArtifacts();

			MacroFuncs.Add(2, () =>
			{
				var goldCurtainArtifact = ADB[40];

				Debug.Assert(goldCurtainArtifact != null);

				return goldCurtainArtifact.DoorGate != null ? " a drawn gold curtain covers a doorway to the" : " another doorway is";
			});

			MacroFuncs.Add(3, () =>
			{
				var goldCurtainArtifact = ADB[40];

				Debug.Assert(goldCurtainArtifact != null);

				return goldCurtainArtifact.DoorGate != null ? "  A gold curtain is open to the north." : "";
			});

			var synonyms = new Dictionary<long, string[]>()
			{
				{ 1, new string[] { "small wooden shovel", "small scoop shovel", "wooden scoop shovel", "small shovel", "wooden shovel", "scoop shovel", "shovel" } },
				{ 2, new string[] { "adventurer" } },
				{ 3, new string[] { "cloth bound diary", "cloth diary", "bound diary", "diary" } },
				{ 4, new string[] { "life orb", "orb" } },
				{ 5, new string[] { "cube" } },
				{ 6, new string[] { "cups" } },
				{ 7, new string[] { "large limb", "tree limb", "limb" } },
				{ 8, new string[] { "villager in chains", "chained villager" } },
				{ 9, new string[] { "jewel bag", "jewels" } },
				{ 11, new string[] { "chained merchant", "dwarf merchant", "merchant" } },
				{ 12, new string[] { "key" } },
				{ 13, new string[] { "axe" } },
				{ 14, new string[] { "shortsword", "sword" } },
				{ 15, new string[] { "rabbit" } },
				{ 16, new string[] { "small box", "wooden box", "box" } },
				{ 17, new string[] { "rock" } },
				{ 18, new string[] { "ring" } },
				{ 19, new string[] { "pea sized crystal ball", "pea-sized ball", "pea sized ball", "crystal ball", "ball" } },
				{ 20, new string[] { "box" } },
				{ 21, new string[] { "key" } },
				{ 22, new string[] { "key" } },
				{ 23, new string[] { "coins" } },
				{ 26, new string[] { "straw mattresses", "mattresses" } },
				{ 27, new string[] { "key" } },
				{ 29, new string[] { "bones" } },
				{ 31, new string[] { "shark", "fish" } },
				{ 32, new string[] { "cleaver" } },
				{ 33, new string[] { "weapon racks", "weapons" } },
				{ 34, new string[] { "key" } },
				{ 35, new string[] { "wine barrels", "wines" } },
				{ 36, new string[] { "cups" } },
				{ 37, new string[] { "ivory safe", "door", "safe" } },
				{ 38, new string[] { "goebel" } },
				{ 39, new string[] { "door" } },
				{ 40, new string[] { "curtain" } },
				{ 41, new string[] { "book" } },
				{ 42, new string[] { "bones" } },
				{ 43, new string[] { "pedestal" } },
				{ 44, new string[] { "large device", "green device", "device" } },
				{ 45, new string[] { "key" } },
				{ 46, new string[] { "animals" } },
				{ 47, new string[] { "key" } },
				{ 49, new string[] { "large device", "green device", "device" } },
				{ 50, new string[] { "magic bow", "elven bow", "bow" } },
				{ 51, new string[] { "large dead death dog", "dead death dog", "dead dog" } },
				{ 52, new string[] { "smoldering fluids", "ashen fluids", "ash", "fluids" } },
				{ 53, new string[] { "ugly dead one-eyed ogre", "ugly dead one eyed ogre", "ugly dead ogre", "dead one-eyed ogre", "dead one eyed ogre", "dead ogre" } },
				{ 54, new string[] { "body" } },
				{ 55, new string[] { "body" } },
				{ 56, new string[] { "dead rat" } },
				{ 57, new string[] { "dead rat" } },
				{ 58, new string[] { "dead zombie" } },
				{ 59, new string[] { "dead zombie" } },
				{ 60, new string[] { "dead zombie" } },
				{ 62, new string[] { "dead patrol" } },
				{ 63, new string[] { "dead zombie" } },
				{ 64, new string[] { "dead soldiers" } },
				{ 65, new string[] { "dead citizens" } },
				{ 66, new string[] { "dead guards" } },
				{ 67, new string[] { "dead guard" } },
				{ 68, new string[] { "dead chef" } },
				{ 69, new string[] { "dead old woman", "dead drow woman", "dead woman" } },
				{ 70, new string[] { "dead diners" } },
				{ 71, new string[] { "dead drows" } },
				{ 72, new string[] { "dead bartender", "dead barkeeper" } },
				{ 73, new string[] { "dead patron" } },
				{ 74, new string[] { "dead guard" } },
				{ 75, new string[] { "dead priest" } },
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
				{ 1, new string[] { "large dog", "death dog", "dog" } },
				{ 2, new string[] { "sorcerer", "wizard", "mage" } },
				{ 3, new string[] { "one eyed ogre", "ogre" } },
				{ 4, new string[] { "breanok" } },
				{ 5, new string[] { "pantwell" } },
				{ 6, new string[] { "rat" } },
				{ 7, new string[] { "rat" } },
				{ 8, new string[] { "black haired zombie", "zombie" } },
				{ 9, new string[] { "leather clad zombie", "zombie" } },
				{ 10, new string[] { "zombie" } },
				{ 11, new string[] { "chain-mail zombie", "chain mail zombie", "chainmail zombie" } },
				{ 12, new string[] { "zombies" } },
				{ 13, new string[] { "zombie" } },
				{ 14, new string[] { "soldiers", "drows" } },
				{ 15, new string[] { "citizens", "drows" } },
				{ 16, new string[] { "guard" } },
				{ 17, new string[] { "guard" } },
				{ 18, new string[] { "chef" } },
				{ 19, new string[] { "old woman", "drow woman", "woman" } },
				{ 20, new string[] { "diners", "drows" } },
				{ 21, new string[] { "drows" } },
				{ 22, new string[] { "bartender" } },
				{ 23, new string[] { "patron" } },
				{ 24, new string[] { "guard" } },
				{ 25, new string[] { "drow priest", "priest" } },
				{ 26, new string[] { "animals" } },
			};

			foreach (var synonym in synonyms)
			{
				CreateMonsterSynonyms(synonym.Key, synonym.Value);
			}
		}

		public override void ConvertToCarriedInventory(IList<IArtifact> weaponList)
		{
			// Convert large tree limb into artifact type treasure

			var treeLimbArtifact = ADB[7];

			Debug.Assert(treeLimbArtifact != null);

			ConvertWeaponToGoldOrTreasure(treeLimbArtifact, false);

			base.ConvertToCarriedInventory(weaponList);
		}

		public override IArtifact GetBlockedDirectionArtifact(long ro, long r2, Direction dir)
		{
			IArtifact artifact = null;

			var slimeArtifact1 = ADB[24];

			Debug.Assert(slimeArtifact1 != null);

			var slimeArtifact2 = ADB[25];

			Debug.Assert(slimeArtifact2 != null);

			var largeRockArtifact = ADB[17];

			Debug.Assert(largeRockArtifact != null);

			var lifeOrbArtifact = ADB[4];

			Debug.Assert(lifeOrbArtifact != null);

			var ac = largeRockArtifact.DoorGate;

			Debug.Assert(ac != null);

			// If slime in room, can't move past it

			if ((r2 == 21 || r2 == 44) && (ro == 21 || ro == 44) && (slimeArtifact1.IsInRoomUid(ro) || slimeArtifact2.IsInRoomUid(ro)))
			{
				artifact = slimeArtifact1;
			}

			// If rock in room, can't move past it

			else if (r2 == 19 && ro == 18 && !largeRockArtifact.IsInLimbo() && ac.GetKeyUid() != -2)
			{
				artifact = largeRockArtifact;
			}

			// And if room == 15, can't get past orb

			else if (ro == 15 && dir == Direction.South)
			{
				artifact = lifeOrbArtifact;
			}

			return artifact;
		}

		public virtual void PrintMonsterCurse(IMonster monster, long effectUid)
		{
			Debug.Assert(monster != null);

			var rl = RollDice(1, 100, 0);

			// Say each curse only once

			if (rl < 41 && monster.Reaction == Friendliness.Enemy && monster.HasCarriedInventory() && !gGameState.GetMonsterCurse(effectUid - 7))
			{
				var effect = EDB[effectUid];

				Debug.Assert(effect != null);

				Out.Print("{0} says, {1}", monster.GetTheName(true, true, false, false, true, Buf), effect.Desc);

				gGameState.SetMonsterCurse(effectUid - 7, true);
			}
		}

		public Engine()
		{
			// Note: this is an example of a macro function that will be used by both EamonDD and EamonRT in macro
			// resolution.  It is hardened to check for the existance of Character, which will only exist in
			// EamonRT (the GameState object, though not used here, is another thing to always check for).

			MacroFuncs.Add(1, () =>
			{
				return Character != null ? Character.Name : UnknownName;
			});
		}
	}
}
