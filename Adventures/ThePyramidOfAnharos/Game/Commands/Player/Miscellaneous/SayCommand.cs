
// SayCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Primitive.Enums;
using static ThePyramidOfAnharos.Game.Plugin.Globals;

namespace ThePyramidOfAnharos.Game.Commands
{
	[ClassMappings]
	public class SayCommand : EamonRT.Game.Commands.SayCommand, ISayCommand
	{
		public override void ProcessEvents(EventType eventType)
		{
			base.ProcessEvents(eventType);

			if (eventType == EventType.AfterPrintSayText)
			{
				if (ProcessedPhrase.Equals("for anharos", StringComparison.OrdinalIgnoreCase))
				{
					if (gGameState.Ro == 6 || gGameState.Ro == 14)
					{
						gGameState.KE = gGameState.KE == 0 ? 1 : 0;

						gEngine.PrintEffectDesc(11 - gGameState.KE);
					}
					else if (gGameState.Ro == 12 || gGameState.Ro == 16)
					{
						gGameState.KF = gGameState.KF == 0 ? 1 : 0;

						gEngine.PrintEffectDesc(13 - gGameState.KF);
					}
				}
			}
		}
	}
}
