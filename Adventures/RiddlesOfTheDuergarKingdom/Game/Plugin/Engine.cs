
// Engine.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using Eamon;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using static RiddlesOfTheDuergarKingdom.Game.Plugin.Globals;

namespace RiddlesOfTheDuergarKingdom.Game.Plugin
{
	public class Engine : EamonRT.Game.Plugin.Engine, Framework.Plugin.IEngine
	{
		StringBuilder Framework.Plugin.IEngine.Buf { get; set; }

		StringBuilder Framework.Plugin.IEngine.Buf01 { get; set; }

		public virtual long PoisonInjuryTurns { get; protected set; } = 7;

		public virtual long IbexAbandonTurns { get; protected set; } = 15;

		public virtual bool BlackSpiderJumps { get; set; }

		public virtual bool PlayerAttacksBlackSpider { get; set; }

		public virtual bool EnemyInExitedDarkRoom { get; set; }

		public virtual long[] ArchaeologyDepartmentMonsterUids { get; set; }

		public virtual long[] ArchaeologyDepartmentStartRoomUids { get; set; }

		public virtual long[] ArchaeologyDepartmentAbandonedRoomUids { get; set; }

		public virtual long[] GradStudentStartRoomUids { get; set; }

		public virtual long[] RanchHandsStartRoomUids { get; set; }

		public virtual long[] SupportBeamsRoomUids { get; set; }

		public virtual long[] TracksRoomUids { get; set; }

		public virtual long[] LavaRiverRoomUids { get; set; }

		public virtual long[] BroilingRoomUids { get; set; }

		public virtual long[] BeachRoomUids { get; set; }

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

			// Suppress black spider's initial attack

			if (artifact.Uid == 43)
			{
				BlackSpiderJumps = true;
			}
			else
			{
				base.PrintMonsterAlive(artifact);
			}
		}

		public override void AddUniqueCharsToArtifactAndMonsterNames()
		{
			base.AddUniqueCharsToArtifactAndMonsterNames();

			// Tracks #2

			var oreCartTracksArtifact = ADB[45];

			Debug.Assert(oreCartTracksArtifact != null);

			oreCartTracksArtifact.Name = oreCartTracksArtifact.Name.TrimEnd('#');

			// Hand crank #2

			var handCrankArtifact = ADB[60];

			Debug.Assert(handCrankArtifact != null);

			handCrankArtifact.Name = handCrankArtifact.Name.TrimEnd('#');

			// Blacksmith's smock #2

			var blacksmithSmockArtifact = ADB[68];

			Debug.Assert(blacksmithSmockArtifact != null);

			blacksmithSmockArtifact.Name = blacksmithSmockArtifact.Name.TrimEnd('#');
		}

