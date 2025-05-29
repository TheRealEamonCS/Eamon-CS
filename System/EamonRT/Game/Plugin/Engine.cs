
// Engine.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Eamon;
using Eamon.Framework;
using Eamon.Framework.Args;
using Eamon.Framework.Primitive.Classes;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Extensions;
using Eamon.Game.Utilities;
using Eamon.ThirdParty;
using EamonRT.Framework;
using EamonRT.Framework.Args;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Components;
using EamonRT.Framework.Parsing;
using EamonRT.Framework.Plugin;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.Globals;

namespace EamonRT.Game.Plugin
{
	public class Engine : EamonDD.Game.Plugin.Engine, IEngine
	{
		#region Explicit Properties

		StringBuilder IEngine.Buf { get; set; }

		StringBuilder IEngine.Buf01 { get; set; }

		#endregion

		#region Public Properties

		public virtual string PageSep { get; protected set; } = "@@PB";

		/*
		public virtual HashSet<string> IgnoredTokenHashSet { get; set; }
		*/

		public virtual IList<ICommand> CommandList { get; set; }

		public virtual IList<ICommand> LastCommandList { get; set; }

		public virtual IList<Action> MiscEventFuncList { get; set; }

		public virtual IList<Action> MiscEventFuncList02 { get; set; }

		public virtual IList<Action> MiscEventFuncList03 { get; set; }

		public virtual IList<Action> SkillIncreaseFuncList { get; set; }

		public virtual IList<long> LoopMonsterUidList { get; set; }

		public virtual long ActionListCounter { get; set; }

		public virtual long PauseCombatActionsCounter { get; set; }

		public virtual long LoopMonsterUidListIndex { get; set; }

		public virtual long LoopMonsterUid { get; set; }

		public virtual long LoopMemberNumber { get; set; }

		public virtual long LoopAttackNumber { get; set; }

		public virtual long LoopGroupCount { get; set; }

		public virtual long LoopFailedMoveMemberCount { get; set; }

		public virtual IMonster LoopLastDobjMonster { get; set; }

		public virtual IIntroStory IntroStory { get; set; }

		public virtual IMainLoop MainLoop { get; set; }

		public virtual ISentenceParser SentenceParser { get; set; }

		public virtual ICommandParser CommandParser { get; set; }

		public virtual IState InitialState { get; set; }

		public virtual IState CurrState { get; set; }

		public virtual IState NextState { get; set; }

		public virtual IGameState GameState { get; set; }

		public virtual ICharacter Character { get; set; }

		public virtual ExitType ExitType { get; set; }

		public virtual string CommandPrompt { get; set; }

		public virtual ICommand CurrCommand
		{
			get
			{
				return CurrState as ICommand;
			}
		}

		public virtual ICommand NextCommand
		{
			get
			{
				return NextState as ICommand;
			}
		}

		public virtual ICommand LastCommand
		{
			get
			{
				return LastCommandList.Count > 0 ? LastCommandList[LastCommandList.Count - 1] : null;
			}
		}

		public virtual bool CommandPromptSeen { get; set; }

		public virtual bool ShouldPreTurnProcess { get; set; }

		public virtual bool PauseCombatAfterSkillGains { get; set; }

		public virtual bool UseRevealContentMonsterTheName { get; set; }

		public virtual bool RtSuppressPostInputSleep { get; set; }

		public virtual bool PlayerMoved { get; set; }

		public virtual bool GameRunning
		{
			get
			{
				return ExitType == ExitType.None;
			}
		}

		public virtual bool DeleteGameStateAfterLoop
		{
			get
			{
				return ExitType == ExitType.GoToMainHall || ExitType == ExitType.StartOver || ExitType == ExitType.FinishAdventure || ExitType == ExitType.DeleteCharacter;
			}
		}

		public virtual bool StartOver
		{
			get
			{
				return ExitType == ExitType.StartOver;
			}
		}

		public virtual bool ErrorExit
		{
			get
			{
				return ExitType == ExitType.Error;
			}
		}

		public virtual bool ExportCharacterGoToMainHall
		{
			get
			{
				return ExitType == ExitType.GoToMainHall || ExitType == ExitType.FinishAdventure;
			}
		}

		public virtual bool ExportCharacter
		{
			get
			{
				return ExitType == ExitType.FinishAdventure;
			}
		}

		public virtual bool DeleteCharacter
		{
			get
			{
				return ExitType == ExitType.DeleteCharacter;
			}
		}

		public override bool EnableMutateProperties
		{
			get
			{
				return base.EnableMutateProperties && GameState != null;
			}
		}

		public virtual long StartRoom { get; set; }

		public virtual long NumSaveSlots { get; set; }

		public virtual long ScaledHardinessUnarmedMaxDamage { get; set; }

		public virtual double ScaledHardinessMaxDamageDivisor { get; set; }

		public virtual bool EnforceMonsterWeightLimits { get; set; }

		public virtual bool UseMonsterScaledHardinessValues { get; set; }

		public virtual bool AutoDisplayUnseenArtifactDescs { get; set; }

		public virtual bool ExposeContainersRecursively { get; set; }

		public virtual PoundCharPolicy PoundCharPolicy { get; set; }

		public virtual PercentCharPolicy PercentCharPolicy { get; set; }

		#endregion

		#region Public Methods

		public override RetCode LoadPluginClassMappings()
		{
			RetCode rc;

			rc = base.LoadPluginClassMappings();

			if (rc != RetCode.Success)
			{
				goto Cleanup;
			}

			rc = LoadPluginClassMappings01(Assembly.GetExecutingAssembly());

		Cleanup:

			return rc;
		}

		public override void InitSystem()
		{
			base.InitSystem();

			CommandList = new List<ICommand>();

			LastCommandList = new List<ICommand>();

			MiscEventFuncList = new List<Action>();

			MiscEventFuncList02 = new List<Action>();

			MiscEventFuncList03 = new List<Action>();

			SkillIncreaseFuncList = new List<Action>();

			IntroStory = CreateInstance<IIntroStory>();

			MainLoop = CreateInstance<IMainLoop>();

			SentenceParser = CreateInstance<ISentenceParser>();

			CommandParser = CreateInstance<ICommandParser>();

			CommandPrompt = EnableScreenReaderMode ? "Your command: " : "> ";

			ShouldPreTurnProcess = true;
		}

		public override void ResetProperties(PropertyResetCode resetCode)
		{
			base.ResetProperties(resetCode);

			switch (resetCode)
			{
				case PropertyResetCode.All:

					LastCommandList.Clear();

					MiscEventFuncList.Clear();

					MiscEventFuncList02.Clear();

					MiscEventFuncList03.Clear();

					SkillIncreaseFuncList.Clear();

					ActionListCounter = 0;

					PauseCombatActionsCounter = 0;

					LoopFailedMoveMemberCount = 0;

					SentenceParser.LastInputStr = "";

					SentenceParser.Clear();

					CommandParser.LastInputStr = "";

					CommandParser.LastHimNameStr = "";

					CommandParser.LastHerNameStr = "";

					CommandParser.LastItNameStr = "";

					CommandParser.LastThemNameStr = "";

					CommandParser.Clear();

					//CommandPromptSeen = false;

					ShouldPreTurnProcess = true;

					PauseCombatAfterSkillGains = false;

					PlayerMoved = false;

					break;

				case PropertyResetCode.RestoreGame:

					MiscEventFuncList.Clear();

					MiscEventFuncList02.Clear();

					MiscEventFuncList03.Clear();

					SkillIncreaseFuncList.Clear();

					ActionListCounter = 0;

					PauseCombatActionsCounter = 0;

					SentenceParser.LastInputStr = "";

					SentenceParser.Clear();

					CommandParser.LastInputStr = "";

					CommandParser.LastHimNameStr = "";

					CommandParser.LastHerNameStr = "";

					CommandParser.LastItNameStr = "";

					CommandParser.LastThemNameStr = "";

					CommandParser.Clear();

					PauseCombatAfterSkillGains = false;

					break;

				case PropertyResetCode.SwitchContext:

					MiscEventFuncList.Clear();

					MiscEventFuncList02.Clear();

					MiscEventFuncList03.Clear();

					SkillIncreaseFuncList.Clear();

					ActionListCounter = 0;

					PauseCombatActionsCounter = 0;

					SentenceParser.Clear();

					CommandParser.Clear();

					PauseCombatAfterSkillGains = false;

					break;

				case PropertyResetCode.RevealContainerContents:

					// Do nothing

					break;
			}
		}

		public override bool ShouldSleepAfterInput(StringBuilder buf, char inputFillChar)
		{
			return GameState == null || BortCommand ? base.ShouldSleepAfterInput(buf, inputFillChar) : RtSuppressPostInputSleep ? false : true;
		}

		public virtual void PrintPlayerRoom(IRoom room)
		{
			Debug.Assert(room != null);

			gEngine.Buf.Clear();

			var rc = room.IsLit() ? room.BuildPrintedFullDesc(gEngine.Buf, verboseRoomDesc: GameState.Vr, verboseMonsterDesc: GameState.Vm, verboseArtifactDesc: GameState.Va, verboseNames: GameState.Vn) : room.BuildPrintedTooDarkToSeeDesc(gEngine.Buf);

			Debug.Assert(IsSuccess(rc));

			Out.Write("{0}", gEngine.Buf);
		}

		public virtual void PrintToWhom()
		{
			Out.Write("{0}To whom? ", Environment.NewLine);
		}

		public virtual void PrintFromWhom()
		{
			Out.Write("{0}From whom? ", Environment.NewLine);
		}

		public virtual void PrintVerbWhoOrWhat(ICommand command)
		{
			Debug.Assert(command != null);

			Out.Write("{0}{1} who or what? ", Environment.NewLine, command.Verb.FirstCharToUpper());
		}

		public virtual void PrintVerbPrepWhoOrWhat(ICommand command)
		{
			Debug.Assert(command != null);

			Out.Write("{0}{1} {2}who or what? ", Environment.NewLine, command.Verb.FirstCharToUpper(), command.IsDobjPrepEnabled && command.Prep != null && Enum.IsDefined(typeof(ContainerType), command.Prep.ContainerType) ? EvalContainerType(command.Prep.ContainerType, "inside ", "on ", "under ", "behind ") : "");
		}

		public virtual void PrintFromPrepWhat(ICommand command)
		{
			Debug.Assert(command != null);

			Out.Write("{0}From {1}what? ", Environment.NewLine, Enum.IsDefined(typeof(ContainerType), command.ContainerType) ? EvalContainerType(command.ContainerType, "inside ", "on ", "under ", "behind ") : "");
		}

		public virtual void PrintPutObjPrepWhat(ICommand command, IArtifact artifact)
		{
			Debug.Assert(command != null && artifact != null);

			Out.Write("{0}Put {1} {2} what? ", Environment.NewLine, artifact.EvalPlural("it", "them"), Enum.IsDefined(typeof(ContainerType), command.ContainerType) ? EvalContainerType(command.ContainerType, "inside", "on", "under", "behind") : "in");
		}

		public virtual void PrintUseObjOnWhoOrWhat(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			Out.Write("{0}Use {1} on who or what? ", Environment.NewLine, artifact.EvalPlural("it", "them"));
		}

		public virtual void PrintWhamHitObj(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			Out.Print("Wham!  You hit {0}!", artifact.GetTheName());
		}

		public virtual void PrintMonsterAlive(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			Out.Print("{0} {1} alive!", artifact.GetTheName(true), artifact.EvalPlural("is", "are"));
		}

		public virtual void PrintLightOut(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			Out.Print("{0} goes out.", artifact.GetTheName(true));
		}

		public virtual void PrintDeadBodyComesToLife(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			Out.Print("{0} {1}", artifact.GetTheName(true), IsRulesetVersion(5, 62) ?
				string.Format("come{0} alive!", artifact.EvalPlural("s", "")) :
				string.Format("come{0} to life!", artifact.EvalPlural("s", "")));
		}

		public virtual void PrintArtifactVanishes(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			Out.Print("{0} vanish{1}!", artifact.GetTheName(true), artifact.EvalPlural("es", ""));
		}

		public virtual void PrintArtifactBreaks(IRoom room, IMonster monster, IArtifact artifact, bool prependNewLine = false)
		{
			Debug.Assert(room != null && monster != null && artifact != null);

			if (monster.IsCharacterMonster() || room.IsViewable())
			{
				Out.Print("{0}{1} break{2}!", prependNewLine ? Environment.NewLine : "", artifact.GetTheName(true), artifact.EvalPlural("s", ""));
			}
			else
			{
				Out.Print("{0}Something breaks!", prependNewLine ? Environment.NewLine : "");
			}
		}

		public virtual void PrintEnterExtinguishLightChoice(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			Out.Write("{0}It's not dark here.  Extinguish {1} (Y/N): ", Environment.NewLine, artifact.GetTheName());
		}

		public virtual void PrintArtifactIsWorth(IArtifact artifact, long goldAmount)
		{
			Debug.Assert(artifact != null);

			if (!IsRulesetVersion(5, 62))
			{
				if (goldAmount > 0)
				{
					gEngine.Buf01.SetFormat("{0} gold piece{1}", goldAmount, goldAmount != 1 ? "s" : "");
				}
				else
				{
					gEngine.Buf01.SetFormat("nothing");
				}

				var ac = artifact.Drinkable;

				Out.Write("{0}{1}{2} {3} worth {4}.",
					Environment.NewLine,
					artifact.GetTheName(true, false),
					ac != null && ac.Field2 < 1 && !artifact.Name.Contains("empty", StringComparison.OrdinalIgnoreCase) ? " (empty)" : "",
					artifact.EvalPlural("is", "are"),
					gEngine.Buf01);
			}
		}

		public virtual void PrintEnemiesNearby()
		{
			Out.Print("You can't do that with unfriendlies about!");
		}

		public virtual void PrintNothingHappens()
		{
			Out.Print("Nothing happens.");
		}

		public virtual void PrintFullDesc(IArtifact artifact, bool showName, bool showVerboseName)
		{
			Debug.Assert(artifact != null);

			gEngine.Buf.Clear();

			var rc = artifact.BuildPrintedFullDesc(gEngine.Buf, showName, showVerboseName);

			Debug.Assert(IsSuccess(rc));

			Out.Write("{0}", gEngine.Buf);
		}

		public virtual void PrintMonsterCantFindExit(IMonster monster, IRoom room, string monsterName, bool isPlural, bool fleeing)
		{
			Debug.Assert(monster != null);

			Out.Print("{0}", monster.GetCantFindExitDescString(room, monsterName, isPlural, fleeing));
		}

		public virtual void PrintMonsterMembersExitRoom(IMonster monster, IRoom room, string monsterName, bool isPlural, bool fleeing)
		{
			Debug.Assert(monster != null);

			Out.Print("{0}", monster.GetMembersExitRoomDescString(room, monsterName, isPlural, fleeing));
		}

		public virtual void PrintMonsterExitsRoom(IMonster monster, IRoom room, string monsterName, bool isPlural, bool fleeing, Direction direction)
		{
			Debug.Assert(monster != null);

			Out.Print("{0}", monster.GetExitRoomDescString(room, monsterName, isPlural, fleeing, direction));
		}

