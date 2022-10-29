
// DrinkCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Primitive.Enums;
using static TheVileGrimoireOfJaldial.Game.Plugin.Globals;

namespace TheVileGrimoireOfJaldial.Game.Commands
{
	[ClassMappings]
	public class DrinkCommand : EamonRT.Game.Commands.DrinkCommand, IDrinkCommand
	{
		public override void ProcessEvents(EventType eventType)
		{
			base.ProcessEvents(eventType);

			// The Molten Iron Ale is a critical resource in the final battle with Jaldi'al... but it also restores Hardiness drained by shadows.  What to do?

			if (eventType == EventType.AfterDrinkArtifact && DobjArtifact.Uid == 15 && gGameState.PlayerHardinessPointsDrained > 0)
			{
				gOut.Print("You suddenly feel stronger as the warm afterglow of the ale fades!");

				ActorMonster.Hardiness++;

				gGameState.PlayerHardinessPointsDrained--;
			}
		}
	}
}
