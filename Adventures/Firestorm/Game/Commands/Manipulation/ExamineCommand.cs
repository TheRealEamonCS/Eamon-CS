
// ExamineCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Primitive.Enums;
using static Firestorm.Game.Plugin.Globals;

namespace Firestorm.Game.Commands
{
	[ClassMappings]
	public class ExamineCommand : EamonRT.Game.Commands.ExamineCommand, IExamineCommand
	{
		public override void ProcessEvents(EventType eventType)
		{
			base.ProcessEvents(eventType);

			if (eventType == EventType.AfterPrintArtifactFullDesc)
			{
				// Pebbles

				if (DobjArtifact.Uid == 40)
				{
					gEngine.PrintPebblesLeft(DobjArtifact);
				}

				// Healing herbs

				else if (DobjArtifact.Uid == 41)
				{
					gEngine.PrintHealingHerbsLeft(DobjArtifact);
				}
			}
		}
	}
}
