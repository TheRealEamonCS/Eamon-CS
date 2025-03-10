﻿
// Command.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Collections.Generic;
using System.Text;
using Eamon.Framework;
using Eamon.Framework.Args;
using Eamon.Framework.Primitive.Classes;
using Eamon.Framework.Primitive.Enums;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Parsing;
using EamonRT.Game.States;
using static EamonRT.Game.Plugin.Globals;

namespace EamonRT.Game.Commands
{
	public abstract class Command : State, ICommand
	{
		private ICommandImpl CommandImpl { get; set; }

		public virtual ICommandParser CommandParser
		{
			get
			{
				return CommandImpl.CommandParser;
			}

			set
			{
				CommandImpl.CommandParser = value;
			}
		}

		public virtual IMonster ActorMonster
		{
			get
			{
				return CommandImpl.ActorMonster;
			}

			set
			{
				CommandImpl.ActorMonster = value;
			}
		}

		public virtual IRoom ActorRoom
		{
			get
			{
				return CommandImpl.ActorRoom;
			}

			set
			{
				CommandImpl.ActorRoom = value;
			}
		}

		public virtual IGameBase Dobj
		{
			get
			{
				return CommandImpl.Dobj;
			}

			set
			{
				CommandImpl.Dobj = value;
			}
		}

		public virtual IArtifact DobjArtifact
		{
			get
			{
				return CommandImpl.DobjArtifact;
			}
		}

		public virtual IMonster DobjMonster
		{
			get
			{
				return CommandImpl.DobjMonster;
			}
		}

		public virtual IGameBase Iobj
		{
			get
			{
				return CommandImpl.Iobj;
			}

			set
			{
				CommandImpl.Iobj = value;
			}
		}

		public virtual IArtifact IobjArtifact
		{
			get
			{
				return CommandImpl.IobjArtifact;
			}
		}

		public virtual IMonster IobjMonster
		{
			get
			{
				return CommandImpl.IobjMonster;
			}
		}

		public virtual string[] Synonyms
		{
			get
			{
				return CommandImpl.Synonyms;
			}

			set
			{
				CommandImpl.Synonyms = value;
			}
		}

		public virtual long SortOrder
		{
			get
			{
				return CommandImpl.SortOrder;
			}

			set
			{
				CommandImpl.SortOrder = value;
			}
		}

		public virtual string Verb
		{
			get
			{
				return CommandImpl.Verb;
			}

			set
			{
				CommandImpl.Verb = value;
			}
		}

		public virtual string ParserMatchName
		{
			get
			{
				return CommandImpl.ParserMatchName;
			}

			set
			{
				CommandImpl.ParserMatchName = value;
			}
		}

		public virtual IPrep Prep
		{
			get
			{
				return CommandImpl.Prep;
			}

			set
			{
				CommandImpl.Prep = value;
			}
		}

		public virtual CommandType Type
		{
			get
			{
				return CommandImpl.Type;
			}

			set
			{
				CommandImpl.Type = value;
			}
		}

		public virtual ContainerType ContainerType
		{
			get
			{
				return CommandImpl.ContainerType;
			}

			set
			{
				CommandImpl.ContainerType = value;
			}
		}

		public virtual bool GetCommandCalled
		{
			get
			{
				return CommandImpl.GetCommandCalled;
			}

			set
			{
				CommandImpl.GetCommandCalled = value;
			}
		}

		public virtual bool IsNew
		{
			get
			{
				return CommandImpl.IsNew;
			}

			set
			{
				CommandImpl.IsNew = value;
			}
		}

		public virtual bool IsListed
		{
			get
			{
				return CommandImpl.IsListed;
			}

			set
			{
				CommandImpl.IsListed = value;
			}
		}

		public virtual bool IsSentenceParserEnabled
		{
			get
			{
				return CommandImpl.IsSentenceParserEnabled;
			}

			set
			{
				CommandImpl.IsSentenceParserEnabled = value;
			}
		}

