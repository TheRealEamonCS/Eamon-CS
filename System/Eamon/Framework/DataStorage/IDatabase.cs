
// IDatabase.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using Eamon.Framework.DataStorage.Generic;
using Eamon.Framework.Helpers.Generic;

namespace Eamon.Framework.DataStorage
{
	/// <summary></summary>
	public interface IDatabase
	{
		/// <summary></summary>
		IDbTable<IConfig> ConfigTable { get; set; }

		/// <summary></summary>
		IDbTable<IFileset> FilesetTable { get; set; }

		/// <summary></summary>
		IDbTable<ICharacter> CharacterTable { get; set; }

		/// <summary></summary>
		IDbTable<IModule> ModuleTable { get; set; }

		/// <summary></summary>
		IDbTable<IRoom> RoomTable { get; set; }

		/// <summary></summary>
		IDbTable<IArtifact> ArtifactTable { get; set; }

		/// <summary></summary>
		IDbTable<IEffect> EffectTable { get; set; }

		/// <summary></summary>
		IDbTable<IMonster> MonsterTable { get; set; }

		/// <summary></summary>
		IDbTable<IHint> HintTable { get; set; }

		/// <summary></summary>
		IDbTable<ITrigger> TriggerTable { get; set; }

		/// <summary></summary>
		IDbTable<IScript> ScriptTable { get; set; }

		/// <summary></summary>
		IDbTable<IGameState> GameStateTable { get; set; }

		/// <summary></summary>
		/// <param name="table"></param>
		/// <param name="fileName"></param>
		/// <param name="validate"></param>
		/// <param name="printOutput"></param>
		/// <returns></returns>
		RetCode LoadRecords<T, U>(ref IDbTable<T> table, string fileName, bool validate = true, bool printOutput = true) where T : class, IGameBase where U : class, IHelper<T>;

		/// <summary></summary>
		/// <param name="fileName"></param>
		/// <param name="validate"></param>
		/// <param name="printOutput"></param>
		/// <returns></returns>
		RetCode LoadConfigs(string fileName, bool validate = true, bool printOutput = true);

		/// <summary></summary>
		/// <param name="fileName"></param>
		/// <param name="validate"></param>
		/// <param name="printOutput"></param>
		/// <returns></returns>
		RetCode LoadFilesets(string fileName, bool validate = true, bool printOutput = true);

		/// <summary></summary>
		/// <param name="fileName"></param>
		/// <param name="validate"></param>
		/// <param name="printOutput"></param>
		/// <returns></returns>
		RetCode LoadCharacters(string fileName, bool validate = true, bool printOutput = true);

		/// <summary></summary>
		/// <param name="fileName"></param>
		/// <param name="validate"></param>
		/// <param name="printOutput"></param>
		/// <returns></returns>
		RetCode LoadModules(string fileName, bool validate = true, bool printOutput = true);

		/// <summary></summary>
		/// <param name="fileName"></param>
		/// <param name="validate"></param>
		/// <param name="printOutput"></param>
		/// <returns></returns>
		RetCode LoadRooms(string fileName, bool validate = true, bool printOutput = true);

		/// <summary></summary>
		/// <param name="fileName"></param>
		/// <param name="validate"></param>
		/// <param name="printOutput"></param>
		/// <returns></returns>
		RetCode LoadArtifacts(string fileName, bool validate = true, bool printOutput = true);

		/// <summary></summary>
		/// <param name="fileName"></param>
		/// <param name="validate"></param>
		/// <param name="printOutput"></param>
		/// <returns></returns>
		RetCode LoadEffects(string fileName, bool validate = true, bool printOutput = true);

		/// <summary></summary>
		/// <param name="fileName"></param>
		/// <param name="validate"></param>
		/// <param name="printOutput"></param>
		/// <returns></returns>
		RetCode LoadMonsters(string fileName, bool validate = true, bool printOutput = true);

		/// <summary></summary>
		/// <param name="fileName"></param>
		/// <param name="validate"></param>
		/// <param name="printOutput"></param>
		/// <returns></returns>
		RetCode LoadHints(string fileName, bool validate = true, bool printOutput = true);

