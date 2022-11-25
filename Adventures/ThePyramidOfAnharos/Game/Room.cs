
// Room.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon;
using Eamon.Framework;
using Eamon.Game.Attributes;
using System;
using System.Text;
using static ThePyramidOfAnharos.Game.Plugin.Globals;

namespace ThePyramidOfAnharos.Game
{
	[ClassMappings]
	public class Room : Eamon.Game.Room, IRoom
	{
		public override bool IsDirectionInObviousExitsList(long index)
		{
			// Suppress up/down on stone ramp

			return base.IsDirectionInObviousExitsList(index) && (Uid == 16 ? index != 6 : Uid == 19 || Uid == 20 ? index != 5 && index != 6 : Uid == 21 ? index != 5 : true);
		}

		public override string GetObviousExits()
		{
			// No obvious exits in desert

			return Zone != 1 ? base.GetObviousExits() : "";
		}

		public override RetCode GetExitList(StringBuilder buf, Func<string, string> modFunc = null, bool useNames = true)
		{
			// No obvious exits in desert

			return Zone != 1 ? base.GetExitList(buf, modFunc, useNames) : RetCode.Success;
		}
	}
}