		public virtual void PrintMonsterEntersRoom(IMonster monster, IRoom room, string monsterName, bool isPlural, bool fleeing, Direction direction)
		{
			Debug.Assert(monster != null);

			Out.Print("{0}", monster.GetEnterRoomDescString(room, monsterName, isPlural, fleeing, direction));
		}

		public virtual void PrintMonsterGetsAngry(IMonster monster, bool printFinalNewLine = true)
		{
			Debug.Assert(monster != null);

			Out.Write("{0}{1} get{2} angry!{3}",
				Environment.NewLine,
				monster.GetTheName(true),
				monster.EvalPlural("s", ""),
				printFinalNewLine ? Environment.NewLine : "");
		}

		public virtual void PrintMonsterEmotes(IMonster monster, bool friendSmile = true)
		{
			Debug.Assert(monster != null);

			if (IsRulesetVersion(5, 62) && monster.Reaction == Friendliness.Friend)
			{
				Out.Write("{0}{1} {2}{3} back.",
					Environment.NewLine,
					monster.GetTheName(true),
					monster.EvalReaction("growl", "ignore", friendSmile ? "smile" : "wave"),
					monster.EvalPlural("s", ""));
			}
			else
			{
				Out.Write("{0}{1} {2}{3} {4}you{5}",
					Environment.NewLine,
					monster.GetTheName(true),
					monster.EvalReaction("growl", "ignore", friendSmile ? "smile" : "wave"),
					monster.EvalPlural("s", ""),
					monster.Reaction != Friendliness.Neutral ? "at " : "",
					IsRulesetVersion(5, 62) && monster.Reaction == Friendliness.Enemy ? "!" : ".");
			}
		}

		public virtual void PrintFullDesc(IMonster monster, bool showName, bool showVerboseName)
		{
			Debug.Assert(monster != null);

			gEngine.Buf.Clear();

			var rc = monster.BuildPrintedFullDesc(gEngine.Buf, showName, showVerboseName);

			Debug.Assert(IsSuccess(rc));

			Out.Write("{0}", gEngine.Buf);
		}

		public virtual void PrintHealthImproves(IMonster monster)
		{
			Debug.Assert(monster != null);

			var isCharMonster = monster.IsCharacterMonster();

			if (IsRulesetVersion(5, 62))
			{
				Out.Print("Some of {0} wounds seem to clear up.",
					isCharMonster ? "your" :
					monster.EvalPlural(monster.GetTheName(), monster.GetArticleName(false, true, false, false, true)).AddPossessiveSuffix());
			}
			else
			{
				Out.Print("{0} health improves!",
					isCharMonster ? "Your" :
					monster.EvalPlural(monster.GetTheName(true), monster.GetArticleName(true, true, false, false, true)).AddPossessiveSuffix());
			}
		}

		public virtual void PrintHealthStatus(IMonster monster, bool includeUninjuredGroupMonsters)
		{
			Debug.Assert(monster != null);

			var isCharMonster = monster.IsCharacterMonster();

			var isUninjuredGroupMonster = includeUninjuredGroupMonsters && monster.CurrGroupCount > 1 && monster.DmgTaken == 0;

			gEngine.Buf.SetFormat("{0}{1} {2} ",
				Environment.NewLine,
				isCharMonster ? "You" :
				isUninjuredGroupMonster ? "They" :
				monster.GetTheName(true, true, false, false, true),
				isCharMonster || isUninjuredGroupMonster ? "are" : "is");

			monster.AddHealthStatus(gEngine.Buf);

			Out.Write("{0}", gEngine.Buf);
		}

		public virtual void PrintDoesntHaveIt(IMonster monster)
		{
			Debug.Assert(monster != null);

			Out.Print("{0}{1} have it.", monster.GetTheName(true), monster.EvalPlural(" doesn't", " don't"));
		}

		public virtual void PrintTooManyWeapons()
		{
			Out.Print("As you {0}enter the Main Hall, Lord William Missilefire {1}, \"You have too many weapons to keep them all, four is the legal limit.\"", 
				IsRulesetVersion(5, 62) ? "start to " : "", 
				IsRulesetVersion(5, 62) ? "appears and tells you" : "approaches you and says");
		}

		public virtual void PrintDeliverGoods()
		{
			if (IsRulesetVersion(5, 62))
			{
				Out.Write("{0}As you deliver your treasures to Sam Slicker, the local buyer for such things, he examines your goods and pays you ", Environment.NewLine);
			}
			else
			{			
				Out.Print("You deliver your goods to Sam Slicker, the local buyer for such things.  He examines your items and pays you what they are worth.");
			}
		}

		public virtual void PrintYourWeaponsAre()
		{
			Out.WriteLine("{0}Your weapons are:{0}{1}", Environment.NewLine, gEngine.Buf);
		}

		public virtual void PrintEnterWeaponToSell()
		{
			Out.Write("{0}Enter the number of a weapon to sell: ", Environment.NewLine);
		}

		public virtual void PrintAllWoundsHealed()
		{
			if (IsRulesetVersion(62))
			{
				Out.Print("Your wounds heal!");
			}
			else
			{
				Out.Print("All of your wounds are healed.");
			}
		}

		public virtual void PrintYouHavePerished()
		{
			Out.Print("You have perished.  Now what?");
		}

		public virtual void PrintRestoreSavedGame()
		{
			Out.Write("{0} 1. Restore a saved game", Environment.NewLine);
		}

		public virtual void PrintStartOver()
		{
			Out.Write("{0} 2. Start over (saved games will be deleted)", Environment.NewLine);
		}

		public virtual void PrintAcceptDeath()
		{
			Out.Print(" 3. Give up, accept death");
		}

		public virtual void PrintEnterDeadMenuChoice()
		{
			Out.Write("{0}Your choice: ", Environment.NewLine);
		}

		public virtual void PrintReallyWantToStartOver()
		{
			Out.Write("{0}Do you really want to start over (Y/N): ", Environment.NewLine);
		}

		public virtual void PrintReallyWantToAcceptDeath()
		{
			Out.Write("{0}Do you really want to accept death (Y/N): ", Environment.NewLine);
		}

		public virtual void PrintWakingUpMonsters()
		{
			Out.WriteLine("Please wait a short while (waking up the monsters...)");
		}

		public virtual void PrintBaseProgramVersion()
		{
			Out.WriteLine("[Base Program {0}]", ProgVersion);
		}

		public virtual void PrintWelcomeToEamonCS()
		{
			Out.Print("Welcome to the Eamon CS fantasy gaming system!");
		}

		public virtual void PrintWelcomeBack()
		{
			Out.Print("Welcome back to {0}!", Module.Name);
		}

		public virtual void PrintEnterSeeIntroStoryChoice()
		{
			Out.Write("{0}Would you like to see the introduction story again (Y/N) [{1}N]: ", Environment.NewLine, EnableScreenReaderMode ? "Default " : "");
		}

		public virtual void PrintEnterWeaponNumberChoice()
		{
			Out.Write("{0}Press the number of the weapon to select: ", Environment.NewLine);
		}

		public virtual void PrintNoIntroStory()
		{
			Out.Print("There is no introduction story for this adventure.");
		}

		public virtual void PrintSavedGamesDeleted()
		{
			Out.Print("Your saved games have been deleted.");
		}

		public virtual void PrintRestartGameUsingResume()
		{
			Out.Print("You can restart the game by running the Resume[AdventureName].psh file.");
		}

		public virtual void PrintMemorialService()
		{
			Out.Print("When word of your untimely passing reaches the Main Hall, all your friends gather for a round at the bar.  They honor you with tales of your great deeds and with one last toast.  If you were there you'd be surprised to see the burly Irishman tear up.");
		}

		public virtual void PrintSavedGames()
		{
			Out.Print("Saved games:");
		}

		public virtual void PrintSaveSlot(long saveSlot, string saveName, bool printFinalNewLine = false)
		{
			Debug.Assert(saveSlot > 0 && saveName != null);

			Out.Write("{0}{1,3}. {2}{3}", Environment.NewLine, saveSlot, saveName, printFinalNewLine ? Environment.NewLine : "");
		}

		public virtual void PrintQuickSave(long saveSlot, string saveName)
		{
			Debug.Assert(saveSlot > 0 && saveName != null);

			Out.Print("[QUICK SAVE {0}: {1}]", saveSlot, saveName);
		}

		public virtual void PrintUsingSlotInstead(long saveSlot)
		{
			Debug.Assert(saveSlot > 0);

			Out.Print("[Using #{0} instead{1}]", saveSlot, EnableScreenReaderMode ? "" : ".");
		}

		public virtual void PrintEnterSaveSlotChoice(long numMenuItems)
		{
			Debug.Assert(numMenuItems > 0);

			Out.Write("{0}Enter 1-{1} for saved position: ", Environment.NewLine, numMenuItems);
		}

		public virtual void PrintEnterRestoreSlotChoice(long numMenuItems)
		{
			Debug.Assert(numMenuItems > 0);

			Out.Write("{0}Your choice (1-{1}): ", Environment.NewLine, numMenuItems);
		}

		public virtual void PrintChangingHim(string himStr)
		{
			Debug.Assert(himStr != null);

			Out.Print("{{Changing him:  \"{0}\".}}", himStr);
		}

		public virtual void PrintChangingHer(string herStr)
		{
			Debug.Assert(herStr != null);

			Out.Print("{{Changing her:  \"{0}\".}}", herStr);
		}

		public virtual void PrintChangingIt(string itStr)
		{
			Debug.Assert(itStr != null);

			Out.Print("{{Changing it:  \"{0}\".}}", itStr);
		}

		public virtual void PrintChangingThem(string themStr)
		{
			Debug.Assert(themStr != null);

			Out.Print("{{Changing them:  \"{0}\".}}", themStr);
		}

		public virtual void PrintDiscardMessage(string inputStr)
		{
			Debug.Assert(inputStr != null);

			Out.Print("{{Discarding:  \"{0}\".}}", inputStr);
		}

		public virtual void PrintGoodsPayment(bool goodsExist, long goldAmount)
		{
			if (IsRulesetVersion(5, 62))
			{
				Out.Write("{0} gold piece{1}.{2}", goldAmount, goldAmount != 1 ? "s" : "", Environment.NewLine);
			}
			else
			{
				Out.Print("{0}He pays you {1} gold piece{2} total.", goodsExist ? Environment.NewLine : "", goldAmount, goldAmount != 1 ? "s" : "");
			}
		}

		public virtual void PrintMacroReplacedPagedString(string str, StringBuilder buf)
		{
			Debug.Assert(str != null && buf != null);

			buf.Clear();

			var rc = ResolveUidMacros(str, buf, true, true);

			Debug.Assert(IsSuccess(rc));

			Out.WriteLine();

			var pages = buf.ToString().Split(new string[] { PageSep }, StringSplitOptions.RemoveEmptyEntries);

			for (var i = 0; i < pages.Length; i++)
			{
				if (i > 0)
				{
					Out.WriteLine("{0}{1}{0}", Environment.NewLine, LineSep);
				}

				Out.Write("{0}", pages[i]);

				if (i < pages.Length - 1)
				{
					Out.WriteLine();

					In.KeyPress(buf);
				}
			}

			Out.WriteLine();
		}

		public virtual void BuildRevealContentsListDescString(IMonster monster, IArtifact artifact, IList<IArtifact> revealContentsList, ContainerType containerType, bool showCharOwned, IRecordNameListArgs recordNameListArgs = null)
		{
			Debug.Assert(artifact != null && revealContentsList != null && revealContentsList.Count > 0 && Enum.IsDefined(typeof(ContainerType), containerType));

			gEngine.Buf01.SetFormat("{0} {1}",
				monster != null && !monster.IsCharacterMonster() ? (UseRevealContentMonsterTheName ? monster.GetTheName(groupCountOne: true) : monster.GetArticleName(groupCountOne: true)) : "you",
				monster != null && !monster.IsCharacterMonster() ? "finds" : "find");

			gEngine.Buf.SetFormat("{0}{1} {2}, {3} ",
				Environment.NewLine,
				EvalContainerType(containerType, "Inside", "On", "Under", "Behind"),
				artifact.GetTheName(showCharOwned: showCharOwned),
				gEngine.Buf01.ToString());

			if (recordNameListArgs == null)
			{
				recordNameListArgs = CreateInstance<IRecordNameListArgs>(x =>
				{
					x.ArticleType = ArticleType.A;

					x.ShowCharOwned = showCharOwned;			// TODO: verify

					x.StateDescCode = StateDescDisplayCode.None;

					x.ShowContents = false;

					x.GroupCountOne = false;
				});
			}

			var rc = GetRecordNameList(revealContentsList.Cast<IGameBase>().ToList(), recordNameListArgs, gEngine.Buf);

			Debug.Assert(IsSuccess(rc));

			gEngine.Buf.AppendFormat(".{0}", Environment.NewLine);
		}

		public virtual long WeaponPowerCompare(IArtifact artifact1, IArtifact artifact2)
		{
			Debug.Assert(artifact1 != null && artifact2 != null);

			var ac1 = artifact1.GeneralWeapon;

			Debug.Assert(ac1 != null);

			var ac2 = artifact2.GeneralWeapon;

			Debug.Assert(ac2 != null);

			var result1 = ac1.Field3 * ac1.Field4;

			var result2 = ac2.Field3 * ac2.Field4;

			return result1 > result2 ? 1 : result1 < result2 ? -1 : 0;
		}

		public virtual long WeaponPowerCompare(long artifactUid1, long artifactUid2)
		{
			return WeaponPowerCompare(ADB[artifactUid1], ADB[artifactUid2]);
		}

		public virtual IArtifact GetMostPowerfulWeapon(IList<IArtifact> artifactList)
		{
			IArtifact cw = null;

			Debug.Assert(artifactList != null);

			foreach (var artifact in artifactList)
			{
				var ac = artifact.GeneralWeapon;

				if (ac != null && (cw == null || WeaponPowerCompare(artifact, cw) > 0))
				{
					cw = artifact;

					Debug.Assert(cw.Uid > 0);
				}
			}

			return cw;
		}

		public virtual long GetMostPowerfulWeaponUid(IList<IArtifact> artifactList)
		{
			Debug.Assert(artifactList != null);

			var cw = GetMostPowerfulWeapon(artifactList);

			return cw != null ? cw.Uid : 0;     // Note: -1 not returned!
		}

		public virtual void EnforceCharMonsterWeightLimits(IRoom room = null, bool printOutput = false)
		{
			Debug.Assert(gCharMonster != null);

			Debug.Assert(gCharRoom != null);

			var enableOutput = Out.EnableOutput;

			try
			{
				RevealContentCounter--;

				Out.EnableOutput = printOutput;

				if (room == null)
				{
					room = gCharRoom;
				}

				var artifactList = gCharMonster.GetContainedList().OrderByDescending(a => a.RecursiveWeight).ToList();

				var charWeight = 0L;

				var rc = gCharMonster.GetFullInventoryWeight(ref charWeight, recurse: true);

				Debug.Assert(IsSuccess(rc));

				while (artifactList.Count > 0 && charWeight > gCharMonster.GetWeightCarryableGronds())
				{
					var artifact = artifactList[0];

					Debug.Assert(artifact != null);

					Debug.Assert(!artifact.IsUnmovable01());

					artifactList.RemoveAt(0);

					if (artifact.IsWornByMonster(gCharMonster))
					{
						var removeCommand = CreateInstance<IRemoveCommand>(x =>
						{
							x.ActorMonster = gCharMonster;

							x.ActorRoom = room;

							x.Dobj = artifact;
						});

						removeCommand.Execute();
					}

					var dropCommand = CreateInstance<IDropCommand>(x =>
					{
						x.ActorMonster = gCharMonster;

						x.ActorRoom = room;

						x.Dobj = artifact;
					});

					dropCommand.Execute();

					charWeight -= artifact.RecursiveWeight;
				}
			}
			finally
			{
				RevealContentCounter++;

				Out.EnableOutput = enableOutput;
			}
		}