		/// <summary></summary>
		/// <param name="fileName"></param>
		/// <param name="validate"></param>
		/// <param name="printOutput"></param>
		/// <returns></returns>
		RetCode LoadTriggers(string fileName, bool validate = true, bool printOutput = true);

		/// <summary></summary>
		/// <param name="fileName"></param>
		/// <param name="validate"></param>
		/// <param name="printOutput"></param>
		/// <returns></returns>
		RetCode LoadScripts(string fileName, bool validate = true, bool printOutput = true);

		/// <summary></summary>
		/// <param name="fileName"></param>
		/// <param name="validate"></param>
		/// <param name="printOutput"></param>
		/// <returns></returns>
		RetCode LoadGameStates(string fileName, bool validate = true, bool printOutput = true);

		/// <summary></summary>
		/// <param name="table"></param>
		/// <param name="fileName"></param>
		/// <param name="printOutput"></param>
		/// <returns></returns>
		RetCode SaveRecords<T>(IDbTable<T> table, string fileName, bool printOutput = true) where T : class, IGameBase;

		/// <summary></summary>
		/// <param name="fileName"></param>
		/// <param name="printOutput"></param>
		/// <returns></returns>
		RetCode SaveConfigs(string fileName, bool printOutput = true);

		/// <summary></summary>
		/// <param name="fileName"></param>
		/// <param name="printOutput"></param>
		/// <returns></returns>
		RetCode SaveFilesets(string fileName, bool printOutput = true);

		/// <summary></summary>
		/// <param name="fileName"></param>
		/// <param name="printOutput"></param>
		/// <returns></returns>
		RetCode SaveCharacters(string fileName, bool printOutput = true);

		/// <summary></summary>
		/// <param name="fileName"></param>
		/// <param name="printOutput"></param>
		/// <returns></returns>
		RetCode SaveModules(string fileName, bool printOutput = true);

		/// <summary></summary>
		/// <param name="fileName"></param>
		/// <param name="printOutput"></param>
		/// <returns></returns>
		RetCode SaveRooms(string fileName, bool printOutput = true);

		/// <summary></summary>
		/// <param name="fileName"></param>
		/// <param name="printOutput"></param>
		/// <returns></returns>
		RetCode SaveArtifacts(string fileName, bool printOutput = true);

		/// <summary></summary>
		/// <param name="fileName"></param>
		/// <param name="printOutput"></param>
		/// <returns></returns>
		RetCode SaveEffects(string fileName, bool printOutput = true);

		/// <summary></summary>
		/// <param name="fileName"></param>
		/// <param name="printOutput"></param>
		/// <returns></returns>
		RetCode SaveMonsters(string fileName, bool printOutput = true);

		/// <summary></summary>
		/// <param name="fileName"></param>
		/// <param name="printOutput"></param>
		/// <returns></returns>
		RetCode SaveHints(string fileName, bool printOutput = true);

		/// <summary></summary>
		/// <param name="fileName"></param>
		/// <param name="printOutput"></param>
		/// <returns></returns>
		RetCode SaveTriggers(string fileName, bool printOutput = true);

		/// <summary></summary>
		/// <param name="fileName"></param>
		/// <param name="printOutput"></param>
		/// <returns></returns>
		RetCode SaveScripts(string fileName, bool printOutput = true);

		/// <summary></summary>
		/// <param name="fileName"></param>
		/// <param name="printOutput"></param>
		/// <returns></returns>
		RetCode SaveGameStates(string fileName, bool printOutput = true);

		/// <summary></summary>
		/// <param name="table"></param>
		/// <param name="dispose"></param>
		/// <returns></returns>
		RetCode FreeRecords<T>(IDbTable<T> table, bool dispose = true) where T : class, IGameBase;

		/// <summary></summary>
		/// <param name="dispose"></param>
		/// <returns></returns>
		RetCode FreeConfigs(bool dispose = true);

		/// <summary></summary>
		/// <param name="dispose"></param>
		/// <returns></returns>
		RetCode FreeFilesets(bool dispose = true);

		/// <summary></summary>
		/// <param name="dispose"></param>
		/// <returns></returns>
		RetCode FreeCharacters(bool dispose = true);

		/// <summary></summary>
		/// <param name="dispose"></param>
		/// <returns></returns>
		RetCode FreeModules(bool dispose = true);

