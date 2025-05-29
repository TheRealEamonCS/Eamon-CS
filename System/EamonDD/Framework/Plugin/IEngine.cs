
// IEngine.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Text;
using Eamon.Framework;
using Eamon.Framework.Menus;
using EamonDD.Framework.Menus;

namespace EamonDD.Framework.Plugin
{
	/// <inheritdoc />
	public interface IEngine : Eamon.Framework.Plugin.IEngine
	{
		#region Explicit Properties

		/// <summary></summary>
		new StringBuilder Buf { get; set; }

		#endregion

		#region Public Properties

		/// <summary></summary>
		string[] Argv { get; set; }

		/// <summary></summary>
		long WordWrapCurrColumn { get; set; }

		/// <summary></summary>
		char WordWrapLastChar { get; set; }

		/// <summary></summary>
		string ConfigFileName { get; set; }

		/// <summary></summary>
		IDdMenu DdMenu { get; set; }

		/// <summary></summary>
		IMenu Menu { get; set; }

		/// <summary></summary>
		IModule Module { get; set; }

		/// <summary></summary>
		IConfig Config { get; set; }

		/// <summary></summary>
		bool BortCommand { get; set; }

		/// <summary></summary>
		bool DdSuppressPostInputSleep { get; set; }

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
		bool CharArtsModified { get; set; }

		/// <summary></summary>
		bool EffectsModified { get; set; }

		/// <summary></summary>
		bool MonstersModified { get; set; }

		/// <summary></summary>
		bool HintsModified { get; set; }

		#endregion

		#region Public Methods

		/// <summary></summary>
		/// <returns></returns>
		bool IsAdventureFilesetLoaded();

		/// <summary></summary>
		/// <returns></returns>
		bool IsCharacterInventoryLoaded();

		/// <summary></summary>
		/// <param name="secondPass"></param>
		/// <param name="ddfnFlag"></param>
		/// <param name="nlFlag"></param>
		void DdProcessArgv(bool secondPass, ref bool ddfnFlag, ref bool nlFlag);

		#endregion
	}
}
