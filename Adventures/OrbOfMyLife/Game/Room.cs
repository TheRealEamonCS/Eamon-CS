
// Room.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Linq;
using System.Text;
using Eamon;
using Eamon.Framework;
using Eamon.Framework.Args;
using Eamon.Game.Attributes;
using static OrbOfMyLife.Game.Plugin.Globals;

namespace OrbOfMyLife.Game
{
	[ClassMappings(typeof(IRoom))]
	public class Room : Eamon.Game.Room, Framework.IRoom
	{
		public override bool IsViewable()
		{
			return base.IsViewable() && (!gGameState.IC || IsDreamDimensionRoom());
		}

		public override bool IsDirectionInObviousExitsList(long index)
		{
			var eastRoomUids = new long[] { 49 };

			// Suppress various "invisible" exits

			return !(eastRoomUids.Contains(Uid) && index == 3) ? base.IsDirectionInObviousExitsList(index) : false;
		}

		public override string GetObviousExits()
		{
			var directionRoomUids = new long[] { 15, 16, 17, 18, 19, 20, 21, 43 };

			// Modify "Obvious exits" string

			return directionRoomUids.Contains(Uid) ? string.Format("{0}Obvious directions:  ", Environment.NewLine) : Uid != 57 ? base.GetObviousExits() : "";
		}

		public override RetCode GetExitList(StringBuilder buf, Func<string, string> modFunc = null, bool useNames = true)
		{
			RetCode rc;

			rc = Uid != 57 ? base.GetExitList(buf, modFunc, useNames) : RetCode.Success;

			buf.Replace("north, south, east, west, up, down, northeast, northwest, southeast, and southwest", "everywhere");

			if (gEngine.VerboseRoomDescOrNotSeen)
			{
				var effectUid = Uid == 13 ? 7 : Uid == 34 ? 32 : Uid == 50 ? 15 : 0;

				var effect = effectUid != 0 ? gEDB[effectUid] : null;

				if (effect != null)
				{
					buf.AppendFormat("{0}{0}{1}", Environment.NewLine, effect.Desc);
				}
			}

			return rc;
		}

		public override RetCode BuildPrintedFullDesc(StringBuilder buf, Func<IMonster, bool> monsterFindFunc = null, Func<IArtifact, bool> artifactFindFunc = null, bool verboseRoomDesc = false, bool verboseMonsterDesc = false, bool verboseArtifactDesc = false, bool verboseNames = false, IRecordNameListArgs recordNameListArgs = null)
		{
			RetCode rc;

			gEngine.VerboseRoomDescOrNotSeen = verboseRoomDesc || Seen == false;

			rc = base.BuildPrintedFullDesc(buf, monsterFindFunc, artifactFindFunc, verboseRoomDesc, verboseMonsterDesc, verboseArtifactDesc, verboseNames, recordNameListArgs);

			gEngine.VerboseRoomDescOrNotSeen = false;

			return rc;
		}

		public virtual bool IsDreamDimensionRoom()
		{
			return Zone == 2;
		}
	}
}
