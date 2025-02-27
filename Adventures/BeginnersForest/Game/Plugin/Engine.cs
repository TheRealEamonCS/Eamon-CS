
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
using Eamon.Framework.Primitive.Enums;
using static BeginnersForest.Game.Plugin.Globals;

namespace BeginnersForest.Game.Plugin
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

		public override void AddUniqueCharsToArtifactAndMonsterNames()
		{
			base.AddUniqueCharsToArtifactAndMonsterNames();

			// Entrance/exit gates

			var entryGateArtifact = ADB[19];

			Debug.Assert(entryGateArtifact != null);

			entryGateArtifact.Seen = true;

			entryGateArtifact.Name = entryGateArtifact.Name.TrimEnd('#');

			var exitGateArtifact = ADB[20];

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

			var spookMonster = MDB[9];

			Debug.Assert(spookMonster != null);

			spookMonster.GroupCount = 0;

			spookMonster.InitGroupCount = 0;

			spookMonster.CurrGroupCount = 0;

			var sirGrummorMonster = MDB[4];

			Debug.Assert(sirGrummorMonster != null);

			if (Character.Gender == Gender.Female)
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

			var i = Array.FindIndex(Character.Weapons, x => x == weapon);

			if (i != GameState.UsedWpnIdx)
			{
				artifact.SetInLimbo();

				GameState.SetHeldWpnUid(HeldWpnIdx++, artifact.Uid);
			}

			return artifact;
		}

		public override void ResetMonsterStats(IMonster monster)
		{
			Debug.Assert(monster != null && monster.IsCharacterMonster());

			if (GameState.Speed > 0)
			{
				monster.Agility /= 2;

				var ringArtifact = ADB[2];

				Debug.Assert(ringArtifact != null);

				ringArtifact.SetInLimbo();
			}

			GameState.Speed = 0;
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

		public override void MonsterDies(IMonster actorMonster, IMonster dobjMonster)
		{
			Debug.Assert(dobjMonster != null && !dobjMonster.IsCharacterMonster());

			// Repetitive Spooks' counter reset

			if (dobjMonster.Uid == 9)
			{
				var resetGroupCount = dobjMonster.CurrGroupCount == 1;

				base.MonsterDies(actorMonster, dobjMonster);

				if (resetGroupCount)
				{
					dobjMonster.GroupCount = 0;

					dobjMonster.InitGroupCount = 0;

					dobjMonster.CurrGroupCount = 0;
				}
			}
			else
			{
				base.MonsterDies(actorMonster, dobjMonster);
			}
		}

		public Engine()
		{
			((Framework.Plugin.IEngine)this).Buf = new StringBuilder(BufSize);

			((Framework.Plugin.IEngine)this).Buf01 = new StringBuilder(BufSize);
		}
	}
}
