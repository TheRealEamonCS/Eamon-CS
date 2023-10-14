
// MainLoop.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using System.Linq;
using Eamon.Game.Attributes;
using EamonRT.Framework;
using static TheWayfarersInn.Game.Plugin.Globals;

namespace TheWayfarersInn.Game
{
	[ClassMappings]
	public class MainLoop : EamonRT.Game.MainLoop, IMainLoop
	{
		public override void Shutdown()
		{
			long reward = 0;

			var propertyDeedArtifact = gADB[70];

			Debug.Assert(propertyDeedArtifact != null);

			var burlapSackArtifact = gADB[34];

			Debug.Assert(burlapSackArtifact != null);

			var childsSkeletonArtifact = gADB[54];

			Debug.Assert(childsSkeletonArtifact != null);

			var diaryArtifact = gADB[85];

			Debug.Assert(diaryArtifact != null);

			var fineClothingArtifact = gADB[185];

			Debug.Assert(fineClothingArtifact != null);

			// If fine clothing boosting Charisma don't sell to Sam Slicker

			if (fineClothingArtifact.IsWornByMonster(gCharMonster) && gGameState.FineClothingEnchanted)
			{
				fineClothingArtifact.SetInLimbo();
			}

			// Reward for overdue property taxes levied on former owner (Greer Blackthorn)	

			if (propertyDeedArtifact.IsCarriedByMonster(gCharMonster, true))
			{
				gOut.Print("{0}", gEngine.LineSep);

				gEngine.PrintEffectDesc(228);

				reward += 625;

				propertyDeedArtifact.SetInLimbo();

				gEngine.In.KeyPress(gEngine.Buf);
			}

			var bodyPartsList = burlapSackArtifact.GetContainedList(recurse: true).Where(a => ((Framework.IArtifact)a).IsArtisanBodyPartArtifact()).ToList();

			Debug.Assert(bodyPartsList != null);

			// Reward for recovery of artisan renovators for proper burial

			if (burlapSackArtifact.IsCarriedByMonster(gCharMonster, true) && bodyPartsList.Count > 0)
			{
				gOut.Print("{0}", gEngine.LineSep);

				gEngine.PrintEffectDesc(235);

				reward += (long)Math.Round(((double)bodyPartsList.Count / (double)6) * 250);		// TODO: eliminate hardcode

				gEngine.In.KeyPress(gEngine.Buf);
			}

			bodyPartsList = gEngine.GetArtifactList(a => ((Framework.IArtifact)a).IsArtisanBodyPartArtifact()).ToList();

			foreach (var artifact in bodyPartsList)
			{
				artifact.SetInLimbo();
			}

			var artifactList = burlapSackArtifact.GetContainedList(recurse: true).ToList();

			Debug.Assert(artifactList != null);

			// Move burlap sack contents into player inventory

			if (burlapSackArtifact.IsCarriedByMonster(gCharMonster, true))
			{
				foreach (var artifact in artifactList)
				{
					artifact.SetCarriedByMonster(gCharMonster);
				}

				burlapSackArtifact.SetInLimbo();
			}

			// Reward from investors for eliminating haunting and making inn viable again

			if (gGameState.CharlotteRestInPeace)
			{
				gEngine.SyndicateReward = 500;

				var monsterList = gEngine.GetMonsterList(m => m.Field2 > 0);

				var rewardPerMonster = 500.0 / (double)monsterList.Count;

				foreach (var monster in monsterList)
				{
					var multiplier = (double)gGameState.GetMonsterTotalDmgTaken(monster.Uid) / (double)monster.Field2;

					gEngine.SyndicateReward += (long)Math.Round(rewardPerMonster * multiplier);
				}

				if (gEngine.SyndicateReward > 1000)
				{
					gEngine.SyndicateReward = 1000;
				}

				gOut.Print("{0}", gEngine.LineSep);

				gEngine.PrintEffectDesc(237);

				reward += gEngine.SyndicateReward;

				gEngine.In.KeyPress(gEngine.Buf);
			}

			// Reward from Innkeeper for Charlotte's recovery

			if (childsSkeletonArtifact.IsCarriedByMonster(gCharMonster, true))
			{
				gOut.Print("{0}", gEngine.LineSep);

				gEngine.PrintEffectDesc(229);

				reward += 250;

				childsSkeletonArtifact.SetInLimbo();

				gEngine.In.KeyPress(gEngine.Buf);

				// Greer Blackthorn jailed, killer executed

				if (diaryArtifact.IsCarriedByMonster(gCharMonster, true))
				{
					gOut.Print("{0}", gEngine.LineSep);

					gEngine.PrintEffectDesc(231);

					diaryArtifact.SetInLimbo();

					gEngine.In.KeyPress(gEngine.Buf);
				}
			}

			if (reward != 0)
			{
				gCharacter.HeldGold += reward;
			}
			else
			{
				gOut.Print("{0}", gEngine.LineSep);

				gEngine.PrintEffectDesc(226);

				gEngine.In.KeyPress(gEngine.Buf);
			}

			base.Shutdown();
		}
	}
}
