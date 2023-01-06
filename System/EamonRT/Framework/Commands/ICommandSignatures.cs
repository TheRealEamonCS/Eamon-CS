
// ICommandSignatures.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Collections.Generic;
using System.Text;
using Eamon.Framework;
using Eamon.Framework.Args;
using Eamon.Framework.Primitive.Classes;
using Eamon.Framework.Primitive.Enums;
using EamonRT.Framework.Parsing;

namespace EamonRT.Framework.Commands
{
	/// <summary></summary>
	public interface ICommandSignatures
	{
		/// <summary></summary>
		ICommandParser CommandParser { get; set; }

		/// <summary></summary>
		IMonster ActorMonster { get; set; }

		/// <summary></summary>
		IRoom ActorRoom { get; set; }

		/// <summary></summary>
		IGameBase Dobj { get; set; }

		/// <summary></summary>
		IArtifact DobjArtifact { get; }

		/// <summary></summary>
		IMonster DobjMonster { get; }

		/// <summary></summary>
		IGameBase Iobj { get; set; }

		/// <summary></summary>
		IArtifact IobjArtifact { get; }

		/// <summary></summary>
		IMonster IobjMonster { get; }

		/// <summary></summary>
		string[] Synonyms { get; set; }

		/// <summary></summary>
		long SortOrder { get; set; }

		/// <summary></summary>
		string Verb { get; set; }

		/// <summary></summary>
		IPrep Prep { get; set; }

		/// <summary></summary>
		CommandType Type { get; set; }

		/// <summary></summary>
		ContainerType ContainerType { get; set; }

		/// <summary></summary>
		bool GetCommandCalled { get; set; }

		/// <summary></summary>
		bool IsNew { get; set; }

		/// <summary></summary>
		bool IsListed { get; set; }

		/// <summary></summary>
		bool IsSentenceParserEnabled { get; set; }

		/// <summary></summary>
		bool IsDobjPrepEnabled { get; set; }

		/// <summary></summary>
		bool IsIobjEnabled { get; set; }

		/// <summary></summary>
		bool IsDarkEnabled { get; set; }

		/// <summary></summary>
		bool IsPlayerEnabled { get; set; }

		/// <summary></summary>
		bool IsMonsterEnabled { get; set; }

		/// <summary></summary>
		/// <param name="obj"></param>
		void PrintCantVerbObj(IGameBase obj);

		/// <summary></summary>
		/// <param name="artifact"></param>
		void PrintCantVerbIt(IArtifact artifact);

		/// <summary></summary>
		/// <param name="artifact"></param>
		void PrintCantVerbThat(IArtifact artifact);

		/// <summary></summary>
		/// <param name="obj1"></param>
		/// <param name="obj2"></param>
		void PrintDoYouMeanObj1OrObj2(IGameBase obj1, IGameBase obj2);

		/// <summary></summary>
		/// <param name="obj"></param>
		void PrintWhyAttack(IGameBase obj);

		/// <summary></summary>
		/// <param name="artifact"></param>
		void PrintTakingFirst(IArtifact artifact);

		/// <summary></summary>
		/// <param name="artifact"></param>
		void PrintRemovingFirst(IArtifact artifact);

		/// <summary></summary>
		/// <param name="artifact"></param>
		void PrintBestLeftAlone(IArtifact artifact);

		/// <summary></summary>
		/// <param name="artifact"></param>
		/// <param name="getAll"></param>
		void PrintTooHeavy(IArtifact artifact, bool getAll = false);

		/// <summary></summary>
		/// <param name="artifact"></param>
		void PrintMustBeFreed(IArtifact artifact);

		/// <summary></summary>
		/// <param name="artifact"></param>
		void PrintMustFirstOpen(IArtifact artifact);

		/// <summary></summary>
		/// <param name="artifact"></param>
		void PrintMustFirstClose(IArtifact artifact);

		/// <summary></summary>
		/// <param name="artifact"></param>
		void PrintWorn(IArtifact artifact);

		/// <summary></summary>
		/// <param name="artifact"></param>
		void PrintRemoved(IArtifact artifact);

		/// <summary></summary>
		/// <param name="artifact"></param>
		void PrintOpened(IArtifact artifact);

