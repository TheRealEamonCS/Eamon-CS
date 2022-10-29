
// IntroStory.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework;
using EamonRT.Framework.Primitive.Enums;
using static AlternateBeginnersCave.Game.Plugin.Globals;

namespace AlternateBeginnersCave.Game
{
	[ClassMappings]
	public class IntroStory : EamonRT.Game.IntroStory, IIntroStory
	{
		public override void PrintOutputBeginnersPrelude()
		{
			gEngine.PrintEffectDesc(14);

			var room = gRDB[gEngine.StartRoom];

			Debug.Assert(room != null);

			gOut.Print("{0}", room.Desc);

			gEngine.PrintEffectDesc(16);
		}

		public override void PrintOutputBeginnersTooManyWeapons()
		{
			gEngine.PrintEffectDesc(19);
		}

		public override void PrintOutputBeginnersNoWeapons()
		{
			gEngine.PrintEffectDesc(15);
		}

		public override void PrintOutputBeginnersNotABeginner()
		{
			gEngine.PrintEffectDesc(17);
		}

		public override void PrintOutputBeginnersMayNowProceed()
		{
			gEngine.PrintEffectDesc(18);
		}

		public IntroStory()
		{
			StoryType = IntroStoryType.Beginners;
		}
	}
}
