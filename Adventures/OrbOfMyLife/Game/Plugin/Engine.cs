
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
using Eamon.Game.Utilities;
using EamonRT.Framework.Components;
using EamonRT.Framework.States;
using static OrbOfMyLife.Game.Plugin.Globals;

namespace OrbOfMyLife.Game.Plugin
{
	public class Engine : EamonRT.Game.Plugin.Engine, Framework.Plugin.IEngine
	{
		public virtual bool VerboseRoomDescOrNotSeen { get; set; }

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

		public override void PrintPlayerRoom(IRoom room)
		{
			Debug.Assert(room != null);

			Debug.Assert(gCharMonster != null);

			var printRoom = true;

			// Falling

			if (room.Uid == 68)
			{
				if (ShouldPreTurnProcess && ++gGameState.FC > 9)
				{
					gGameState.FC = 0;

					PrintEffectDesc(35);

					GameState.Die = 1;

					CurrState.NextState = CreateInstance<IPlayerDeadState>(x =>
					{
						x.PrintLineSep = true;
					});

					CurrState.GotoCleanup = true;

					goto Cleanup;
				}
			}

			if (gGameState.IC)
			{
				Buf.Clear();

				Buf.Append("Your eyes are closed.");

				if (!room.IsViewable())
				{
					Buf.Append(" You can't see.");

					printRoom = false;
				}

				Out.Print("{0}", Buf);
			}
			else
			{
				if (room.Uid == 49)
				{
					Out.Print("The light hurts your eyes.");
				}
			}

			if (gGameState.IV)
			{
				Out.Print("You're invisible.");

				if (ShouldPreTurnProcess && ++gGameState.VC > 5)
				{
					gGameState.VC = 0;

					var combatComponent = CreateInstance<ICombatComponent>(x =>
					{
						x.SetNextStateFunc = s => CurrState.NextState = s;

						x.ActorRoom = room;

						x.Dobj = gCharMonster;

						x.OmitArmor = true;
					});

					combatComponent.ExecuteCalculateDamage(3, 1);

					if (GameState.Die > 0)
					{
						CurrState.GotoCleanup = true;

						goto Cleanup;
					}
				}
			}

			if (printRoom)
			{
				base.PrintPlayerRoom(room);
			}

		Cleanup:

			;
		}

		public override void PrintMonsterGetsAngry(IMonster monster, bool printFinalNewLine = true)
		{
			Debug.Assert(monster != null);

			Out.Write("{0}{1} get{2} angry!{3}",
				Environment.NewLine,
				monster.EvalInRoomViewability(monster.EvalPlural("The defender", "The defenders"), monster.GetTheName(true)),
				monster.EvalPlural("s", ""),
				printFinalNewLine ? Environment.NewLine : "");
		}

		public override void PrintMonsterEmotes(IMonster monster, bool friendSmile = true)
		{
			Debug.Assert(monster != null);

			var rl = RollDice(1, 100, 0);

			// Hero

			if (monster.Uid == 2)
			{
				Out.Write("{0}{1} {2} at you.", Environment.NewLine, monster.GetTheName(true), "squawks");
			}

			// Siren Witch

			else if (monster.Uid == 5)
			{
				Out.Write("{0}{1} {2} at you.", Environment.NewLine, monster.GetTheName(true), "cackles");
			}

			// Pike

			else if (monster.Uid == 14)
			{
				Out.Write("{0}{1} {2} at you.", Environment.NewLine, monster.GetTheName(true), "snaps");
			}

			// Squirrel

			else if (monster.Uid == 16)
			{
				Out.Write("{0}{1} {2} at you.", Environment.NewLine, monster.GetTheName(true), rl > 50 ? "chitters" : "chatters");
			}

			// Serpent

			else if (monster.Uid == 21)
			{
				Out.Write("{0}{1} {2} at you.", Environment.NewLine, monster.GetTheName(true), "hisses");
			}
			else
			{
				base.PrintMonsterEmotes(monster, friendSmile);
			}
		}

		public override void PrintTooManyWeapons()
		{
			Out.Print("As you start to enter the royal palace, Lord Harvey Killabrew appears and tells you, \"You have too many weapons to keep them all, four is the legal limit.\"");
		}