		/// <summary></summary>
		/// <param name="artifact"></param>
		void PrintClosed(IArtifact artifact);

		/// <summary></summary>
		/// <param name="artifact"></param>
		void PrintReceived(IArtifact artifact);

		/// <summary></summary>
		/// <param name="artifact"></param>
		void PrintRetrieved(IArtifact artifact);

		/// <summary></summary>
		/// <param name="artifact"></param>
		/// <param name="getAll"></param>
		void PrintTaken(IArtifact artifact, bool getAll = false);

		/// <summary></summary>
		/// <param name="artifact"></param>
		void PrintDropped(IArtifact artifact);

		/// <summary></summary>
		/// <param name="artifact"></param>
		void PrintReadied(IArtifact artifact);

		/// <summary></summary>
		/// <param name="artifact"></param>
		void PrintNotOpen(IArtifact artifact);

		/// <summary></summary>
		/// <param name="artifact"></param>
		void PrintAlreadyOpen(IArtifact artifact);

		/// <summary></summary>
		/// <param name="artifact"></param>
		void PrintWontOpen(IArtifact artifact);

		/// <summary></summary>
		/// <param name="artifact"></param>
		void PrintWontFit(IArtifact artifact);

		/// <summary></summary>
		/// <param name="artifact"></param>
		void PrintFull(IArtifact artifact);

		/// <summary></summary>
		/// <param name="artifact"></param>
		void PrintOutOfSpace(IArtifact artifact);

		/// <summary></summary>
		/// <param name="artifact"></param>
		void PrintLocked(IArtifact artifact);

		/// <summary></summary>
		/// <param name="artifact"></param>
		void PrintBrokeIt(IArtifact artifact);

		/// <summary></summary>
		/// <param name="artifact"></param>
		void PrintHaveToForceOpen(IArtifact artifact);

		/// <summary></summary>
		/// <param name="artifact"></param>
		void PrintWearingRemoveFirst(IArtifact artifact);

		/// <summary></summary>
		/// <param name="artifact"></param>
		void PrintWearingRemoveFirst01(IArtifact artifact);

		/// <summary></summary>
		/// <param name="shield"></param>
		/// <param name="weapon"></param>
		void PrintCantWearShieldWithWeapon(IArtifact shield, IArtifact weapon);

		/// <summary></summary>
		/// <param name="artifact"></param>
		/// <param name="containerType"></param>
		/// <param name="isPlural"></param>
		void PrintContainerNotEmpty(IArtifact artifact, ContainerType containerType, bool isPlural);

		/// <summary></summary>
		/// <param name="artifact"></param>
		void PrintVerbItAll(IArtifact artifact);

		/// <summary></summary>
		/// <param name="artifact"></param>
		void PrintNoneLeft(IArtifact artifact);

		/// <summary></summary>
		void PrintOkay();

		/// <summary></summary>
		/// <param name="artifact"></param>
		void PrintOkay(IArtifact artifact);

		/// <summary></summary>
		/// <param name="artifact"></param>
		void PrintFeelBetter(IArtifact artifact);

		/// <summary></summary>
		/// <param name="artifact"></param>
		void PrintFeelWorse(IArtifact artifact);

		/// <summary></summary>
		/// <param name="artifact"></param>
		void PrintTryDifferentCommand(IArtifact artifact);

		/// <summary></summary>
		/// <param name="artifact"></param>
		void PrintNotWeapon(IArtifact artifact);

		/// <summary></summary>
		/// <param name="artifact"></param>
		void PrintNotReadyableWeapon(IArtifact artifact);

		/// <summary></summary>
		/// <param name="artifact"></param>
		void PrintNotWhileCarryingObj(IArtifact artifact);

		/// <summary></summary>
		/// <param name="artifact"></param>
		void PrintNotWhileWearingObj(IArtifact artifact);

		/// <summary></summary>
		/// <param name="artifact"></param>
		void PrintWontLight(IArtifact artifact);

		/// <summary></summary>
		/// <param name="artifact"></param>
		void PrintLightObj(IArtifact artifact);

		/// <summary></summary>
		/// <param name="artifact"></param>
		void PrintExtinguishObj(IArtifact artifact);