		/// <summary></summary>
		/// <param name="dispose"></param>
		/// <returns></returns>
		RetCode FreeRooms(bool dispose = true);

		/// <summary></summary>
		/// <param name="dispose"></param>
		/// <returns></returns>
		RetCode FreeArtifacts(bool dispose = true);

		/// <summary></summary>
		/// <param name="dispose"></param>
		/// <returns></returns>
		RetCode FreeEffects(bool dispose = true);

		/// <summary></summary>
		/// <param name="dispose"></param>
		/// <returns></returns>
		RetCode FreeMonsters(bool dispose = true);

		/// <summary></summary>
		/// <param name="dispose"></param>
		/// <returns></returns>
		RetCode FreeHints(bool dispose = true);

		/// <summary></summary>
		/// <param name="dispose"></param>
		/// <returns></returns>
		RetCode FreeTriggers(bool dispose = true);

		/// <summary></summary>
		/// <param name="dispose"></param>
		/// <returns></returns>
		RetCode FreeScripts(bool dispose = true);

		/// <summary></summary>
		/// <param name="dispose"></param>
		/// <returns></returns>
		RetCode FreeGameStates(bool dispose = true);

		/// <summary></summary>
		/// <param name="table"></param>
		/// <returns></returns>
		long GetRecordsCount<T>(IDbTable<T> table) where T : class, IGameBase;

		/// <summary></summary>
		/// <returns></returns>
		long GetConfigsCount();

		/// <summary></summary>
		/// <returns></returns>
		long GetFilesetsCount();

		/// <summary></summary>
		/// <returns></returns>
		long GetCharactersCount();

		/// <summary></summary>
		/// <returns></returns>
		long GetModulesCount();

		/// <summary></summary>
		/// <returns></returns>
		long GetRoomsCount();

		/// <summary></summary>
		/// <returns></returns>
		long GetArtifactsCount();

		/// <summary></summary>
		/// <returns></returns>
		long GetEffectsCount();

		/// <summary></summary>
		/// <returns></returns>
		long GetMonstersCount();

		/// <summary></summary>
		/// <returns></returns>
		long GetHintsCount();

		/// <summary></summary>
		/// <returns></returns>
		long GetTriggersCount();

		/// <summary></summary>
		/// <returns></returns>
		long GetScriptsCount();

		/// <summary></summary>
		/// <returns></returns>
		long GetGameStatesCount();

		/// <summary></summary>
		/// <param name="table"></param>
		/// <param name="uid"></param>
		/// <returns></returns>
		T FindRecord<T>(IDbTable<T> table, long uid) where T : class, IGameBase;

		/// <summary></summary>
		/// <param name="uid"></param>
		/// <returns></returns>
		IConfig FindConfig(long uid);

		/// <summary></summary>
		/// <param name="uid"></param>
		/// <returns></returns>
		IFileset FindFileset(long uid);

		/// <summary></summary>
		/// <param name="uid"></param>
		/// <returns></returns>
		ICharacter FindCharacter(long uid);

		/// <summary></summary>
		/// <param name="uid"></param>
		/// <returns></returns>
		IModule FindModule(long uid);

		/// <summary></summary>
		/// <param name="uid"></param>
		/// <returns></returns>
		IRoom FindRoom(long uid);

		/// <summary></summary>
		/// <param name="uid"></param>
		/// <returns></returns>
		IArtifact FindArtifact(long uid);

		/// <summary></summary>
		/// <param name="uid"></param>
		/// <returns></returns>
		IEffect FindEffect(long uid);

		/// <summary></summary>
		/// <param name="uid"></param>
		/// <returns></returns>
		IMonster FindMonster(long uid);

		/// <summary></summary>
		/// <param name="uid"></param>
		/// <returns></returns>
		IHint FindHint(long uid);

		/// <summary></summary>
		/// <param name="uid"></param>
		/// <returns></returns>
		ITrigger FindTrigger(long uid);

		/// <summary></summary>
		/// <param name="uid"></param>
		/// <returns></returns>
		IScript FindScript(long uid);

		/// <summary></summary>
		/// <param name="uid"></param>
		/// <returns></returns>
		IGameState FindGameState(long uid);

