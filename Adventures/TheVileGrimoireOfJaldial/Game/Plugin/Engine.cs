
// Engine.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Eamon;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Extensions;
using static TheVileGrimoireOfJaldial.Game.Plugin.Globals;

namespace TheVileGrimoireOfJaldial.Game.Plugin
{
	public class Engine : EamonRT.Game.Plugin.Engine, Framework.Plugin.IEngine
	{
		public virtual long StartHour { get; set; }

		public virtual long StartMinute { get; set; }

		public virtual long[] NonEmotingMonsterUids { get; set; }

		public virtual long EventRoll { get; set; }

		public virtual long FrequencyRoll { get; set; }

		public virtual long InitiativeMonsterUid { get; set; }

		public virtual bool EncounterSurprises { get; set; }

		public virtual bool CarrionCrawlerFlails { get; set; }

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

		public override void PrintMonsterAlive(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			// Cutlass

			if (artifact.Uid != 34)
			{
				base.PrintMonsterAlive(artifact);
			}
		}

		public override void PrintLightOut(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			if (artifact.Uid == 1)
			{
				Out.Print("{0} suddenly flickers and then goes out.", artifact.GetTheName(true));
			}
			else
			{
				base.PrintLightOut(artifact);
			}
		}

		public override void PrintArtifactIsWorth(IArtifact artifact, long goldAmount)
		{
			PushRulesetVersion(0);

			base.PrintArtifactIsWorth(artifact, goldAmount);

			PopRulesetVersion();
		}

		public override void PrintMonsterEmotes(IMonster monster, bool friendSmile = true)
		{
			Debug.Assert(monster != null);

			var rl = RollDice(1, 100, 0);

			// Giant rat

			if (monster.Uid == 1)
			{
				Out.Write("{0}{1} {2} at you.", Environment.NewLine, monster.GetTheName(true), rl > 80 ? "squeals" : rl > 50 ? "squeaks" : "hisses");
			}

			// Skeleton/Gargoyle

			else if ((monster.Uid == 3 || monster.Uid == 8) && rl > 50)
			{
				Out.Write("{0}{1} hisses at you.", Environment.NewLine, monster.GetTheName(true));
			}

			// Zombie

			else if (monster.Uid == 4 && rl > 50)
			{
				Out.Write("{0}{1} snarls at you.", Environment.NewLine, monster.GetTheName(true));
			}

			// Ghoul/Ghast

			else if (monster.Uid == 6 || monster.Uid == 7)
			{
				Out.Write("{0}{1} {2} at you.", Environment.NewLine, monster.GetTheName(true), rl > 80 ? "hisses" : rl > 50 ? "snarls" : "growls");
			}

			// Shadow/Specter/Wraith/Dark Hood/Animated suit of armor

			else if (monster.Uid == 9 || monster.Uid == 14 || monster.Uid == 16 || monster.Uid == 21 || monster.Uid == 23)
			{
				Out.Write("{0}{1} gestures at you.", Environment.NewLine, monster.GetTheName(true));
			}

			// Will-o'-the-wisp

			else if (monster.Uid == 10)
			{
				Out.Write("{0}{1} {2} at you.", Environment.NewLine, monster.GetTheName(true), rl > 50 ? "brightly flashes" : "hums");
			}

			// Pocket dragon/Giant crayfish/Giant scorpion

			else if ((monster.Uid == 24 && monster.Reaction != Friendliness.Neutral) || monster.Uid == 37 || monster.Uid == 39)
			{
				Out.Write("{0}{1} hisses at you.", Environment.NewLine, monster.GetTheName(true));
			}

			// Beholder

			else if (monster.Uid == 36 && monster.Reaction == Friendliness.Friend)
			{
				Out.Write("{0}{1} smiles back.", Environment.NewLine, monster.GetTheName(true));
			}

			// Griffin/Small griffin

			else if (monster.Uid == 40 || (monster.Uid == 41 && monster.Reaction != Friendliness.Neutral))
			{
				Out.Write("{0}{1} {2} at you.", Environment.NewLine, monster.GetTheName(true), rl > 80 ? monster.EvalPlural("screeches", "screech") : rl > 50 ? monster.EvalPlural("squawks", "squawk") : monster.EvalPlural("hisses", "hiss"));
			}

			// Jaldi'al the lich

			else if (monster.Uid == 43)
			{
				Out.Write("{0}{1} {2} at you.", Environment.NewLine, monster.GetTheName(true), rl > 80 ? "hollowly chuckles" : rl > 50 ? "gestures" : "glares");
			}

			// Jungle bekkah

			else if (monster.Uid == 44)
			{
				Out.Write("{0}{1} {2} at you.", Environment.NewLine, monster.GetTheName(true), rl > 80 ? monster.EvalPlural("roars", "roar") : rl > 50 ? monster.EvalPlural("snarls", "snarl") : monster.EvalPlural("hisses", "hiss"));
			}

			// Non-emoting monsters

			else if (NonEmotingMonsterUids.Contains(monster.Uid))
			{
				Out.Write("{0}{1} {2} not responsive.", Environment.NewLine, monster.GetTheName(true), monster.EvalPlural("is", "are"));
			}
			else
			{
				base.PrintMonsterEmotes(monster, friendSmile);
			}
		}