		/// <summary></summary>
		/// <param name="artifact"></param>
		void PrintLightExtinguished(IArtifact artifact);

		/// <summary></summary>
		/// <param name="artifact"></param>
		void PrintAlreadyWearingObj(IArtifact artifact);

		/// <summary></summary>
		/// <param name="artifact"></param>
		void PrintWhamHitObj(IArtifact artifact);

		/// <summary></summary>
		/// <param name="artifact"></param>
		/// <param name="showName"></param>
		/// <param name="showVerboseName"></param>
		void PrintFullDesc(IArtifact artifact, bool showName, bool showVerboseName);

		/// <summary></summary>
		/// <param name="artifact"></param>
		/// <param name="objAmount"></param>
		/// <param name="objEdible"></param>
		void PrintObjAmountLeft(IArtifact artifact, long objAmount, bool objEdible);

		/// <summary></summary>
		/// <param name="artifact"></param>
		/// <param name="containerArtifactList"></param>
		/// <param name="containerType"></param>
		/// <param name="showCharOwned"></param>
		/// <param name="recordNameListArgs"></param>
		void PrintPrepContainerYouSee(IArtifact artifact, IList<IArtifact> containerArtifactList, ContainerType containerType, bool showCharOwned, IRecordNameListArgs recordNameListArgs = null);

		/// <summary></summary>
		/// <param name="artifact"></param>
		/// <param name="containerType"></param>
		/// <param name="showCharOwned"></param>
		void PrintNothingPrepContainer(IArtifact artifact, ContainerType containerType, bool showCharOwned);

		/// <summary></summary>
		/// <param name="artifact"></param>
		/// <param name="direction"></param>
		void PrintAttemptingToFlee(IArtifact artifact, Direction direction);

		/// <summary></summary>
		/// <param name="weapon"></param>
		/// <param name="shield"></param>
		void PrintCantReadyWeaponWithShield(IArtifact weapon, IArtifact shield);

		/// <summary></summary>
		/// <param name="monster"></param>
		void PrintPolitelyRefuses(IMonster monster);

		/// <summary></summary>
		/// <param name="monster"></param>
		void PrintTakesTheMoney(IMonster monster);

		/// <summary></summary>
		/// <param name="monster"></param>
		void PrintWontLetYou(IMonster monster);

		/// <summary></summary>
		/// <param name="monster"></param>
		void PrintHealthImproves(IMonster monster);

		/// <summary></summary>
		/// <param name="monster"></param>
		void PrintHaventSavedGameYet(IMonster monster);

		/// <summary></summary>
		/// <param name="monster"></param>
		/// <param name="showName"></param>
		/// <param name="showVerboseName"></param>
		void PrintFullDesc(IMonster monster, bool showName, bool showVerboseName);

		/// <summary></summary>
		/// <param name="monster"></param>
		/// <param name="includeUninjuredGroupMonsters"></param>
		void PrintHealthStatus(IMonster monster, bool includeUninjuredGroupMonsters);

		/// <summary></summary>
		/// <param name="monster"></param>
		/// <param name="goldAmount"></param>
		void PrintGiveGoldPiecesTo(IMonster monster, long goldAmount);

		/// <summary></summary>
		/// <param name="monster"></param>
		/// <param name="monsterWornArtifactList"></param>
		/// <param name="recordNameListArgs"></param>
		void PrintActorIsWearing(IMonster monster, IList<IArtifact> monsterWornArtifactList, IRecordNameListArgs recordNameListArgs = null);

		/// <summary></summary>
		/// <param name="monster"></param>
		/// <param name="monsterCarriedArtifactList"></param>
		/// <param name="recordNameListArgs"></param>
		void PrintActorIsCarrying(IMonster monster, IList<IArtifact> monsterCarriedArtifactList, IRecordNameListArgs recordNameListArgs = null);

		/// <summary></summary>
		/// <param name="artifact"></param>
		/// <param name="monster"></param>
		/// <param name="objOpened"></param>
		/// <param name="objEdible"></param>
		void PrintOpensConsumesAndHandsBack(IArtifact artifact, IMonster monster, bool objOpened, bool objEdible);

