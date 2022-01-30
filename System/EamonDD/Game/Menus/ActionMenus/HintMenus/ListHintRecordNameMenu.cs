
// ListHintRecordNameMenu.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework;
using Eamon.Framework.Helpers;
using Eamon.Game.Attributes;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.PluginContext;

namespace EamonDD.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class ListHintRecordNameMenu : ListRecordNameMenu<IHint, IHintHelper>, IListHintRecordNameMenu
	{
		public ListHintRecordNameMenu()
		{
			Title = "LIST HINT RECORD NAMES";

			RecordTable = Globals.Database.HintTable;

			RecordTypeName = "Hint";
		}
	}
}
