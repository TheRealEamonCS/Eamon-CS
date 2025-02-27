
// IEngine.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Text;
using Eamon.Framework;

namespace TheDeepCanyon.Framework.Plugin
{
	/// <inheritdoc />
	public interface IEngine : EamonRT.Framework.Plugin.IEngine
	{
		/// <summary></summary>
		new StringBuilder Buf { get; set; }

		/// <summary></summary>
		new StringBuilder Buf01 { get; set; }

		long ResurrectMonsterUid { get; set; }

		/// <summary></summary>
		/// <param name="monster"></param>
		/// <returns></returns>
		void MagicRingLowersMonsterStats(IMonster monster);
	}
}
