
// Engine.cs

// Copyright (c) 2014+ by Kenneth Pedersen.  All rights reserved.

using System.Collections.Generic;
using System.Diagnostics;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using static LandOfTheMountainKing.Game.Plugin.PluginContext;

namespace LandOfTheMountainKing.Game
{
	[ClassMappings(typeof(IEngine))]
	public class Engine : EamonRT.Game.Engine, EamonRT.Framework.IEngine
	{
		public override void InitArtifacts()
		{
			base.InitArtifacts();

			var synonyms = new Dictionary<long, string[]>()
			{
				{ 1, new string[] { "axe" } },
				{ 2, new string[] { "tree" } },
				{ 3, new string[] { "opening" } },
				{ 6, new string[] { "lamp" } },
				{ 8, new string[] { "cliffs" } },
				{ 9, new string[] { "ocean" } },
				{ 10, new string[] { "beach" } },
				{ 12, new string[] { "ravine" } },
				{ 13, new string[] { "tree" } },
				{ 14, new string[] { "platform" } },
				{ 15, new string[] { "stairs" } },
				{ 18, new string[] { "sun beams" } },
				{ 19, new string[] { "plain" } },
				{ 20, new string[] { "lightning wand" } },
				{ 21, new string[] { "Lisa's body" } },
				{ 22, new string[] { "club" } },
				{ 24, new string[] { "dead swamp monster" } },
				{ 25, new string[] { "sword" } },
				{ 26, new string[] { "man's body", "dead man" } },
				{ 27, new string[] { "necklace", "locket" } },
				{ 33, new string[] { "dead unarmed warrior", "dead warrior" } },
				{ 34, new string[] { "armor" } },
				{ 36, new string[] { "doors" } },
				{ 38, new string[] { "berry", "berries" } },
				{ 49, new string[] { "door" } },
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
				{ 1, new string[] { "king" } },
				{ 2, new string[] { "bat" } },
				{ 3, new string[] { "woman" } },
				{ 5, new string[] { "warrior" } },
				{ 6, new string[] { "monster" } },
				{ 7, new string[] { "wolf" } },
				{ 8, new string[] { "squid" } },
			};

			foreach (var synonym in synonyms)
			{
				CreateMonsterSynonyms(synonym.Key, synonym.Value);
			}
		}

		public Engine()
		{
			PoundCharPolicy = PoundCharPolicy.None;
		}
	}
}
