
// IPluginGlobals.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework;
using Eamon.Framework.Menus;
using EamonDD.Framework.Menus;

namespace EamonDD.Framework.Plugin
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
		new IEngine Engine { get; set; }

		/// <summary></summary>
		IDdMenu DdMenu { get; set; }

		/// <summary></summary>
		IMenu Menu { get; set; }

		/// <summary></summary>
		IModule Module { get; set; }

		/// <summary></summary>
		IConfig Config { get; set; }

		/// <summary></summary>
		bool ConfigsModified { get; set; }

		/// <summary></summary>
		bool FilesetsModified { get; set; }

		/// <summary></summary>
		bool CharactersModified { get; set; }

		/// <summary></summary>
		bool ModulesModified { get; set; }

		/// <summary></summary>
		bool RoomsModified { get; set; }

		/// <summary></summary>
		bool ArtifactsModified { get; set; }

		/// <summary></summary>
		bool EffectsModified { get; set; }

		/// <summary></summary>
		bool MonstersModified { get; set; }

		/// <summary></summary>
		bool HintsModified { get; set; }

		/// <summary></summary>
		bool TriggersModified { get; set; }

		/// <summary></summary>
		bool ScriptsModified { get; set; }
	}
}
