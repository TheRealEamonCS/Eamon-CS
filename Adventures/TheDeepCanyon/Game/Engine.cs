
// Engine.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework;
using Eamon.Game.Attributes;
using static TheDeepCanyon.Game.Plugin.PluginContext;

namespace TheDeepCanyon.Game
{
	[ClassMappings(typeof(IEngine))]
	public class Engine : EamonRT.Game.Engine, EamonRT.Framework.IEngine
	{

	}
}
