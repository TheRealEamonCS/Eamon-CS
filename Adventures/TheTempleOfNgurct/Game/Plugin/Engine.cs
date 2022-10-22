
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
using EamonRT.Framework.Components;
using EamonRT.Framework.States;
using static TheTempleOfNgurct.Game.Plugin.Globals;

namespace TheTempleOfNgurct.Game.Plugin
{
	public class Engine : EamonRT.Game.Plugin.Engine, Framework.Plugin.IEngine
	{
		public virtual bool FireDamage { get; set; }

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

			// Cobra

			if (monster.Uid == 52)
			{
				Out.Write("{0}{1} hisses at you.", Environment.NewLine, monster.GetTheName(true));
			}
			else
			{
				base.PrintMonsterEmotes(monster, friendSmile);
			}
		}

		public override void InitArtifacts()
		{
			base.InitArtifacts();

			MacroFuncs.Add(2, () =>
			{
				var cellDoorArtifact = ADB[87];

				Debug.Assert(cellDoorArtifact != null);

				var ac = cellDoorArtifact.DoorGate;

				Debug.Assert(ac != null);

				return ac.GetKeyUid() > 0 ? "locked" : "unlocked";
			});

			MacroFuncs.Add(3, () =>
			{
				var cellDoorArtifact = ADB[88];

				Debug.Assert(cellDoorArtifact != null);

				var ac = cellDoorArtifact.DoorGate;

				Debug.Assert(ac != null);

				return ac.GetKeyUid() > 0 ? "locked" : "unlocked";
			});

			MacroFuncs.Add(4, () =>
			{
				var result = "floor";

				var characterRoom = GameState != null && GameState.Ro > 0 ? RDB[GameState.Ro] : null;

				if (characterRoom != null && characterRoom.Type == RoomType.Outdoors)
				{
					result = "ground";
				}

				return result;
			});

			var synonyms = new Dictionary<long, string[]>()
			{
				{ 11, new string[] { "sword" } },
				{ 34, new string[] { "axe" } },
				{ 38, new string[] { "sword" } },
				{ 40, new string[] { "sword" } },
				{ 41, new string[] { "scimitar" } },
				{ 42, new string[] { "sword" } },
				{ 43, new string[] { "pan" } },
				{ 47, new string[] { "ingots", "ingot" } },
				{ 51, new string[] { "healing potion", "potion" } },
				{ 52, new string[] { "human blood", "blood", "potion" } },
				{ 53, new string[] { "sulphuric acid", "acid", "h2so4", "potion" } },
				{ 56, new string[] { "bars", "bar" } },
				{ 58, new string[] { "stones", "stone" } },
				{ 62, new string[] { "healing potion", "potion" } },
				{ 63, new string[] { "wand" } },
				{ 68, new string[] { "pig" } },
				{ 69, new string[] { "bottle", "label" } },
				{ 70, new string[] { "key" } },
				{ 72, new string[] { "keys" } },
				{ 73, new string[] { "key" } },
				{ 74, new string[] { "dagger" } },
				{ 75, new string[] { "robes", "robe" } },
				{ 76, new string[] { "blanks" } },
				{ 78, new string[] { "dagger" } },
				{ 80, new string[] { "ring" } },
				{ 81, new string[] { "bound slave", "bound girl", "slave girl", "girl" } },
				{ 82, new string[] { "hieroglyphs", "glyphs", "inscriptions", "wall" } },
				{ 83, new string[] { "door" } },
				{ 84, new string[] { "door" } },
				{ 85, new string[] { "door" } },
				{ 86, new string[] { "door" } },
				{ 87, new string[] { "door" } },
				{ 88, new string[] { "door" } },
			};

			foreach (var synonym in synonyms)
			{
				CreateArtifactSynonyms(synonym.Key, synonym.Value);
			}

			var artTypes = new ArtifactType[] { ArtifactType.Drinkable, ArtifactType.Edible };

			var artUids = new long[] { 51, 62, 68 };

			// Sets up potion/roast pig random heal amounts

			for (var i = 0; i < artUids.Length; i++)
			{
				var healingArtifact = ADB[artUids[i]];

				Debug.Assert(healingArtifact != null);

				var ac = healingArtifact.GetArtifactCategory(artTypes);

				Debug.Assert(ac != null);

				ac.Field1 = RollDice(1, 10, 0);
			}

			// Places fireball wand and ring of regeneration

			var wandArtifact = ADB[63];

			Debug.Assert(wandArtifact != null);

			wandArtifact.Location = RollDice(1, 28, 28);

			var ringArtifact = ADB[64];

			Debug.Assert(ringArtifact != null);

			ringArtifact.Location = wandArtifact.Location;
		}