		public override void InitArtifacts()
		{
			base.InitArtifacts();

			MacroFuncs.Add(7, () =>
			{
				var oreCartArtifact = ADB[46];

				Debug.Assert(oreCartArtifact != null);

				return oreCartArtifact.IsInRoom() ? "But your eyes drift to the tracks, and you wonder if there may be a better option." : "It's probably best to leave it on the tracks.";
			});

			MacroFuncs.Add(9, () =>
			{
				var goldNuggetsArtifact = ADB[66];

				Debug.Assert(goldNuggetsArtifact != null);

				return GetStringFromNumber(goldNuggetsArtifact.Value / 2, false, gEngine.Buf01);
			});

			var synonyms = new Dictionary<long, string[]>()
			{
				{ 1, new string[] { "rock strewn Gully", "rocky gully", "gully" } },
				{ 2, new string[] { "large rock", "boulder", "rock" } },
				{ 3, new string[] { "narrow crevice", "vertical crevice", "crevice" } },
				{ 4, new string[] { "fence" } },
				{ 5, new string[] { "gate" } },
				{ 6, new string[] { "bell" } },
				{ 7, new string[] { "tent" } },
				{ 8, new string[] { "tent" } },
				{ 9, new string[] { "tent" } },
				{ 10, new string[] { "tent" } },
				{ 11, new string[] { "tent" } },
				{ 12, new string[] { "tent" } },
				{ 14, new string[] { "wood ladder", "ladder" } },
				{ 15, new string[] { "lever" } },
				{ 16, new string[] { "wood cart", "cart" } },
				{ 17, new string[] { "anvil" } },
				{ 18, new string[] { "winch" } },
				{ 19, new string[] { "animals", "mules", "donkeys", "horses" } },
				{ 20, new string[] { "oil lamp", "lantern", "lamp" } },					// TODO: refactor?
				{ 21, new string[] { "lake", "water" } },
				{ 22, new string[] { "stone cabin door", "cabin door", "cabin", "door" } },
				{ 23, new string[] { "stone outhouse door", "outhouse door", "outhouse", "door" } },
				{ 24, new string[] { "stone hydrostation door", "hydrostation door", "hydrostation", "door" } },
				{ 25, new string[] { "sluice", "gate" } },
				{ 26, new string[] { "crank" } },
				{ 27, new string[] { "door" } },
				{ 28, new string[] { "spiral stairway", "spiral stairs", "staircase", "stairway", "stairs" } },
				{ 29, new string[] { "iron turbine", "steam turbine", "turbine" } },
				{ 30, new string[] { "gears" } },
				{ 31, new string[] { "blacksmith hammer", "hammer" } },
				{ 32, new string[] { "wood workbench", "wooden bench", "wood bench", "workbench", "bench" } },
				{ 35, new string[] { "garden" } },
				{ 39, new string[] { "supports", "beams" } },
				{ 41, new string[] { "pen" } },
				{ 42, new string[] { "toilet", "lavatory", "commode", "throne", "loo", "john" } },
				{ 43, new string[] { "spider" } },
				{ 46, new string[] { "cart" } },
				{ 47, new string[] { "sign" } },
				{ 48, new string[] { "crusher" } },
				{ 49, new string[] { "grinder" } },
				{ 50, new string[] { "sifter" } },
				{ 51, new string[] { "belt" } },
				{ 52, new string[] { "fine debris mound", "debris mound", "fine debris", "fine mound", "debris", "mound" } },
				{ 53, new string[] { "pit" } },
				{ 54, new string[] { "lava fall", "lava", "fall" } },
				{ 55, new string[] { "river of fire", "fire river", "lava", "river" } },
				{ 56, new string[] { "salamander eggs", "eggs" } },
				{ 58, new string[] { "river" } },
				{ 59, new string[] { "water fall", "water", "fall" } },
				{ 60, new string[] { "crank" } },
				{ 61, new string[] { "water gate", "diversion gate", "gate" } },
				{ 62, new string[] { "bleached bones", "skeleton", "bones" } },
				{ 63, new string[] { "skull" } },
				{ 64, new string[] { "tattered pack", "backpack", "pack" } },
				{ 65, new string[] { "pan" } },
				{ 66, new string[] { "nuggets" } },
				{ 67, new string[] { "blacksmith smock", "smock" } },
				{ 68, new string[] { "blacksmith smock", "smock" } },
				{ 70, new string[] { "tree" } },
				{ 75, new string[] { "hermit's body", "dead hermit", "dead body", "body" } },
				{ 76, new string[] { "Enquirer", "newspaper", "paper", "magazine" } },
				{ 77, new string[] { "giant cob web", "giant spiderweb", "giant spider web", "giant web", "cobweb", "spiderweb", "cob web", "spider web", "web" } },
				{ 78, new string[] { "path" } },
				{ 88, new string[] { "pot of coffee", "coffee", "pot" } },
				{ 89, new string[] { "dry creek", "dry bed", "creek bed", "creek", "bed" } },
				{ 90, new string[] { "den" } },
				{ 91, new string[] { "door" } },
				{ 92, new string[] { "tent" } },
				{ 93, new string[] { "panniers", "pannier" } },
			};

			foreach (var synonym in synonyms)
			{
				CreateArtifactSynonyms(synonym.Key, synonym.Value);
			}
		}

