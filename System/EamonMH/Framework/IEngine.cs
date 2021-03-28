
// IEngine.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework.Primitive.Classes;

namespace EamonMH.Framework
{
	/// <summary></summary>
	public interface IEngine : Eamon.Framework.IEngine
	{
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
	};
}