		public virtual void NormalizeArtifactValuesAndWeights()
		{
			var artifactList = Database.ArtifactTable.Records.ToList();

			foreach (var artifact in artifactList)
			{
				if (artifact.Value < 0)
				{
					artifact.Value = 0;
				}

				if (artifact.Weight < 0 && !artifact.IsUnmovable01())
				{
					artifact.Weight = 0;
				}
			}
		}

		public virtual void AddUniqueCharsToArtifactAndMonsterNames()
		{
			var recordList = new List<IGameBase>();

			var artifactList = PoundCharPolicy == PoundCharPolicy.PlayerArtifactsOnly ? Database.ArtifactTable.Records.Where(a => a.IsCharOwned).ToList() :
									PoundCharPolicy == PoundCharPolicy.AllArtifacts ? Database.ArtifactTable.Records.ToList() :
									new List<IArtifact>();

			artifactList.Reverse();

			var i = 0;

			while (i < artifactList.Count && artifactList[i].IsCharOwned)
			{
				i++;
			}

			if (i > 0)
			{
				artifactList.Reverse(0, i);
			}

			if (artifactList.Count > i)
			{
				artifactList.Reverse(i, artifactList.Count - i);
			}

			recordList.AddRange(artifactList);

			var monsterList = PercentCharPolicy == PercentCharPolicy.AllMonsters ? Database.MonsterTable.Records.ToList() :
									new List<IMonster>();

			recordList.AddRange(monsterList);

			AddUniqueCharsToRecordNames(recordList);
		}

		public virtual void AddMissingDescs()
		{
			var monsterList = GetMonsterList(m => string.IsNullOrWhiteSpace(m.Desc) || m.Desc.Equals("NONE", StringComparison.OrdinalIgnoreCase));

			foreach (var monster in monsterList)
			{
				monster.Desc = string.Format("{0} {1}.", monster.EvalPlural("This is", "These are"), monster.GetArticleName());
			}

			var artifactList = GetArtifactList(a => string.IsNullOrWhiteSpace(a.Desc) || a.Desc.Equals("NONE", StringComparison.OrdinalIgnoreCase));

			foreach (var artifact in artifactList)
			{
				artifact.Desc = string.Format("{0} {1}.", artifact.EvalPlural("This is", "These are"), artifact.GetArticleName());
			}
		}

		public virtual void InitSaArray()
		{
			var spellValues = EnumUtil.GetValues<Spell>();

			foreach (var sv in spellValues)
			{
				var i = (long)sv;

				GameState.SetSa(i, Character.GetSpellAbility(i));
			}
		}

		public virtual void CreateCommands()
		{
			var commands = ClassMappingsDictionary.Keys.Where(x => x.GetInterfaces().Contains(typeof(ICommand)));

			foreach (var command in commands)
			{
				if (Module.NumDirs == 12 || !(command.IsSameOrSubclassOf(typeof(INeCommand)) || command.IsSameOrSubclassOf(typeof(INwCommand)) || command.IsSameOrSubclassOf(typeof(ISeCommand)) || command.IsSameOrSubclassOf(typeof(ISwCommand)) || command.IsSameOrSubclassOf(typeof(IInCommand)) || command.IsSameOrSubclassOf(typeof(IOutCommand))))
				{
					var command01 = CreateInstance<ICommand>(command);

					if (command01 != null && !string.IsNullOrWhiteSpace(command01.Verb))
					{
						CommandList.Add(command01);
					}
				}
			}

			CommandList = CommandList.OrderBy(x => x.SortOrder).ToList();
		}

		public virtual void InitRooms()
		{
			// Do nothing
		}

		public virtual void InitArtifacts()
		{
			var artifactList = Database.ArtifactTable.Records.ToList();

			foreach (var artifact in artifactList)
			{
				TruncatePluralTypeEffectDesc(artifact.PluralType, ArtNameLen);
			}
		}

		public virtual void InitMonsters()
		{
			if (UseMonsterScaledHardinessValues)
			{
				InitMonsterScaledHardinessValues();
			}

			var monsterList = Database.MonsterTable.Records.ToList();

			foreach (var monster in monsterList)
			{
				monster.InitGroupCount = monster.GroupCount;

				monster.CurrGroupCount = monster.GroupCount;

				monster.ResolveReaction(Character);

				if (EnforceMonsterWeightLimits && !monster.IsCharacterMonster())
				{
					var rc = monster.EnforceFullInventoryWeightLimits(recurse: true);

					Debug.Assert(IsSuccess(rc));
				}

				if (monster.Weapon > 0)
				{
					var artifact = ADB[monster.Weapon];

					if (artifact != null)
					{
						artifact.AddStateDesc(artifact.GetReadyWeaponDesc());
					}
				}

				TruncatePluralTypeEffectDesc(monster.PluralType, MonNameLen);
			}
		}

		public virtual void InitMonsterScaledHardinessValues()
		{
			var maxDamage = ScaledHardinessUnarmedMaxDamage;

			Debug.Assert(gCharMonster != null);

			if (gCharMonster.Weapon > 0)       // Will always be most powerful weapon
			{
				var artifact = ADB[gCharMonster.Weapon];

				Debug.Assert(artifact != null);

				var ac = artifact.GeneralWeapon;

				Debug.Assert(ac != null);

				maxDamage = ac.Field3 * ac.Field4;
			}

			var damageFactor = (long)Math.Round((double)maxDamage / ScaledHardinessMaxDamageDivisor);

			if (damageFactor < 1)
			{
				damageFactor = 1;
			}

			var monsterList = Database.MonsterTable.Records.ToList();

			foreach (var m in monsterList)
			{
				SetScaledHardiness(m, damageFactor);
			}
		}

		public virtual void ConvertArtifactToCharArtifact(IArtifact artifact, IArtifactCategory ac)
		{
			RetCode rc;

			Debug.Assert(artifact != null);

			Debug.Assert(ac != null);

			var artifact01 = new Eamon.Game.Artifact();         // Create export Artifact using explicit base class

			Debug.Assert(artifact01 != null);

			artifact01.CopyPropertiesFrom(artifact, recurse: true);

			artifact01.Name = artifact01.Name.Trim().TrimEnd('#');

			Debug.Assert(!string.IsNullOrWhiteSpace(artifact01.Name));

			if (ac.Type == ArtifactType.Weapon || ac.Type == ArtifactType.MagicWeapon)
			{
				rc = artifact01.RemoveStateDesc(artifact01.GetReadyWeaponDesc());

				Debug.Assert(IsSuccess(rc));
			}

			if (!string.IsNullOrWhiteSpace(artifact01.Desc))
			{
				gEngine.Buf.Clear();

				rc = ResolveUidMacros(artifact01.Desc, gEngine.Buf, true, true);

				Debug.Assert(IsSuccess(rc));

				if (gEngine.Buf.Length <= ArtDescLen)
				{
					artifact01.Desc = CloneInstance(gEngine.Buf.ToString());
				}
			}

			artifact01.IsCharOwned = true;

			var ac01 = new Eamon.Game.Primitive.Classes.ArtifactCategory();         // Create export ArtifactCategory using explicit base class

			Debug.Assert(ac01 != null);

			ac01.CopyPropertiesFrom(ac, recurse: true);

			artifact01.SetArtifactCategoryCount(1);

			artifact01.SetCategory(0, ac01);

			gDatabase.ExecuteOnArtifactTable(ArtifactTableType.CharArt, () =>
			{
				artifact01.Uid = Database.GetArtifactUid();
			});

			artifact01.SetParentReferences();

			if (ac.Type == ArtifactType.Wearable)
			{
				artifact01.SetWornByCharacter(Character);
			}
			else
			{
				artifact01.SetCarriedByCharacter(Character);
			}

			gDatabase.ExecuteOnArtifactTable(ArtifactTableType.CharArt, () =>
			{
				rc = Database.AddArtifact(artifact01);

				Debug.Assert(IsSuccess(rc));
			});

			artifact.SetInLimbo();
		}

		public virtual IMonster ConvertArtifactToMonster(IArtifact artifact, Action<IMonster> initialize = null, bool addToDatabase = false)
		{
			Debug.Assert(artifact != null);

			var monster = CreateInstance<IMonster>(x =>
			{
				x.Uid = Database.GetMonsterUid();

				x.Name = CloneInstance(artifact.Name);

				x.Seen = artifact.Seen;

				x.ArticleType = artifact.ArticleType;

				x.StateDesc = CloneInstance(artifact.StateDesc);

				x.Desc = CloneInstance(artifact.Desc);

				x.IsListed = artifact.IsListed;

				x.PluralType = artifact.PluralType;

				x.Hardiness = 16;

				x.Agility = 15;

				x.GroupCount = 1;

				x.AttackCount = 1;

				x.Courage = 100;

				x.Location = artifact.Location;

				x.CombatCode = CombatCode.Weapons;

				x.NwDice = 1;

				x.NwSides = 4;

				x.Friendliness = (Friendliness)200;

				x.Gender = Gender.Male;

				x.CurrGroupCount = 1;

				x.InitParry = EnableEnhancedCombat ? 50 : 0;

				x.Reaction = Friendliness.Friend;
			});

			if (initialize != null)
			{
				initialize(monster);
			}

			if (addToDatabase)
			{
				var rc = Database.AddMonster(monster);

				Debug.Assert(IsSuccess(rc));
			}

			return monster;
		}

		public virtual void ConvertCharacterToMonster()
		{
			RetCode rc;

			var monster = CreateInstance<IMonster>(x =>
			{
				x.Uid = Database.GetMonsterUid();

				x.Name = Character.Name.Trim();

				x.Desc = string.Format("You are the {0} {1}.", Character.EvalGender("mighty", "fair", "androgynous"), Character.Name);

				x.Hardiness = Character.GetStat(Stat.Hardiness);

				x.Agility = Character.GetStat(Stat.Agility);

				x.GroupCount = 1;

				x.AttackCount = 1;

				x.CurrGroupCount = 1;

				x.Armor = 0;

				x.Weapon = -1;

				x.NwDice = 1;

				x.NwSides = 2;

				x.Friendliness = (Friendliness)200;

				x.Gender = Character.Gender;

				x.InitParry = EnableEnhancedCombat ? 50 : 0;

				x.Reaction = Friendliness.Friend;
			});

			rc = Database.AddMonster(monster);

			Debug.Assert(IsSuccess(rc));

			GameState.Cm = monster.Uid;

			Debug.Assert(GameState.Cm > 0);

			ConvertCharArtifactsToArtifacts(monster);
		}

		public virtual void ConvertMonsterToCharacter(IMonster monster, IList<IArtifact> weaponList)
		{
			Debug.Assert(monster != null && weaponList != null);

			ResetMonsterStats(monster);

			Character.Name = monster.Name.Trim().TrimEnd('%');

			Character.SetStat(Stat.Hardiness, monster.Hardiness);

			Character.SetStat(Stat.Agility, monster.Agility);

			Character.Gender = monster.Gender;

			for (var i = 0; i < weaponList.Count; i++)
			{
				var artifact = weaponList[i];

				Debug.Assert(artifact != null);

				var ac = artifact.GeneralWeapon;

				Debug.Assert(ac != null);

				ConvertArtifactToCharArtifact(artifact, ac);
			}

			Character.AddUniqueCharsToWeaponNames();

			Out.Print("{0}", LineSep);
		}

		public virtual void ResetMonsterStats(IMonster monster)
		{
			Debug.Assert(monster != null && monster.IsCharacterMonster());

			if (GameState.Speed > 0)
			{
				monster.Agility /= 2;
			}

			GameState.Speed = 0;
		}

		public virtual void SetArmorClass()
		{
			var artUids = new long[] { GameState.Ar, GameState.Sh };

			foreach (var artUid in artUids)
			{
				if (artUid > 0)
				{
					var artifact = ADB[artUid];

					Debug.Assert(artifact != null);

					var ac = artifact.Wearable;

					Debug.Assert(ac != null);

					ConvertArtifactToCharArtifact(artifact, ac);
				}
			}

			// GameState.Ar = 0;

			// GameState.Sh = 0;
		}

		public virtual void ConvertToCarriedInventory(IList<IArtifact> weaponList)
		{
			long c;

			Debug.Assert(weaponList != null);

			weaponList.Clear();

			for (var i = 0; i < GameState.HeldWpnUids.Count; i++)
			{
				var artifactUid = GameState.GetHeldWpnUid(i);

				if (artifactUid > 0)
				{
					var artifact = ADB[artifactUid];

					Debug.Assert(artifact != null);

					artifact.SetCarriedByMonster(gCharMonster);
				}
			}

			var artifactList = GetArtifactList(a => a.IsWornByMonster(gCharMonster));

			foreach (var artifact in artifactList)
			{
				artifact.SetCarriedByMonster(gCharMonster);
			}

			do
			{
				c = 0;

				artifactList = GetArtifactList(a => a.IsCarriedByMonster(gCharMonster));

				foreach (var artifact in artifactList)
				{
					var ac = artifact.GeneralContainer;

					if (ac != null)
					{
						var artifactList01 = GetArtifactList(a => a.IsCarriedByContainer(artifact));

						foreach (var artifact01 in artifactList01)
						{
							if (artifact01.Seen == true || artifact01.GetCarriedByContainerContainerType() != ContainerType.In || (artifact.InContainer != null && artifact.InContainer.IsOpen()) || artifact.ShouldExposeInContentsWhenClosed())
							{
								artifact01.SetCarriedByMonster(gCharMonster);

								c = 1;
							}
						}
					}
				}
			}
			while (c == 1);

			artifactList = Database.ArtifactTable.Records.ToList();

			foreach (var artifact in artifactList)
			{
				if (artifact.IsCarriedByMonster(gCharMonster))
				{
					var ac = artifact.GeneralWeapon;

					if (ac != null && ac == artifact.GetCategory(0) && artifact.IsReadyableByMonster(gCharMonster))         // Note: ancillary non-Category(0) weapon Artifacts are sold to Sam Slicker
					{
						weaponList.Add(artifact);

						artifact.SetInLimbo();
					}
				}

				if (artifact.Uid != GameState.Ar && artifact.Uid != GameState.Sh && !weaponList.Contains(artifact))
				{
					artifact.Seen = false;
				}
			}

			if (weaponList.Count > NumCharacterWeapons)
			{
				Out.Print("{0}", LineSep);
			}
		}

