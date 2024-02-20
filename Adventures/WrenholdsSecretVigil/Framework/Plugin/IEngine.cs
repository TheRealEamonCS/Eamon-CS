
// IEngine.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

namespace WrenholdsSecretVigil.Framework.Plugin
{
	/// <inheritdoc />
	public interface IEngine : EamonRT.Framework.Plugin.IEngine
	{
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
