
// IEngine.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;

namespace TheWayfarersInn.Framework.Plugin
{
	/// <inheritdoc />
	public interface IEngine : EamonRT.Framework.Plugin.IEngine
	{
		string AccountingLedgerData { get; set; }

		string WallMuralData { get; set; }

		string KitchenMessageData { get; set; }

		long UnseenApparitionAttacks { get; set; }

		long SyndicateReward { get; set; }

		long[][] WanderRoomUids { get; set; }

		long[] NorthWindowRoomUids { get; set; }

		long[] SouthWindowRoomUids { get; set; }

		long[] WestWindowRoomUids { get; set; }

		long[] InnkeepersQuartersRoomUids { get; set; }

		long[] GuestRoomUids { get; set; }

		long[] NonEmotingMonsterUids { get; set; }

		IList<Action<Eamon.Framework.IRoom>> ForestEventFuncList { get; set; }

		IList<Action<Eamon.Framework.IRoom>> RiverEventFuncList { get; set; }

		IList<Action<Eamon.Framework.IRoom, Eamon.Framework.IMonster>> ChildsApparitionEventFuncList { get; set; }

		bool IsWindowRoomUid(long roomUid);

		bool IsKitchenShelfBalanced();

		IList<IMonster> GetCompanionList();

		Eamon.Framework.IRoom GetRandomWayfarersInnRoom(long[] omittedRoomUids);

		void GetOutdoorsHauntingData(long charRoomUid, long unseenApparitionRoomUid, ref string stateDesc);

		void GetIndoorsHauntingData(long charRoomUid, long unseenApparitionRoomUid);

		void BuildDecorationArtifact(long artifactUid, long effectUid, string name, string[] synonyms, string stateDesc, ArticleType articleType = ArticleType.A, PluralType pluralType = PluralType.S, bool isPlural = false);
	}
}
