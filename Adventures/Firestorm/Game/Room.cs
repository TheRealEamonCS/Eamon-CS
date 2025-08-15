
// Room.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework;
using Eamon.Game.Attributes;
using static Firestorm.Game.Plugin.Globals;

namespace Firestorm.Game
{
	[ClassMappings]
	public class Room : Eamon.Game.Room, IRoom
	{
		public override long GetDir(long index)
		{
			if (gEngine.EnableMutateProperties)
			{
				// Combat zone

				if (Uid == 86)
				{
					var thorakMonster = gMDB[27];

					return thorakMonster != null && !thorakMonster.IsInLimbo() && index == 2 ? 0 : base.GetDir(index);
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
			// Dungeon pitfall

			return Uid != 68 || index != 2 ? base.IsDirectionInObviousExitsList(index) : true;
		}
	}
}
