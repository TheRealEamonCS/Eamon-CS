
// Engine.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Collections.Generic;
using System.Diagnostics;
using Eamon.Framework;
using Eamon.Game.Attributes;
using static TheTrainingGround.Game.Plugin.PluginContext;

namespace TheTrainingGround.Game
{
	[ClassMappings(typeof(IEngine))]
	public class Engine : EamonRT.Game.Engine, EamonRT.Framework.IEngine
	{
		public override void AddUniqueCharsToArtifactAndMonsterNames()
		{
			base.AddUniqueCharsToArtifactAndMonsterNames();

			// Obsidian scroll case

			var scrollCaseArtifact = gADB[51];

			Debug.Assert(scrollCaseArtifact != null);

			scrollCaseArtifact.Seen = true;

			scrollCaseArtifact.Name = scrollCaseArtifact.Name.TrimEnd('#');

			// Graffiti

			for (var i = 46; i <= 50; i++)
			{
				var graffitiArtifact = gADB[i];

				Debug.Assert(graffitiArtifact != null);

				graffitiArtifact.Seen = true;

				graffitiArtifact.Name = graffitiArtifact.Name.TrimEnd('#');
			}
		}

		public override void PrintMonsterAlive(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			// Obsidian scroll case

			if (artifact.Uid != 30)
			{
				base.PrintMonsterAlive(artifact);
			}
		}

		public override void InitArtifacts()
		{
			base.InitArtifacts();

			var wallValues = new string[] { "walls", "wall", "writing", "writings", "marks", "markings" };

			var synonyms = new Dictionary<long, string[]>()
			{
				// Graffiti

				{ 46, wallValues },
				{ 47, wallValues },
				{ 48, wallValues },
				{ 49, wallValues },
				{ 50, wallValues },
			};

			foreach (var synonym in synonyms)
			{
				CreateArtifactSynonyms(synonym.Key, synonym.Value);
			}
		}

		public override void InitMonsters()
		{
			base.InitMonsters();

			// Kobold6 is mentioned in Kobold5's description

			var monster = gMDB[11];

			Debug.Assert(monster != null);

			monster.Seen = true;
		}

		public override void RevealDisguisedMonster(IRoom room, IArtifact artifact)
		{
			Debug.Assert(room != null);

			Debug.Assert(artifact != null);

			base.RevealDisguisedMonster(room, artifact);

			// Replace obsidian scroll case with dummy

			if (artifact.Uid == 30)
			{
				var scrollCaseArtifact = gADB[51];

				Debug.Assert(scrollCaseArtifact != null);

				scrollCaseArtifact.SetInRoomUid(gGameState.Ro);
			}
		}
	}
}
