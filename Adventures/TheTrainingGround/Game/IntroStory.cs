﻿
// IntroStory.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework;
using static TheTrainingGround.Game.Plugin.Globals;

namespace TheTrainingGround.Game
{
	[ClassMappings]
	public class IntroStory : EamonRT.Game.IntroStory, IIntroStory
	{
		public override void PrintOutput()
		{
			base.PrintOutput();

			gEngine.In.KeyPress(Buf);

			gOut.Print("{0}", gEngine.LineSep);

			// Inspect the player

			gEngine.PrintEffectDesc(17);

			if (gCharacter.ArmorExpertise > 25)
			{
				gEngine.PrintEffectDesc(18);

				gEngine.MainLoop.ShouldExecute = false;

				gEngine.ExitType = ExitType.GoToMainHall;
			}
			else
			{
				if (gCharacter.ArmorExpertise == 0)
				{
					gEngine.PrintEffectDesc(19);
				}

				gOut.Print("\"OK, let's be careful in there{0}!\" he says, as he walks away.", gCharacter.EvalGender(", son", ", miss", ""));
			}
		}
	}
}
