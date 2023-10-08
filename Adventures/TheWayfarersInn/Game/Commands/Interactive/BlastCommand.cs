
// BlastCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using static TheWayfarersInn.Game.Plugin.Globals;

namespace TheWayfarersInn.Game.Commands
{
	[ClassMappings]
	public class BlastCommand : EamonRT.Game.Commands.BlastCommand, IBlastCommand
	{
		public override bool ShouldAllowSkillGains()
		{
			// BLASTing unseen apparition never increases skill

			return DobjMonster != null && DobjMonster.Uid == 2 ? false : base.ShouldAllowSkillGains();
		}
	}
}
