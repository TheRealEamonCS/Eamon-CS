
// EditCharacterRecordOneFieldMenu.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework;
using Eamon.Framework.Helpers;
using Eamon.Game.Attributes;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.Globals;

namespace EamonDD.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class EditCharacterRecordOneFieldMenu : EditRecordOneFieldMenu<ICharacter, ICharacterHelper>, IEditCharacterRecordOneFieldMenu
	{
		public override void UpdateGlobals()
		{
			gEngine.CharactersModified = true;
		}

		public EditCharacterRecordOneFieldMenu()
		{
			Title = "EDIT CHARACTER RECORD FIELD";

			RecordTable = gEngine.Database.CharacterTable;

			RecordTypeName = "Character";
		}
	}
}
