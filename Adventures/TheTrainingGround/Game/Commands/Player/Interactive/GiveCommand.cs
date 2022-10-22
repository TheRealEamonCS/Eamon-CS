
// GiveCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static TheTrainingGround.Game.Plugin.Globals;

namespace TheTrainingGround.Game.Commands
{
	[ClassMappings]
	public class GiveCommand : EamonRT.Game.Commands.GiveCommand, IGiveCommand
	{
		public override void PrintGiveObjToActor(IArtifact artifact, IMonster monster)
		{
			Debug.Assert(artifact != null && monster != null);

			base.PrintGiveObjToActor(artifact, monster);

			// Give rapier to Jacques

			if (monster.Uid == 5 && artifact.Uid == 8 && !gGameState.JacquesRecoversRapier)
			{
				gEngine.PrintEffectDesc(22);

				gGameState.JacquesRecoversRapier = true;
			}
		}

		public override void ProcessEvents(EventType eventType)
		{
			base.ProcessEvents(eventType);

			if (eventType == EventType.AfterEnforceMonsterWeightLimitsCheck)
			{
				// Give obsidian scroll case to Emerald Warrior

				if (IobjMonster.Uid == 14 && DobjArtifact.Uid == 51)
				{
					DobjArtifact.SetInLimbo();

					IobjMonster.SetInLimbo();

					gEngine.PrintEffectDesc(14);

					GotoCleanup = true;
				}
			}
			else if (eventType == EventType.BeforeTakePlayerGold)
			{
				// Buy potion from gnome

				if (IobjMonster.Uid == 20)
				{
					if (GoldAmount >= 100)
					{
						var redPotionArtifact = gADB[40];

						Debug.Assert(redPotionArtifact != null);

						var bluePotionArtifact = gADB[41];

						Debug.Assert(bluePotionArtifact != null);

						if (redPotionArtifact.IsCarriedByMonsterUid(20) || bluePotionArtifact.IsCarriedByMonsterUid(20))
						{
							gCharacter.HeldGold -= GoldAmount;

							if (GoldAmount > 100)
							{
								gEngine.PrintEffectDesc(30);
							}

							var potionArtifact = redPotionArtifact.IsCarriedByMonsterUid(20) ? redPotionArtifact : bluePotionArtifact;

							potionArtifact.SetInRoomUid(gGameState.Ro);

							gEngine.PrintEffectDesc(31);

							NextState = gEngine.CreateInstance<IStartState>();
						}
						else
						{
							gEngine.PrintEffectDesc(29);
						}
					}
					else
					{
						gEngine.PrintEffectDesc(28);
					}

					GotoCleanup = true;
				}
			}
		}
	}
}
