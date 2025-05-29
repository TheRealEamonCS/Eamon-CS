
// AnalyseCharacterRecordInterdependenciesMenu01.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework;
using Eamon.Game.Attributes;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.Globals;

namespace EamonDD.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class AnalyseCharacterRecordInterdependenciesMenu01 : AnalyseRecordInterdependenciesMenu01<ICharacter>, IAnalyseCharacterRecordInterdependenciesMenu01
	{
		public AnalyseCharacterRecordInterdependenciesMenu01()
		{
			AnalyseMenu = gEngine.CreateInstance<IAnalyseCharacterRecordInterdependenciesMenu>();
		}
	}
}
