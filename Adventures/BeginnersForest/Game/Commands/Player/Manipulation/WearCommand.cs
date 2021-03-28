
// WearCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Primitive.Enums;
using static BeginnersForest.Game.Plugin.PluginContext;

namespace BeginnersForest.Game.Commands
{
	[ClassMappings]
	public class WearCommand : EamonRT.Game.Commands.WearCommand, IWearCommand
	{
		public override void ProcessEvents(EventType eventType)
		{
			// Magic Excercise Ring

			if (eventType == EventType.AfterArtifactWear && DobjArtifact.Uid == 2 && gGameState.Speed <= 0)
			{
				var command = Globals.CreateInstance<ISpeedCommand>(x =>
				{
					x.CastSpell = false;
				});

				CopyCommandData(command);

				NextState = command;

				GotoCleanup = true;
			}
			else
			{
				base.ProcessEvents(eventType);
			}
		}
	}
}
