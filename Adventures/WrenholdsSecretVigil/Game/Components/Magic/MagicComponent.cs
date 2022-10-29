
// MagicComponent.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Linq;
using Eamon.Game.Attributes;
using EamonRT.Framework.Components;
using EamonRT.Framework.Primitive.Enums;
using static WrenholdsSecretVigil.Game.Plugin.Globals;

namespace WrenholdsSecretVigil.Game.Components
{
	[ClassMappings]
	public class MagicComponent : EamonRT.Game.Components.MagicComponent, IMagicComponent
	{
		public override bool ShouldShowBlastSpellAttack()
		{
			var artUids = new long[] { 17, 24, 25 };

			return (DobjMonster != null || !artUids.Contains(DobjArtifact.Uid)) && base.ShouldShowBlastSpellAttack();
		}

		public override void CheckAfterCastPower()
		{
			gEngine.PrintEffectDesc(45);

			MagicState = MagicState.EndMagic;
		}
	}
}
