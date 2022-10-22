
// MagicComponent.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Components;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static TheTrainingGround.Game.Plugin.Globals;

namespace TheTrainingGround.Game.Components
{
	[ClassMappings]
	public class MagicComponent : EamonRT.Game.Components.MagicComponent, IMagicComponent
	{
		public override void CheckAfterCastBlast()
		{
			// BLAST Bozworth

			if (DobjMonster != null && DobjMonster.Uid == 20)
			{
				gEngine.PrintEffectDesc(21);

				DobjMonster.SetInLimbo();

				SetNextStateFunc(gEngine.CreateInstance<IStartState>());

				MagicState = MagicState.EndMagic;

				goto Cleanup;
			}

			base.CheckAfterCastBlast();

		Cleanup:

			;
		}

		public override void CheckAfterCastPower()
		{
			var hammerArtifact = gADB[24];

			Debug.Assert(hammerArtifact != null);

			// Thor's hammer appears in Norse Mural room

			if (ActorRoom.Uid == 22 && !gGameState.ThorsHammerAppears)
			{
				hammerArtifact.SetInRoom(ActorRoom);

				gEngine.PrintEffectDesc(7);

				gGameState.ThorsHammerAppears = true;

				SetNextStateFunc(gEngine.CreateInstance<IStartState>());

				goto Cleanup;
			}

			var rl = gEngine.RollDice(1, 100, 0);

			// 20% chance of gender change

			if (rl < 21 && gGameState.GenderChangeCounter < 2)
			{
				ActorMonster.Gender = ActorMonster.EvalGender(Gender.Female, Gender.Male, Gender.Neutral);

				gCharacter.Gender = ActorMonster.Gender;

				gOut.Print("You feel different... more {0}.", ActorMonster.EvalGender("masculine", "feminine", "androgynous"));

				gGameState.GenderChangeCounter++;

				goto Cleanup;
			}

			// 40% chance Charisma up (one time only)

			if (rl < 41 && !gGameState.CharismaBoosted)
			{
				gCharacter.ModStat(Stat.Charisma, 2);

				gOut.Print("You suddenly feel more {0}.", gCharacter.EvalGender("handsome", "beautiful", "androgynous"));

				gGameState.CharismaBoosted = true;

				goto Cleanup;
			}

			// 5% Chance of being hit by lightning!

			if (rl > 94)
			{
				gEngine.PrintEffectDesc(33);

				var combatComponent = gEngine.CreateInstance<ICombatComponent>(x =>
				{
					x.SetNextStateFunc = SetNextStateFunc;

					x.ActorRoom = ActorRoom;

					x.Dobj = ActorMonster;

					x.OmitArmor = true;
				});

				combatComponent.ExecuteCalculateDamage(1, 10);

				goto Cleanup;
			}

			PrintSonicBoom(ActorRoom);

		Cleanup:

			MagicState = MagicState.EndMagic;
		}
	}
}
