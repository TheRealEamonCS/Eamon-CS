
// Engine.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using Eamon;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Extensions;
using TheWayfarersInn.Framework.Primitive.Classes;
using TheWayfarersInn.Framework.Primitive.Enums;
using static TheWayfarersInn.Game.Plugin.Globals;

namespace TheWayfarersInn.Game.Plugin
{
	public class Engine : EamonRT.Game.Plugin.Engine, Framework.Plugin.IEngine
	{
		public virtual string AccountingLedgerData { get; set; }

		public virtual string WallMuralData { get; set; }

		public virtual string KitchenMessageData { get; set; }

		public virtual long UnseenApparitionAttacks { get; set; }

		public virtual long SyndicateReward { get; set; }

		public virtual long[][] WanderRoomUids { get; set; }

		public virtual long[] NorthWindowRoomUids { get; set; }

		public virtual long[] SouthWindowRoomUids { get; set; }

		public virtual long[] WestWindowRoomUids { get; set; }

		public virtual long[] InnkeepersQuartersRoomUids { get; set; }

		public virtual long[] GuestRoomUids { get; set; }

		public virtual long[] NonEmotingMonsterUids { get; set; }

		public virtual IList<Action<IRoom>> ForestEventFuncList { get; set; }

		public virtual IList<Action<IRoom>> RiverEventFuncList { get; set; }

		public virtual IList<Action<IRoom, IMonster>> ChildsApparitionEventFuncList { get; set; }

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

			// Bats in loft

			if (artifact.Uid != 116)
			{
				base.PrintMonsterAlive(artifact);
			}
		}

		public override void PrintMonsterEmotes(IMonster monster, bool friendSmile = true)
		{
			Debug.Assert(monster != null);

			var eventState = gGameState.GetEventState(EventState.ChildsApparition);

			var rl = RollDice(1, 100, 0);

			// Blue-banded centipedes

			if (monster.Uid == 3)
			{
				Out.Write("{0}{1} {2} at you.", Environment.NewLine, monster.GetTheName(true), rl > 80 ? monster.EvalPlural("chitters", "chitter") : rl > 50 ? monster.EvalPlural("hisses", "hiss") : monster.EvalPlural("clicks", "click"));
			}

			// Rat swarm / Bat swarm

			else if (monster.Uid == 5 || monster.Uid == 6)
			{
				Out.Write("{0}{1} {2} at you.", Environment.NewLine, monster.GetTheName(true), rl > 80 ? "squeals" : rl > 50 ? "hisses" : "squeaks");
			}

			// Dire wolves

			else if (monster.Uid == 7)
			{
				Out.Write("{0}{1} {2}{3} at you.", Environment.NewLine, monster.GetTheName(true), rl > 80 ? "howl" : rl > 50 ? "snarl" : "growl", monster.EvalPlural("s", ""));
			}

			// Grave robbers

			else if (monster.Uid == 8)
			{
				Out.Write("{0}{1} {2}{3} at you.", Environment.NewLine, monster.GetTheName(true), rl > 50 ? "gesture" : "curse", monster.EvalPlural("s", ""));
			}

			// Giant yellow jackets

			else if (monster.Uid == 9)
			{
				Out.Write("{0}{1} buzz{2} at you.", Environment.NewLine, monster.GetTheName(true), monster.EvalPlural("es", ""));
			}

			// Giant bombardier beetles

			else if (monster.Uid == 10)
			{
				Out.Write("{0}{1} {2} at you.", Environment.NewLine, monster.GetTheName(true), rl > 50 ? monster.EvalPlural("hisses", "hiss") : monster.EvalPlural("clicks", "click"));
			}

			// Giant fire beetles

			else if (monster.Uid == 11)
			{
				Out.Write("{0}{1} {2} at you.", Environment.NewLine, monster.GetTheName(true), rl > 50 ? monster.EvalPlural("hisses", "hiss") : monster.EvalPlural("crackles", "crackle"));
			}

			// Rust monster

			else if (monster.Uid == 20)
			{
				Out.Write("{0}{1} {2} at you.", Environment.NewLine, monster.GetTheName(true), rl > 50 ? "chitters" : "clicks");
			}

			// Peryton

			else if (monster.Uid == 21)
			{
				Out.Write("{0}{1} {2} at you.", Environment.NewLine, monster.GetTheName(true), rl > 80 ? "growls" : rl > 50 ? "squawks" : "screeches");
			}

			// Harpy

			else if (monster.Uid == 22)
			{
				Out.Write("{0}{1} {2} at you.", Environment.NewLine, monster.GetTheName(true), rl > 80 ? "growls" : rl > 50 ? "cries" : "screeches");
			}

			// Hearthwatcher

			else if (monster.Uid == 23)
			{
				Out.Write("{0}{1} {2} at you.", Environment.NewLine, monster.GetTheName(true), rl > 75 ? "growls" : rl > 50 ? "roars" : rl > 25 ? "snarls" : "bellows");
			}

			// Non-emoting monsters

			else if (NonEmotingMonsterUids.Contains(monster.Uid) || (monster.Uid == 4 && (eventState == 2 || eventState == 3)))
			{
				Out.Write("{0}{1} {2} not responsive.", Environment.NewLine, monster.GetTheName(true), monster.EvalPlural("is", "are"));
			}
			else
			{
				base.PrintMonsterEmotes(monster, friendSmile);
			}
		}

		public override void InitRooms()
		{
			base.InitRooms();

			MacroFuncs.Add(6, () =>
			{
				var result = "";

				if (GameState != null)
				{
					var direWolvesMonster = MDB[7];

					Debug.Assert(direWolvesMonster != null);

					var direWolfPupsArtifact = ADB[117];

					Debug.Assert(direWolfPupsArtifact != null);

					var kennelOccupied = direWolvesMonster.IsInRoomUid(22) || direWolfPupsArtifact.IsInRoomUid(22) || direWolfPupsArtifact.IsCarriedByContainerUid(15);

					if (!kennelOccupied)
					{
						result = ", now home to little more than assorted vermin";
					}
				}

				return result;
			});

			MacroFuncs.Add(10, () =>
			{
				var result = "; vibrant green ferns and shrubs grow at their base";

				if (gGameState != null && gGameState.WallMapRead)
				{
					result = ". Vibrant green ferns and shrubs grow at their base; however, a treeline gap leads into the forest depths";
				}

				return result;
			});

			MacroFuncs.Add(24, () =>
			{
				var result = "a framed portrait hangs next to";

				if (GameState != null)
				{
					var framedPortraitArtifact = ADB[30];

					Debug.Assert(framedPortraitArtifact != null);

					if (!framedPortraitArtifact.IsEmbeddedInRoomUid(24))
					{
						result = "you see";
					}
				}

				return result;
			});

			MacroFuncs.Add(25, () =>
			{
				var result = ". A hunter's trophy display hangs above a giant fireplace.";

				if (GameState != null)
				{
					var huntersTrophyDisplayArtifact = ADB[38];

					Debug.Assert(huntersTrophyDisplayArtifact != null);

					if (!huntersTrophyDisplayArtifact.IsEmbeddedInRoomUid(26))
					{
						result = " near a giant fireplace.";
					}
				}

				return result;
			});

			MacroFuncs.Add(26, () =>
			{
				var result = "A saddle rests on a rack";

				if (GameState != null)
				{
					var saddleArtifact = ADB[128];

					Debug.Assert(saddleArtifact != null);

					if (!saddleArtifact.IsEmbeddedInRoomUid(21))
					{
						result = "A rack sits";
					}
				}

				return result;
			});

			MacroFuncs.Add(27, () =>
			{
				var result = "";

				if (GameState != null)
				{
					var poolTableArtifact = ADB[138];

					Debug.Assert(poolTableArtifact != null);

					var dartboardArtifact = ADB[139];

					Debug.Assert(dartboardArtifact != null);

					var woodenChessPiecesArtifact = ADB[140];

					Debug.Assert(woodenChessPiecesArtifact != null);

					var boardGameArtifact = ADB[142];

					Debug.Assert(boardGameArtifact != null);

					var artifactList = new List<IArtifact>()
					{
						poolTableArtifact
					};

					if (dartboardArtifact.IsEmbeddedInRoomUid(27))
					{
						artifactList.Add(dartboardArtifact);
					}

					if (woodenChessPiecesArtifact.IsEmbeddedInRoomUid(27))
					{
						artifactList.Add(woodenChessPiecesArtifact);
					}

					if (boardGameArtifact.IsEmbeddedInRoomUid(27))
					{
						artifactList.Add(boardGameArtifact);
					}

					for (var i = 0; i < artifactList.Count; i++)
					{
						result += string.Format("{0}{1}", i == 0 ? " " : i == artifactList.Count - 1 && artifactList.Count > 2 ? ", and " : i == artifactList.Count - 1 ? " and " : ", ", artifactList[i].GetArticleName());
					}
				}

				return result;
			});

			MacroFuncs.Add(29, () =>
			{
				var result = "a towering bear statue whose gaze seems to follow you";

				var giantWoodenStatueArtifact = ADB[28];

				Debug.Assert(giantWoodenStatueArtifact != null);

				if (giantWoodenStatueArtifact.IsInLimbo())
				{
					result = "an open space where the towering bear statue once stood";
				}

				return result;
			});

			MacroFuncs.Add(54, () =>
			{
				var result = " is dry";

				var waterArtifact = ADB[60];

				Debug.Assert(waterArtifact != null);

				if (waterArtifact.IsCarriedByContainerUid(24))
				{
					result = " contains water";
				}

				return result;
			});

			MacroFuncs.Add(57, () => IsKitchenShelfBalanced() ? "" : " crooked");

			var roomDescTemplate = @"You are in {0} room{1} with {2} furniture set{3}. The {4} window in the {5} wall overlooks {6} and casts {7} glow across the space. The {8} air smells faintly of {9}. {10}";

			var adjectiveList = new List<string>() { "a dimly lit", "a musty", "a dusty", "a dirty", "a tidy", "a ramshackle", "an unremarkable" };

			var colorList = new List<string>() { "a weathered oak", "a worn ebony", "an aged mahogany", "a faded pine", "a dark walnut", "a rustic chestnut" };

			var windowNuanceList = new List<string>() { "cracked", "broken", "dust-covered", "cobweb-covered", "dirty", "grimy" };

			var wallTypeList = new List<string>() { "stone", "wooden", "moss-covered", "crumbling", "damp", "grungy", "textured", "plastered", "weathered", "rough", "bare" };

			var sceneryList = new List<string>() { "tall trees", "green grass", "blooming wildflowers", "a carpet of fallen leaves", "a thicket of ferns", "a jumble of mossy rocks", "tangled forest undergrowth", "a canopy of green foliage" };

			var lightList = new List<string>() { "a flickering", "a diffused", "a dancing", "a dim", "a faint", "a murky", "an amber" };

			var temperatureList = new List<string>() { "comfortable", "pleasant", "chilly", "cold", "warm", "hot", "sweltering", "stifling", "stagnant" };

			var scentList = new List<string>() { "mold", "decay", "damp moss", "fresh grass", "pine needles", "aromatic bark", "fruit", "rust", "lavender", "vanilla" };

			var usedAdjectiveList = new List<string>();

			var usedColorList = new List<string>();

			var usedWindowNuanceList = new List<string>();

			var usedWallTypeList = new List<string>();

			var usedSceneryList = new List<string>();

			var usedLightList = new List<string>();

			var usedTemperatureList = new List<string>();

			var usedScentList = new List<string>();

			// Procedurally generate guest room Descs

			foreach (var roomUid in GuestRoomUids)
			{
				var room = RDB[roomUid];

				Debug.Assert(room != null);

				room.Desc = string.Format(roomDescTemplate,
					GetNonRepeatingRandomElement(adjectiveList, usedAdjectiveList),
					room.Uid == 42 ? " adorned with glyphs, transformed into a fortified outpost" :
					room.Uid == 63 ? ", clearly under renovation" :
					"",
					GetNonRepeatingRandomElement(colorList, usedColorList),
					room.Uid == 42 ? ", strategically placed" :
					room.Uid == 63 ? ", haphazardly placed" :
					"",
					GetNonRepeatingRandomElement(windowNuanceList, usedWindowNuanceList),
					GetNonRepeatingRandomElement(wallTypeList, usedWallTypeList),
					GetNonRepeatingRandomElement(sceneryList, usedSceneryList),
					GetNonRepeatingRandomElement(lightList, usedLightList),
					GetNonRepeatingRandomElement(temperatureList, usedTemperatureList),
					GetNonRepeatingRandomElement(scentList, usedScentList),
					room.Uid == 42 || room.Uid == 59 ? " There is a jagged breach in the north wall." :
					room.Uid == 45 || room.Uid == 57 ? " There is a jagged breach in the south wall." :
					"");
			}
		}

