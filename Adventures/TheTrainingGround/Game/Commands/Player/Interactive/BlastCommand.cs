
// BlastCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static TheTrainingGround.Game.Plugin.PluginContext;

namespace TheTrainingGround.Game.Commands
{
	[ClassMappings]
	public class BlastCommand : EamonRT.Game.Commands.BlastCommand, IBlastCommand
	{
		public override void Execute()
		{
			Debug.Assert(DobjArtifact != null || DobjMonster != null);

			// BLAST backpack

			if (gCharacter.GetSpellAbilities(Spell.Blast) > 0 && DobjArtifact != null && DobjArtifact.Uid == 13)
			{
				PrintDontNeedTo();

				NextState = Globals.CreateInstance<IStartState>();
			}
			else
			{
				base.Execute();
			}
		}

		public override bool ShouldAllowSkillGains()
		{
			// BLASTing Bozworth never increases skill

			return DobjMonster != null && DobjMonster.Uid == 20 ? false : base.ShouldAllowSkillGains();
		}
	}
}