		public override void InitMonsters()
		{
			base.InitMonsters();

			MacroFuncs.Add(1, () =>
			{
				var gradStudentCompanionMonster = GameState != null ? MDB[gGameState.GradStudentCompanionUid] : null;

				return gradStudentCompanionMonster != null ? gradStudentCompanionMonster.Name : "???";
			});

			MacroFuncs.Add(2, () =>
			{
				var monsterList = GameState != null ? GetMonsterList(m => m.IsInRoomUid(GameState.Ro)) : new List<IMonster>();

				return monsterList.Count > 2 ? "all" : "both";
			});

			MacroFuncs.Add(3, () =>
			{
				var gradStudentCompanionMonster = GameState != null ? MDB[gGameState.GradStudentCompanionUid] : null;

				return gradStudentCompanionMonster != null ? gradStudentCompanionMonster.EvalGender("his", "her", "its") : "???";
			});

			MacroFuncs.Add(4, () =>
			{
				var gradStudentCompanionMonster = GameState != null ? MDB[gGameState.GradStudentCompanionUid] : null;

				return gradStudentCompanionMonster != null ? gradStudentCompanionMonster.EvalGender("He", "She", "It") : "???";
			});

			MacroFuncs.Add(5, () =>
			{
				var gradStudentCompanionMonster = GameState != null ? MDB[gGameState.GradStudentCompanionUid] : null;

				return gradStudentCompanionMonster != null ? gradStudentCompanionMonster.EvalGender("he", "she", "it") : "???";
			});

			MacroFuncs.Add(6, () =>
			{
				return GameState != null && gGameState.VolcanoErupting ? "nobody will answer you." : "someone will arrive shortly.";
			});

			MacroFuncs.Add(8, () =>
			{
				var gradStudentCompanionMonster = GameState != null ? MDB[gGameState.GradStudentCompanionUid] : null;

				return gradStudentCompanionMonster != null ? gradStudentCompanionMonster.EvalGender("him", "her", "it") : "???";
			});

			MacroFuncs.Add(10, () =>
			{
				return GameState != null && gGameState.WaiverSigned ? "So, unfortunately, I can't help you as you've already used your allotment!" : "However, if you were an independent contractor, I could probably help you!";
			});

			MacroFuncs.Add(11, () =>
			{
				var artifactList = gCharMonster != null ? gCharMonster.GetContainedList() : new List<IArtifact>();

				return artifactList.Count > 0 ? "  Upon further reflection, you realize you must shed your inventory (both worn and carried), as it is too unbalancing." : "";
			});

			var synonyms = new Dictionary<long, string[]>()
			{
				{ 1, new string[] { "viper", "snake" } },
				{ 2, new string[] { "Professor", "Berescroft" } },
				{ 3, new string[] { "guard" } },
				{ 4, new string[] { "guard" } },
				{ 5, new string[] { "foreman" } },
				{ 6, new string[] { "medic" } },
				{ 7, new string[] { "cook" } },
				{ 8, new string[] { "Winslow", "grad student", "grad", "student" } },
				{ 9, new string[] { "Emilie", "grad student", "grad", "student" } },
				{ 10, new string[] { "Noella", "grad student", "grad", "student" } },
				{ 11, new string[] { "undergrad student team", "undergrad team members", "student team members", "undergrad students", "student team", "team members", "undergrads", "students", "team", "members" } },
				{ 12, new string[] { "undergrad student team", "undergrad team members", "student team members", "undergrad students", "student team", "team members", "undergrads", "students", "team", "members" } },
				{ 13, new string[] { "undergrad student team", "undergrad team members", "student team members", "undergrad students", "student team", "team members", "undergrads", "students", "team", "members" } },
				{ 14, new string[] { "undergrad student team", "undergrad team members", "student team members", "undergrad students", "student team", "team members", "undergrads", "students", "team", "members" } },
				{ 15, new string[] { "ranch", "hands" } },
				{ 16, new string[] { "spider" } },
				{ 17, new string[] { "large salamander", "fire salamander", "salamander" } },
				{ 18, new string[] { "small salamander", "fire salamander", "salamander" } },
				{ 19, new string[] { "troll" } },
				{ 20, new string[] { "naga" } },
				{ 21, new string[] { "highland goat", "highland sheep", "ibex", "goat", "sheep" } },
			};

			foreach (var synonym in synonyms)
			{
				CreateMonsterSynonyms(synonym.Key, synonym.Value);
			}

			var berescroftMonster = MDB[2];

			Debug.Assert(berescroftMonster != null);

			berescroftMonster.Location = ArchaeologyDepartmentStartRoomUids[RollDice(1, 26, -1)];

			var vigilantGuardMonster = MDB[3];

			Debug.Assert(vigilantGuardMonster != null);

			vigilantGuardMonster.Location = ArchaeologyDepartmentStartRoomUids[RollDice(1, 13, -1)];

			var watchfulGuardMonster = MDB[4];

			Debug.Assert(watchfulGuardMonster != null);

			watchfulGuardMonster.Location = ArchaeologyDepartmentStartRoomUids[RollDice(1, 26, -1)];

			var fieldForemanMonster = MDB[5];

			Debug.Assert(fieldForemanMonster != null);

			fieldForemanMonster.Location = ArchaeologyDepartmentStartRoomUids[RollDice(1, 26, -1)];

			var winslowMonster = MDB[8];

			Debug.Assert(winslowMonster != null);

			winslowMonster.Location = GradStudentStartRoomUids[RollDice(1, 4, -1)];

			var emilieMonster = MDB[9];

			Debug.Assert(emilieMonster != null);

			emilieMonster.Location = GradStudentStartRoomUids[RollDice(1, 4, -1)];

			var ranchHandsMonster = MDB[15];

			Debug.Assert(ranchHandsMonster != null);

			ranchHandsMonster.Location = RanchHandsStartRoomUids[RollDice(1, 2, -1)];

			// A grad student will accompany the player on the adventure!

			gGameState.GradStudentCompanionUid = Character.Gender == Gender.Female ? 8 : 9;
		}