		public virtual void SellExcessWeapons(IList<IArtifact> weaponList)
		{
			Debug.Assert(weaponList != null);

			if (weaponList.Count > NumCharacterWeapons)
			{
				PrintTooManyWeapons();

				while (weaponList.Count > NumCharacterWeapons)
				{
					Out.Print("{0}", LineSep);

					gEngine.Buf.Clear();

					var rc = ListRecords(weaponList.Cast<IGameBase>().ToList(), true, true, gEngine.Buf);

					Debug.Assert(IsSuccess(rc));

					PrintYourWeaponsAre();

					Out.Print("{0}", LineSep);

					PrintEnterWeaponToSell();

					gEngine.Buf.Clear();

					rc = In.ReadField(gEngine.Buf, BufSize01, null, ' ', '\0', false, null, ModifyCharToUpper, IsCharDigit, null);

					Debug.Assert(IsSuccess(rc));

					var m = Convert.ToInt64(gEngine.Buf.Trim().ToString());

					if (m >= 1 && m <= weaponList.Count)
					{
						weaponList[(int)m - 1].SetCarriedByMonster(gCharMonster);

						weaponList.RemoveAt((int)m - 1);
					}
				}
			}
		}

		public virtual void SellInventoryToMerchant(bool sellInventory = true)
		{
			var c = 0L;

			var w = 0L;

			PrintDeliverGoods();

			if (sellInventory)
			{
				var c2 = Character.GetMerchantAdjustedCharisma();

				var rtio = GetMerchantRtio(c2);

				var artifactList = GetArtifactList(a => a.IsCarriedByMonster(gCharMonster));

				foreach (var artifact in artifactList)
				{
					var m = artifact.Gold != null ? artifact.Value : GetMerchantBidPrice(artifact.Value, rtio);

					if (m < 0)
					{
						m = 0;
					}

					PrintArtifactIsWorth(artifact, m);

					w = w + m;

					c = 1;
				}

				Character.HeldGold += w;
			}

			PrintGoodsPayment(c == 1, w);

			In.KeyPress(gEngine.Buf);
		}

		public virtual void DeadMenu(bool printLineSep, ref bool restoreGame)
		{
			while (true)
			{
				restoreGame = false;

				if (printLineSep)
				{
					Out.Print("{0}", LineSep);
				}

				PrintYouHavePerished();

				PrintRestoreSavedGame();

				PrintStartOver();

				PrintAcceptDeath();

				Out.Print("{0}", LineSep);

				PrintEnterDeadMenuChoice();

				gEngine.Buf.Clear();

				var rc = In.ReadField(gEngine.Buf, BufSize02, null, ' ', '\0', false, null, ModifyCharToUpper, IsChar1To3, null);

				Debug.Assert(IsSuccess(rc));

				var i = Convert.ToInt64(gEngine.Buf.Trim().ToString());

				var confirmed = false;

				if (i == 2 || i == 3)
				{
					Out.Print("{0}", LineSep);

					if (i == 2)
					{
						PrintReallyWantToStartOver();
					}
					else
					{
						PrintReallyWantToAcceptDeath();
					}

					gEngine.Buf.Clear();

					rc = In.ReadField(gEngine.Buf, BufSize02, null, ' ', '\0', false, null, ModifyCharToUpper, IsCharYOrN, null);

					Debug.Assert(IsSuccess(rc));

					if (gEngine.Buf.Length > 0 && gEngine.Buf[0] == 'Y')
					{
						confirmed = true;
					}
					else
					{
						printLineSep = true;
					}
				}

				if (i == 3)
				{
					if (confirmed)
					{
						ExitType = ExitType.GoToMainHall;

						MainLoop.ShouldShutdown = false;

						break;
					}
				}
				else if (i == 2)
				{
					if (confirmed)
					{
						ExitType = ExitType.StartOver;

						MainLoop.ShouldShutdown = false;

						break;
					}
				}
				else
				{
					restoreGame = true;

					break;
				}
			}
		}

		public virtual void LightOut(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			var rc = artifact.RemoveStateDesc(artifact.GetProvidingLightDesc());

			Debug.Assert(IsSuccess(rc));

			GameState.Ls = 0;

			PrintLightOut(artifact);
		}

		public virtual void MonsterGetsAggravated(IMonster monster, bool printFinalNewLine = true)
		{
			Debug.Assert(monster != null);

			if (monster.Reaction > Friendliness.Enemy)
			{
				Debug.Assert(gCharMonster != null);

				var charRoom = gCharMonster.GetInRoom();

				Debug.Assert(charRoom != null);

				var room = monster.GetInRoom();

				Debug.Assert(room != null);

				if (!Enum.IsDefined(typeof(Friendliness), monster.Friendliness))
				{
					monster.Friendliness -= 100;

					monster.Friendliness = (Friendliness)((long)monster.Friendliness / 2);

					monster.Friendliness += 100;
				}
				else
				{
					monster.Friendliness--;
				}

				var monsterList = new List<IMonster>() { monster };

				if (IsRulesetVersion(5, 62))
				{
					monsterList.AddRange(GetMonsterList(m => !m.IsCharacterMonster() && m.Uid != monster.Uid && m.IsInRoom(room) && m.Reaction > Friendliness.Enemy));

					foreach (var monster01 in monsterList)
					{
						monster01.ResolveReaction(Character);
					}
				}
				else
				{
					monster.Reaction--;
				}

				foreach (var monster01 in monsterList)
				{
					if (monster01.Reaction == Friendliness.Enemy)
					{
						MiscEventFuncList02.Add(() =>
						{
							if (monster01.IsInRoom(charRoom))
							{
								PrintMonsterGetsAngry(monster01, printFinalNewLine);
							}
						});
					}
				}
			}
		}

		public virtual void MonsterDies(IMonster actorMonster, IMonster dobjMonster)
		{
			RetCode rc;

			// ActorMonster may be null or non-null

			Debug.Assert(dobjMonster != null && !dobjMonster.IsCharacterMonster());

			try
			{
				RevealContentCounter--;

				var room = dobjMonster.GetInRoom();

				Debug.Assert(room != null);

				if (dobjMonster.CurrGroupCount > 1)
				{
					if (dobjMonster.Weapon > 0)
					{
						var weapon = GetNthArtifact(dobjMonster.GetCarriedList(), dobjMonster.CurrGroupCount - 1, a => a.IsReadyableByMonster(dobjMonster) && a.Uid != dobjMonster.Weapon);

						if (weapon != null)
						{
							weapon.SetInRoom(room);
						}
					}

					dobjMonster.CurrGroupCount--;

					// dobjMonster.Parry = dobjMonster.InitParry;

					dobjMonster.DmgTaken = 0;

					if (EnforceMonsterWeightLimits)
					{
						rc = dobjMonster.EnforceFullInventoryWeightLimits(recurse: true);

						Debug.Assert(IsSuccess(rc));
					}
				}
				else
				{
					if (dobjMonster.Weapon > 0)
					{
						var weapon = ADB[dobjMonster.Weapon];

						Debug.Assert(weapon != null);

						rc = weapon.RemoveStateDesc(weapon.GetReadyWeaponDesc());

						Debug.Assert(IsSuccess(rc));

						dobjMonster.Weapon = -1;
					}

					dobjMonster.SetInLimbo();

					dobjMonster.CurrGroupCount = dobjMonster.GroupCount;

					// dobjMonster.ResolveReaction(Character);

					dobjMonster.Parry = dobjMonster.InitParry;

					dobjMonster.DmgTaken = 0;

					var artifactList = GetArtifactList(a => a.IsCarriedByMonster(dobjMonster) || a.IsWornByMonster(dobjMonster));

					foreach (var artifact in artifactList)
					{
						artifact.SetInRoom(room);
					}

					ProcessMonsterDeathEvents(dobjMonster);

					if (dobjMonster.DeadBody > 0)
					{
						var deadBody = ADB[dobjMonster.DeadBody];

						Debug.Assert(deadBody != null);

						if (!deadBody.IsCharOwned)
						{
							deadBody.SetInRoom(room);
						}
					}
				}
			}
			finally
			{
				RevealContentCounter++;
			}
		}

		public virtual void ProcessMonsterDeathEvents(IMonster monster)
		{
			Debug.Assert(monster != null && !monster.IsCharacterMonster());

			// --> Add effects of monster's death here
		}

		public virtual string GetMonsterWeaponName(IMonster monster)
		{
			Debug.Assert(monster != null);

			var weaponArtifact = monster.Weapon > 0 ? ADB[monster.Weapon] : null;

			return weaponArtifact != null ? weaponArtifact.GetArticleName() : monster.Weapon == 0 ? "natural weapons" : "no weapon";
		}

		public virtual void RevealDisguisedMonster(IRoom room, IArtifact artifact)
		{
			RetCode rc;

			Debug.Assert(room != null);

			Debug.Assert(artifact != null);

			var ac = artifact.DisguisedMonster;

			Debug.Assert(ac != null);

			PrintMonsterAlive(artifact);

			if (ac.Field2 > 0)
			{
				for (var i = 0; i < ac.Field3; i++)
				{
					var effect = EDB[ac.Field2 + i];

					if (effect != null)
					{
						gEngine.Buf.Clear();

						rc = effect.BuildPrintedFullDesc(gEngine.Buf);
					}
					else
					{
						gEngine.Buf.SetPrint("{0}", "???");

						rc = RetCode.Success;
					}

					Debug.Assert(IsSuccess(rc));

					Out.Write("{0}", gEngine.Buf);
				}
			}

			artifact.SetInLimbo();

			var monster = MDB[ac.Field1];

			Debug.Assert(monster != null);

			monster.SetInRoomUid(GameState.Ro);
		}

		public virtual void RevealEmbeddedArtifact(IRoom room, IArtifact artifact)
		{
			Debug.Assert(room != null);

			Debug.Assert(artifact != null);

			var shouldShowUnseenArtifacts = false;

			var containerArtifact = artifact.GetCarriedByContainer();

			while (containerArtifact != null && containerArtifact.IsCarriedByContainer())
			{
				containerArtifact = containerArtifact.GetCarriedByContainer();
			}

			// Move an embedded container into the room

			if (containerArtifact != null && containerArtifact.IsEmbeddedInRoom(room))
			{
				containerArtifact.SetInRoom(room);

				var ac = containerArtifact.DoorGate;

				if (ac != null)
				{
					ac.Field4 = 0;
				}

				shouldShowUnseenArtifacts = true;
			}

			// Move an embedded artifact into the room

			if (artifact.IsEmbeddedInRoom(room))
			{
				artifact.SetInRoom(room);

				var ac = artifact.DoorGate;

				if (ac != null)
				{
					ac.Field4 = 0;
				}

				shouldShowUnseenArtifacts = true;
			}

			if (!shouldShowUnseenArtifacts)
			{
				shouldShowUnseenArtifacts = AutoDisplayUnseenArtifactDescs;
			}

			if (!shouldShowUnseenArtifacts)
			{
				var command = CommandParser.NextState as ICommand;

				shouldShowUnseenArtifacts = command != null && command.ShouldShowUnseenArtifacts(room, artifact);
			}

			// Fully describe an unseen container

			if (shouldShowUnseenArtifacts && containerArtifact != null && !containerArtifact.Seen)
			{
				PrintFullDesc(containerArtifact, false, false);

				containerArtifact.Seen = true;
			}

			// Fully describe an unseen artifact

			if (shouldShowUnseenArtifacts && !artifact.Seen)
			{
				PrintFullDesc(artifact, false, false);

				artifact.Seen = true;
			}
		}

		public virtual void RevealContainerContents(IRoom room, IMonster monster, IArtifact artifact, long location, bool printOutput)
		{
			Debug.Assert(room != null && artifact != null);

			try
			{
				RevealContentCounter--;

				var containerTypes = new ContainerType[] { ContainerType.In, ContainerType.On, ContainerType.Under, ContainerType.Behind };

				var containerTypeList = new List<ContainerType>();

				var containerContentsList = new List<string>();

				if (artifact.IsInLimbo() && location != LimboLocation)
				{
					foreach (var containerType in containerTypes)
					{
						if (artifact.ShouldRevealContentsWhenMovedIntoLimbo(containerType))
						{
							containerTypeList.Add(containerType);
						}
					}
				}
				else if (!artifact.IsInLimbo() && location != LimboLocation)
				{
					foreach (var containerType in containerTypes)
					{
						if (artifact.ShouldRevealContentsWhenMoved(containerType))
						{
							containerTypeList.Add(containerType);
						}
					}
				}

				if (containerTypeList.Count > 0)
				{
					RevealContainerContents02(room, monster, artifact, location, containerTypeList.ToArray(), printOutput && room.Uid == GameState.Ro && room.IsViewable() && monster != null ? containerContentsList : null);
				}

				foreach (var containerContentsDesc in containerContentsList)
				{
					Out.Write("{0}", containerContentsDesc);
				}
			}
			finally
			{
				RevealContentCounter++;
			}
		}

		public virtual void RevealContainerContents02(IRoom room, IMonster monster, IArtifact artifact, long location, ContainerType[] containerTypes, IList<string> containerContentsList = null)
		{
			RetCode rc;

			Debug.Assert(room != null && artifact != null);

			if (containerTypes == null || containerTypes.Length < 1)
			{
				containerTypes = new ContainerType[] { ContainerType.Under, ContainerType.Behind };
			}

			var charMonster = gCharMonster;

			Debug.Assert(charMonster != null);

			IMonster revealMonster = null;

			var showCharOwned = !artifact.IsCarriedByMonster(charMonster) && !artifact.IsWornByMonster(charMonster);

			var recordNameListArgs = CreateInstance<IRecordNameListArgs>(x =>
			{
				x.ArticleType = ArticleType.A;

				x.ShowCharOwned = false;

				x.StateDescCode = StateDescDisplayCode.None;

				x.ShowContents = false;

				x.GroupCountOne = false;
			});

			foreach (var containerType in containerTypes)
			{
				var ac = EvalContainerType(containerType, artifact.InContainer, artifact.OnContainer, artifact.UnderContainer, artifact.BehindContainer);

				if (ac != null)
				{
					var revealContentsList = artifact.GetContainedList(containerType: containerType);

					var revealContentsList02 = revealContentsList.OrderByDescending(a => a.RecursiveWeight).ToList();

					var revealContents = revealContentsList02.Count > 0;

					foreach (var revealArtifact in revealContentsList02)
					{
						revealArtifact.Location = location;

						while (true) 
						{ 
							recordNameListArgs.ShowCharOwned = !revealArtifact.IsCarriedByMonster(charMonster) && !revealArtifact.IsWornByMonster(charMonster);

							revealMonster = revealArtifact.GetCarriedByMonster();

							if (revealMonster == null)
							{
								revealMonster = revealArtifact.GetWornByMonster();
							}

							var revealContainer = revealArtifact.GetCarriedByContainer();

							var revealContainerType = revealArtifact.GetCarriedByContainerContainerType();

							var revealContainerAc = revealContainer != null && Enum.IsDefined(typeof(ContainerType), revealContainerType) ? EvalContainerType(revealContainerType, revealContainer.InContainer, revealContainer.OnContainer, revealContainer.UnderContainer, revealContainer.BehindContainer) : null;

							if (revealArtifact.IsCarriedByMonster(charMonster) || revealArtifact.IsWornByMonster(charMonster))
							{
								var charWeight = 0L;

								rc = charMonster.GetFullInventoryWeight(ref charWeight, recurse: true);

								Debug.Assert(IsSuccess(rc));

								var revealArtifactTooHeavy = charWeight > charMonster.GetWeightCarryableGronds();

								if (revealArtifact.IsWornByMonster(charMonster) && (revealArtifact.Wearable == null || revealArtifactTooHeavy))
								{
									revealArtifact.SetCarriedByMonster(charMonster);
								}

								if (revealArtifact.IsCarriedByMonster(charMonster) && revealArtifactTooHeavy)
								{
									revealArtifact.SetInRoom(room);
								}
							}
							else if (revealMonster != null)
							{
								var monWeight = 0L;

								rc = revealMonster.GetFullInventoryWeight(ref monWeight, recurse: true);

								Debug.Assert(IsSuccess(rc));

								var revealArtifactTooHeavy = EnforceMonsterWeightLimits && (revealArtifact.RecursiveWeight > revealMonster.GetWeightCarryableGronds() || monWeight > revealMonster.GetWeightCarryableGronds() * revealMonster.CurrGroupCount);

								if (revealArtifact.IsWornByMonster(revealMonster) && (revealArtifact.Wearable == null || revealArtifactTooHeavy))
								{
									revealArtifact.SetCarriedByMonster(revealMonster);
								}

								if (revealArtifact.IsCarriedByMonster(revealMonster) && revealArtifactTooHeavy)
								{
									revealArtifact.SetInRoom(room);
								}
							}
							else if (revealContainer != null && revealContainerAc != null)
							{
								var count = 0L;

								var weight = 0L;

								rc = revealContainer.GetContainerInfo(ref count, ref weight, revealContainerType, false);

								Debug.Assert(IsSuccess(rc));

								if (count > revealContainerAc.Field4 || weight > revealContainerAc.Field3)
								{
									revealArtifact.Location = revealContainer.Location;

									continue;
								}
							}
							else if (revealArtifact.IsEmbeddedInRoom(room))
							{
								if (artifact.IsInRoom(room))
								{
									revealArtifact.SetCarriedByContainer(artifact, containerType);

									revealContents = false;
								}
								else
								{
									revealArtifact.SetInRoom(room);
								}
							}
							else if (revealArtifact.IsInLimbo())
							{
								revealArtifact.SetCarriedByContainer(artifact, containerType);

								revealContents = false;
							}

							break;
						}
					}

					if (revealContents && containerContentsList != null)
					{
						BuildRevealContentsListDescString(revealMonster != null ? revealMonster : monster, artifact, revealContentsList, containerType, showCharOwned, recordNameListArgs);

						containerContentsList.Add(gEngine.Buf.ToString());
					}
				}
			}
		}

