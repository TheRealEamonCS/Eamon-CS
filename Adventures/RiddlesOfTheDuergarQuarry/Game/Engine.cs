
// Engine.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Collections.Generic;
using System.Linq;
using Eamon.Framework;
using Eamon.Game.Attributes;
using static RiddlesOfTheDuergarQuarry.Game.Plugin.PluginContext;

namespace RiddlesOfTheDuergarQuarry.Game
{
	[ClassMappings(typeof(IEngine))]
	public class Engine : EamonRT.Game.Engine, EamonRT.Framework.IEngine
	{
		public override void InitArtifacts()
		{
			base.InitArtifacts();

			MacroFuncs.Add(1, () =>
			{
				var roomUids = new long[] { };

				var result = "east and west";

				if (gGameState != null && roomUids.Contains(gGameState.Ro))
				{
					result = "north and south";
				}

				return result;
			});

			MacroFuncs.Add(2, () =>
			{
				var result = "northeast";

				if (gGameState != null)
				{
					// TODO: implement
				}

				return result;
			});

			var synonyms = new Dictionary<long, string[]>()
			{
				{ 1, new string[] { "everdes", "volcano", "mountain" } },
				{ 2, new string[] { "purple lavender", "sage", "lavender" } },
				{ 3, new string[] { "dust devils", "devils", "whirlwinds", "devil", "whirlwind" } },
				{ 5, new string[] { "fence" } },
				{ 14, new string[] { "gate" } },
				{ 77, new string[] { "narrow crevice", "vertical crevice", "crevice", "crevace", "fissure" } },
				{ 78, new string[] { "large rock", "boulder", "rock" } },
				{ 79, new string[] { "bell", "chime", "hammer" } },
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
				{ 18, new string[] { "viper", "snake" } },
			};

			foreach (var synonym in synonyms)
			{
				CreateMonsterSynonyms(synonym.Key, synonym.Value);
			}
		}
	}
}
