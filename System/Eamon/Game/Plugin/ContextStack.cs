
// ContextStack.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Eamon.Framework.Plugin;
using static Eamon.Game.Plugin.Globals;

namespace Eamon.Game.Plugin
{
	public static class ContextStack
	{
		public static Stack<IEngine> EngineStack { get; set; } = new Stack<IEngine>();

		public static void PushEngine(Type engineType = null)
		{
			if (engineType == null)
			{
				engineType = typeof(Engine);
			}

			if (gEngine != null)
			{
				EngineStack.Push(gEngine);
			}

			gEngine = (IEngine)Activator.CreateInstance(engineType);

			Debug.Assert(gEngine != null);
		}

		public static void PopEngine()
		{
			gEngine = EngineStack.Count > 0 ? EngineStack.Pop() : null;
		}
	}
}
