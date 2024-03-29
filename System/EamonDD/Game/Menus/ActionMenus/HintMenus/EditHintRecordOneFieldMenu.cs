﻿
// EditHintRecordOneFieldMenu.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework;
using Eamon.Framework.Helpers;
using Eamon.Game.Attributes;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.Globals;

namespace EamonDD.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class EditHintRecordOneFieldMenu : EditRecordOneFieldMenu<IHint, IHintHelper>, IEditHintRecordOneFieldMenu
	{
		public override void PrintPostListLineSep()
		{
			gOut.Print("{0}", gEngine.LineSep);
		}

		public override void UpdateGlobals()
		{
			gEngine.HintsModified = true;
		}

		public EditHintRecordOneFieldMenu()
		{
			Title = "EDIT HINT RECORD FIELD";

			RecordTable = gDatabase.HintTable;

			RecordTypeName = "Hint";
		}
	}
}
