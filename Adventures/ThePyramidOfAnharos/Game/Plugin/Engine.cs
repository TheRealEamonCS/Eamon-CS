
// Engine.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using Eamon;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using EamonRT.Framework.Components;
using EamonRT.Framework.States;
using static ThePyramidOfAnharos.Game.Plugin.Globals;

namespace ThePyramidOfAnharos.Game.Plugin
{
	public class Engine : EamonRT.Game.Plugin.Engine, Framework.Plugin.IEngine
	{
		public virtual string MapData { get; set; }

		public virtual bool TaxLevied { get; set; }

		public virtual bool GuardsAttack { get; set; }

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
				monster.EvalReaction("growl", "gaze", "smile"),
				monster.EvalPlural("s", ""));
		}

		public override void InitArtifacts()
		{
			base.InitArtifacts();

			MacroFuncs.Add(1, () =>
			{
				// Western pyramid door

				return GameState != null && GameState.Ro == 14 ? "an exit west into the desert" : "a passage east into the core of the pyramid";
			});

			MacroFuncs.Add(2, () =>
			{
				// Southern pyramid secret door

				return GameState != null && GameState.Ro == 16 ? "an exit into the desert to the south" : "a room inside the pyramid to the north";
			});

			MacroFuncs.Add(4, () =>
			{
				var result = "";

				// Pyramid / Floor

				if (GameState != null)
				{
					var effectUid = GameState.Ro == 12 ? 41 : GameState.Ro > 13 && GameState.Ro < 25 ? 39 : GameState.Ro > 24 && GameState.Ro < 43 ? 40 : 0;

					var effect = effectUid > 0 ? EDB[effectUid] : null;

					if (effect != null)
					{
						result = effect.Desc;
					}
				}

				if (result == "")
				{
					result = "You see nothing special.";
				}

				return result;
			});

			var synonyms = new Dictionary<long, string[]>()
			{
				{ 3, new string[] { "sword" } },
				{ 4, new string[] { "prod" } },
				{ 6, new string[] { "rifle", "gun" } },
				{ 7, new string[] { "rifle", "gun" } },
				{ 10, new string[] { "scimitar" } },
				{ 12, new string[] { "bag" } },
				{ 14, new string[] { "glyphs", "glyph" } },
				{ 16, new string[] { "torches" } },
				{ 17, new string[] { "torches" } },
				{ 21, new string[] { "wall", "flame" } },
				{ 22, new string[] { "wall", "flame" } },
				{ 23, new string[] { "cloud" } },
				{ 24, new string[] { "cloud" } },
				{ 25, new string[] { "body" } },
				{ 26, new string[] { "body" } },
				{ 27, new string[] { "body" } },
				{ 28, new string[] { "door" } },
				{ 29, new string[] { "body" } },
				{ 30, new string[] { "glyphs", "glyph" } },
				{ 31, new string[] { "moon pool", "pool" } },
				{ 32, new string[] { "glyphs", "glyph" } },
				{ 34, new string[] { "mummy", "Anharos" } },
				{ 35, new string[] { "door" } },
				{ 38, new string[] { "Diamond", "Purity" } },
				{ 39, new string[] { "case" } },
				{ 40, new string[] { "glyphs", "glyph" } },
				{ 42, new string[] { "leather", "armor" } },
				{ 43, new string[] { "chain", "armor" } },
				{ 44, new string[] { "plate", "armor" } },
				{ 47, new string[] { "caravan" } },
				{ 48, new string[] { "water column", "column", "water" } },
				{ 49, new string[] { "merchant", "Farouk" } },
				{ 56, new string[] { "dead Omar", "dead body", "body", "Omar" } },
				{ 57, new string[] { "dead Ali", "dead body", "body", "Ali" } },
				{ 58, new string[] { "scorpion" } },
				{ 59, new string[] { "dead devil", "dust devil", "devil" } },
				{ 60, new string[] { "dead hermit", "crazed hermit", "hermit" } },
				{ 61, new string[] { "dead Farouk", "dead body", "body", "Farouk" } },
				{ 62, new string[] { "dead Saala el Kahir", "dead body", "body", "Saala", "el Kahir", "Kahir" } },
				{ 64, new string[] { "dead Hamid", "dead body", "body", "Hamid" } },
				{ 65, new string[] { "dead Abou", "dead body", "body", "Abou" } },
				{ 66, new string[] { "dead Fouad", "dead body", "body", "Fouad" } },
				{ 67, new string[] { "dead Mahomet", "dead body", "body", "Mahomet" } },
				{ 68, new string[] { "dead Rahman", "dead body", "body", "Rahman" } },
				{ 69, new string[] { "dead Yussuf", "dead body", "body", "Yussuf" } },
				{ 70, new string[] { "dead Masoud", "dead body", "body", "Masoud" } },
				{ 71, new string[] { "dead Farah", "dead body", "body", "Farah" } },
				{ 72, new string[] { "dead Sheba", "dead body", "body", "Sheba" } },
				{ 73, new string[] { "dead Basra", "dead body", "body", "Basra" } },
				{ 74, new string[] { "dead children", "Riff children", "children", "dead Riff child", "dead child", "Riff child", "child" } },
				{ 75, new string[] { "guards", "dead guard", "guard" } },
				{ 76, new string[] { "glyphs", "glyph" } },
			};

			foreach (var synonym in synonyms)
			{
				CreateArtifactSynonyms(synonym.Key, synonym.Value);
			}
		}

		public override void InitMonsters()
		{
			base.InitMonsters();

			MacroFuncs.Add(3, () =>
			{
				// Not enough water for Farouk

				return GameState != null && gGameState.KW < 20 ? ", but you have none to spare" : "";
			});

			var synonyms = new Dictionary<long, string[]>()
			{
				{ 4, new string[] { "devil" } },
				{ 5, new string[] { "hermit" } },
				{ 7, new string[] { "Saala", "el Kahir", "Kahir" } },
				{ 8, new string[] { "avatar", "Alaxar" } },
				{ 19, new string[] { "children", "child" } },
			};

			foreach (var synonym in synonyms)
			{
				CreateMonsterSynonyms(synonym.Key, synonym.Value);
			}

			// Desert guide

			if (gGameState.GU > 0)
			{
				var guideMonster = MDB[gGameState.GU];

				Debug.Assert(guideMonster != null);

				guideMonster.SetInRoomUid(1);

				guideMonster.Reaction = Friendliness.Friend;

				gCharacter.HeldGold -= (long)Math.Floor((200.0 / gGameState.GU) / gGameState.GU);
			}
		}

		public override void SellInventoryToMerchant(bool sellInventory = true)
		{
			base.SellInventoryToMerchant(sellInventory);

			if (Character.HeldGold > 0 && TaxLevied)
			{
				Out.Print("{0}", LineSep);

				Out.Print("Unfortunately, it's all taxed away.");

				Character.HeldGold = 0;

				In.KeyPress(Buf);
			}
		}

		public override void CheckToExtinguishLightSource()
		{
			// do nothing
		}

		public override void MoveMonsters(params Func<IMonster, bool>[] whereClauseFuncs)
		{
			// Move avatar of Alaxar / guards even when unseen

			base.MoveMonsters
			(
				whereClauseFuncs != null && whereClauseFuncs.Length > 0 ? 
				whereClauseFuncs : 
				new Func<IMonster, bool>[] { m => !m.IsCharacterMonster() && (m.Uid == 8 || m.Uid == 20 || m.Seen) && m.Location == GameState.R3 }
			);
		}

		public virtual void PrintGuideMonsterDirection()
		{
			Debug.Assert(gCharMonster != null);

			var room = gCharMonster.GetInRoom();

			Debug.Assert(room != null);

			if (room.Uid > 0 && room.Uid < 6 && gGameState.GU > 0)
			{
				var guideMonster = MDB[gGameState.GU];

				Debug.Assert(guideMonster != null);

				if (guideMonster.IsInRoom(room) && guideMonster.Reaction == Friendliness.Friend)
				{
					var direction = "";

					// Omar's directions

					if (guideMonster.Uid == 1)
					{
						var directions = gGameState.KV != 0 ?
							new string[] { "", "west", "west", "northwest", "north", "west" } :
							new string[] { "", "east", "southeast", "south", "east", "east" };

						direction = directions[room.Uid];
					}

					// Ali's directions

					else
					{
						direction = gGameState.KV == 1 ?
							"north and/or west" :
							"south and/or east";
					}

					Out.Print("{0} suggests going {1}.", guideMonster.GetTheName(true), direction);
				}
			}
		}

		public virtual void PrintTheGlyphsRead(long effectUid)
		{
			Out.Print("The glyphs read:");

			PrintEffectDesc(effectUid);
		}

		public virtual void InjurePartyAndDamageEquipment(IRoom room, long effectUid, long deadBodyRoomUid, long equipmentDamageAmount, double injuryMultiplier, Action<IState> setNextStateFunc, ref bool gotoCleanup)
		{
			Debug.Assert(room != null && effectUid > 0 && equipmentDamageAmount > 0 && injuryMultiplier > 0.0 && setNextStateFunc != null);

			PrintEffectDesc(effectUid);

			var monsterList = GetMonsterList(m => m.IsCharacterMonster(), m => !m.IsCharacterMonster() && m.Reaction == Friendliness.Friend && m.IsInRoom(room));

			foreach (var monster in monsterList)
			{
				var dice = (long)Math.Floor(injuryMultiplier * (monster.Hardiness - monster.DmgTaken) + 1);

				var combatComponent = CreateInstance<ICombatComponent>(x =>
				{
					x.SetNextStateFunc = setNextStateFunc;

					x.ActorRoom = room;

					x.Dobj = monster;

					x.OmitArmor = true;
				});

				combatComponent.ExecuteCalculateDamage(dice, 1);

				var deadBodyArtifact = monster.DeadBody > 0 ? ADB[monster.DeadBody] : null;

				if (deadBodyArtifact != null && !deadBodyArtifact.IsInLimbo())
				{
					deadBodyArtifact.SetInRoomUid(deadBodyRoomUid);
				}

				if (gGameState.Die > 0)
				{
					gotoCleanup = true;

					goto Cleanup;
				}
			}

			foreach (var monster in monsterList)
			{
				DamageWeaponsAndArmor(room, monster, equipmentDamageAmount);
			}

		Cleanup:

			;
		}

		public Engine()
		{
			PushRulesetVersion(62);

			EnableNegativeRoomUidLinks = true;

			PoundCharPolicy = PoundCharPolicy.None;

			MapData = @"ICAgICAvXCAgICAgICAgICAgICAgICAgICAgICAgIF8NCiAgICAvICBcICAgICAgICAgICAgICAgICAgICAgICEgIQ0KICAgLyAgICBcICAgLS0+ICAgLS0+ICAgLS0+ICAgISAhDQogIC8gICAgICBcICAgICAgICAgICAgICAgICAgICAhICENCiAvX19fX19fX19cICAgICAgICAgICAgICAgICAgIFtfXQ0KDQogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIQ0KICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICENCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICBWDQoNCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAhDQogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIQ0KICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIFYNCg0KICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIC9cDQogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAvICBcDQogICAgICAgICAgICAgICAgICAgICAgICAgICAgICBcICAvDQogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgXC8=";
		}
	}
}