		public virtual bool IsDobjPrepEnabled
		{
			get
			{
				return CommandImpl.IsDobjPrepEnabled;
			}

			set
			{
				CommandImpl.IsDobjPrepEnabled = value;
			}
		}

		public virtual bool IsIobjEnabled
		{
			get
			{
				return CommandImpl.IsIobjEnabled;
			}

			set
			{
				CommandImpl.IsIobjEnabled = value;
			}
		}

		public virtual bool IsDarkEnabled
		{
			get
			{
				return CommandImpl.IsDarkEnabled;
			}

			set
			{
				CommandImpl.IsDarkEnabled = value;
			}
		}
		
		public virtual bool IsPlayerEnabled
		{
			get
			{
				return CommandImpl.IsPlayerEnabled;
			}

			set
			{
				CommandImpl.IsPlayerEnabled = value;
			}
		}
		
		public virtual bool IsMonsterEnabled
		{
			get
			{
				return CommandImpl.IsMonsterEnabled;
			}

			set
			{
				CommandImpl.IsMonsterEnabled = value;
			}
		}

		public virtual void BuildPrepContainerYouSeePrefix(IArtifact artifact, ContainerType containerType, bool showCharOwned)
		{
			CommandImpl.BuildPrepContainerYouSeePrefix(artifact, containerType, showCharOwned);
		}

		public virtual void PrintCantVerbObj(IGameBase obj)
		{
			CommandImpl.PrintCantVerbObj(obj);
		}

		public virtual void PrintCantVerbIt(IArtifact artifact)
		{
			CommandImpl.PrintCantVerbIt(artifact);
		}

		public virtual void PrintCantVerbThat(IArtifact artifact)
		{
			CommandImpl.PrintCantVerbThat(artifact);
		}

		public virtual void PrintDoYouMeanObj1OrObj2(IGameBase obj1, IGameBase obj2)
		{
			CommandImpl.PrintDoYouMeanObj1OrObj2(obj1, obj2);
		}

		public virtual void PrintWhyAttack(IGameBase obj)
		{
			CommandImpl.PrintWhyAttack(obj);
		}

		public virtual void PrintTakingFirst(IArtifact artifact)
		{
			CommandImpl.PrintTakingFirst(artifact);
		}

		public virtual void PrintRemovingFirst(IArtifact artifact)
		{
			CommandImpl.PrintRemovingFirst(artifact);
		}

		public virtual void PrintBestLeftAlone(IArtifact artifact)
		{
			CommandImpl.PrintBestLeftAlone(artifact);
		}

		public virtual void PrintTooHeavy(IArtifact artifact, bool getAll = false)
		{
			CommandImpl.PrintTooHeavy(artifact, getAll);
		}

		public virtual void PrintMustBeFreed(IArtifact artifact)
		{
			CommandImpl.PrintMustBeFreed(artifact);
		}

		public virtual void PrintMustFirstOpen(IArtifact artifact)
		{
			CommandImpl.PrintMustFirstOpen(artifact);
		}

		public virtual void PrintMustFirstClose(IArtifact artifact)
		{
			CommandImpl.PrintMustFirstClose(artifact);
		}

		public virtual void PrintWorn(IArtifact artifact)
		{
			CommandImpl.PrintWorn(artifact);
		}

		public virtual void PrintRemoved(IArtifact artifact)
		{
			CommandImpl.PrintRemoved(artifact);
		}

		public virtual void PrintOpened(IArtifact artifact)
		{
			CommandImpl.PrintOpened(artifact);
		}

		public virtual void PrintClosed(IArtifact artifact)
		{
			CommandImpl.PrintClosed(artifact);
		}

		public virtual void PrintReceived(IArtifact artifact)
		{
			CommandImpl.PrintReceived(artifact);
		}

		public virtual void PrintRetrieved(IArtifact artifact)
		{
			CommandImpl.PrintRetrieved(artifact);
		}

		public virtual void PrintTaken(IArtifact artifact, bool getAll = false)
		{
			CommandImpl.PrintTaken(artifact, getAll);
		}

