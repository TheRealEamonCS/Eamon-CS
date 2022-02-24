
// IEngine.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework;
using static TheDeepCanyon.Game.Plugin.PluginContext;

namespace TheDeepCanyon.Framework
{
	public interface IEngine : EamonRT.Framework.IEngine
	{
		/// <summary></summary>
		/// <param name="monster"></param>
		/// <returns></returns>
		void MagicRingLowersMonsterStats(IMonster monster);
	}
}
