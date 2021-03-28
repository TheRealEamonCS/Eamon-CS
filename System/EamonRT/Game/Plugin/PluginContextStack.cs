
// PluginContextStack.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;

namespace EamonRT.Game.Plugin
{
	public static class PluginContextStack
	{
		public static void PushConstants(Type constantsType = null)
		{
			EamonDD.Game.Plugin.PluginContextStack.PushConstants(constantsType);
		}

		public static void PushClassMappings(Type classMappingsType = null)
		{
			EamonDD.Game.Plugin.PluginContextStack.PushClassMappings(classMappingsType);
		}

		public static void PushGlobals(Type globalsType = null)
		{
			EamonDD.Game.Plugin.PluginContextStack.PushGlobals(globalsType);
		}

		public static void PopConstants()
		{
			EamonDD.Game.Plugin.PluginContextStack.PopConstants();
		}

		public static void PopClassMappings()
		{
			EamonDD.Game.Plugin.PluginContextStack.PopClassMappings();
		}

		public static void PopGlobals()
		{
			EamonDD.Game.Plugin.PluginContextStack.PopGlobals();
		}
	}
}