		public override void PrintTooManyWeapons()
		{
			PushRulesetVersion(0);

			base.PrintTooManyWeapons();

			PopRulesetVersion();
		}

		public override void PrintDeliverGoods()
		{
			PushRulesetVersion(0);

			base.PrintDeliverGoods();

			PopRulesetVersion();
		}

		public override void PrintGoodsPayment(bool goodsExist, long goldAmount)
		{
			PushRulesetVersion(0);

			base.PrintGoodsPayment(goodsExist, goldAmount);

			PopRulesetVersion();
		}

		public override void InitArtifacts()
		{
			base.InitArtifacts();

			MacroFuncs.Add(1, () =>
			{
				var largeFountainArtifact = ADB[24];

				Debug.Assert(largeFountainArtifact != null);

				return largeFountainArtifact?.DoorGate != null ? "A small staircase leads down into darkness, and a passage leads back southward.  From below, many different noises can be discerned." : "A passage leads back southward.";
			});

			MacroFuncs.Add(2, () =>
			{
				var buriedCasketArtifact = ADB[35];

				Debug.Assert(buriedCasketArtifact != null);

				return buriedCasketArtifact.InContainer.IsOpen() ? "open" : "closed";
			});

			var synonyms = new Dictionary<long, string[]>()
			{
				{ 1, new string[] { "magi-torch", "torch instruction label", "torch instructions", "torch label", "instruction label", "instruction tag", "instructions", "label", "tag" } },
				{ 3, new string[] { "small crypt door", "crypt door", "crypt", "door" } },
				{ 4, new string[] { "small tomb door", "tomb door", "tomb", "door" } },
				{ 5, new string[] { "crypt door", "door" } },
				{ 6, new string[] { "bucket" } },
				{ 8, new string[] { "bones" } },
				{ 9, new string[] { "leather bound book", "leather book", "bound book", "book" } },
				{ 10, new string[] { "gravestone", "stone" } },
				{ 11, new string[] { "dragon's treasure", "dragon's hoard", "treasure hoard", "treasure", "hoard" } },
				{ 12, new string[] { "goblet", "cup" } },
				{ 13, new string[] { "chest" } },
				{ 14, new string[] { "old map", "map" } },
				{ 15, new string[] { "ale bottle", "bottle", "ale" } },
				{ 16, new string[] { "gauntlets", "gauntlet" } },
				{ 17, new string[] { "wine", "cask" } },
				{ 18, new string[] { "beholder's treasure", "beholder's hoard", "treasure hoard", "treasure", "hoard" } },
				{ 19, new string[] { "cloak" } },
				{ 20, new string[] { "pieces" } },
				{ 21, new string[] { "pouch containing stones", "pouch with stones", "pouch", "stones" } },
				{ 22, new string[] { "egg" } },
				{ 23, new string[] { "nest" } },
				{ 24, new string[] { "fountain", "basin", "grotesque face", "face" } },
				{ 25, new string[] { "gold pile", "gold coins", "gold", "coins" } },
				{ 26, new string[] { "wood throne", "throne" } },
				{ 27, new string[] { "book", "manuscript" } },
				{ 28, new string[] { "rod" } },
				{ 29, new string[] { "mace" } },
				{ 31, new string[] { "sword" } },
				{ 32, new string[] { "dagger" } },
				{ 33, new string[] { "parchment" } },
				{ 34, new string[] { "sword" } },
				{ 35, new string[] { "buried coffin", "casket", "coffin" } },
				{ 36, new string[] { "skeleton", "bones" } },
				{ 37, new string[] { "cross" } },
				{ 38, new string[] { "coil" } },
				{ 43, new string[] { "dead tree" } },
				{ 44, new string[] { "dead forest nettles", "dead blood nettles", "dead bloodnettles", "dead nettles" } },
				{ 45, new string[] { "cloak" } },
				{ 46, new string[] { "dead whipweed", "dead weed" } },
				{ 48, new string[] { "dead dragon" } },
				{ 49, new string[] { "dead crimson amoeba", "dead amoeba", "wine" } },
				{ 50, new string[] { "dead saber claws" } },
				{ 51, new string[] { "jade dust", "jade splinters", "jade chunks", "debris" } },
				{ 53, new string[] { "dead crayfish" } },
				{ 54, new string[] { "dead scorpion" } },
				{ 56, new string[] { "dead griffin cubs", "dead griffin babies", "dead baby griffins", "dead griffin baby" } },
				{ 57, new string[] { "Jaldial the lich's remains", "Jaldi'al's remains", "Jaldial's remains", "lich's remains", "lich remains", "remains", "dead Jaldi'al", "dead Jaldial", "dead lich" } },
				{ 58, new string[] { "dead jungle cats", "dead bekkahs", "dead cats" } },
				{ 59, new string[] { "dead Reginald" } },
				{ 60, new string[] { "dead Dubro" } },
				{ 61, new string[] { "dead Joque" } },
				{ 62, new string[] { "dead Trevor" } },
				{ 63, new string[] { "stone coffin", "coffin" } },
			};

			foreach (var synonym in synonyms)
			{
				CreateArtifactSynonyms(synonym.Key, synonym.Value);
			}

			var torchArtifact = ADB[1];

			Debug.Assert(torchArtifact != null);

			gGameState.TorchRounds = RollDice(1, 81, 399);

			torchArtifact.Value = (long)Math.Round(5.0 * ((double)gGameState.TorchRounds / 50.0));

			torchArtifact.LightSource.Field1 = gGameState.TorchRounds;

			var lanternArtifact = ADB[39];

			Debug.Assert(lanternArtifact != null);

			lanternArtifact.LightSource.Field1 = RollDice(1, 81, 299);
		}