		public virtual IArtifact GetBlockedDirectionArtifact(long ro, long r2, Direction dir)
		{
			return null;
		}

		public virtual ICommand GetCommandUsingToken(IMonster monster, string token, bool synonymMatch = true, bool partialMatch = true)
		{
			Debug.Assert(monster != null);

			Debug.Assert(!string.IsNullOrWhiteSpace(token));

			var command = CommandList.FirstOrDefault(x =>
			{
				var result = false;

				result = x.Verb != null && x.Verb.Equals(token, StringComparison.OrdinalIgnoreCase) && x.IsEnabled(monster);

				if (result)
				{
					x.ParserMatchName = CloneInstance(x.Verb);
				}

				return result;
			});

			if (command == null && synonymMatch)
			{
				command = CommandList.FirstOrDefault(x =>
				{
					var result = false;

					result = x.Synonyms != null && x.Synonyms.FirstOrDefault(s =>
					{
						var result01 = false;

						result01 = s.Equals(token, StringComparison.OrdinalIgnoreCase);

						if (result01)
						{
							x.ParserMatchName = CloneInstance(s);
						}

						return result01;

					}) != null && x.IsEnabled(monster);

					return result;
				});
			}

			if (command == null && partialMatch)
			{
				command = CommandList.FirstOrDefault(x =>
				{
					var result = false;

					result = x.Verb != null && (x.Verb.StartsWith(token, StringComparison.OrdinalIgnoreCase) || x.Verb.EndsWith(token, StringComparison.OrdinalIgnoreCase)) && x.IsEnabled(monster);

					if (result)
					{
						x.ParserMatchName = CloneInstance(x.Verb);
					}

					return result;
				});
			}

			if (command == null && synonymMatch && partialMatch)
			{
				command = CommandList.FirstOrDefault(x =>
				{
					var result = false;

					result = x.Synonyms != null && x.Synonyms.FirstOrDefault(s =>
					{
						var result01 = false;

						result01 = s.StartsWith(token, StringComparison.OrdinalIgnoreCase) || s.EndsWith(token, StringComparison.OrdinalIgnoreCase);

						if (result01)
						{
							x.ParserMatchName = CloneInstance(s);
						}

						return result01;

					}) != null && x.IsEnabled(monster);

					return result;
				});
			}

			return command;
		}

		public virtual void CheckDoor(IRoom room, IArtifact artifact, ref bool found, ref long roomUid)
		{
			Debug.Assert(room != null);

			Debug.Assert(artifact != null);

			found = false;

			roomUid = 0;

			if (!artifact.IsCharOwned)
			{
				var ac = artifact.DoorGate;

				if (ac != null)
				{
					if (artifact.IsInRoom(room) || artifact.IsEmbeddedInRoom(room) || artifact.IsCarriedByContainerContainerTypeExposedToRoom(room))
					{
						found = true;
					}
				}

				if (found)
				{
					if (artifact.Seen && room.IsViewable())
					{
						ac.Field4 = 0;
					}

					if (ac.Field4 != 0)
					{
						found = false;
					}
				}

				if (found && ac.IsOpen())
				{
					roomUid = ac.Field1;
				}
			}
		}

		public virtual void CheckNumberOfExits(IRoom room, IMonster monster, bool fleeing, ref long numExits)
		{
			Debug.Assert(room != null);

			Debug.Assert(monster != null);

			numExits = 0;

			var directionValues = EnumUtil.GetValues<Direction>();

			for (var i = 0; i < Module.NumDirs; i++)
			{
				var dv = directionValues[i];

				var found = false;

				var roomUid = 0L;

				var artUid = room.GetDirectionDoorUid(dv);

				if (artUid > 0)
				{
					var artifact = ADB[artUid];

					Debug.Assert(artifact != null);

					CheckDoor(room, artifact, ref found, ref roomUid);
				}
				else
				{
					roomUid = room.GetDir(dv);
				}

				if (roomUid != 0 && (!monster.CanMoveInDirection(dv, fleeing) || !monster.CanMoveToRoomUid(roomUid, fleeing) || GetBlockedDirectionArtifact(room.Uid, roomUid, dv) != null))
				{
					roomUid = 0;
				}

				if (IsValidRoomUid01(roomUid) && (monster.IsCharacterMonster() || (roomUid > 0 && RDB[roomUid] != null)))
				{
					numExits++;
				}
			}
		}

		public virtual void GetRandomMoveDirection(IRoom room, IMonster monster, bool fleeing, ref Direction direction, ref bool found, ref long roomUid)
		{
			long rl;

#if DEBUG

			long numExits = 0;

			CheckNumberOfExits(room, monster, fleeing, ref numExits);

			Debug.Assert(numExits > 0);

#endif

			direction = 0;

			do
			{
				rl = RollDice(1, Module.NumDirs, 0);

				found = false;

				roomUid = 0;

				var artUid = room.GetDirectionDoorUid((Direction)rl);

				if (artUid > 0)
				{
					var artifact = ADB[artUid];

					Debug.Assert(artifact != null);

					CheckDoor(room, artifact, ref found, ref roomUid);
				}
				else
				{
					roomUid = room.GetDir(rl);
				}

				if (roomUid != 0 && (!monster.CanMoveInDirection((Direction)rl, fleeing) || !monster.CanMoveToRoomUid(roomUid, fleeing) || GetBlockedDirectionArtifact(room.Uid, roomUid, (Direction)rl) != null))
				{
					roomUid = 0;
				}
			}
			while (roomUid == 0 || !IsValidRandomMoveDirection(room.Uid, roomUid) || IsValidRoomDirectionDoorUid01(roomUid) || (!monster.IsCharacterMonster() && (roomUid < 1 || RDB[roomUid] == null)));

			direction = (Direction)rl;
		}

		public virtual void GetRandomMoveDirection(IRoom room, IMonster monster, bool fleeing, ref Direction direction)
		{
			var found = false;

			var roomUid = 0L;

			GetRandomMoveDirection(room, monster, fleeing, ref direction, ref found, ref roomUid);
		}

		public virtual void MoveMonsterToRandomAdjacentRoom(IRoom room, IMonster monster, bool fleeing, bool pauseCombat, bool printOutput = true)
		{
			RetCode rc;

			Debug.Assert(room != null);

			Debug.Assert(monster != null);

			Debug.Assert(gCharMonster != null);

			var numExits = 0L;

			CheckNumberOfExits(room, monster, fleeing, ref numExits);

			var rl = fleeing ? monster.GetFleeingMemberCount() : monster.CurrGroupCount;

			var monster01 = CloneInstance(monster);

			Debug.Assert(monster01 != null);

			monster01.CurrGroupCount = rl;

			var monsterName = monster01.EvalInRoomViewability(rl > 1 ? "Unseen entities" : "An unseen entity", monster01.InitGroupCount > rl ? monster01.GetArticleName(true) : monster01.GetTheName(true));

			if (numExits == 0)
			{
				if (gCharMonster.IsInRoom(room) && printOutput)
				{
					PrintMonsterCantFindExit(monster, room, monsterName, rl > 1, fleeing);

					if (pauseCombat)
					{
						PauseCombat();
					}
				}

				if (rl < monster.CurrGroupCount)
				{
					monster.CurrGroupCount -= rl;

					LoopFailedMoveMemberCount = rl;
				}

				goto Cleanup;
			}

			if (rl < monster.CurrGroupCount)
			{
				monster.CurrGroupCount -= rl;

				if (gCharMonster.IsInRoom(room) && printOutput)
				{
					PrintMonsterMembersExitRoom(monster, room, monsterName, rl > 1, fleeing);

					if (pauseCombat)
					{
						PauseCombat();
					}
				}

				if (EnforceMonsterWeightLimits)
				{
					rc = monster.EnforceFullInventoryWeightLimits(recurse: true);

					Debug.Assert(IsSuccess(rc));
				}
			}
			else
			{
				Direction direction = 0;

				var found = false;

				var roomUid = 0L;

				GetRandomMoveDirection(room, monster, fleeing, ref direction, ref found, ref roomUid);

				Debug.Assert(Enum.IsDefined(typeof(Direction), direction));

				Debug.Assert(roomUid > 0);

				if (gCharMonster.IsInRoom(room) && printOutput)
				{
					PrintMonsterExitsRoom(monster, room, monsterName, rl > 1, fleeing, direction);

					if (pauseCombat)
					{
						PauseCombat();
					}
				}

				monster.Location = roomUid;

				var room01 = RDB[roomUid];

				Debug.Assert(room01 != null);

				var monsterName01 = monster.EvalInRoomViewability(rl > 1 ? "Unseen entities" : "An unseen entity", monster.GetArticleName(true));

				var direction01 = GetDirection(direction);

				Debug.Assert(direction01 != null);

				if (gCharMonster.IsInRoom(room01) && printOutput)
				{
					PrintMonsterEntersRoom(monster, room01, monsterName01, rl > 1, fleeing, direction01.EnterDir);

					if (pauseCombat)
					{
						PauseCombat();
					}
				}
			}

		Cleanup:

			;
		}

		public virtual IList<IMonster> GetRandomMonsterList(long numMonsters, params Func<IMonster, bool>[] whereClauseFuncs)
		{
			Debug.Assert(numMonsters > 0);

			var monsterList = new List<IMonster>();

			var origMonsterList = GetMonsterList(whereClauseFuncs);

			if (numMonsters > origMonsterList.Count)
			{
				numMonsters = origMonsterList.Count;
			}

			while (numMonsters > 0)
			{
				var rl = (int)RollDice(1, origMonsterList.Count, 0);

				monsterList.Add(origMonsterList[rl - 1]);

				origMonsterList.RemoveAt(rl - 1);

				numMonsters--;
			}

			return monsterList;
		}

		public virtual IList<IGameBase> FilterRecordList(IList<IGameBase> recordList, string name)
		{
			Debug.Assert(recordList != null);

			Debug.Assert(!string.IsNullOrWhiteSpace(name));

			var name01 = CloneInstance(name);

			var tokens = name01.Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);

			long i = -1;

			if (tokens.Length > 1)
			{
				i = GetNumberFromString(tokens[0]);

				if (i > 0)
				{
					name01 = name01.Substring(tokens[0].Length + 1);
				}
				else
				{
					i = -1;
				}
			}

			var filteredRecordList = recordList.Where(r =>
			{
				var result = false;

				var cmpName = r is IArtifact ? name : name01;

				result = r.Name.Equals(cmpName, StringComparison.OrdinalIgnoreCase);

				if (result)
				{
					r.ParserMatchName = CloneInstance(r.Name);
				}

				return result;

			}).ToList();

			if (filteredRecordList.Count == 0)
			{
				filteredRecordList = recordList.Where(r =>
				{
					var result = false;

					var cmpName = r is IArtifact ? name : name01;

					result = r.Name.StartsWith(cmpName, StringComparison.OrdinalIgnoreCase) || r.Name.EndsWith(cmpName, StringComparison.OrdinalIgnoreCase);

					if (result)
					{
						r.ParserMatchName = CloneInstance(r.Name);
					}

					return result;

				}).ToList();
			}

			/*
			if (filteredRecordList.Count == 0)
			{
				filteredRecordList = recordList.Where(r =>
				{
					var result = false;

					var cmpName = r is IArtifact ? name : name01;

					tokens = r.Name.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

					result = tokens.FirstOrDefault(t =>
					{
						var result01 = false;

						if (!IgnoredTokenHashSet.Contains(t))
						{
							result01 = t.Equals(cmpName, StringComparison.OrdinalIgnoreCase);
						}

						return result01;

					}) != null;

					if (result)
					{
						r.ParserMatchName = CloneInstance(r.Name);
					}

					return result;

				}).ToList();
			}

			if (filteredRecordList.Count == 0)
			{
				filteredRecordList = recordList.Where(r =>
				{
					var result = false;

					var cmpName = r is IArtifact ? name : name01;

					tokens = r.Name.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

					result = tokens.FirstOrDefault(t =>
					{
						var result01 = false;

						if (!IgnoredTokenHashSet.Contains(t))
						{
							result01 = t.StartsWith(cmpName, StringComparison.OrdinalIgnoreCase) || t.EndsWith(cmpName, StringComparison.OrdinalIgnoreCase);
						}

						return result01;

					}) != null;

					if (result)
					{
						r.ParserMatchName = CloneInstance(r.Name);
					}

					return result;

				}).ToList();
			}
			*/

			if (filteredRecordList.Count == 0)
			{
				filteredRecordList = recordList.Where(r =>
				{
					var result = false;

					var pluralName = r.GetPluralName01();

					if (r is IArtifact a)
					{
						result = a.IsPlural && pluralName.Equals(name, StringComparison.OrdinalIgnoreCase);
					}
					else if (r is IMonster m)
					{
						result = m.GroupCount > 1 && pluralName.Equals(name01, StringComparison.OrdinalIgnoreCase);
					}

					if (result)
					{
						r.ParserMatchName = CloneInstance(pluralName);
					}

					return result;

				}).ToList();
			}