		public override void InitMonsters()
		{
			base.InitMonsters();

			// Note: since these macro functions are installed by InitMonsters, we can assume this is
			// being executed by EamonRT so there is no need to overharden the functions.  But, it is
			// still recommended!

			MacroFuncs.Add(1, () =>
			{
				return gCharMonster != null ? gCharMonster.EvalGender(" sir", " madam", "") : "";
			});

			MacroFuncs.Add(5, () =>
			{
				var result = "room";

				var characterRoom = GameState != null && GameState.Ro > 0 ? RDB[GameState.Ro] : null;

				if (characterRoom != null && characterRoom.Type == RoomType.Outdoors)
				{
					result = "area";
				}

				return result;
			});

			// Sets up random monster rooms

			for (var i = 7; i <= 11; i++)
			{
				var randomMonster = MDB[i];

				Debug.Assert(randomMonster != null);

				while (true)
				{
					randomMonster.Location = RollDice(1, 56, 3);

					if (randomMonster.Location != 58)
					{
						break;
					}
				}
			}
		}

		public override void ResetMonsterStats(IMonster monster)
		{
			Debug.Assert(monster != null && monster.IsCharacterMonster());

			monster.Agility = Character.GetStat(Stat.Agility);

			GameState.Speed = 0;
		}

		public override void ConvertToCarriedInventory(IList<IArtifact> weaponList)
		{
			// Convert fireball wand into artifact type gold

			var wandArtifact = ADB[63];

			Debug.Assert(wandArtifact != null);

			ConvertWeaponToGoldOrTreasure(wandArtifact, true);

			base.ConvertToCarriedInventory(weaponList);
		}

		public override void MonsterDies(IMonster ActorMonster, IMonster DobjMonster)
		{
			Debug.Assert(DobjMonster != null);

			if (DobjMonster.Uid == 30)
			{
				gGameState.KeyRingRoomUid = DobjMonster.Location;
			}
			else if (DobjMonster.Uid == 56)
			{
				gGameState.AlkandaKilled = true;
			}

			var dmgTaken = DobjMonster.DmgTaken;

			base.MonsterDies(ActorMonster, DobjMonster);

			DobjMonster.DmgTaken = dmgTaken;
		}

		public virtual IList<IMonster> GetTrapMonsterList(long numMonsters, long roomUid)
		{
			var monsterList = GetRandomMonsterList(numMonsters, m => m.IsCharacterMonster() || (m.Seen && m.IsInRoomUid(roomUid)));

			Debug.Assert(monsterList != null);

			return monsterList;
		}

		public virtual void ApplyTrapDamage(Action<IState> setNextStateFunc, IMonster monster, long numDice, long numSides, bool omitArmor)
		{
			Debug.Assert(setNextStateFunc != null && monster != null);

			var combatComponent = CreateInstance<ICombatComponent>(x =>
			{
				x.SetNextStateFunc = setNextStateFunc;

				x.ActorRoom = monster.GetInRoom();

				x.Dobj = monster;

				x.OmitArmor = omitArmor;
			});

			combatComponent.ExecuteCalculateDamage(numDice, numSides);
		}

		public virtual bool GetWanderingMonster()
		{
			var found = false;

			while (gGameState.DwLoopCounter <= 15)
			{
				if (gGameState.WanderingMonster > 26)
				{
					gGameState.WanderingMonster = 12;
				}

				var wanderingMonster = MDB[gGameState.WanderingMonster];

				Debug.Assert(wanderingMonster != null);

				if (wanderingMonster.DmgTaken == 0)
				{
					wanderingMonster.Location = GameState.Ro;

					gGameState.WanderingMonster++;

					found = true;

					break;
				}

				gGameState.WanderingMonster++;

				gGameState.DwLoopCounter++;
			}

			return found;
		}

		public Engine()
		{
			ToughDesc = string.Format("Monsters usually fall into one of the following categories, but it is possible to create hybrids that are weak in some areas and strong in others:{0}{0}Weak Monsters - wimps and small creatures like rats, kobolds, etc.{0}Medium Monsters - petty thugs, orcs, goblins, etc.{0}Tough Monsters - giants, trolls, highly skilled warriors, etc.{0}Exceptional Monsters - dragons, demons, special villians, etc.", Environment.NewLine);

			CourageDesc = string.Format("Courage works as follows:{0}{0}1-100% - the chance the Monster won't flee combat and/or follow a fleeing player (if enemy).", Environment.NewLine);

			PushRulesetVersion(5);

			MacroFuncs.Add(6, () =>
			{
				var result = "(the specifics are left to your imagination)";

				if (GameState != null && GameState.MatureContent)
				{
					result = Encoding.UTF8.GetString(Convert.FromBase64String("LS0gZWF0aW5nIGh1bWFuIGJhYmllcywgcmFwaW5nIHdvbWVuLCBhbmQgc28gZm9ydGg="));
				}

				return result;
			});

			MacroFuncs.Add(7, () =>
			{
				var result = "You fool!  You just climbed down into the excrement duct!";

				if (GameState != null && GameState.MatureContent)
				{
					result = Encoding.UTF8.GetString(Convert.FromBase64String("WW91IHN0dXBpZCBqZXJrISAgWW91IGp1c3QgY2xpbWJlZCBkb3duIGludG8gdGhlIHNoaXQgaG9sZSE="));
				}

				return result;
			});

			PoundCharPolicy = PoundCharPolicy.None;
		}
	}
}
