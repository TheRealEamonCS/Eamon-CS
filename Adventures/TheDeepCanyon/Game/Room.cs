
// Room.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Text;
using Eamon;
using Eamon.Framework;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using static TheDeepCanyon.Game.Plugin.PluginContext;

namespace TheDeepCanyon.Game
{
	[ClassMappings]
	public class Room : Eamon.Game.Room, IRoom
	{
		public override bool IsDirectionInObviousExitsList(long index)
		{
			// Suppress west exit in Falconer's camp

			return base.IsDirectionInObviousExitsList(index) && (Uid != 8 || index != 4);
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
