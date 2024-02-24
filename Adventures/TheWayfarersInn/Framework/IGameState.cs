
// IGameState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Collections.Generic;
using TheWayfarersInn.Framework.Primitive.Classes;
using TheWayfarersInn.Framework.Primitive.Enums;
using static TheWayfarersInn.Game.Plugin.Globals;

namespace TheWayfarersInn.Framework
{
	/// <inheritdoc />
	public interface IGameState : Eamon.Framework.IGameState
	{
		#region Properties

		/// <summary></summary>
		long WoodenBridgeUseCounter { get; set; }

		/// <summary></summary>
		long TotalCentipedeCounter { get; set; }

		/// <summary></summary>
		long AttackingCentipedeCounter { get; set; }

		/// <summary></summary>
		long BedroomTurnCounter { get; set; }

		/// <summary></summary>
		long KitchenRiddleState { get; set; }

		/// <summary></summary>
		long NolanPuddingAttackOdds { get; set; }

		/// <summary></summary>
		long NolanRustMonsterAttackOdds { get; set; }

		/// <summary></summary>
		long MetalArmorArtifactUid { get; set; }

		/// <summary></summary>
		long[] MonsterTotalDmgTaken { get; set; }

		/// <summary></summary>
		long[] EventStates { get; set; }

		/// <summary></summary>
		bool OutdoorsHauntingSeen { get; set; }

		/// <summary></summary>
		bool IndoorsHauntingSeen { get; set; }

		/// <summary></summary>
		bool UnseenApparitionMet { get; set; }

		/// <summary></summary>
		bool CharlottePeeks { get; set; }

		/// <summary></summary>
		bool CharlotteDeathSeen { get; set; }

		/// <summary></summary>
		bool CharlotteMet { get; set; }

		/// <summary></summary>
		bool CharlotteBlackthornStory { get; set; }

		/// <summary></summary>
		bool CharlotteArtisansStory { get; set; }

		/// <summary></summary>
		bool CharlottePortraitGiven { get; set; }

		/// <summary></summary>
		bool CharlotteBonesGiven { get; set; }

		/// <summary></summary>
		bool CharlotteBonesPurified { get; set; }

		/// <summary></summary>
		bool CharlotteBonesBlessed { get; set; }

		/// <summary></summary>
		bool CharlotteRestInPeace { get; set; }

		/// <summary></summary>
		bool CharlotteReunited { get; set; }

		/// <summary></summary>
		bool MirrorPassphraseSpoken { get; set; }

		/// <summary></summary>
		bool HearthwatcherPassphraseSpoken { get; set; }

		/// <summary></summary>
		bool LsInGoldFound { get; set; }

		/// <summary></summary>
		bool LsUnderGoldFound { get; set; }

		/// <summary></summary>
		bool DiaryRead { get; set; }

		/// <summary></summary>
		bool ForgecraftCodexRead { get; set; }

		/// <summary></summary>
		bool WallMapRead { get; set; }

		/// <summary></summary>
		bool LadderUsed { get; set; }

		/// <summary></summary>
		bool BourbonAppeared { get; set; }

		/// <summary></summary>
		bool BourbonNoticed { get; set; }

		/// <summary></summary>
		bool DartboardCreepsOut { get; set; }

		/// <summary></summary>
		bool FineClothingEnchanted { get; set; }

		IDictionary<long, GuestRoomData> GuestRoomDictionary { get; set; }

		/// <summary></summary>
		IList<long> OpenWindowRoomUids { get; set; }

		/// <summary></summary>
		IList<long> ForgedArtifactUids { get; set; }

		#endregion

		#region Methods

		/// <summary></summary>
		/// <param name="monsterUid"></param>
		/// <returns></returns>
		long GetMonsterTotalDmgTaken(long monsterUid);

		/// <summary></summary>
		/// <param name="eventState"></param>
		/// <returns></returns>
		long GetEventState(EventState eventState);

		/// <summary></summary>
		/// <param name="monsterUid"></param>
		/// <param name="value"></param>
		void SetMonsterTotalDmgTaken(long monsterUid, long value);

		/// <summary></summary>
		/// <param name="eventState"></param>
		/// <param name="value"></param>
		void SetEventState(EventState eventState, long value);

		#endregion
	}
}
