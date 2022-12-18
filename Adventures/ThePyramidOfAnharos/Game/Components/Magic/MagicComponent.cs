
// MagicComponent.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Game.Attributes;
using EamonRT.Framework.Components;
using EamonRT.Framework.Primitive.Enums;
using static ThePyramidOfAnharos.Game.Plugin.Globals;

namespace ThePyramidOfAnharos.Game.Components
{
	[ClassMappings]
	public class MagicComponent : EamonRT.Game.Components.MagicComponent, IMagicComponent
	{
		public override void CheckAfterCastPower()
		{
			PowerEventRoll = gEngine.RollDice(1, 100, 0);

			if (PowerEventRoll > 80 /* && ActorRoom.Zone != 2 */)
			{
				gOut.Print("A dust storm blows up suddenly. The stinging sand flays your skin.");

				var combatComponent = gEngine.CreateInstance<ICombatComponent>(x =>
				{
					x.SetNextStateFunc = SetNextStateFunc;

					x.ActorRoom = ActorRoom;

					x.Dobj = ActorMonster;

					x.OmitArmor = true;
				});

				combatComponent.ExecuteCalculateDamage(1, 1);
			}
			else if (PowerEventRoll <= 40)
			{
				gOut.Print("A golden glow illuminates the area.");

				gEngine.ResurrectDeadBodies(ActorRoom, a => a.Uid >= 56 && a.Uid <= 75 && (a.IsCarriedByCharacter() || a.IsInRoom(ActorRoom)));
			}
			else
			{
				gOut.Print("An unseen voice speaks the following:");

				if (PowerEventRoll > 70)
				{
					gOut.Print("   Anharos must be made whole again.");
				}
				else if (PowerEventRoll > 60)
				{
					gOut.Print("   Follow the spectrum precisely.");
				}
				else if (PowerEventRoll > 50)
				{
					gOut.Print("   Remember - For Anharos.");
				}
				else if (PowerEventRoll > 40)
				{
					gOut.Print("   Only the pure can resist the gem.");
				}
			}

			MagicState = MagicState.EndMagic;
		}
	}
}
