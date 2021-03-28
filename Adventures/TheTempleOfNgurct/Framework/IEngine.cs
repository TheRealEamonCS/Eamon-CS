
// IEngine.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using Eamon.Framework;
using EamonRT.Framework.States;

namespace TheTempleOfNgurct.Framework
{
	/// <summary></summary>
	public interface IEngine : EamonRT.Framework.IEngine
	{
		/// <summary></summary>
		/// <param name="numMonsters"></param>
		/// <param name="roomUid"></param>
		/// <returns></returns>
		IList<IMonster> GetTrapMonsterList(long numMonsters, long roomUid);

		/// <summary></summary>
		/// <param name="setNextStateFunc"></param>
		/// <param name="monster"></param>
		/// <param name="numDice"></param>
		/// <param name="numSides"></param>
		/// <param name="omitArmor"></param>
		void ApplyTrapDamage(Action<IState> setNextStateFunc, IMonster monster, long numDice, long numSides, bool omitArmor);

		/// <summary></summary>
		/// <returns></returns>
		bool GetWanderingMonster();
	}
}
