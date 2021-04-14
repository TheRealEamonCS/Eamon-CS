
// MainLoop.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework;
using static AlternateBeginnersCave.Game.Plugin.PluginContext;

namespace AlternateBeginnersCave.Game
{
	[ClassMappings]
	public class MainLoop : EamonRT.Game.MainLoop, IMainLoop
	{
		public override void Shutdown()
		{
			base.Shutdown();

			// Nina's Special Purpose

			var ninaMonster = gMDB[11];

			Debug.Assert(ninaMonster != null);

			if (ninaMonster.Location == gGameState.Ro && ninaMonster.Reaction == Friendliness.Friend)
			{
				var reward = gCharacter.GetStats(Stat.Charisma) * 15;

				gCharacter.HeldGold += reward;

				gOut.Print("{0}", Globals.LineSep);

				gOut.Print("Nina sells her slippers to Sam and gives you {0} gold piece{1}!", reward, reward != 1 ? "s" : "");

				if (gCharacter.Gender == Gender.Male)
				{
					gOut.Print("She also gives you a kiss!");
				}

				Globals.In.KeyPress(Globals.Buf);
			}
		}
	}
}
