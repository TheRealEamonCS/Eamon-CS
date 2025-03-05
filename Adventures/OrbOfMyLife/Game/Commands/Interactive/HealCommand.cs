
// HealCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static OrbOfMyLife.Game.Plugin.Globals;

namespace OrbOfMyLife.Game.Commands
{
	[ClassMappings]
	public class HealCommand : EamonRT.Game.Commands.HealCommand, IHealCommand
	{
		public override void ExecuteForPlayer()
		{
			// Sagonne

			if (DobjMonster?.Uid == 22 && gCharacter.GetSpellAbility(Spell.Heal) > 0)
			{
				gOut.Print("Sagonne can only be healed with his Life Orb.");

				NextState = gEngine.CreateInstance<IStartState>();
			}
			else
			{
				base.ExecuteForPlayer();
			}
		}
	}
}
