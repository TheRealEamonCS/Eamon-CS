
// PluginClassMappings.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Reflection;
using Eamon;

namespace TheTempleOfNgurct.Game.Plugin
{
	public class PluginClassMappings : EamonRT.Game.Plugin.PluginClassMappings, Framework.Plugin.IPluginClassMappings
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

		public PluginClassMappings()
		{
			RulesetVersion = 5;
		}
	}
}
