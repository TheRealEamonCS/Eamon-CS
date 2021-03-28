
// DeleteEffectRecordMenu.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework;
using Eamon.Framework.Helpers;
using Eamon.Game.Attributes;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.PluginContext;

namespace EamonDD.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class DeleteEffectRecordMenu : DeleteRecordMenu<IEffect, IEffectHelper>, IDeleteEffectRecordMenu
	{
		public override void PrintPostListLineSep()
		{
			gOut.Print("{0}", Globals.LineSep);
		}

		public override void UpdateGlobals()
		{
			Globals.EffectsModified = true;

			if (Globals.Module != null)
			{
				Globals.Module.NumEffects--;

				Globals.ModulesModified = true;
			}
		}

		public DeleteEffectRecordMenu()
		{
			Title = "DELETE EFFECT RECORD";

			RecordTable = Globals.Database.EffectTable;

			RecordTypeName = "Effect";
		}
	}
}
