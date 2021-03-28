
// IGameState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

namespace TheTempleOfNgurct.Framework
{
	/// <summary></summary>
	public interface IGameState : Eamon.Framework.IGameState
	{
		#region Properties

		/// <summary></summary>
		long WanderingMonster { get; set; }

		/// <summary></summary>
		long DwLoopCounter { get; set; }

		/// <summary></summary>
		long WandCharges { get; set; }

		/// <summary></summary>
		long Regenerate { get; set; }

		/// <summary></summary>
		long KeyRingRoomUid { get; set; }

		/// <summary></summary>
		bool AlkandaKilled { get; set; }

		/// <summary></summary>
		bool AlignmentConflict { get; set; }

		/// <summary></summary>
		bool CobraAppeared { get; set; }

		#endregion
	}
}
