
// Engine.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

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
				{ 2, new string[] { "bag" } },
				{ 6, new string[] { "miners pick", "pick" } },
				{ 15, new string[] { "gold", "ore" } },
				{ 16, new string[] { "big stick" } },
				{ 17, new string[] { "trap" } },
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
				{ 2, new string[] { "cougar", "lion", "puma", "wildcat", "bobcat" } },
			};

			foreach (var synonym in synonyms)
			{
				CreateMonsterSynonyms(synonym.Key, synonym.Value);
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