		public override void InitArtifacts()
		{
			base.InitArtifacts();

			MacroFuncs.Add(1, () =>
			{
				return gGameState != null ? gGameState.MW : UnknownName;
			});

			MacroFuncs.Add(2, () =>
			{
				return gGameState != null ? gGameState.TW : UnknownName;
			});

			MacroFuncs.Add(3, () =>
			{
				return gGameState != null ? gGameState.CW : UnknownName;
			});

			var synonyms = new Dictionary<long, string[]>()
			{
				{ 2, new string[] { "holy book", "book" } },
				{ 3, new string[] { "book of dark", "dark book", "loose book", "book" } },
				{ 4, new string[] { "magic book", "book of tolor", "tolor book", "book" } },
				{ 5, new string[] { "emerald key", "key of isk", "key" } },
				{ 6, new string[] { "tube", "sword of light", "light sword", "sword" } },
				{ 7, new string[] { "paintings", "painting" } },
				{ 8, new string[] { "ball" } },
				{ 9, new string[] { "life orb", "orb" } },
				{ 10, new string[] { "wood box", "box" } },
				{ 11, new string[] { "darkness cloak", "dark cloak", "cloak" } },
				{ 12, new string[] { "light staff", "staff" } },
				{ 13, new string[] { "light gate", "gate" } },
				{ 14, new string[] { "reome jewel", "jewel" } },
				{ 15, new string[] { "jade talisman", "talisman" } },
				{ 16, new string[] { "paintings", "painting" } },
				{ 17, new string[] { "levitation cloak", "cloak" } },
				{ 18, new string[] { "glass globe" } },
				{ 20, new string[] { "scrap" } },
				{ 22, new string[] { "sword" } },
				{ 23, new string[] { "life orb", "orb" } },
				{ 24, new string[] { "two handed sword", "sword" } },
				{ 25, new string[] { "offering plate", "plate" } },
				{ 26, new string[] { "idol" } },
				{ 27, new string[] { "coins", "coin" } },
				{ 28, new string[] { "knife" } },
				{ 29, new string[] { "orb" } },
				{ 30, new string[] { "old box", "copper box", "box" } },
				{ 31, new string[] { "glass" } },
				{ 32, new string[] { "wood crate", "crate" } },
				{ 35, new string[] { "note" } },
				{ 36, new string[] { "chained centurion", "centurion" } },
				{ 37, new string[] { "small key", "iron key", "key" } },
				{ 38, new string[] { "old axe", "iron axe", "axe" } },
				{ 39, new string[] { "table cloth", "tablecloth", "cloth" } },
				{ 40, new string[] { "paper" } },
				{ 41, new string[] { "small gold box", "small box", "golden box", "gold box", "box" } },
				{ 42, new string[] { "ring" } },
				{ 43, new string[] { "lies book", "lie book", "book" } },
				{ 44, new string[] { "small chest", "treasure chest", "chest" } },
				{ 45, new string[] { "key" } },
				{ 46, new string[] { "cross" } },
				{ 47, new string[] { "gemstone", "gem" } },
				{ 51, new string[] { "wisp of smoke", "pungent smoke", "smoke" } },
				{ 52, new string[] { "body" } },
				{ 53, new string[] { "dead priest" } },
				{ 54, new string[] { "dead priest" } },
				{ 55, new string[] { "dead witch" } },
				{ 56, new string[] { "dead council", "dead elders", "dead elder" } },
				{ 57, new string[] { "body" } },
				{ 58, new string[] { "body" } },
				{ 59, new string[] { "black wisp", "shadowy wisp" } },
				{ 60, new string[] { "wisp" } },
				{ 61, new string[] { "wisp" } },
				{ 62, new string[] { "cloud" } },
				{ 63, new string[] { "body" } },
				{ 64, new string[] { "dead fish" } },
				{ 68, new string[] { "dust" } },
				{ 69, new string[] { "body" } },
				{ 70, new string[] { "body" } },
				{ 71, new string[] { "dead snake" } },
				{ 72, new string[] { "body" } },
				{ 73, new string[] { "body" } },
				{ 74, new string[] { "dead bum" } },
				{ 75, new string[] { "corpse" } },
				{ 76, new string[] { "body" } },
				{ 77, new string[] { "body" } },
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
				{ 1, new string[] { "Dark Ness", "Ness" } },
				{ 2, new string[] { "eagle", "hawk", "bird" } },
				{ 3, new string[] { "priest" } },
				{ 4, new string[] { "priest" } },
				{ 5, new string[] { "witch", "old hag", "hag" } },
				{ 6, new string[] { "council of elders", "council" } },
				{ 7, new string[] { "dream giant", "giant" } },
				{ 8, new string[] { "dream giant", "giant" } },
				{ 9, new string[] { "prince", "darkness" } },
				{ 10, new string[] { "demon" } },
				{ 11, new string[] { "demon" } },
				{ 12, new string[] { "demon", "trowsk" } },
				{ 13, new string[] { "wizard" } },
				{ 14, new string[] { "fish" } },
				{ 18, new string[] { "beggar" } },
				{ 19, new string[] { "harold", "dup" } },
				{ 21, new string[] { "snake" } },
				{ 22, new string[] { "wizard" } },
				{ 24, new string[] { "bum" } },
				{ 26, new string[] { "soldier", "warrior" } },
				{ 27, new string[] { "troll", "scout" } },
			};

			foreach (var synonym in synonyms)
			{
				CreateMonsterSynonyms(synonym.Key, synonym.Value);
			}
		}

