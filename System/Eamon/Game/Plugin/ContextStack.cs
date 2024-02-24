
// ContextStack.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Eamon.Framework.Plugin;
using static Eamon.Game.Plugin.Globals;

namespace Eamon.Game.Plugin
{
	/// <summary>
	/// Manages the plugin context stack in the Eamon CS game system.
	/// </summary>
	/// <remarks>
	/// This class is designed to handle a dynamic stack of <see cref="IEngine">Engine</see> instances. When the
	/// program call stack unwinds, Engines are pushed onto this stack in reverse order. The Engine for the currently
	/// executing plugin is always positioned at the top of the stack, providing critical functionality. To optimize
	/// accessibility, the stack top is referenced by the <see cref="Globals.gEngine">Globals.gEngine</see> property,
	/// instead of being stored within the stack itself.
	/// 
	/// When the current plugin's execution completes, its corresponding Engine is popped off the top of the stack
	/// and discarded. This ensures the previously executed plugin's Engine is readily accessible for subsequent
	/// operations.
	/// </remarks>
	public static class ContextStack
	{
		/// <summary>The stack of <see cref="IEngine">Engine</see> instances for currently executing plugins.</summary>
		/// <remarks>This is loaded in reverse order, in a manner similar to the loading of the
		/// <see cref="IEngine.ClassMappingsDictionary">ClassMappingsDictionary</see>. During plugin startup the program
		/// drills to the bottom of the system. Then, as the call stack unwinds, Engines are pushed on at each level, 
		/// ensuring the currently executing plugin always has access to its specific Engine instance.
		/// </remarks>
		public static Stack<IEngine> EngineStack { get; set; } = new Stack<IEngine>();

		/// <summary>Push a new <see cref="IEngine">Engine</see> on the stack top.</summary>
		/// <param name="engineType">The type of Engine to instantiate.</param>
		/// <remarks>This typically happens when a new plugin is initialized. If the <paramref name="engineType"/> is null, 
		/// a default is used.</remarks>
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

		/// <summary>Pop the current <see cref="IEngine">Engine</see> off the stack top.</summary>
		/// <remarks>
		/// The Engine now on the stack top will be referenced by <see cref="Globals.gEngine">Globals.gEngine</see>. If the stack
		/// is empty, it will be null.
		/// </remarks>
		public static void PopEngine()
		{
			gEngine = EngineStack.Count > 0 ? EngineStack.Pop() : null;
		}
	}
}
