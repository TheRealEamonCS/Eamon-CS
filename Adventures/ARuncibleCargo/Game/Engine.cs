
// Engine.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Eamon.Framework;
using Eamon.Framework.Primitive.Classes;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using static ARuncibleCargo.Game.Plugin.PluginContext;

namespace ARuncibleCargo.Game
{
	[ClassMappings(typeof(IEngine))]
	public class Engine : EamonRT.Game.Engine, EamonRT.Framework.IEngine
	{
		public override void PrintMonsterAlive(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			// Bill in oven, Lil in cell

			if (artifact.Uid != 82 && artifact.Uid != 135)
			{
				base.PrintMonsterAlive(artifact);
			}
		}

		public override void InitArtifacts()
		{
			base.InitArtifacts();

			// Note: since these macro functions are installed by InitArtifacts, we can assume this is
			// being executed by EamonRT so there is no need to overharden the functions.  But, it is
			// still recommended!

			MacroFuncs.Add(1, () =>
			{
				var cargoArtifact = gADB[129];

				Debug.Assert(cargoArtifact != null);

				var lilMonster = gMDB[37];

				Debug.Assert(lilMonster != null);

				if (cargoArtifact.IsCarriedByMonster(lilMonster))
				{
					return ", dropping the \"Runcible Cargo\" at your feet as she disappears.";
				}
				else
				{
					return ".";
				}
			});

			MacroFuncs.Add(2, () =>
			{
				if (gGameState != null)
				{
					var shopDoorArtifact = gADB[gGameState.Ro == 20 ? 136 : 17];

					Debug.Assert(shopDoorArtifact != null);

					var ac = shopDoorArtifact.DoorGate;

					Debug.Assert(ac != null);

					return ac.Field2 != 0 ? "  You've locked yourself out." : "";
				}
				else
				{
					return "";
				}
			});

			MacroFuncs.Add(3, () =>
			{
				if (gGameState != null)
				{
					var lilMonster = gMDB[37];

					Debug.Assert(lilMonster != null);

					return lilMonster.Reaction > Friendliness.Enemy ? "Larcenous Lil prepares to attack!" : "";
				}
				else
				{
					return "";
				}
			});

			var synonyms = new Dictionary<long, string[]>()
			{
				{ 25, new string[] { "lantern" } },
				{ 37, new string[] { "bureau", "drawers" } },
				{ 69, new string[] { "window" } },
				{ 73, new string[] { "sign" } },
				{ 74, new string[] { "window" } },
				{ 78, new string[] { "refrigerator", "fridge" } },
				{ 80, new string[] { "oven" } },
				{ 89, new string[] { "sign" } },
				{ 90, new string[] { "sign" } },
				{ 95, new string[] { "lantern" } },
				{ 118, new string[] { "desks" } },
				{ 129, new string[] { "cargo" } },
				{ 136, new string[] { "rear door", "shop door", "door" } },
			};

			foreach (var synonym in synonyms)
			{
				CreateArtifactSynonyms(synonym.Key, synonym.Value);
			}

			// Signs (Sam's/weathered/supported/station)

			var signArtifact = gADB[16];

			Debug.Assert(signArtifact != null);

			signArtifact.Seen = true;

			var weatheredSignArtifact = gADB[73];

			Debug.Assert(weatheredSignArtifact != null);

			weatheredSignArtifact.Seen = true;

			var supportedSignArtifact = gADB[89];

			Debug.Assert(supportedSignArtifact != null);

			supportedSignArtifact.Seen = true;

			var stationSignArtifact = gADB[90];

			Debug.Assert(stationSignArtifact != null);

			stationSignArtifact.Seen = true;

			// (Barney) Rubble, Maintenance grate, Sewer grate

			Globals.DoubleDoorList.Add(Globals.CreateInstance<IArtifactLinkage>(x => 
			{
				x.RoomUid = 12;
				x.ArtifactUid1 = 18;
				x.ArtifactUid2 = 139;
			}));

			Globals.DoubleDoorList.Add(Globals.CreateInstance<IArtifactLinkage>(x =>
			{
				x.RoomUid = 13;
				x.ArtifactUid1 = 139;
				x.ArtifactUid2 = 18;
			}));

			Globals.DoubleDoorList.Add(Globals.CreateInstance<IArtifactLinkage>(x =>
			{
				x.RoomUid = 13;
				x.ArtifactUid1 = 24;
				x.ArtifactUid2 = 140;
			}));

			Globals.DoubleDoorList.Add(Globals.CreateInstance<IArtifactLinkage>(x =>
			{
				x.RoomUid = 14;
				x.ArtifactUid1 = 140;
				x.ArtifactUid2 = 24;
			}));

			Globals.DoubleDoorList.Add(Globals.CreateInstance<IArtifactLinkage>(x =>
			{
				x.RoomUid = 17;
				x.ArtifactUid1 = 26;
				x.ArtifactUid2 = 138;
			}));

			Globals.DoubleDoorList.Add(Globals.CreateInstance<IArtifactLinkage>(x =>
			{
				x.RoomUid = 29;
				x.ArtifactUid1 = 138;
				x.ArtifactUid2 = 26;
			}));
		}

		public override void ConvertToCarriedInventory(IList<IArtifact> weaponList)
		{
			// Can't sell Sam Slicker's shop key

			var shopKeyArtifact = gADB[9];

			Debug.Assert(shopKeyArtifact != null);

			shopKeyArtifact.SetInLimbo();

			base.ConvertToCarriedInventory(weaponList);
		}

		public override void MonsterGetsAggravated(IMonster monster, bool printFinalNewLine = true)
		{
			Debug.Assert(monster != null);

			// Keep Pookas Neutral

			if (monster.Uid > 3)
			{
				base.MonsterGetsAggravated(monster, printFinalNewLine);

				// Sync friendliness of Commander & troops, Prince & Guards

				if (monster.Uid == 27 || monster.Uid == 28)
				{
					var monster01 = gMDB[monster.Uid == 27 ? 28 : 27];

					Debug.Assert(monster01 != null);

					base.MonsterGetsAggravated(monster01, printFinalNewLine);
				}
				else if (monster.Uid == 38 || monster.Uid == 39)
				{
					var monster01 = gMDB[monster.Uid == 38 ? 39 : 38];

					Debug.Assert(monster01 != null);

					base.MonsterGetsAggravated(monster01, printFinalNewLine);
				}
			}
		}

		public override void PrintTooManyWeapons()
		{
			gOut.Print("As you enter the Main Hall, Lord William Crankhandle approaches you and says, \"You have too many weapons to keep them all, four is the legal limit.\"");
		}

		public override void PrintDeliverGoods()
		{
			gOut.Print("As Sam Slicker, the local buyer of treasure is vacationing, you grant yourself the gold he would have given you.");
		}

		public override void PrintGoodsPayment(bool goodsExist, long payment)
		{
			gOut.Print("{0}You take {1} gold piece{2} total.", goodsExist ? Environment.NewLine : "", payment, payment != 1 ? "s" : "");
		}

		public Engine()
		{
			PoundCharPolicy = PoundCharPolicy.PlayerArtifactsOnly;
		}
	}
}
