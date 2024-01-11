
// AnalyseHintRecordInterdependenciesMenu.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework;
using Eamon.Framework.Helpers;
using Eamon.Game.Attributes;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.Globals;

namespace EamonDD.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class AnalyseHintRecordInterdependenciesMenu : AnalyseRecordInterdependenciesMenu<IHint, IHintHelper>, IAnalyseHintRecordInterdependenciesMenu
	{
		public AnalyseHintRecordInterdependenciesMenu()
		{
			Title = "ANALYSE HINT RECORDS";

			RecordTable = gDatabase.HintTable;

			RecordTypeName = "Hint";
		}
	}
}