			if (filteredRecordList.Count == 0)
			{
				filteredRecordList = recordList.Where(r =>
				{
					var result = false;

					var pluralName = r.GetPluralName01();

					if (r is IArtifact a)
					{
						result = a.IsPlural && (pluralName.StartsWith(name, StringComparison.OrdinalIgnoreCase) || pluralName.EndsWith(name, StringComparison.OrdinalIgnoreCase));
					}
					else if (r is IMonster m)
					{
						result = m.GroupCount > 1 && (pluralName.StartsWith(name01, StringComparison.OrdinalIgnoreCase) || pluralName.EndsWith(name01, StringComparison.OrdinalIgnoreCase));
					}

					if (result)
					{
						r.ParserMatchName = CloneInstance(pluralName);
					}

					return result;

				}).ToList();
			}

			/*
			if (filteredRecordList.Count == 0)
			{
				filteredRecordList = recordList.Where(r =>
				{
					var result = false;

					var pluralName = r.GetPluralName01();

					tokens = pluralName.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

					result = tokens.FirstOrDefault(t =>
					{
						var result01 = false;

						if (!IgnoredTokenHashSet.Contains(t))
						{
							if (r is IArtifact a)
							{
								result01 = a.IsPlural && t.Equals(name, StringComparison.OrdinalIgnoreCase);
							}
							else if (r is IMonster m)
							{
								result01 = m.GroupCount > 1 && t.Equals(name01, StringComparison.OrdinalIgnoreCase);
							}
						}

						return result01;

					}) != null;

					if (result)
					{
						r.ParserMatchName = CloneInstance(pluralName);
					}

					return result;

				}).ToList();
			}

			if (filteredRecordList.Count == 0)
			{
				filteredRecordList = recordList.Where(r =>
				{
					var result = false;

					var pluralName = r.GetPluralName01();

					tokens = pluralName.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

					result = tokens.FirstOrDefault(t =>
					{
						var result01 = false;

						if (!IgnoredTokenHashSet.Contains(t))
						{
							if (r is IArtifact a)
							{
								result01 = a.IsPlural && (t.StartsWith(name, StringComparison.OrdinalIgnoreCase) || t.EndsWith(name, StringComparison.OrdinalIgnoreCase));
							}
							else if (r is IMonster m)
							{
								result01 = m.GroupCount > 1 && (t.StartsWith(name01, StringComparison.OrdinalIgnoreCase) || t.EndsWith(name01, StringComparison.OrdinalIgnoreCase));
							}
						}

						return result01;

					}) != null;

					if (result)
					{
						r.ParserMatchName = CloneInstance(pluralName);
					}

					return result;

				}).ToList();
			}
			*/

			if (filteredRecordList.Count == 0)
			{
				filteredRecordList = recordList.Where(r =>
				{
					var result = false;

					var cmpName = r is IArtifact ? name : name01;

					result = r.Synonyms != null && r.Synonyms.FirstOrDefault(s =>
					{
						var result01 = false;

						result01 = s.Equals(cmpName, StringComparison.OrdinalIgnoreCase);

						if (result01)
						{
							r.ParserMatchName = CloneInstance(s);
						}

						return result01;

					}) != null;

					return result;

				}).ToList();
			}

			if (filteredRecordList.Count == 0)
			{
				filteredRecordList = recordList.Where(r =>
				{
					var result = false;

					var cmpName = r is IArtifact ? name : name01;

					result = r.Synonyms != null && r.Synonyms.FirstOrDefault(s =>
					{
						var result01 = false;

						result01 = s.StartsWith(cmpName, StringComparison.OrdinalIgnoreCase) || s.EndsWith(cmpName, StringComparison.OrdinalIgnoreCase);

						if (result01)
						{
							r.ParserMatchName = CloneInstance(s);
						}

						return result01;

					}) != null;

					return result;

				}).ToList();
			}

			/*
			if (filteredRecordList.Count == 0)
			{
				filteredRecordList = recordList.Where(r =>
				{
					var result = false;

					var cmpName = r is IArtifact ? name : name01;

					if (r.Synonyms != null)
					{
						foreach (var s in r.Synonyms)
						{
							tokens = s.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

							result = tokens.FirstOrDefault(t =>
							{
								var result01 = false;

								if (!IgnoredTokenHashSet.Contains(t))
								{
									result01 = t.Equals(cmpName, StringComparison.OrdinalIgnoreCase);
								}

								return result01;

							}) != null;

							if (result)
							{
								r.ParserMatchName = CloneInstance(s);

								break;
							}
						}
					}

					return result;

				}).ToList();
			}

			if (filteredRecordList.Count == 0)
			{
				filteredRecordList = recordList.Where(r =>
				{
					var result = false;

					var cmpName = r is IArtifact ? name : name01;

					if (r.Synonyms != null)
					{
						foreach (var s in r.Synonyms)
						{
							tokens = s.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

							result = tokens.FirstOrDefault(t =>
							{
								var result01 = false;

								if (!IgnoredTokenHashSet.Contains(t))
								{
									result01 = t.StartsWith(cmpName, StringComparison.OrdinalIgnoreCase) || t.EndsWith(cmpName, StringComparison.OrdinalIgnoreCase);
								}

								return result01;

							}) != null;

							if (result)
							{
								r.ParserMatchName = CloneInstance(s);

								break;
							}
						}
					}

					return result;

				}).ToList();
			}
			*/

			filteredRecordList = filteredRecordList.Distinct().GroupBy(r =>
			{
				var result = r.Name.ToLower();

				if (r is IArtifact a && a.IsPlural)
				{
					result = a.GetPluralName01().ToLower();
				}
				else if (r is IMonster m && m.GroupCount > 1)
				{
					result = m.GetPluralName01().ToLower();
				}

				return result;

			}).Select(r => r.FirstOrDefault()).OrderBy(r => r is IArtifact ? 2 : 1).ToList();

			if (i > 0)
			{
				filteredRecordList.RemoveAll(r => r is IMonster m && (m.GroupCount == 1 || m.GroupCount < i));
			}

			return filteredRecordList;
		}

		public virtual IList<IArtifact> GetReadyableWeaponList(IMonster monster)
		{
			Debug.Assert(monster != null);

			var charMonster = gCharMonster;

			Debug.Assert(charMonster != null);

			var room = monster.GetInRoom();

			Debug.Assert(room != null);

			var monsterList = GetMonsterList(m => m.Uid != monster.Uid && m.Uid != charMonster.Uid && m.IsInRoom(room));

			var artifactList = GetArtifactList(a =>
			{
				var result = false;

				if (a.IsReadyableByMonster(monster))
				{
					if (a.IsCarriedByMonster(monster))
					{
						result = true;
					}
					else if (a.IsInRoom(room))
					{
						result = monsterList.FirstOrDefault(m => m.Weapon == -a.Uid - 1) == null &&
										(monster.Weapon == -a.Uid - 1 || a.Seen || !room.IsLit()) &&
										(charMonster.Weapon > 0 || !a.IsCharOwned);
					}
					else if (a.IsCarriedByContainerContainerTypeExposedToMonster(monster, ExposeContainersRecursively) || a.IsCarriedByContainerContainerTypeExposedToRoom(room, ExposeContainersRecursively))
					{
						result = !IsRulesetVersion(5, 62) &&
										monsterList.FirstOrDefault(m => m.Weapon == -a.Uid - 1) == null &&
										(monster.Weapon == -a.Uid - 1 || a.GetCarriedByContainer().Seen || !room.IsLit()) &&
										(monster.Weapon == -a.Uid - 1 || a.Seen || !room.IsLit()) &&
										(charMonster.Weapon > 0 || !a.IsCharOwned);
					}
				}

				return result;

			}).OrderByDescending(a01 =>
			{
				if (monster.Weapon != -a01.Uid - 1)
				{
					var ac = a01.GeneralWeapon;

					Debug.Assert(ac != null);

					return ac.Field3 * ac.Field4;
				}
				else
				{
					return long.MaxValue;
				}
			}).ThenByDescending(a02 =>
			{
				if (a02.IsCarriedByMonster(monster))
				{
					return 2;
				}
				else if (a02.IsCarriedByMonster(monster, true))
				{
					return 1;
				}
				else
				{
					return 0;
				}
			}).ToList();

			// Filter out two-handed weapons if monster wearing shield

			var shield = monster.GetWornList().FirstOrDefault(a =>
			{
				var ac = a.Wearable;

				Debug.Assert(ac != null);

				return ac.Field1 == 1;
			});

			if (shield != null)
			{
				artifactList = artifactList.Where(a =>
				{
					var ac = a.GeneralWeapon;

					Debug.Assert(ac != null);

					return ac.Field5 < 2;

				}).ToList();
			}

			return artifactList;
		}

		public virtual IList<IMonster> GetEmotingMonsterList(IRoom room, IMonster monster, bool friendSmile = true)
		{
			Debug.Assert(room != null && monster != null);

			return room.IsViewable() ? GetMonsterList(m => m.IsInRoom(room) && m != monster) : new List<IMonster>();
		}

		public virtual IList<IArtifact> BuildLoopWeaponArtifactList(IMonster monster)
		{
			Debug.Assert(monster != null);

			IList<IArtifact> artifactList = null;

			if ((monster.CombatCode == CombatCode.NaturalWeapons || monster.CombatCode == CombatCode.NaturalAttacks) && monster.Weapon <= 0)
			{
				artifactList = GetReadyableWeaponList(monster);

				if (artifactList != null && artifactList.Count > 0)
				{
					var wpnArtifact = artifactList[0];

					Debug.Assert(wpnArtifact != null);

					var ac = wpnArtifact.GeneralWeapon;

					Debug.Assert(ac != null);

					if (monster.Weapon != -wpnArtifact.Uid - 1 && monster.NwDice * monster.NwSides > ac.Field3 * ac.Field4 && monster.ShouldPreferNaturalWeaponsToWeakerWeapon(wpnArtifact))
					{
						artifactList = null;
					}
				}
			}
			else if ((monster.CombatCode == CombatCode.Weapons || monster.CombatCode == CombatCode.Attacks) && monster.Weapon < 0)
			{
				artifactList = GetReadyableWeaponList(monster);
			}

			return artifactList;
		}

		public virtual IList<IArtifact> GetImportedPlayerInventory()
		{
			var artifactList = new List<IArtifact>();

			if (GameState != null)
			{
				for (var i = 0; i < GameState.ImportedArtUids.Count; i++)
				{
					var artifact = ADB[GameState.GetImportedArtUid(i)];

					Debug.Assert(artifact != null);

					artifactList.Add(artifact);
				}
			}

			return artifactList;
		}

		public virtual void HideImportedPlayerInventory()
		{
			// Look up the player character

			var charMonster = gCharMonster;

			Debug.Assert(charMonster != null);

			// Grab a random Room - we'll use StartRoom here since it really doesn't matter

			var room = RDB[StartRoom];

			Debug.Assert(room != null);

			// This queries the game database for all Artifacts brought in by the player character

			var artifactList = GetImportedPlayerInventory();

			// Suppress output to the console

			Out.EnableOutput = false;

			foreach (var artifact in artifactList)
			{
				// If the Artifact is worn by the player character

				if (artifact.IsWornByMonster(charMonster))
				{
					// In order to keep the game state from getting messed up we have to simulate an actual RemoveCommand execution

					CurrState = CreateInstance<IRemoveCommand>(x =>
					{
						x.ActorMonster = charMonster;

						x.ActorRoom = room;

						x.Dobj = artifact;
					});

					// Execute the RemoveCommand... after this the Artifact will no longer be worn (and the game state will be properly updated)

					CurrCommand.Execute();
				}

				// In order to keep the game state from getting messed up we have to simulate an actual DropCommand execution

				CurrState = CreateInstance<IDropCommand>(x =>
				{
					x.ActorMonster = charMonster;

					x.ActorRoom = room;

					x.Dobj = artifact;
				});

				// Execute the DropCommand... after this the Artifact will no longer be carried (and the game state will be properly updated)

				CurrCommand.Execute();

				// Now we can move it into limbo without messing anything up

				artifact.SetInLimbo();
			}

			// Enable console output again

			Out.EnableOutput = true;

			// We need to recreate the initial game state since we were creating and executing the fake Commands above

			CreateInitialState(false);
		}

		public virtual void RestoreImportedPlayerInventory()
		{
			// Look up the player character

			var charMonster = gCharMonster;

			Debug.Assert(charMonster != null);

			// Grab a random Room - we'll use StartRoom here since it really doesn't matter

			var room = RDB[StartRoom];

			Debug.Assert(room != null);

			// This queries the game database for all Artifacts brought in by the player character

			var artifactList = GetImportedPlayerInventory();

			// Suppress output to the console

			Out.EnableOutput = false;

			foreach (var artifact in artifactList)
			{
				// First make the Artifact carried by the player character... you don't need to put it in Room and then do
				// GetCommand because there isn't really any game state to mess up... but you could if you wanted to I guess
				// just to be safe

				artifact.SetCarriedByMonster(charMonster);

				// If the Artifact is Wearable, make sure it's worn (otherwise it will accidentally get sold to Sam Slicker)

				if (artifact.Wearable != null)
				{
					// In order to keep the game state from getting messed up we have to simulate an actual WearCommand execution

					CurrState = CreateInstance<IWearCommand>(x =>
					{
						x.ActorMonster = charMonster;

						x.ActorRoom = room;

						x.Dobj = artifact;
					});

					// Execute the WearCommand... after this the Artifact will be worn (and the game state will be properly updated)

					CurrCommand.Execute();
				}
			}

			// Enable console output again

			Out.EnableOutput = true;

			// (No need to update CurrState to anything else since we're shutting down)
		}

		public virtual RetCode BuildCommandList(IList<ICommand> commands, CommandType cmdType, StringBuilder buf, ref bool newSeen)
		{
			StringBuilder buf02, buf03;
			RetCode rc;
			int i;

			if (commands == null || !Enum.IsDefined(typeof(CommandType), cmdType) || buf == null)
			{
				rc = RetCode.InvalidArg;

				// PrintError

				goto Cleanup;
			}

			rc = RetCode.Success;

			buf02 = new StringBuilder(BufSize);

			buf03 = new StringBuilder(BufSize);

			i = 0;

			foreach (var c in commands)
			{
				if (cmdType == CommandType.None || c.Type == cmdType)
				{
					i++;

					buf02.SetFormat("{0}{1}",
						c.GetPrintedVerb(),
						c.IsNew ? " (*)" : "");

					buf03.AppendFormat("{0,-15}{1}",
						buf02.ToString(),
						(i % 5) == 0 ? Environment.NewLine : "");

					if (c.IsNew)
					{
						newSeen = true;
					}
				}
			}

			buf.AppendFormat("{0}{1}{2}",
				Environment.NewLine,
				buf03.Length > 0 ? buf03.ToString() : "(None)",
				i == 0 || (i % 5) != 0 ? Environment.NewLine : "");

		Cleanup:

			return rc;
		}