		/// <summary></summary>
		/// <param name="table"></param>
		/// <param name="type"></param>
		/// <param name="exactMatch"></param>
		/// <returns></returns>
		T FindRecord<T>(IDbTable<T> table, Type type, bool exactMatch = false) where T : class, IGameBase;

		/// <summary></summary>
		/// <param name="table"></param>
		/// <param name="record"></param>
		/// <param name="makeCopy"></param>
		/// <returns></returns>
		RetCode AddRecord<T>(IDbTable<T> table, T record, bool makeCopy = false) where T : class, IGameBase;

		/// <summary></summary>
		/// <param name="config"></param>
		/// <param name="makeCopy"></param>
		/// <returns></returns>
		RetCode AddConfig(IConfig config, bool makeCopy = false);

		/// <summary></summary>
		/// <param name="fileset"></param>
		/// <param name="makeCopy"></param>
		/// <returns></returns>
		RetCode AddFileset(IFileset fileset, bool makeCopy = false);

		/// <summary></summary>
		/// <param name="character"></param>
		/// <param name="makeCopy"></param>
		/// <returns></returns>
		RetCode AddCharacter(ICharacter character, bool makeCopy = false);

		/// <summary></summary>
		/// <param name="module"></param>
		/// <param name="makeCopy"></param>
		/// <returns></returns>
		RetCode AddModule(IModule module, bool makeCopy = false);

		/// <summary></summary>
		/// <param name="room"></param>
		/// <param name="makeCopy"></param>
		/// <returns></returns>
		RetCode AddRoom(IRoom room, bool makeCopy = false);

		/// <summary></summary>
		/// <param name="artifact"></param>
		/// <param name="makeCopy"></param>
		/// <returns></returns>
		RetCode AddArtifact(IArtifact artifact, bool makeCopy = false);

		/// <summary></summary>
		/// <param name="effect"></param>
		/// <param name="makeCopy"></param>
		/// <returns></returns>
		RetCode AddEffect(IEffect effect, bool makeCopy = false);

		/// <summary></summary>
		/// <param name="monster"></param>
		/// <param name="makeCopy"></param>
		/// <returns></returns>
		RetCode AddMonster(IMonster monster, bool makeCopy = false);

		/// <summary></summary>
		/// <param name="hint"></param>
		/// <param name="makeCopy"></param>
		/// <returns></returns>
		RetCode AddHint(IHint hint, bool makeCopy = false);

		/// <summary></summary>
		/// <param name="trigger"></param>
		/// <param name="makeCopy"></param>
		/// <returns></returns>
		RetCode AddTrigger(ITrigger trigger, bool makeCopy = false);

		/// <summary></summary>
		/// <param name="script"></param>
		/// <param name="makeCopy"></param>
		/// <returns></returns>
		RetCode AddScript(IScript script, bool makeCopy = false);

		/// <summary></summary>
		/// <param name="gameState"></param>
		/// <param name="makeCopy"></param>
		/// <returns></returns>
		RetCode AddGameState(IGameState gameState, bool makeCopy = false);

		/// <summary></summary>
		/// <param name="table"></param>
		/// <param name="uid"></param>
		/// <returns></returns>
		T RemoveRecord<T>(IDbTable<T> table, long uid) where T : class, IGameBase;

		/// <summary></summary>
		/// <param name="uid"></param>
		/// <returns></returns>
		IConfig RemoveConfig(long uid);

		/// <summary></summary>
		/// <param name="uid"></param>
		/// <returns></returns>
		IFileset RemoveFileset(long uid);

		/// <summary></summary>
		/// <param name="uid"></param>
		/// <returns></returns>
		ICharacter RemoveCharacter(long uid);

		/// <summary></summary>
		/// <param name="uid"></param>
		/// <returns></returns>
		IModule RemoveModule(long uid);

		/// <summary></summary>
		/// <param name="uid"></param>
		/// <returns></returns>
		IRoom RemoveRoom(long uid);

		/// <summary></summary>
		/// <param name="uid"></param>
		/// <returns></returns>
		IArtifact RemoveArtifact(long uid);

		/// <summary></summary>
		/// <param name="uid"></param>
		/// <returns></returns>
		IEffect RemoveEffect(long uid);

		/// <summary></summary>
		/// <param name="uid"></param>
		/// <returns></returns>
		IMonster RemoveMonster(long uid);

