
// ContextStack.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;

namespace EamonDD.Game.Plugin
{
	public static class ContextStack
	{
		public static void PushEngine(Type engineType = null)
		{
			Eamon.Game.Plugin.ContextStack.PushEngine(engineType);
		}

		public static void PopEngine()
		{
			Eamon.Game.Plugin.ContextStack.PopEngine();
		}
	}
}