		public override void InitArtifacts()
		{
			base.InitArtifacts();

			MacroFuncs.Add(1, () =>
			{
				return Encoding.UTF8.GetString(Convert.FromBase64String("QmVhdXR5IGlzIGluIHRoZSBleWUgb2YgdGhlIGJlaG9sZGVy"));
			});

			MacroFuncs.Add(7, () =>
			{
				var result = "";

				var southDoorArtifact = ADB[89];

				Debug.Assert(southDoorArtifact != null);

				if (southDoorArtifact.DoorGate.IsOpen())
				{
					result = " A series of deep gouges mar the inner wood, as if made by some large animal.";
				}

				return result;
			});

			MacroFuncs.Add(8, () =>
			{
				var result = "";

				var eastDoorArtifact = ADB[95];

				Debug.Assert(eastDoorArtifact != null);

				if (eastDoorArtifact.DoorGate.IsOpen())
				{
					result = " The outer wood is slightly faded, as if exposed to sunlight for an extended period.";
				}

				return result;
			});

			MacroFuncs.Add(9, () =>
			{
				var result = "";

				var northDoorArtifact = ADB[107];

				Debug.Assert(northDoorArtifact != null);

				if (northDoorArtifact.DoorGate.IsOpen())
				{
					result = " The door creaks loudly as you push it open.";
				}

				return result;
			});

			MacroFuncs.Add(12, () =>
			{
				return Character != null ? Character.Name : UnknownName;
			});

			MacroFuncs.Add(15, () =>
			{
				var result = "";

				var diaryArtifact = ADB[85];

				Debug.Assert(diaryArtifact != null);

				if (diaryArtifact.Readable.IsOpen())
				{
					result = ", but the ink is still legible. It contains handwritten entries dating back years";
				}

				return result;
			});

			MacroFuncs.Add(16, () =>
			{
				var hauntingArtifact = ADB[151];

				Debug.Assert(hauntingArtifact != null);

				return string.Format("{0}{1}", hauntingArtifact.GetArticleName(), hauntingArtifact.StateDesc);
			});

			MacroFuncs.Add(17, () =>
			{
				var reactionStringArray = new string[] { "chills you to the bone", "makes your skin crawl", "gives you goosebumps", "fills you with dread" };

				var hauntingArtifact = ADB[151];

				Debug.Assert(hauntingArtifact != null);

				return reactionStringArray[hauntingArtifact.Field1];
			});

			MacroFuncs.Add(18, () =>
			{
				GuestRoomData guestRoomData = null;

				return GameState != null && gGameState.GuestRoomDictionary != null && gGameState.GuestRoomDictionary.TryGetValue(GameState.Ro, out guestRoomData) && guestRoomData != null && !string.IsNullOrWhiteSpace(guestRoomData.FurnitureSetDesc) ? guestRoomData.FurnitureSetDesc : "A154";
			});

			MacroFuncs.Add(19, () =>
			{
				var effectUid = 0L;

				var arcaneCookbookArtifact = ADB[171];

				Debug.Assert(arcaneCookbookArtifact != null);

				switch (arcaneCookbookArtifact.ParserMatchName)
				{
					case "mystic mornings":
					case "mystic mornings section":

						effectUid = 114;

						break;

					case "enchanted noons":
					case "enchanted noons section":

						effectUid = 115;

						break;

					case "twilight enchantments":
					case "twilight enchantments section":

						effectUid = 116;

						break;

					case "sorcerer's sweets":
					case "sorcerer's sweets section":

						effectUid = 117;

						break;

					case "ethereal elixers":
					case "ethereal elixers section":

						effectUid = 118;

						break;

					default:

						effectUid = 119;
						
						break;
				}

				var effect = EDB[effectUid];

				Debug.Assert(effect != null);

				return effect.Desc;
			});

			MacroFuncs.Add(20, () =>
			{
				var result = "";

				var accountingLedgerArtifact = ADB[68];

				Debug.Assert(accountingLedgerArtifact != null);

				if (accountingLedgerArtifact.Readable.IsOpen())
				{
					result = " with faded ink, crossed-out entries, and smudges";
				}

				return result;
			});

			MacroFuncs.Add(21, () =>
			{
				var result = "pups";

				var direWolvesMonster = MDB[7];

				Debug.Assert(direWolvesMonster != null);

				if (direWolvesMonster.IsInLimbo())
				{
					result = "orphans";
				}

				return result;
			});

			MacroFuncs.Add(22, () =>
			{
				var result = "eastern";

				if (GameState != null && GameState.Ro == 7)
				{
					result = "western";
				}

				return result;
			});

			MacroFuncs.Add(31, () =>
			{
				var result = "";

				var direWolfPupsArtifact = ADB[117];

				Debug.Assert(direWolfPupsArtifact != null);

				if (!direWolfPupsArtifact.IsCarriedByContainerUid(15))
				{
					result = "empty and ";
				}

				return result;
			});

			// These MacroFuncs can be far more compact when needed

			MacroFuncs.Add(32, () => ADB[11].Moved ? "" : " sitting on a shelf");      // Rusty oil lantern

			MacroFuncs.Add(33, () => ADB[13].Moved ? "You see a shovel" : "A shovel lies nearby");      // Shovel

			MacroFuncs.Add(34, () => ADB[34].Moved ? "You see a moldy burlap sack" : "A moldy burlap sack lies discarded in a dimly lit corner of the cellar");      // Burlap sack

			MacroFuncs.Add(35, () => ADB[46].Moved ? "You see a mop here" : "A mop leans against the wall");      // Mop

			MacroFuncs.Add(36, () => ADB[55].Moved ? "" : " clutched tightly in the skeleton's arms");      // Teddy bear

			MacroFuncs.Add(37, () => ADB[87].Moved ? " has been" : " lies on the floor nearby,");      // Severed hand

			MacroFuncs.Add(38, () => ADB[93].Moved ? "" : " lies nearby on a dark stain in the floor");      // Desiccated thigh bone

			MacroFuncs.Add(39, () => ADB[102].Moved ? " has been" : " lies on the floor,");      // Severed arm

			MacroFuncs.Add(40, () => ADB[111].Moved ? " here" : " lying in a corner");      // Broom

			MacroFuncs.Add(41, () => ADB[112].Moved ? "" : " propped against the wall");      // Ladder

			MacroFuncs.Add(42, () => ADB[113].Moved ? " have" : " are scattered across the floor, their");      // Paint cans

			MacroFuncs.Add(43, () => ADB[117].Moved ? " wriggle" : " roll around in a sunbeam");      // Dire wolf pups

			MacroFuncs.Add(44, () => ADB[123].Moved ? "You see" : "Behind the bales of hay, you spot");      // Glass jar

			MacroFuncs.Add(45, () => ADB[124].Moved ? "" : ", draped over large objects of various sizes throughout the barn. They appear oddly lumpy in places as if something is moving underneath");      // Canvas covers

			MacroFuncs.Add(46, () => ADB[127].Moved ? " that were once supple are" : " hang over the sides of the stalls, once supple but");      // Horse harnesses

			MacroFuncs.Add(47, () => ADB[128].Moved ? "" : " perched atop a wooden rack");      // Saddle

			MacroFuncs.Add(48, () => ADB[139].Moved ? " is present" : " hangs on the back wall of the room");      // Dartboard

			MacroFuncs.Add(49, () => ADB[140].Moved ? "You see some wooden chess pieces" : "Amidst the debris, you spot wooden chess pieces scattered across the floor");      // Wooden chess pieces

			MacroFuncs.Add(50, () => ADB[164].Moved ? " is here" : " hangs on the workshop wall");      // Hammer

			MacroFuncs.Add(51, () => ADB[167].Moved ? "You see" : "Tucked under the workbench, you find");      // Forgecraft Codex

			MacroFuncs.Add(52, () => ADB[174].Moved ? "" : " on the shelf");      // Plates

			MacroFuncs.Add(53, () => ADB[175].Moved ? " are" : " sit on the shelf by the plates,");      // Utensils

			MacroFuncs.Add(55, () =>
			{
				var result = " It has long been empty, with no sign of water remaining, only a layer of dust.";

				var waterArtifact = ADB[60];

				Debug.Assert(waterArtifact != null);

				if (waterArtifact.IsCarriedByContainerUid(24))
				{
					result = " It has been filled with water.";
				}

				return result;
			});

			MacroFuncs.Add(56, () =>
			{
				var result = " at an odd angle. Its unevenness adds a sense of disarray to the room.";

				if (GameState != null)
				{
					if (gGameState.KitchenRiddleState == 2)
					{
						result = " at an odd angle, roughly balanced by the placement of the plates.";
					}
					else if (IsKitchenShelfBalanced())
					{
						result = ", finely balanced by the placement of the plates and utensils.";
					}
				}

				return result;
			});

			var synonyms = new Dictionary<long, string[]>()
			{
				{ 1, new string[] { "rickety suspension bridge", "wooden suspension bridge", "rickety bridge", "wooden bridge", "bridge" } },
				{ 2, new string[] { "weathered sign", "wooden sign" } },
				{ 3, new string[] { "west wing", "west building", "wayfarers inn", "inn" } },
				{ 4, new string[] { "east wing", "east building", "collapsed east wing", "collapsed east building", "collapsed wing", "collapsed building", "wayfarers inn", "inn" } },
				{ 5, new string[] { "supplies", "supply" } },
				{ 6, new string[] { "bottle of oil", "kerosene bottle", "oil bottle", "bottle", "kerosene", "oil", "label" } },
				{ 7, new string[] { "toolshed door", "shed door", "shed", "door" } },
				{ 8, new string[] { "small barn door", "barn door", "barn", "door" } },
				{ 9, new string[] { "stables door", "stable door", "stables", "door" } },
				{ 10, new string[] { "kennel door", "door" } },
				{ 11, new string[] { "rusty oil lamp", "rusty lantern", "rusty lamp", "oil lantern", "oil lamp", "lantern", "lamp" } },
				{ 12, new string[] { "pick axe", "pick" } },
				{ 16, new string[] { "tree line gap", "treeline", "tree line", "gap", "footpath", "small path", "path" } },
				{ 17, new string[] { "well" } },
				{ 18, new string[] { "reinforced doors", "reinforced door", "double doors", "double door", "doors", "door" } },
				{ 19, new string[] { "welcome sign" } },
				{ 20, new string[] { "stone temple", "stone temple door", "stone door", "temple door", "door" } },
				{ 21, new string[] { "graveyard", "cemetary" } },
				{ 24, new string[] { "pool" } },
				{ 25, new string[] { "gravestones", "gravestone", "tombstones", "tombstone", "gravesites", "gravesite", "markers", "marker" } },
				{ 26, new string[] { "desk" } },
				{ 27, new string[] { "key ring", "keys", "key" } },
				{ 28, new string[] { "giant wood statue", "towering bear statue", "towering statue", "bear statue", "giant statue", "wooden statue", "wood statue", "statue", "statue base", "base", "giant bear", "bear", "ancient runes", "runes", "rune", "script", "text" } },
				{ 29, new string[] { "clock" } },
				{ 30, new string[] { "framed picture", "portrait", "picture" } },
				{ 31, new string[] { "plaque" } },
				{ 32, new string[] { "doors", "door" } },
				{ 33, new string[] { "cellar", "door" } },
				{ 34, new string[] { "sack" } },
				{ 35, new string[] { "great stairway", "great stairs", "staircase", "stairway", "stairs" } },
				{ 36, new string[] { "leather sectional", "leather sofas", "leather sofa", "leather furniture", "sectionals", "sectional", "sofas", "sofa", "furniture" } },
				{ 37, new string[] { "fireplace" } },
				{ 38, new string[] { "hunter's display", "trophy display", "display" } },
				{ 39, new string[] { "dry fountain" } },
				{ 41, new string[] { "large oak tree", "majestic tree", "large tree", "oak tree", "oak", "tree" } },
				{ 42, new string[] { "large hole", "wall hole", "wall", "hole" } },
				{ 43, new string[] { "gnawed rib cage", "gnawed ribs", "ribcage", "rib cage", "ribs" } },
				{ 44, new string[] { "door" } },
				{ 45, new string[] { "door" } },
				{ 48, new string[] { "boiler" } },
				{ 49, new string[] { "complex ducting network", "complex network", "ducting network", "complex ducting", "network of ducting", "network", "ducting", "ducts" } },
				{ 50, new string[] { "giant spider webs", "giant spider web", "giant webs", "giant web", "spiderwebs", "spiderweb", "spider webs", "spider web", "webs", "web" } },
				{ 51, new string[] { "body part pile", "body pile", "part pile", "pile of parts", "pile of bodies", "body parts", "pile", "parts" } },
				{ 52, new string[] { "head" } },
				{ 53, new string[] { "crumbling wall", "brick wall", "back wall", "wall" } },
				{ 54, new string[] { "child skeleton", "child's bones", "child bones", "skeleton", "bones" } },
				{ 55, new string[] { "stuffed bear", "stuffed toy", "teddy", "bear", "toy" } },
				{ 56, new string[] { "tanks", "tank" } },
				{ 57, new string[] { "moldy goop", "black goop", "goop" } },
				{ 58, new string[] { "freshly dug hole", "fresh hole", "dug hole", "ground hole", "open grave", "hole", "grave" } },
				{ 59, new string[] { "dirt mound", "mound", "dirt" } },
				{ 61, new string[] { "door" } },
				{ 62, new string[] { "desk" } },
				{ 63, new string[] { "bookshelves", "bookshelf", "cases", "case", "shelves", "shelf" } },
				{ 64, new string[] { "cabinets", "cabinet" } },
				{ 65, new string[] { "expense work sheets", "worksheets", "worksheet", "work sheets", "work sheet", "sheets", "sheet" } },
				{ 66, new string[] { "documents", "document" } },
				{ 68, new string[] { "ledger" } },
				{ 69, new string[] { "journal" } },
				{ 70, new string[] { "deed" } },
				{ 71, new string[] { "full body-length mirror", "full-length mirror", "full mirror" } },
				{ 72, new string[] { "narrow stairway", "narrow stairs", "staircase", "stairway", "stairs" } },
				{ 73, new string[] { "door" } },
				{ 74, new string[] { "door" } },
				{ 75, new string[] { "sturdy table" } },
				{ 77, new string[] { "shelf" } },
				{ 78, new string[] { "neglected sofa" } },
				{ 79, new string[] { "bed" } },
				{ 81, new string[] { "night stand", "stand" } },
				{ 82, new string[] { "four poster bed", "bed" } },
				{ 84, new string[] { "night stand", "stand" } },
				{ 86, new string[] { "door", "intricate glyph", "glyph" } },
				{ 87, new string[] { "hand" } },
				{ 88, new string[] { "door" } },
				{ 89, new string[] { "door" } },
				{ 90, new string[] { "door" } },
				{ 91, new string[] { "door" } },
				{ 92, new string[] { "map" } },
				{ 93, new string[] { "desiccated thighbone", "desiccated bone", "thigh bone", "thighbone", "bone" } },
				{ 94, new string[] { "great stairway", "great stairs", "staircase", "stairway", "stairs" } },
				{ 95, new string[] { "door" } },
				{ 96, new string[] { "east wing", "east building", "collapsed east wing", "collapsed east building", "collapsed wing", "collapsed building", "wayfarers inn", "inn" } },
				{ 97, new string[] { "court", "yard" } },
				{ 98, new string[] { "spire" } },
				{ 99, new string[] { "majestic tree", "oak tree", "oak", "tree" } },
				{ 100, new string[] { "door" } },
				{ 101, new string[] { "door" } },
				{ 102, new string[] { "arm" } },
				{ 103, new string[] { "door" } },
				{ 104, new string[] { "door" } },
				{ 105, new string[] { "door" } },
				{ 106, new string[] { "door" } },
				{ 107, new string[] { "door" } },
				{ 108, new string[] { "door" } },
				{ 109, new string[] { "mural" } },
				{ 110, new string[] { "foot" } },
				{ 113, new string[] { "cans", "can" } },
				{ 114, new string[] { "note book", "notes", "book" } },
				{ 115, new string[] { "ancient tools", "ancient machinery", "old tools", "old machinery", "machinery" } },
				{ 117, new string[] { "wolf pups", "wolf puppies", "wolf puppy", "pups", "puppies", "puppy" } },
				{ 119, new string[] { "paper scrap", "scrap", "paper" } },
				{ 121, new string[] { "old wood chest", "old chest", "wooden chest", "wood chest", "chest" } },
				{ 122, new string[] { "hay bales", "hay bale", "hay", "bales", "bale" } },
				{ 123, new string[] { "gold coins", "gold coin", "coins", "coin", "jar" } },
				{ 124, new string[] { "canvas", "covers", "cover" } },
				{ 125, new string[] { "furniture" } },
				{ 126, new string[] { "horseshoes", "horseshoe", "pile", "shoes", "shoe" } },
				{ 127, new string[] { "harnesses", "harness" } },
				{ 130, new string[] { "bourbon bottle", "liquor bottle", "bottle", "bourbon", "alcohol", "liquor", "Jax Darnel's", "label" } },
				{ 131, new string[] { "note" } },
				{ 133, new string[] { "cabinet" } },
				{ 138, new string[] { "table" } },
				{ 139, new string[] { "dart board", "darts", "board" } },
				{ 140, new string[] { "wooden pieces", "chess pieces", "pieces" } },
				{ 141, new string[] { "card deck", "cards", "deck" } },
				{ 142, new string[] { "board game", "board", "game" } },
				{ 143, new string[] { "pine forest", "pine trees", "pine tree", "pines", "pine", "trees", "tree", "vegetation", "undergrowth", "overgrowth", "mosses", "moss", "ferns", "fern", "wildflowers", "wildflower", "flowers", "flower", "shrubs", "shrub", "plants", "plant" } },
				{ 144, new string[] { "rocks", "stones", "granite formation", "granite", "formation" } },
				{ 145, new string[] { "tiaga gorge", "tiaga", "canyon", "ravine", "cliffs", "cliff" } },
				{ 146, new string[] { "stream", "whitewater", "rushing water", "water", "rapids", "whirlpools", "whirlpool", "waterfalls", "waterfall" } },
				{ 147, new string[] { "clearing", "meadow", "forest", "trees", "tree", "undergrowth", "grasses", "grass", "mosses", "moss", "ferns", "fern", "wildflowers", "wildflower", "flowers", "flower", "shrubs", "shrub", "plants", "plant" } },
				{ 149, new string[] { "bugs", "bug", "butterflies", "butterfly", "bees", "bee", "flies", "fly" } },
				{ 154, new string[] { "set of furniture", "furniture", "set", "bed", "nightstand", "night stand", "stand", "dresser", "drawers", "armchair", "arm chair", "chair", "writing desk", "desk" } },
				{ 162, new string[] { "dusty workbench", "dusty bench", "bench" } },
				{ 163, new string[] { "bellows" } },
				{ 164, new string[] { "hammers" } },
				{ 165, new string[] { "tongs", "tong", "vices", "vice", "files", "file", "rasps", "rasp", "grindstones", "grindstone", "chisels", "chisel", "punches", "punch", "aprons", "apron", "gloves", "glove" } },
				{ 166, new string[] { "trough" } },
				{ 167, new string[] { "forgecraft book", "codex", "book", "tome" } },
				{ 168, new string[] { "wooden table", "wood table" } },
				{ 169, new string[] { "pots", "pans", "pot", "pan" } },
				{ 170, new string[] { "hearth" } },
				{ 171, new string[] { "arcane book", "cookbook", "book", "tome", "mystic mornings", "mystic mornings section", "enchanted noons", "enchanted noons section", "twilight enchantments", "twilight enchantments section", "sorcerer's sweets", "sorcerer's sweets section", "ethereal elixers", "ethereal elixers section" } },
				{ 173, new string[] { "crooked shelf", "oddly angled shelf", "oddly-angled shelf" } },
				{ 176, new string[] { "riddle" } },
				{ 177, new string[] { "armor" } },
				{ 178, new string[] { "shoes" } },
				{ 179, new string[] { "staff" } },
				{ 180, new string[] { "mace" } },
				{ 181, new string[] { "spear" } },
				{ 182, new string[] { "long bow", "bow" } },
				{ 183, new string[] { "dummy" } },
				{ 184, new string[] { "wardrobe", "sturdy padlock", "padlock", "armoire lock", "lock" } },
				{ 185, new string[] { "clothing", "fine outfit", "outfit", "fine clothes", "clothes" } },
				{ 187, new string[] { "supplies", "rope", "grappling hook", "hook", "spy glass", "spyglass", "compass" } },
				{ 188, new string[] { "unreachable loft" } },

			};

			foreach (var synonym in synonyms)
			{
				CreateArtifactSynonyms(synonym.Key, synonym.Value);
			}

			var foundArtifactRoomUids = GuestRoomUids.Where(uid => uid != 63).ToList();

			Shuffle(foundArtifactRoomUids);

			foundArtifactRoomUids = foundArtifactRoomUids.Take(6).ToList();

			// Procedurally generate found artifacts

			var foundArtifactNicknacksArray = new[]
			{
				new { Name = "weathered book", Desc = "You discover a weathered book of faded illustrations.", Weight = 5, Synonyms = new string[] { "book" } },
				new { Name = "pocket watch", Desc = "You discover a broken pocket watch, silent and still.", Weight = 1, Synonyms = new string[] { "watch" } },
				new { Name = "copper hairpin", Desc = "You discover a tarnished copper hairpin with delicate engravings.", Weight = 0, Synonyms = new string[] { "hairpin", "hair pin" } },
				new { Name = "chipped teacup", Desc = "You discover a chipped teacup with a faded floral design.", Weight = 1, Synonyms = new string[] { "chipped tea cup", "teacup", "tea cup" } },
				new { Name = "wooden marionette", Desc = "You discover a wooden marionette with tangled strings and faded paint.", Weight = 3, Synonyms = new string[] { "wooden puppet", "marionette", "puppet" } },
				new { Name = "dried flower", Desc = "You discover a dried, pressed flower between the pages of a diary.", Weight = 0, Synonyms = new string[] { "flower" } },
				new { Name = "rusty key", Desc = "You discover a rusty key that doesn't match any locks nearby.", Weight = 1, Synonyms = new string[] { "key" } },
				new { Name = "lace handkerchief", Desc = "You discover a tattered lace handkerchief with embroidered initials.", Weight = 0, Synonyms = new string[] { "handkerchief", "hanky" } },
				new { Name = "feather quill", Desc = "You discover a feather quill with worn and faded plumage.", Weight = 1, Synonyms = new string[] { "quill" } },
				new { Name = "faded painting", Desc = "You discover a faded painting portraying a picturesque landscape.", Weight = 7, Synonyms = new string[] { "painting" } },
			};

			var foundArtifactNicknacksList = foundArtifactNicknacksArray.ToList();
			
			Shuffle(foundArtifactNicknacksList);

			foundArtifactNicknacksList = foundArtifactNicknacksList.Take(3).ToList();

			for (var i = 0; i < 3; i++)
			{
				var foundArtifact = ADB[155 + i];

				Debug.Assert(foundArtifact != null);

				foundArtifact.Name = CloneInstance(foundArtifactNicknacksList[i].Name);

				foundArtifact.Desc = CloneInstance(foundArtifactNicknacksList[i].Desc);

				foundArtifact.Value = RollDice(1, 3, -1);

				foundArtifact.Weight = foundArtifactNicknacksList[i].Weight;

				foundArtifact.Synonyms = CloneInstance(foundArtifactNicknacksList[i].Synonyms);
			}

			var foundArtifactValuablesArray = new[]
			{
				new { Name = "jeweled pendant", Desc = "You discover an ornate pendant adorned with jewels and an unknown crest.", IsPlural = false, Weight = 1, Synonyms = new string[] { "ornate pendant", "pendant" } },
				new { Name = "silver candelabra", Desc = "You discover a silver candelabra with intricate carvings.", IsPlural = false, Weight = 7, Synonyms = new string[] { "candelabra" } },
				new { Name = "tarnished coin", Desc = "You discover some tarnished coins from a lost kingdom of antiquity.", IsPlural = true, Weight = 10, Synonyms = new string[] { "coins", "coin" } },
				new { Name = "music box", Desc = "You discover an exquisite wooden music box with a haunting melody.", IsPlural = false, Weight = 3, Synonyms = new string[] { "box" } },
				new { Name = "gemstone", Desc = "You discover a collection of valuable gemstones wrapped in a velvet cloth.", IsPlural = true, Weight = 5, Synonyms = new string[] { "gems", "gemstone collection", "collection" } },
				new { Name = "crystal vial", Desc = "You discover a small, delicate crystal vial filled with shimmering dust.", IsPlural = false, Weight = 2, Synonyms = new string[] { "vial" } },
				new { Name = "feathered mask", Desc = "You discover an elaborate mask adorned with feathers and gemstones.", IsPlural = false, Weight = 3, Synonyms = new string[] { "mask" } },
				new { Name = "moth specimen", Desc = "You discover a rare and perfectly preserved moth specimen encased in glass.", IsPlural = false, Weight = 5, Synonyms = new string[] { "moth", "specimen" } },
				new { Name = "crystal sphere", Desc = "You discover a set of delicate, crystal-clear spheres, each containing a mesmerizing scene of distant landscapes.", IsPlural = true, Weight = 6, Synonyms = new string[] { "spheres", "sphere" } },
				new { Name = "gold letter opener", Desc = "You discover an elegant gold letter opener with subtle engravings on the blade.", IsPlural = false, Weight = 2, Synonyms = new string[] { "gold opener", "letter opener", "opener" } },
			};

			var foundArtifactValuablesList = foundArtifactValuablesArray.ToList();

			Shuffle(foundArtifactValuablesList);

			foundArtifactValuablesList = foundArtifactValuablesList.Take(3).ToList();

			for (var i = 0; i < 3; i++)
			{
				var foundArtifact = ADB[158 + i];

				Debug.Assert(foundArtifact != null);

				foundArtifact.Name = CloneInstance(foundArtifactValuablesArray[i].Name);

				foundArtifact.Desc = CloneInstance(foundArtifactValuablesArray[i].Desc);

				foundArtifact.IsPlural = foundArtifactValuablesArray[i].IsPlural;

				if (foundArtifact.IsPlural)
				{
					foundArtifact.ArticleType = ArticleType.Some;

					foundArtifact.PluralType = PluralType.S;
				}

				foundArtifact.Value = 20;

				foundArtifact.Weight = foundArtifactValuablesArray[i].Weight;

				foundArtifact.Synonyms = CloneInstance(foundArtifactValuablesArray[i].Synonyms);
			}

			var containerTypeArray = new ContainerType[] { ContainerType.In, ContainerType.On, ContainerType.Under, ContainerType.Behind };

			var furnitureSetDescTemplate = @"You see {0}, {1}, and {2}. Nearby sits {3} and {4}.";

			var bedList = new List<string>() { "a creaky four-poster bed draped in dusty curtains", "a rusted iron-framed bed with tattered beddings", "a makeshift bed with a lumpy straw mattress", "a tattered canopy bed with moth-eaten curtains", "a worn-out sleigh bed with creaky springs" };

			var nightstandList = new List<string>() { "a bedside nightstand with peeling paint", "a small nightstand with chipped edges", "a wobbly nightstand stacked with old candles", "a chipped nightstand with a cracked oil lamp", "a weathered nightstand with a stack of yellowed letters" };

			var dresserList = new List<string>() { "a cracked wooden dresser with missing knobs", "a dusty dresser with drawers slightly ajar", "a cracked dresser missing a drawer", "a dusty dresser with a broken mirror", "a peeling dresser with mismatched knobs" };

			var armchairList = new List<string>() { "a moth-eaten armchair covered in cobwebs", "a sagging armchair with torn upholstery", "a tattered armchair with an unraveling cushion", "a threadbare armchair with a missing leg", "a faded armchair with cobwebs in the corners" };

			var writingDeskList = new List<string>() { "a weathered writing desk with faded ink stains", "a warped writing desk with a scratched surface", "a splintered writing desk with dry inkwells", "a warped writing desk with moldy parchment", "a splintered writing desk with crumbling quills" };

			var usedBedList = new List<string>();

			var usedNightstandList = new List<string>();

			var usedDresserList = new List<string>();

			var usedArmchairList = new List<string>();

			var usedWritingDeskList = new List<string>();

			// Procedurally generate guest room furniture sets

			foreach (var roomUid in GuestRoomUids)
			{
				var guestRoomData = new GuestRoomData();

				guestRoomData.FurnitureSetDesc = string.Format(furnitureSetDescTemplate,
					GetNonRepeatingRandomElement(bedList, usedBedList),
					GetNonRepeatingRandomElement(nightstandList, usedNightstandList),
					GetNonRepeatingRandomElement(dresserList, usedDresserList),
					GetNonRepeatingRandomElement(armchairList, usedArmchairList),
					GetNonRepeatingRandomElement(writingDeskList, usedWritingDeskList));

				var i = foundArtifactRoomUids.IndexOf(roomUid);

				// Add found artifact to furniture set

				if (i >= 0)
				{
					guestRoomData.FoundArtifactContainerType = GetRandomElement(containerTypeArray);

					guestRoomData.FoundArtifactUid = 155 + i;
				}

				gGameState.GuestRoomDictionary.Add(roomUid, guestRoomData);
			}

			var fineClothingDescTemplate = @"The fine clothing ensemble consists of {0}.";

			var fineClothingDescArray = new string[]
			{
				"a muted blue and gray jacket, a loose-fitting shirt, trousers, knee-high boots, a silk scarf, and a wide-brimmed hat",
				"a deep purple and gold waistcoat, flowing shirt, fitted trousers, knee-high boots, a wide belt, and a feathered hat",
				"a rustic brown tunic, rugged trousers, a hooded cloak, leather boots, and a wide leather belt",
				"a burgundy jacket with silver patterns, a white shirt, fitted trousers, leather boots, a silk ascot, and a wide-brimmed hat",
				"a sandy beige tunic, wide-legged trousers, patterned scarf, leather vest, leather boots, and some beaded bracelets",
				"a deep violet robe with silver celestial patterns, fitted trousers, velvet cloak, knee-high boots, and a moon-shaped pendant",
				"a fitted leather jacket, comfortable shirt, well-worn trousers, sturdy boots, wide belt, and leather wristbands",
				"an emerald waistcoat, gold filigree, ruffled shirt, slim trousers, polished leather shoes, pocket watch chain, and emerald brooch",
			};

			// Procedurally generate fine clothing

			var fineClothingArtifact = ADB[185];

			Debug.Assert(fineClothingArtifact != null);

			fineClothingArtifact.Desc = string.Format(fineClothingDescTemplate, GetRandomElement(fineClothingDescArray));
		}