		/// <summary></summary>
		/// <param name="uid"></param>
		/// <returns></returns>
		IHint RemoveHint(long uid);

		/// <summary></summary>
		/// <param name="uid"></param>
		/// <returns></returns>
		ITrigger RemoveTrigger(long uid);

		/// <summary></summary>
		/// <param name="uid"></param>
		/// <returns></returns>
		IScript RemoveScript(long uid);

		/// <summary></summary>
		/// <param name="uid"></param>
		/// <returns></returns>
		IGameState RemoveGameState(long uid);

		/// <summary></summary>
		/// <param name="table"></param>
		/// <param name="type"></param>
		/// <param name="exactMatch"></param>
		/// <returns></returns>
		T RemoveRecord<T>(IDbTable<T> table, Type type, bool exactMatch = false) where T : class, IGameBase;

		/// <summary></summary>
		/// <param name="table"></param>
		/// <param name="allocate"></param>
		/// <returns></returns>
		long GetRecordUid<T>(IDbTable<T> table, bool allocate = true) where T : class, IGameBase;

		/// <summary></summary>
		/// <param name="allocate"></param>
		/// <returns></returns>
		long GetConfigUid(bool allocate = true);

		/// <summary></summary>
		/// <param name="allocate"></param>
		/// <returns></returns>
		long GetFilesetUid(bool allocate = true);

		/// <summary></summary>
		/// <param name="allocate"></param>
		/// <returns></returns>
		long GetCharacterUid(bool allocate = true);

		/// <summary></summary>
		/// <param name="allocate"></param>
		/// <returns></returns>
		long GetModuleUid(bool allocate = true);

		/// <summary></summary>
		/// <param name="allocate"></param>
		/// <returns></returns>
		long GetRoomUid(bool allocate = true);

		/// <summary></summary>
		/// <param name="allocate"></param>
		/// <returns></returns>
		long GetArtifactUid(bool allocate = true);

		/// <summary></summary>
		/// <param name="allocate"></param>
		/// <returns></returns>
		long GetEffectUid(bool allocate = true);

		/// <summary></summary>
		/// <param name="allocate"></param>
		/// <returns></returns>
		long GetMonsterUid(bool allocate = true);

		/// <summary></summary>
		/// <param name="allocate"></param>
		/// <returns></returns>
		long GetHintUid(bool allocate = true);

		/// <summary></summary>
		/// <param name="allocate"></param>
		/// <returns></returns>
		long GetTriggerUid(bool allocate = true);

		/// <summary></summary>
		/// <param name="allocate"></param>
		/// <returns></returns>
		long GetScriptUid(bool allocate = true);

		/// <summary></summary>
		/// <param name="allocate"></param>
		/// <returns></returns>
		long GetGameStateUid(bool allocate = true);

		/// <summary></summary>
		/// <param name="table"></param>
		/// <param name="uid"></param>
		void FreeRecordUid<T>(IDbTable<T> table, long uid) where T : class, IGameBase;

		/// <summary></summary>
		/// <param name="uid"></param>
		void FreeConfigUid(long uid);

		/// <summary></summary>
		/// <param name="uid"></param>
		void FreeFilesetUid(long uid);

		/// <summary></summary>
		/// <param name="uid"></param>
		void FreeCharacterUid(long uid);

		/// <summary></summary>
		/// <param name="uid"></param>
		void FreeModuleUid(long uid);

		/// <summary></summary>
		/// <param name="uid"></param>
		void FreeRoomUid(long uid);

		/// <summary></summary>
		/// <param name="uid"></param>
		void FreeArtifactUid(long uid);

		/// <summary></summary>
		/// <param name="uid"></param>
		void FreeEffectUid(long uid);

		/// <summary></summary>
		/// <param name="uid"></param>
		void FreeMonsterUid(long uid);

		/// <summary></summary>
		/// <param name="uid"></param>
		void FreeHintUid(long uid);

		/// <summary></summary>
		/// <param name="uid"></param>
		void FreeTriggerUid(long uid);

		/// <summary></summary>
		/// <param name="uid"></param>
		void FreeScriptUid(long uid);

		/// <summary></summary>
		/// <param name="uid"></param>
		void FreeGameStateUid(long uid);
	}
}