		public override void InitMonsters()
		{
			base.InitMonsters();

			MacroFuncs.Add(4, () =>
			{
				var reginaldMonster = MDB[46];

				return GetMonsterWeaponName(reginaldMonster);
			});

			MacroFuncs.Add(5, () =>
			{
				var dubroMonster = MDB[47];

				return GetMonsterWeaponName(dubroMonster);
			});

			MacroFuncs.Add(6, () =>
			{
				var joqueMonster = MDB[48];

				return GetMonsterWeaponName(joqueMonster);
			});

			MacroFuncs.Add(7, () =>
			{
				var trevorMonster = MDB[49];

				return GetMonsterWeaponName(trevorMonster);
			});

			var synonyms = new Dictionary<long, string[]>()
			{
				{ 1, new string[] { "rat" } },
				{ 10, new string[] { "will-o'-wisp", "will o' the wisp", "will o' wisp", "wisp" } },
				{ 11, new string[] { "crawler" } },
				{ 12, new string[] { "mound" } },
				{ 13, new string[] { "jelly" } },
				{ 20, new string[] { "bloodnettle clump", "bloodnettle", "clump" } },
				{ 21, new string[] { "hood" } },
				{ 22, new string[] { "weed" } },
				{ 23, new string[] { "animated armor", "suit of armor", "armor" } },
				{ 24, new string[] { "dragon" } },
				{ 25, new string[] { "amoeba" } },
				{ 31, new string[] { "possessed sword", "cutlass", "sword" } },
				{ 32, new string[] { "statues" } },
				{ 37, new string[] { "crayfish" } },
				{ 38, new string[] { "weird" } },
				{ 39, new string[] { "scorpion" } },
				{ 41, new string[] { "baby griffins", "griffin cubs", "griffins", "babies", "cubs" } },
				{ 43, new string[] { "jaldial", "lich" } },
				{ 44, new string[] { "jungle cats", "bekkahs", "cats" } },
				{ 50, new string[] { "genie" } },
			};

			foreach (var synonym in synonyms)
			{
				CreateMonsterSynonyms(synonym.Key, synonym.Value);
			}
		}

		public override void ResetMonsterStats(IMonster monster)
		{
			Debug.Assert(monster != null && monster.IsCharacterMonster());

			base.ResetMonsterStats(monster);

			// Ensure monster not paralyzed or clumsy

			gGameState.ParalyzedTargets.Remove(monster.Uid);

			gGameState.ClumsyTargets.Remove(monster.Uid);
		}

		public override void MonsterGetsAggravated(IMonster monster, bool printFinalNewLine = true)
		{
			PushRulesetVersion(0);

			base.MonsterGetsAggravated(monster, printFinalNewLine);

			PopRulesetVersion();
		}