		public override void InitMonsters()
		{
			base.InitMonsters();

			MacroFuncs.Add(59, () =>
			{
				var nolanMonster = MDB[24];

				return GetMonsterWeaponName(nolanMonster);
			});

			var synonyms = new Dictionary<long, string[]>()
			{
				{ 1, new string[] { "pudding" } },
				{ 2, new string[] { "eerie presence", "unseen ghost", "unseen entity", "unseen spirit", "unseen poltergeist", "apparition", "presence", "ghost", "entity", "spirit", "poltergeist" } },
				{ 3, new string[] { "blue banded centipedes", "centipedes", "centipede" } },
				{ 4, new string[] { "child apparition", "child ghost", "child", "apparition", "ghost", "Charlotte", "girl" } },
				{ 5, new string[] { "swarm of rats", "swarm", "rats" } },
				{ 6, new string[] { "swarm of bats", "swarm", "bats" } },
				{ 7, new string[] { "wolves", "wolf" } },
				{ 8, new string[] { "robbers", "crypt crashers", "crashers", "robber", "crasher" } },
				{ 9, new string[] { "giant jackets", "giant jacket", "yellow jackets", "yellow jacket", "jackets", "jacket", "insects", "insect" } },
				{ 10, new string[] { "giant beetles", "giant beetle", "bombardier beetles", "bombardier beetle", "beetles", "beetle", "insects", "insect" } },
				{ 11, new string[] { "giant beetles", "giant beetle", "fire beetles", "fire beetle", "beetles", "beetle", "insects", "insect" } },
				{ 12, new string[] { "giant recluses", "giant recluse", "brown recluses", "brown recluse", "giant spiders", "giant spider", "recluses", "recluse", "spiders", "spider" } },
				{ 13, new string[] { "giant widows", "giant widow", "black widows", "black widow", "giant spiders", "giant spider", "widows", "widow", "spiders", "spider" } },
				{ 14, new string[] { "fungus" } },
				{ 15, new string[] { "shaman", "gnoll" } },
				{ 16, new string[] { "warrior", "gnolls", "gnoll" } },
				{ 17, new string[] { "tracker", "gnoll" } },
				{ 18, new string[] { "bloom" } },
				{ 19, new string[] { "vines", "vine" } },
				{ 20, new string[] { "monster" } },
				{ 23, new string[] { "hearth watcher", "monsterous bear", "giant bear", "massive bear", "huge bear", "bear" } },

			};

			foreach (var synonym in synonyms)
			{
				CreateMonsterSynonyms(synonym.Key, synonym.Value);
			}

			// Place unseen apparition

			var unseenApparitionMonster = MDB[2];

			Debug.Assert(unseenApparitionMonster != null);

			var room = GetRandomWayfarersInnRoom(new long[] { 42 });

			unseenApparitionMonster.Location = room.Uid;

			// Note if player armor is metal

			var armorArtifact = GameState.Ar > 0 ? ADB[GameState.Ar] : null;

			var wearableAc = armorArtifact != null ? armorArtifact.Wearable : null;

			gGameState.MetalArmorArtifactUid = armorArtifact != null && wearableAc != null && ((wearableAc.Field1 > (long)Armor.LeatherShield && wearableAc.Field1 < (long)Armor.MagicExotic8) || wearableAc.Field1 > (long)Armor.MagicExoticShield9) ? armorArtifact.Uid : 0;

			// Store monster Hardiness values for later reward calculations

			var monsterList = Database.MonsterTable.Records.Where(m => m.Uid != 2 && m.Friendliness < Friendliness.Friend).ToList();

			foreach (var monster in monsterList)
			{
				monster.Field2 = monster.Uid == 3 ? monster.Hardiness * gGameState.TotalCentipedeCounter : monster.Hardiness * monster.GroupCount;
			}
		}