		public override void ResetMonsterStats(IMonster monster)
		{
			Debug.Assert(monster != null && monster.IsCharacterMonster());

			base.ResetMonsterStats(monster);

			// Ensure monster not poisoned

			gGameState.PoisonedTargets.Remove(monster.Uid);

			// TODO: unschedule pending events ???
		}

		public override void MonsterDies(IMonster actorMonster, IMonster dobjMonster)
		{
			Debug.Assert(dobjMonster != null);

			gGameState.PoisonedTargets.Remove(dobjMonster.Uid);

			// TODO: unschedule pending events ???

			base.MonsterDies(actorMonster, dobjMonster);
		}

		public override IList<IMonster> GetHostileMonsterList(IMonster monster)
		{
			Debug.Assert(monster != null);

			var monsterList = base.GetHostileMonsterList(monster);

			// Black spider always attacks player

			if (monster.Uid == 16)
			{
				monsterList = monsterList.Where(m => m.Uid == GameState.Cm).ToList();
			}

			// Only player can attack black spider

			else if (monster.Uid != GameState.Cm)
			{
				monsterList = monsterList.Where(m => m.Uid != 16).ToList();
			}

			return monsterList;
		}

		public virtual void ArchaeologyDepartmentAbandonsExcavationSite()
		{
			// Archaeology Department staff and students disappear

			foreach (var monsterUid in ArchaeologyDepartmentMonsterUids)
			{
				if (monsterUid != gGameState.GradStudentCompanionUid)
				{
					var monster = MDB[monsterUid];

					Debug.Assert(monster != null);

					monster.SetInLimbo();
				}
			}

			// Iron gate left unlocked and open 

			var ironGateArtifact = ADB[5];

			Debug.Assert(ironGateArtifact != null);

			ironGateArtifact.Field2 = 0;

			ironGateArtifact.Field3 = 0;

			// Corral left unlocked and open

			var corralArtifact = ADB[13];

			Debug.Assert(corralArtifact != null);

			corralArtifact.StateDesc = " which has been abandoned";

			corralArtifact.InContainer.Field1 = 0;

			corralArtifact.InContainer.Field2 = 1;

			// Tents/pack animals gone

			var artifactUids = new long[] { 7, 8, 9, 10, 11, 12, 19, 92 };

			foreach (var artifactUid in artifactUids)
			{
				var artifact = ADB[artifactUid];

				Debug.Assert(artifact != null);

				artifact.SetInLimbo();
			}
		}

