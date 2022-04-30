
// PluginGlobals.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Eamon.Framework;
using Eamon.Framework.DataStorage;
using Eamon.Framework.DataStorage.Generic;
using Eamon.Framework.Plugin;
using Eamon.Framework.Portability;
using static Eamon.Game.Plugin.PluginContext;

namespace Eamon.Game.Plugin
{
	public class PluginGlobals : IPluginGlobals
	{
		public virtual StringBuilder Buf { get; set; } = new StringBuilder(Constants.BufSize);

		public virtual IDatabase Database
		{
			get
			{
				return Databases != null && DbStackTop >= 0 && DbStackTop < Databases.Length ? Databases[DbStackTop] : null;
			}
		}

		public virtual IDictionary<Type, Type> ClassMappingsDictionary
		{
			get
			{
				return ClassMappings.ClassMappingsDictionary;
			}
		}

		public virtual ITextReader In
		{
			get
			{
				return ClassMappings.In;
			}
			set
			{
				ClassMappings.In = value;
			}
		}

		public virtual ITextWriter Out
		{
			get
			{
				return ClassMappings.Out;
			}
			set
			{
				ClassMappings.Out = value;
			}
		}

		public virtual ITextWriter Error
		{
			get
			{
				return ClassMappings.Error;
			}
			set
			{
				ClassMappings.Error = value;
			}
		}

		public virtual IMutex Mutex
		{
			get
			{
				return ClassMappings.Mutex;
			}
			set
			{
				ClassMappings.Mutex = value;
			}
		}

		public virtual ITransferProtocol TransferProtocol
		{
			get
			{
				return ClassMappings.TransferProtocol;
			}
			set
			{
				ClassMappings.TransferProtocol = value;
			}
		}

		public virtual IDirectory Directory
		{
			get
			{
				return ClassMappings.Directory;
			}
			set
			{
				ClassMappings.Directory = value;
			}
		}

		public virtual IFile File
		{
			get
			{
				return ClassMappings.File;
			}
			set
			{
				ClassMappings.File = value;
			}
		}

		public virtual IPath Path
		{
			get
			{
				return ClassMappings.Path;
			}
			set
			{
				ClassMappings.Path = value;
			}
		}

		public virtual ISharpSerializer SharpSerializer
		{
			get
			{
				return ClassMappings.SharpSerializer;
			}
			set
			{
				ClassMappings.SharpSerializer = value;
			}
		}

		public virtual IThread Thread
		{
			get
			{
				return ClassMappings.Thread;
			}
			set
			{
				ClassMappings.Thread = value;
			}
		}

		public virtual MemoryStream CloneStream
		{
			get
			{
				return ClassMappings.CloneStream;
			}
			set
			{
				ClassMappings.CloneStream = value;
			}
		}

		public virtual IEngine Engine { get; set; }

		public virtual IRoom RevealContentRoom { get; set; }

		public virtual IMonster RevealContentMonster { get; set; }

		public virtual IList<IArtifact> RevealContentArtifactList { get; set; }

		public virtual IList<Action> RevealContentFuncList { get; set; }

		public virtual long RevealContentCounter { get; set; }

		public virtual string WorkDir
		{
			get
			{
				return ClassMappings.WorkDir;
			}
			set
			{
				ClassMappings.WorkDir = value;
			}
		}

		public virtual string FilePrefix
		{
			get
			{
				return ClassMappings.FilePrefix;
			}
			set
			{
				ClassMappings.FilePrefix = value;
			}
		}

		public virtual string LineSep { get; set; } = new string('-', (int)Constants.RightMargin);

		public virtual long RulesetVersion
		{
			get
			{
				return ClassMappings.RulesetVersion;
			}
		}

		public virtual bool EnableGameOverrides
		{
			get
			{
				return ClassMappings.EnableGameOverrides;
			}
		}

		public virtual bool LineWrapUserInput { get; set; }

		public virtual bool RunGameEditor
		{
			get
			{
				return ClassMappings.RunGameEditor;
			}
			set
			{
				ClassMappings.RunGameEditor = value;
			}
		}

