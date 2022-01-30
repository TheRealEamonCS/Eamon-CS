
// IGameState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

namespace WrenholdsSecretVigil.Framework
{
	/// <summary></summary>
	public interface IGameState : Eamon.Framework.IGameState
	{
		#region Properties

		/// <summary></summary>
		long MedallionCharges { get; set; }

		/// <summary></summary>
		long SlimeBlasts { get; set; }

		/// <summary></summary>
		bool PulledRope { get; set; }

		/// <summary></summary>
		bool RemovedLifeOrb { get; set; }

		/// <summary></summary>
		bool[] MonsterCurses { get; set; }

		#endregion

		#region Methods

		/// <summary></summary>
		/// <param name="index"></param>
		/// <returns></returns>
		bool GetMonsterCurses(long index);

		/// <summary></summary>
		/// <param name="index"></param>
		/// <param name="value"></param>
		void SetMonsterCurses(long index, bool value);

		#endregion
	}
}