		public override void InitMonsterScaledHardinessValues()
		{
			Eamon.Framework.Primitive.Classes.IArtifactCategory ac;

			var maxDamage = ScaledHardinessUnarmedMaxDamage;

			Debug.Assert(gCharMonster != null);

			if (gCharMonster.Weapon > 0)       // will always be most powerful weapon
			{
				var artifact = ADB[gCharMonster.Weapon];

				Debug.Assert(artifact != null);

				ac = artifact.GeneralWeapon;

				Debug.Assert(ac != null);

				maxDamage = ac.Field3 * ac.Field4;
			}

			// adjust maxDamage for Nolan's machete (if necessary)

			var macheteArtifact = ADB[152];

			Debug.Assert(macheteArtifact != null);

			ac = macheteArtifact.GeneralWeapon;

			Debug.Assert(ac != null);

			if (ac.Field3 * ac.Field4 > maxDamage)
			{
				maxDamage = ac.Field3 * ac.Field4;
			}

			var damageFactor = (long)Math.Round((double)maxDamage / ScaledHardinessMaxDamageDivisor);

			if (damageFactor < 1)
			{
				damageFactor = 1;
			}

			var monsterList = Database.MonsterTable.Records.ToList();

			foreach (var m in monsterList)
			{
				SetScaledHardiness(m, damageFactor);
			}
		}