		/// <summary></summary>
		/// <param name="artifact"></param>
		/// <param name="monster"></param>
		/// <param name="objOpened"></param>
		void PrintConsumesItAll(IArtifact artifact, IMonster monster, bool objOpened);

		/// <summary></summary>
		/// <param name="artifact"></param>
		/// <param name="monster"></param>
		/// <param name="objOpened"></param>
		void PrintConsumesItAllHandsBack(IArtifact artifact, IMonster monster, bool objOpened);

		/// <summary></summary>
		/// <param name="artifact"></param>
		/// <param name="monster"></param>
		void PrintGiveObjToActor(IArtifact artifact, IMonster monster);

		/// <summary></summary>
		/// <param name="artifact"></param>
		/// <param name="monster"></param>
		void PrintObjBelongsToActor(IArtifact artifact, IMonster monster);

		/// <summary></summary>
		/// <param name="monster"></param>
		/// <param name="key"></param>
		void PrintFreeActorWithKey(IMonster monster, IArtifact key);

		/// <summary></summary>
		/// <param name="artifact"></param>
		/// <param name="key"></param>
		void PrintOpenObjWithKey(IArtifact artifact, IArtifact key);

		/// <summary></summary>
		/// <param name="artifact"></param>
		/// <param name="container"></param>
		/// <param name="containerType"></param>
		void PrintPutObjPrepContainer(IArtifact artifact, IArtifact container, ContainerType containerType);

		/// <summary></summary>
		/// <param name="monster"></param>
		/// <param name="artifact"></param>
		/// <param name="container"></param>
		/// <param name="containerType"></param>
		/// <param name="omitWeightCheck"></param>
		void PrintActorRemovesObjPrepContainer(IMonster monster, IArtifact artifact, IArtifact container, ContainerType containerType, bool omitWeightCheck);

		/// <summary></summary>
		/// <param name="monster"></param>
		/// <param name="artifact"></param>
		void PrintActorPicksUpObj(IMonster monster, IArtifact artifact);

		/// <summary></summary>
		/// <param name="monster"></param>
		/// <param name="artifact"></param>
		void PrintActorReadiesObj(IMonster monster, IArtifact artifact);

		/// <summary></summary>
		/// <param name="monster"></param>
		/// <param name="artifact"></param>
		/// <param name="container"></param>
		/// <param name="containerType"></param>
		/// <param name="omitWeightCheck"></param>
		void PrintActorRemovesObjPrepContainer01(IMonster monster, IArtifact artifact, IArtifact container, ContainerType containerType, bool omitWeightCheck);

		/// <summary></summary>
		/// <param name="monster"></param>
		void PrintActorPicksUpWeapon(IMonster monster);

		/// <summary></summary>
		/// <param name="monster"></param>
		void PrintActorReadiesWeapon(IMonster monster);

		/// <summary></summary>
		/// <param name="room"></param>
		/// <param name="artifact"></param>
		void PrintBortVisitArtifact(IRoom room, IArtifact artifact);

		/// <summary></summary>
		/// <param name="room"></param>
		/// <param name="monster"></param>
		void PrintBortVisitMonster(IRoom room, IMonster monster);

		/// <summary></summary>
		/// <param name="room"></param>
		void PrintBortVisitRoom(IRoom room);

		/// <summary></summary>
		/// <param name="room"></param>
		/// <param name="artifact"></param>
		void PrintBortRecallArtifact(IRoom room, IArtifact artifact);

		/// <summary></summary>
		/// <param name="room"></param>
		/// <param name="monster"></param>
		void PrintBortRecallMonster(IRoom room, IMonster monster);

		/// <summary></summary>
		/// <param name="artifact"></param>
		void PrintBortArtifactRoomInvalid(IArtifact artifact);

		/// <summary></summary>
		/// <param name="monster"></param>
		void PrintBortMonsterRoomInvalid(IMonster monster);

		/// <summary></summary>
		void PrintBortArtifactInvalid();

		/// <summary></summary>
		void PrintBortMonsterInvalid();

		/// <summary></summary>
		void PrintBortRoomInvalid();

		/// <summary></summary>
		/// <param name="hintNum"></param>
		/// <param name="question"></param>
		void PrintHintQuestion(long hintNum, string question);

