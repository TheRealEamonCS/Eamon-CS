
// AttackCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static OrbOfMyLife.Game.Plugin.Globals;

namespace OrbOfMyLife.Game.Commands
{
	[ClassMappings]
	public class AttackCommand : EamonRT.Game.Commands.AttackCommand, IAttackCommand
	{
		public override void ProcessEvents(EventType eventType)
		{
			base.ProcessEvents(eventType);

			// Gate of Light

			if (eventType == EventType.BeforeAttackArtifact && DobjArtifact.Uid == 13)
			{
				PrintDontFollowYou();

				NextState = gEngine.CreateInstance<IStartState>();

				GotoCleanup = true;
			}
		}
	}
}