		public virtual void SteamTurbineStopsRunning()
		{
			// Iron steam turbine

			ADB[29].StateDesc = "";

			gGameState.SteamTurbineRunning = false;

			// Rock crusher

			ADB[48].StateDesc = "";

			gGameState.RockCrusherRunning = false;

			// Rock grinder

			ADB[49].StateDesc = "";

			gGameState.RockGrinderRunning = false;

			// Debris sifter

			ADB[50].StateDesc = "";

			gGameState.DebrisSifterRunning = false;
		}

		public virtual void RockCrusherDestroysContents(IRoom room)
		{
			Debug.Assert(room != null);

			var rockCrusherArtifact = ADB[48];

			Debug.Assert(rockCrusherArtifact != null);

			var artifactList = rockCrusherArtifact.GetContainedList();

			foreach (var artifact in artifactList)
			{
				if (room.IsViewable())
				{
					Out.Print("{0} mangles {1} and tears {2} apart!", rockCrusherArtifact.GetTheName(true), artifact.GetTheName(), artifact.EvalPlural("it", "them"));

					Out.Print("{0} {1} destroyed!", artifact.GetTheName(true), artifact.EvalPlural("is", "are"));
				}

				if (false)
				{
					// TODO: implement
				}
				else
				{
					artifact.SetInLimbo();
				}
			}
		}

		public virtual void RockGrinderDestroysContents(IRoom room)
		{
			Debug.Assert(room != null);

			var rockGrinderArtifact = ADB[49];

			Debug.Assert(rockGrinderArtifact != null);

			var artifactList = rockGrinderArtifact.GetContainedList();

			foreach (var artifact in artifactList)
			{
				if (room.IsViewable())
				{
					Out.Print("{0} mutilates {1} beyond recognition!", rockGrinderArtifact.GetTheName(true), artifact.GetTheName());

					Out.Print("{0} {1} destroyed!", artifact.GetTheName(true), artifact.EvalPlural("is", "are"));
				}

				if (false)
				{
					// TODO: implement
				}
				else
				{
					artifact.SetInLimbo();
				}
			}
		}

		public virtual void DebrisSifterVibratesContents(IRoom room, IArtifact artifact = null)
		{
			Debug.Assert(room != null);

			var debrisSifterArtifact = ADB[50];

			Debug.Assert(debrisSifterArtifact != null);

			var artifactList = debrisSifterArtifact.GetContainedList();

			foreach (var artifact02 in artifactList)
			{
				if (artifact == null || artifact.Uid == artifact02.Uid)
				{
					if (false)
					{
						// TODO: implement
					}
					else if (room.IsViewable())
					{
						Out.Print("{0} vibrates {1} violently but to little harm!", debrisSifterArtifact.GetTheName(true), artifact02.GetTheName());
					}
				}
			}
		}

		public Engine()
		{
			((Framework.Plugin.IEngine)this).Buf = new StringBuilder(BufSize);

			((Framework.Plugin.IEngine)this).Buf01 = new StringBuilder(BufSize);

			ArchaeologyDepartmentMonsterUids = new long[] { 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };

			ArchaeologyDepartmentStartRoomUids = new long[] { 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 35, 129, 130, 131, 132, 133, 155 };

			ArchaeologyDepartmentAbandonedRoomUids = new long[] { 12, 14, 15, 16, 17, 18, 19, 20, 22, 23, 24, 26, 27, 28, 29, 32, 33, 34 };

			GradStudentStartRoomUids = new long[] { 26, 27, 28, 29 };

			RanchHandsStartRoomUids = new long[] { 22, 155 };

			SupportBeamsRoomUids = new long[] { 82, 85, 86, 89, 90, 93, 94, 102, 105, 108, 110, 112 };

			TracksRoomUids = new long[] { 82, 83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 102, 103, 104, 105, 106, 107, 108, 109, 110, 111, 112, 113 };

			LavaRiverRoomUids = new long[] { 57, 60, 62, 64, 65 };

			BroilingRoomUids = new long[] { 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 77, 78, 79, 80, 81 };

			BeachRoomUids = new long[] { 36, 37, 38, 39 };
		}
	}
}
