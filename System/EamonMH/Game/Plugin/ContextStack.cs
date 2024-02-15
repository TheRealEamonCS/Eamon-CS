
// ContextStack.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;

namespace EamonMH.Game.Plugin
{
	/// <inheritdoc cref="Eamon.Game.Plugin.ContextStack"/>
	public static class ContextStack
	{
		/// <inheritdoc cref="Eamon.Game.Plugin.ContextStack.PushEngine(Type)"/>
		public static void PushEngine(Type engineType = null)
		{
			Eamon.Game.Plugin.ContextStack.PushEngine(engineType);
		}

		/// <inheritdoc cref="Eamon.Game.Plugin.ContextStack.PopEngine()"/>
		public static void PopEngine()
		{
			Eamon.Game.Plugin.ContextStack.PopEngine();
		}
	}
}
