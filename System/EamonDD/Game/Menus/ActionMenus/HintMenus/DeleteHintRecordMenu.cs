
// DeleteHintRecordMenu.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework;
using Eamon.Framework.Helpers;
using Eamon.Game.Attributes;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.PluginContext;

namespace EamonDD.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class DeleteHintRecordMenu : DeleteRecordMenu<IHint, IHintHelper>, IDeleteHintRecordMenu
	{
		public override void PrintPostListLineSep()
		{
			gOut.Print("{0}", Globals.LineSep);
		}

		public override void UpdateGlobals()
		{
			Globals.HintsModified = true;

			if (Globals.Module != null)
			{
				Globals.Module.NumHints--;

				Globals.ModulesModified = true;
			}
		}

		public DeleteHintRecordMenu()
		{
			Title = "DELETE HINT RECORD";

			RecordTable = Globals.Database.HintTable;

			RecordTypeName = "Hint";
		}
	}
}