		public virtual void PrintDropped(IArtifact artifact)
		{
			CommandImpl.PrintDropped(artifact);
		}

		public virtual void PrintReadied(IArtifact artifact)
		{
			CommandImpl.PrintReadied(artifact);
		}

		public virtual void PrintNotOpen(IArtifact artifact)
		{
			CommandImpl.PrintNotOpen(artifact);
		}

		public virtual void PrintAlreadyOpen(IArtifact artifact)
		{
			CommandImpl.PrintAlreadyOpen(artifact);
		}

		public virtual void PrintWontOpen(IArtifact artifact)
		{
			CommandImpl.PrintWontOpen(artifact);
		}

		public virtual void PrintWontFit(IArtifact artifact)
		{
			CommandImpl.PrintWontFit(artifact);
		}

		public virtual void PrintFull(IArtifact artifact)
		{
			CommandImpl.PrintFull(artifact);
		}

		public virtual void PrintOutOfSpace(IArtifact artifact)
		{
			CommandImpl.PrintOutOfSpace(artifact);
		}

		public virtual void PrintLocked(IArtifact artifact)
		{
			CommandImpl.PrintLocked(artifact);
		}

		public virtual void PrintBrokeIt(IArtifact artifact)
		{
			CommandImpl.PrintBrokeIt(artifact);
		}

		public virtual void PrintHaveToForceOpen(IArtifact artifact)
		{
			CommandImpl.PrintHaveToForceOpen(artifact);
		}

		public virtual void PrintWearingRemoveFirst(IArtifact artifact)
		{
			CommandImpl.PrintWearingRemoveFirst(artifact);
		}

		public virtual void PrintWearingRemoveFirst01(IArtifact artifact)
		{
			CommandImpl.PrintWearingRemoveFirst01(artifact);
		}

		public virtual void PrintCantWearShieldWithWeapon(IArtifact shield, IArtifact weapon)
		{
			CommandImpl.PrintCantWearShieldWithWeapon(shield, weapon);
		}

		public virtual void PrintContainerNotEmpty(IArtifact artifact, ContainerType containerType, bool isPlural)
		{
			CommandImpl.PrintContainerNotEmpty(artifact, containerType, isPlural);
		}

		public virtual void PrintVerbItAll(IArtifact artifact)
		{
			CommandImpl.PrintVerbItAll(artifact);
		}

		public virtual void PrintNoneLeft(IArtifact artifact)
		{
			CommandImpl.PrintNoneLeft(artifact);
		}

		public virtual void PrintOkay()
		{
			CommandImpl.PrintOkay();
		}

		public virtual void PrintOkay(IArtifact artifact)
		{
			CommandImpl.PrintOkay(artifact);
		}

		public virtual void PrintFeelBetter(IArtifact artifact)
		{
			CommandImpl.PrintFeelBetter(artifact);
		}

		public virtual void PrintFeelWorse(IArtifact artifact)
		{
			CommandImpl.PrintFeelWorse(artifact);
		}

		public virtual void PrintTryDifferentCommand(IArtifact artifact)
		{
			CommandImpl.PrintTryDifferentCommand(artifact);
		}

		public virtual void PrintNotWeapon(IArtifact artifact)
		{
			CommandImpl.PrintNotWeapon(artifact);
		}

		public virtual void PrintNotReadyableWeapon(IArtifact artifact)
		{
			CommandImpl.PrintNotReadyableWeapon(artifact);
		}

		public virtual void PrintNotWhileCarryingObj(IArtifact artifact)
		{
			CommandImpl.PrintNotWhileCarryingObj(artifact);
		}

		public virtual void PrintNotWhileWearingObj(IArtifact artifact)
		{
			CommandImpl.PrintNotWhileWearingObj(artifact);
		}

		public virtual void PrintWontLight(IArtifact artifact)
		{
			CommandImpl.PrintWontLight(artifact);
		}

		public virtual void PrintLightObj(IArtifact artifact)
		{
			CommandImpl.PrintLightObj(artifact);
		}

