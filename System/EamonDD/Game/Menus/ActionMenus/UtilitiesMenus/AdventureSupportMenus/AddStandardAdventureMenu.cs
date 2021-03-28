
// AddStandardAdventureMenu.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonDD.Framework.Menus.ActionMenus;
using static EamonDD.Game.Plugin.PluginContext;

namespace EamonDD.Game.Menus.ActionMenus
{
	[ClassMappings]
	public class AddStandardAdventureMenu : AdventureSupportMenu01, IAddStandardAdventureMenu
	{
		public override void Execute()
		{
			gOut.WriteLine();

			gEngine.PrintTitle("ADD STANDARD ADVENTURE", true);

			Debug.Assert(!gEngine.IsAdventureFilesetLoaded());

			GotoCleanup = false;

			GetAdventureName();

			if (GotoCleanup)
			{
				goto Cleanup;
			}

			GetAuthorName();

			GetAuthorInitials();

			SelectAdvDbDataFiles();

			QueryToAddAdventure();

			if (GotoCleanup)
			{
				goto Cleanup;
			}

			CreateQuickLaunchFiles();

			CreateAdventureFolder();

			CreateHintsXml();

			UpdateAdvDbDataFiles();

			PrintAdventureCreated();

		Cleanup:

			if (GotoCleanup)
			{
				// TODO: rollback adventure buildout if necessary
			}
		}

		public AddStandardAdventureMenu()
		{

		}
	}
}
