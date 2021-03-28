
// BlastCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static TheTrainingGround.Game.Plugin.PluginContext;

namespace TheTrainingGround.Game.Commands
{
	[ClassMappings]
	public class BlastCommand : EamonRT.Game.Commands.BlastCommand, IBlastCommand
	{
		public override void ProcessEvents(EventType eventType)
		{
			// BLAST Bozworth

			if (eventType == EventType.AfterPlayerSpellCastCheck && DobjMonster != null && DobjMonster.Uid == 20)
			{
				gEngine.PrintEffectDesc(21);

				DobjMonster.SetInLimbo();

				NextState = Globals.CreateInstance<IStartState>();

				GotoCleanup = true;
			}
			else
			{
				base.ProcessEvents(eventType);
			}
		}

		public override bool ShouldAllowSkillGains()
		{
			// BLASTing Bozworth never increases skill

			return DobjMonster != null && DobjMonster.Uid == 20 ? false : base.ShouldAllowSkillGains();
		}
	}
}
