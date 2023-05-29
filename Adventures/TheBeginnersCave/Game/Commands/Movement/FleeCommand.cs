
// FleeCommand.cs

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
	public class FleeCommand : EamonRT.Game.Commands.FleeCommand, IFleeCommand
	{
		public override void PrintCalmDown()
		{
			gOut.Print("What are you fleeing from?");
		}

		public override void PrintNoPlaceToGo()
		{
			gOut.Print("There's no place to run!");
		}

		public override void ProcessEvents(EventType eventType)
		{
			base.ProcessEvents(eventType);

			if (eventType == EventType.AfterNumberOfExitsCheck)
			{
				// another classic Eamon moment...

				var mimicMonster = gMDB[7];

				Debug.Assert(mimicMonster != null);

				if (mimicMonster.IsInRoom(ActorRoom))
				{
					gOut.Print("You are held fast by the mimic and cannot flee!");

					NextState = gEngine.CreateInstance<IMonsterStartState>();

					GotoCleanup = true;
				}
			}
		}
	}
}
