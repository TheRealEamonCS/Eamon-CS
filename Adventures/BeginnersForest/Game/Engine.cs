
// Engine.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Eamon.Framework;
using Eamon.Framework.Primitive.Classes;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using static BeginnersForest.Game.Plugin.PluginContext;

namespace BeginnersForest.Game
{
	[ClassMappings(typeof(IEngine))]
	public class Engine : EamonRT.Game.Engine, EamonRT.Framework.IEngine
	{
		/// <summary></summary>
		public virtual long HeldWpnIdx { get; set; }

		public override void AddUniqueCharsToArtifactAndMonsterNames()
		{
			base.AddUniqueCharsToArtifactAndMonsterNames();

			// Entrance/exit gates

			var entryGateArtifact = gADB[19];

			Debug.Assert(entryGateArtifact != null);

			entryGateArtifact.Seen = true;

			entryGateArtifact.Name = entryGateArtifact.Name.TrimEnd('#');

			var exitGateArtifact = gADB[20];

			Debug.Assert(exitGateArtifact != null);

			exitGateArtifact.Seen = true;

			exitGateArtifact.Name = exitGateArtifact.Name.TrimEnd('#');
		}

		public override void InitArtifacts()
		{
			base.InitArtifacts();

			var gateValues = new string[] { "green arch", "vine covered arch", "arch", "vines", "vine", "wrought iron gate", "gate", "words" };

			var synonyms = new Dictionary<long, string[]>()
			{
				// Hidden bridge

				{ 17, new string[] { "giant green blanket", "green blanket", "giant blanket", "giant green", "blanket", "large shape", "shape" } },

				// Entrance/exit gates

				{ 19, gateValues },
				{ 20, gateValues },
			};

			foreach (var synonym in synonyms)
			{
				CreateArtifactSynonyms(synonym.Key, synonym.Value);
			}
		}

		public override void InitMonsters()
		{
			base.InitMonsters();

			// Set Group Spooks to 0 for Spook routine

			var spookMonster = gMDB[9];

			Debug.Assert(spookMonster != null);

			spookMonster.GroupCount = 0;

			spookMonster.InitGroupCount = 0;

			spookMonster.CurrGroupCount = 0;

			var sirGrummorMonster = gMDB[4];

			Debug.Assert(sirGrummorMonster != null);

			if (gCharacter.Gender == Gender.Female)
			{
				// Queen's gift

				gGameState.QueenGiftEffectUid = 6;

				gGameState.QueenGiftArtifactUid = 15;

				// Sir Grummor is always kind to the ladies!

				sirGrummorMonster.Friendliness = (Friendliness)200;

				sirGrummorMonster.Reaction = Friendliness.Friend;
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

		public override void MonsterDies(IMonster OfMonster, IMonster DfMonster)
		{
			Debug.Assert(DfMonster != null && !DfMonster.IsCharacterMonster());

			// Repetitive Spooks' counter reset

			if (DfMonster.Uid == 9)
			{
				var resetGroupCount = DfMonster.CurrGroupCount == 1;

				base.MonsterDies(OfMonster, DfMonster);

				if (resetGroupCount)
				{
					DfMonster.GroupCount = 0;

					DfMonster.InitGroupCount = 0;

					DfMonster.CurrGroupCount = 0;
				}
			}
			else
			{
				base.MonsterDies(OfMonster, DfMonster);
			}
		}
	}
}