		public override void MonsterGetsAggravated(IMonster monster, bool printFinalNewLine = true)
		{
			Debug.Assert(monster != null);

			// Keep unconscious Darrk Ness Neutral

			if (monster.Uid != 1 || monster.StateDesc.Length <= 0)
			{
				base.MonsterGetsAggravated(monster, printFinalNewLine);
			}
		}

		public override void MonsterDies(IMonster actorMonster, IMonster dobjMonster)
		{
			Debug.Assert(dobjMonster != null);

			Debug.Assert(gCharRoom != null);

			// Darrk Ness

			if (dobjMonster.Uid == 1)
			{
				gGameState.FL = true;
			}

			var crystalBallArtifact = ADB[8];

			Debug.Assert(crystalBallArtifact != null);

			var crystalBallCarried = crystalBallArtifact.IsCarriedByMonster(dobjMonster);

			base.MonsterDies(actorMonster, dobjMonster);

			// Crystal ball shatters

			if (crystalBallCarried)
			{
				CrystalBallShatters(gCharRoom, crystalBallArtifact);
			}
		}

		public override void RevealEmbeddedArtifact(IRoom room, IArtifact artifact)
		{
			Debug.Assert(room != null);

			// Eyes open

			if (room.IsViewable())
			{
				base.RevealEmbeddedArtifact(room, artifact);
			}
		}

		public override IList<IMonster> GetHostileMonsterList(IMonster monster)
		{
			Debug.Assert(monster != null);

			var monsterList = base.GetHostileMonsterList(monster);

			// Invisible player is never attacked

			if (gGameState.IV)
			{
				monsterList = monsterList.Where(m => m.Uid != GameState.Cm).ToList();
			}

			return monsterList;
		}

		public override IList<IMonster> GetEmotingMonsterList(IRoom room, IMonster monster, bool friendSmile = true)
		{
			// Some monsters don't emote

			return base.GetEmotingMonsterList(room, monster, friendSmile).Where(m => m.Uid != 1 || m.StateDesc.Length <= 0).ToList();
		}

		public override bool IsValidRandomMoveDirection(long oldRoomUid, long newRoomUid)
		{
			// Lost In Woods

			if (newRoomUid == 3)
			{
				return true;
			}

			// Oval Chamber With Fountain

			else if (oldRoomUid == 13 && newRoomUid != 11)
			{
				return false;
			}
			else
			{
				return base.IsValidRandomMoveDirection(oldRoomUid, newRoomUid);
			}
		}

		public override void MoveMonsters(params Func<IMonster, bool>[] whereClauseFuncs)
		{
			Debug.Assert(gCharRoom != null);

			var partyCount = GetMonsterList(m => !m.IsCharacterMonster() && (m.Seen || gGameState.IC) && m.Reaction == Friendliness.Friend && m.Location == GameState.R3).Count();

			// Factor eyes closed and invisibility into movement

			base.MoveMonsters
			(
				whereClauseFuncs != null && whereClauseFuncs.Length > 0 ?
				whereClauseFuncs :
				new Func<IMonster, bool>[] { m => !m.IsCharacterMonster() && (m.Seen || gGameState.IC) && (m.Reaction == Friendliness.Friend || !gGameState.IV || partyCount > 0) && m.Location == GameState.R3 }
			);
		}

