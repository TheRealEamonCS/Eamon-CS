
// Engine.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using Eamon;
using Eamon.Framework;
using Eamon.Framework.Primitive.Classes;
using static BeginnersCaveII.Game.Plugin.Globals;

namespace BeginnersCaveII.Game.Plugin
{
	public class Engine : EamonRT.Game.Plugin.Engine, Framework.Plugin.IEngine
	{
		StringBuilder Framework.Plugin.IEngine.Buf { get; set; }

		StringBuilder Framework.Plugin.IEngine.Buf01 { get; set; }

		/// <summary></summary>
		public virtual long HeldWpnIdx { get; set; }

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

			var rl = RollDice(1, 100, 0);

			// Cave rat

			if (monster.Uid == 3)
			{
				Out.Write("{0}{1} {2} at you.", Environment.NewLine, monster.GetTheName(true), rl > 66 ? "squeals" : rl > 33 ? "squeaks" : "hisses");
			}

			// Large snake

			else if (monster.Uid == 5)
			{
				Out.Write("{0}{1} hisses at you.", Environment.NewLine, monster.GetTheName(true));
			}

			// Wild boar / bull

			else if (monster.Uid == 6 || monster.Uid == 14)
			{
				Out.Write("{0}{1} {2} at you.", Environment.NewLine, monster.GetTheName(true), rl > 66 ? "grunts" : rl > 33 ? "bellows" : "snorts");
			}

			// Wild dog

			else if (monster.Uid == 10)
			{
				Out.Write("{0}{1} {2} at you.", Environment.NewLine, monster.GetTheName(true), rl > 50 ? "growls" : "barks");
			}

			// Black widow

			else if (monster.Uid == 16)
			{
				Out.Write("{0}{1} is not responsive.", Environment.NewLine, monster.GetTheName(true));
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

			var i = Array.FindIndex(Character.Weapons, x => x == weapon);

			if (i != GameState.UsedWpnIdx)
			{
				artifact.SetInLimbo();

				GameState.SetHeldWpnUid(HeldWpnIdx++, artifact.Uid);
			}

			return artifact;
		}

		public override void ConvertToCarriedInventory(IList<IArtifact> weaponList)
		{
			for (var i = 0; i < GameState.HeldWpnUids.Length; i++)
			{
				if (GameState.GetHeldWpnUid(i) > 0)
				{
					var artifact = ADB[GameState.GetHeldWpnUid(i)];

					Debug.Assert(artifact != null);

					artifact.SetCarriedByMonster(gCharMonster);
				}
			}

			base.ConvertToCarriedInventory(weaponList);
		}

		public Engine()
		{
			((Framework.Plugin.IEngine)this).Buf = new StringBuilder(BufSize);

			((Framework.Plugin.IEngine)this).Buf01 = new StringBuilder(BufSize);
		}
	}
}
