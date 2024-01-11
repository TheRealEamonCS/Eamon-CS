
// EditCharacterRecordManyFieldsMenu.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework;
using Eamon.Framework.Helpers;
using Eamon.Game.Attributes;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.Globals;

namespace EamonDD.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class EditCharacterRecordManyFieldsMenu : EditRecordManyFieldsMenu<ICharacter, ICharacterHelper>, IEditCharacterRecordManyFieldsMenu
	{
		public override void UpdateGlobals()
		{
			gEngine.CharactersModified = true;
		}

		public EditCharacterRecordManyFieldsMenu()
		{
			Title = "EDIT CHARACTER RECORD FIELDS";

			RecordTable = gDatabase.CharacterTable;

			RecordTypeName = "Character";
		}
	}
}