		public virtual StringBuilder NormalizePlayerInput(StringBuilder buf)
		{
			Debug.Assert(buf != null);

			var guid = Guid.NewGuid();

			var oneGuidString = string.Format(" {0} ", guid.ToString());

			var twoGuidString = string.Format(" {0} {0} ", guid.ToString());

			buf.SetFormat(" {0} ", buf.ToString().ToLower());

			while (Regex.IsMatch(buf.ToString(), EverythingRegexPattern))
			{
				buf.SetFormat("{0}", Regex.Replace(buf.ToString(), EverythingRegexPattern, " all "));
			}

			while (Regex.IsMatch(buf.ToString(), ExceptRegexPattern))
			{
				buf.SetFormat("{0}", Regex.Replace(buf.ToString(), ExceptRegexPattern, " but "));
			}

			while (Regex.IsMatch(buf.ToString(), CommandSepRegexPattern))
			{
				buf.SetFormat("{0}", Regex.Replace(buf.ToString(), CommandSepRegexPattern, oneGuidString));
			}

			buf.SetFormat(" {0} ", Regex.Replace(buf.ToString(), @"\s+", " ").Trim());

			while (buf.ToString().IndexOf(twoGuidString) >= 0)
			{
				buf.SetFormat("{0}", buf.ToString().Replace(twoGuidString, oneGuidString));
			}

			while (buf.ToString().StartsWith(oneGuidString))
			{
				buf.SetFormat("{0}", buf.ToString().Substring(oneGuidString.Length));
			}

			while (buf.ToString().EndsWith(oneGuidString))
			{
				buf.SetFormat("{0}", buf.ToString().Substring(0, buf.Length - oneGuidString.Length));
			}

			buf.SetFormat("{0}", buf.ToString().Replace(guid.ToString(), ","));

			return buf.Trim();
		}

		public virtual StringBuilder ReplacePrepositions(StringBuilder buf)
		{
			Debug.Assert(buf != null);

			buf.SetFormat(" {0} ", buf.ToString().ToLower());

			buf = buf.Replace(" in to ", " into ");

			buf = buf.Replace(" inside of ", " in ");

			buf = buf.Replace(" inside ", " in ");

			buf = buf.Replace(" from in ", " fromin ");

			buf = buf.Replace(" outside ", " out ");
			
			buf = buf.Replace(" on to ", " onto ");

			buf = buf.Replace(" on top of ", " on ");

			buf = buf.Replace(" from on ", " fromon ");

			buf = buf.Replace(" below ", " under ").Replace(" beneath ", " under ").Replace(" underneath ", " under ");

			buf = buf.Replace(" from under ", " fromunder ");

			buf = buf.Replace(" in back of ", " behind ");

			buf = buf.Replace(" from behind ", " frombehind ");

			return buf.Trim();
		}

		public virtual bool IsValidRandomMoveDirection(long oldRoomUid, long newRoomUid)
		{
			return oldRoomUid != newRoomUid;
		}

		public virtual bool IsQuotedStringCommand(ICommand command)
		{
			Debug.Assert(command != null);

			// These Commands are free form and may contain quoted strings

			return command is ISaveCommand || command is ISayCommand;
		}

		public virtual bool ResurrectDeadBodies(IRoom room, params Func<IArtifact, bool>[] whereClauseFuncs)
		{
			Debug.Assert(room != null);

			if (whereClauseFuncs == null || whereClauseFuncs.Length == 0)
			{
				whereClauseFuncs = new Func<IArtifact, bool>[]
				{
					a => (a.IsCarriedByMonster(gCharMonster) || a.IsInRoom(room)) && a.DeadBody != null
				};
			}

			var found = false;

			var artifactList = GetArtifactList(whereClauseFuncs);

			foreach (var artifact in artifactList)
			{
				var monster = Database.MonsterTable.Records.FirstOrDefault(m => m.DeadBody == artifact.Uid);

				if (monster != null && monster.GroupCount == 1)
				{
					monster.SetInRoom(room);

					monster.Parry = monster.InitParry;

					monster.DmgTaken = 0;

					artifact.SetInLimbo();

					PrintDeadBodyComesToLife(artifact);

					found = true;
				}
			}

			return found;
		}

		public virtual bool MakeArtifactsVanish(IRoom room, params Func<IArtifact, bool>[] whereClauseFuncs)
		{
			Debug.Assert(room != null);

			if (whereClauseFuncs == null || whereClauseFuncs.Length == 0)
			{
				whereClauseFuncs = new Func<IArtifact, bool>[]
				{
					a => a.IsInRoom(room) && !a.IsUnmovable()
				};
			}

			var artifactList = GetArtifactList(whereClauseFuncs);

			foreach (var artifact in artifactList)
			{
				artifact.SetInLimbo();

				PrintArtifactVanishes(artifact);
			}

			return artifactList.Count > 0;
		}

		public virtual bool SaveThrow(Stat stat, long bonus = 0)
		{
			Debug.Assert(gCharMonster != null);

			var value = 0L;

			switch (stat)
			{
				case Stat.Hardiness:

					// This is the saving throw vs. opening doors, etc

					value = gCharMonster.Hardiness + bonus;

					break;

				case Stat.Agility:

					// This is the saving throw vs. avoiding traps, etc

					value = gCharMonster.Agility + bonus;

					break;

				case Stat.Intellect:

					// This is the saving throw vs. searching, etc

					value = Character.GetStat(Stat.Intellect) + bonus;

					break;

				default:

					// This is the saving throw vs. death or magic

					value = (long)Math.Round((double)(gCharMonster.Agility + Character.GetStat(Stat.Charisma) + gCharMonster.Hardiness) / 3.0) + bonus;

					break;
			}

			var rl = RollDice(1, 22, 2);

			return rl <= value;
		}

		public virtual void DamageWeaponsAndArmor(IRoom room, IMonster monster, long damage = 1, bool recurse = false)
		{
			Debug.Assert(room != null);

			Debug.Assert(monster != null);

			Debug.Assert(damage > 0);

			// Damage weapons

			var artifactList = GetArtifactList(a => (a.IsCarriedByMonster(monster, recurse) || a.IsWornByMonster(monster, recurse)) && !a.IsWornByMonster(monster) && a.GeneralWeapon != null);

			foreach (var artifact in artifactList)
			{
				artifact.GeneralWeapon.Field4 = Math.Max(0, artifact.GeneralWeapon.Field4 - damage);

				if (artifact.GeneralWeapon.Field4 <= 0)
				{
					if (artifact.IsCarriedByMonster(MonsterType.Any))
					{
						PrintArtifactBreaks(room, monster, artifact);
					}

					if (monster.Weapon == artifact.Uid)
					{
						artifact.RemoveStateDesc(artifact.GetReadyWeaponDesc());

						monster.Weapon = -1;
					}

					artifact.SetInLimbo();

					artifact.GeneralWeapon.Field4 = 1;
				}
			}

			// Damage armor

			artifactList = GetArtifactList(a => (a.IsCarriedByMonster(monster, recurse) || a.IsWornByMonster(monster, recurse)) && a.Wearable != null && a.Wearable.Field1 > 0 && a.Wearable.Field2 == (long)Clothing.ArmorShields);

			Out.EnableOutput = false;

			foreach (var artifact in artifactList)
			{
				var wornByChar = artifact.IsWornByMonster(gCharMonster);

				var wornByMonster = artifact.IsWornByMonster();

				if (wornByChar)
				{
					var command = CreateInstance<IRemoveCommand>(x =>
					{
						x.ActorMonster = monster;

						x.ActorRoom = room;

						x.Dobj = artifact;
					});

					command.Execute();
				}
				else if (wornByMonster)
				{
					/*
					var command = CreateInstance<IMonsterRemoveCommand>(x =>
					{
						x.ActorMonster = monster;

						x.ActorRoom = room;

						x.Dobj = artifact;
					});

					command.Execute();
					*/
				}

				artifact.Wearable.Field1 = Math.Max(0, artifact.Wearable.Field1 - damage);

				while (artifact.Wearable.Field1 > 0 && !IsValidArtifactArmor(artifact.Wearable.Field1, false))
				{
					artifact.Wearable.Field1--;
				}

				if (artifact.Wearable.Field1 <= 0)
				{
					if (artifact.IsCarriedByMonster(MonsterType.Any))
					{
						Out.EnableOutput = true;

						PrintArtifactBreaks(room, monster, artifact);

						Out.EnableOutput = false;
					}

					artifact.SetInLimbo();

					artifact.Wearable.Field1 = 0;
				}

				if (wornByChar && artifact.IsCarriedByMonster(gCharMonster))
				{
					var command = CreateInstance<IWearCommand>(x =>
					{
						x.ActorMonster = monster;

						x.ActorRoom = room;

						x.Dobj = artifact;
					});

					command.Execute();
				}
				else if (wornByMonster && artifact.IsCarriedByMonster())
				{
					/*
					var command = CreateInstance<IMonsterWearCommand>(x =>
					{
						x.ActorMonster = monster;

						x.ActorRoom = room;

						x.Dobj = artifact;
					});

					command.Execute();
					*/
				}
			}

			Out.EnableOutput = true;
		}

		public virtual void InjurePartyAndDamageEquipment(IInjureAndDamageArgs injureAndDamageArgs, ref bool gotoCleanup)
		{
			Debug.Assert(injureAndDamageArgs != null && injureAndDamageArgs.Room != null && injureAndDamageArgs.EquipmentDamageAmount > 0 && injureAndDamageArgs.InjuryMultiplier > 0.0 && injureAndDamageArgs.SetNextStateFunc != null);

			if (injureAndDamageArgs.EffectUid > 0)
			{
				PrintEffectDesc(injureAndDamageArgs.EffectUid);
			}

			var monsterList = GetMonsterList(m => m.IsCharacterMonster(), m => !m.IsCharacterMonster() && m.Reaction == Friendliness.Friend && m.IsInRoom(injureAndDamageArgs.Room));

			foreach (var monster in monsterList)
			{
				var containedList = monster.GetContainedList();

				var dice = (long)Math.Floor(injureAndDamageArgs.InjuryMultiplier * (monster.Hardiness - monster.DmgTaken) + 1);

				var combatComponent = CreateInstance<ICombatComponent>(x =>
				{
					x.SetNextStateFunc = injureAndDamageArgs.SetNextStateFunc;

					x.ActorRoom = injureAndDamageArgs.Room;

					x.Dobj = monster;

					x.OmitArmor = true;
				});

				combatComponent.ExecuteCalculateDamage(dice, 1);

				if (GameState.Die > 0)
				{
					gotoCleanup = true;

					goto Cleanup;
				}

				if (monster.IsInLimbo())
				{
					foreach (var artifact in containedList)
					{
						artifact.SetCarriedByMonster(monster);
					}
				}

				DamageWeaponsAndArmor(injureAndDamageArgs.Room, monster, injureAndDamageArgs.EquipmentDamageAmount);

				if (monster.IsInLimbo())
				{
					var deadBodyArtifact = monster.DeadBody > 0 ? ADB[monster.DeadBody] : null;

					if (deadBodyArtifact != null && !deadBodyArtifact.IsInLimbo())
					{
						deadBodyArtifact.SetInRoomUid(injureAndDamageArgs.DeadBodyRoomUid);
					}

					foreach (var artifact in containedList)
					{
						if (!artifact.IsInLimbo())
						{
							artifact.SetInRoomUid(injureAndDamageArgs.DeadBodyRoomUid);
						}
					}
				}
			}

		Cleanup:

			;
		}

		public virtual void CheckActionList(IList<Action> actionList)
		{
			Debug.Assert(actionList != null);

			if (GameState.Die <= 0)
			{
				for (var i = 0; i < actionList.Count; i++)
				{
					actionList[i]();
				}
			}

			actionList.Clear();
		}

		public virtual void CheckPlayerSkillGains()
		{
			CheckActionList(SkillIncreaseFuncList);

			if (PauseCombatAfterSkillGains)
			{
				PauseCombat();
			}

			PauseCombatAfterSkillGains = false;
		}

		public virtual void CheckRevealContainerContents()
		{
			if (GameState.Die <= 0)
			{
				for (var i = 0; i < RevealContentFuncList.Count; i++)
				{
					RevealContentFuncList[i]();
				}
			}

			ResetProperties(PropertyResetCode.RevealContainerContents);
		}

		public virtual void CheckToProcessActionLists()
		{
			CheckActionList(MiscEventFuncList);

			CheckPlayerSkillGains();

			CheckActionList(MiscEventFuncList02);

			CheckRevealContainerContents();

			CheckActionList(MiscEventFuncList03);
		}

		public virtual void CheckToExtinguishLightSource()
		{
			Debug.Assert(GameState.Ls > 0);

			var artifact = ADB[GameState.Ls];

			Debug.Assert(artifact != null);

			var ac = artifact.LightSource;

			Debug.Assert(ac != null);

			if (ac.Field1 != -1)
			{
				PrintEnterExtinguishLightChoice(artifact);

				gEngine.Buf.Clear();

				var rc = In.ReadField(gEngine.Buf, BufSize02, null, ' ', '\0', false, null, ModifyCharToUpper, IsCharYOrN, null);

				Debug.Assert(IsSuccess(rc));

				if (gEngine.Buf.Length > 0 && gEngine.Buf[0] == 'Y')
				{
					rc = artifact.RemoveStateDesc(artifact.GetProvidingLightDesc());

					Debug.Assert(IsSuccess(rc));

					GameState.Ls = 0;
				}
			}
		}

		public virtual void PauseCombat()
		{
			Thread.Sleep(GameState.PauseCombatMs);

			PauseCombatActionsCounter++;

			if (GameState.Die <= 0 && GameState.PauseCombatActions > 0 && PauseCombatActionsCounter % GameState.PauseCombatActions == 0)
			{
				In.KeyPress(gEngine.Buf);

				Out.Print("{0}", LineSep);
			}
		}

		public virtual void TransportRoomContentsBetweenRooms(IRoom oldRoom, IRoom newRoom, bool includeEmbedded = true)
		{
			Debug.Assert(oldRoom != null);

			Debug.Assert(newRoom != null);

			var monsterList = GetMonsterList(m => m.IsInRoom(oldRoom)).ToList();

			foreach (var m in monsterList)
			{
				m.SetInRoom(newRoom);
			}

			var artifactList = GetArtifactList(a => a.IsInRoom(oldRoom)).ToList();

			foreach (var a in artifactList)
			{
				a.SetInRoom(newRoom);
			}

			if (includeEmbedded)
			{
				artifactList = GetArtifactList(a => a.IsEmbeddedInRoom(oldRoom)).ToList();

				foreach (var a in artifactList)
				{
					a.SetEmbeddedInRoom(newRoom);
				}
			}
		}

		// Note: this method should only be used when oldRoom and newRoom are logically equivalent

		public virtual void TransportPlayerBetweenRooms(IRoom oldRoom, IRoom newRoom, IEffect effect)
		{
			Debug.Assert(oldRoom != null);

			Debug.Assert(newRoom != null);

			var gameState = GetGameState();

			Debug.Assert(gameState != null);

			TransportRoomContentsBetweenRooms(oldRoom, newRoom);

			if (effect != null)
			{
				PrintEffectDesc(effect);
			}

			gameState.Ro = newRoom.Uid;

			gameState.R2 = gameState.Ro;
		}

		public virtual void CreateArtifactSynonyms(long artifactUid, params string[] synonyms)
		{
			Debug.Assert(synonyms != null && synonyms.Length > 0);

			var artifact = ADB[artifactUid];

			Debug.Assert(artifact != null);

			artifact.Synonyms = CloneInstance(synonyms);
		}

