
// Engine.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Eamon.Framework;
using Eamon.Framework.Primitive.Classes;
using Eamon.Game.Attributes;
using static AlternateBeginnersCave.Game.Plugin.PluginContext;

namespace AlternateBeginnersCave.Game
{
	[ClassMappings(typeof(IEngine))]
	public class Engine : EamonRT.Game.Engine, EamonRT.Framework.IEngine
	{
		/// <summary></summary>
		public virtual long HeldWpnIdx { get; set; }

		public override void AddUniqueCharsToArtifactAndMonsterNames()
		{
			base.AddUniqueCharsToArtifactAndMonsterNames();

			var signArtifact = gADB[23];     // Sign #2

			Debug.Assert(signArtifact != null);

			signArtifact.Name = signArtifact.Name.TrimEnd('#');
		}

		public override void InitArtifacts()
		{
			base.InitArtifacts();

			var synonyms = new Dictionary<long, string[]>()
			{
				{ 5, new string[] { "label" } },
				{ 20, new string[] { "vulture" } },
				{ 21, new string[] { "odd-looking torch", "odd looking torch", "odd torch", "torch" } },
				{ 22, new string[] { "massive inset ring", "inset ring", "ring" } },
				{ 37, new string[] { "shimmering blank wall", "shimmering wall", "blank wall", "east wall", "blank", "wall" } },
			};

			foreach (var synonym in synonyms)
			{
				CreateArtifactSynonyms(synonym.Key, synonym.Value);
			}
		}

		public override IArtifact ConvertWeaponToArtifact(ICharacterArtifact weapon)
		{
			var artifact = base.ConvertWeaponToArtifact(weapon);

			var i = Array.FindIndex(gCharacter.Weapons, x => x == weapon);

			if (i != gGameState.UsedWpnIdx)
			{
				artifact.SetInLimbo();

				gGameState.SetHeldWpnUids(HeldWpnIdx++, artifact.Uid);
			}

			return artifact;
		}

		public override void ResetMonsterStats(IMonster monster)
		{
			Debug.Assert(monster != null && monster.IsCharacterMonster());

			base.ResetMonsterStats(monster);

			// Eighty-six the weird magic dagger-related Hardiness increase upon exit (omitting the in-game death check for simplicity)

			if (gGameState.MagicDaggerCounter > 0)
			{
				monster.Hardiness -= 5;

				gGameState.MagicDaggerCounter = -1;
			}
		}

		public override void ConvertToCarriedInventory(IList<IArtifact> weaponList)
		{
			for (var i = 0; i < gGameState.HeldWpnUids.Length; i++)
			{
				if (gGameState.GetHeldWpnUids(i) > 0)
				{
					var artifact = gADB[gGameState.GetHeldWpnUids(i)];

					Debug.Assert(artifact != null);

					artifact.SetCarriedByCharacter();
				}
			}

			base.ConvertToCarriedInventory(weaponList);
		}
	}
}
