﻿
// Engine.cs

// Copyright (c) 2014+ by Kenneth Pedersen.  All rights reserved.

using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using Eamon;
using Eamon.Framework.Primitive.Enums;
using static LandOfTheMountainKing.Game.Plugin.Globals;

namespace LandOfTheMountainKing.Game.Plugin
{
	public class Engine : EamonRT.Game.Plugin.Engine, Framework.Plugin.IEngine
	{
		StringBuilder Framework.Plugin.IEngine.Buf { get; set; }

		StringBuilder Framework.Plugin.IEngine.Buf01 { get; set; }

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

			MacroFuncs.Add(1, () =>
			{
				var lampDirDescs = new string[]
				{
					"",
					"to the south over the ocean",
					"to the southwest over the ocean",
					"to the west onto the cliffs",
					"to the northwest into the forest",
					"to the north onto the bridge crossing the ravine",
					"to the northeast where a huge tree is standing",
					"to the east over the ocean beyond the beach",
					"to the southeast over the ocean"
				};

				var index = gLMKKP1 != null ? gLMKKP1.Lampdir : 7;

				Debug.Assert(index >= 1 && index <= 8);

				return lampDirDescs[index];
			});

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
			((Framework.Plugin.IEngine)this).Buf = new StringBuilder(BufSize);

			((Framework.Plugin.IEngine)this).Buf01 = new StringBuilder(BufSize);

			PoundCharPolicy = PoundCharPolicy.None;
		}
	}
}
