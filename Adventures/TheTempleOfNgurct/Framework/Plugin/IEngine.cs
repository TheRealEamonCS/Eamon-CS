
// IEngine.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Text;
using Eamon.Framework;
using EamonRT.Framework.States;

namespace TheTempleOfNgurct.Framework.Plugin
{
	/// <inheritdoc />
	public interface IEngine : EamonRT.Framework.Plugin.IEngine
	{
		/// <summary></summary>
		new StringBuilder Buf { get; set; }

		/// <summary></summary>
		new StringBuilder Buf01 { get; set; }

		/// <summary></summary>
		bool FireDamage { get; set; }

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
