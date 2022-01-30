
// IntroStory.cs

// Copyright (c) 2014+ by Kenneth Pedersen.  All rights reserved.

using Eamon.Game.Attributes;
using EamonRT.Framework;
using EamonRT.Framework.Primitive.Enums;
using static LandOfTheMountainKing.Game.Plugin.PluginContext;

namespace LandOfTheMountainKing.Game
{
	[ClassMappings]
	public class IntroStory : EamonRT.Game.IntroStory, IIntroStory
	{
		public override void PrintOutputDefault() //test
		{
			gOut.Print("{0}", Globals.LineSep);

			gEngine.PrintEffectDesc(1);
			gEngine.PrintEffectDesc(2);
			gEngine.PrintEffectDesc(3);
			gEngine.PrintEffectDesc(4);
			gEngine.PrintEffectDesc(5);
			gEngine.PrintEffectDesc(41);
			gEngine.PrintEffectDesc(6);
			gEngine.PrintEffectDesc(7);
		}
	}
}