		public override void MonsterDies(IMonster actorMonster, IMonster dobjMonster)
		{
			Debug.Assert(dobjMonster != null);

			// Blue-banded centipedes

			if (dobjMonster.Uid == 3)
			{
				if (dobjMonster.CurrGroupCount > 1)
				{
					dobjMonster.GroupCount--;

					dobjMonster.InitGroupCount--;
				}
				else
				{
					dobjMonster.GroupCount = 0;

					dobjMonster.InitGroupCount = 0;
				}

				if (gGameState.AttackingCentipedeCounter > 0)
				{
					gGameState.AttackingCentipedeCounter--;
				}
			}

			base.MonsterDies(actorMonster, dobjMonster);

			// Update total damage taken stats

			if (dobjMonster.Field2 > 0)
			{
				gGameState.SetMonsterTotalDmgTaken(dobjMonster.Uid, gGameState.GetMonsterTotalDmgTaken(dobjMonster.Uid) + dobjMonster.Hardiness);

				if (gGameState.GetMonsterTotalDmgTaken(dobjMonster.Uid) > dobjMonster.Field2)
				{
					gGameState.SetMonsterTotalDmgTaken(dobjMonster.Uid, dobjMonster.Field2);
				}
			}

			// Blue-banded centipedes

			if (dobjMonster.Uid == 3 && dobjMonster.IsInLimbo())
			{
				if (gGameState.TotalCentipedeCounter <= 0)
				{
					MiscEventFuncList02.Add(() =>
					{
						PrintEffectDesc(110);
					});
				}

				gGameState.SetEventState(EventState.BlueBandedCentipedes, 0);
			}
		}

