
// IntroStory.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework;
using EamonRT.Framework.Primitive.Enums;
using static TheBeginnersCave.Game.Plugin.PluginContext;

namespace TheBeginnersCave.Game
{
	[ClassMappings]
	public class IntroStory : EamonRT.Game.IntroStory, IIntroStory
	{
		public override void PrintOutputBeginnersPrelude()
		{
			gEngine.PrintEffectDesc(8);

			var room = gRDB[gEngine.StartRoom];

			Debug.Assert(room != null);

			gOut.Print("{0}", room.Desc);

			gEngine.PrintEffectDesc(10);
		}

		public override void PrintOutputBeginnersTooManyWeapons()
		{
			gEngine.PrintEffectDesc(13);
		}

		public override void PrintOutputBeginnersNoWeapons()
		{
			gEngine.PrintEffectDesc(9);
		}

		public override void PrintOutputBeginnersNotABeginner()
		{
			gEngine.PrintEffectDesc(11);
		}

		public override void PrintOutputBeginnersMayNowProceed()
		{
			gEngine.PrintEffectDesc(12);
		}

		public IntroStory()
		{
			StoryType = IntroStoryType.Beginners;
		}
	}
}
