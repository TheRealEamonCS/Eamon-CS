
// BlastCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using static StrongholdOfKahrDur.Game.Plugin.Globals;

namespace StrongholdOfKahrDur.Game.Commands
{
	[ClassMappings]
	public class BlastCommand : EamonRT.Game.Commands.BlastCommand, IBlastCommand
	{
		public override bool ShouldAllowSkillGains()
		{
			var helmArtifact = gADB[25];

			Debug.Assert(helmArtifact != null);

			// When Necromancer is BLASTed only allow skill increases if wearing Wizard's Helm

			return DobjMonster != null && DobjMonster.Uid == 22 && !helmArtifact.IsWornByMonster(ActorMonster) ? false : base.ShouldAllowSkillGains();
		}
	}
}
