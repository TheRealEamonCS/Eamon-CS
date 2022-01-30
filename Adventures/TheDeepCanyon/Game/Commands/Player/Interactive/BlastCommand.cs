
// BlastCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Primitive.Enums;
using static TheDeepCanyon.Game.Plugin.PluginContext;

namespace TheDeepCanyon.Game.Commands
{
	[ClassMappings]
	public class BlastCommand : EamonRT.Game.Commands.BlastCommand, IBlastCommand
	{
		public override void ProcessEvents(EventType eventType)
		{
			base.ProcessEvents(eventType);

			// BLAST elephants

			if (eventType == EventType.AfterCastSpellCheck && DobjMonster != null && DobjMonster.Uid == 24)
			{
				gOut.Print("The power of Elzod protects the elephants from your magical attack!");

				GotoCleanup = true;
			}
		}

		public override bool ShouldAllowSkillGains()
		{
			// BLASTing elephants never increases skill

			return DobjMonster != null && DobjMonster.Uid == 24 ? false : base.ShouldAllowSkillGains();
		}
	}
}
