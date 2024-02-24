
// IEngine.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework;
using Eamon.Framework.Menus;
using Eamon.Framework.Primitive.Classes;
using EamonMH.Framework.Menus;

namespace EamonMH.Framework.Plugin
{
	/// <inheritdoc />
	public interface IEngine : Eamon.Framework.Plugin.IEngine
	{
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
		string CharacterName { get; set; }

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

		#endregion

		#region Public Methods

		/// <summary></summary>
		/// <param name="ch"></param>
		/// <returns></returns>
		bool IsCharDOrM(char ch);

		/// <summary></summary>
		/// <param name="ch"></param>
		/// <returns></returns>
		bool IsCharROrT(char ch);

		/// <summary></summary>
		/// <param name="ch"></param>
		/// <returns></returns>
		bool IsCharDOrIOrX(char ch);

		/// <summary></summary>
		/// <param name="ch"></param>
		/// <returns></returns>
		bool IsCharDOrWOrX(char ch);

		/// <summary></summary>
		/// <param name="ch"></param>
		/// <returns></returns>
		bool IsCharUOrCOrX(char ch);

		/// <summary></summary>
		/// <param name="ch"></param>
		/// <returns></returns>
		bool IsChar1OrX(char ch);

		/// <summary></summary>
		/// <param name="ch"></param>
		/// <returns></returns>
		bool IsChar1Or2OrX(char ch);

		/// <summary></summary>
		/// <param name="ch"></param>
		/// <returns></returns>
		bool IsCharTOrL(char ch);

		/// <summary></summary>
		/// <param name="ch"></param>
		/// <returns></returns>
		bool IsCharBOrSOrAOrX(char ch);

		/// <summary></summary>
		/// <param name="ch"></param>
		/// <returns></returns>
		bool IsCharGOrFOrPOrX(char ch);

		/// <summary></summary>
		/// <param name="ch"></param>
		/// <returns></returns>
		bool IsCharWpnType(char ch);

		/// <summary></summary>
		/// <param name="ch"></param>
		/// <returns></returns>
		bool IsCharWpnTypeOrX(char ch);

		/// <summary></summary>
		/// <param name="ch"></param>
		/// <returns></returns>
		bool IsCharSpellType(char ch);

		/// <summary></summary>
		/// <param name="ch"></param>
		/// <returns></returns>
		bool IsCharSpellTypeOrX(char ch);

		/// <summary></summary>
		/// <param name="ch"></param>
		/// <returns></returns>
		bool IsCharMarcosNumOrX(char ch);

		/// <summary></summary>
		/// <param name="ch"></param>
		/// <returns></returns>
		bool IsCharWpnNumOrX(char ch);

		/// <summary></summary>
		/// <param name="ch"></param>
		/// <returns></returns>
		bool IsCharStat(char ch);

		/// <summary></summary>
		/// <returns></returns>
		long GetMaxArmorMarcosNum();

		/// <summary></summary>
		/// <param name="marcosNum"></param>
		/// <returns></returns>
		IArmor GetArmorByMarcosNum(long marcosNum);

		/// <summary></summary>
		/// <param name="secondPass"></param>
		/// <param name="nlFlag"></param>
		void MhProcessArgv(bool secondPass, ref bool nlFlag);

		#endregion
	}
}
