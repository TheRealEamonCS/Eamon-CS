
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

		public override void InitMonsters()
		{
			base.InitMonsters();

			var synonyms = new Dictionary<long, string[]>()
			{
				// TODO
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

			if (room.Uid > 0 && room.Uid < 6)
			{
				var guideMonster = MDB[1];

				Debug.Assert(guideMonster != null);

				if (!guideMonster.IsInRoom(room))
				{
					guideMonster = MDB[2];

					Debug.Assert(guideMonster != null);
				}

				if (guideMonster.IsInRoom(room))
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
