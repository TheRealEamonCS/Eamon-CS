
// MagicComponent.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Game.Attributes;
using EamonRT.Framework.Components;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static OrbOfMyLife.Game.Plugin.Globals;

namespace OrbOfMyLife.Game.Components
{
	[ClassMappings]
	public class MagicComponent : EamonRT.Game.Components.MagicComponent, IMagicComponent
	{
		public override void CheckAfterAggravateMonster()
		{
			// Demon of Trowsk

			if (DobjMonster != null && DobjMonster.Uid == 12)
			{
				PrintZapDirectHit();

				gOut.Print("The demon crumbles to dust!");

				DobjMonster.SetInLimbo();

				gEngine.PauseCombat();

				MagicState = MagicState.EndMagic;

				goto Cleanup;
			}

			base.CheckAfterAggravateMonster();

		Cleanup:

			;
		}

		public override void CheckAfterCastPower()
		{
			if (ActorRoom.Uid == 68)
			{
				gEngine.PrintEffectDesc(34);

				gGameState.R2 = 65;

				SetNextStateFunc(gEngine.CreateInstance<IAfterPlayerMoveState>());

				MagicState = MagicState.EndMagic;
			}
			else
			{
				MagicState = MagicState.BeginSpellSpeed;
			}
		}
	}
}