		public virtual bool DeleteGameStateFromMainHall
		{
			get
			{
				return ClassMappings.DeleteGameStateFromMainHall;
			}
			set
			{
				ClassMappings.DeleteGameStateFromMainHall = value;
			}
		}

		public virtual bool ConvertDatafileToMscorlib
		{
			get
			{
				return ClassMappings.ConvertDatafileToMscorlib;
			}
			set
			{
				ClassMappings.ConvertDatafileToMscorlib = value;
			}
		}

		public virtual Coord CursorPosition { get; set; }

		public virtual IRecordDb<IConfig> CFGDB { get; set; }

		public virtual IRecordDb<IFileset> FSDB { get; set; }

		public virtual IRecordDb<ICharacter> CHRDB { get; set; }

		public virtual IRecordDb<IModule> MODDB { get; set; }

		public virtual IRecordDb<IRoom> RDB { get; set; }

		public virtual IRecordDb<IArtifact> ADB { get; set; }

		public virtual IRecordDb<IEffect> EDB { get; set; }

		public virtual IRecordDb<IMonster> MDB { get; set; }

		public virtual IRecordDb<IHint> HDB { get; set; }

		public virtual IRecordDb<ITrigger> TDB { get; set; }

		public virtual IRecordDb<IScript> SDB { get; set; }

		public virtual IRecordDb<IGameState> GSDB { get; set; }

		/// <summary></summary>
		public virtual IDatabase[] Databases { get; set; }

		/// <summary></summary>
		public virtual long DbStackTop { get; set; }

		public virtual void HandleException(Exception ex, string stackTraceFile, string errorMessage)
		{
			ClassMappings.HandleException(ex, stackTraceFile, errorMessage);
		}

		public virtual RetCode PushDatabase()
		{
			RetCode rc;

			rc = RetCode.Success;

			if (Databases == null || DbStackTop + 1 >= Databases.Length)
			{
				rc = RetCode.IsFull;

				goto Cleanup;
			}

			Databases[++DbStackTop] = ClassMappings.CreateInstance<IDatabase>();

		Cleanup:

			return rc;
		}

		public virtual RetCode PushDatabase(IDatabase database)
		{
			RetCode rc;

			if (database == null)
			{
				rc = RetCode.InvalidArg;

				// PrintError

				goto Cleanup;
			}

			rc = RetCode.Success;

			if (Databases == null || DbStackTop + 1 >= Databases.Length)
			{
				rc = RetCode.IsFull;

				goto Cleanup;
			}

			Databases[++DbStackTop] = database;

		Cleanup:

			return rc;
		}

		public virtual RetCode PopDatabase(bool freeDatabase = true)
		{
			RetCode rc;

			rc = RetCode.Success;

			if (Databases == null || DbStackTop < 0)
			{
				rc = RetCode.IsEmpty;

				goto Cleanup;
			}

			if (Database != null && freeDatabase)
			{
				Database.FreeConfigs();

				Database.FreeFilesets();

				Database.FreeCharacters();

				Database.FreeModules();

				Database.FreeRooms();

				Database.FreeArtifacts();

				Database.FreeEffects();

				Database.FreeMonsters();

				Database.FreeHints();

				Database.FreeTriggers();

				Database.FreeScripts();

				Database.FreeGameStates();
			}

			Databases[DbStackTop--] = null;

		Cleanup:

			return rc;
		}

		public virtual RetCode GetDatabase(long index, ref IDatabase database)
		{
			RetCode rc;

			if (Databases == null || index < 0 || index > DbStackTop)
			{
				rc = RetCode.InvalidArg;

				// PrintError

				goto Cleanup;
			}

			rc = RetCode.Success;

			database = Databases[index];

		Cleanup:

			return rc;
		}

		public virtual RetCode SaveDatabase(string fileName)
		{
			RetCode rc;

			if (string.IsNullOrWhiteSpace(fileName))
			{
				rc = RetCode.InvalidArg;

				// PrintError

				goto Cleanup;
			}

			rc = RetCode.Success;

			if (Databases == null || DbStackTop < 0 || Database == null)
			{
				rc = RetCode.IsEmpty;

				goto Cleanup;
			}

			File.Delete(fileName);

			SharpSerializer.Serialize(Database, fileName);

		Cleanup:

			return rc;
		}

