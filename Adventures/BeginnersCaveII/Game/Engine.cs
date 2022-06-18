
// Engine.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Eamon.Framework;
using Eamon.Framework.Primitive.Classes;
using Eamon.Game.Attributes;
using static BeginnersCaveII.Game.Plugin.PluginContext;

namespace BeginnersCaveII.Game
{
	[ClassMappings(typeof(IEngine))]
	public class Engine : EamonRT.Game.Engine, EamonRT.Framework.IEngine
	{
		/// <summary></summary>
		public virtual long HeldWpnIdx { get; set; }

		public override void PrintMonsterEmotes(IMonster monster, bool friendSmile = true)
		{
			Debug.Assert(monster != null);

			var rl = RollDice(1, 100, 0);

			// Cave rat

			if (monster.Uid == 3)
			{
				gOut.Write("{0}{1} {2} at you.", Environment.NewLine, monster.GetTheName(true), rl > 80 ? "squeals" : rl > 50 ? "squeaks" : "hisses");
			}

			// Large snake

			else if (monster.Uid == 5)
			{
				gOut.Write("{0}{1} hisses at you.", Environment.NewLine, monster.GetTheName(true));
			}

			// Wild boar / bull

			else if (monster.Uid == 6 || monster.Uid == 14)
			{
				gOut.Write("{0}{1} {2} at you.", Environment.NewLine, monster.GetTheName(true), rl > 80 ? "grunts" : rl > 50 ? "bellows" : "snorts");
			}

			// Wild dog

			else if (monster.Uid == 10)
			{
				gOut.Write("{0}{1} {2} at you.", Environment.NewLine, monster.GetTheName(true), rl > 50 ? "growls" : "barks");
			}

			// Black widow

			else if (monster.Uid == 16)
			{
				gOut.Write("{0}{1} is not responsive.", Environment.NewLine, monster.GetTheName(true));
			}
			else
			{
				base.PrintMonsterEmotes(monster, friendSmile);
			}
		}

		public override void InitArtifacts()
		{
			base.InitArtifacts();

			var synonyms = new Dictionary<long, string[]>()
			{
				{ 21, new string[] { "book", "shelf", "shelves" } },
				{ 22, new string[] { "rock", "wall", "south wall" } },
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
