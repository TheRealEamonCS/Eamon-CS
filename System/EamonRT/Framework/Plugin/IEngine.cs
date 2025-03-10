﻿
// IEngine.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Text;
using Eamon;
using Eamon.Framework;
using Eamon.Framework.Args;
using Eamon.Framework.Primitive.Classes;
using Eamon.Framework.Primitive.Enums;
using EamonRT.Framework.Args;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Parsing;
using EamonRT.Framework.States;

namespace EamonRT.Framework.Plugin
{
	/// <inheritdoc />
	public interface IEngine : EamonDD.Framework.Plugin.IEngine
	{
		#region Explicit Properties

		/// <summary></summary>
		new StringBuilder Buf { get; set; }

		/// <summary></summary>
		StringBuilder Buf01 { get; set; }

		#endregion

		#region Public Properties

		/// <summary>
		/// Gets the page separator sequence used to delineate the pages of a multi-page text block.
		/// </summary>
		/// <remarks>
		/// These are typically embedded in the prologue and epilogue stories of games. After each page is read,
		/// the next is accessed when the user presses Enter.
		/// </remarks>
		string PageSep { get; }

		/// <summary></summary>
		IList<ICommand> CommandList { get; set; }

		/// <summary></summary>
		IList<ICommand> LastCommandList { get; set; }

		/// <summary></summary>
		IList<Action> MiscEventFuncList { get; set; }

		/// <summary></summary>
		IList<Action> MiscEventFuncList02 { get; set; }

		/// <summary></summary>
		IList<Action> MiscEventFuncList03 { get; set; }

		/// <summary></summary>
		IList<Action> SkillIncreaseFuncList { get; set; }

		/// <summary></summary>
		IList<long> LoopMonsterUidList { get; set; }

		/// <summary></summary>
		long ActionListCounter { get; set; }

		/// <summary>
		/// Gets or sets a value that tracks the number of <see cref="IMonster">Monster</see> actions performed during a
		/// combat round.
		/// </summary>
		/// <remarks>
		/// This counter, associated with the PauseCombatActions EamonRT setting, is incremented for each <see cref="IMonster">Monster</see>
		/// combat round action. When it reaches <see cref="IGameState.PauseCombatActions">PauseCombatActions</see>, the player must press
		/// Enter to continue the combat round.
		/// </remarks>
		long PauseCombatActionsCounter { get; set; }

		/// <summary></summary>
		long LoopMonsterUidListIndex { get; set; }

		/// <summary></summary>
		long LoopMonsterUid { get; set; }

		/// <summary></summary>
		long LoopMemberNumber { get; set; }

		/// <summary></summary>
		long LoopAttackNumber { get; set; }

		/// <summary></summary>
		long LoopGroupCount { get; set; }

		/// <summary></summary>
		long LoopFailedMoveMemberCount { get; set; }

		/// <summary></summary>
		IMonster LoopLastDobjMonster { get; set; }

		/// <summary></summary>
		IIntroStory IntroStory { get; set; }

		/// <summary></summary>
		IMainLoop MainLoop { get; set; }

		/// <summary></summary>
		ISentenceParser SentenceParser { get; set; }

		/// <summary></summary>
		ICommandParser CommandParser { get; set; }

		/// <summary></summary>
		IState InitialState { get; set; }

		/// <summary></summary>
		IState CurrState { get; set; }

		/// <summary></summary>
		IState NextState { get; set; }

		/// <summary></summary>
		IGameState GameState { get; set; }

		/// <summary></summary>
		ICharacter Character { get; set; }

		/// <summary></summary>
		ExitType ExitType { get; set; }

		/// <summary>
		/// Gets the command prompt presented to the player when requesting input.
		/// </summary>
		string CommandPrompt { get; set; }

		/// <summary></summary>
		ICommand CurrCommand { get; }

		/// <summary></summary>
		ICommand NextCommand { get; }

		/// <summary></summary>
		ICommand LastCommand { get; }

