
// Room.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework;
using Eamon.Game.Attributes;
using static TheSubAquanLaboratory.Game.Plugin.Globals;

namespace TheSubAquanLaboratory.Game
{
	[ClassMappings]
	public class Room : Eamon.Game.Room, IRoom
	{
		public override long GetDir(long index)
		{
			if (gEngine.EnableMutateProperties)
			{
				if (Uid == 2)
				{
					var backWallArtifact = gADB[83];

					return backWallArtifact != null && backWallArtifact.IsInLimbo() && index == 1 ? 17 : base.GetDir(index);
				}
				else if (Uid == 10)
				{
					return gGameState.Flood != 0 && (index == 5 || index == 7 || index == 8) ? -20 : base.GetDir(index);
				}
				else if (Uid == 43)
				{
					var ovalDoorArtifact = gADB[16];

					return ovalDoorArtifact != null && ovalDoorArtifact.IsInLimbo() && index == 4 ? 9 : base.GetDir(index);
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
	}
}
