
// AddCharacterRecordCopyMenu.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework;
using Eamon.Framework.Helpers;
using Eamon.Game.Attributes;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.Globals;

namespace EamonDD.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class AddCharacterRecordCopyMenu : AddRecordCopyMenu<ICharacter, ICharacterHelper>, IAddCharacterRecordCopyMenu
	{
		public override void UpdateGlobals()
		{
			gEngine.CharactersModified = true;
		}

		public AddCharacterRecordCopyMenu()
		{
			Title = "COPY CHARACTER RECORD";

			RecordTable = gDatabase.CharacterTable;

			RecordTypeName = "Character";
		}
	}
}