		public override void MonsterDies(IMonster actorMonster, IMonster dobjMonster)
		{
			Debug.Assert(dobjMonster != null);

			var room = dobjMonster.GetInRoom() as Framework.IRoom;

			Debug.Assert(room != null);

			// Wandering monsters

			if (dobjMonster.Uid >= 1 && dobjMonster.Uid <= 17)
			{
				if (room.IsLit())
				{
					Out.Print("{0}As {1} dies, its body {2}.", Environment.NewLine, dobjMonster.GetTheName(), room.IsCryptRoom() ? "is enveloped in a cloud of sinister black smoke" : "dissolves and is absorbed into the ground");
				}
			}

			// Possessed cutlass

			else if (dobjMonster.Uid == 31)
			{
				var cutlassArtifact = ADB[34];

				Debug.Assert(cutlassArtifact != null);

				cutlassArtifact.SetArtifactCategoryCount(1);
			}

			// Giant crayfish

			else if (dobjMonster.Uid == 37)
			{
				gGameState.GiantCrayfishKilled = true;
			}

			// Water weird

			else if (dobjMonster.Uid == 38)
			{
				gGameState.WaterWeirdKilled = true;
			}

			// Efreeti

			else if (dobjMonster.Uid == 50)
			{
				gGameState.EfreetiKilled = true;
			}

			gGameState.ParalyzedTargets.Remove(dobjMonster.Uid);

			gGameState.ClumsyTargets.Remove(dobjMonster.Uid);

			base.MonsterDies(actorMonster, dobjMonster);
		}

		public override IList<IArtifact> GetReadyableWeaponList(IMonster monster)
		{
			IList<IArtifact> result;

			PushRulesetVersion(0);

			result = base.GetReadyableWeaponList(monster);

			PopRulesetVersion();

			return result;
		}

		public override IList<IMonster> GetHostileMonsterList(IMonster monster)
		{
			Debug.Assert(monster != null);

			var monsterList = base.GetHostileMonsterList(monster);

			// Bloodnettle always chooses same victim

			if (monster.Uid == 20 && gGameState.BloodnettleVictimUid != 0)
			{
				if (monsterList.FirstOrDefault(m => m.Uid == gGameState.BloodnettleVictimUid) != null)
				{
					monsterList = monsterList.Where(m => m.Uid == gGameState.BloodnettleVictimUid).ToList();
				}
				else
				{
					gGameState.BloodnettleVictimUid = 0;
				}
			}

			return monsterList;
		}

		public override IList<IMonster> GetEmotingMonsterList(IRoom room, IMonster monster, bool friendSmile = true)
		{
			// Some monsters don't emote

			return base.GetEmotingMonsterList(room, monster, friendSmile).Where(m => !NonEmotingMonsterUids.Contains(m.Uid) && (friendSmile || !gGameState.ParalyzedTargets.ContainsKey(m.Uid))).ToList();
		}

		public override void CheckToExtinguishLightSource()
		{
			// do nothing
		}

		public override void MoveMonsters(params Func<IMonster, bool>[] whereClauseFuncs)
		{
			base.MoveMonsters(m => !m.IsCharacterMonster() && (m.Cast<Framework.IMonster>().Seen02 || m.Reaction == Friendliness.Friend) && m.Location == GameState.R3);
		}

		public Engine()
		{
			// Accommodate oversized Effect Descs

			EffDescLen = 1024;

			// Restrict number of save game slots

			NumSaveSlots = 1;

			StartHour = 6;

			StartMinute = 45;

			NonEmotingMonsterUids = new long[] { 13, 18, 19, 20, 22, 25, 31, 32, 38 };

			PushRulesetVersion(5);

			MacroFuncs.Add(3, () =>
			{
				var result = "to bright sunlight";

				if (GameState != null)
				{
					var room = RDB[17] as Framework.IRoom;

					Debug.Assert(room != null);

					if (gGameState.IsNightTime())
					{
						result = "into the night";
					}
					else if (room.GetWeatherIntensity() > 1)
					{
						result = "to daylight";
					}
				}

				return result;
			});

			MacroFuncs.Add(8, () =>
			{
				var result = "You are at a bend in the passage.  To the south, you see a small chamber of carved stone, and eastward the corridor continues.";

				if (GameState != null && !gGameState.GetSecretDoor(1))
				{
					result = "You are at a dead-end in the passage.  Eastward the corridor continues.";
				}

				return result;
			});

			MacroFuncs.Add(9, () =>
			{
				var result = "  You may go north or east.";

				if (GameState != null)
				{
					if (gGameState.GetSecretDoor(2) && !gGameState.GetSecretDoor(4))
					{
						result = "  You may go north.";
					}
					else if (!gGameState.GetSecretDoor(2) && gGameState.GetSecretDoor(4))
					{
						result = "  You may go east.";
					}
				}

				return result;
			});

			MacroFuncs.Add(10, () =>
			{
				var result = " between the two openings (to the east and south)";

				if (GameState != null && !gGameState.GetSecretDoor(3))
				{
					result = "";
				}

				return result;
			});
		}
	}
}
