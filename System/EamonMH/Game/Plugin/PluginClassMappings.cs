
// PluginClassMappings.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Reflection;
using Eamon;
using EamonMH.Framework.Plugin;

namespace EamonMH.Game.Plugin
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

			rc = LoadPluginClassMappings01(Assembly.GetExecutingAssembly());

		Cleanup:

			return rc;
		}
	}
}
