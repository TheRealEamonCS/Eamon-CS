
// ExamineCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Primitive.Enums;
using static RiddlesOfTheDuergarKingdom.Game.Plugin.Globals;

namespace RiddlesOfTheDuergarKingdom.Game.Commands
{
	[ClassMappings]
	public class ExamineCommand : EamonRT.Game.Commands.ExamineCommand, IExamineCommand
	{
		public override void ProcessEvents(EventType eventType)
		{
			base.ProcessEvents(eventType);

			if (eventType == EventType.AfterExamineMonsterHealthStatus)
			{
				if (gGameState.PoisonedTargets.ContainsKey(DobjMonster.Uid))
				{
					gOut.Print("{0} is poisoned at this time!", DobjMonster.GetTheName(true, true, false, false, true));
				}
			}
		}
	}
}
