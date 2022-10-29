
// BlastCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static AlternateBeginnersCave.Game.Plugin.Globals;

namespace AlternateBeginnersCave.Game.Commands
{
	[ClassMappings]
	public class BlastCommand : EamonRT.Game.Commands.BlastCommand, IBlastCommand
	{
		public override void ProcessEvents(EventType eventType)
		{
			base.ProcessEvents(eventType);

			// Illusionary wall

			if (eventType == EventType.BeforeAttackArtifact && DobjArtifact.Uid == 37)
			{
				PrintDontFollowYou();

				NextState = gEngine.CreateInstance<IStartState>();

				GotoCleanup = true;
			}
		}
	}
}
