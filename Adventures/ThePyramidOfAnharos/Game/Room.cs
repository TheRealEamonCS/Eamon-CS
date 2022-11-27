
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
		public override long GetDir(long index)
		{
			if (gEngine.EnableMutateProperties)
			{
				if (Uid == 6)
				{
					return gGameState.KE != 0 && index == 3 ? 14 : base.GetDir(index);
				}
				else if (Uid == 14)
				{
					return gGameState.KE != 0 && index == 4 ? 6 : base.GetDir(index);
				}
				else if (Uid == 12)
				{
					return gGameState.KF != 0 && index == 1 ? 16 : base.GetDir(index);
				}
				else if (Uid == 16)
				{
					return gGameState.KF != 0 && index == 2 ? 12 : base.GetDir(index);
				}
				else
				{
					return base.GetDir(index);
				}
			}
			else
			{
				return base.GetDir(index);
			}
		}

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
