
// Engine.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using Eamon;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using static StrongholdOfKahrDur.Game.Plugin.Globals;

namespace StrongholdOfKahrDur.Game.Plugin
{
	public class Engine : EamonRT.Game.Plugin.Engine, Framework.Plugin.IEngine
	{
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

			Out.Write("{0}{1} {2}{3} at you.",
				Environment.NewLine,
				monster.GetTheName(true),
				monster.EvalReaction("growl", "look", friendSmile ? "smile" : "wave"),
				monster.EvalPlural("s", ""));
		}

		public override void PrintTooManyWeapons()
		{
			Out.Print("As you enter the Main Hall, Lord William Crankhandle approaches you and says, \"You have too many weapons to keep them all, four is the legal limit.\"");
		}

		public override void PrintDeliverGoods()
		{
			Out.Print("You sell your goods to {0}the local buyer of treasure (under the sign of 3 balls).  He examines your items and pays you what they are worth.", Character.Name.Equals("tom zucchini", StringComparison.OrdinalIgnoreCase) ? "" : "Tom Zucchini, ");
		}

		public override void AddUniqueCharsToArtifactAndMonsterNames()
		{
			base.AddUniqueCharsToArtifactAndMonsterNames();

			var secretDoorArtifact = ADB[10];     // Secret door #2

			Debug.Assert(secretDoorArtifact != null);

			secretDoorArtifact.Name = secretDoorArtifact.Name.TrimEnd('#');
		}

		public override void InitArtifacts()
		{
			base.InitArtifacts();

			var synonyms = new Dictionary<long, string[]>()
			{
				{ 2, new string[] { "glasses" } },
				{ 4, new string[] { "door" } },
				{ 5, new string[] { "potion" } },
				{ 7, new string[] { "east gate", "east door", "portcullis", "gate", "door" } },
				{ 8, new string[] { "west gate", "west door", "portcullis", "gate", "door" } },
				{ 9, new string[] { "whitestone", "karamir" } },
				{ 10, new string[] { "door" } },
				{ 11, new string[] { "shelf" } },
				{ 12, new string[] { "gemstones", "stones" } },
				{ 13, new string[] { "coins" } },
				{ 14, new string[] { "boots", "levitation" } },
				{ 15, new string[] { "pouch" } },
				{ 16, new string[] { "key" } },
				{ 17, new string[] { "door" } },
				{ 18, new string[] { "amulet", "courage" } },
				{ 19, new string[] { "phial", "dragon", "spice" } },
				{ 20, new string[] { "weed" } },
				{ 21, new string[] { "stone" } },
				{ 22, new string[] { "powder" } },
				{ 23, new string[] { "scroll" } },
				{ 24, new string[] { "kettle", "pot" } },
				{ 25, new string[] { "wizard's helmet", "wizard helm", "wizard helmet", "helmet" } },
				{ 26, new string[] { "mirabelle", "niece", "woman", "girl" } },
				{ 27, new string[] { "sword" } },
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
				{ 1, new string[] { "guard" } },
				{ 2, new string[] { "golem" } },
				{ 3, new string[] { "golem" } },
				{ 6, new string[] { "hydra" } },
				{ 8, new string[] { "evil ent", "tree ent", "ent" } },
				{ 9, new string[] { "evil ent", "tree ent", "ent" } },
				{ 10, new string[] { "evil ent", "tree ent", "ent" } },
				{ 11, new string[] { "guard" } },
				{ 12, new string[] { "guard" } },
				{ 13, new string[] { "guard" } },
				{ 14, new string[] { "jelly", "blob" } },
				{ 15, new string[] { "sharruk", "lich" } },
				{ 16, new string[] { "guard" } },
				{ 17, new string[] { "chieftain" } },
				{ 18, new string[] { "soldier" } },
				{ 19, new string[] { "scout" } },
				{ 20, new string[] { "brother" } },
				{ 22, new string[] { "wizard" } },
				{ 23, new string[] { "demonic snake", "serpent", "snake" } },
				{ 24, new string[] { "hound", "dog" } },
				{ 25, new string[] { "demon" } },
				{ 26, new string[] { "mirabelle", "niece", "girl" } },
			};

			foreach (var synonym in synonyms)
			{
				CreateMonsterSynonyms(synonym.Key, synonym.Value);
			}
		}

