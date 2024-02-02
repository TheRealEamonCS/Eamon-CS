
// ExamineCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using static TheBeginnersCave.Game.Plugin.Globals;

namespace TheBeginnersCave.Game.Commands
{
	[ClassMappings]
	public class ExamineCommand : EamonRT.Game.Commands.ExamineCommand, IExamineCommand
	{
		public override void ExecuteForPlayer()
		{
			Debug.Assert(DobjArtifact != null || DobjMonster != null);

			// Don't show bites/drinks left for spices

			if (DobjArtifact != null && DobjArtifact.Uid == 8)
			{
				var ac = DobjArtifact.Edible;

				Debug.Assert(ac != null);

				var field2 = ac.Field2;

				ac.Field2 = gEngine.InfiniteDrinkableEdible;

				base.ExecuteForPlayer();

				ac.Field2 = field2;
			}
			else
			{
				base.ExecuteForPlayer();
			}
		}
	}
}