		public override void GetRandomMoveDirection(IRoom room, IMonster monster, bool fleeing, ref Direction direction, ref bool found, ref long roomUid)
		{
			Debug.Assert(room != null);

			// Exclude movement into Tiaga Gorge

			do
			{
				base.GetRandomMoveDirection(room, monster, fleeing, ref direction, ref found, ref roomUid);
			}
			while ((room.Uid == 4 || room.Uid == 5 || room.Uid == 6 || room.Uid == 7) && roomUid == -60);
		}

		public override IList<IArtifact> GetReadyableWeaponList(IMonster monster)
		{
			var result = base.GetReadyableWeaponList(monster);

			// Nolan won't take an unforged weapon off the anvil

			if (monster.Uid == 24)
			{
				result = result.Where(a => !a.IsCarriedByContainerUid(161) || gGameState.ForgedArtifactUids.Contains(a.Uid)).ToList();
			}

			return result;
		}

		public override IList<IMonster> GetHostileMonsterList(IMonster monster)
		{
			Debug.Assert(monster != null);

			var monsterList = base.GetHostileMonsterList(monster);

			// Nolan

			if (monster.Uid == 24)
			{
				var weaponArtifact = monster.Weapon > 0 ? ADB[monster.Weapon] : null;

				var weaponType = weaponArtifact != null ? (Weapon)weaponArtifact.Field2 : 0;

				var rl = RollDice(1, 100, 0);

				// Refuses to attack black pudding

				if (weaponArtifact == null || (weaponType == Weapon.Sword && rl >= gGameState.NolanPuddingAttackOdds))
				{
					monsterList = monsterList.Where(m => m.Uid != 1).ToList();
				}

				rl = RollDice(1, 100, 0);

				// Refuses to attack rust monster

				if ((weaponType == Weapon.Axe || weaponType == Weapon.Spear || weaponType == Weapon.Sword) && rl >= gGameState.NolanRustMonsterAttackOdds)
				{
					monsterList = monsterList.Where(m => m.Uid != 20).ToList();
				}
			}

			// Charlotte is never attacked

			monsterList = monsterList.Where(m => m.Uid != 4).ToList();

			return monsterList;
		}

		public override IList<IMonster> GetEmotingMonsterList(IRoom room, IMonster monster, bool friendSmile = true)
		{
			// Some monsters don't emote

			return base.GetEmotingMonsterList(room, monster, friendSmile).Where(m => !NonEmotingMonsterUids.Contains(m.Uid)).ToList();
		}

		public override void CheckToExtinguishLightSource()
		{
			// do nothing
		}

		public virtual bool IsWindowRoomUid(long roomUid)
		{
			Debug.Assert(roomUid > 0);

			return NorthWindowRoomUids.Contains(roomUid) || SouthWindowRoomUids.Contains(roomUid) || WestWindowRoomUids.Contains(roomUid);
		}

		public virtual bool IsKitchenShelfBalanced()
		{
			var artifactList = GetArtifactList(a => a.IsCarriedByContainerUid(173) && a.GetCarriedByContainerContainerType() == ContainerType.On);

			return GameState != null && (gGameState.KitchenRiddleState == 3 || gGameState.KitchenRiddleState == 4) && artifactList.Count == 2;
		}

		public virtual IList<IMonster> GetCompanionList()
		{
			return GetMonsterList(m => !m.IsCharacterMonster() && m.Reaction == Friendliness.Friend && m.IsInRoomUid(GameState.Ro));
		}

		public virtual IRoom GetRandomWayfarersInnRoom(long[] omittedRoomUids)
		{
			Framework.IRoom room = null;

			do
			{
				var rl = RollDice(1, Module.NumRooms, 0);

				room = RDB[rl] as Framework.IRoom;
			}
			while (!room.IsWayfarersInnRoom() || (omittedRoomUids != null && omittedRoomUids.Contains(room.Uid)) || (gCharRoom.IsWayfarersInnClearingRoom() && !gGameState.OutdoorsHauntingSeen && !IsWindowRoomUid(room.Uid)));

			return room;
		}

		public virtual void GetOutdoorsHauntingData(long charRoomUid, long unseenApparitionRoomUid, ref string stateDesc)
		{
			switch (charRoomUid)
			{
				case 9:

					switch (unseenApparitionRoomUid)
					{
						case 39:

							stateDesc = " in a western third-floor window";

							break;

						case 45:

							stateDesc = " in a southern first-floor window";

							break;

						case 48:

							stateDesc = " in a southwestern first-floor window";

							break;

						case 60:

							stateDesc = " in a southern second-floor window";

							break;

						case 63:

							stateDesc = " in a southwestern second-floor window";

							break;
					}

					break;

				case 10:

					switch (unseenApparitionRoomUid)
					{
						case 54:

							stateDesc = " in a southeastern second-floor window";

							break;

						case 57:

							stateDesc = " in a southern second-floor window";

							break;
					}

					break;

				case 11:

					switch (unseenApparitionRoomUid)
					{
						case 38:

							stateDesc = " in a northern third-floor window";

							break;

						case 53:

							stateDesc = " in a northeastern second-floor window";

							break;

						case 56:

							stateDesc = " in a northern second-floor window";

							break;
					}

					break;

				case 12:

					switch (unseenApparitionRoomUid)
					{
						case 44:

							stateDesc = " in a northern first-floor window";

							break;

						case 46:

							stateDesc = " in a western first-floor window";

							break;

						case 47:

							stateDesc = " in a northwestern first-floor window";

							break;

						case 59:

							stateDesc = " in a northern second-floor window";

							break;

						case 61:

							stateDesc = " in a western second-floor window";

							break;

						case 62:

							stateDesc = " in a northwestern second-floor window";

							break;
					}

					break;
			}
		}

		public virtual void GetIndoorsHauntingData(long charRoomUid, long unseenApparitionRoomUid)
		{

		}

		public virtual void BuildDecorationArtifact(long artifactUid, long effectUid, string name, string[] synonyms, string stateDesc, ArticleType articleType = ArticleType.A, PluralType pluralType = PluralType.S, bool isPlural = false)
		{
			Debug.Assert(artifactUid > 0 && effectUid > 0 && !string.IsNullOrWhiteSpace(name) && synonyms != null && synonyms.Length > 0 && Enum.IsDefined(typeof(ArticleType), articleType) && Enum.IsDefined(typeof(PluralType), pluralType));

			var effect = EDB[effectUid];

			Debug.Assert(effect != null);

			var decorationArtifact = ADB[artifactUid];

			Debug.Assert(decorationArtifact != null);

			decorationArtifact.Name = name;

			decorationArtifact.Desc = CloneInstance(effect.Desc);

			decorationArtifact.Synonyms = synonyms;

			decorationArtifact.StateDesc = stateDesc;

			decorationArtifact.ArticleType = articleType;

			decorationArtifact.PluralType = pluralType;

			decorationArtifact.IsPlural = isPlural;

			decorationArtifact.SetInRoom(gCharRoom);

			Out.Print(effect.Desc);
		}

