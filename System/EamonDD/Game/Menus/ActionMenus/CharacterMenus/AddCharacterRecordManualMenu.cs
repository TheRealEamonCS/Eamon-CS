
// AddCharacterRecordManualMenu.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework;
using Eamon.Framework.Helpers;
using Eamon.Game.Attributes;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.PluginContext;

namespace EamonDD.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class AddCharacterRecordManualMenu : AddRecordManualMenu<ICharacter, ICharacterHelper>, IAddCharacterRecordManualMenu
	{
		public override void UpdateGlobals()
		{
			Globals.CharactersModified = true;
		}

		public AddCharacterRecordManualMenu()
		{
			Title = "ADD CHARACTER RECORD";

			RecordTable = Globals.Database.CharacterTable;

			RecordTypeName = "Character";
		}
	}
}
