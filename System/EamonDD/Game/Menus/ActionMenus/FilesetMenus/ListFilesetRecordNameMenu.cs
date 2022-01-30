
// ListFilesetRecordNameMenu.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework;
using Eamon.Framework.Helpers;
using Eamon.Game.Attributes;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.PluginContext;

namespace EamonDD.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class ListFilesetRecordNameMenu : ListRecordNameMenu<IFileset, IFilesetHelper>, IListFilesetRecordNameMenu
	{
		public ListFilesetRecordNameMenu()
		{
			Title = "LIST FILESET RECORD NAMES";

			RecordTable = Globals.Database.FilesetTable;

			RecordTypeName = "Fileset";
		}
	}
}
