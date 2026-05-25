
// ApproachCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static TheBeginnersCave.Game.Plugin.Globals;

namespace TheBeginnersCave.Game.Commands
{
	[ClassMappings]
	public class ApproachCommand : EamonRT.Game.Commands.ApproachCommand, IApproachCommand
	{
		public override void ProcessEvents(EventType eventType)
		{
			base.ProcessEvents(eventType);

			if (eventType == EventType.BeforeApproach)
			{
				// Another classic Eamon moment...

				var mimicMonster = gMDB[7];

				Debug.Assert(mimicMonster != null);

				if (mimicMonster.IsInRoom(ActorRoom))
				{
					gEngine.PrintHeldFast(false);

					NextState = gEngine.CreateInstance<IMonsterStartState>();

					GotoCleanup = true;
				}
			}
		}
	}
}
