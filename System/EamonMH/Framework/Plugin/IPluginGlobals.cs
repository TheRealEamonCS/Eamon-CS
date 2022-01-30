
// IPluginGlobals.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework;
using Eamon.Framework.Menus;
using EamonMH.Framework.Menus;

namespace EamonMH.Framework.Plugin
{
	/// <summary></summary>
	public interface IPluginGlobals : Eamon.Framework.Plugin.IPluginGlobals
	{
		/// <summary></summary>
		string[] Argv { get; set; }

		/// <summary></summary>
		long WordWrapCurrColumn { get; set; }

		/// <summary></summary>
		char WordWrapLastChar { get; set; }

		/// <summary></summary>
		string ConfigFileName { get; set; }

		/// <summary></summary>
		string CharacterName { get; set; }

		/// <summary></summary>
		new IEngine Engine { get; set; }

		/// <summary></summary>
		IMhMenu MhMenu { get; set; }

		/// <summary></summary>
		IMenu Menu { get; set; }

		/// <summary></summary>
		IFileset Fileset { get; set; }

		/// <summary></summary>
		ICharacter Character { get; set; }

		/// <summary></summary>
		IConfig Config { get; set; }

		/// <summary></summary>
		bool GoOnAdventure { get; set; }

		/// <summary></summary>
		bool ConfigsModified { get; set; }

		/// <summary></summary>
		bool FilesetsModified { get; set; }

		/// <summary></summary>
		bool CharactersModified { get; set; }

		/// <summary></summary>
		bool EffectsModified { get; set; }
	}
}