		public override void MonsterGetsAggravated(IMonster monster, bool printFinalNewLine = true)
		{
			base.MonsterGetsAggravated(monster, printFinalNewLine);

			// If player aggravates Mirabelle, Jollifrud gets aggravated and vice versa

			if (monster.Uid == 20 || monster.Uid == 26)
			{
				var monster01 = MDB[monster.Uid == 20 ? 26 : 20];

				Debug.Assert(monster01 != null);

				base.MonsterGetsAggravated(monster01, printFinalNewLine);
			}
		}

		public override void MonsterDies(IMonster actorMonster, IMonster dobjMonster)
		{
			base.MonsterDies(actorMonster, dobjMonster);

			if (actorMonster != null && actorMonster.Uid == GameState.Cm)
			{
				// If player kills Mirabelle, Jollifrud gets angry and vice versa

				if (dobjMonster.Uid == 20 || dobjMonster.Uid == 26)
				{
					var monster = MDB[dobjMonster.Uid == 20 ? 26 : 20];

					Debug.Assert(monster != null);

					if (monster.Reaction > Friendliness.Enemy)
					{
						Out.WriteLine();
					}

					while (monster.Reaction > Friendliness.Enemy)
					{
						base.MonsterGetsAggravated(monster, false);
					}
				}
			}
		}

		public override void MoveMonsters(params Func<IMonster, bool>[] whereClauseFuncs)
		{
			// Monsters can't move in/out of pit w/o magical help

			var pitMove = GameState.R3 == 84 && GameState.Ro == 94;

			if (!pitMove)
			{
				pitMove = GameState.R3 == 94 && GameState.Ro == 84;
			}

			if (!pitMove)
			{
				base.MoveMonsters(whereClauseFuncs);
			}
		}

		public virtual bool SpellReagentsInCauldron(IArtifact cauldronArtifact)
		{
			Debug.Assert(cauldronArtifact != null);

			var dragonSpiceArtifact = ADB[19];

			Debug.Assert(dragonSpiceArtifact != null);

			var kingswortWeedArtifact = ADB[20];

			Debug.Assert(kingswortWeedArtifact != null);

			var rubyStoneArtifact = ADB[21];

			Debug.Assert(rubyStoneArtifact != null);

			var residuumPowderArtifact = ADB[22];

			Debug.Assert(residuumPowderArtifact != null);

			return dragonSpiceArtifact.IsCarriedByContainer(cauldronArtifact) && kingswortWeedArtifact.IsCarriedByContainer(cauldronArtifact) && rubyStoneArtifact.IsCarriedByContainer(cauldronArtifact) && residuumPowderArtifact.IsCarriedByContainer(cauldronArtifact);
		}

		public virtual void SummonMonster(IRoom room, long monsterUid)
		{
			Debug.Assert(room != null);

			// Necromancer summons other monsters...

			var monster = MDB[monsterUid];

			Debug.Assert(monster != null);

			// Dead, hasn't been previously summoned, or is somewhere else

			if (monster.IsInLimbo() || !monster.IsInRoom(room))
			{
				// Preset 'Seen' flag for smoother effect

				monster.Seen = true;

				// Put monster in room

				monster.SetInRoom(room);

				// Reset group size to one

				monster.GroupCount = 1;

				monster.InitGroupCount = 1;

				monster.CurrGroupCount = 1;

				// Reset to 0 damage taken

				monster.DmgTaken = 0;
			}
			else
			{
				// Monster is already summoned and present so add another to the group

				monster.GroupCount++;

				monster.InitGroupCount++;

				monster.CurrGroupCount++;
			}
		}

		public Engine()
		{
			UseMonsterScaledHardinessValues = true;
		}
	}
}
