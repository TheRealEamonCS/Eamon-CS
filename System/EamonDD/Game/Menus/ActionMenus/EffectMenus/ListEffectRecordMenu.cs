﻿
// ListEffectRecordMenu.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework;
using Eamon.Framework.Helpers;
using Eamon.Game.Attributes;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.Globals;

namespace EamonDD.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class ListEffectRecordMenu : ListRecordDetailMenu<IEffect, IEffectHelper>, IListEffectRecordMenu
	{
		public override void PrintPostListLineSep()
		{
			gOut.Print("{0}", gEngine.LineSep);
		}

		public ListEffectRecordMenu()
		{
			Title = "LIST EFFECT RECORD DETAILS";

			RecordTable = gDatabase.EffectTable;

			RecordTypeName = "Effect";
		}
	}
}
