
// AttackCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static WrenholdsSecretVigil.Game.Plugin.Globals;

namespace WrenholdsSecretVigil.Game.Commands
{
	[ClassMappings]
	public class AttackCommand : EamonRT.Game.Commands.AttackCommand, IAttackCommand
	{
		public override void Execute()
		{
			Debug.Assert(DobjArtifact != null || DobjMonster != null);

			var diaryArtifact = gADB[3];

			Debug.Assert(diaryArtifact != null);

			var wpnArtifact = ActorMonster.Weapon > 0 ? gADB[ActorMonster.Weapon] : null;

			var ac = wpnArtifact != null ? wpnArtifact.GeneralWeapon : null;

			// Find diary on dead adventurer

			if ((BlastSpell || ActorMonster.Weapon > 0) && DobjArtifact != null && DobjArtifact.Uid == 2 && diaryArtifact.IsInLimbo())
			{
				base.Execute();

				gEngine.MiscEventFuncList02.Add(() =>
				{
					gOut.Print("Inside the tattered shirt is a small cloth-bound book that appears to be a diary.");

					diaryArtifact.SetInRoom(ActorRoom);
				});
			}
			else if (BlastSpell)
			{
				// Blast rock

				if (DobjArtifact != null && DobjArtifact.Uid == 17)
				{
					gEngine.PrintEffectDesc(14);

					DobjArtifact.SetInLimbo();

					NextState = gEngine.CreateInstance<IMonsterStartState>();
				}

				// Blast slime

				else if (DobjArtifact != null && (DobjArtifact.Uid == 24 || DobjArtifact.Uid == 25))
				{
					var slimeArtifact1 = gADB[24];

					Debug.Assert(slimeArtifact1 != null);

					var slimeArtifact2 = gADB[25];

					Debug.Assert(slimeArtifact2 != null);

					gGameState.SlimeBlasts++;

					gEngine.PrintEffectDesc(1 + gGameState.SlimeBlasts);

					if (gGameState.SlimeBlasts == 3)
					{
						slimeArtifact1.SetInLimbo();

						slimeArtifact2.SetInLimbo();
					}

					NextState = gEngine.CreateInstance<IMonsterStartState>();
				}
				else
				{
					base.Execute();
				}
			}

			// Attack slime will dissolve weapon (bows excluded)

			else if (DobjArtifact != null && (DobjArtifact.Uid == 24 || DobjArtifact.Uid == 25) && ac != null && ac.Field2 != (long)Weapon.Bow)
			{
				gEngine.PrintEffectDesc(18);

				if (gGameState.Ls == wpnArtifact.Uid)
				{
					gEngine.LightOut(wpnArtifact);
				}

				wpnArtifact.SetInLimbo();

				var rc = wpnArtifact.RemoveStateDesc(wpnArtifact.GetReadyWeaponDesc());

				Debug.Assert(gEngine.IsSuccess(rc));

				ActorMonster.Weapon = -1;

				NextState = gEngine.CreateInstance<IMonsterStartState>();
			}
			else
			{
				base.Execute();
			}
		}
	}
}
