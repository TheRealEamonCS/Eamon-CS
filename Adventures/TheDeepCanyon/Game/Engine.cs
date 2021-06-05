
// Engine.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using static TheDeepCanyon.Game.Plugin.PluginContext;

namespace TheDeepCanyon.Game
{
	[ClassMappings(typeof(IEngine))]
	public class Engine : EamonRT.Game.Engine, EamonRT.Framework.IEngine
	{
		public override void AddUniqueCharsToArtifactAndMonsterNames()
		{
			base.AddUniqueCharsToArtifactAndMonsterNames();

			var diamondsArtifact = gADB[10];

			Debug.Assert(diamondsArtifact != null);

			diamondsArtifact.Name = diamondsArtifact.Name.TrimEnd('#');
		}

		public override void InitArtifacts()
		{
			base.InitArtifacts();

			MacroFuncs.Add(1, () =>
			{
				// silver lantern

				return gGameState != null && gGameState.Ls == 7 ? "lit" : "unlit";
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
				gOut.Print("{0} wakes up!", room.IsLit() ? "Fido" : "Something");

				gGameState.FidoSleepCounter = 0;

				monster.StateDesc = "";

				monster.Reaction = Friendliness.Enemy;
			}
			else
			{
				base.MonsterGetsAggravated(monster, printFinalNewLine);
			}
		}

		public override void MonsterDies(IMonster OfMonster, IMonster DfMonster)
		{
			Debug.Assert(DfMonster != null);

			var room = DfMonster.GetInRoom();

			Debug.Assert(room != null);

			base.MonsterDies(OfMonster, DfMonster);

			var goldCoinsArtifact = gADB[3];

			Debug.Assert(goldCoinsArtifact != null);

			var silverCoinsArtifact = gADB[4];

			Debug.Assert(silverCoinsArtifact != null);

			// Gold miner

			if (DfMonster.Uid == 3 && goldCoinsArtifact.IsInLimbo())
			{
				gOut.Print("{0}The gold miner drops his only fortune to the ground.", Environment.NewLine);

				goldCoinsArtifact.SetInRoom(room);
			}

			// Falconer

			else if (DfMonster.Uid == 4 && silverCoinsArtifact.IsInLimbo())
			{
				gOut.Print("{0}The falconer drops some silver coins as he dies.", Environment.NewLine);

				silverCoinsArtifact.SetInRoom(room);
			}

			// Fido

			else if (DfMonster.Uid == 11)
			{
				gGameState.FidoSleepCounter = 0;

				DfMonster.StateDesc = "";

				DfMonster.Reaction = Friendliness.Enemy;
			}
		}

		public override void CheckNumberOfExits(IRoom room, IMonster monster, bool fleeing, ref long numExits)
		{
			Debug.Assert(room != null);

			base.CheckNumberOfExits(room, monster, fleeing, ref numExits);

			// Exclude west exit in Falconer's camp

			if (room.Uid == 8)
			{
				numExits--;
			}
		}

		public override void GetRandomMoveDirection(IRoom room, IMonster monster, bool fleeing, ref Direction direction, ref bool found, ref long roomUid)
		{
			Debug.Assert(room != null);

			// Exclude west exit in Falconer's camp

			do
			{
				base.GetRandomMoveDirection(room, monster, fleeing, ref direction, ref found, ref roomUid);
			}
			while (room.Uid == 8 && direction == Direction.West);
		}
	}
}
