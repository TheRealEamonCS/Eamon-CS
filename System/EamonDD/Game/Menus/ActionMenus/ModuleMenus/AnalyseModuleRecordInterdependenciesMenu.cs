
// AnalyseModuleRecordInterdependenciesMenu.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework;
using Eamon.Framework.Helpers;
using Eamon.Game.Attributes;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.PluginContext;

namespace EamonDD.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class AnalyseModuleRecordInterdependenciesMenu : AnalyseRecordInterdependenciesMenu<IModule, IModuleHelper>, IAnalyseModuleRecordInterdependenciesMenu
	{
		public AnalyseModuleRecordInterdependenciesMenu()
		{
			Title = "ANALYSE MODULE RECORDS";

			RecordTable = Globals.Database.ModuleTable;

			RecordTypeName = "Module";
		}
	}
}
