
// ReadyCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Primitive.Enums;
using static AlternateBeginnersCave.Game.Plugin.Globals;

namespace AlternateBeginnersCave.Game.Commands
{
	[ClassMappings]
	public class ReadyCommand : EamonRT.Game.Commands.ReadyCommand, IReadyCommand
	{
		public override void ProcessEvents(EventType eventType)
		{
			base.ProcessEvents(eventType);

			// Magic dagger

			if (eventType == EventType.AfterReadyArtifact && DobjArtifact.Uid == 8 && gGameState.MagicDaggerCounter == 0 && gGameState.DrankVial)
			{
				gGameState.MagicDaggerCounter = 35;

				gEngine.PrintEffectDesc(9);

				gCharMonster.Hardiness += 5;

				GotoCleanup = true;
			}
		}
	}
}
