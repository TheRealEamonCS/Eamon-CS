
// IntroStory.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using System.Text;
using Eamon;
using Eamon.Game.Attributes;
using EamonRT.Framework;
using static ThePyramidOfAnharos.Game.Plugin.Globals;

namespace ThePyramidOfAnharos.Game
{
	[ClassMappings]
	public class IntroStory : EamonRT.Game.IntroStory, IIntroStory
	{
		public override void PrintOutputDefault()
		{
			RetCode rc;

			gOut.Print("{0}", gEngine.LineSep);

			var buf = new StringBuilder(gEngine.BufSize);

			var effectUids = new long[] { 200, 203, 206, 209, 212 };

			for (var i = 0; i < effectUids.Length; i++)
			{
				var effectUid = effectUids[i];

				var effect = gEDB[effectUid];

				buf.Clear();

				rc = gEngine.ResolveUidMacros(effect.Desc, buf, true, true);

				Debug.Assert(gEngine.IsSuccess(rc));

				if (i > 0)
				{
					gOut.WriteLine("{0}{1}{0}", Environment.NewLine, gEngine.LineSep);
				}

				gOut.Print("{0}", buf);

				if (effectUid == 209)
				{
					gOut.WriteLine("{0}{1}{0}", Environment.NewLine, gEngine.LineSep);

					gOut.Write("{0}Do you want a Good, an Average, or No guide (G/A/N): ", Environment.NewLine);

					buf.Clear();

					rc = gEngine.In.ReadField(buf, gEngine.BufSize02, null, ' ', '\0', false, null, gEngine.ModifyCharToUpper, gEngine.IsCharGOrAOrN, null);

					Debug.Assert(gEngine.IsSuccess(rc));

					gEngine.Thread.Sleep(150);

					gOut.WriteLine("{0}{1}{0}", Environment.NewLine, gEngine.LineSep);

					if (buf.Length > 0 && buf[0] == 'G')
					{
						gGameState.GU = 1;

						gOut.Print("You are able to secure the services of Omar, a veteran caravan guide.");
					}
					else if (buf.Length > 0 && buf[0] == 'A')
					{
						gGameState.GU = 2;

						gOut.Print("You find Ali, a camel tender, who claims to be from the desert.");
					}
					else
					{
						gGameState.GU = 0;

						gOut.Print("Okay, you're on your own.");
					}

					gEngine.PrintEffectDesc(211);

					gOut.WriteLine();

					gEngine.In.KeyPress(buf);
				}
				else if (i < effectUids.Length - 1)
				{
					gOut.WriteLine();

					gEngine.In.KeyPress(buf);
				}
			}
		}
	}
}
