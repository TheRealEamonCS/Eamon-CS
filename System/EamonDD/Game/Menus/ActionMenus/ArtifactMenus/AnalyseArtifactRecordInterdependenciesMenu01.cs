
// AnalyseArtifactRecordInterdependenciesMenu01.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework;
using Eamon.Game.Attributes;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.PluginContext;

namespace EamonDD.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class AnalyseArtifactRecordInterdependenciesMenu01 : AnalyseRecordInterdependenciesMenu01<IArtifact>, IAnalyseArtifactRecordInterdependenciesMenu01
	{
		public AnalyseArtifactRecordInterdependenciesMenu01()
		{
			AnalyseMenu = Globals.CreateInstance<IAnalyseArtifactRecordInterdependenciesMenu>();
		}
	}
}
