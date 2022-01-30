
// EditEffectRecordMenu.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework;
using Eamon.Framework.Helpers;
using Eamon.Game.Attributes;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.PluginContext;

namespace EamonDD.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class EditEffectRecordMenu : EditRecordManyFieldsMenu<IEffect, IEffectHelper>, IEditEffectRecordMenu
	{
		public override void UpdateGlobals()
		{
			Globals.EffectsModified = true;
		}

		public EditEffectRecordMenu()
		{
			Title = "EDIT EFFECT RECORD FIELDS";

			RecordTable = Globals.Database.EffectTable;

			RecordTypeName = "Effect";
		}
	}
}
