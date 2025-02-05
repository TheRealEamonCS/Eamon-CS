
// Engine.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using Eamon;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using static TheDeepCanyon.Game.Plugin.Globals;

namespace TheDeepCanyon.Game.Plugin
{
	public class Engine : EamonRT.Game.Plugin.Engine, Framework.Plugin.IEngine
	{
		public virtual long ResurrectMonsterUid { get; set; }

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

			// Mountain lion

			if (monster.Uid == 2)
			{
				Out.Write("{0}{1} {2} at you!", Environment.NewLine, monster.GetTheName(true), rl > 66 ? "roars" : rl > 33 ? "snarls" : "hisses");
			}

			// Various bats

			else if (monster.Uid >= 6 && monster.Uid <= 10)
			{
				if (monster.Reaction == Friendliness.Neutral)
				{
					Out.Write("{0}{1} ignores you.", Environment.NewLine, monster.GetTheName(true));
				}
				else
				{
					Out.Write("{0}{1} {2} at you{3}", Environment.NewLine, monster.GetTheName(true), rl > 50 ? "squeaks" : "hisses", monster.EvalReaction("!", ".", "."));
				}
			}

			// Pig

			else if (monster.Uid == 13)
			{
				Out.Write("{0}{1} squeals at you!", Environment.NewLine, monster.GetTheName(true));
			}

			// Goose

			else if (monster.Uid == 14)
			{
				Out.Write("{0}{1} honks at you!", Environment.NewLine, monster.GetTheName(true));
			}

			// Horse

			else if (monster.Uid == 15)
			{
				if (monster.Reaction == Friendliness.Neutral)
				{
					Out.Write("{0}{1} ignores you.", Environment.NewLine, monster.GetTheName(true));
				}
				else
				{
					Out.Write("{0}{1} {2} at you{3}", Environment.NewLine, monster.GetTheName(true), rl > 66 ? "whinnies" : rl > 33 ? "snorts" : monster.EvalReaction("squeals", "nickers", "nickers"), monster.EvalReaction("!", ".", "."));
				}
			}

			// Daisy

			else if (monster.Uid == 16)
			{
				if (monster.Reaction == Friendliness.Neutral)
				{
					Out.Write("{0}{1} ignores you.", Environment.NewLine, monster.GetTheName(true));
				}
				else
				{
					Out.Write("{0}{1} {2} at you{3}", Environment.NewLine, monster.GetTheName(true), rl > 66 ? "moos" : rl > 33 ? "snorts" : monster.EvalReaction("bellows", "grunts", "grunts"), monster.EvalReaction("!", ".", "."));
				}
			}

			// Groundhog

			else if (monster.Uid == 17)
			{
				Out.Write("{0}{1} {2} at you!", Environment.NewLine, monster.GetTheName(true), rl > 66 ? "chatters" : rl > 33 ? "hisses" : "whistles");
			}

			// Brown ferret/Black ferret

			else if (monster.Uid == 18 || monster.Uid == 19)
			{
				if (monster.Reaction == Friendliness.Neutral)
				{
					Out.Write("{0}{1} ignores you.", Environment.NewLine, monster.GetTheName(true));
				}
				else
				{
					Out.Write("{0}{1} {2} at you{3}", Environment.NewLine, monster.GetTheName(true), rl > 66 ? "chatters" : rl > 33 ? "hisses" : monster.EvalReaction("barks", "dooks", "dooks"), monster.EvalReaction("!", ".", "."));
				}
			}

			// Kiord

			else if (monster.Uid == 21)
			{
				Out.Write("{0}{1} {2} at you!", Environment.NewLine, monster.GetTheName(true), rl > 66 ? "screeches" : rl > 33 ? "hisses" : "squawks");
			}

			// Goat

			else if (monster.Uid == 22)
			{
				Out.Write("{0}{1} {2} at you!", Environment.NewLine, monster.GetTheName(true), rl > 50 ? "bleats" : "bellows");
			}

