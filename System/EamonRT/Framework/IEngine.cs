
// IEngine.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Text;
using Eamon;
using Eamon.Framework;
using Eamon.Framework.Primitive.Classes;
using Eamon.Framework.Primitive.Enums;
using EamonRT.Framework.Commands;

namespace EamonRT.Framework
{
	/// <summary></summary>
	public interface IEngine : EamonDD.Framework.IEngine
	{
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

		/// <summary></summary>
		bool EnforceMonsterWeightLimits { get; set; }

		/// <summary></summary>
		bool UseMonsterScaledHardinessValues { get; set; }

		/// <summary></summary>
		bool AutoDisplayUnseenArtifactDescs { get; set; }

		/// <summary></summary>
		bool ExposeContainersRecursively { get; set; }

		/// <summary></summary>
		PoundCharPolicy PoundCharPolicy { get; set; }

		/// <summary></summary>
		PercentCharPolicy PercentCharPolicy { get; set; }

		/// <summary></summary>
		void PrintPlayerRoom();

		/// <summary></summary>
		/// <param name="artifact"></param>
		void PrintMonsterAlive(IArtifact artifact);

		/// <summary></summary>
		/// <param name="artifact"></param>
		void PrintLightOut(IArtifact artifact);

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
		void AddUniqueCharsToArtifactAndMonsterNames();

		/// <summary></summary>
		void AddMissingDescs();

		/// <summary></summary>
		void InitSaArray();

		/// <summary></summary>
		void CreateCommands();

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
		/// <param name="monster"></param>
		/// <param name="printLineSep"></param>
		/// <param name="restoreGame"></param>
		void DeadMenu(IMonster monster, bool printLineSep, ref bool restoreGame);

		/// <summary></summary>
		/// <param name="artifact"></param>
		void LightOut(IArtifact artifact);

		/// <summary></summary>
		/// <param name="monster"></param>
		/// <param name="printFinalNewLine"></param>
		void MonsterGetsAggravated(IMonster monster, bool printFinalNewLine = true);

		/// <summary></summary>
		/// <param name="monster"></param>
		/// <param name="friendSmile"></param>
		void MonsterEmotes(IMonster monster, bool friendSmile = true);

		/// <summary></summary>
		/// <param name="OfMonster"></param>
		/// <param name="DfMonster"></param>
		void MonsterDies(IMonster OfMonster, IMonster DfMonster);

		/// <summary></summary>
		/// <param name="monster"></param>
		void ProcessMonsterDeathEvents(IMonster monster);

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
		/// <param name="revealContentListIndex"></param>
		/// <param name="containerTypes"></param>
		/// <param name="containerContentsList"></param>
		void RevealContainerContents(IRoom room, long revealContentListIndex, ContainerType[] containerTypes, IList<string> containerContentsList = null);

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
		/// <param name="callSleep"></param>
		/// <param name="printOutput"></param>
		void MoveMonsterToRandomAdjacentRoom(IRoom room, IMonster monster, bool fleeing, bool callSleep, bool printOutput = true);

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
		/// <param name="monster"></param>
		/// <returns></returns>
		IList<IMonster> GetHostileMonsterList(IMonster monster);

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
		/// <param name="spellValue"></param>
		/// <param name="shouldAllowSkillGains"></param>
		/// <returns></returns>
		bool CheckPlayerSpellCast(Spell spellValue, bool shouldAllowSkillGains);

		/// <summary></summary>
		/// <param name="stat"></param>
		/// <param name="bonus"></param>
		/// <returns></returns>
		bool SaveThrow(Stat stat, long bonus = 0);

		/// <summary></summary>
		/// <param name="ac"></param>
		/// <param name="af"></param>
		void CheckPlayerSkillGains(IArtifactCategory ac, long af);

		/// <summary></summary>
		void CheckToExtinguishLightSource();

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
		/// <param name="str"></param>
		/// <param name="buf"></param>
		void PrintMacroReplacedPagedString(string str, StringBuilder buf);

		/// <summary></summary>
		/// <param name="artifactUid"></param>
		/// <param name="synonyms"></param>
		void CreateArtifactSynonyms(long artifactUid, params string[] synonyms);

		/// <summary></summary>
		/// <param name="monsterUid"></param>
		/// <param name="synonyms"></param>
		void CreateMonsterSynonyms(long monsterUid, params string[] synonyms);

		/// <summary></summary>
		/// <param name="ofMonster"></param>
		/// <param name="dfMonster"></param>
		/// <param name="ac"></param>
		/// <param name="af"></param>
		/// <param name="oddsToHit"></param>
		void GetOddsToHit(IMonster ofMonster, IMonster dfMonster, IArtifactCategory ac, long af, ref long oddsToHit);

		/// <summary></summary>
		/// <param name="printLineSep"></param>
		void CreateInitialState(bool printLineSep);

		/// <summary></summary>
		void MoveMonsters(params Func<IMonster, bool>[] whereClauseFuncs);

		/// <summary></summary>
		/// <param name="secondPass"></param>
		/// <param name="nlFlag"></param>
		void RtProcessArgv(bool secondPass, ref bool nlFlag);
	};
}