		public virtual RetCode RestoreDatabase(string fileName)
		{
			RetCode rc;

			if (string.IsNullOrWhiteSpace(fileName))
			{
				rc = RetCode.InvalidArg;

				// PrintError

				goto Cleanup;
			}

			rc = RetCode.Success;

			if (Databases == null || DbStackTop + 1 >= Databases.Length)
			{
				rc = RetCode.IsFull;

				goto Cleanup;
			}

			ResetRevealContentProperties();

			UpgradeDatafile(fileName);

			var database = SharpSerializer.Deserialize<IDatabase>(fileName);

			if (database == null)
			{
				rc = RetCode.Failure;

				goto Cleanup;
			}

			RestoreRecords(database?.ConfigTable?.Records?.Cast<IGameBase>().ToList());

			RestoreRecords(database?.FilesetTable?.Records?.Cast<IGameBase>().ToList());

			RestoreRecords(database?.CharacterTable?.Records?.Cast<IGameBase>().ToList());

			RestoreRecords(database?.ModuleTable?.Records?.Cast<IGameBase>().ToList());

			RestoreRecords(database?.RoomTable?.Records?.Cast<IGameBase>().ToList());

			RestoreRecords(database?.ArtifactTable?.Records?.Cast<IGameBase>().ToList());

			RestoreRecords(database?.EffectTable?.Records?.Cast<IGameBase>().ToList());

			RestoreRecords(database?.MonsterTable?.Records?.Cast<IGameBase>().ToList());

			RestoreRecords(database?.HintTable?.Records?.Cast<IGameBase>().ToList());

			RestoreRecords(database?.TriggerTable?.Records?.Cast<IGameBase>().ToList());

			RestoreRecords(database?.ScriptTable?.Records?.Cast<IGameBase>().ToList());

			RestoreRecords(database?.GameStateTable?.Records?.Cast<IGameBase>().ToList());

			Databases[++DbStackTop] = database;

		Cleanup:

			return rc;
		}

		public virtual RetCode ClearDbStack()
		{
			RetCode rc;

			rc = RetCode.Success;

			while (DbStackTop >= 0)
			{
				rc = PopDatabase();

				if (rc != RetCode.Success)
				{
					break;
				}
			}

			return rc;
		}

		public virtual RetCode GetDbStackTop(ref long dbStackTop)
		{
			RetCode rc;

			rc = RetCode.Success;

			dbStackTop = DbStackTop;

			return rc;
		}

		public virtual RetCode GetDbStackSize(ref long dbStackSize)
		{
			RetCode rc;

			rc = RetCode.Success;

			dbStackSize = Databases != null ? Databases.Length : 0;

			return rc;
		}

		public virtual void InitSystem()
		{
			if (!ClassMappings.IgnoreMutex)
			{
				ClassMappings.Mutex.CreateAndWaitOne();
			}

			Engine = ClassMappings.CreateInstance<IEngine>();

			RevealContentArtifactList = new List<IArtifact>();

			RevealContentFuncList = new List<Action>();

			RevealContentCounter = 1;

			Databases = new IDatabase[Constants.NumDatabases];

			DbStackTop = -1;

			PushDatabase();

			CFGDB = ClassMappings.CreateInstance<IRecordDb<IConfig>>();

			FSDB = ClassMappings.CreateInstance<IRecordDb<IFileset>>();

			CHRDB = ClassMappings.CreateInstance<IRecordDb<ICharacter>>();

			MODDB = ClassMappings.CreateInstance<IRecordDb<IModule>>();

			RDB = ClassMappings.CreateInstance<IRecordDb<IRoom>>();

			ADB = ClassMappings.CreateInstance<IRecordDb<IArtifact>>();

			EDB = ClassMappings.CreateInstance<IRecordDb<IEffect>>();

			MDB = ClassMappings.CreateInstance<IRecordDb<IMonster>>();

			HDB = ClassMappings.CreateInstance<IRecordDb<IHint>>();

			TDB = ClassMappings.CreateInstance<IRecordDb<ITrigger>>();

			SDB = ClassMappings.CreateInstance<IRecordDb<IScript>>();

			GSDB = ClassMappings.CreateInstance<IRecordDb<IGameState>>();
		}