		public virtual void PrintExtinguishObj(IArtifact artifact)
		{
			CommandImpl.PrintExtinguishObj(artifact);
		}

		public virtual void PrintLightExtinguished(IArtifact artifact)
		{
			CommandImpl.PrintLightExtinguished(artifact);
		}

		public virtual void PrintAlreadyWearingObj(IArtifact artifact)
		{
			CommandImpl.PrintAlreadyWearingObj(artifact);
		}

		public virtual void PrintWhamHitObj(IArtifact artifact)
		{
			CommandImpl.PrintWhamHitObj(artifact);
		}

		public virtual void PrintFullDesc(IArtifact artifact, bool showName, bool showVerboseName)
		{
			CommandImpl.PrintFullDesc(artifact, showName, showVerboseName);
		}

		public virtual void PrintObjAmountLeft(IArtifact artifact, long objAmount, bool objEdible)
		{
			CommandImpl.PrintObjAmountLeft(artifact, objAmount, objEdible);
		}

		public virtual void PrintPrepContainerYouSee(IArtifact artifact, IList<IArtifact> containerArtifactList, ContainerType containerType, bool showCharOwned, IRecordNameListArgs recordNameListArgs = null)
		{
			CommandImpl.PrintPrepContainerYouSee(artifact, containerArtifactList, containerType, showCharOwned, recordNameListArgs);
		}

		public virtual void PrintNothingPrepContainer(IArtifact artifact, ContainerType containerType, bool showCharOwned)
		{
			CommandImpl.PrintNothingPrepContainer(artifact, containerType, showCharOwned);
		}

		public virtual void PrintAttemptingToFlee(IArtifact artifact, Direction direction)
		{
			CommandImpl.PrintAttemptingToFlee(artifact, direction);
		}

		public virtual void PrintCantReadyWeaponWithShield(IArtifact weapon, IArtifact shield)
		{
			CommandImpl.PrintCantReadyWeaponWithShield(weapon, shield);
		}

		public virtual void PrintPolitelyRefuses(IMonster monster)
		{
			CommandImpl.PrintPolitelyRefuses(monster);
		}

		public virtual void PrintTakesTheMoney(IMonster monster)
		{
			CommandImpl.PrintTakesTheMoney(monster);
		}

		public virtual void PrintWontLetYou(IMonster monster)
		{
			CommandImpl.PrintWontLetYou(monster);
		}

		public virtual void PrintHealthImproves(IMonster monster)
		{
			CommandImpl.PrintHealthImproves(monster);
		}

		public virtual void PrintAssumeCombatStance(IMonster monster)
		{
			CommandImpl.PrintAssumeCombatStance(monster);
		}

		public virtual void PrintAssumeCombatStance01(IMonster monster)
		{
			CommandImpl.PrintAssumeCombatStance01(monster);
		}

		public virtual void PrintCombatStance(IMonster monster)
		{
			CommandImpl.PrintCombatStance(monster);
		}

		public virtual void PrintHaventSavedGameYet(IMonster monster)
		{
			CommandImpl.PrintHaventSavedGameYet(monster);
		}

		public virtual void PrintFullDesc(IMonster monster, bool showName, bool showVerboseName)
		{
			CommandImpl.PrintFullDesc(monster, showName, showVerboseName);
		}

		public virtual void PrintHealthStatus(IMonster monster, bool includeUninjuredGroupMonsters)
		{
			CommandImpl.PrintHealthStatus(monster, includeUninjuredGroupMonsters);
		}

		public virtual void PrintGiveGoldPiecesTo(IMonster monster, long goldAmount)
		{
			CommandImpl.PrintGiveGoldPiecesTo(monster, goldAmount);
		}

		public virtual void PrintActorIsWearing(IMonster monster, IList<IArtifact> monsterWornArtifactList, IRecordNameListArgs recordNameListArgs = null)
		{
			CommandImpl.PrintActorIsWearing(monster, monsterWornArtifactList, recordNameListArgs);
		}

