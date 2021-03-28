
// IEngine.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

namespace WrenholdsSecretVigil.Framework
{
	/// <summary></summary>
	public interface IEngine : EamonRT.Framework.IEngine
	{
		/// <summary></summary>
		/// <param name="monster"></param>
		/// <param name="effectUid"></param>
		/// <returns></returns>
		string GetMonsterCurse(Eamon.Framework.IMonster monster, long effectUid);
	}
}
