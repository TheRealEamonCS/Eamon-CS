
// Command.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework;
using Eamon.Framework.Primitive.Classes;
using Eamon.Framework.Primitive.Enums;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Parsing;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Game.States;
using static EamonRT.Game.Plugin.PluginContext;

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

		public virtual void PrintTooHeavy(IArtifact artifact)
		{
			CommandImpl.PrintTooHeavy(artifact);
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

		public virtual void PrintTaken(IArtifact artifact)
		{
			CommandImpl.PrintTaken(artifact);
		}

		public virtual void PrintDropped(IArtifact artifact)
		{
			CommandImpl.PrintDropped(artifact);
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

		public virtual void PrintAlreadyBrokeIt(IArtifact artifact)
		{
			CommandImpl.PrintAlreadyBrokeIt(artifact);
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

		public virtual void PrintLightExtinguished(IArtifact artifact)
		{
			CommandImpl.PrintLightExtinguished(artifact);
		}

		public virtual void PrintCantReadyWeaponWithShield(IArtifact weapon, IArtifact shield)
		{
			CommandImpl.PrintCantReadyWeaponWithShield(weapon, shield);
		}

		public virtual void PrintPolitelyRefuses(IMonster monster)
		{
			CommandImpl.PrintPolitelyRefuses(monster);
		}

		public virtual void PrintGiveObjToActor(IArtifact artifact, IMonster monster)
		{
			CommandImpl.PrintGiveObjToActor(artifact, monster);
		}

		public virtual void PrintObjBelongsToActor(IArtifact artifact, IMonster monster)
		{
			CommandImpl.PrintObjBelongsToActor(artifact, monster);
		}

		public virtual void PrintOpenObjWithKey(IArtifact artifact, IArtifact key)
		{
			CommandImpl.PrintOpenObjWithKey(artifact, key);
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

		public virtual void PrintDontNeedTo()
		{
			CommandImpl.PrintDontNeedTo();
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

		public virtual bool IsAllowedInRoom()
		{
			return CommandImpl.IsAllowedInRoom();
		}

		public virtual bool ShouldAllowSkillGains()
		{
			return CommandImpl.ShouldAllowSkillGains();
		}

		public virtual bool ShouldShowUnseenArtifacts(IRoom room, IArtifact artifact)
		{
			return CommandImpl.ShouldShowUnseenArtifacts(room, artifact);
		}

		public override bool ShouldPreTurnProcess()
		{
			return CommandImpl.ShouldPreTurnProcess();
		}

		public override void Execute()
		{
			CommandImpl.Execute();
		}

		public override void PreExecute()
		{
			CommandImpl.PreExecute();
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
			CommandImpl = Globals.CreateInstance<ICommandImpl>(x =>
			{
				x.Command = this;
			});
		}
	}
}
