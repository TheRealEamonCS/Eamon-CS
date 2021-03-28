
// PluginContextStack.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Eamon.Framework.Plugin;
using static Eamon.Game.Plugin.PluginContext;

namespace Eamon.Game.Plugin
{
	public static class PluginContextStack
	{
		private static Stack<IPluginConstants> PluginConstantsStack { get; set; } = new Stack<IPluginConstants>();

		private static Stack<IPluginClassMappings> PluginClassMappingsStack { get; set; } = new Stack<IPluginClassMappings>();

		private static Stack<IPluginGlobals> PluginGlobalsStack { get; set; } = new Stack<IPluginGlobals>();

		public static void PushConstants(Type constantsType = null)
		{
			if (constantsType == null)
			{
				constantsType = typeof(PluginConstants);
			}

			if (Constants != null)
			{
				PluginConstantsStack.Push(Constants);
			}

			Constants = (IPluginConstants)Activator.CreateInstance(constantsType);

			Debug.Assert(Constants != null);
		}

		public static void PushClassMappings(Type classMappingsType = null)
		{
			if (classMappingsType == null)
			{
				classMappingsType = typeof(PluginClassMappings);
			}

			if (ClassMappings != null)
			{
				PluginClassMappingsStack.Push(ClassMappings);
			}

			ClassMappings = (IPluginClassMappings)Activator.CreateInstance(classMappingsType);

			Debug.Assert(ClassMappings != null);
		}

		public static void PushGlobals(Type globalsType = null)
		{
			if (globalsType == null)
			{
				globalsType = typeof(PluginGlobals);
			}

			if (Globals != null)
			{
				PluginGlobalsStack.Push(Globals);
			}

			Globals = (IPluginGlobals)Activator.CreateInstance(globalsType);

			Debug.Assert(Globals != null);
		}

		public static void PopConstants()
		{
			Constants = PluginConstantsStack.Count > 0 ? PluginConstantsStack.Pop() : null;
		}

		public static void PopClassMappings()
		{
			ClassMappings = PluginClassMappingsStack.Count > 0 ? PluginClassMappingsStack.Pop() : null;
		}

		public static void PopGlobals()
		{
			Globals = PluginGlobalsStack.Count > 0 ? PluginGlobalsStack.Pop() : null;
		}
	}
}
