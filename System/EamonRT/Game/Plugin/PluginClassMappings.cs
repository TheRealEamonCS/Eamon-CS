
// PluginClassMappings.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Reflection;
using Eamon;
using EamonRT.Framework.Plugin;

namespace EamonRT.Game.Plugin
{
	public class PluginClassMappings : EamonDD.Game.Plugin.PluginClassMappings, IPluginClassMappings
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