			// Chicken

			else if (monster.Uid == 23)
			{
				if (monster.Reaction == Friendliness.Neutral)
				{
					Out.Write("{0}{1} ignores you.", Environment.NewLine, monster.GetTheName(true));
				}
				else
				{
					Out.Write("{0}{1} {2} at you{3}", Environment.NewLine, monster.GetTheName(true), rl > 66 ? "clucks" : rl > 33 ? "cackles" : monster.EvalReaction("squawks", "warbles", "warbles"), monster.EvalReaction("!", ".", "."));
				}
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

		public override void AddUniqueCharsToArtifactAndMonsterNames()
		{
			base.AddUniqueCharsToArtifactAndMonsterNames();

			var diamondsArtifact = ADB[10];

			Debug.Assert(diamondsArtifact != null);

			diamondsArtifact.Name = diamondsArtifact.Name.TrimEnd('#');
		}

		public override void InitArtifacts()
		{
			base.InitArtifacts();

			MacroFuncs.Add(1, () =>
			{
				// Silver lantern

				return GameState != null && GameState.Ls == 7 ? "lit" : "unlit";
			});

			var synonyms = new Dictionary<long, string[]>()
			{
				{ 2, new string[] { "bag", "sack" } },
				{ 5, new string[] { "raptor", "bird" } },
				{ 6, new string[] { "miners pick", "pick" } },
				{ 7, new string[] { "lantern" } },
				{ 11, new string[] { "pitchfork", "fork" } },
				{ 15, new string[] { "gold", "ore" } },
				{ 16, new string[] { "big stick" } },
				{ 17, new string[] { "trap" } },
				{ 29, new string[] { "dead baby worm", "dead sand worm", "dead worm" } },
				{ 30, new string[] { "dead lion", "dead cougar", "dead puma", "dead wildcat", "dead bobcat" } },
				{ 31, new string[] { "dead miner" } },
				{ 34, new string[] { "dead fox" } },
				{ 35, new string[] { "dead small bat", "dead vampire bat", "dead bat" } },
				{ 36, new string[] { "dead large bat", "dead fruit bat", "dead bat" } },
				{ 37, new string[] { "dead small bat", "dead fruit bat", "dead bat" } },
				{ 38, new string[] { "dead large bat", "dead vampire bat", "dead bat" } },
				{ 39, new string[] { "dead Fido" } },
				{ 44, new string[] { "dead Daisy" } },
				{ 46, new string[] { "dead ferret" } },
				{ 47, new string[] { "dead ferret" } },
				{ 52, new string[] { "tracks", "track" } },
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
				{ 1, new string[] { "baby worm", "sand worm", "worm" } },
				{ 2, new string[] { "cougar", "lion", "puma", "wildcat", "bobcat" } },
				{ 3, new string[] { "miner" } },
				{ 6, new string[] { "fox" } },
				{ 7, new string[] { "small bat", "vampire bat", "bat" } },
				{ 8, new string[] { "large bat", "fruit bat", "bat" } },
				{ 9, new string[] { "small bat", "fruit bat", "bat" } },
				{ 10, new string[] { "large bat", "vampire bat", "bat" } },
				{ 16, new string[] { "cow" } },
				{ 18, new string[] { "ferret" } },
				{ 19, new string[] { "ferret" } },
			};

			foreach (var synonym in synonyms)
			{
				CreateMonsterSynonyms(synonym.Key, synonym.Value);
			}
		}

		public override void MonsterGetsAggravated(IMonster monster, bool printFinalNewLine = true)
		{
			Debug.Assert(monster != null);

			var room = monster.GetInRoom();

			Debug.Assert(room != null);

			// Fido wakes up

			if (monster.Uid == 11 && gGameState.FidoSleepCounter > 0)
			{
				MiscEventFuncList02.Add(() =>
				{
					if (monster.IsInRoom(room))
					{
						Out.Print("{0} wakes up!", room.IsViewable() ? "Fido" : "Something");
					}

					gGameState.FidoSleepCounter = 0;

					monster.StateDesc = "";

					monster.Reaction = Friendliness.Enemy;
				});
			}
			else
			{
				PushRulesetVersion(0);

				base.MonsterGetsAggravated(monster, printFinalNewLine);

				PopRulesetVersion();
			}
		}

		public override void MonsterDies(IMonster actorMonster, IMonster dobjMonster)
		{
			Debug.Assert(dobjMonster != null);

			var room = dobjMonster.GetInRoom();

			Debug.Assert(room != null);

			base.MonsterDies(actorMonster, dobjMonster);

			var goldCoinsArtifact = ADB[3];

			Debug.Assert(goldCoinsArtifact != null);

			var silverCoinsArtifact = ADB[4];

			Debug.Assert(silverCoinsArtifact != null);

			// Gold miner

			if (dobjMonster.Uid == 3 && goldCoinsArtifact.IsInLimbo())
			{
				if (room.IsViewable())
				{
					Out.Print("{0}The gold miner drops his only fortune to the ground.", Environment.NewLine);
				}

				goldCoinsArtifact.SetInRoom(room);
			}

			// Falconer

			else if (dobjMonster.Uid == 4 && silverCoinsArtifact.IsInLimbo())
			{
				if (room.IsViewable())
				{
					Out.Print("{0}The falconer drops some silver coins as he dies.", Environment.NewLine);
				}

				silverCoinsArtifact.SetInRoom(room);
			}

			// Fido

			else if (dobjMonster.Uid == 11)
			{
				gGameState.FidoSleepCounter = 0;

				dobjMonster.StateDesc = "";

				dobjMonster.Reaction = Friendliness.Enemy;
			}
		}

		public override void CheckNumberOfExits(IRoom room, IMonster monster, bool fleeing, ref long numExits)
		{
			Debug.Assert(room != null);

			base.CheckNumberOfExits(room, monster, fleeing, ref numExits);

			// Exclude various "invisible" exits

			if (room.Uid == 8 || room.Uid == 22 || (monster.IsCharacterMonster() && (room.Uid == 41 || room.Uid == 43 || room.Uid == 44 || room.Uid == 61)))
			{
				numExits--;
			}
		}

		public override void GetRandomMoveDirection(IRoom room, IMonster monster, bool fleeing, ref Direction direction, ref bool found, ref long roomUid)
		{
			Debug.Assert(room != null);

			// Exclude various "invisible" exits

			do
			{
				base.GetRandomMoveDirection(room, monster, fleeing, ref direction, ref found, ref roomUid);
			}
			while ((room.Uid == 8 && direction == Direction.West) || (room.Uid == 22 && direction == Direction.North) || (room.Uid == 41 && direction == Direction.North) || (room.Uid == 43 && direction == Direction.East) || (room.Uid == 44 && direction == Direction.West) || (room.Uid == 61 && direction == Direction.West));
		}

		public override IList<IArtifact> GetReadyableWeaponList(IMonster monster)
		{
			IList<IArtifact> result;

			PushRulesetVersion(0);

			result = base.GetReadyableWeaponList(monster);

			PopRulesetVersion();

			return result;
		}

		public virtual void MagicRingLowersMonsterStats(IMonster monster)
		{
			Debug.Assert(monster != null);

			var stat = GetStat(Stat.Hardiness);

			Debug.Assert(stat != null);

			monster.Hardiness -= (long)Math.Round((double)monster.Hardiness * 0.4);

			if (monster.Hardiness < stat.MinValue)
			{
				monster.Hardiness = stat.MinValue;
			}

			stat = GetStat(Stat.Agility);

			Debug.Assert(stat != null);

			monster.Agility -= (long)Math.Round((double)monster.Agility * 0.4);

			if (monster.Agility < stat.MinValue)
			{
				monster.Agility = stat.MinValue;
			}
		}

		public Engine()
		{
			PushRulesetVersion(5);
		}
	}
}
