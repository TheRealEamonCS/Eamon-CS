
// AnalyseModuleRecordInterdependenciesMenu01.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework;
using Eamon.Game.Attributes;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.PluginContext;

namespace EamonDD.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class AnalyseModuleRecordInterdependenciesMenu01 : AnalyseRecordInterdependenciesMenu01<IModule>, IAnalyseModuleRecordInterdependenciesMenu01
	{
		public AnalyseModuleRecordInterdependenciesMenu01()
		{
			AnalyseMenu = Globals.CreateInstance<IAnalyseModuleRecordInterdependenciesMenu>();
		}
	}
}