		public virtual void CreateMonsterSynonyms(long monsterUid, params string[] synonyms)
		{
			Debug.Assert(synonyms != null && synonyms.Length > 0);

			var monster = MDB[monsterUid];

			Debug.Assert(monster != null);

			monster.Synonyms = CloneInstance(synonyms);
		}

		public virtual void GetOddsToHit(IMonster actorMonster, IMonster dobjMonster, IArtifactCategory ac, long af, ref long oddsToHit)
		{
			Debug.Assert(actorMonster != null);

			Debug.Assert(dobjMonster != null);

			Debug.Assert(ac == null || (ac != null && ac.IsWeapon01()));

			var x = actorMonster.Agility;

			var f = dobjMonster.Agility;

			var a = actorMonster.Armor;

			var d = dobjMonster.Armor;

			if (a > 8)
			{
				a = 8;
			}

			if (d > 8)
			{
				d = 8;
			}

			if (x > 30)
			{
				x = 30;
			}

			if (f > 30)
			{
				f = 30;
			}

			var odds = 50 + 2 * (x - f - a + d);

			if (ac != null)
			{
				d = ac.Field1;

				if (d > 50)
				{
					d = 50;
				}

				odds = (long)Math.Round((double)odds + ((double)d / 2.0));
			}

			if (actorMonster.IsCharacterMonster())
			{
				odds += ((af + Character.ArmorExpertise) * (-af > Character.ArmorExpertise ? 1 : 0));

				d = ac != null ? Character.GetWeaponAbility(ac.Field2) : 0;

				if (d > 122)
				{
					d = 122;
				}

				odds = (long)Math.Round((double)odds + ((double)d / 4.0));
			}

			if (GameState != null && GameState.EnhancedCombat)
			{
				/*
					This formula results in a smooth progression from ParrySetting 0 to 100.

					Original formula:  
					
						ParryMod = ModMax - (ParrySetting / 100) * (ModMax - ModMin)

					This formula: 
				
						ParryMod = 1.60 - (ParrySetting / 100) * (1.60 - 0.40)
						ParryMod = 1.60 - (ParrySetting / 100) * 1.20
						ParryMod = 1.60 - (ParrySetting * 0.012)

					ParrySetting =   0 => ParryMod = 1.6 (max offense)
					ParrySetting =  50 => ParryMod = 1.0 (neutral)
					ParrySetting = 100 => ParryMod = 0.4 (max defense)
				*/

				var actorParryMod = 1.60 - ((double)actorMonster.Parry * 0.012);

				var dobjParryMod = 1.60 - ((double)dobjMonster.Parry * 0.012);

				odds = (long)Math.Round((double)odds * actorParryMod * dobjParryMod);
			}

			oddsToHit = Math.Max(5, odds);
		}

		public virtual void CreateInitialState(bool printLineSep)
		{
			if (GameState.Die != 1)
			{
				CurrState = CreateInstance<IAfterPlayerMoveState>();
			}
			else
			{
				CurrState = CreateInstance<IPlayerDeadState>(x =>
				{
					x.PrintLineSep = printLineSep;
				});
			}
		}

		public virtual void MoveMonsters(params Func<IMonster, bool>[] whereClauseFuncs)
		{
			if (whereClauseFuncs == null || whereClauseFuncs.Length == 0)
			{
				whereClauseFuncs = new Func<IMonster, bool>[]
				{
					m => !m.IsCharacterMonster() && m.Seen && m.Location == GameState.R3
				};
			}

			var monsterList = GetMonsterList(whereClauseFuncs);

			foreach (var monster in monsterList)
			{
				if (monster.CanMoveToRoomUid(GameState.Ro, false) && (monster.Reaction == Friendliness.Friend || (monster.Reaction == Friendliness.Enemy && monster.CheckCourage())))
				{
					monster.Location = GameState.Ro;
				}
			}
		}

		public virtual void RtProcessArgv(bool secondPass, ref bool nlFlag)
		{
			long i;

			for (i = 0; i < Argv.Length; i++)
			{
				if (Argv[i].Equals("--workingDirectory", StringComparison.OrdinalIgnoreCase) || Argv[i].Equals("-wd", StringComparison.OrdinalIgnoreCase))
				{
					if (++i < Argv.Length)
					{
						// Do nothing
					}
				}
				else if (Argv[i].Equals("--filePrefix", StringComparison.OrdinalIgnoreCase) || Argv[i].Equals("-fp", StringComparison.OrdinalIgnoreCase))
				{
					if (++i < Argv.Length)
					{
						// Do nothing
					}
				}
				else if (Argv[i].Equals("--configFileName", StringComparison.OrdinalIgnoreCase) || Argv[i].Equals("-cfgfn", StringComparison.OrdinalIgnoreCase))
				{
					if (++i < Argv.Length && !secondPass)
					{
						ConfigFileName = Argv[i].Trim();
					}
				}
				else if (Argv[i].Equals("--filesetFileName", StringComparison.OrdinalIgnoreCase) || Argv[i].Equals("-fsfn", StringComparison.OrdinalIgnoreCase))
				{
					if (++i < Argv.Length && secondPass)
					{
						Config.RtFilesetFileName = Argv[i].Trim();

						ConfigsModified = true;
					}
				}
				else if (Argv[i].Equals("--characterFileName", StringComparison.OrdinalIgnoreCase) || Argv[i].Equals("-chrfn", StringComparison.OrdinalIgnoreCase))
				{
					if (++i < Argv.Length && secondPass)
					{
						Config.RtCharacterFileName = Argv[i].Trim();

						ConfigsModified = true;
					}
				}
				else if (Argv[i].Equals("--moduleFileName", StringComparison.OrdinalIgnoreCase) || Argv[i].Equals("-modfn", StringComparison.OrdinalIgnoreCase))
				{
					if (++i < Argv.Length && secondPass)
					{
						Config.RtModuleFileName = Argv[i].Trim();

						ConfigsModified = true;
					}
				}
				else if (Argv[i].Equals("--roomFileName", StringComparison.OrdinalIgnoreCase) || Argv[i].Equals("-rfn", StringComparison.OrdinalIgnoreCase))
				{
					if (++i < Argv.Length && secondPass)
					{
						Config.RtRoomFileName = Argv[i].Trim();

						ConfigsModified = true;
					}
				}
				else if (Argv[i].Equals("--artifactFileName", StringComparison.OrdinalIgnoreCase) || Argv[i].Equals("-afn", StringComparison.OrdinalIgnoreCase))
				{
					if (++i < Argv.Length && secondPass)
					{
						Config.RtArtifactFileName = Argv[i].Trim();

						ConfigsModified = true;
					}
				}
				else if (Argv[i].Equals("--effectFileName", StringComparison.OrdinalIgnoreCase) || Argv[i].Equals("-efn", StringComparison.OrdinalIgnoreCase))
				{
					if (++i < Argv.Length && secondPass)
					{
						Config.RtEffectFileName = Argv[i].Trim();

						ConfigsModified = true;
					}
				}
				else if (Argv[i].Equals("--monsterFileName", StringComparison.OrdinalIgnoreCase) || Argv[i].Equals("-monfn", StringComparison.OrdinalIgnoreCase))
				{
					if (++i < Argv.Length && secondPass)
					{
						Config.RtMonsterFileName = Argv[i].Trim();

						ConfigsModified = true;
					}
				}
				else if (Argv[i].Equals("--hintFileName", StringComparison.OrdinalIgnoreCase) || Argv[i].Equals("-hfn", StringComparison.OrdinalIgnoreCase))
				{
					if (++i < Argv.Length && secondPass)
					{
						Config.RtHintFileName = Argv[i].Trim();

						ConfigsModified = true;
					}
				}
				else if (Argv[i].Equals("--gameStateFileName", StringComparison.OrdinalIgnoreCase) || Argv[i].Equals("-gsfn", StringComparison.OrdinalIgnoreCase))
				{
					if (++i < Argv.Length && secondPass)
					{
						Config.RtGameStateFileName = Argv[i].Trim();

						ConfigsModified = true;
					}
				}
				else if (Argv[i].Equals("--enableScreenReaderMode", StringComparison.OrdinalIgnoreCase) || Argv[i].Equals("-esrm", StringComparison.OrdinalIgnoreCase))
				{
					// Do nothing
				}
				else if (Argv[i].Equals("--ignoreMutex", StringComparison.OrdinalIgnoreCase) || Argv[i].Equals("-im", StringComparison.OrdinalIgnoreCase))
				{
					// Do nothing
				}
				else if (Argv[i].Equals("--disableValidation", StringComparison.OrdinalIgnoreCase) || Argv[i].Equals("-dv", StringComparison.OrdinalIgnoreCase))
				{
					// Do nothing
				}
				else if (Argv[i].Equals("--deleteGameState", StringComparison.OrdinalIgnoreCase) || Argv[i].Equals("-dgs", StringComparison.OrdinalIgnoreCase))
				{
					if (secondPass)
					{
						DeleteGameStateFromMainHall = true;
					}
				}
				else if (secondPass)
				{
					if (!nlFlag)
					{
						Out.Print("{0}", LineSep);
					}

					Out.Write("{0}Unrecognized command line argument: [{1}]", Environment.NewLine, Argv[i]);

					nlFlag = true;
				}
			}
		}

		/// <summary></summary>
		public virtual void ConvertCharArtifactsToArtifacts(IMonster monster)
		{
			RetCode rc;

			Debug.Assert(monster != null);

			var artifactList = Character.GetContainedList().OrderBy(a => a.Uid).ToList();

			var weaponList = artifactList.Where(a => a.GeneralWeapon != null).OrderBy(a => a.Uid).ToList();

			if (IntroStory.StoryType == IntroStoryType.Beginners && weaponList.Count > 1 && GameState.UsedWpnIdx < 0)
			{
				GameState.UsedWpnIdx = 0;
			}

			for (var i = 0; i < artifactList.Count; i++)
			{
				var artifact = artifactList[i];

				Debug.Assert(artifact != null);

				var ac = artifact.GetCategory(0);

				Debug.Assert(ac != null);

				var artifact01 = CreateInstance<IArtifact>();			// Create game Artifact using dependency injection

				Debug.Assert(artifact01 != null);

				artifact01.CopyPropertiesFrom(artifact, recurse: true);

				if (artifact.GeneralWeapon != null)
				{
					artifact01.Name = artifact01.Name.Trim().TrimEnd('#');
				}

				Debug.Assert(!string.IsNullOrWhiteSpace(artifact01.Name));

				var ac01 = CreateInstance<IArtifactCategory>();         // Create game ArtifactCategory using dependency injection

				Debug.Assert(ac01 != null);

				ac01.CopyPropertiesFrom(ac, recurse: true);

				artifact01.SetArtifactCategoryCount(1);

				artifact01.SetCategory(0, ac01);

				artifact01.Uid = Database.GetArtifactUid();

				artifact01.SetParentReferences();

				if (artifact.IsWornByCharacter())
				{
					artifact01.SetWornByMonster(monster);
				}
				else if (artifact.IsCarriedByCharacter())
				{
					if (artifact.GeneralWeapon != null && GameState.UsedWpnIdx >= 0 && weaponList[(int)GameState.UsedWpnIdx] != artifact)
					{
						artifact01.SetInLimbo();

						GameState.SetHeldWpnUid(artifact01.Uid);
					}
					else
					{
						artifact01.SetCarriedByMonster(monster);
					}
				}
				else
				{
					Debug.Assert(1 == 0);
				}

				rc = Database.AddArtifact(artifact01);

				Debug.Assert(IsSuccess(rc));

				GameState.SetImportedArtUid(artifact01.Uid);
			}

			var armorArtifact = monster.GetWornList().FirstOrDefault(a => a.Wearable.Field1 >= 2);

			if (armorArtifact != null)
			{
				GameState.Ar = armorArtifact.Uid;

				Debug.Assert(GameState.Ar > 0);

				monster.Armor = armorArtifact.Wearable.Field1 / 2;

				if (monster.Armor >= 3)
				{
					monster.Armor += 2;
				}
			}

			var shieldArtifact = monster.GetWornList().FirstOrDefault(a => a.Wearable.Field1 == 1);

			if (shieldArtifact != null)
			{
				GameState.Sh = shieldArtifact.Uid;

				Debug.Assert(GameState.Sh > 0);

				monster.Armor += shieldArtifact.Wearable.Field1;
			}

			var weaponArtifact = monster.GetCarriedList().Where(a => a.GeneralWeapon != null && (a.GeneralWeapon.Field5 == 1 || shieldArtifact == null)).OrderByDescending(a => a.Field3 * a.Field4).FirstOrDefault();

			if (weaponArtifact != null)
			{
				monster.Weapon = weaponArtifact.Uid;

				Debug.Assert(monster.Weapon > 0);
			}
		}

		/// <summary>Encode an 6-digit number where the high 3 digits are the maxValue and the low 3 digits are the minValue</summary>
		/// <param name="minValue"></param>
		/// <param name="maxValue"></param>
		/// <returns></returns>
		public virtual long ScaledValueMinMaxEncode(long minValue, long maxValue)
		{
			return maxValue * 1000 + minValue;
		}

		/// <summary>Decode an 6-digit number where the high 3 digits are the maxValue and the low 3 digits are the minValue</summary>
		/// <param name="encodedValue"></param>
		/// <param name="minValue"></param>
		/// <param name="maxValue"></param>
		public virtual void ScaledValueMinMaxDecode(long encodedValue, out long minValue, out long maxValue)
		{
			minValue = encodedValue % 1000;

			maxValue = encodedValue / 1000;
		}

		/// <summary></summary>
		/// <param name="monster"></param>
		/// <param name="damageFactor"></param>
		public virtual void SetScaledHardiness(IMonster monster, long damageFactor)
		{
			Debug.Assert(monster != null);

			Debug.Assert(damageFactor > 0);

			if (monster.Field1 > 0)
			{
				monster.Hardiness = damageFactor * monster.Field1;

				if (monster.Field2 > 0)
				{
					long minValue = 0;
					
					long maxValue = 0;
					
					ScaledValueMinMaxDecode(monster.Field2, out minValue, out maxValue);

					if (minValue > 0 && monster.Hardiness < minValue)
					{
						monster.Hardiness = minValue;
					}
					
					if (maxValue > 0 && monster.Hardiness > maxValue)
					{
						monster.Hardiness = maxValue;
					}
				}
			}
		}

		#endregion

		public Engine()
		{
			((IEngine)this).Buf = new StringBuilder(BufSize);

			((IEngine)this).Buf01 = new StringBuilder(BufSize);

			/*
			IgnoredTokenHashSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
			{
				"a", "an", "the", "of", "in", "on", "at", "to", "with", "by", "for", "from",
				"and", "or", "but", "is", "are", "was", "were", "be", "being", "been",
				"this", "that", "these", "those", "it", "its", "they", "their", "there",
				"here", "where", "when", "what", "which", "who", "whom", "whose"
			};
			*/

			RevealContainerContentsFunc = RevealContainerContents;

			StartRoom = 1;

			NumSaveSlots = 5;

			ScaledHardinessUnarmedMaxDamage = 20;

			ScaledHardinessMaxDamageDivisor = 2.0;

			EnforceMonsterWeightLimits = true;

			PoundCharPolicy = PoundCharPolicy.AllArtifacts;

			PercentCharPolicy = PercentCharPolicy.None;
		}
	}
}