		public virtual void PrintActorIsCarrying(IMonster monster, IList<IArtifact> monsterCarriedArtifactList, IRecordNameListArgs recordNameListArgs = null)
		{
			CommandImpl.PrintActorIsCarrying(monster, monsterCarriedArtifactList, recordNameListArgs);
		}

		public virtual void PrintOpensConsumesAndHandsBack(IArtifact artifact, IMonster monster, bool objOpened, bool objEdible)
		{
			CommandImpl.PrintOpensConsumesAndHandsBack(artifact, monster, objOpened, objEdible);
		}

		public virtual void PrintConsumesItAll(IArtifact artifact, IMonster monster, bool objOpened)
		{
			CommandImpl.PrintConsumesItAll(artifact, monster, objOpened);
		}

		public virtual void PrintConsumesItAllHandsBack(IArtifact artifact, IMonster monster, bool objOpened)
		{
			CommandImpl.PrintConsumesItAllHandsBack(artifact, monster, objOpened);
		}

		public virtual void PrintGiveObjToActor(IArtifact artifact, IMonster monster)
		{
			CommandImpl.PrintGiveObjToActor(artifact, monster);
		}

		public virtual void PrintObjBelongsToActor(IArtifact artifact, IMonster monster)
		{
			CommandImpl.PrintObjBelongsToActor(artifact, monster);
		}

		public virtual void PrintFreeActorWithKey(IMonster monster, IArtifact key)
		{
			CommandImpl.PrintFreeActorWithKey(monster, key);
		}

		public virtual void PrintOpenObjWithKey(IArtifact artifact, IArtifact key)
		{
			CommandImpl.PrintOpenObjWithKey(artifact, key);
		}

		public virtual void PrintPutObjPrepContainer(IArtifact artifact, IArtifact container, ContainerType containerType)
		{
			CommandImpl.PrintPutObjPrepContainer(artifact, container, containerType);
		}

		public virtual void PrintActorRemovesObjPrepContainer(IMonster monster, IArtifact artifact, IArtifact container, ContainerType containerType, bool omitWeightCheck)
		{
			CommandImpl.PrintActorRemovesObjPrepContainer(monster, artifact, container, containerType, omitWeightCheck);
		}

		public virtual void PrintActorPicksUpObj(IMonster monster, IArtifact artifact)
		{
			CommandImpl.PrintActorPicksUpObj(monster, artifact);
		}

		public virtual void PrintActorReadiesObj(IMonster monster, IArtifact artifact)
		{
			CommandImpl.PrintActorReadiesObj(monster, artifact);
		}

		public virtual void PrintActorRemovesObjPrepContainer01(IMonster monster, IArtifact artifact, IArtifact container, ContainerType containerType, bool omitWeightCheck)
		{
			CommandImpl.PrintActorRemovesObjPrepContainer01(monster, artifact, container, containerType, omitWeightCheck);
		}

		public virtual void PrintActorPicksUpWeapon(IMonster monster)
		{
			CommandImpl.PrintActorPicksUpWeapon(monster);
		}

		public virtual void PrintActorReadiesWeapon(IMonster monster)
		{
			CommandImpl.PrintActorReadiesWeapon(monster);
		}

		public virtual void PrintBortVisitArtifact(IRoom room, IArtifact artifact)
		{
			CommandImpl.PrintBortVisitArtifact(room, artifact);
		}

		public virtual void PrintBortVisitMonster(IRoom room, IMonster monster)
		{
			CommandImpl.PrintBortVisitMonster(room, monster);
		}

		public virtual void PrintBortVisitRoom(IRoom room)
		{
			CommandImpl.PrintBortVisitRoom(room);
		}

		public virtual void PrintBortRecallArtifact(IRoom room, IArtifact artifact)
		{
			CommandImpl.PrintBortRecallArtifact(room, artifact);
		}

		public virtual void PrintBortRecallMonster(IRoom room, IMonster monster)
		{
			CommandImpl.PrintBortRecallMonster(room, monster);
		}

