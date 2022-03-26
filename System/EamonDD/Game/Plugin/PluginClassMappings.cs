
// PluginClassMappings.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Reflection;
using Eamon;
using EamonDD.Framework.Plugin;

namespace EamonDD.Game.Plugin
{
	public class PluginClassMappings : Eamon.Game.Plugin.PluginClassMappings, IPluginClassMappings
	{
		public override RetCode LoadPluginClassMappings()
		{
			RetCode rc;

			rc = base.LoadPluginClassMappings();

			if (rc != RetCode.Success)
			{
				goto Cleanup;
			}

#if !DEBUG
			if (RunGameEditor)
			{
				rc = LoadPluginClassMappings01(Assembly.GetExecutingAssembly());
			}
#else
			rc = LoadPluginClassMappings01(Assembly.GetExecutingAssembly());
#endif

		Cleanup:

			return rc;
		}
	}
}
