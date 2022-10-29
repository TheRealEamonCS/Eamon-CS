
// EditEffectRecordMenu.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework;
using Eamon.Framework.Helpers;
using Eamon.Game.Attributes;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.Globals;

namespace EamonDD.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class EditEffectRecordMenu : EditRecordManyFieldsMenu<IEffect, IEffectHelper>, IEditEffectRecordMenu
	{
		public override void UpdateGlobals()
		{
			gEngine.EffectsModified = true;
		}

		public EditEffectRecordMenu()
		{
			Title = "EDIT EFFECT RECORD FIELDS";

			RecordTable = gEngine.Database.EffectTable;

			RecordTypeName = "Effect";
		}
	}
}
