
// ContextStack.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;

namespace EamonRT.Game.Plugin
{
    /// <inheritdoc cref="EamonDD.Game.Plugin.ContextStack"/>
    public static class ContextStack
	{
        /// <inheritdoc cref="EamonDD.Game.Plugin.ContextStack.PushEngine(Type)"/>
		public static void PushEngine(Type engineType = null)
		{
			EamonDD.Game.Plugin.ContextStack.PushEngine(engineType);
		}

        /// <inheritdoc cref="EamonDD.Game.Plugin.ContextStack.PopEngine()"/>
		public static void PopEngine()
		{
			EamonDD.Game.Plugin.ContextStack.PopEngine();
		}
	}
}
