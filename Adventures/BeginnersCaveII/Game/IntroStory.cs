
// IntroStory.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework;
using EamonRT.Framework.Primitive.Enums;
using static BeginnersCaveII.Game.Plugin.PluginContext;

namespace BeginnersCaveII.Game
{
	[ClassMappings]
	public class IntroStory : EamonRT.Game.IntroStory, IIntroStory
	{
		public override void PrintOutputBeginnersPrelude()
		{
			gEngine.PrintEffectDesc(3);

			var room = gRDB[gEngine.StartRoom];

			Debug.Assert(room != null);

			gOut.Print("{0}", room.Desc);

			gEngine.PrintEffectDesc(5);
		}

		public override void PrintOutputBeginnersTooManyWeapons()
		{
			gEngine.PrintEffectDesc(8);
		}

		public override void PrintOutputBeginnersNoWeapons()
		{
			gEngine.PrintEffectDesc(4);
		}

		public override void PrintOutputBeginnersNotABeginner()
		{
			gEngine.PrintEffectDesc(6);
		}

		public override void PrintOutputBeginnersMayNowProceed()
		{
			gEngine.PrintEffectDesc(7);
		}

		public IntroStory()
		{
			StoryType = IntroStoryType.Beginners;
		}
	}
}