		public virtual void DeinitSystem()
		{
			ClearDbStack();
		}

		public virtual RetCode PushRulesetVersion(long rulesetVersion)
		{
			return ClassMappings.PushRulesetVersion(rulesetVersion);
		}

		public virtual RetCode PopRulesetVersion()
		{
			return ClassMappings.PopRulesetVersion();
		}

		public virtual RetCode ClearRvStack()
		{
			return ClassMappings.ClearRvStack();
		}

		public virtual RetCode GetRvStackTop(ref long rvStackTop)
		{
			return ClassMappings.GetRvStackTop(ref rvStackTop);
		}

		public virtual RetCode GetRvStackSize(ref long rvStackSize)
		{
			return ClassMappings.GetRvStackSize(ref rvStackSize);
		}

		public virtual T CreateInstance<T>(Type ifaceType, Action<T> initialize = null) where T : class
		{
			return ClassMappings.CreateInstance(ifaceType, initialize);
		}

		public virtual T CreateInstance<T>(Action<T> initialize = null) where T : class
		{
			return ClassMappings.CreateInstance(initialize);
		}

		public virtual T CloneInstance<T>(T source) where T : class
		{
			return ClassMappings.CloneInstance(source);
		}

		public virtual bool CompareInstances<T>(T object1, T object2) where T : class
		{
			return ClassMappings.CompareInstances(object1, object2);
		}

		public virtual bool IsRulesetVersion(params long[] versions)
		{
			return ClassMappings.IsRulesetVersion(versions);
		}

		public virtual string GetPrefixedFileName(string fileName)
		{
			return ClassMappings.GetPrefixedFileName(fileName);
		}

		public virtual void ReplaceDatafileValues(string fileName, string[] patterns, string[] replacements)
		{
			if (string.IsNullOrWhiteSpace(fileName) || patterns == null || replacements == null || patterns.Length != replacements.Length)
			{
				// PrintError

				goto Cleanup;
			}

			var contents = File.ReadAllText(fileName);

			for (var i = 0; i < patterns.Length; i++)
			{
				contents = Regex.Replace(contents, patterns[i], replacements[i]);
			}

			File.WriteAllText(fileName, contents);

		Cleanup:

			;
		}

		public virtual void ReplaceDatafileValues01(string fileName)
		{
			if (string.IsNullOrWhiteSpace(fileName))
			{
				// PrintError

				goto Cleanup;
			}

			var contentsModified = false;

			var contents = File.ReadAllText(fileName);

			if (ConvertDatafileToMscorlib)
			{
				if (Regex.IsMatch(contents, Constants.CoreLibRegexPattern))
				{
					contents = Regex.Replace(contents, Constants.CoreLibRegexPattern, Constants.MscorlibName);

					contentsModified = true;
				}
			}
			else
			{ 
				if (Regex.IsMatch(contents, Constants.MscorlibRegexPattern))
				{
					contents = Regex.Replace(contents, Constants.MscorlibRegexPattern, Constants.CoreLibName);

					contentsModified = true;
				}
			}

			if (contentsModified)
			{
				File.WriteAllText(fileName, contents);
			}

		Cleanup:

			;
		}

