
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
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static Dharmaquest.Game.Plugin.Globals;

namespace Dharmaquest.Game.Plugin
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

			// Sacred bull

			if (monster.Uid == 16 && monster.Reaction != Friendliness.Neutral)
			{
				var emoteStrings = monster.Reaction == Friendliness.Friend ? new string[] { "nuzzles", "snorts", "grunts" } : new string[] { "paws", "bellows", "grunts" };

				Out.Write("{0}{1} {2} at you.", Environment.NewLine, monster.GetTheName(true), gEngine.GetRandomElement(emoteStrings));
			}

			// Python

			else if (monster.Uid == 20 && monster.Reaction != Friendliness.Neutral)
			{
				var emoteStrings = monster.Reaction == Friendliness.Friend ? new string[] { "sways gently", "glides smoothly", "flicks its tongue" } : new string[] { "hisses", "rears threateningly", "coils menacingly" };

				Out.Write("{0}{1} {2} at you.", Environment.NewLine, monster.GetTheName(true), gEngine.GetRandomElement(emoteStrings));
			}

			// Leopard

			else if (monster.Uid == 30 && monster.Reaction != Friendliness.Neutral)
			{
				var emoteStrings = monster.Reaction == Friendliness.Friend ? new string[] { "nuzzles", "purrs", "slowly blinks" } : new string[] { "roars", "snarls", "hisses" };

				Out.Write("{0}{1} {2} at you.", Environment.NewLine, monster.GetTheName(true), gEngine.GetRandomElement(emoteStrings));
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
				IEffect inscriptionEffect = null;

				if (gGameState != null)
				{
					if (gGameState.Ro == 62)
					{
						inscriptionEffect = EDB[18];
					}
					else if (gGameState.Ro == 63)
					{
						inscriptionEffect = EDB[16];
					}
					else if (gGameState.Ro == 65)
					{
						inscriptionEffect = EDB[17];
					}
					else if (gGameState.Ro == 70)
					{
						inscriptionEffect = EDB[13];
					}
					else if (gGameState.Ro == 72)
					{
						inscriptionEffect = EDB[14];
					}
				}

				return inscriptionEffect != null ? inscriptionEffect.Desc : UnknownName;
			});

			var synonyms = new Dictionary<long, string[]>()
			{
				{ 1, new string[] { "tablet" } },
				{ 2, new string[] { "tablet" } },
				{ 3, new string[] { "tablet" } },
				{ 4, new string[] { "ring" } },
				{ 5, new string[] { "ring" } },
				{ 6, new string[] { "ring" } },
				{ 7, new string[] { "ring" } },
				{ 9, new string[] { "drowning child", "boy", "child" } },
				{ 10, new string[] { "woman" } },
				{ 11, new string[] { "little child", "girl", "child" } },
				{ 12, new string[] { "patroclos' statue", "statue", "patroclos" } },
				{ 13, new string[] { "apollo's statue", "statue", "apollo" } },
				{ 14, new string[] { "poseidon's statue", "statue", "poseidon" } },
				{ 15, new string[] { "aphrodite's statue", "statue", "aphrodite" } },
				{ 16, new string[] { "artemis' statue", "statue", "artemis" } },
				{ 17, new string[] { "chest" } },
				{ 18, new string[] { "cup" } },
				{ 20, new string[] { "salt block", "block" } },
				{ 24, new string[] { "gold ingots" } },
				{ 25, new string[] { "large sacks", "sacks", "coins" } },
				{ 26, new string[] { "box" } },
				{ 27, new string[] { "box" } },
				{ 29, new string[] { "spear" } },
				{ 30, new string[] { "spear" } },
				{ 32, new string[] { "sword" } },
				{ 33, new string[] { "sword" } },
				{ 37, new string[] { "axe" } },
				{ 41, new string[] { "dreamer" } },
				{ 43, new string[] { "stalker" } },
				{ 44, new string[] { "drinker" } },
				{ 45, new string[] { "body" } },
				{ 46, new string[] { "body" } },
				{ 47, new string[] { "body" } },
				{ 48, new string[] { "body" } },
				{ 49, new string[] { "body" } },
				{ 50, new string[] { "statue" } },
				{ 51, new string[] { "statue" } },
				{ 52, new string[] { "statue" } },
				{ 53, new string[] { "body" } },
				{ 54, new string[] { "body" } },
				{ 55, new string[] { "body" } },
				{ 56, new string[] { "body" } },
				{ 57, new string[] { "body" } },
				{ 58, new string[] { "armor" } },
				{ 60, new string[] { "dead sacred bull", "bull" } },
				{ 61, new string[] { "body" } },
				{ 62, new string[] { "body" } },
				{ 63, new string[] { "body" } },
				{ 64, new string[] { "dead sacred python", "python" } },
				{ 65, new string[] { "body" } },
				{ 66, new string[] { "body" } },
				{ 67, new string[] { "body" } },
				{ 68, new string[] { "body" } },
				{ 69, new string[] { "body" } },
				{ 70, new string[] { "body" } },
				{ 71, new string[] { "body" } },
				{ 72, new string[] { "body" } },
				{ 73, new string[] { "body" } },
				{ 74, new string[] { "leopard" } },
				{ 75, new string[] { "sphinx" } },
				{ 76, new string[] { "body" } },
				{ 77, new string[] { "body" } },
				{ 78, new string[] { "body" } },
				{ 79, new string[] { "body" } },
				{ 80, new string[] { "body" } },
			};

			foreach (var synonym in synonyms)
			{
				CreateArtifactSynonyms(synonym.Key, synonym.Value);
			}

			// Nemo's armour

			var armourArtifact = ADB[58];

			Debug.Assert(armourArtifact != null);

			var armourArtifact02 = GameState?.Ar > 0 ? ADB[GameState.Ar] : null;

			if (armourArtifact02 != null)
			{
				armourArtifact.Value = armourArtifact02.Value / 2;

				armourArtifact.Weight = armourArtifact02.Weight;
			}
		}

		public override void InitMonsters()
		{
			base.InitMonsters();

			var synonyms = new Dictionary<long, string[]>()
			{
				{ 15, new string[] { "wizard" } },
				{ 16, new string[] { "bull" } },
				{ 20, new string[] { "sacred python", "snake" } },
				{ 30, new string[] { "cat" } },
			};

			foreach (var synonym in synonyms)
			{
				CreateMonsterSynonyms(synonym.Key, synonym.Value);
			}
		}

		public override void MonsterDies(IMonster actorMonster, IMonster dobjMonster)
		{
			Debug.Assert(dobjMonster != null);

			// Sphinx

			if (dobjMonster.Uid == 31)
			{
				gGameState.SphinxKilled = true;
			}

			base.MonsterDies(actorMonster, dobjMonster);
		}

		public override void CheckNumberOfExits(IRoom room, IMonster monster, bool fleeing, ref long numExits)
		{
			Debug.Assert(room != null);

			var excludedRoomUids = new long[] { 4, 36, 38, 43, 49, 50, 67, 69 };

			base.CheckNumberOfExits(room, monster, fleeing, ref numExits);

			// Exclude various "invisible" exits

			if (excludedRoomUids.Contains(room.Uid))
			{
				numExits--;
			}
		}

		public override void GetRandomMoveDirection(IRoom room, IMonster monster, bool fleeing, ref Direction direction, ref bool found, ref long roomUid)
		{
			Debug.Assert(room != null);

			var northRoomUids = new long[] { 43, 67 };

			var eastRoomUids = new long[] { 4 };

			var downRoomUids = new long[] { 36, 38, 49, 50, 69 };

			// Exclude various "invisible" exits

			do
			{
				base.GetRandomMoveDirection(room, monster, fleeing, ref direction, ref found, ref roomUid);
			}
			while ((northRoomUids.Contains(room.Uid) && direction == Direction.North) || (eastRoomUids.Contains(room.Uid) && direction == Direction.East) || (downRoomUids.Contains(room.Uid) && direction == Direction.Down));
		}

		public override IList<IMonster> GetHostileMonsterList(IMonster monster)
		{
			Debug.Assert(monster != null);

			var monsterList = base.GetHostileMonsterList(monster);

			// Sacred bull / Python are never attacked

			monsterList = monsterList.Where(m => m.Uid != 16 && m.Uid != 20).ToList();

			return monsterList;
		}

		public virtual void ApolloCursesPlayer()
		{
			Debug.Assert(gCharMonster != null);

			// Curses

			Out.Print("Apollo puts a curse on you for killing his sacred python!");

			gCharMonster.Gender = gCharMonster.EvalGender(Gender.Female, Gender.Male, Gender.Neutral);

			Out.Print("You are now {0}!", gCharMonster.Gender.ToString().ToLower());

			// Allies become enemies

			var monsterList = GetFriendlyMonsterList(gCharMonster);

			foreach (var monster in monsterList)
			{
				monster.Reaction = Friendliness.Enemy;

				Out.Print("{0} begins to mutter!", monster.GetTheName(true));
			}
		}

		public virtual void PoseidonCursesPlayer(IState printPlayerRoomState)
		{
			Debug.Assert(printPlayerRoomState != null);

			Debug.Assert(gCharMonster != null);

			Debug.Assert(gCharRoom != null);

			// Carried artifacts disappear

			var artifactList = gCharMonster.GetCarriedList();

			foreach (var artifact in artifactList)
			{
				Out.EnableOutput = false;

				CurrState = CreateInstance<IDropCommand>(x =>
				{
					x.ActorMonster = gCharMonster;

					x.ActorRoom = gCharRoom;

					x.Dobj = artifact;
				});

				CurrCommand.Execute();

				CurrState = printPlayerRoomState;

				Out.EnableOutput = true;

				Out.Print("{0} disappear{1}!", artifact.GetTheName(true), artifact.EvalPlural("s", ""));

				artifact.SetInLimbo();
			}
		}

		public Engine()
		{
			PushRulesetVersion(5);

			MacroFuncs.Add(1, () =>
			{
				return Character != null ? Character.Name : UnknownName;
			});

			PoundCharPolicy = PoundCharPolicy.None;
		}
	}
}