		/// <summary></summary>
		bool CommandPromptSeen { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether "pre-turn" processing should be done at the beginning of a new round.
		/// </summary>
		/// <remarks>
		/// This processing typically occurs after the player executes a <see cref="ICommand">Command</see> that directly impacts the
		/// game world, but before new input is requested. Included tasks are burning down light sources and <see cref="Spell.Speed">Speed</see>
		/// spells, regenerating <see cref="Spell">Spell</see> abilities, and firing certain game-specific events. Generally speaking, "pre-turn"
		/// processing is suppressed for "out-of-band" Commands like <see cref="IStatusCommand">Status</see>, etc.
		/// </remarks>
		bool ShouldPreTurnProcess { get; set; }

		/// <summary></summary>
		bool PauseCombatAfterSkillGains { get; set; }

		/// <summary></summary>
		bool UseRevealContentMonsterTheName { get; set; }

		/// <summary></summary>
		bool PlayerMoved { get; set; }

		/// <summary>
		/// Indicates whether the EamonRT main game loop is currently running.
		/// </summary>
		/// <remarks>
		/// The <see cref="IMainLoop">MainLoop</see>'s <see cref="IMainLoop.Execute">Execute</see> method will continue to run while
		/// this property evaluates to true. The processing loop drives the game engine's central state machine. The property is
		/// sensitive to changes in <see cref="ExitType">ExitType</see>.
		/// </remarks>
		bool GameRunning { get; }

		/// <summary></summary>
		bool DeleteGameStateAfterLoop { get; }

		/// <summary></summary>
		bool StartOver { get; }

		/// <summary></summary>
		bool ErrorExit { get; }

		/// <summary></summary>
		bool ExportCharacterGoToMainHall { get; }

		/// <summary></summary>
		bool ExportCharacter { get; }

		/// <summary></summary>
		bool DeleteCharacter { get; }

		/// <summary>
		/// Gets or sets the <see cref="IRoom">Room</see><see cref="IGameBase.Uid"> Uid</see> of the player character's initial location at
		/// the beginning of the game.
		/// </summary>
		long StartRoom { get; set; }

		/// <summary>
		/// Gets or sets the number of Saved Game slots available for use in the game.
		/// </summary>
		long NumSaveSlots { get; set; }

		/// <summary></summary>
		long ScaledHardinessUnarmedMaxDamage { get; set; }

		/// <summary></summary>
		double ScaledHardinessMaxDamageDivisor { get; set; }

		/// <summary>
		/// Indicates whether non-player character <see cref="IMonster">Monster</see>s should have carried inventory weight limits.
		/// </summary>
		bool EnforceMonsterWeightLimits { get; set; }

		/// <summary></summary>
		bool UseMonsterScaledHardinessValues { get; set; }

		/// <summary></summary>
		bool AutoDisplayUnseenArtifactDescs { get; set; }

		/// <summary></summary>
		bool ExposeContainersRecursively { get; set; }

		/// <summary>
		/// The game engine policy for appending pound characters ('#') to <see cref="IArtifact">Artifact</see>s with duplicate names.
		/// </summary>
		/// <remarks>
		/// This helps the parser identify specific <see cref="IArtifact">Artifact</see>s during disambiguation.
		/// </remarks>
		PoundCharPolicy PoundCharPolicy { get; set; }

		/// <summary>
		/// The game engine policy for appending percent characters ('%') to <see cref="IMonster">Monster</see>s with duplicate names.
		/// </summary>
		/// <remarks>
		/// This helps the parser identify specific <see cref="IMonster">Monster</see>s during disambiguation.
		/// </remarks>
		PercentCharPolicy PercentCharPolicy { get; set; }

		#endregion

		#region Public Methods

		/// <summary></summary>
		/// <param name="room"></param>
		void PrintPlayerRoom(IRoom room);

		/// <summary></summary>
		void PrintToWhom();

		/// <summary></summary>
		void PrintFromWhom();

		/// <summary></summary>
		/// <param name="command"></param>
		void PrintVerbWhoOrWhat(ICommand command);

		/// <summary></summary>
		/// <param name="command"></param>
		void PrintVerbPrepWhoOrWhat(ICommand command);

		/// <summary></summary>
		/// <param name="command"></param>
		void PrintFromPrepWhat(ICommand command);

		/// <summary></summary>
		/// <param name="command"></param>
		/// <param name="artifact"></param>
		void PrintPutObjPrepWhat(ICommand command, IArtifact artifact);

		/// <summary></summary>
		/// <param name="artifact"></param>
		void PrintUseObjOnWhoOrWhat(IArtifact artifact);

		/// <summary></summary>
		/// <param name="artifact"></param>
		void PrintWhamHitObj(IArtifact artifact);

		/// <summary></summary>
		/// <param name="artifact"></param>
		void PrintMonsterAlive(IArtifact artifact);

		/// <summary></summary>
		/// <param name="artifact"></param>
		void PrintLightOut(IArtifact artifact);

		/// <summary></summary>
		/// <param name="artifact"></param>
		void PrintDeadBodyComesToLife(IArtifact artifact);

		/// <summary></summary>
		/// <param name="artifact"></param>
		void PrintArtifactVanishes(IArtifact artifact);

		/// <summary></summary>
		/// <param name="room"></param>
		/// <param name="monster"></param>
		/// <param name="artifact"></param>
		/// <param name="prependNewLine"></param>
		void PrintArtifactBreaks(IRoom room, IMonster monster, IArtifact artifact, bool prependNewLine = false);

		/// <summary></summary>
		/// <param name="artifact"></param>
		void PrintEnterExtinguishLightChoice(IArtifact artifact);

		/// <summary></summary>
		/// <param name="artifact"></param>
		/// <param name="goldAmount"></param>
		void PrintArtifactIsWorth(IArtifact artifact, long goldAmount);

		/// <summary></summary>
		void PrintEnemiesNearby();

		/// <summary></summary>
		void PrintNothingHappens();

		/// <summary></summary>
		/// <param name="artifact"></param>
		/// <param name="showName"></param>
		/// <param name="showVerboseName"></param>
		void PrintFullDesc(IArtifact artifact, bool showName, bool showVerboseName);

		/// <summary></summary>
		/// <param name="monster"></param>
		/// <param name="room"></param>
		/// <param name="monsterName"></param>
		/// <param name="isPlural"></param>
		/// <param name="fleeing"></param>
		void PrintMonsterCantFindExit(IMonster monster, IRoom room, string monsterName, bool isPlural, bool fleeing);

		/// <summary></summary>
		/// <param name="monster"></param>
		/// <param name="room"></param>
		/// <param name="monsterName"></param>
		/// <param name="isPlural"></param>
		/// <param name="fleeing"></param>
		void PrintMonsterMembersExitRoom(IMonster monster, IRoom room, string monsterName, bool isPlural, bool fleeing);

		/// <summary></summary>
		/// <param name="monster"></param>
		/// <param name="room"></param>
		/// <param name="monsterName"></param>
		/// <param name="isPlural"></param>
		/// <param name="fleeing"></param>
		/// <param name="direction"></param>
		void PrintMonsterExitsRoom(IMonster monster, IRoom room, string monsterName, bool isPlural, bool fleeing, Direction direction);

		/// <summary></summary>
		/// <param name="monster"></param>
		/// <param name="room"></param>
		/// <param name="monsterName"></param>
		/// <param name="isPlural"></param>
		/// <param name="fleeing"></param>
		/// <param name="direction"></param>
		void PrintMonsterEntersRoom(IMonster monster, IRoom room, string monsterName, bool isPlural, bool fleeing, Direction direction);

		/// <summary></summary>
		/// <param name="monster"></param>
		/// <param name="printFinalNewLine"></param>
		void PrintMonsterGetsAngry(IMonster monster, bool printFinalNewLine = true);

		/// <summary></summary>
		/// <param name="monster"></param>
		/// <param name="friendSmile"></param>
		void PrintMonsterEmotes(IMonster monster, bool friendSmile = true);

		/// <summary></summary>
		/// <param name="monster"></param>
		/// <param name="showName"></param>
		/// <param name="showVerboseName"></param>
		void PrintFullDesc(IMonster monster, bool showName, bool showVerboseName);

		/// <summary></summary>
		/// <param name="monster"></param>
		void PrintHealthImproves(IMonster monster);

		/// <summary></summary>
		/// <param name="monster"></param>
		/// <param name="includeUninjuredGroupMonsters"></param>
		void PrintHealthStatus(IMonster monster, bool includeUninjuredGroupMonsters);

		/// <summary></summary>
		/// <param name="monster"></param>
		void PrintDoesntHaveIt(IMonster monster);

		/// <summary></summary>
		void PrintTooManyWeapons();

		/// <summary></summary>
		void PrintDeliverGoods();

		/// <summary></summary>
		void PrintYourWeaponsAre();

		/// <summary></summary>
		void PrintEnterWeaponToSell();

		/// <summary></summary>
		void PrintAllWoundsHealed();

		/// <summary></summary>
		void PrintYouHavePerished();

		/// <summary></summary>
		void PrintRestoreSavedGame();

		/// <summary></summary>
		void PrintStartOver();

		/// <summary></summary>
		void PrintAcceptDeath();

		/// <summary></summary>
		void PrintEnterDeadMenuChoice();

		/// <summary></summary>
		void PrintReallyWantToStartOver();

		/// <summary></summary>
		void PrintReallyWantToAcceptDeath();

		/// <summary></summary>
		void PrintWakingUpMonsters();

		/// <summary></summary>
		void PrintBaseProgramVersion();

		/// <summary></summary>
		void PrintWelcomeToEamonCS();

		/// <summary></summary>
		void PrintWelcomeBack();

		/// <summary></summary>
		void PrintEnterSeeIntroStoryChoice();

		/// <summary></summary>
		void PrintEnterWeaponNumberChoice();

		/// <summary></summary>
		void PrintNoIntroStory();

		/// <summary></summary>
		void PrintSavedGamesDeleted();

		/// <summary></summary>
		void PrintRestartGameUsingResume();

		/// <summary></summary>
		void PrintMemorialService();

		/// <summary></summary>
		void PrintSavedGames();

		/// <summary></summary>
		/// <param name="saveSlot"></param>
		/// <param name="saveName"></param>
		/// <param name="printFinalNewLine"></param>
		void PrintSaveSlot(long saveSlot, string saveName, bool printFinalNewLine = false);

		/// <summary></summary>
		/// <param name="saveSlot"></param>
		/// <param name="saveName"></param>
		void PrintQuickSave(long saveSlot, string saveName);

		/// <summary></summary>
		/// <param name="saveSlot"></param>
		void PrintUsingSlotInstead(long saveSlot);

		/// <summary></summary>
		/// <param name="numMenuItems"></param>
		void PrintEnterSaveSlotChoice(long numMenuItems);

		/// <summary></summary>
		/// <param name="numMenuItems"></param>
		void PrintEnterRestoreSlotChoice(long numMenuItems);

		/// <summary></summary>
		/// <param name="himStr"></param>
		void PrintChangingHim(string himStr);

		/// <summary></summary>
		/// <param name="herStr"></param>
		void PrintChangingHer(string herStr);

		/// <summary></summary>
		/// <param name="itStr"></param>
		void PrintChangingIt(string itStr);

		/// <summary></summary>
		/// <param name="themStr"></param>
		void PrintChangingThem(string themStr);

		/// <summary></summary>
		/// <param name="inputStr"></param>
		void PrintDiscardMessage(string inputStr);

		/// <summary></summary>
		/// <param name="goodsExist"></param>
		/// <param name="goldAmount"></param>
		void PrintGoodsPayment(bool goodsExist, long goldAmount);

		/// <summary></summary>
		/// <param name="str"></param>
		/// <param name="buf"></param>
		void PrintMacroReplacedPagedString(string str, StringBuilder buf);

		/// <summary></summary>
		/// <param name="monster"></param>
		/// <param name="artifact"></param>
		/// <param name="revealContentsList"></param>
		/// <param name="containerType"></param>
		/// <param name="showCharOwned"></param>
		/// <param name="recordNameListArgs"></param>
		void BuildRevealContentsListDescString(IMonster monster, IArtifact artifact, IList<IArtifact> revealContentsList, ContainerType containerType, bool showCharOwned, IRecordNameListArgs recordNameListArgs = null);

		/// <summary></summary>
		/// <param name="artifact1"></param>
		/// <param name="artifact2"></param>
		/// <returns></returns>
		long WeaponPowerCompare(IArtifact artifact1, IArtifact artifact2);

		/// <summary></summary>
		/// <param name="artifactUid1"></param>
		/// <param name="artifactUid2"></param>
		/// <returns></returns>
		long WeaponPowerCompare(long artifactUid1, long artifactUid2);

		/// <summary></summary>
		/// <param name="artifactList"></param>
		/// <returns></returns>
		IArtifact GetMostPowerfulWeapon(IList<IArtifact> artifactList);

		/// <summary></summary>
		/// <param name="artifactList"></param>
		/// <returns></returns>
		long GetMostPowerfulWeaponUid(IList<IArtifact> artifactList);

		/// <summary></summary>
		void EnforceCharacterWeightLimits();

		/// <summary></summary>
		void EnforceCharacterWeightLimits02(IRoom room = null, bool printOutput = false);

		/// <summary></summary>
		void NormalizeArtifactValuesAndWeights();

		/// <summary></summary>
		void AddUniqueCharsToArtifactAndMonsterNames();

		/// <summary></summary>
		void AddMissingDescs();

		/// <summary></summary>
		void InitSaArray();

		/// <summary></summary>
		void CreateCommands();

		/// <summary></summary>
		void InitRooms();

		/// <summary></summary>
		void InitArtifacts();

		/// <summary></summary>
		void InitMonsters();

		/// <summary></summary>
		void InitMonsterScaledHardinessValues();

		/// <summary></summary>
		/// <param name="weapon"></param>
		/// <returns></returns>
		IArtifact ConvertWeaponToArtifact(ICharacterArtifact weapon);

		/// <summary></summary>
		/// <param name="artifact"></param>
		/// <returns></returns>
		ICharacterArtifact ConvertArtifactToWeapon(IArtifact artifact);

		/// <summary></summary>
		/// <param name="artifact"></param>
		/// <param name="initialize"></param>
		/// <param name="addToDatabase"></param>
		/// <returns></returns>
		IMonster ConvertArtifactToMonster(IArtifact artifact, Action<IMonster> initialize = null, bool addToDatabase = false);

		/// <summary></summary>
		/// <returns></returns>
		IMonster ConvertCharacterToMonster();

		/// <summary></summary>
		/// <param name="monster"></param>
		/// <param name="weaponList"></param>
		void ConvertMonsterToCharacter(IMonster monster, IList<IArtifact> weaponList);

		/// <summary></summary>
		/// <param name="monster"></param>
		void ResetMonsterStats(IMonster monster);

		/// <summary></summary>
		void SetArmorClass();

		/// <summary></summary>
		/// <param name="weaponList"></param>
		void ConvertToCarriedInventory(IList<IArtifact> weaponList);

		/// <summary></summary>
		/// <param name="weaponList"></param>
		void SellExcessWeapons(IList<IArtifact> weaponList);

		/// <summary></summary>
		/// <param name="sellInventory"></param>
		void SellInventoryToMerchant(bool sellInventory = true);

		/// <summary></summary>
		/// <param name="printLineSep"></param>
		/// <param name="restoreGame"></param>
		void DeadMenu(bool printLineSep, ref bool restoreGame);

		/// <summary></summary>
		/// <param name="artifact"></param>
		void LightOut(IArtifact artifact);

		/// <summary></summary>
		/// <param name="monster"></param>
		/// <param name="printFinalNewLine"></param>
		void MonsterGetsAggravated(IMonster monster, bool printFinalNewLine = true);

		/// <summary></summary>
		/// <param name="actorMonster"></param>
		/// <param name="dobjMonster"></param>
		void MonsterDies(IMonster actorMonster, IMonster dobjMonster);

		/// <summary></summary>
		/// <param name="monster"></param>
		void ProcessMonsterDeathEvents(IMonster monster);

		/// <summary></summary>
		/// <param name="monster"></param>
		/// <returns></returns>
		string GetMonsterWeaponName(IMonster monster);

		/// <summary></summary>
		/// <param name="room"></param>
		/// <param name="artifact"></param>
		void RevealDisguisedMonster(IRoom room, IArtifact artifact);

		/// <summary></summary>
		/// <param name="room"></param>
		/// <param name="artifact"></param>
		void RevealEmbeddedArtifact(IRoom room, IArtifact artifact);

		/// <summary></summary>
		/// <param name="room"></param>
		/// <param name="monster"></param>
		/// <param name="artifact"></param>
		/// <param name="location"></param>
		/// <param name="printOutput"></param>
		void RevealContainerContents(IRoom room, IMonster monster, IArtifact artifact, long location, bool printOutput);

		/// <summary></summary>
		/// <param name="room"></param>
		/// <param name="monster"></param>
		/// <param name="artifact"></param>
		/// <param name="location"></param>
		/// <param name="containerTypes"></param>
		/// <param name="containerContentsList"></param>
		void RevealContainerContents02(IRoom room, IMonster monster, IArtifact artifact, long location, ContainerType[] containerTypes, IList<string> containerContentsList = null);

		/// <summary></summary>
		/// <param name="ro"></param>
		/// <param name="r2"></param>
		/// <param name="dir"></param>
		/// <returns></returns>
		IArtifact GetBlockedDirectionArtifact(long ro, long r2, Direction dir);

		/// <summary></summary>
		/// <param name="monster"></param>
		/// <param name="token"></param>
		/// <param name="synonymMatch"></param>
		/// <param name="partialMatch"></param>
		/// <returns></returns>
		ICommand GetCommandUsingToken(IMonster monster, string token, bool synonymMatch = true, bool partialMatch = true);

		/// <summary></summary>
		/// <param name="room"></param>
		/// <param name="artifact"></param>
		/// <param name="found"></param>
		/// <param name="roomUid"></param>
		void CheckDoor(IRoom room, IArtifact artifact, ref bool found, ref long roomUid);

		/// <summary></summary>
		/// <param name="room"></param>
		/// <param name="monster"></param>
		/// <param name="fleeing"></param>
		/// <param name="numExits"></param>
		void CheckNumberOfExits(IRoom room, IMonster monster, bool fleeing, ref long numExits);

		/// <summary></summary>
		/// <param name="room"></param>
		/// <param name="monster"></param>
		/// <param name="fleeing"></param>
		/// <param name="direction"></param>
		/// <param name="found"></param>
		/// <param name="roomUid"></param>
		void GetRandomMoveDirection(IRoom room, IMonster monster, bool fleeing, ref Direction direction, ref bool found, ref long roomUid);

		/// <summary></summary>
		/// <param name="room"></param>
		/// <param name="monster"></param>
		/// <param name="fleeing"></param>
		/// <param name="direction"></param>
		void GetRandomMoveDirection(IRoom room, IMonster monster, bool fleeing, ref Direction direction);

		/// <summary></summary>
		/// <param name="room"></param>
		/// <param name="monster"></param>
		/// <param name="fleeing"></param>
		/// <param name="pauseCombat"></param>
		/// <param name="printOutput"></param>
		void MoveMonsterToRandomAdjacentRoom(IRoom room, IMonster monster, bool fleeing, bool pauseCombat, bool printOutput = true);

		/// <summary></summary>
		/// <param name="numMonsters"></param>
		/// <param name="whereClauseFuncs"></param>
		/// <returns></returns>
		IList<IMonster> GetRandomMonsterList(long numMonsters, params Func<IMonster, bool>[] whereClauseFuncs);

		/// <summary></summary>
		/// <param name="recordList"></param>
		/// <param name="name"></param>
		/// <returns></returns>
		IList<IGameBase> FilterRecordList(IList<IGameBase> recordList, string name);

		/// <summary></summary>
		/// <param name="monster"></param>
		/// <returns></returns>
		IList<IArtifact> GetReadyableWeaponList(IMonster monster);

		/// <summary></summary>
		/// <param name="room"></param>
		/// <param name="monster"></param>
		/// <param name="friendSmile"></param>
		/// <returns></returns>
		IList<IMonster> GetEmotingMonsterList(IRoom room, IMonster monster, bool friendSmile = true);

		/// <summary></summary>
		/// <param name="monster"></param>
		/// <returns></returns>
		IList<IArtifact> BuildLoopWeaponArtifactList(IMonster monster);

		/// <summary>
		/// Gets the original carried/worn inventory brought by the player into the game.
		/// </summary>
		IList<IArtifact> GetImportedPlayerInventory();

		/// <summary>
		/// Hides the original carried/worn inventory brought by the player into the game by moving each <see cref="IArtifact">Artifact</see> into Limbo.
		/// </summary>
		/// <remarks>
		/// This method is intended to be used with <see cref="RestoreImportedPlayerInventory()">RestoreImportedPlayerInventory</see> to hide the player's inventory
		/// when entering the game and then restore it again prior to exiting the game.  It should only be used in <see cref="IMainLoop.Startup">MainLoop.Startup</see>.
		/// </remarks>
		void HideImportedPlayerInventory();

		/// <summary>
		/// Restores the original carried/worn inventory brought by the player into the game by moving each <see cref="IArtifact">Artifact</see> back to its initial
		/// location.
		/// </summary>
		/// <remarks>
		/// This method is intended to be used with <see cref="HideImportedPlayerInventory()">HideImportedPlayerInventory</see> to hide the player's inventory when
		/// entering the game and then restore it again prior to exiting the game.  It should only be used in <see cref="IMainLoop.Shutdown">MainLoop.Shutdown</see>.
		/// </remarks>
		void RestoreImportedPlayerInventory();

		/// <summary></summary>
		/// <param name="commands"></param>
		/// <param name="cmdType"></param>
		/// <param name="buf"></param>
		/// <param name="newSeen"></param>
		/// <returns></returns>
		RetCode BuildCommandList(IList<ICommand> commands, CommandType cmdType, StringBuilder buf, ref bool newSeen);

		/// <summary></summary>
		/// <param name="buf"></param>
		/// <returns></returns>
		StringBuilder NormalizePlayerInput(StringBuilder buf);

		/// <summary></summary>
		/// <param name="buf"></param>
		/// <returns></returns>
		StringBuilder ReplacePrepositions(StringBuilder buf);

		/// <summary></summary>
		/// <param name="oldRoomUid"></param>
		/// <param name="newRoomUid"></param>
		/// <returns></returns>
		bool IsValidRandomMoveDirection(long oldRoomUid, long newRoomUid);

		/// <summary></summary>
		/// <param name="command"></param>
		/// <returns></returns>
		bool IsQuotedStringCommand(ICommand command);

		/// <summary></summary>
		/// <param name="room"></param>
		/// <param name="whereClauseFuncs"></param>
		/// <returns></returns>
		bool ResurrectDeadBodies(IRoom room, params Func<IArtifact, bool>[] whereClauseFuncs);

		/// <summary></summary>
		/// <param name="room"></param>
		/// <param name="whereClauseFuncs"></param>
		/// <returns></returns>
		bool MakeArtifactsVanish(IRoom room, params Func<IArtifact, bool>[] whereClauseFuncs);

		/// <summary></summary>
		/// <param name="stat"></param>
		/// <param name="bonus"></param>
		/// <returns></returns>
		bool SaveThrow(Stat stat, long bonus = 0);

		/// <summary></summary>
		/// <param name="room"></param>
		/// <param name="monster"></param>
		/// <param name="damage"></param>
		/// <param name="recurse"></param>
		void DamageWeaponsAndArmor(IRoom room, IMonster monster, long damage = 1, bool recurse = false);

		/// <summary></summary>
		/// <param name="injureAndDamageArgs"></param>
		/// <param name="gotoCleanup"></param>
		void InjurePartyAndDamageEquipment(IInjureAndDamageArgs injureAndDamageArgs, ref bool gotoCleanup);

		/// <summary></summary>
		/// <param name="actionList"></param>
		void CheckActionList(IList<Action> actionList);

		/// <summary></summary>
		void CheckPlayerSkillGains();

		/// <summary></summary>
		void CheckRevealContainerContents();

		/// <summary></summary>
		void CheckToProcessActionLists();

		/// <summary></summary>
		void CheckToExtinguishLightSource();

		/// <summary></summary>
		void PauseCombat();

		/// <summary></summary>
		/// <param name="oldRoom"></param>
		/// <param name="newRoom"></param>
		/// <param name="includeEmbedded"></param>
		void TransportRoomContentsBetweenRooms(IRoom oldRoom, IRoom newRoom, bool includeEmbedded = true);

		/// <summary></summary>
		/// <param name="oldRoom"></param>
		/// <param name="newRoom"></param>
		/// <param name="effect"></param>
		void TransportPlayerBetweenRooms(IRoom oldRoom, IRoom newRoom, IEffect effect);

		/// <summary></summary>
		/// <param name="artifactUid"></param>
		/// <param name="synonyms"></param>
		void CreateArtifactSynonyms(long artifactUid, params string[] synonyms);

		/// <summary></summary>
		/// <param name="monsterUid"></param>
		/// <param name="synonyms"></param>
		void CreateMonsterSynonyms(long monsterUid, params string[] synonyms);

		/// <summary></summary>
		/// <param name="actorMonster"></param>
		/// <param name="dobjMonster"></param>
		/// <param name="ac"></param>
		/// <param name="af"></param>
		/// <param name="oddsToHit"></param>
		void GetOddsToHit(IMonster actorMonster, IMonster dobjMonster, IArtifactCategory ac, long af, ref long oddsToHit);

		/// <summary></summary>
		/// <param name="printLineSep"></param>
		void CreateInitialState(bool printLineSep);

		/// <summary></summary>
		void MoveMonsters(params Func<IMonster, bool>[] whereClauseFuncs);

		/// <summary></summary>
		/// <param name="secondPass"></param>
		/// <param name="nlFlag"></param>
		void RtProcessArgv(bool secondPass, ref bool nlFlag);

		#endregion
	}
}
