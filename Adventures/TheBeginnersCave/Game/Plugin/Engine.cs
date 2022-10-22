
// Engine.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using Eamon;
using Eamon.Framework;
using Eamon.Framework.Primitive.Classes;
using static TheBeginnersCave.Game.Plugin.Globals;

namespace TheBeginnersCave.Game.Plugin
{
	public class Engine : EamonRT.Game.Plugin.Engine, Framework.Plugin.IEngine
	{
		public virtual string AlightDesc { get; protected set; } = "(alight)";

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

		public override void PrintTooManyWeapons()
		{
			Out.Print("As you leave for the Main Hall, the Knight Marshal reappears and tells you, \"You have too many weapons to keep them all, four is the legal limit.\"");
		}

		public override void InitArtifacts()
		{
			base.InitArtifacts();

			MacroFuncs.Add(1, () =>
			{
				if (GameState != null && gGameState.Trollsfire == 1)
				{
					return string.Format("{0}{0}Trollsfire is alight!", Environment.NewLine);
				}
				else
				{
					return "";
				}
			});

			var synonyms = new Dictionary<long, string[]>()
			{
				{ 3, new string[] { "label", "strange potion", "potion", "bottle" } },
				{ 14, new string[] { "east wall", "wall", "smooth shape", "shape", "secret door", "door", "passage", "tunnel" } },
				{ 15, new string[] { "water", "sea water", "ocean water" } },
				{ 24, new string[] { "broken old boat", "old broken boat", "broken boat", "old boat" } },
			};

			foreach (var synonym in synonyms)
			{
				CreateArtifactSynonyms(synonym.Key, synonym.Value);
			}
		}

		public override void InitMonsters()
		{
			base.InitMonsters();

			var pirateMonster = MDB[8];

			Debug.Assert(pirateMonster != null);

			if (pirateMonster.Weapon == 10)
			{
				gGameState.Trollsfire = 1;
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

					artifact.SetCarriedByCharacter();
				}
			}

			base.ConvertToCarriedInventory(weaponList);
		}

		public override void MonsterDies(IMonster ActorMonster, IMonster DobjMonster)
		{
			Debug.Assert(DobjMonster != null && !DobjMonster.IsCharacterMonster());

			if (DobjMonster.Uid == 8)
			{
				var trollsfireArtifact = ADB[10];

				Debug.Assert(trollsfireArtifact != null);

				var printEffect = trollsfireArtifact.IsCarriedByMonster(DobjMonster) || trollsfireArtifact.IsWornByMonster(DobjMonster);

				base.MonsterDies(ActorMonster, DobjMonster);

				if (printEffect)
				{
					var effect = EDB[3];

					Debug.Assert(effect != null);

					Out.Write("{0}{0}{1}", Environment.NewLine, effect.Desc);
				}
			}
			else
			{
				base.MonsterDies(ActorMonster, DobjMonster);
			}
		}
	}
}
