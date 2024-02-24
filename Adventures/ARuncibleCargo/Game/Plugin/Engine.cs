
// Engine.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using Eamon;
using Eamon.Framework;
using Eamon.Framework.Primitive.Classes;
using Eamon.Framework.Primitive.Enums;
using static ARuncibleCargo.Game.Plugin.Globals;

namespace ARuncibleCargo.Game.Plugin
{
	public class Engine : EamonRT.Game.Plugin.Engine, Framework.Plugin.IEngine
	{
		public virtual string SnapshotFileName { get; protected set; } = "SNAPSHOT_001.DAT";

		public virtual IList<IArtifactLinkage> DoubleDoorList { get; set; }

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

		public override void InitSystem()
		{
			base.InitSystem();

			DoubleDoorList = new List<IArtifactLinkage>();
		}

		public override void PrintMonsterAlive(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			// Bill in oven, Lil in cell

			if (artifact.Uid != 82 && artifact.Uid != 135)
			{
				base.PrintMonsterAlive(artifact);
			}
		}

		public override void PrintTooManyWeapons()
		{
			Out.Print("As you enter the Main Hall, Lord William Crankhandle approaches you and says, \"You have too many weapons to keep them all, four is the legal limit.\"");
		}

		public override void PrintDeliverGoods()
		{
			Out.Print("As Sam Slicker, the local buyer of treasure is vacationing, you grant yourself the gold he would have given you.");
		}

		public override void PrintGoodsPayment(bool goodsExist, long payment)
		{
			Out.Print("{0}You take {1} gold piece{2} total.", goodsExist ? Environment.NewLine : "", payment, payment != 1 ? "s" : "");
		}

		public override void InitArtifacts()
		{
			base.InitArtifacts();

			// Note: since these macro functions are installed by InitArtifacts, we can assume this is
			// being executed by EamonRT so there is no need to overharden the functions.  But, it is
			// still recommended!

			MacroFuncs.Add(1, () =>
			{
				var cargoArtifact = ADB[129];

				Debug.Assert(cargoArtifact != null);

				var lilMonster = MDB[37];

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
				if (GameState != null)
				{
					var shopDoorArtifact = ADB[GameState.Ro == 20 ? 136 : 17];

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
				if (GameState != null)
				{
					var lilMonster = MDB[37];

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
				{ 111, new string[] { "north door", "door" } },
				{ 112, new string[] { "west door", "door" } },
				{ 113, new string[] { "east door", "door" } },
				{ 118, new string[] { "desks" } },
				{ 129, new string[] { "cargo" } },
				{ 136, new string[] { "rear door", "shop door", "door" } },
			};

			foreach (var synonym in synonyms)
			{
				CreateArtifactSynonyms(synonym.Key, synonym.Value);
			}

			// Signs (Sam's/weathered/supported/station)

			var signArtifact = ADB[16];

			Debug.Assert(signArtifact != null);

			signArtifact.Seen = true;

			var weatheredSignArtifact = ADB[73];

			Debug.Assert(weatheredSignArtifact != null);

			weatheredSignArtifact.Seen = true;

			var supportedSignArtifact = ADB[89];

			Debug.Assert(supportedSignArtifact != null);

			supportedSignArtifact.Seen = true;

			var stationSignArtifact = ADB[90];

			Debug.Assert(stationSignArtifact != null);

			stationSignArtifact.Seen = true;

			// (Barney) Rubble, Maintenance grate, Sewer grate

			DoubleDoorList.Add(CreateInstance<IArtifactLinkage>(x =>
			{
				x.RoomUid = 12;
				x.ArtifactUid1 = 18;
				x.ArtifactUid2 = 139;
			}));

			DoubleDoorList.Add(CreateInstance<IArtifactLinkage>(x =>
			{
				x.RoomUid = 13;
				x.ArtifactUid1 = 139;
				x.ArtifactUid2 = 18;
			}));

			DoubleDoorList.Add(CreateInstance<IArtifactLinkage>(x =>
			{
				x.RoomUid = 13;
				x.ArtifactUid1 = 24;
				x.ArtifactUid2 = 140;
			}));

			DoubleDoorList.Add(CreateInstance<IArtifactLinkage>(x =>
			{
				x.RoomUid = 14;
				x.ArtifactUid1 = 140;
				x.ArtifactUid2 = 24;
			}));

			DoubleDoorList.Add(CreateInstance<IArtifactLinkage>(x =>
			{
				x.RoomUid = 17;
				x.ArtifactUid1 = 26;
				x.ArtifactUid2 = 138;
			}));

			DoubleDoorList.Add(CreateInstance<IArtifactLinkage>(x =>
			{
				x.RoomUid = 29;
				x.ArtifactUid1 = 138;
				x.ArtifactUid2 = 26;
			}));
		}

		public override void ConvertToCarriedInventory(IList<IArtifact> weaponList)
		{
			// Can't sell Sam Slicker's shop key

			var shopKeyArtifact = ADB[9];

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
					var monster01 = MDB[monster.Uid == 27 ? 28 : 27];

					Debug.Assert(monster01 != null);

					base.MonsterGetsAggravated(monster01, printFinalNewLine);
				}
				else if (monster.Uid == 38 || monster.Uid == 39)
				{
					var monster01 = MDB[monster.Uid == 38 ? 39 : 38];

					Debug.Assert(monster01 != null);

					base.MonsterGetsAggravated(monster01, printFinalNewLine);
				}
			}
		}

		public Engine()
		{
			PoundCharPolicy = PoundCharPolicy.PlayerArtifactsOnly;
		}
	}
}
