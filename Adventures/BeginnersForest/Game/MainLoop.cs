
// MainLoop.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework;
using static BeginnersForest.Game.Plugin.PluginContext;

namespace BeginnersForest.Game
{
	[ClassMappings]
	public class MainLoop : EamonRT.Game.MainLoop, IMainLoop
	{
		public override void Startup()
		{
			base.Startup();

			// Entrance/exit gate rooms already seen

			var room1 = gRDB[1];

			Debug.Assert(room1 != null);

			room1.Seen = true;

			var room33 = gRDB[33];

			Debug.Assert(room33 != null);

			room33.Seen = true;
		}
	}
}
