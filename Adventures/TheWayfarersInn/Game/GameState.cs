
// GameState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using Eamon.Framework;
using Eamon.Game.Attributes;
using TheWayfarersInn.Framework.Primitive.Classes;
using TheWayfarersInn.Framework.Primitive.Enums;
using static TheWayfarersInn.Game.Plugin.Globals;

namespace TheWayfarersInn.Game
{
	[ClassMappings(typeof(IGameState))]
	public class GameState : Eamon.Game.GameState, Framework.IGameState
	{
		[FieldName(1121)]
		public virtual long WoodenBridgeUseCounter { get; set; }

		[FieldName(1122)]
		public virtual long TotalCentipedeCounter { get; set; }

		[FieldName(1123)]
		public virtual long AttackingCentipedeCounter { get; set; }

		[FieldName(1124)]
		public virtual long BedroomTurnCounter { get; set; }

		[FieldName(1125)]
		public virtual long KitchenRiddleState { get; set; }

		[FieldName(1126)]
		public virtual long NolanPuddingAttackOdds { get; set; }

		[FieldName(1127)]
		public virtual long NolanRustMonsterAttackOdds { get; set; }

		[FieldName(1128)]
		public virtual long MetalArmorArtifactUid { get; set; }

		[FieldName(1129)]
		public virtual long[] MonsterTotalDmgTaken { get; set; }

		[FieldName(1130)]
		public virtual long[] EventStates { get; set; }

		public virtual bool OutdoorsHauntingSeen { get; set; }

		public virtual bool IndoorsHauntingSeen { get; set; }

		public virtual bool UnseenApparitionMet { get; set; }

		public virtual bool CharlottePeeks { get; set; }

		public virtual bool CharlotteDeathSeen { get; set; }

		public virtual bool CharlotteMet { get; set; }

		public virtual bool CharlotteBlackthornStory { get; set; }

		public virtual bool CharlotteArtisansStory { get; set; }

		public virtual bool CharlottePortraitGiven { get; set; }

		public virtual bool CharlotteBonesGiven { get; set; }

		public virtual bool CharlotteBonesPurified { get; set; }

		public virtual bool CharlotteBonesBlessed { get; set; }

		public virtual bool CharlotteRestInPeace { get; set; }

		public virtual bool CharlotteReunited { get; set; }

		public virtual bool MirrorPassphraseSpoken { get; set; }

		public virtual bool HearthwatcherPassphraseSpoken { get; set; }

		public virtual bool LsInGoldFound { get; set; }

		public virtual bool LsUnderGoldFound { get; set; }

		public virtual bool DiaryRead { get; set; }

		public virtual bool ForgecraftCodexRead { get; set; }

		public virtual bool WallMapRead { get; set; }

		public virtual bool LadderUsed { get; set; }

		public virtual bool BourbonAppeared { get; set; }

		public virtual bool BourbonNoticed { get; set; }

		public virtual bool DartboardCreepsOut { get; set; }

		public virtual bool FineClothingEnchanted { get; set; }

		[FieldName(1131)]
		public virtual IDictionary<long, GuestRoomData> GuestRoomDictionary { get; set; }

		[FieldName(1132)]
		public virtual IList<long> OpenWindowRoomUids { get; set; }

		[FieldName(1133)]
		public virtual IList<long> ForgedArtifactUids { get; set; }

		public virtual long GetMonsterTotalDmgTaken(long monsterUid)
		{
			return MonsterTotalDmgTaken[monsterUid];
		}

		public virtual long GetEventState(EventState eventState)
		{
			return EventStates[(long)eventState];
		}

		public virtual void SetMonsterTotalDmgTaken(long monsterUid, long value)
		{
			MonsterTotalDmgTaken[monsterUid] = value;
		}

		public virtual void SetEventState(EventState eventState, long value)
		{
			EventStates[(long)eventState] = value;
		}

		public GameState()
		{
			TotalCentipedeCounter = 50;

			NolanPuddingAttackOdds = 100;

			NolanRustMonsterAttackOdds = 100;

			MonsterTotalDmgTaken = new long[26];      // TODO: eliminate hardcode

			EventStates = new long[Enum.GetNames(typeof(EventState)).Length];

			GuestRoomDictionary = new Dictionary<long, GuestRoomData>();

			OpenWindowRoomUids = new List<long>() { 39, 47, 53 };

			ForgedArtifactUids = new List<long>();
		}
	}
}
