
// MainLoop.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using System.Linq;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework;
using static TheSubAquanLaboratory.Game.Plugin.Globals;

namespace TheSubAquanLaboratory.Game
{
	[ClassMappings]
	public class MainLoop : EamonRT.Game.MainLoop, IMainLoop
	{
		public override void Shutdown()
		{
			// End of game specials

			gOut.Print("{0}", gEngine.LineSep);

			gEngine.PrintEffectDesc(71);

			gEngine.In.KeyPress(gEngine.Buf);

			// Calculate number of lab rooms explored

			var rooms = gEngine.Database.RoomTable.Records.Where(r => r.Zone == 2).ToList();

			var seenCount = rooms.Count(r => r.Seen);

			gGameState.QuestValue += (long)Math.Round(((double)seenCount / (double)rooms.Count) * 350);

			// 100% of quest = base reward of 1250

			gOut.Print("{0}", gEngine.LineSep);

			if (gGameState.QuestValue > 0)
			{
				var reward = (long)Math.Round((double)gGameState.QuestValue * ((double)gCharacter.GetStat(Stat.Charisma) / (double)10));

				if (reward > 3000)
				{
					reward = 3000;
				}

				gCharacter.HeldGold += reward;

				gEngine.PrintEffectDesc(78);

				gOut.Print("The mayor then calculates the value of the information you have presented him with and pays you {0} gold piece{1}.", reward, reward != 1 ? "s" : "");
			}
			else
			{
				gEngine.PrintEffectDesc(76);
			}

			gEngine.In.KeyPress(gEngine.Buf);

			base.Shutdown();
		}
	}
}
