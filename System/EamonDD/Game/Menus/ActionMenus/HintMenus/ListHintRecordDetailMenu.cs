
// ListHintRecordDetailMenu.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework;
using Eamon.Framework.Helpers;
using Eamon.Game.Attributes;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.PluginContext;

namespace EamonDD.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class ListHintRecordDetailMenu : ListRecordDetailMenu<IHint, IHintHelper>, IListHintRecordDetailMenu
	{
		public override void PrintPostListLineSep()
		{
			gOut.Print("{0}", Globals.LineSep);
		}

		public ListHintRecordDetailMenu()
		{
			Title = "LIST HINT RECORD DETAILS";

			RecordTable = Globals.Database.HintTable;

			RecordTypeName = "Hint";
		}
	}
}
