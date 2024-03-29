﻿
// Room.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Text;
using Eamon;
using Eamon.Framework;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using static TheDeepCanyon.Game.Plugin.Globals;

namespace TheDeepCanyon.Game
{
	[ClassMappings]
	public class Room : Eamon.Game.Room, IRoom
	{
		public override long GetDir(long index)
		{
			if (gEngine.EnableMutateProperties)
			{
				if (Uid == 21)
				{
					var elephantsMonster = gMDB[24];

					return elephantsMonster != null && elephantsMonster.IsInLimbo() && index == 4 ? 45 : base.GetDir(index);
				}
				if (Uid == 26)
				{
					var fidoMonster = gMDB[11];

					return (gGameState.FidoSleepCounter > 0 || (fidoMonster != null && fidoMonster.IsInLimbo())) && index == 2 ? 25 : base.GetDir(index);
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
			// Suppress various "invisible" exits

			return base.IsDirectionInObviousExitsList(index) && ((Uid != 8 && Uid != 22) || (Uid == 8 && index != 4) || (Uid == 22 && index != 1));
		}

		public override RetCode BuildPrintedTooDarkToSeeDesc(StringBuilder buf)
		{
			RetCode rc;

			if (buf == null)
			{
				rc = RetCode.InvalidArg;

				// PrintError

				goto Cleanup;
			}

			rc = RetCode.Success;

			buf.SetPrint("It's too dark to see anything.");

		Cleanup:

			return rc;
		}
	}
}