		public virtual void PrintBortArtifactRoomInvalid(IArtifact artifact)
		{
			CommandImpl.PrintBortArtifactRoomInvalid(artifact);
		}

		public virtual void PrintBortMonsterRoomInvalid(IMonster monster)
		{
			CommandImpl.PrintBortMonsterRoomInvalid(monster);
		}

		public virtual void PrintBortArtifactInvalid()
		{
			CommandImpl.PrintBortArtifactInvalid();
		}

		public virtual void PrintBortMonsterInvalid()
		{
			CommandImpl.PrintBortMonsterInvalid();
		}

		public virtual void PrintBortRoomInvalid()
		{
			CommandImpl.PrintBortRoomInvalid();
		}

		public virtual void PrintHintQuestion(long hintNum, string question)
		{
			CommandImpl.PrintHintQuestion(hintNum, question);
		}

		public virtual void PrintHintQuestion01(string question)
		{
			CommandImpl.PrintHintQuestion01(question);
		}

		public virtual void PrintHintAnswer(string answer, StringBuilder buf)
		{
			CommandImpl.PrintHintAnswer(answer, buf);
		}

		public virtual void PrintSayText(string printedPhrase)
		{
			CommandImpl.PrintSayText(printedPhrase);
		}

		public virtual void PrintBortUsage()
		{
			CommandImpl.PrintBortUsage();
		}

		public virtual void PrintSettingsUsage()
		{
			CommandImpl.PrintSettingsUsage();
		}

		public virtual void PrintNotEnoughGold()
		{
			CommandImpl.PrintNotEnoughGold();
		}

		public virtual void PrintMustFirstReadyWeapon()
		{
			CommandImpl.PrintMustFirstReadyWeapon();
		}

		public virtual void PrintDontHaveItNotHere()
		{
			CommandImpl.PrintDontHaveItNotHere();
		}

		public virtual void PrintDontHaveIt()
		{
			CommandImpl.PrintDontHaveIt();
		}

		public virtual void PrintDontHaveIt02(IArtifact artifact)
		{
			CommandImpl.PrintDontHaveIt02(artifact);
		}

		public virtual void PrintDontNeedTo()
		{
			CommandImpl.PrintDontNeedTo();
		}

		public virtual void PrintDontNeedTo02(IArtifact artifact)
		{
			CommandImpl.PrintDontNeedTo02(artifact);
		}

		public virtual void PrintCantDoThat()
		{
			CommandImpl.PrintCantDoThat();
		}

		public virtual void PrintCantVerbThat()
		{
			CommandImpl.PrintCantVerbThat();
		}

		public virtual void PrintCantVerbHere()
		{
			CommandImpl.PrintCantVerbHere();
		}

		public virtual void PrintBeMoreSpecific()
		{
			CommandImpl.PrintBeMoreSpecific();
		}

		public virtual void PrintNobodyHereByThatName()
		{
			CommandImpl.PrintNobodyHereByThatName();
		}

		public virtual void PrintNothingHereByThatName()
		{
			CommandImpl.PrintNothingHereByThatName();
		}

		public virtual void PrintYouSeeNothingSpecial()
		{
			CommandImpl.PrintYouSeeNothingSpecial();
		}

		public virtual void PrintDontBeAbsurd()
		{
			CommandImpl.PrintDontBeAbsurd();
		}

		public virtual void PrintCalmDown()
		{
			CommandImpl.PrintCalmDown();
		}

		public virtual void PrintNoPlaceToGo()
		{
			CommandImpl.PrintNoPlaceToGo();
		}

		public virtual void PrintAttackNonEnemy()
		{
			CommandImpl.PrintAttackNonEnemy();
		}

		public virtual void PrintAreYouSure()
		{
			CommandImpl.PrintAreYouSure();
		}

		public virtual void PrintReturnToMainHall()
		{
			CommandImpl.PrintReturnToMainHall();
		}

		public virtual void PrintReallyWantToQuit()
		{
			CommandImpl.PrintReallyWantToQuit();
		}

