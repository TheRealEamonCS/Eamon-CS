
// CombatComponent.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Game.Attributes;
using EamonRT.Framework.Components;

namespace TheTempleOfNgurct.Game.Components
{
	[ClassMappings]
	public class CombatComponent : EamonRT.Game.Components.CombatComponent, ICombatComponent
	{
		public override void ExecuteAttack()
		{
			// Only use Fractional strength for regular attacks

			if (!BlastSpell)
			{
				UseFractionalStrength = true;
			}

			base.ExecuteAttack();
		}
	}
}