		public Engine()
		{
			ArtDescLen = 510;

			EffDescLen = 510;

			HntAnswerLen = 510;

			MonDescLen = 510;

			RmDescLen = 510;

			MacroFuncs.Add(2, () =>
			{
				var result = "reach for your weapon";

				if (UnseenApparitionAttacks == 1)
				{
					result = "cast the Blast spell";
				}
				else if (UnseenApparitionAttacks == 2)
				{
					result = "stand there helplessly";
				}

				return result;
			});

			MacroFuncs.Add(3, () =>
			{
				var result = "";

				var monsterList = GameState != null ? GetCompanionList() : new List<IMonster>();

				if (monsterList.Count > 0)
				{
					result = " and your companions";
				}

				return result;
			});

			MacroFuncs.Add(4, () =>
			{
				var result = "You";

				var monsterList = GameState != null ? GetCompanionList() : new List<IMonster>();

				if (monsterList.Count > 0)
				{
					result = string.Format("You can hear your companions' screams echoing through the {0} as it tears them limb from limb. You", gCharRoom.EvalRoomType("room", "area"));
				}

				return result;
			});

			MacroFuncs.Add(5, () =>
			{
				var result = "limb from limb";

				var monsterList = GameState != null ? GetCompanionList() : new List<IMonster>();

				if (monsterList.Count > 0)
				{
					result = "apart piece by piece";
				}

				return result;
			});

			MacroFuncs.Add(11, () =>
			{
				var result = "";

				var numPaths = 1;

				var room = RDB[10];

				Debug.Assert(room != null);

				var room02 = RDB[11];

				Debug.Assert(room02 != null);

				if (GameState != null)
				{
					if (gGameState.WoodenBridgeUseCounter > 2)
					{
						result += " There is also a path leading south in the southeast corner of the clearing.";

						numPaths++;
					}

					if (room.Seen && room02.Seen)
					{
						result += string.Format(" You must have missed {0} when you first explored the clearing.", numPaths > 1 ? "them" : "it");
					}

					result += string.Format(" You make a mental note to investigate {0} later.", numPaths > 1 ? "these" : "this");
				}

				return result;
			});

			MacroFuncs.Add(13, () =>
			{
				return GameState != null && NorthWindowRoomUids.Contains(GameState.Ro) ? "north" : 
							GameState != null && SouthWindowRoomUids.Contains(GameState.Ro) ? "south" :
							"west";
			});

			MacroFuncs.Add(14, () =>
			{
				return GameState != null && GameState.Ro == 13 && GameState.R3 == 9 ? "You" : 
							"You work your way around the massive building, admiring its fine but crumbling stonework architecture. Eventually, you";
			});

			MacroFuncs.Add(23, () =>
			{
				return Character != null ? Character.EvalGender(" of man", " of woman", "") : "";
			});

			MacroFuncs.Add(28, () =>
			{
				return SyndicateReward.ToString();
			});

			MacroFuncs.Add(30, () =>
			{
				return gCharRoom != null ? gCharRoom.EvalRoomType("floor", "ground") : "floor";
			});

			MacroFuncs.Add(58, () => gCharRoom != null ? gCharRoom.EvalRoomType("room", "area") : "room");

			UseMonsterScaledHardinessValues = true;

			ExposeContainersRecursively = true;

			PoundCharPolicy = PoundCharPolicy.None;

			AccountingLedgerData = @"Ky0tLS0tLS0tLS0rLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0rLS0tLS0tLS0tKy0tLS0tLS0tLS0rCnwgTW9udGggICAgfCBEZXNjcmlwdGlvbiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgfCBJbmNvbWUgIHwgRXhwZW5zZXMgfAorLS0tLS0tLS0tLSstLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLSstLS0tLS0tLS0rLS0tLS0tLS0tLSsKfCBEYXJrICAgICB8IFJvb20gcmVudGFsICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICB8IDEyMDAgICAgfCAtICAgICAgICB8CnwgRGFyayAgICAgfCBGb29kIHNhbGVzICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgfCA2MDAgICAgIHwgLSAgICAgICAgfAp8IERhcmsgICAgIHwgQmFyIHNhbGVzICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIHwgOTAwICAgICB8IC0gICAgICAgIHwKfCBEYXJrICAgICB8IFB1cmNoYXNlIG9mIHN1cHBsaWVzIGZyb20gbG9jYWwgZmFybWVyICAgICB8IC0gICAgICAgfCAyNTAgICAgICB8CnwgRGFyayAgICAgfCBXYWdlcyBwYWlkIHRvIHN0YWZmICAgICAgICAgICAgICAgICAgICAgICAgfCAtICAgICAgIHwgMjAwICAgICAgfAp8IERhcmsgICAgIHwgTW9ydGdhZ2UgcGF5bWVudCB0byBHcmVlciBCbGFja3Rob3JuICAgICAgIHwgLSAgICAgICB8IDIwMDAgICAgIHwKfCBEYXJrICAgICB8IFJlcGFpciBjb3N0cyBmb3Igcm9vZiBkYW1hZ2UgICAgICAgICAgICAgICB8IC0gICAgICAgfCA3NTAgICAgICB8CnwgRnJvc3QgICAgfCBSb29tIHJlbnRhbCAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgfCA2MDAgICAgIHwgLSAgICAgICAgfAp8IEZyb3N0ICAgIHwgRm9vZCBzYWxlcyAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIHwgMzYwICAgICB8IC0gICAgICAgIHwKfCBGcm9zdCAgICB8IEJhciBzYWxlcyAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICB8IDQ4MCAgICAgfCAtICAgICAgICB8CnwgRnJvc3QgICAgfCBQdXJjaGFzZSBvZiBhbGUgYW5kIHdpbmUgZnJvbSBzdXBwbGllciAgICAgfCAtICAgICAgIHwgMzUwICAgICAgfAp8IEZyb3N0ICAgIHwgUHVyY2hhc2Ugb2YgbGluZW5zIGFuZCB0b3dlbHMgICAgICAgICAgICAgIHwgLSAgICAgICB8IDE1MCAgICAgIHwKfCBGcm9zdCAgICB8IFdhZ2VzIHBhaWQgdG8gc3RhZmYgICAgICAgICAgICAgICAgICAgICAgICB8IC0gICAgICAgfCAyMDAgICAgICB8CnwgRnJvc3QgICAgfCBNb3J0Z2FnZSBwYXltZW50IHRvIEdyZWVyIEJsYWNrdGhvcm4gICAgICAgfCAtICAgICAgIHwgMjAwMCAgICAgfAp8IFJhaW4gICAgIHwgUm9vbSByZW50YWwgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIHwgMTA4MCAgICB8IC0gICAgICAgIHwKfCBSYWluICAgICB8IEZvb2Qgc2FsZXMgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICB8IDEyMDAgICAgfCAtICAgICAgICB8CnwgUmFpbiAgICAgfCBCYXIgc2FsZXMgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgfCA3MjAgICAgIHwgLSAgICAgICAgfAp8IFJhaW4gICAgIHwgUHVyY2hhc2Ugb2YgY2FuZGxlcyBhbmQgbGFudGVybnMgICAgICAgICAgIHwgLSAgICAgICB8IDE1MCAgICAgIHwKfCBSYWluICAgICB8IFdhZ2VzIHBhaWQgdG8gc3RhZmYgICAgICAgICAgICAgICAgICAgICAgICB8IC0gICAgICAgfCAyMDAgICAgICB8CnwgUmFpbiAgICAgfCBNb3J0Z2FnZSBwYXltZW50IHRvIEdyZWVyIEJsYWNrdGhvcm4gICAgICAgfCAtICAgICAgIHwgMjAwMCAgICAgfAp8IFJhaW4gICAgIHwgUmVwYWlyIGNvc3RzIGZvciBicm9rZW4gd2luZG93ICAgICAgICAgICAgIHwgLSAgICAgICB8IDUwICAgICAgIHwKKy0tLS0tLS0tLS0rLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0rLS0tLS0tLS0tKy0tLS0tLS0tLS0rCnwgICAgICAgICAgfCBUb3RhbDogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgfCA3MTQwIEdQIHwgODMwMCBHUCAgfAorLS0tLS0tLS0tLSstLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLSstLS0tLS0tLS0rLS0tLS0tLS0tLSsK";

			// Art by Sebastian Stoecker; found at the ASCII Art Archive: https://www.asciiart.eu/animals/deer

			WallMuralData = @"ICAgICAgLC8gIFwuCiAgICAgfCggICAgKXwKXGAtLl86LFwgIC8uO18sLScvCiBgLlxfYFwnKShgLydfLywnCiAgICAgKS9gLiwnXCgKICAgICB8LiAgICAsfAogICAgIDo2KSAgKDY7CiAgICAgIFxgXCBfKFwKICAgICAgIFwuXyc7IGAuX19fLi4uLS0tLi5fX19fX19fXy4uLi0tLS0tLS5fCiAgICAgICAgXCAgIHwgICAsJyAgIC4gIC4gICAgIC4gICAgICAgLiAgICAgLmA6LgogICAgICAgICBcYC4nIC4gIC4gICAgICAgICAuICAgLiAgIC4gICAgIC4gICAuIFxcCiAgICAgICAgICBgLiAgICAgICAuICAgLiAgXCAgLiAgIC4gICAuLjo6OiAuICAgIDo6CiAgICAgICAgICAgIFwgLiAgICAuICAuICAgLi46Ojo6Ojo6OicnICAnOiAgICAuIHx8CiAgICAgICAgICAgICBcICAgYC4gOi4gLjonICAgICAgICAgICAgXCAgJy4gLiAgIDs7CiAgICAgICAgICAgICAgYC5fICBcIDo6OiA7ICAgICAgICAgICBfLFwgIDouICB8LygKICAgICAgICAgICAgICAgICBgLmA6OjogLy0tLi4uLi0tLScnJyBcIGAuIDouIDpgXGAKICAgICAgICAgICAgICAgICAgfCB8Oic6ICAgICAgICAgICAgICAgXCAgYC4gOi5cCiAgICAgICAgICAgICAgICAgIHwgfCcgOyAgICAgICAgICAgICAgICBcICAoXCAgLlwKICAgICAgICAgICAgICAgICAgfCB8LjogICAgICAgICAgICAgICAgICBcICBcYC4gIDoKICAgICAgICAgICAgICAgICAgfC58IHwgICAgICAgICAgICAgICAgICAgKSAvICA6LnwKICAgICAgICAgICAgICAgICAgfCB8LnwgICAgICAgICAgICAgICAgICAvLi8gICB8IHwKICAgICAgICAgICAgICAgICAgfC58IHwgICAgICAgICAgICAgICAgIC8gLyAgICB8IHwKICAgICAgICAgICAgICAgICAgfCB8IHwgICAgICAgICAgICAgICAgLy4vICAgICB8LnwKICAgICAgICAgICAgICAgICAgO187XzsgICAgICAgICAgICAgICwnXy8gICAgICA7X3wKICAgICAgICAgICAgICAgICAnLS9fKCAgICAgICAgICAgICAgJy0tJyAgICAgIC8sJyBTU3Q=";

			KitchenMessageData = @"IkJhbGFuY2UgdGhlIHBhc3QgdG8gcmV2ZWFsIHRoZSB3YXksCkluIHRoaXMgYXJyYW5nZW1lbnQsIHRoZSBhbnN3ZXIgd2lsbCBzd2F5LgpQbGF0ZXMgYW5kIHV0ZW5zaWxzLCBlYWNoIGhvbGRzIGEgd2VpZ2h0LApTZWVrIHRoZSBiYWxhbmNlLCBkbyBub3QgaGVzaXRhdGUuIgo=";

			WanderRoomUids = new long[][]
			{
				null,

				// Black pudding

				new long[] { 32, 33 },

				// Unseen apparition

				null,

				// Blue-banded centipedes

				new long[] { 27 },

				// Charlotte

				null,

				// Rat swarm

				new long[] { 19 },

				// Bat swarm

				new long[] { 67 },

				// Dire wolves

				null,

				// Grave robbers

				null,

				// Giant yellow jackets

				new long[] { 37, 38, 39 },

				// Giant bombardier beetles

				new long[] { 52, 53, 54 },

				// Giant fire beetles

				new long[] { 52, 53, 54 },

				// Giant brown recluses

				new long[] { 34 },

				// Giant black widows

				new long[] { 34 },

				// Violet fungus

				new long[] { 29 },

				// Gnoll shaman

				new long[] { 42 },

				// Gnoll warriors

				new long[] { 42 },
				
				// Gnoll tracker

				new long[] { 42 },

				// Witherbloom

				new long[] { 47 },

				// Assassin Vines

				new long[] { 47 },

				// Rust monster

				new long[] { 43, 44, 45 },

				// Peryton

				new long[] { 55, 56, 57 },

				// Harpy

				new long[] { 58, 59, 60 },

				// Hearthwatcher

				new long[] { 24, 26, 30, 31 },

				// Nolan

				null,

				// Player

				null,
			};

			NorthWindowRoomUids = new long[] { 38, 42, 44, 47, 53, 56, 59, 62 };

			SouthWindowRoomUids = new long[] { 45, 48, 54, 57, 60, 63 };

			WestWindowRoomUids = new long[] { 39, 46, 61 };

			InnkeepersQuartersRoomUids = new long[] { 37, 38, 39, 66 };

			GuestRoomUids = new long[] { 42, 44, 45, 47, 48, 53, 54, 56, 57, 59, 60, 62, 63 };

			NonEmotingMonsterUids = new long[] { 1, 2, 12, 13, 14, 18, 19 };

			ForestEventFuncList = new List<Action<IRoom>> 
			{ 
				r => PrintEffectDesc(71),
				r => BuildDecorationArtifact(150, 72, "red fox", new string[] { "wildlife", "animals", "animal", "red fox", "fox" }, null),
				r => BuildDecorationArtifact(150, 73, "family of deer", new string[] { "wildlife", "animals", "animal", "family of deer", "family", "deer", "deer family" }, null),
				r => PrintEffectDesc(74),
				r => PrintEffectDesc(75),
				r => PrintEffectDesc(76), 
				r => BuildDecorationArtifact(150, 77, "bald eagle", new string[] { "wildlife", "animals", "animal", "bald eagle", "eagle" }, null),
				r => PrintEffectDesc(78),
				r => PrintEffectDesc(79), 
				r => BuildDecorationArtifact(150, 80, "squirrel", new string[] { "wildlife", "animals", "animal", "squirrel" }, null)
			};

			ForestEventFuncList = ForestEventFuncList.OrderBy(n => Rand.Next()).ToList();

			RiverEventFuncList = new List<Action<IRoom>> 
			{
				r => PrintEffectDesc(81),
				r => PrintEffectDesc(82),
				r => BuildDecorationArtifact(150, 83, "deer", new string[] { "wildlife", "animals", "animal", "deer" }, null),
				r => BuildDecorationArtifact(150, 84, "golden eagle", new string[] { "wildlife", "animals", "animal", "eagles", "eagle" }, null, ArticleType.Some, PluralType.S, true),
				r => PrintEffectDesc(85),
				r => PrintEffectDesc(86),
				r => PrintEffectDesc(87),
				r => BuildDecorationArtifact(150, 88, "elk", new string[] { "wildlife", "animals", "animal" }, null, ArticleType.Some, PluralType.None, true),
				r => PrintEffectDesc(89),
				r => BuildDecorationArtifact(150, 90, "mountain lion", new string[] { "wildlife", "animals", "animal", "lion", "cat" }, null),
			};

			RiverEventFuncList = RiverEventFuncList.OrderBy(n => Rand.Next()).ToList();

			ChildsApparitionEventFuncList = new List<Action<IRoom, IMonster>>
			{
				(r, m) => Out.Print("{0} plays with her spectral teddy, {1}.", m.GetTheName(true), GetRandomElement(new string[] { "fiddling with its stubby ears", "pulling gently at its button eyes", "stroking its fur", "poking at its belly", "holding it by the arms" })),
				(r, m) => Out.Print("{0} shifts her stance from her {1} foot.", m.GetTheName(true), GetRandomElement(new string[] { "right to left", "left to right" })),
				(r, m) => Out.Print("{0} hums to herself, what sounds like {1}.", m.GetTheName(true), GetRandomElement(new string[] { "a lullaby", "a local folk song", "a made-up song" })),
				(r, m) => Out.Print("{0} {1}.", m.GetTheName(true), GetRandomElement(new string[] { "stands on one foot, trying to balance herself", "plays a game of hopscotch", "waves her arms in the air like a bird flapping its wings", "makes finger puppets with her little hands" })),
				(r, m) => Out.Print("{0} {1}, though probably just out of habit.", m.GetTheName(true), GetRandomElement(new string[] { "yawns", "hiccups", "sneezes", "shivers", "scratches an itch" })),
				(r, m) =>
				{
					var excludedArtifactUids = new long[] { 30, 54, 55 };

					var recordArray = r.GetContainedList().Where(r01 => r01.Seen && ((r01 is IMonster m01 && m01.Uid != m.Uid) || (r01 is IArtifact a01 && !excludedArtifactUids.Contains(a01.Uid)))).ToArray();

					if (recordArray != null && recordArray.Length > 0)
					{
						var record = GetRandomElement(recordArray);

						Debug.Assert(record != null);

						var m02 = record as IMonster;

						var actionType = GetRandomElement(new string[] { "glances at", "looks at", "carefully examines" });

						Out.Print("{0} {1} {2}.", 
							m.GetTheName(true), 
							actionType, 
							m02 != null && m02.IsCharacterMonster() ? "you" : record.GetTheName());
					}
				},
				(r, m) =>
				{
					var excludedArtifactUids = new long[] { 30, 54, 55 };

					var artifactArray = gCharMonster.GetContainedList().Where(a => !excludedArtifactUids.Contains(a.Uid)).ToArray();

					if (artifactArray != null && artifactArray.Length > 0)
					{
						var artifact = GetRandomElement(artifactArray);

						Debug.Assert(artifact != null);

						var actionType = GetRandomElement(new string[] { "glances at", "looks at", "carefully examines" });

						Out.Print("{0} {1} your {2}.", 
							m.GetTheName(true), 
							actionType,
							artifact.GetNoneName());
					}
				},
				(r, m) =>
				{
					var excludedArtifactUids = new long[] { 30, 54, 55 };

					var recordArray = r.GetContainedList().Where(r01 => r01.Seen && r01 is IMonster m01 && m01.Uid != m.Uid && !m01.IsCharacterMonster() && m01.Reaction == Friendliness.Friend).ToArray();

					if (recordArray != null && recordArray.Length > 0)
					{
						var record = GetRandomElement(recordArray);

						Debug.Assert(record != null);

						var monster = record as IMonster;

						Debug.Assert(monster != null);

						var artifactArray = monster.GetContainedList().Where(a => !excludedArtifactUids.Contains(a.Uid)).ToArray();

						if (artifactArray != null && artifactArray.Length > 0)
						{
							var artifact = GetRandomElement(artifactArray);

							Debug.Assert(artifact != null);

							var actionType = GetRandomElement(new string[] { "glances at", "looks at", "carefully examines" });

							Out.Print("{0} {1} {2} {3}.", 
								m.GetTheName(true), 
								actionType,
								monster.GetTheName().AddPossessiveSuffix(),
								artifact.GetNoneName());
						}
					}
				},
				(r, m) =>
				{
					var excludedArtifactUids = new long[] { 30, 54, 55 };

					var recordArray = r.GetContainedList().Where(r01 => r01.Seen && ((r01 is IMonster m01 && m01.Uid != m.Uid) || (r01 is IArtifact a01 && !excludedArtifactUids.Contains(a01.Uid)))).ToArray();

					if (recordArray != null && recordArray.Length > 0)
					{
						var record = GetRandomElement(recordArray);

						Debug.Assert(record != null);

						var m02 = record as IMonster;

						var a02 = record as IArtifact;

						Out.Print("{0} tries to touch {1}, but {2} hand passes through{3}.",
							m.GetTheName(true),
							m02 != null && m02.IsCharacterMonster() ? "you" : record.GetTheName(),
							m.EvalGender("his", "her", "its"),
							m02 != null && m02.IsCharacterMonster() ? " you" : m02 != null ? m02.EvalGender(" him", " her", " it") : a02 != null ? a02.EvalPlural(" it", " them") : "");
					}
				},
				(r, m) =>
				{
					var surfaceType = RollDice(1, r.EvalRoomType(3, 2), 0);

					Out.Print("{0} {1} through {2} but reappears shortly after.", 
						m.GetTheName(true), 
						surfaceType == 1 || surfaceType == 2 ? "vanishes" : "floats", 
						surfaceType == 1 ? "a wall" : surfaceType == 2 ? r.EvalRoomType("the floor", "the ground") : "the ceiling");
				},
			};

			ChildsApparitionEventFuncList = ChildsApparitionEventFuncList.OrderBy(n => Rand.Next()).ToList();
		}
	}
}
