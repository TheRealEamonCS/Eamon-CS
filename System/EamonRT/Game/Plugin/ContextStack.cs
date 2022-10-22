
// ContextStack.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;

namespace EamonRT.Game.Plugin
{
	public static class ContextStack
	{
		public static void PushEngine(Type engineType = null)
		{
			EamonDD.Game.Plugin.ContextStack.PushEngine(engineType);
		}

		public static void PopEngine()
		{
			EamonDD.Game.Plugin.ContextStack.PopEngine();
		}
	}
}
