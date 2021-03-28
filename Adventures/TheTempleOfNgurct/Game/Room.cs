
// Room.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework;
using Eamon.Game.Attributes;
using static TheTempleOfNgurct.Game.Plugin.PluginContext;

namespace TheTempleOfNgurct.Game
{
	[ClassMappings]
	public class Room : Eamon.Game.Room, IRoom
	{
		public override long GetDirs(long index)
		{
			if (Globals.EnableGameOverrides)
			{
				if (Uid == 33)
				{
					var oakDoorArtifact = gADB[85];

					var ac = oakDoorArtifact != null ? oakDoorArtifact.DoorGate : null;

					return ac != null && (ac.GetKeyUid() <= 0 || !oakDoorArtifact.Seen) && index == 1 ? 18 : base.GetDirs(index);
				}
				else if (Uid == 45)
				{
					var cellDoorArtifact = gADB[87];

					var ac = cellDoorArtifact != null ? cellDoorArtifact.DoorGate : null;

					return ac != null && ac.GetKeyUid() <= 0 && index == 4 ? 26 : base.GetDirs(index);
				}
				else if (Uid == 46)
				{
					var cellDoorArtifact = gADB[88];

					var ac = cellDoorArtifact != null ? cellDoorArtifact.DoorGate : null;

					return ac != null && ac.GetKeyUid() <= 0 && index == 3 ? 27 : base.GetDirs(index);
				}
				else if (Uid == 55)
				{
					var cellDoorArtifact = gADB[86];

					var ac = cellDoorArtifact != null ? cellDoorArtifact.DoorGate : null;

					return ac != null && ac.GetKeyUid() <= 0 && index == 1 ? 56 : base.GetDirs(index);
				}
				else
				{
					return base.GetDirs(index);
				}
			}
			else
			{
				return base.GetDirs(index);
			}
		}
	}
}
