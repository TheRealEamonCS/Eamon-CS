
// DeleteEffectRecordMenu.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework;
using Eamon.Framework.Helpers;
using Eamon.Game.Attributes;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.Globals;

namespace EamonDD.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class DeleteEffectRecordMenu : DeleteRecordMenu<IEffect, IEffectHelper>, IDeleteEffectRecordMenu
	{
		public override void PrintPostListLineSep()
		{
			gOut.Print("{0}", gEngine.LineSep);
		}

		public override void UpdateGlobals()
		{
			gEngine.EffectsModified = true;

			if (gEngine.Module != null)
			{
				gEngine.Module.NumEffects--;

				gEngine.ModulesModified = true;
			}
		}

		public DeleteEffectRecordMenu()
		{
			Title = "DELETE EFFECT RECORD";

			RecordTable = gEngine.Database.EffectTable;

			RecordTypeName = "Effect";
		}
	}
}
