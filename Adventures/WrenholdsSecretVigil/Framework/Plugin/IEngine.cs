
// IEngine.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Text;

namespace WrenholdsSecretVigil.Framework.Plugin
{
	/// <inheritdoc />
	public interface IEngine : EamonRT.Framework.Plugin.IEngine
	{
		/// <summary></summary>
		new StringBuilder Buf { get; set; }

		/// <summary></summary>
		new StringBuilder Buf01 { get; set; }

		/// <summary></summary>
		bool MonsterCurses { get; set; }

		/// <summary></summary>
		bool DeviceOpened { get; set; }

		/// <summary></summary>
		/// <param name="monster"></param>
		/// <param name="effectUid"></param>
		void PrintMonsterCurse(Eamon.Framework.IMonster monster, long effectUid);
	}
}
