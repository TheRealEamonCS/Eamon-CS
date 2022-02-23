
// BlastCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using static StrongholdOfKahrDur.Game.Plugin.PluginContext;

namespace StrongholdOfKahrDur.Game.Commands
{
	[ClassMappings]
	public class BlastCommand : EamonRT.Game.Commands.BlastCommand, IBlastCommand
	{
		public override bool ShouldAllowSkillGains()
		{
			// When Necromancer is blasted only allow skill increases if wearing Wizard's Helm

			if (DobjMonster != null && DobjMonster.Uid == 22)
			{
				var helmArtifact = gADB[25];

				Debug.Assert(helmArtifact != null);

				return helmArtifact.IsWornByCharacter();
			}
			else
			{
				return base.ShouldAllowSkillGains();
			}
		}
	}
}
