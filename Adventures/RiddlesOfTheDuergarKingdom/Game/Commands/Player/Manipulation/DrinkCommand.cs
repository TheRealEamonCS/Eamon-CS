
// DrinkCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Primitive.Enums;
using static RiddlesOfTheDuergarKingdom.Game.Plugin.PluginContext;

namespace RiddlesOfTheDuergarKingdom.Game.Commands
{
	[ClassMappings]
	public class DrinkCommand : EamonRT.Game.Commands.DrinkCommand, IDrinkCommand
	{
		public override void ProcessEvents(EventType eventType)
		{
			base.ProcessEvents(eventType);

			// Coffee pot (super high-octane)

			if (eventType == EventType.AfterDrinkArtifact && DobjArtifact.Uid == 88)
			{
				gEngine.PrintEffectDesc(83);

				gGameState.CoffeePotUsed = true;

				var command = Globals.CreateInstance<ISpeedCommand>(x =>
				{
					x.CastSpell = false;
				});

				CopyCommandData(command);

				NextState = command;

				GotoCleanup = true;
			}
		}
	}
}
