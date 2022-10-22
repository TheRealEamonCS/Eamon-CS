
// Engine.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon;
using System.Reflection;
using static TheWayfarersInn.Game.Plugin.Globals;

namespace TheWayfarersInn.Game.Plugin
{
	public class Engine : EamonRT.Game.Plugin.Engine, Framework.Plugin.IEngine
	{
		public override RetCode LoadPluginClassMappings()
		{
			RetCode rc;

			rc = base.LoadPluginClassMappings();

			if (rc != RetCode.Success)
			{
				goto Cleanup;
			}

			rc = LoadPluginClassMappings01(Assembly.GetExecutingAssembly());

		Cleanup:

			return rc;
		}
	}
}