		public virtual void PrintChangeSaveName()
		{
			CommandImpl.PrintChangeSaveName();
		}

		public virtual void PrintEnterSaveName()
		{
			CommandImpl.PrintEnterSaveName();
		}

		public virtual void PrintEnterHintChoice()
		{
			CommandImpl.PrintEnterHintChoice();
		}

		public virtual void PrintAnotherHint()
		{
			CommandImpl.PrintAnotherHint();
		}

		public virtual void PrintNothingHappens()
		{
			CommandImpl.PrintNothingHappens();
		}

		public virtual void PrintNoObviousWayToDoThat()
		{
			CommandImpl.PrintNoObviousWayToDoThat();
		}

		public virtual void PrintDontHaveTheKey()
		{
			CommandImpl.PrintDontHaveTheKey();
		}

		public virtual void PrintSettingsChanged()
		{
			CommandImpl.PrintSettingsChanged();
		}

		public virtual void PrintGameRestored()
		{
			CommandImpl.PrintGameRestored();
		}

		public virtual void PrintGameSaved()
		{
			CommandImpl.PrintGameSaved();
		}

		public virtual void PrintGameNotSaved()
		{
			CommandImpl.PrintGameNotSaved();
		}

		public virtual void PrintNoHintsAvailable()
		{
			CommandImpl.PrintNoHintsAvailable();
		}

		public virtual void PrintNoHintsAvailableNow()
		{
			CommandImpl.PrintNoHintsAvailableNow();
		}

		public virtual void PrintYourQuestion()
		{
			CommandImpl.PrintYourQuestion();
		}

		public virtual void PrintNothingToDrop()
		{
			CommandImpl.PrintNothingToDrop();
		}

		public virtual void PrintNothingToGet()
		{
			CommandImpl.PrintNothingToGet();
		}

		public virtual void PrintAlreadyWearingArmor()
		{
			CommandImpl.PrintAlreadyWearingArmor();
		}

		public virtual void PrintAlreadyWearingShield()
		{
			CommandImpl.PrintAlreadyWearingShield();
		}

		public virtual void PrintZapDirectHit()
		{
			CommandImpl.PrintZapDirectHit();
		}

		public virtual bool IsAllowedInRoom()
		{
			return CommandImpl.IsAllowedInRoom();
		}

		public virtual bool ShouldAllowSkillGains()
		{
			return CommandImpl.ShouldAllowSkillGains();
		}

		public virtual bool ShouldAllowRedirectToGetCommand()
		{
			return CommandImpl.ShouldAllowRedirectToGetCommand();
		}

		public virtual bool ShouldShowUnseenArtifacts(IRoom room, IArtifact artifact)
		{
			return CommandImpl.ShouldShowUnseenArtifacts(room, artifact);
		}

		public override void Stage()
		{
			CommandImpl.Stage();
		}

		public override void Execute()
		{
			CommandImpl.Execute();
		}

		public virtual void ExecuteForPlayer()
		{
			CommandImpl.ExecuteForPlayer();
		}

		public virtual void ExecuteForMonster()
		{
			CommandImpl.ExecuteForMonster();
		}

		public virtual string GetPrintedVerb()
		{
			return CommandImpl.GetPrintedVerb();
		}

		public virtual bool IsEnabled(IMonster monster)
		{
			return CommandImpl.IsEnabled(monster);
		}

		public virtual bool IsPrepEnabled(IPrep prep)
		{
			return CommandImpl.IsPrepEnabled(prep);
		}

		public virtual void CopyCommandData(ICommand destCommand, bool includeIobj = true)
		{
			CommandImpl.CopyCommandData(destCommand, includeIobj);
		}

		public virtual void RedirectToGetCommand<T>(IArtifact artifact, bool printTaking = true) where T : class, ICommand
		{
			CommandImpl.RedirectToGetCommand<T>(artifact, printTaking);
		}

		public Command()
		{
			CommandImpl = gEngine.CreateInstance<ICommandImpl>(x =>
			{
				x.Command = this;
			});
		}
	}
}