		public virtual void UpgradeDatafile(string fileName)
		{
			if (string.IsNullOrWhiteSpace(fileName))
			{
				// PrintError

				goto Cleanup;
			}

			var workDir = Directory.GetCurrentDirectory().Replace('/', '\\');

			var needsUpgrade = true;

			while (needsUpgrade)
			{
				var firstLine = File.ReadFirstLine(fileName);

				var upgraded = false;

				if (firstLine.Contains("Version=1.3.0.0"))
				{
					if (firstLine.Contains("Game.DataStorage.ArtifactDbTable") || firstLine.Contains("Game.DataStorage.Database"))
					{
						ReplaceDatafileValues
						(
							fileName,
							new string[]
							{
								@"Version=1\.3\.0\.0",
								@"EAMON CS 1\.3",
								@"Eamon\.Game\.Primitive\.Classes\.ArtifactClass",
								"<SingleArray name=\"Classes\">",
								"<Simple name=\"Field5\"",
								"<Simple name=\"Field6\"",
								"<Simple name=\"Field7\"",
								"<Simple name=\"Field8\"",
							},
							new string[]
							{
								"Version=1.4.0.0",
								"EAMON CS 1.4",
								"Eamon.Game.Primitive.Classes.ArtifactCategory",
								"<SingleArray name=\"Categories\">",
								"<Simple name=\"Field1\"",
								"<Simple name=\"Field2\"",
								"<Simple name=\"Field3\"",
								"<Simple name=\"Field4\"",
							}
						);

						upgraded = true;
					}

					if (!upgraded)
					{
						ReplaceDatafileValues
						(
							fileName,
							new string[]
							{
								@"Version=1\.3\.0\.0",
								@"EAMON CS 1\.3",
							},
							new string[]
							{
								"Version=1.4.0.0",
								"EAMON CS 1.4",
							}
						);
					}
				}
				else if (firstLine.Contains("Version=1.4.0.0"))
				{
					if (firstLine.Contains("Game.DataStorage.CharacterDbTable") || firstLine.Contains("Game.DataStorage.Database"))
					{
						ReplaceDatafileValues
						(
							fileName,
							new string[]
							{
								@"Version=1\.4\.0\.0",
								@"EAMON CS 1\.4",
								@"Eamon\.Game\.Primitive\.Classes\.CharacterWeapon",
								"<Simple name=\"Complexity\"",
								"<Simple name=\"Type\" value=\"Axe\"",
								"<Simple name=\"Type\" value=\"Bow\"",
								"<Simple name=\"Type\" value=\"Club\"",
								"<Simple name=\"Type\" value=\"Spear\"",
								"<Simple name=\"Type\" value=\"Sword\"",
								"<Simple name=\"Dice\"",
								"<Simple name=\"Sides\"",
							},
							new string[]
							{
								"Version=1.5.0.0",
								"EAMON CS 1.5",
								"Eamon.Game.Primitive.Classes.CharacterArtifact",
								"<Simple name=\"Field1\"",
								"<Simple name=\"Field2\" value=\"1\"",
								"<Simple name=\"Field2\" value=\"2\"",
								"<Simple name=\"Field2\" value=\"3\"",
								"<Simple name=\"Field2\" value=\"4\"",
								"<Simple name=\"Field2\" value=\"5\"",
								"<Simple name=\"Field3\"",
								"<Simple name=\"Field4\"",
							}
						);

						upgraded = true;
					}

					if (!upgraded)
					{
						ReplaceDatafileValues
						(
							fileName,
							new string[]
							{
								@"Version=1\.4\.0\.0",
								@"EAMON CS 1\.4",
							},
							new string[]
							{
								"Version=1.5.0.0",
								"EAMON CS 1.5",
							}
						);
					}
				}
				else if (firstLine.Contains("Version=1.5.0.0"))
				{
					if (firstLine.Contains("Game.DataStorage.ArtifactDbTable") || firstLine.Contains("Game.DataStorage.Database"))
					{
						ReplaceDatafileValues
						(
							fileName,
							new string[]
							{
								@"Version=1\.5\.0\.0",
								@"EAMON CS 1\.5",
								"<Simple name=\"Type\" value=\"Container\"",
								"<Simple name=\"Location\" value=\"4(...)\"",
								"<Simple name=\"Location\" value=\"2(...)\"",
							},
							new string[]
							{
								"Version=1.6.0.0",
								"EAMON CS 1.6",
								"<Simple name=\"Type\" value=\"InContainer\"",
								"<Simple name=\"Location\" value=\"7$1\"",
								"<Simple name=\"Location\" value=\"5$1\"",
							}
						);

						if (workDir.EndsWith(@"\Adventures\ARuncibleCargo"))
						{
							ReplaceDatafileValues
							(
								fileName,
								new string[]
								{
									@"Eamon\.Game\.Artifact, Eamon",
								},
								new string[]
								{
									"ARuncibleCargo.Game.Artifact, ARuncibleCargo",
								}
							);
						}

						upgraded = true;
					}

					if (firstLine.Contains("Game.DataStorage.MonsterDbTable") || firstLine.Contains("Game.DataStorage.Database"))
					{
						if (workDir.EndsWith(@"\Adventures\TheTrainingGround"))
						{
							ReplaceDatafileValues
							(
								fileName,
								new string[]
								{
									@"Eamon\.Game\.Monster, Eamon",
								},
								new string[]
								{
									"TheTrainingGround.Game.Monster, TheTrainingGround",
								}
							);
						}
					}

					if (!upgraded)
					{
						ReplaceDatafileValues
						(
							fileName,
							new string[]
							{
								@"Version=1\.5\.0\.0",
								@"EAMON CS 1\.5",
							},
							new string[]
							{
								"Version=1.6.0.0",
								"EAMON CS 1.6",
							}
						);
					}
				}
				else if (firstLine.Contains("Version=1.6.0.0"))
				{
					if (firstLine.Contains("Game.DataStorage.RoomDbTable") || firstLine.Contains("Game.DataStorage.Database"))
					{
						ReplaceDatafileValues
						(
							fileName,
							new string[]
							{
								@"Version=1\.6\.0\.0",
								@"EAMON CS 1\.6",
								"(<SingleArray name=\"Dirs\">\\r?\\n?.*<Items>)((.*\\r?\\n?){12})",
							},
							new string[]
							{
								"Version=1.7.0.0",
								"EAMON CS 1.7",
								string.Format("$1$2{0}<Simple value=\"0\" />{1}{0}<Simple value=\"0\" />{1}", new String(' ', firstLine.Contains("Game.DataStorage.Database") ? 20 : 16), Environment.NewLine),
							}
						);

						upgraded = true;
					}

					if (firstLine.Contains("Game.DataStorage.ModuleDbTable") || firstLine.Contains("Game.DataStorage.Database"))
					{
						ReplaceDatafileValues
						(
							fileName,
							new string[]
							{
								"<Simple name=\"NumDirs\" value=\"10\"",
							},
							new string[]
							{
								"<Simple name=\"NumDirs\" value=\"12\"",
							}
						);
					}

					if (!upgraded)
					{
						ReplaceDatafileValues
						(
							fileName,
							new string[]
							{
								@"Version=1\.6\.0\.0",
								@"EAMON CS 1\.6",
							},
							new string[]
							{
								"Version=1.7.0.0",
								"EAMON CS 1.7",
							}
						);
					}
				}
				else if (firstLine.Contains("Version=1.7.0.0"))
				{
					if (firstLine.Contains("Game.DataStorage.ConfigDbTable") || firstLine.Contains("Game.DataStorage.FilesetDbTable") || firstLine.Contains("Game.DataStorage.Database"))
					{

						ReplaceDatafileValues
						(
							fileName,
							new string[]
							{
								@"Version=1\.7\.0\.0",
								@"EAMON CS 1\.7",
								"<Simple name=\"(.*)\" value=\"(.*)\\.XML\"",
							},
							new string[]
							{
								"Version=1.8.0.0",
								"EAMON CS 1.8",
								"<Simple name=\"$1\" value=\"$2.DAT\"",
							}
						);

						upgraded = true;
					}

					if (!upgraded)
					{
						ReplaceDatafileValues
						(
							fileName,
							new string[]
							{
								@"Version=1\.7\.0\.0",
								@"EAMON CS 1\.7",
							},
							new string[]
							{
								"Version=1.8.0.0",
								"EAMON CS 1.8",
							}
						);
					}
				}
				else
				{
					needsUpgrade = false;
				}
			}

			ReplaceDatafileValues01(fileName);

		Cleanup:

			;
		}

		public virtual void ResetRevealContentProperties(bool resetObjects = true)
		{
			if (resetObjects)
			{
				RevealContentRoom = null;

				RevealContentMonster = null;
			}

			RevealContentArtifactList.Clear();

			RevealContentFuncList.Clear();
		}

		/// <summary></summary>
		/// <param name="recordList"></param>
		public virtual void RestoreRecords(IList<IGameBase> recordList)
		{
			if (recordList != null)
			{
				foreach (var r in recordList)
				{
					r.SetParentReferences();

					// Note: may want to be really rigorous here and also validate record

					if (r is IMonster)
					{
						// Note: may want to be really rigorous here and also validate weapon/shield combo
					}
				}
			}
		}
	}
}
