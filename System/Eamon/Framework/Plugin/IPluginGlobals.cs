
// IPluginGlobals.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Eamon.Framework.DataStorage;
using Eamon.Framework.DataStorage.Generic;
using Eamon.Framework.Portability;

namespace Eamon.Framework.Plugin
{
	/// <summary></summary>
	public interface IPluginGlobals
	{
		/// <summary></summary>
		StringBuilder Buf { get; set; }

		/// <summary></summary>
		IDatabase Database { get; }

		/// <summary></summary>
		IDictionary<Type, Type> ClassMappingsDictionary { get; }

		/// <summary></summary>
		ITextReader In { get; set; }

		/// <summary></summary>
		ITextWriter Out { get; set; }

		/// <summary></summary>
		ITextWriter Error { get; set; }

		/// <summary></summary>
		IMutex Mutex { get; set; }

		/// <summary></summary>
		ITransferProtocol TransferProtocol { get; set; }

		/// <summary></summary>
		IDirectory Directory { get; set; }

		/// <summary></summary>
		IFile File { get; set; }

		/// <summary></summary>
		IPath Path { get; set; }

		/// <summary></summary>
		ISharpSerializer SharpSerializer { get; set; }

		/// <summary></summary>
		IThread Thread { get; set; }

		/// <summary></summary>
		MemoryStream CloneStream { get; set; }

		/// <summary></summary>
		IEngine Engine { get; set; }

		/// <summary></summary>
		IRoom RevealContentRoom { get; set; }

		/// <summary></summary>
		IMonster RevealContentMonster { get; set; }

		/// <summary></summary>
		IList<IArtifact> RevealContentArtifactList { get; set; }

		/// <summary></summary>
		IList<Action> RevealContentFuncList { get; set; }

		/// <summary></summary>
		long RevealContentCounter { get; set; }

		/// <summary></summary>
		string WorkDir { get; set; }

		/// <summary></summary>
		string FilePrefix { get; set; }

		/// <summary></summary>
		string LineSep { get; set; }

		/// <summary></summary>
		long RulesetVersion { get; }

		/// <summary></summary>
		bool EnableGameOverrides { get; }

		/// <summary></summary>
		bool LineWrapUserInput { get; set; }

		/// <summary></summary>
		bool RunGameEditor { get; set; }

		/// <summary></summary>
		bool DeleteGameStateFromMainHall { get; set; }

		/// <summary></summary>
		bool ConvertDatafileToMscorlib { get; set; }

		/// <summary></summary>
		Coord CursorPosition { get; set; }

		/// <summary></summary>
		IRecordDb<IConfig> CFGDB { get; set; }

		/// <summary></summary>
		IRecordDb<IFileset> FSDB { get; set; }

		/// <summary></summary>
		IRecordDb<ICharacter> CHRDB { get; set; }

		/// <summary></summary>
		IRecordDb<IModule> MODDB { get; set; }

		/// <summary></summary>
		IRecordDb<IRoom> RDB { get; set; }

		/// <summary></summary>
		IRecordDb<IArtifact> ADB { get; set; }

		/// <summary></summary>
		IRecordDb<IEffect> EDB { get; set; }

		/// <summary></summary>
		IRecordDb<IMonster> MDB { get; set; }

		/// <summary></summary>
		IRecordDb<IHint> HDB { get; set; }

		/// <summary></summary>
		IRecordDb<ITrigger> TDB { get; set; }

		/// <summary></summary>
		IRecordDb<IScript> SDB { get; set; }

		/// <summary></summary>
		IRecordDb<IGameState> GSDB { get; set; }

		/// <summary></summary>
		/// <param name="ex"></param>
		/// <param name="stackTraceFile"></param>
		/// <param name="errorMessage"></param>
		void HandleException(Exception ex, string stackTraceFile, string errorMessage);

		/// <summary></summary>
		/// <returns></returns>
		RetCode PushDatabase();

		/// <summary></summary>
		/// <param name="database"></param>
		/// <returns></returns>
		RetCode PushDatabase(IDatabase database);

		/// <summary></summary>
		/// <param name="freeDatabase"></param>
		/// <returns></returns>
		RetCode PopDatabase(bool freeDatabase = true);

		/// <summary></summary>
		/// <param name="index"></param>
		/// <param name="database"></param>
		/// <returns></returns>
		RetCode GetDatabase(long index, ref IDatabase database);

		/// <summary></summary>
		/// <param name="fileName"></param>
		/// <returns></returns>
		RetCode SaveDatabase(string fileName);

		/// <summary></summary>
		/// <param name="fileName"></param>
		/// <returns></returns>
		RetCode RestoreDatabase(string fileName);

		/// <summary></summary>
		/// <returns></returns>
		RetCode ClearDbStack();

		/// <summary></summary>
		/// <param name="dbStackTop"></param>
		/// <returns></returns>
		RetCode GetDbStackTop(ref long dbStackTop);

		/// <summary></summary>
		/// <param name="dbStackSize"></param>
		/// <returns></returns>
		RetCode GetDbStackSize(ref long dbStackSize);

		/// <summary></summary>
		void InitSystem();

		/// <summary></summary>
		void DeinitSystem();

		/// <summary></summary>
		/// <param name="rulesetVersion"></param>
		/// <returns></returns>
		RetCode PushRulesetVersion(long rulesetVersion);

		/// <summary></summary>
		/// <returns></returns>
		RetCode PopRulesetVersion();

		/// <summary></summary>
		/// <returns></returns>
		RetCode ClearRvStack();

		/// <summary></summary>
		/// <param name="rvStackTop"></param>
		/// <returns></returns>
		RetCode GetRvStackTop(ref long rvStackTop);

		/// <summary></summary>
		/// <param name="rvStackSize"></param>
		/// <returns></returns>
		RetCode GetRvStackSize(ref long rvStackSize);

		/// <summary></summary>
		/// <param name="ifaceType"></param>
		/// <param name="initialize"></param>
		/// <returns></returns>
		T CreateInstance<T>(Type ifaceType, Action<T> initialize = null) where T : class;

		/// <summary></summary>
		/// <param name="initialize"></param>
		/// <returns></returns>
		T CreateInstance<T>(Action<T> initialize = null) where T : class;

		/// <summary></summary>
		/// <param name="source"></param>
		/// <returns></returns>
		T CloneInstance<T>(T source) where T : class;

		/// <summary></summary>
		/// <param name="object1"></param>
		/// <param name="object2"></param>
		/// <returns></returns>
		bool CompareInstances<T>(T object1, T object2) where T : class;

		/// <summary></summary>
		/// <param name="versions"></param>
		/// <returns></returns>
		bool IsRulesetVersion(params long[] versions);

		/// <summary></summary>
		/// <param name="fileName"></param>
		/// <returns></returns>
		string GetPrefixedFileName(string fileName);

		/// <summary></summary>
		/// <param name="fileName"></param>
		/// <param name="patterns"></param>
		/// <param name="replacements"></param>
		void ReplaceDatafileValues(string fileName, string[] patterns, string[] replacements);

		/// <summary></summary>
		/// <param name="fileName"></param>
		void ReplaceDatafileValues01(string fileName);

		/// <summary></summary>
		/// <param name="fileName"></param>
		void UpgradeDatafile(string fileName);

		/// <summary></summary>
		/// <param name="resetObjects"></param>
		void ResetRevealContentProperties(bool resetObjects = true);
	}
}