		/// <summary></summary>
		/// <param name="question"></param>
		void PrintHintQuestion01(string question);

		/// <summary></summary>
		/// <param name="answer"></param>
		/// <param name="buf"></param>
		void PrintHintAnswer(string answer, StringBuilder buf);

		/// <summary></summary>
		void PrintSayText(string printedPhrase);

		/// <summary></summary>
		void PrintBortUsage();

		/// <summary></summary>
		void PrintSettingsUsage();

		/// <summary></summary>
		void PrintNotEnoughGold();

		/// <summary></summary>
		void PrintMustFirstReadyWeapon();

		/// <summary></summary>
		void PrintDontHaveItNotHere();

		/// <summary></summary>
		void PrintDontHaveIt();

		/// <summary></summary>
		void PrintDontNeedTo();

		/// <summary></summary>
		void PrintCantDoThat();

		/// <summary></summary>
		void PrintCantVerbThat();

		/// <summary></summary>
		void PrintCantVerbHere();

		/// <summary></summary>
		void PrintBeMoreSpecific();

		/// <summary></summary>
		void PrintNobodyHereByThatName();

		/// <summary></summary>
		void PrintNothingHereByThatName();

		/// <summary></summary>
		void PrintYouSeeNothingSpecial();

		/// <summary></summary>
		void PrintDontBeAbsurd();

		/// <summary></summary>
		void PrintCalmDown();

		/// <summary></summary>
		void PrintNoPlaceToGo();

		/// <summary></summary>
		void PrintAttackNonEnemy();

		/// <summary></summary>
		void PrintAreYouSure();

		/// <summary></summary>
		void PrintReturnToMainHall();

		/// <summary></summary>
		void PrintReallyWantToQuit();

		/// <summary></summary>
		void PrintChangeSaveName();

		/// <summary></summary>
		void PrintEnterSaveName();

		/// <summary></summary>
		void PrintEnterHintChoice();

		/// <summary></summary>
		void PrintAnotherHint();

		/// <summary></summary>
		void PrintNothingHappens();

		/// <summary></summary>
		void PrintNoObviousWayToDoThat();

		/// <summary></summary>
		void PrintDontHaveTheKey();

		/// <summary></summary>
		void PrintSettingsChanged();

		/// <summary></summary>
		void PrintGameRestored();

		/// <summary></summary>
		void PrintGameSaved();

		/// <summary></summary>
		void PrintGameNotSaved();

		/// <summary></summary>
		void PrintNoHintsAvailable();

		/// <summary></summary>
		void PrintNoHintsAvailableNow();

		/// <summary></summary>
		void PrintYourQuestion();

		/// <summary></summary>
		void PrintNothingToDrop();

		/// <summary></summary>
		void PrintNothingToGet();

		/// <summary></summary>
		void PrintAlreadyWearingArmor();

		/// <summary></summary>
		void PrintAlreadyWearingShield();

		/// <summary></summary>
		void PrintZapDirectHit();

		/// <summary></summary>
		void Stage();

		/// <summary></summary>
		/// <returns></returns>
		bool IsAllowedInRoom();

		/// <summary></summary>
		/// <returns></returns>
		bool ShouldAllowSkillGains();

		/// <summary></summary>
		/// <param name="room"></param>
		/// <param name="artifact"></param>
		/// <returns></returns>
		bool ShouldShowUnseenArtifacts(IRoom room, IArtifact artifact);

		/// <summary></summary>
		/// <returns></returns>
		string GetPrintedVerb();

		/// <summary></summary>
		/// <param name="monster"></param>
		/// <returns></returns>
		bool IsEnabled(IMonster monster);

		/// <summary></summary>
		/// <param name="prep"></param>
		/// <returns></returns>
		bool IsPrepEnabled(IPrep prep);

		/// <summary></summary>
		/// <param name="destCommand"></param>
		/// <param name="includeIobj"></param>
		void CopyCommandData(ICommand destCommand, bool includeIobj = true);

		/// <summary></summary>
		/// <param name="artifact"></param>
		/// <param name="printTaking"></param>
		void RedirectToGetCommand<T>(IArtifact artifact, bool printTaking = true) where T : class, ICommand;
	}
}
