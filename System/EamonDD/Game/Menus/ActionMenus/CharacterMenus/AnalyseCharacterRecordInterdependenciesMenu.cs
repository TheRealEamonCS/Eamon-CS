
// AnalyseCharacterRecordInterdependenciesMenu.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework;
using Eamon.Framework.Helpers;
using Eamon.Game.Attributes;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.Globals;

namespace EamonDD.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class AnalyseCharacterRecordInterdependenciesMenu : AnalyseRecordInterdependenciesMenu<ICharacter, ICharacterHelper>, IAnalyseCharacterRecordInterdependenciesMenu
	{
		public AnalyseCharacterRecordInterdependenciesMenu()
		{
			Title = "ANALYSE CHARACTER RECORDS";

			RecordTable = gDatabase.CharacterTable;

			RecordTypeName = "Character";
		}
	}
}
