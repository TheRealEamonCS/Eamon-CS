
// StateImpl.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using System.Linq;
using Eamon.Framework;
using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static TheWayfarersInn.Game.Plugin.Globals;

namespace TheWayfarersInn.Game.States
{
	[ClassMappings]
	public class StateImpl : EamonRT.Game.States.StateImpl, IStateImpl
	{
		public override void PrintObjBlocksTheWay(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			var artifactUids = new long[] { 7, 8, 9, 10, 20 };

			// Toolshed / Small Barn / Stable / Kennel / Temple

			if (artifactUids.Contains(artifact.Uid))
			{
				gOut.Print("{0} door blocks the way!", artifact.GetTheName(true));
			}
			else
			{
				base.PrintObjBlocksTheWay(artifact);
			}
		}

		public override void BeforePrintCommands()
		{
			if (!gGameState.MatureContent)
			{
				gOut.Print("To play this game you must opt-in by typing SET MATURECONTENT TRUE.");
			}

			base.BeforePrintCommands();
		}
	}
}
