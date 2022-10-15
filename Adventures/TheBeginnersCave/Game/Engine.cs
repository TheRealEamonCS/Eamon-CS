
// Engine.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Eamon.Framework;
using Eamon.Framework.Primitive.Classes;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using static TheBeginnersCave.Game.Plugin.PluginContext;

namespace TheBeginnersCave.Game
{
	[ClassMappings(typeof(IEngine))]
	public class Engine : EamonRT.Game.Engine, EamonRT.Framework.IEngine
	{
		/// <summary></summary>
		public virtual long HeldWpnIdx { get; set; }

		public override void PrintTooManyWeapons()
		{
			gOut.Print("As you leave for the Main Hall, the Knight Marshal reappears and tells you, \"You have too many weapons to keep them all, four is the legal limit.\"");
		}

		public override void InitArtifacts()
		{
			base.InitArtifacts();

			MacroFuncs.Add(1, () =>
			{
				if (gGameState != null && gGameState.Trollsfire == 1)
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

			var pirateMonster = gMDB[8];

			Debug.Assert(pirateMonster != null);

			if (pirateMonster.Weapon == 10)
			{
				gGameState.Trollsfire = 1;
			}
		}

		public override IArtifact ConvertWeaponToArtifact(ICharacterArtifact weapon)
		{
			var artifact = base.ConvertWeaponToArtifact(weapon);

			var i = Array.FindIndex(gCharacter.Weapons, x => x == weapon);

			if (i != gGameState.UsedWpnIdx)
			{
				artifact.SetInLimbo();

				gGameState.SetHeldWpnUid(HeldWpnIdx++, artifact.Uid);
			}

			return artifact;
		}

		public override void ConvertToCarriedInventory(IList<IArtifact> weaponList)
		{
			for (var i = 0; i < gGameState.HeldWpnUids.Length; i++)
			{
				if (gGameState.GetHeldWpnUid(i) > 0)
				{
					var artifact = gADB[gGameState.GetHeldWpnUid(i)];

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
				var trollsfireArtifact = gADB[10];

				Debug.Assert(trollsfireArtifact != null);

				var printEffect = trollsfireArtifact.IsCarriedByMonster(DobjMonster) || trollsfireArtifact.IsWornByMonster(DobjMonster);

				base.MonsterDies(ActorMonster, DobjMonster);

				if (printEffect)
				{
					var effect = gEDB[3];

					Debug.Assert(effect != null);

					gOut.Write("{0}{0}{1}", Environment.NewLine, effect.Desc);
				}
			}
			else
			{
				base.MonsterDies(ActorMonster, DobjMonster);
			}
		}
	}
}
