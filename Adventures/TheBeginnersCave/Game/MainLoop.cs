﻿
// MainLoop.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework;
using static TheBeginnersCave.Game.Plugin.Globals;

namespace TheBeginnersCave.Game
{
	[ClassMappings]
	public class MainLoop : EamonRT.Game.MainLoop, IMainLoop
	{
		public override void Shutdown()
		{
			base.Shutdown();

			// Duke Luxom's reward

			var cynthiaMonster = gMDB[3];

			Debug.Assert(cynthiaMonster != null);

			if (cynthiaMonster.Location == gGameState.Ro && cynthiaMonster.Reaction > Friendliness.Enemy)
			{
				var reward = gCharacter.GetStat(Stat.Charisma) * 7;

				gCharacter.HeldGold += reward;

				gOut.Print("{0}", gEngine.LineSep);

				gOut.Print("Additionally, you receive {0} gold piece{1} for the safe return of Cynthia.", reward, reward != 1 ? "s" : "");

				gEngine.In.KeyPress(gEngine.Buf);
			}
		}
	}
}
