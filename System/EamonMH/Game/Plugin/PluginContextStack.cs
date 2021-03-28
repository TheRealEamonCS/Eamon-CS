
// PluginContextStack.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;

namespace EamonMH.Game.Plugin
{
	public static class PluginContextStack
	{
		public static void PushConstants(Type constantsType = null)
		{
			Eamon.Game.Plugin.PluginContextStack.PushConstants(constantsType);
		}

		public static void PushClassMappings(Type classMappingsType = null)
		{
			Eamon.Game.Plugin.PluginContextStack.PushClassMappings(classMappingsType);
		}

		public static void PushGlobals(Type globalsType = null)
		{
			Eamon.Game.Plugin.PluginContextStack.PushGlobals(globalsType);
		}

		public static void PopConstants()
		{
			Eamon.Game.Plugin.PluginContextStack.PopConstants();
		}

		public static void PopClassMappings()
		{
			Eamon.Game.Plugin.PluginContextStack.PopClassMappings();
		}

		public static void PopGlobals()
		{
			Eamon.Game.Plugin.PluginContextStack.PopGlobals();
		}
	}
}
