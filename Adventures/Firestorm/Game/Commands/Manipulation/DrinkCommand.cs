
// DrinkCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static Firestorm.Game.Plugin.Globals;

namespace Firestorm.Game.Commands
{
	[ClassMappings]
	public class DrinkCommand : EamonRT.Game.Commands.DrinkCommand, IDrinkCommand
	{
		public override void ProcessEvents(EventType eventType)
		{
			base.ProcessEvents(eventType);

			if (eventType == EventType.BeforeDrinkArtifact)
			{
				// Beer

				if (DobjArtifact.Uid == 22)
				{
					gOut.Print("Gulp...");

					GotoCleanup = true;
				}

				// Healing potion

				else if (DobjArtifact.Uid == 63)
				{
					gOut.Print("You can't use it!");

					NextState = gEngine.CreateInstance<IStartState>();

					GotoCleanup = true;
				}
			}
		}
	}
}
