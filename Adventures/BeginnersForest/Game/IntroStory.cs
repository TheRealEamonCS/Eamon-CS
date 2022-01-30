
// IntroStory.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Game.Attributes;
using EamonRT.Framework;
using EamonRT.Framework.Primitive.Enums;
using static BeginnersForest.Game.Plugin.PluginContext;

namespace BeginnersForest.Game
{
	[ClassMappings]
	public class IntroStory : EamonRT.Game.IntroStory, IIntroStory
	{
		public override void PrintOutputBeginnersPrelude()
		{
			gEngine.PrintEffectDesc(9);

			gEngine.PrintEffectDesc(11);
		}

		public override void PrintOutputBeginnersTooManyWeapons()
		{
			gEngine.PrintEffectDesc(16);
		}

		public override void PrintOutputBeginnersNoWeapons()
		{
			gEngine.PrintEffectDesc(12);
		}

		public override void PrintOutputBeginnersNotABeginner()
		{
			gEngine.PrintEffectDesc(13);
		}

		public override void PrintOutputBeginnersMayNowProceed()
		{
			gEngine.PrintEffectDesc(15);
		}

		public IntroStory()
		{
			StoryType = IntroStoryType.Beginners;
		}
	}
}
