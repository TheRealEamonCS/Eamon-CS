
// UseCommand.cs

// Copyright (c) 2014+ by Kenneth Pedersen.  All rights reserved.

using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Primitive.Enums;
using static LandOfTheMountainKing.Game.Plugin.Globals;

namespace LandOfTheMountainKing.Game.Commands
{
	[ClassMappings]
	public class UseCommand : EamonRT.Game.Commands.UseCommand, IUseCommand
	{
		public override void ProcessEvents(EventType eventType)
		{
			base.ProcessEvents(eventType);

			if (eventType == EventType.BeforeUseArtifact)
			{
				switch (DobjArtifact.Uid)
				{
					case 7:

						// USE BUTTON

						gLMKKP1.Lampdir = gLMKKP1.Lampdir + 1;
						if (gLMKKP1.Lampdir == 9)
						{
							gLMKKP1.Lampdir = 1;
						}

						if (gLMKKP1.Lampdir == 1)
						{
							gEngine.PrintEffectDesc(8);
						}
						if (gLMKKP1.Lampdir == 2)
						{
							gEngine.PrintEffectDesc(9);							
						}
						if (gLMKKP1.Lampdir == 3)
						{
							gEngine.PrintEffectDesc(10);
							var room = gRDB[10];
							// this actually toggles the light level between light and dark
							// the EvalLightLevel method evaluates the room's current light level and returns darkValue if its dark, lightValue if its light
							room.LightLvl = room.EvalLightLevel(LightLevel.Light, LightLevel.Dark);
						}
						if (gLMKKP1.Lampdir == 4)
						{
							gEngine.PrintEffectDesc(11);
							var room = gRDB[10];
							// this actually toggles the light level between light and dark
							// the EvalLightLevel method evaluates the room's current light level and returns darkValue if its dark, lightValue if its light
							room.LightLvl = room.EvalLightLevel(LightLevel.Light, LightLevel.Dark);
						}
						if (gLMKKP1.Lampdir == 5)
						{
							gEngine.PrintEffectDesc(12);
						}
						if (gLMKKP1.Lampdir == 6)
						{
							gEngine.PrintEffectDesc(13);
						}
						if (gLMKKP1.Lampdir == 7)
						{
							gEngine.PrintEffectDesc(14);
						}
						if (gLMKKP1.Lampdir == 8)
						{
							gEngine.PrintEffectDesc(15);
						}
						GotoCleanup = true;
												
						break;
				}
			}
		}
	}
}
