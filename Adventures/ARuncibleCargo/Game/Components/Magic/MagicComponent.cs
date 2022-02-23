
// MagicComponent.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Linq;
using Eamon.Game.Attributes;
using EamonRT.Framework.Components;
using static ARuncibleCargo.Game.Plugin.PluginContext;

namespace ARuncibleCargo.Game.Components
{
	[ClassMappings]
	public class MagicComponent : EamonRT.Game.Components.MagicComponent, IMagicComponent
	{
		public override bool ShouldShowBlastSpellAttack()
		{
			var artUids = new long[] { 30, 80, 41, 129 };

			return (DobjMonster != null || !artUids.Contains(DobjArtifact.Uid)) && base.ShouldShowBlastSpellAttack();
		}
	}
}
