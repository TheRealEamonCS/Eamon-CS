﻿
// Engine.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using Eamon;
using Eamon.Framework.Primitive.Enums;
using static ThePyramidOfAnharos.Game.Plugin.Globals;

namespace ThePyramidOfAnharos.Game.Plugin
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

		public override void InitArtifacts()
		{
			base.InitArtifacts();

			var synonyms = new Dictionary<long, string[]>()
			{
				{ 3, new string[] { "sword" } },
				{ 4, new string[] { "prod" } },
				{ 6, new string[] { "rifle", "gun" } },
				{ 7, new string[] { "rifle", "gun" } },
				{ 10, new string[] { "scimitar" } },
				{ 12, new string[] { "bag" } },
				{ 21, new string[] { "wall", "flame" } },
				{ 22, new string[] { "wall", "flame" } },
				{ 23, new string[] { "cloud" } },
				{ 24, new string[] { "cloud" } },
				{ 25, new string[] { "body" } },
				{ 26, new string[] { "body" } },
				{ 27, new string[] { "body" } },
				{ 28, new string[] { "door" } },
				{ 29, new string[] { "body" } },
				{ 31, new string[] { "moon pool", "pool" } },
				{ 34, new string[] { "mummy", "Anharos" } },
				{ 35, new string[] { "door" } },
				{ 38, new string[] { "Diamond", "Purity" } },
				{ 39, new string[] { "case" } },
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
				var guideMonster = gMDB[gGameState.GU];

				Debug.Assert(guideMonster != null);

				guideMonster.SetInRoomUid(1);

				guideMonster.Reaction = Friendliness.Friend;

				gCharacter.HeldGold -= (long)Math.Floor((200.0 / gGameState.GU) / gGameState.GU);
			}
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

		public Engine()
		{
			EnableNegativeRoomUidLinks = true;
		}
	}
}
