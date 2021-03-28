
// ReadCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Primitive.Enums;
using static BeginnersForest.Game.Plugin.PluginContext;

namespace BeginnersForest.Game.Commands
{
	[ClassMappings]
	public class ReadCommand : EamonRT.Game.Commands.ReadCommand, IReadCommand
	{
		public override void ProcessEvents(EventType eventType)
		{
			if (eventType == EventType.AfterArtifactRead)
			{
				// Scroll vanishes

				if (DobjArtifact.Uid == 3)
				{
					DobjArtifact.SetInLimbo();
				}
			}

			base.ProcessEvents(eventType);
		}

		public override void Execute()
		{
			Debug.Assert(DobjArtifact != null);

			// Gate at entrance/exit

			if (DobjArtifact.Uid == 19 || DobjArtifact.Uid == 20)
			{
				var command = Globals.CreateInstance<IExamineCommand>();

				CopyCommandData(command);

				NextState = command;
			}
			else
			{
				base.Execute();
			}
		}
	}
}
