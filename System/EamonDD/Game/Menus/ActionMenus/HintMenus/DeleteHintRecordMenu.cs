
// DeleteHintRecordMenu.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework;
using Eamon.Framework.Helpers;
using Eamon.Game.Attributes;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.Globals;

namespace EamonDD.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class DeleteHintRecordMenu : DeleteRecordMenu<IHint, IHintHelper>, IDeleteHintRecordMenu
	{
		public override void PrintPostListLineSep()
		{
			gOut.Print("{0}", gEngine.LineSep);
		}

		public override void UpdateGlobals()
		{
			gEngine.HintsModified = true;

			if (gEngine.Module != null)
			{
				gEngine.Module.NumHints--;

				gEngine.ModulesModified = true;
			}
		}

		public DeleteHintRecordMenu()
		{
			Title = "DELETE HINT RECORD";

			RecordTable = gDatabase.HintTable;

			RecordTypeName = "Hint";
		}
	}
}
