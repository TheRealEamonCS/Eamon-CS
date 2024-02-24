
// IEngine.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework;

namespace TheDeepCanyon.Framework.Plugin
{
	/// <inheritdoc />
	public interface IEngine : EamonRT.Framework.Plugin.IEngine
	{
		long ResurrectMonsterUid { get; set; }

		/// <summary></summary>
		/// <param name="monster"></param>
		/// <returns></returns>
		void MagicRingLowersMonsterStats(IMonster monster);
	}
}
