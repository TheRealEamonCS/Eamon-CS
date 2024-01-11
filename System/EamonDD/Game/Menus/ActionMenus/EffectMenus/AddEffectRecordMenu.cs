
// AddEffectRecordMenu.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework;
using Eamon.Framework.Helpers;
using Eamon.Game.Attributes;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.Globals;

namespace EamonDD.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class AddEffectRecordMenu : AddRecordManualMenu<IEffect, IEffectHelper>, IAddEffectRecordMenu
	{
		public override void UpdateGlobals()
		{
			gEngine.EffectsModified = true;

			if (gEngine.Module != null)
			{
				gEngine.Module.NumEffects++;

				gEngine.ModulesModified = true;
			}
		}

		public AddEffectRecordMenu()
		{
			Title = "ADD EFFECT RECORD";

			RecordTable = gDatabase.EffectTable;

			RecordTypeName = "Effect";
		}
	}
}