		public virtual void BuildRandomRoomExits(IRoom room)
		{
			Debug.Assert(room != null);

			var directionValues = EnumUtil.GetValues<Direction>();

			foreach (var dv in directionValues)
			{
				room.SetDir(dv, 0);
			}

			var directionValues02 = directionValues.Where(dv => dv != Direction.In && dv != Direction.Out).ToList();

			Shuffle(directionValues02);

			var nx = RollDice(1, 8, 0);         // directionValues02.Count

			for (var i = 0; i < nx; i++)
			{
				room.SetDir(directionValues02[i], room.Uid);
			}

			var rl = RollDice(1, 100, 0);

			if (rl > 50)
			{
				var rl02 = RollDice(1, nx, 0);

				room.SetDir(directionValues02[(int)rl02 - 1], 35);
			}
		}

		public virtual void BuildRandomMonster(IRoom room, IMonster monster, IArtifact deadBodyArtifact)
		{
			Debug.Assert(room != null);

			Debug.Assert(monster != null);

			Debug.Assert(deadBodyArtifact != null);

			var monsterNames = new string[] 
			{
				"gargoyle",
				"minor demon",
				"horror",
				"spirit",
				"zombie",
				"rabbit",
				"raccoon",
				"wolf",
				"cougar",
				"bear"
			};

			var mf = RollDice(1, 5, room.Uid == 3 ? 5 : 0);

			monster.Name = monsterNames[mf - 1];

			deadBodyArtifact.Name = string.Format("dead {0}", monster.Name);

			monster.Hardiness = (mf + (5 * (room.Uid != 3 ? 1 : 0)) - (5 * (room.Uid == 3 ? 1 : 0))) * 4 + 1;

			monster.Weapon = 0;

			monster.Parry = monster.InitParry;

			monster.DmgTaken = 0;

			monster.SetInRoom(room);
		}

		public virtual void MonstersGetUnnerved(bool prependNewLine = false)
		{
			Debug.Assert(gCharRoom != null);

			var monsterList = GetMonsterList(m => !m.IsCharacterMonster() && m.Reaction != Friendliness.Friend && m.IsInRoom(gCharRoom) && (m.Uid != 1 || m.StateDesc.Length <= 0));

			if (prependNewLine && monsterList.Count > 0)
			{
				Out.WriteLine();
			}

			foreach (var monster in monsterList)
			{
				Out.Print("{0} {1} slightly unnerved!", gCharRoom.EvalViewability(monster.EvalPlural("An unseen entity", "Some unseen entities"), monster.GetTheName(true)), monster.EvalPlural("is", "are"));

				monster.Courage -= 10;

				if (monster.Courage < 0)
				{
					monster.Courage = 0;
				}
			}
		}

		public virtual void CrystalBallShatters(IRoom room, IArtifact artifact)
		{
			Debug.Assert(room != null);

			Debug.Assert(artifact != null);

			var tolorMonster = MDB[13];

			Debug.Assert(tolorMonster != null);

			var shatteredGlassArtifact = ADB[31];

			Debug.Assert(shatteredGlassArtifact != null);

			Out.WriteLine();

			if (tolorMonster.IsInRoom(room))
			{
				PrintEffectDesc(25);

				tolorMonster.Friendliness = (Friendliness)100;

				tolorMonster.ResolveReaction(gCharacter);

				tolorMonster.Friendliness = tolorMonster.Reaction;
			}
			else
			{
				PrintEffectDesc(26);
			}

			artifact.SetInLimbo();

			shatteredGlassArtifact.SetInRoom(room);
		}

		public Engine()
		{
			PushRulesetVersion(62);

			MacroFuncs.Add(4, () =>
			{
				return GameState != null && GameState.Ro == 50 ? "back into the waking world where the images fade away even though your eyes remain shut" : "into a dream world where you can see images appearing even though your eyes are shut";
			});

			MacroFuncs.Add(5, () =>
			{
				return GameState != null && GameState.Ro == 50 ? "returning to normal darkness once again" : "but can see perfectly";
			});

			MacroFuncs.Add(6, () =>
			{
				return gGameState != null && gGameState.IC ? "meet" : "see";
			});

			MacroFuncs.Add(7, () =>
			{
				var room = GameState != null && RDB != null ? RDB[GameState.Ro] : null;

				return room != null && !room.IsViewable() ? "gasps" : "looks on";
			});

			EnableNegativeRoomUidLinks = true;

			PoundCharPolicy = PoundCharPolicy.None;
		}
	}
}
