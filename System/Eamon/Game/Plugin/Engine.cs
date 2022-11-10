
// Engine.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Eamon.Framework;
using Eamon.Framework.Args;
using Eamon.Framework.DataStorage;
using Eamon.Framework.DataStorage.Generic;
using Eamon.Framework.Helpers;
using Eamon.Framework.Plugin;
using Eamon.Framework.Portability;
using Eamon.Framework.Primitive.Classes;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using Eamon.Game.Utilities;
//using static Eamon.Game.Plugin.Globals;

namespace Eamon.Game.Plugin
{
	public class Engine : IEngine
	{
		#region Public Properties

		public virtual string[] CommandSepTokens { get; protected set; }

		public virtual string[] PronounTokens { get; protected set; }

		public virtual string ToughDesc { get; protected set; }

		public virtual string CourageDesc { get; protected set; }

		public virtual int ArtNameLen { get; protected set; } = 40;

		public virtual int ArtStateDescLen { get; protected set; } = 64;

		public virtual int ArtDescLen { get; protected set; } = 256;

		public virtual int CharNameLen { get; protected set; } = 32;

		public virtual int CharArtNameLen { get; protected set; } = 40;

		public virtual int CharArtDescLen { get; protected set; } = 256;

		public virtual int EffDescLen { get; protected set; } = 256;

		public virtual int FsNameLen { get; protected set; } = 40;

		public virtual int FsFileNameLen { get; protected set; } = 64;

		public virtual int HntQuestionLen { get; protected set; } = 256;

		public virtual int HntAnswerLen { get; protected set; } = 256;

		public virtual int ModNameLen { get; protected set; } = 40;

		public virtual int ModDescLen { get; protected set; } = 256;

		public virtual int ModAuthorLen { get; protected set; } = 40;

		public virtual int ModVolLabelLen { get; protected set; } = 7;

		public virtual int ModSerialNumLen { get; protected set; } = 3;

		public virtual int MonNameLen { get; protected set; } = 40;

		public virtual int MonStateDescLen { get; protected set; } = 64;

		public virtual int MonDescLen { get; protected set; } = 256;

		public virtual int RmNameLen { get; protected set; } = 50;

		public virtual int RmDescLen { get; protected set; } = 256;

		public virtual long AxePrice { get; protected set; } = 25;

		public virtual long BowPrice { get; protected set; } = 40;

		public virtual long MacePrice { get; protected set; } = 20;

		public virtual long SpearPrice { get; protected set; } = 25;

		public virtual long SwordPrice { get; protected set; } = 30;

		public virtual long ShieldPrice { get; protected set; } = 50;

		public virtual long LeatherArmorPrice { get; protected set; } = 100;

		public virtual long ChainMailPrice { get; protected set; } = 250;

		public virtual long PlateMailPrice { get; protected set; } = 500;

		public virtual long BlastPrice { get; protected set; } = 1000;

		public virtual long HealPrice { get; protected set; } = 500;

		public virtual long SpeedPrice { get; protected set; } = 4000;

		public virtual long PowerPrice { get; protected set; } = 100;

		public virtual long RecallPrice { get; protected set; } = 100;

		public virtual long StatGainPrice { get; protected set; } = 4500;

		public virtual long WeaponTrainingPrice { get; protected set; } = 1000;

		public virtual long ArmorTrainingPrice { get; protected set; } = 1000;

		public virtual long SpellTrainingPrice { get; protected set; } = 1000;

		public virtual long InfoBoothPrice { get; protected set; } = 30;

		public virtual long FountainPrice { get; protected set; } = 20;

		public virtual long NumDatabases { get; protected set; } = 10;

		public virtual long NumRulesetVersions { get; protected set; } = 10;

		public virtual long NumArtifactCategories { get; protected set; } = 4;

		public virtual int BufSize { get; protected set; } = 1024;

		public virtual int BufSize01 { get; protected set; } = 6;

		public virtual int BufSize02 { get; protected set; } = 1;

		public virtual int BufSize03 { get; protected set; } = 64;

		public virtual string ResolveEffectRegexPattern { get; protected set; } = @"[^\*]\*[0-9]{3}|\*\*[0-9]{3}";

		public virtual string ResolveUidMacroRegexPattern { get; protected set; } = @"[^\*]\*[0-9]{3}|\*\*[0-9]{3}|[^@]@[0-9]{3}|@@[0-9]{3}";

		public virtual string ValidWorkDirRegexPattern { get; protected set; } = @"^(NONE)$|^(\.)$|^(\.\.\\\.\.\\Adventures\\[a-zA-Z0-9]+)$|^(\.\.\/\.\.\/Adventures\/[a-zA-Z0-9]+)$";

		public virtual string CoreLibRegexPattern { get; protected set; } = @"System\.Private\.CoreLib, Version=6\.0\.0\.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e";

		public virtual string MscorlibRegexPattern { get; protected set; } = @"mscorlib, Version=4\.0\.0\.0, Culture=neutral, PublicKeyToken=b77a5c561934e089";

		public virtual string CoreLibName { get; protected set; } = @"System.Private.CoreLib, Version=6.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e";

		public virtual string MscorlibName { get; protected set; } = @"mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089";

		public virtual string RecIdepErrorFmtStr { get; protected set; } = "The {0} field refers to {1} Uid {2}, {3}.";

		public virtual string AndroidAdventuresDir { get; protected set; } = @"..\EamonPM.Android\Assets\Adventures";

		public virtual string AdventuresDir { get; protected set; } = @"..\..\Adventures";

		public virtual string QuickLaunchDir { get; protected set; } = @"..\..\QuickLaunch";

		public virtual string DefaultWorkDir { get; protected set; } = ".";

		public virtual string ProcessMutexName { get; protected set; }

		public virtual string EamonDesktopSlnFile { get; protected set; } = @"..\..\Eamon.Desktop.sln";

		public virtual string StackTraceFile { get; protected set; } = "STACKTRACE.TXT";

		public virtual string ProgVersion { get; protected set; } = "2.1.0";

		public virtual long InfiniteDrinkableEdible { get; protected set; } = 9999;

		public virtual long DirectionExit { get; protected set; } = -999;

		public virtual long LimboLocation { get; protected set; } = 0;

		public virtual long MinWeaponComplexity { get; protected set; } = -50;

		public virtual long MaxWeaponComplexity { get; protected set; } = 50;

		public virtual long MinGoldValue { get; protected set; } = -99999;

		public virtual long MaxGoldValue { get; protected set; } = 999999;

		public virtual long MaxPathLen { get; protected set; } = 256;

		public virtual long MaxRecursionLevel { get; protected set; } = 100;

		public virtual int WindowWidth { get; protected set; } = 80;

		public virtual int WindowHeight { get; protected set; } = 50;

		public virtual int BufferWidth { get; protected set; } = 80;

		public virtual int BufferHeight { get; protected set; } = 9999;

		public virtual long RightMargin { get; protected set; } = 78;

		public virtual long NumRows { get; protected set; } = 22;

		public virtual IDictionary<Type, Type> ClassMappingsDictionary { get; set; } = new Dictionary<Type, Type>();

		public virtual ITextReader In { get; set; }

		public virtual ITextWriter Out { get; set; }

		public virtual ITextWriter Error { get; set; }

		public virtual IMutex Mutex { get; set; }

		public virtual ITransferProtocol TransferProtocol { get; set; }

		public virtual IDirectory Directory { get; set; }

		public virtual IFile File { get; set; }

		public virtual IPath Path { get; set; }

		public virtual ISharpSerializer SharpSerializer { get; set; }

		public virtual IThread Thread { get; set; }

		public virtual MemoryStream CloneStream { get; set; } = new MemoryStream();

		public virtual long MutatePropertyCounter { get; set; } = 1;

		public virtual string WorkDir { get; set; } = "";

		public virtual string FilePrefix { get; set; } = "";

		public virtual long RulesetVersion
		{
			get
			{
				return RulesetVersions != null && RvStackTop >= 0 && RvStackTop < RulesetVersions.Length ? RulesetVersions[RvStackTop] : 0;
			}
		}

		public virtual bool EnableMutateProperties
		{
			get
			{
				return MutatePropertyCounter > 0;
			}
		}

		public virtual bool EnableStdio { get; set; } = true;

		public virtual bool IgnoreMutex { get; set; }

		public virtual bool DisableValidation { get; set; }

		public virtual bool RunGameEditor { get; set; }

		public virtual bool DeleteGameStateFromMainHall { get; set; }

		public virtual bool ConvertDatafileToMscorlib { get; set; }

		public virtual Action<IDictionary<Type, Type>> LoadPortabilityClassMappings { get; set; }

		/// <summary></summary>
		public virtual long[] RulesetVersions { get; set; }

		/// <summary></summary>
		public virtual long RvStackTop { get; set; } = -1;

		public virtual StringBuilder Buf { get; set; }

		public virtual IDatabase Database
		{
			get
			{
				return Databases != null && DbStackTop >= 0 && DbStackTop < Databases.Length ? Databases[DbStackTop] : null;
			}
		}

		public virtual IRoom RevealContentRoom { get; set; }

		public virtual IMonster RevealContentMonster { get; set; }

		public virtual IList<IArtifact> RevealContentArtifactList { get; set; }

		public virtual IList<Action> RevealContentFuncList { get; set; }

		public virtual long RevealContentCounter { get; set; }

		public virtual string LineSep { get; set; }

		public virtual bool LineWrapUserInput { get; set; }

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

		public virtual IRecordDb<IGameState> GSDB { get; set; }

		/// <summary></summary>
		public virtual IDatabase[] Databases { get; set; }

		/// <summary></summary>
		public virtual long DbStackTop { get; set; }

		public virtual IDictionary<long, Func<string>> MacroFuncs { get; set; }

		public virtual Action<IRoom, IMonster, IArtifact, long, bool> RevealContainerContentsFunc { get; set; }

		public virtual IPrep[] Preps { get; set; }

		public virtual string[] Articles { get; set; }

		public virtual string UnknownName { get; set; }

		/// <summary></summary>
		public virtual Random Rand { get; set; }

		/// <summary></summary>
		public virtual string[] NumberStrings { get; set; }

		/// <summary></summary>
		public virtual string[] FieldDescNames { get; set; }

		/// <summary>
		/// Gets or sets an array containing the name for each <see cref="Status"/>.
		/// </summary>
		public virtual string[] StatusNames { get; set; }

		/// <summary>
		/// Gets or sets an array containing the name for each <see cref="Clothing"/>.
		/// </summary>
		public virtual string[] ClothingNames { get; set; }

		/// <summary>
		/// Gets or sets an array containing the description for each <see cref="CombatCode"/>.
		/// </summary>
		public virtual string[] CombatCodeDescs { get; set; }

		/// <summary></summary>
		public virtual string[] ContainerDisplayCodeDescs { get; set; }

		/// <summary>
		/// Gets or sets an array containing the name for each <see cref="LightLevel"/>.
		/// </summary>
		public virtual string[] LightLevelNames { get; set; }

		/// <summary>
		/// Gets or sets an array containing data for each <see cref="Stat"/>.
		/// </summary>
		public virtual IStat[] Stats { get; set; }

		/// <summary>
		/// Gets or sets an array containing data for each <see cref="Spell"/>.
		/// </summary>
		public virtual ISpell[] Spells { get; set; }

		/// <summary>
		/// Gets or sets an array containing data for each <see cref="Weapon"/>.
		/// </summary>
		public virtual IWeapon[] Weapons { get; set; }

		/// <summary>
		/// Gets or sets an array containing data for each <see cref="Armor"/>.
		/// </summary>
		public virtual IArmor[] Armors { get; set; }

		/// <summary>
		/// Gets or sets an array containing data for each <see cref="Direction"/>.
		/// </summary>
		public virtual IDirection[] Directions { get; set; }

		/// <summary>
		/// Gets or sets an array containing data for each <see cref="ArtifactType"/>.
		/// </summary>
		public virtual IArtifactType[] ArtifactTypes { get; set; }

		#endregion

		#region Public Methods

		public virtual void HandleException(Exception ex, string stackTraceFile, string errorMessage)
		{
			Debug.Assert(ex != null && !string.IsNullOrWhiteSpace(stackTraceFile) && !string.IsNullOrWhiteSpace(errorMessage));

			stackTraceFile = Path.Combine(".", stackTraceFile);

			try
			{
				File.WriteAllText(stackTraceFile, ex.ToString());
			}
			catch (Exception)
			{
				// do nothing
			}

			var errorBuf = new StringBuilder(BufSize);

			var exceptionMessage = !string.IsNullOrWhiteSpace(ex.Message) ? ex.Message.Trim() : null;

			errorBuf.AppendFormat("{0}Error: {1}{2}{3}.",
				Environment.NewLine,
				exceptionMessage != null ? exceptionMessage : "",
				exceptionMessage != null && exceptionMessage.EndsWith(".") ? " " : exceptionMessage != null ? ". " : "",
				string.Format("See stack trace file [{0}] for more details", stackTraceFile));

			errorBuf.AppendFormat("{0}", errorMessage);

			Debug.Assert(Error != null);

			Error.WriteLine(errorBuf);
		}

		public virtual void ResolvePortabilityClassMappings()
		{
			Debug.Assert(LoadPortabilityClassMappings != null);

			LoadPortabilityClassMappings(ClassMappingsDictionary);

			In = CreateInstance<ITextReader>(x =>
			{
				x.EnableInput = EnableStdio;
			});

			Debug.Assert(In != null);

			Out = CreateInstance<ITextWriter>(x =>
			{
				x.EnableOutput = EnableStdio;
			});

			Debug.Assert(Out != null);

			Error = CreateInstance<ITextWriter>(x =>
			{
				x.EnableOutput = EnableStdio;

				x.Stdout = false;
			});

			Debug.Assert(Error != null);

			Mutex = CreateInstance<IMutex>();

			Debug.Assert(Mutex != null);

			TransferProtocol = CreateInstance<ITransferProtocol>();

			Debug.Assert(TransferProtocol != null);

			Directory = CreateInstance<IDirectory>();

			Debug.Assert(Directory != null);

			File = CreateInstance<IFile>();

			Debug.Assert(File != null);

			Path = CreateInstance<IPath>();

			Debug.Assert(Path != null);

			SharpSerializer = CreateInstance<ISharpSerializer>();

			Debug.Assert(SharpSerializer != null);

			Thread = CreateInstance<IThread>();

			Debug.Assert(Thread != null);
		}

		public virtual void ProcessArgv(string[] args)
		{
			if (args == null)
			{
				// PrintError

				goto Cleanup;
			}

			for (var i = 0; i < args.Length; i++)
			{
				if (args[i].Equals("--workingDirectory", StringComparison.OrdinalIgnoreCase) || args[i].Equals("-wd", StringComparison.OrdinalIgnoreCase))
				{
					if (++i < args.Length)
					{
						WorkDir = args[i].Trim();

						var regex = new Regex(ValidWorkDirRegexPattern);

						if (WorkDir.Equals("NONE", StringComparison.OrdinalIgnoreCase) || !regex.IsMatch(WorkDir))
						{
							WorkDir = DefaultWorkDir;
						}
					}
				}
				else if (args[i].Equals("--filePrefix", StringComparison.OrdinalIgnoreCase) || args[i].Equals("-fp", StringComparison.OrdinalIgnoreCase))
				{
					if (++i < args.Length)
					{
						FilePrefix = args[i].Trim();
					}
				}
				else if (args[i].Equals("--ignoreMutex", StringComparison.OrdinalIgnoreCase) || args[i].Equals("-im", StringComparison.OrdinalIgnoreCase))
				{
					IgnoreMutex = true;
				}
				else if (args[i].Equals("--disableValidation", StringComparison.OrdinalIgnoreCase) || args[i].Equals("-dv", StringComparison.OrdinalIgnoreCase))
				{
					DisableValidation = true;
				}
				else if (args[i].Equals("--runGameEditor", StringComparison.OrdinalIgnoreCase) || args[i].Equals("-rge", StringComparison.OrdinalIgnoreCase))
				{
					RunGameEditor = true;
				}
			}

		Cleanup:

			;
		}

		public virtual RetCode LoadPluginClassMappings()
		{
			RetCode rc;

			rc = LoadPluginClassMappings01(Assembly.GetExecutingAssembly());

			return rc;
		}

		public virtual RetCode LoadPluginClassMappings01(Assembly plugin)
		{
			RetCode rc;

			if (plugin == null)
			{
				rc = RetCode.InvalidArg;

				// PrintError

				goto Cleanup;
			}

			rc = RetCode.Success;

			var types = plugin.GetTypes().Where(t => t.GetCustomAttributes(typeof(ClassMappingsAttribute), true).FirstOrDefault() != null);

			foreach (var t in types)
			{
				var attributes = t.GetCustomAttributes(typeof(ClassMappingsAttribute), true);

				Debug.Assert(attributes != null);

				foreach (ClassMappingsAttribute a in attributes)
				{
					var ift = a.InterfaceType;

					if (ift == null)
					{
						var ifaces = t.GetInterfaces();

						ift = ifaces != null ? ifaces.FirstOrDefault(t01 => t01.Name.Equals(string.Format("I{0}", t.Name), StringComparison.Ordinal)) : null;
					}

					if (ift == null)
					{
						var errorBuf = new StringBuilder(BufSize);

						errorBuf.AppendFormat("{0}Error: Couldn't find ClassMappings interface for class [{1}].", Environment.NewLine, t.Name);

						errorBuf.AppendFormat("{0}Error: LoadPluginClassMappings01 function call failed for plugin [{1}].", Environment.NewLine, plugin.GetShortName());

						rc = RetCode.Failure;

						Error.WriteLine(errorBuf);

						goto Cleanup;
					}

					ClassMappingsDictionary[ift] = t;
				}
			}

		Cleanup:

			return rc;
		}

		public virtual RetCode PushRulesetVersion(long rulesetVersion)
		{
			RetCode rc;

			rc = RetCode.Success;

			if (RulesetVersions == null || RvStackTop + 1 >= RulesetVersions.Length)
			{
				rc = RetCode.IsFull;

				goto Cleanup;
			}

			RulesetVersions[++RvStackTop] = rulesetVersion;

		Cleanup:

			return rc;
		}

		public virtual RetCode PopRulesetVersion()
		{
			RetCode rc;

			rc = RetCode.Success;

			if (RulesetVersions == null || RvStackTop < 0)
			{
				rc = RetCode.IsEmpty;

				goto Cleanup;
			}

			RulesetVersions[RvStackTop--] = 0;

		Cleanup:

			return rc;
		}

		public virtual RetCode ClearRvStack()
		{
			RetCode rc;

			rc = RetCode.Success;

			while (RvStackTop >= 0)
			{
				rc = PopRulesetVersion();

				if (rc != RetCode.Success)
				{
					break;
				}
			}

			return rc;
		}

		public virtual RetCode GetRvStackTop(ref long rvStackTop)
		{
			RetCode rc;

			rc = RetCode.Success;

			rvStackTop = RvStackTop;

			return rc;
		}

		public virtual RetCode GetRvStackSize(ref long rvStackSize)
		{
			RetCode rc;

			rc = RetCode.Success;

			rvStackSize = RulesetVersions != null ? RulesetVersions.Length : 0;

			return rc;
		}

		public virtual T CreateInstance<T>(Type ifaceType, Action<T> initialize = null) where T : class
		{
			T result = default(T);

			if (ifaceType == null)
			{
				// PrintError

				goto Cleanup;
			}

			Type mappedType;

			if (ClassMappingsDictionary.TryGetValue(ifaceType, out mappedType))
			{
				result = Activator.CreateInstance(mappedType) as T;

				if (result != null && initialize != null)
				{
					initialize(result);
				}
			}

		Cleanup:

			return result;
		}

		public virtual T CreateInstance<T>(Action<T> initialize = null) where T : class
		{
			return CreateInstance(typeof(T), initialize);
		}

		public virtual T CloneInstance<T>(T source) where T : class
		{
			T result = default(T);

			if (source == null)
			{
				// PrintError

				goto Cleanup;
			}

			try
			{
				CloneStream.SetLength(0);

				SharpSerializer.Serialize(source, CloneStream, true);

				CloneStream.Position = 0;

				result = SharpSerializer.Deserialize<T>(CloneStream, true);
			}
			catch (Exception)
			{
				throw;
			}
			finally
			{
				var igb = result as IGameBase;

				if (igb != null)
				{
					igb.SetParentReferences();
				}
			}

		Cleanup:

			return result;
		}

		public virtual bool CompareInstances<T>(T object1, T object2) where T : class
		{
			var result = false;

			if (object1 == null || object2 == null)
			{
				// PrintError

				goto Cleanup;
			}

			byte[] object1Bytes = null;

			byte[] object2Bytes = null;

			CloneStream.SetLength(0);

			SharpSerializer.Serialize(object1, CloneStream, true);

			object1Bytes = CloneStream.ToArray();

			Debug.Assert(object1Bytes != null);

			CloneStream.SetLength(0);

			SharpSerializer.Serialize(object2, CloneStream, true);

			object2Bytes = CloneStream.ToArray();

			Debug.Assert(object2Bytes != null);

			result = object1Bytes.SequenceEqual(object2Bytes);

		Cleanup:

			return result;
		}

		public virtual bool IsRulesetVersion(params long[] versions)
		{
			var result = false;

			if (versions == null)
			{
				// PrintError

				goto Cleanup;
			}

			result = versions.Contains(RulesetVersion);

		Cleanup:

			return result;
		}

		public virtual string GetPrefixedFileName(string fileName)
		{
			string result = null;

			if (string.IsNullOrWhiteSpace(fileName))
			{
				// PrintError

				goto Cleanup;
			}

			var path = Path.GetDirectoryName(fileName);

			var fileName01 = Path.GetFileName(fileName);

			result = Path.Combine(path, string.Format("{0}{1}", !string.IsNullOrWhiteSpace(FilePrefix) ? FilePrefix + "_" : "", fileName01));

		Cleanup:

			return result;
		}

		public virtual void ConvertDatafileFromXmlToDat(string fileName)
		{
			if (string.IsNullOrWhiteSpace(fileName))
			{
				// PrintError

				goto Cleanup;
			}

			var xmlFileName = Path.ChangeExtension(fileName, ".XML");

			if (File.Exists(xmlFileName))
			{
				var contents = File.ReadAllText(xmlFileName);

				File.WriteAllText(fileName, contents);

				File.Delete(xmlFileName);
			}

		Cleanup:

			;
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

			Databases[++DbStackTop] = CreateInstance<IDatabase>();

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

			UpgradeDatafile(fileName);

			var database = SharpSerializer.Deserialize<IDatabase>(fileName);

			if (database == null)
			{
				rc = RetCode.Failure;

				goto Cleanup;
			}

			RestoreRecords(database.ConfigTable?.Records?.Cast<IGameBase>().ToList());

			RestoreRecords(database.FilesetTable?.Records?.Cast<IGameBase>().ToList());

			RestoreRecords(database.CharacterTable?.Records?.Cast<IGameBase>().ToList());

			RestoreRecords(database.ModuleTable?.Records?.Cast<IGameBase>().ToList());

			RestoreRecords(database.RoomTable?.Records?.Cast<IGameBase>().ToList());

			RestoreRecords(database.ArtifactTable?.Records?.Cast<IGameBase>().ToList());

			RestoreRecords(database.EffectTable?.Records?.Cast<IGameBase>().ToList());

			RestoreRecords(database.MonsterTable?.Records?.Cast<IGameBase>().ToList());

			RestoreRecords(database.HintTable?.Records?.Cast<IGameBase>().ToList());

			RestoreRecords(database.GameStateTable?.Records?.Cast<IGameBase>().ToList());

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
			if (!IgnoreMutex)
			{
				Mutex.CreateAndWaitOne();
			}

			RevealContentArtifactList = new List<IArtifact>();

			RevealContentFuncList = new List<Action>();

			RevealContentCounter = 1;

			Databases = new IDatabase[NumDatabases];

			DbStackTop = -1;

			PushDatabase();

			CFGDB = CreateInstance<IRecordDb<IConfig>>();

			FSDB = CreateInstance<IRecordDb<IFileset>>();

			CHRDB = CreateInstance<IRecordDb<ICharacter>>();

			MODDB = CreateInstance<IRecordDb<IModule>>();

			RDB = CreateInstance<IRecordDb<IRoom>>();

			ADB = CreateInstance<IRecordDb<IArtifact>>();

			EDB = CreateInstance<IRecordDb<IEffect>>();

			MDB = CreateInstance<IRecordDb<IMonster>>();

			HDB = CreateInstance<IRecordDb<IHint>>();

			GSDB = CreateInstance<IRecordDb<IGameState>>();

			Preps = new IPrep[]
			{
				CreateInstance<IPrep>(x =>
				{
					x.Name = "in";
					x.ContainerType = ContainerType.In;
				}),
				CreateInstance<IPrep>(x =>
				{
					x.Name = "into";
					x.ContainerType = ContainerType.In;
				}),
				CreateInstance<IPrep>(x =>
				{
					x.Name = "fromin";
					x.ContainerType = ContainerType.In;
				}),
				CreateInstance<IPrep>(x =>
				{
					x.Name = "on";
					x.ContainerType = ContainerType.On;
				}),
				CreateInstance<IPrep>(x =>
				{
					x.Name = "onto";
					x.ContainerType = ContainerType.On;
				}),
				CreateInstance<IPrep>(x =>
				{
					x.Name = "fromon";
					x.ContainerType = ContainerType.On;
				}),
				CreateInstance<IPrep>(x =>
				{
					x.Name = "under";
					x.ContainerType = ContainerType.Under;
				}),
				CreateInstance<IPrep>(x =>
				{
					x.Name = "fromunder";
					x.ContainerType = ContainerType.Under;
				}),
				CreateInstance<IPrep>(x =>
				{
					x.Name = "behind";
					x.ContainerType = ContainerType.Behind;
				}),
				CreateInstance<IPrep>(x =>
				{
					x.Name = "frombehind";
					x.ContainerType = ContainerType.Behind;
				}),
				CreateInstance<IPrep>(x =>
				{
					x.Name = "to";
					x.ContainerType = (ContainerType)(-1);
				}),
				CreateInstance<IPrep>(x =>
				{
					x.Name = "at";
					x.ContainerType = (ContainerType)(-1);
				}),
				CreateInstance<IPrep>(x =>
				{
					x.Name = "from";
					x.ContainerType = (ContainerType)(-1);
				}),
				CreateInstance<IPrep>(x =>
				{
					x.Name = "with";
					x.ContainerType = (ContainerType)(-1);
				}),
				CreateInstance<IPrep>(x =>
				{
					x.Name = "through";
					x.ContainerType = (ContainerType)(-1);
				}),
				CreateInstance<IPrep>(x =>
				{
					x.Name = "along";
					x.ContainerType = (ContainerType)(-1);
				}),
				CreateInstance<IPrep>(x =>
				{
					x.Name = "across";
					x.ContainerType = (ContainerType)(-1);
				})
			};

			Stats = new IStat[]			// TODO: fix
			{
				CreateInstance<IStat>(x =>
				{
					x.Name = "Intellect";
					x.Abbr = "IN";
					x.EmptyVal = "14";
					x.MinValue = 1;
					x.MaxValue = 24;
				}),
				CreateInstance<IStat>(x =>
				{
					x.Name = "Hardiness";
					x.Abbr = "HD";
					x.EmptyVal = "16";
					x.MinValue = 1;
					x.MaxValue = 300;
				}),
				CreateInstance<IStat>(x =>
				{
					x.Name = "Agility";
					x.Abbr = "AG";
					x.EmptyVal = "18";
					x.MinValue = 1;
					x.MaxValue = 30;
				}),
				CreateInstance<IStat>(x =>
				{
					x.Name = "Charisma";
					x.Abbr = "CH";
					x.EmptyVal = "16";
					x.MinValue = 1;
					x.MaxValue = 24;
				})
			};

			Spells = new ISpell[]			// TODO: fix
			{
				CreateInstance<ISpell>(x =>
				{
					x.Name = "Blast";
					x.HokasName = null;
					x.HokasPrice = BlastPrice;
					x.MinValue = 0;
					x.MaxValue = 500;
				}),
				CreateInstance<ISpell>(x =>
				{
					x.Name = "Heal";
					x.HokasName = null;
					x.HokasPrice = HealPrice;
					x.MinValue = 0;
					x.MaxValue = 500;
				}),
				CreateInstance<ISpell>(x =>
				{
					x.Name = "Speed";
					x.HokasName = null;
					x.HokasPrice = SpeedPrice;
					x.MinValue = 0;
					x.MaxValue = 500;
				}),
				CreateInstance<ISpell>(x =>
				{
					x.Name = "Power";
					x.HokasName = null;
					x.HokasPrice = PowerPrice;
					x.MinValue = 0;
					x.MaxValue = 500;
				})
			};

			Weapons = new IWeapon[]			// TODO: fix
			{
				CreateInstance<IWeapon>(x =>
				{
					x.Name = "Axe";
					x.EmptyVal = "5";
					x.MarcosName = null;
					x.MarcosIsPlural = false;
					x.MarcosPluralType = PluralType.S;
					x.MarcosArticleType = ArticleType.An;
					x.MarcosPrice = AxePrice;
					x.MarcosDice = 1;
					x.MarcosSides = 6;
					x.MarcosNumHands = 1;
					x.MinValue = -50;
					x.MaxValue = 122;
				}),
				CreateInstance<IWeapon>(x =>
				{
					x.Name = "Bow";
					x.EmptyVal = "-10";
					x.MarcosName = null;
					x.MarcosIsPlural = false;
					x.MarcosPluralType = PluralType.S;
					x.MarcosArticleType = ArticleType.A;
					x.MarcosPrice = BowPrice;
					x.MarcosDice = 1;
					x.MarcosSides = 6;
					x.MarcosNumHands = 2;
					x.MinValue = -50;
					x.MaxValue = 122;
				}),
				CreateInstance<IWeapon>(x =>
				{
					x.Name = "Club";
					x.EmptyVal = "20";
					x.MarcosName = "Mace";
					x.MarcosIsPlural = false;
					x.MarcosPluralType = PluralType.S;
					x.MarcosArticleType = ArticleType.A;
					x.MarcosPrice = MacePrice;
					x.MarcosDice = 1;
					x.MarcosSides = 4;
					x.MarcosNumHands = 1;
					x.MinValue = -50;
					x.MaxValue = 122;
				}),
				CreateInstance<IWeapon>(x =>
				{
					x.Name = "Spear";
					x.EmptyVal = "10";
					x.MarcosName = null;
					x.MarcosIsPlural = false;
					x.MarcosPluralType = PluralType.S;
					x.MarcosArticleType = ArticleType.A;
					x.MarcosPrice = SpearPrice;
					x.MarcosDice = 1;
					x.MarcosSides = 5;
					x.MarcosNumHands = 1;
					x.MinValue = -50;
					x.MaxValue = 122;
				}),
				CreateInstance<IWeapon>(x =>
				{
					x.Name = "Sword";
					x.EmptyVal = "0";
					x.MarcosName = null;
					x.MarcosIsPlural = false;
					x.MarcosPluralType = PluralType.S;
					x.MarcosArticleType = ArticleType.A;
					x.MarcosPrice = SwordPrice;
					x.MarcosDice = 1;
					x.MarcosSides = 8;
					x.MarcosNumHands = 1;
					x.MinValue = -50;
					x.MaxValue = 122;
				})
			};

			Armors = new IArmor[]
			{
				CreateInstance<IArmor>(x =>
				{
					x.Name = "Skin/Clothes";
					x.MarcosName = null;
					x.MarcosPrice = 0;
					x.MarcosNum = 0;
					x.ArtifactValue = 0;
				}),
				CreateInstance<IArmor>(x =>
				{
					x.Name = "Clothes & Shield";
					x.MarcosName = null;
					x.MarcosPrice = 0;
					x.MarcosNum = 0;
					x.ArtifactValue = 0;
				}),
				CreateInstance<IArmor>(x =>
				{
					x.Name = "Leather";
					x.MarcosName = "Leather Armor";
					x.MarcosPrice = LeatherArmorPrice;
					x.MarcosNum = 1;
					x.ArtifactValue = 100;
				}),
				CreateInstance<IArmor>(x =>
				{
					x.Name = "Leather & Shield";
					x.MarcosName = null;
					x.MarcosPrice = 0;
					x.MarcosNum = 0;
					x.ArtifactValue = 0;
				}),
				CreateInstance<IArmor>(x =>
				{
					x.Name = "Chain Mail";
					x.MarcosName = null;
					x.MarcosPrice = ChainMailPrice;
					x.MarcosNum = 2;
					x.ArtifactValue = 200;
				}),
				CreateInstance<IArmor>(x =>
				{
					x.Name = "Chain Mail & Shield";
					x.MarcosName = null;
					x.MarcosPrice = 0;
					x.MarcosNum = 0;
					x.ArtifactValue = 0;
				}),
				CreateInstance<IArmor>(x =>
				{
					x.Name = "Plate Mail";
					x.MarcosName = null;
					x.MarcosPrice = PlateMailPrice;
					x.MarcosNum = 3;
					x.ArtifactValue = 350;
				}),
				CreateInstance<IArmor>(x =>
				{
					x.Name = "Plate Mail & Shield";
					x.MarcosName = null;
					x.MarcosPrice = 0;
					x.MarcosNum = 0;
					x.ArtifactValue = 0;
				}),
				CreateInstance<IArmor>(x =>
				{
					x.Name = "Magic/Exotic";
					x.MarcosName = null;
					x.MarcosPrice = 0;
					x.MarcosNum = 0;
					x.ArtifactValue = 500;
				}),
				CreateInstance<IArmor>(x =>
				{
					x.Name = "Magic/Exotic & Shield";
					x.MarcosName = null;
					x.MarcosPrice = 0;
					x.MarcosNum = 0;
					x.ArtifactValue = 0;
				}),
				CreateInstance<IArmor>(x =>
				{
					x.Name = "Magic/Exotic";
					x.MarcosName = null;
					x.MarcosPrice = 0;
					x.MarcosNum = 0;
					x.ArtifactValue = 650;
				}),
				CreateInstance<IArmor>(x =>
				{
					x.Name = "Magic/Exotic & Shield";
					x.MarcosName = null;
					x.MarcosPrice = 0;
					x.MarcosNum = 0;
					x.ArtifactValue = 0;
				}),
				CreateInstance<IArmor>(x =>
				{
					x.Name = "Magic/Exotic";
					x.MarcosName = null;
					x.MarcosPrice = 0;
					x.MarcosNum = 0;
					x.ArtifactValue = 800;
				}),
				CreateInstance<IArmor>(x =>
				{
					x.Name = "Magic/Exotic & Shield";
					x.MarcosName = null;
					x.MarcosPrice = 0;
					x.MarcosNum = 0;
					x.ArtifactValue = 0;
				}),
				CreateInstance<IArmor>(x =>
				{
					x.Name = "Magic/Exotic";
					x.MarcosName = null;
					x.MarcosPrice = 0;
					x.MarcosNum = 0;
					x.ArtifactValue = 950;
				}),
				CreateInstance<IArmor>(x =>
				{
					x.Name = "Magic/Exotic & Shield";
					x.MarcosName = null;
					x.MarcosPrice = 0;
					x.MarcosNum = 0;
					x.ArtifactValue = 0;
				})
			};

			Directions = new IDirection[]			// TODO: fix
			{
				CreateInstance<IDirection>(x =>
				{
					x.Name = "North";
					x.PrintedName = "North";
					x.Abbr = "N";
					x.EnterDir = Direction.South;
				}),
				CreateInstance<IDirection>(x =>
				{
					x.Name = "South";
					x.PrintedName = "South";
					x.Abbr = "S";
					x.EnterDir = Direction.North;
				}),
				CreateInstance<IDirection>(x =>
				{
					x.Name = "East";
					x.PrintedName = "East";
					x.Abbr = "E";
					x.EnterDir = Direction.West;
				}),
				CreateInstance<IDirection>(x =>
				{
					x.Name = "West";
					x.PrintedName = "West";
					x.Abbr = "W";
					x.EnterDir = Direction.East;
				}),
				CreateInstance<IDirection>(x =>
				{
					x.Name = "Up";
					x.PrintedName = "Up";
					x.Abbr = "U";
					x.EnterDir = Direction.Down;
				}),
				CreateInstance<IDirection>(x =>
				{
					x.Name = "Down";
					x.PrintedName = "Down";
					x.Abbr = "D";
					x.EnterDir = Direction.Up;
				}),
				CreateInstance<IDirection>(x =>
				{
					x.Name = "Northeast";
					x.PrintedName = "NE";
					x.Abbr = "NE";
					x.EnterDir = Direction.Southwest;
				}),
				CreateInstance<IDirection>(x =>
				{
					x.Name = "Northwest";
					x.PrintedName = "NW";
					x.Abbr = "NW";
					x.EnterDir = Direction.Southeast;
				}),
				CreateInstance<IDirection>(x =>
				{
					x.Name = "Southeast";
					x.PrintedName = "SE";
					x.Abbr = "SE";
					x.EnterDir = Direction.Northwest;
				}),
				CreateInstance<IDirection>(x =>
				{
					x.Name = "Southwest";
					x.PrintedName = "SW";
					x.Abbr = "SW";
					x.EnterDir = Direction.Northeast;
				}),
				CreateInstance<IDirection>(x =>
				{
					x.Name = "In";
					x.PrintedName = "In";
					x.Abbr = "I";
					x.EnterDir = Direction.Out;
				}),
				CreateInstance<IDirection>(x =>
				{
					x.Name = "Out";
					x.PrintedName = "Out";
					x.Abbr = "O";
					x.EnterDir = Direction.In;
				})
			};

			ArtifactTypes = new IArtifactType[]
			{
				CreateInstance<IArtifactType>(x =>
				{
					x.Name = "Gold";
					x.WeightEmptyVal = "15";
					x.LocationEmptyVal = "0";
					x.Field1Name = "Field1";
					x.Field1EmptyVal = "0";
					x.Field2Name = "Field2";
					x.Field2EmptyVal = "0";
					x.Field3Name = "Field3";
					x.Field3EmptyVal = "0";
					x.Field4Name = "Field4";
					x.Field4EmptyVal = "0";
					x.Field5Name = "Field5";
					x.Field5EmptyVal = "0";
				}),
				CreateInstance<IArtifactType>(x =>
				{
					x.Name = "Treasure";
					x.WeightEmptyVal = "15";
					x.LocationEmptyVal = "0";
					x.Field1Name = "Field1";
					x.Field1EmptyVal = "0";
					x.Field2Name = "Field2";
					x.Field2EmptyVal = "0";
					x.Field3Name = "Field3";
					x.Field3EmptyVal = "0";
					x.Field4Name = "Field4";
					x.Field4EmptyVal = "0";
					x.Field5Name = "Field5";
					x.Field5EmptyVal = "0";
				}),
				CreateInstance<IArtifactType>(x =>
				{
					x.Name = "Weapon";
					x.WeightEmptyVal = "25";
					x.LocationEmptyVal = "0";
					x.Field1Name = "Complexity";
					x.Field1EmptyVal = "5";
					x.Field2Name = "Wpn Type";
					x.Field2EmptyVal = "5";
					x.Field3Name = "Dice";
					x.Field3EmptyVal = "1";
					x.Field4Name = "Sides";
					x.Field4EmptyVal = "6";
					x.Field5Name = "Number Of Hands";
					x.Field5EmptyVal = "1";
				}),
				CreateInstance<IArtifactType>(x =>
				{
					x.Name = "Magic Weapon";
					x.WeightEmptyVal = "25";
					x.LocationEmptyVal = "0";
					x.Field1Name = "Complexity";
					x.Field1EmptyVal = "15";
					x.Field2Name = "Wpn Type";
					x.Field2EmptyVal = "5";
					x.Field3Name = "Dice";
					x.Field3EmptyVal = "1";
					x.Field4Name = "Sides";
					x.Field4EmptyVal = "10";
					x.Field5Name = "Number Of Hands";
					x.Field5EmptyVal = "1";
				}),
				CreateInstance<IArtifactType>(x =>
				{
					x.Name = "In Container";
					x.WeightEmptyVal = "15";
					x.LocationEmptyVal = "0";
					x.Field1Name = "Key Uid";
					x.Field1EmptyVal = "0";
					x.Field2Name = "Open/Closed";
					x.Field2EmptyVal = "0";
					x.Field3Name = "Max Weight Inside";
					x.Field3EmptyVal = "0";
					x.Field4Name = "Max Items Inside";
					x.Field4EmptyVal = "0";
					x.Field5Name = "Display Code";
					x.Field5EmptyVal = "0";
				}),
				CreateInstance<IArtifactType>(x =>
				{
					x.Name = "On Container";
					x.WeightEmptyVal = "15";
					x.LocationEmptyVal = "0";
					x.Field1Name = "Field1";
					x.Field1EmptyVal = "0";
					x.Field2Name = "Field2";
					x.Field2EmptyVal = "0";
					x.Field3Name = "Max Weight On";
					x.Field3EmptyVal = "0";
					x.Field4Name = "Max Items On";
					x.Field4EmptyVal = "0";
					x.Field5Name = "Display Code";
					x.Field5EmptyVal = "0";
				}),
				CreateInstance<IArtifactType>(x =>
				{
					x.Name = "Under Container";
					x.WeightEmptyVal = "15";
					x.LocationEmptyVal = "0";
					x.Field1Name = "Field1";
					x.Field1EmptyVal = "0";
					x.Field2Name = "Field2";
					x.Field2EmptyVal = "0";
					x.Field3Name = "Max Weight Under";
					x.Field3EmptyVal = "0";
					x.Field4Name = "Max Items Under";
					x.Field4EmptyVal = "0";
					x.Field5Name = "Display Code";
					x.Field5EmptyVal = "0";
				}),
				CreateInstance<IArtifactType>(x =>
				{
					x.Name = "Behind Container";
					x.WeightEmptyVal = "15";
					x.LocationEmptyVal = "0";
					x.Field1Name = "Field1";
					x.Field1EmptyVal = "0";
					x.Field2Name = "Field2";
					x.Field2EmptyVal = "0";
					x.Field3Name = "Max Weight Behind";
					x.Field3EmptyVal = "0";
					x.Field4Name = "Max Items Behind";
					x.Field4EmptyVal = "0";
					x.Field5Name = "Display Code";
					x.Field5EmptyVal = "0";
				}),
				CreateInstance<IArtifactType>(x =>
				{
					x.Name = "Light Source";
					x.WeightEmptyVal = "10";
					x.LocationEmptyVal = "0";
					x.Field1Name = "Counter";
					x.Field1EmptyVal = "999";
					x.Field2Name = "Field2";
					x.Field2EmptyVal = "0";
					x.Field3Name = "Field3";
					x.Field3EmptyVal = "0";
					x.Field4Name = "Field4";
					x.Field4EmptyVal = "0";
					x.Field5Name = "Field5";
					x.Field5EmptyVal = "0";
				}),
				CreateInstance<IArtifactType>(x =>
				{
					x.Name = "Drinkable";
					x.WeightEmptyVal = "10";
					x.LocationEmptyVal = "0";
					x.Field1Name = "Heal Amount";
					x.Field1EmptyVal = "10";
					x.Field2Name = "Number Of Uses";
					x.Field2EmptyVal = "3";
					x.Field3Name = "Open/Closed";
					x.Field3EmptyVal = "1";
					x.Field4Name = "Field4";
					x.Field4EmptyVal = "0";
					x.Field5Name = "Field5";
					x.Field5EmptyVal = "0";
				}),
				CreateInstance<IArtifactType>(x =>
				{
					x.Name = "Readable";
					x.WeightEmptyVal = "15";
					x.LocationEmptyVal = "0";
					x.Field1Name = "Effect #1";
					x.Field1EmptyVal = "1";
					x.Field2Name = "Number Of Effects";
					x.Field2EmptyVal = "1";
					x.Field3Name = "Open/Closed";
					x.Field3EmptyVal = "1";
					x.Field4Name = "Field4";
					x.Field4EmptyVal = "0";
					x.Field5Name = "Field5";
					x.Field5EmptyVal = "0";
				}),
				CreateInstance<IArtifactType>(x =>
				{
					x.Name = "Door/Gate";
					x.WeightEmptyVal = "-999";
					x.LocationEmptyVal = "0";
					x.Field1Name = "Room Uid Beyond";
					x.Field1EmptyVal = "1";
					x.Field2Name = "Key Uid";
					x.Field2EmptyVal = "0";
					x.Field3Name = "Open/Closed";
					x.Field3EmptyVal = "1";
					x.Field4Name = "Hidden";
					x.Field4EmptyVal = "0";
					x.Field5Name = "Field5";
					x.Field5EmptyVal = "0";
				}),
				CreateInstance<IArtifactType>(x =>
				{
					x.Name = "Edible";
					x.WeightEmptyVal = "10";
					x.LocationEmptyVal = "0";
					x.Field1Name = "Heal Amount";
					x.Field1EmptyVal = "10";
					x.Field2Name = "Number Of Uses";
					x.Field2EmptyVal = "4";
					x.Field3Name = "Open/Closed";
					x.Field3EmptyVal = "1";
					x.Field4Name = "Field4";
					x.Field4EmptyVal = "0";
					x.Field5Name = "Field5";
					x.Field5EmptyVal = "0";
				}),
				CreateInstance<IArtifactType>(x =>
				{
					x.Name = "Bound Monster";
					x.WeightEmptyVal = "999";
					x.LocationEmptyVal = "0";
					x.Field1Name = "Monster Uid";
					x.Field1EmptyVal = "1";
					x.Field2Name = "Key Uid";
					x.Field2EmptyVal = "0";
					x.Field3Name = "Guard Uid";
					x.Field3EmptyVal = "0";
					x.Field4Name = "Field4";
					x.Field4EmptyVal = "0";
					x.Field5Name = "Field5";
					x.Field5EmptyVal = "0";
				}),
				CreateInstance<IArtifactType>(x =>
				{
					x.Name = "Wearable";
					x.WeightEmptyVal = "15";
					x.LocationEmptyVal = "0";
					x.Field1Name = "Armor Class";
					x.Field1EmptyVal = "0";
					x.Field2Name = "Clothing Type";
					x.Field2EmptyVal = "0";
					x.Field3Name = "Field3";
					x.Field3EmptyVal = "0";
					x.Field4Name = "Field4";
					x.Field4EmptyVal = "0";
					x.Field5Name = "Field5";
					x.Field5EmptyVal = "0";
				}),
				CreateInstance<IArtifactType>(x =>
				{
					x.Name = "Disguised Monster";
					x.WeightEmptyVal = "15";
					x.LocationEmptyVal = "0";
					x.Field1Name = "Monster Uid";
					x.Field1EmptyVal = "1";
					x.Field2Name = "Effect #1";
					x.Field2EmptyVal = "1";
					x.Field3Name = "Number Of Effects";
					x.Field3EmptyVal = "1";
					x.Field4Name = "Field4";
					x.Field4EmptyVal = "0";
					x.Field5Name = "Field5";
					x.Field5EmptyVal = "0";
				}),
				CreateInstance<IArtifactType>(x =>
				{
					x.Name = "Dead Body";
					x.WeightEmptyVal = "150";
					x.LocationEmptyVal = "0";
					x.Field1Name = "Can Take";
					x.Field1EmptyVal = "0";
					x.Field2Name = "Field2";
					x.Field2EmptyVal = "0";
					x.Field3Name = "Field3";
					x.Field3EmptyVal = "0";
					x.Field4Name = "Field4";
					x.Field4EmptyVal = "0";
					x.Field5Name = "Field5";
					x.Field5EmptyVal = "0";
				}),
				CreateInstance<IArtifactType>(x =>
				{
					x.Name = "User Defined #1";
					x.WeightEmptyVal = "15";
					x.LocationEmptyVal = "0";
					x.Field1Name = "Field1";
					x.Field1EmptyVal = "0";
					x.Field2Name = "Field2";
					x.Field2EmptyVal = "0";
					x.Field3Name = "Field3";
					x.Field3EmptyVal = "0";
					x.Field4Name = "Field4";
					x.Field4EmptyVal = "0";
					x.Field5Name = "Field5";
					x.Field5EmptyVal = "0";
				}),
				CreateInstance<IArtifactType>(x =>
				{
					x.Name = "User Defined #2";
					x.WeightEmptyVal = "15";
					x.LocationEmptyVal = "0";
					x.Field1Name = "Field1";
					x.Field1EmptyVal = "0";
					x.Field2Name = "Field2";
					x.Field2EmptyVal = "0";
					x.Field3Name = "Field3";
					x.Field3EmptyVal = "0";
					x.Field4Name = "Field4";
					x.Field4EmptyVal = "0";
					x.Field5Name = "Field5";
					x.Field5EmptyVal = "0";
				}),
				CreateInstance<IArtifactType>(x =>
				{
					x.Name = "User Defined #3";
					x.WeightEmptyVal = "15";
					x.LocationEmptyVal = "0";
					x.Field1Name = "Field1";
					x.Field1EmptyVal = "0";
					x.Field2Name = "Field2";
					x.Field2EmptyVal = "0";
					x.Field3Name = "Field3";
					x.Field3EmptyVal = "0";
					x.Field4Name = "Field4";
					x.Field4EmptyVal = "0";
					x.Field5Name = "Field5";
					x.Field5EmptyVal = "0";
				})
			};
		}

		public virtual void DeinitSystem()
		{
			ClearDbStack();
		}

		public virtual void ResetProperties(PropertyResetCode resetCode)
		{
			switch (resetCode)
			{
				case PropertyResetCode.All:
				case PropertyResetCode.RestoreGame:
				case PropertyResetCode.SwitchContext:

					RevealContentRoom = null;

					RevealContentMonster = null;

					RevealContentArtifactList.Clear();

					RevealContentFuncList.Clear();

					break;

				case PropertyResetCode.RevealContainerContents:

					RevealContentArtifactList.Clear();

					RevealContentFuncList.Clear();

					break;
			}
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
				if (Regex.IsMatch(contents, CoreLibRegexPattern))
				{
					contents = Regex.Replace(contents, CoreLibRegexPattern, MscorlibName);

					contentsModified = true;
				}
			}
			else
			{
				if (Regex.IsMatch(contents, MscorlibRegexPattern))
				{
					contents = Regex.Replace(contents, MscorlibRegexPattern, CoreLibName);

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
				else if (firstLine.Contains("Version=1.8.0.0"))
				{
					if (firstLine.Contains("Game.DataStorage.MonsterDbTable") || firstLine.Contains("Game.DataStorage.Database"))
					{
						if (workDir.EndsWith(@"\Adventures\AlternateBeginnersCave"))
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
									"AlternateBeginnersCave.Game.Monster, AlternateBeginnersCave",
								}
							);
						}

						if (workDir.EndsWith(@"\Adventures\BeginnersCaveII"))
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
									"BeginnersCaveII.Game.Monster, BeginnersCaveII",
								}
							);
						}

						if (workDir.EndsWith(@"\Adventures\LandOfTheMountainKing"))
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
									"LandOfTheMountainKing.Game.Monster, LandOfTheMountainKing",
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
								@"Version=1\.8\.0\.0",
								@"EAMON CS 1\.8",
							},
							new string[]
							{
								"Version=2.1.0.0",
								"EAMON CS 2.1",
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

		public virtual IPrep GetPrep(long index)
		{
			return Preps[index];
		}

		public virtual string GetArticle(long index)
		{
			return Articles[index];
		}

		public virtual string GetNumberString(long index)
		{
			return NumberStrings[index];
		}

		public virtual string GetFieldDescName(long index)
		{
			return FieldDescNames[index];
		}

		public virtual string GetFieldDescName(FieldDesc fieldDesc)
		{
			return Enum.IsDefined(typeof(FieldDesc), fieldDesc) ? GetFieldDescName((long)fieldDesc) : UnknownName;
		}

		public virtual string GetStatusName(long index)
		{
			return StatusNames[index];
		}

		public virtual string GetStatusName(Status status)
		{
			return Enum.IsDefined(typeof(Status), status) ? GetStatusName((long)status) : UnknownName;
		}

		public virtual string GetClothingName(long index)
		{
			return ClothingNames[index];
		}

		public virtual string GetClothingName(Clothing clothing)
		{
			return Enum.IsDefined(typeof(Clothing), clothing) ? GetClothingName((long)clothing) : UnknownName;
		}

		public virtual string GetCombatCodeDesc(long index)
		{
			return CombatCodeDescs[index];
		}

		public virtual string GetCombatCodeDesc(CombatCode combatCode)
		{
			return Enum.IsDefined(typeof(CombatCode), combatCode) ? GetCombatCodeDesc((long)combatCode + 2) : UnknownName;
		}

		public virtual string GetContainerDisplayCodeDesc(long index)
		{
			return ContainerDisplayCodeDescs[index];
		}

		public virtual string GetContainerDisplayCodeDesc(ContainerDisplayCode containerDisplayCode)
		{
			return Enum.IsDefined(typeof(ContainerDisplayCode), containerDisplayCode) ? GetContainerDisplayCodeDesc((long)containerDisplayCode) : UnknownName;
		}

		public virtual string GetLightLevelName(long index)
		{
			return LightLevelNames[index];
		}

		public virtual string GetLightLevelName(LightLevel lightLevel)
		{
			return Enum.IsDefined(typeof(LightLevel), lightLevel) ? GetLightLevelName((long)lightLevel) : UnknownName;
		}

		public virtual IStat GetStat(long index)
		{
			return Stats[index];
		}

		public virtual IStat GetStat(Stat stat)
		{
			return Enum.IsDefined(typeof(Stat), stat) ? GetStat((long)stat - 1) : null;
		}

		public virtual ISpell GetSpell(long index)
		{
			return Spells[index];
		}

		public virtual ISpell GetSpell(Spell spell)
		{
			return Enum.IsDefined(typeof(Spell), spell) ? GetSpell((long)spell - 1) : null;
		}

		public virtual IWeapon GetWeapon(long index)
		{
			return Weapons[index];
		}

		public virtual IWeapon GetWeapon(Weapon weapon)
		{
			return Enum.IsDefined(typeof(Weapon), weapon) ? GetWeapon((long)weapon - 1) : null;
		}

		public virtual IArmor GetArmor(long index)
		{
			return Armors[index];
		}

		public virtual IArmor GetArmor(Armor armor)
		{
			return Enum.IsDefined(typeof(Armor), armor) ? GetArmor((long)armor) : null;
		}

		public virtual IDirection GetDirection(long index)
		{
			return Directions[index];
		}

		public virtual IDirection GetDirection(Direction direction)
		{
			return Enum.IsDefined(typeof(Direction), direction) ? GetDirection((long)direction - 1) : null;
		}

		public virtual IArtifactType GetArtifactType(long index)
		{
			return ArtifactTypes[index];
		}

		public virtual IArtifactType GetArtifactType(ArtifactType artifactType)
		{
			return IsValidArtifactType(artifactType) ? GetArtifactType((long)artifactType) : null;
		}

		public virtual bool IsSuccess(RetCode rc)
		{
			return (long)rc >= (long)RetCode.Success;
		}

		public virtual bool IsFailure(RetCode rc)
		{
			return !IsSuccess(rc);
		}

		public virtual bool IsValidPluralType(PluralType pluralType)
		{
			return Enum.IsDefined(typeof(PluralType), pluralType) || (long)pluralType > 1000;
		}

		public virtual bool IsValidArtifactType(ArtifactType artifactType)
		{
			return Enum.IsDefined(typeof(ArtifactType), artifactType) && artifactType != ArtifactType.None;
		}

		public virtual bool IsValidArtifactArmor(long armor)
		{
			return Enum.IsDefined(typeof(Armor), armor) && (armor == (long)Armor.ClothesShield || armor % 2 == 0);
		}

		public virtual bool IsValidMonsterArmor(long armor)
		{
			return armor >= 0;
		}

		public virtual bool IsValidMonsterCourage(long courage)
		{
			return courage >= 0 && courage <= 200;
		}

		public virtual bool IsValidMonsterFriendliness(Friendliness friendliness)
		{
			return Enum.IsDefined(typeof(Friendliness), friendliness) || IsValidMonsterFriendlinessPct(friendliness);
		}

		public virtual bool IsValidMonsterFriendlinessPct(Friendliness friendliness)
		{
			return (long)friendliness >= 100 && (long)friendliness <= 200;
		}

		public virtual bool IsValidDirection(Direction dir)
		{
			var module = GetModule();

			var numDirs = module != null ? module.NumDirs : 6;

			return (long)dir <= numDirs;
		}

		public virtual bool IsValidRoomUid01(long roomUid)
		{
			return roomUid != 0 && roomUid < 1001;
		}

		public virtual bool IsValidRoomDirectionDoorUid01(long roomUid)
		{
			return roomUid > 1000;
		}

		public virtual bool IsArtifactFieldStrength(long value)
		{
			return value >= 1000;
		}

		public virtual bool IsUnmovable(long weight)
		{
			return weight == -999 || weight == 999;
		}

		public virtual bool IsUnmovable01(long weight)
		{
			return weight == -999;
		}

		public virtual long GetWeightCarryableGronds(long hardiness)
		{
			return hardiness * 10;
		}

		public virtual long GetWeightCarryableDos(long hardiness)
		{
			return GetWeightCarryableGronds(hardiness) * 10;
		}

		public virtual long GetIntellectBonusPct(long intellect)
		{
			return (intellect - 13) * 2;
		}

		public virtual long GetCharmMonsterPct(long charisma)
		{
			return (charisma - 10) * 2;
		}

		public virtual long GetPluralTypeEffectUid(PluralType pluralType)
		{
			return (long)pluralType > 1000 ? (long)pluralType - 1000 : 0;
		}

		public virtual long GetArmorFactor(long armorUid, long shieldUid)
		{
			long af = 0;

			if (armorUid > 0)
			{
				var artifact = ADB[armorUid];

				Debug.Assert(artifact != null);

				var ac = artifact.Wearable;

				Debug.Assert(ac != null);

				var f = ac.Field1 / 2;

				if (f > 3)
				{
					f = 3;
				}

				af -= (10 * f);

				if (f == 3)
				{
					af -= 30;
				}
			}

			if (shieldUid > 0)
			{
				var artifact = ADB[shieldUid];

				Debug.Assert(artifact != null);

				var ac = artifact.Wearable;

				Debug.Assert(ac != null);

				af -= (5 * ac.Field1);
			}

			return af;
		}

		public virtual long GetCharismaFactor(long charisma)
		{
			var f = GetCharmMonsterPct(charisma);

			if (f > 28)
			{
				f = 28;
			}

			return f;
		}

		public virtual long GetMonsterFriendlinessPct(Friendliness friendliness)
		{
			return (long)friendliness - 100;
		}

		public virtual long GetArtifactFieldStrength(long value)
		{
			return value - 1000;
		}

		public virtual long GetMerchantAskPrice(double price, double rtio)
		{
			return (long)((price) * (rtio) + .5);
		}

		public virtual long GetMerchantBidPrice(double price, double rtio)
		{
			return (long)((price) / (rtio) + .5);
		}

		public virtual long GetMerchantAdjustedCharisma(long charisma)
		{
			var j = RollDice(1, 11, -6);

			var c2 = charisma + j;

			var stat = GetStat(Stat.Charisma);

			Debug.Assert(stat != null);

			if (c2 < stat.MinValue)
			{
				c2 = stat.MinValue;
			}
			else if (c2 > stat.MaxValue)
			{
				c2 = stat.MaxValue;
			}

			return c2;
		}

		public virtual double GetMerchantRtio(long charisma)
		{
			var stat = GetStat(Stat.Charisma);

			Debug.Assert(stat != null);

			var min = 0;

			var max = 1;

			var a = 0.70;

			var b = 1.30;

			var x = (double)((stat.MaxValue - stat.MinValue) - (charisma - stat.MinValue)) / (double)(stat.MaxValue - stat.MinValue);

			return (((b - a) * (x - min)) / (max - min)) + a;
		}

		public virtual bool IsCharYOrN(char ch)
		{
			ch = Char.ToUpper(ch);

			return ch == 'Y' || ch == 'N';
		}

		public virtual bool IsCharSOrTOrROrX(char ch)
		{
			ch = Char.ToUpper(ch);

			return ch == 'S' || ch == 'T' || ch == 'R' || ch == 'X';
		}

		public virtual bool IsChar0Or1(char ch)
		{
			return ch == '0' || ch == '1';
		}

		public virtual bool IsChar0To2(char ch)
		{
			return ch >= '0' && ch <= '2';
		}

		public virtual bool IsChar0To3(char ch)
		{
			return ch >= '0' && ch <= '3';
		}

		public virtual bool IsChar1To3(char ch)
		{
			return ch >= '1' && ch <= '3';
		}

		public virtual bool IsCharDigit(char ch)
		{
			return Char.IsDigit(ch);
		}

		public virtual bool IsCharDigitOrX(char ch)
		{
			return Char.IsDigit(ch) || Char.ToUpper(ch) == 'X';
		}

		public virtual bool IsCharPlusMinusDigit(char ch)
		{
			return ch == '+' || ch == '-' || Char.IsDigit(ch);
		}

		public virtual bool IsCharAlpha(char ch)
		{
			return Char.IsLetter(ch);
		}

		public virtual bool IsCharAlphaSpace(char ch)
		{
			return Char.IsLetter(ch) || Char.IsWhiteSpace(ch);
		}

		public virtual bool IsCharAlnum(char ch)
		{
			return Char.IsLetterOrDigit(ch);
		}

		public virtual bool IsCharAlnumSpace(char ch)
		{
			return Char.IsLetterOrDigit(ch) || Char.IsWhiteSpace(ch);
		}

		public virtual bool IsCharAlnumPeriodUnderscore(char ch)
		{
			return Char.IsLetterOrDigit(ch) || ch == '.' || ch == '_';
		}

		public virtual bool IsCharPrint(char ch)
		{
			return Char.IsControl(ch) == false;
		}

		public virtual bool IsCharPound(char ch)
		{
			return ch == '#';
		}

		public virtual bool IsCharQuote(char ch)
		{
			return ch == '\'' || ch == '`' || ch == '"';
		}

		public virtual bool IsCharAny(char ch)
		{
			return true;
		}

		public virtual bool IsCharAnyButDquoteCommaColon(char ch)
		{
			return ch != '"' && ch != ',' && ch != ':';
		}

		public virtual bool IsCharAnyButBackForwardSlash(char ch)
		{
			return ch != '\\' && ch != '/';
		}

		public virtual char ModifyCharToUpper(char ch)
		{
			return Char.ToUpper(ch);
		}

		public virtual char ModifyCharToNullOrX(char ch)
		{
			return Char.ToUpper(ch) == 'X' ? 'X' : '\0';
		}

		public virtual char ModifyCharToNull(char ch)
		{
			return '\0';
		}

		public virtual Direction GetDirection(string directionName)
		{
			Direction result = 0;

			Debug.Assert(!string.IsNullOrWhiteSpace(directionName));

			var module = GetModule();

			var numDirs = module != null ? module.NumDirs : 6;

			var directionValues = EnumUtil.GetValues<Direction>();

			for (var i = 0; i < numDirs; i++)
			{
				if (GetDirection(i).PrintedName.Equals(directionName, StringComparison.OrdinalIgnoreCase))
				{
					result = directionValues[i];

					break;
				}
			}

			if (result == 0)
			{
				for (var i = 0; i < numDirs; i++)
				{
					if (GetDirection(i).Name.Equals(directionName, StringComparison.OrdinalIgnoreCase))
					{
						result = directionValues[i];

						break;
					}
				}
			}

			if (result == 0)
			{
				for (var i = 0; i < numDirs; i++)
				{
					if (GetDirection(i).PrintedName.StartsWith(directionName, StringComparison.OrdinalIgnoreCase) || GetDirection(i).PrintedName.EndsWith(directionName, StringComparison.OrdinalIgnoreCase))
					{
						result = directionValues[i];

						break;
					}
				}
			}

			if (result == 0)
			{
				for (var i = 0; i < numDirs; i++)
				{
					if (GetDirection(i).Name.StartsWith(directionName, StringComparison.OrdinalIgnoreCase) || GetDirection(i).Name.EndsWith(directionName, StringComparison.OrdinalIgnoreCase))
					{
						result = directionValues[i];

						break;
					}
				}
			}

			return result;
		}

		public virtual ContainerType GetContainerType(ArtifactType artifactType)
		{
			return artifactType == ArtifactType.InContainer ? ContainerType.In :
						artifactType == ArtifactType.OnContainer ? ContainerType.On :
						artifactType == ArtifactType.UnderContainer ? ContainerType.Under :
						artifactType == ArtifactType.BehindContainer ? ContainerType.Behind :
						(ContainerType)(-1);
		}

		public virtual IConfig GetConfig()
		{
			return Database?.ConfigTable?.Records?.FirstOrDefault();
		}

		public virtual IGameState GetGameState()
		{
			return Database?.GameStateTable?.Records?.FirstOrDefault();
		}

		public virtual IModule GetModule()
		{
			return Database?.ModuleTable?.Records?.FirstOrDefault();
		}

		public virtual T GetRandomElement<T>(T[] array, Func<long> indexFunc = null)
		{
			var result = default(T);

			Debug.Assert(array != null && array.Length > 0);

			if (indexFunc == null)
			{
				indexFunc = () => RollDice(1, array.Length, -1);
			}

			var i = indexFunc();

			if (i >= 0 && i < array.Length)
			{
				result = array[i];
			}

			return result;
		}

		public virtual T EvalFriendliness<T>(Friendliness friendliness, T enemyValue, T neutralValue, T friendValue)
		{
			return friendliness == Friendliness.Enemy ? enemyValue : friendliness == Friendliness.Neutral ? neutralValue : friendValue;
		}

		public virtual T EvalGender<T>(Gender gender, T maleValue, T femaleValue, T neutralValue)
		{
			return gender == Gender.Male ? maleValue : gender == Gender.Female ? femaleValue : neutralValue;
		}

		public virtual T EvalContainerType<T>(ContainerType containerType, T inValue, T onValue, T underValue, T behindValue)
		{
			return containerType == ContainerType.On ? onValue : containerType == ContainerType.Under ? underValue : containerType == ContainerType.Behind ? behindValue : inValue;
		}

		public virtual T EvalRoomType<T>(RoomType roomType, T indoorsValue, T outdoorsValue)
		{
			return roomType == RoomType.Indoors ? indoorsValue : outdoorsValue;
		}

		public virtual T EvalLightLevel<T>(LightLevel lightLevel, T darkValue, T lightValue)
		{
			return lightLevel == LightLevel.Dark ? darkValue : lightValue;
		}

		public virtual T EvalPlural<T>(bool isPlural, T singularValue, T pluralValue)
		{
			return isPlural ? pluralValue : singularValue;
		}

		public virtual string BuildPrompt(long bufSize, char fillChar, long number, string msg, string emptyVal)
		{
			StringBuilder buf;
			int i, p, q, sz;
			string result;

			if (bufSize < 8 || number < 0)
			{
				result = null;

				// PrintError

				goto Cleanup;
			}

			if (fillChar == '\0')
			{
				fillChar = ' ';
			}

			buf = new StringBuilder(BufSize);

			buf.Append(fillChar, (int)bufSize);

			p = 0;

			q = (int)bufSize;

			if (number > 0)
			{
				buf.Replace(p, 4, string.Format("{0,2}. ", number));

				p += 4;
			}

			if (msg != null)
			{
				sz = msg.Length;

				for (i = 0; i < sz && p < q; i++)
				{
					buf[p++] = msg[i];
				}
			}

			if (emptyVal != null)
			{
				sz = emptyVal.Length;

				p = Math.Max(q - (sz + 4), 0);

				buf.Replace(p, Math.Min(sz + 4, q), string.Format("[{0}]{1} ", emptyVal, fillChar == ' ' ? ':' : fillChar));
			}
			else
			{
				p = q - 2;

				buf.Replace(p, 2, string.Format("{0} ", fillChar == ' ' ? ':' : fillChar));
			}

			result = buf.ToString();

		Cleanup:

			return result;
		}

		public virtual string BuildValue(long bufSize, char fillChar, long offset, long longVal, string stringVal, string lookupMsg)
		{
			StringBuilder buf;
			int p, q, sz;
			string result;
			string s;

			if (bufSize < 8 || offset < 0 || offset > bufSize - 1)
			{
				result = null;

				// PrintError

				goto Cleanup;
			}

			if (fillChar == '\0')
			{
				fillChar = ' ';
			}

			buf = new StringBuilder(BufSize);

			buf.Append(fillChar, (int)bufSize);

			p = 0;

			q = (int)bufSize;

			s = stringVal ?? longVal.ToString();

			sz = Math.Min(s.Length, q);

			buf.Replace(p, sz, s);

			p += sz;

			if (lookupMsg != null)
			{
				p = (int)offset;

				s = string.Format("[{0}]", lookupMsg.Length > (q - p) - 2 ? lookupMsg.Substring(0, (q - p) - 2) : lookupMsg);

				sz = Math.Min(s.Length, q - p);

				buf.Replace(p, sz, s);

				p += sz;
			}

			buf.Length = p;

			result = buf.ToString();

		Cleanup:

			return result;
		}

		public virtual string WordWrap(string str, StringBuilder buf, long margin, IWordWrapArgs args, bool clearBuf = true)
		{
			int i, p, q, r;
			int currMargin;
			bool hyphenSeen;
			string result;
			string line;

			if (str == null || buf == null || margin < 1)
			{
				result = null;

				// PrintError

				goto Cleanup;
			}

			if (clearBuf)
			{
				buf.Clear();
			}

			if (args != null)
			{
				if (args.CurrColumn == margin)
				{
					buf.Append(Environment.NewLine);

					args.CurrColumn = 0;
				}

				currMargin = (int)(margin - args.CurrColumn);
			}
			else
			{
				currMargin = (int)margin;
			}

			var delims = Environment.NewLine.Length > 1 ?
					new string[] { Environment.NewLine, Environment.NewLine[1].ToString() } :
					new string[] { Environment.NewLine };

			var lines = str.Split(delims, StringSplitOptions.None);

			for (i = 0; i < lines.Length; i++)
			{
				if (i > 0)
				{
					buf.Append(Environment.NewLine);
				}

				line = lines[i];

				p = 0;

				q = line.Length;

				while (true)
				{
					if (p + currMargin >= q)
					{
						buf.Append(line.Substring(p));

						if (args != null)
						{
							args.CurrColumn = (q - p);
						}

						p += (q - p);

						break;
					}
					else
					{
						r = p + currMargin;

						hyphenSeen = false;

						while (r > p && !Char.IsWhiteSpace(line[r]) && line[r] != '-')
						{
							r--;
						}

						if (r > p)
						{
							if (line[r] == '-')
							{
								hyphenSeen = true;
							}

							buf.Append(line.Substring(p, (r - p)));

							p += (r - p) + 1;

							if (p < q && Char.IsWhiteSpace(line[p]) && (p + 2 < q) && (!Char.IsWhiteSpace(line[p + 1]) || !Char.IsWhiteSpace(line[p + 2])))
							{
								p++;
							}

							if (p < q && Char.IsWhiteSpace(line[p]) && (p + 1 < q) && !Char.IsWhiteSpace(line[p + 1]))
							{
								p++;
							}

							if (hyphenSeen)
							{
								buf.Append('-');
							}
						}
						else
						{
							if (r > 0 || args == null || (!Char.IsWhiteSpace(args.LastChar) && args.LastChar != '-'))
							{
								buf.Append(line.Substring(p, currMargin));

								p += currMargin;
							}
						}

						buf.Append(Environment.NewLine);
					}

					currMargin = (int)margin;
				}
			}

			if (args != null)
			{
				args.LastChar = buf.Length > 0 ? buf[buf.Length - 1] : '\0';
			}

			result = buf.ToString();

		Cleanup:

			return result;
		}

		public virtual string WordWrap(string str, StringBuilder buf, bool clearBuf = true)
		{
			var config = GetConfig();

			return WordWrap(str, buf, config != null ? config.WordWrapMargin : RightMargin, null, clearBuf);
		}

		public virtual string LineWrap(string str, StringBuilder buf, long startColumn, bool clearBuf = true)
		{
			string result;

			var config = GetConfig();

			var rightMargin = config != null ? config.WordWrapMargin : RightMargin;

			if (str == null || buf == null || startColumn < 0 || startColumn >= rightMargin)
			{
				result = null;

				// PrintError

				goto Cleanup;
			}

			if (clearBuf)
			{
				buf.Clear();
			}

			var chunkSize = rightMargin - startColumn;

			while (str.Length > chunkSize)
			{
				buf.AppendFormat("{0}{1}", str.Substring(0, (int)chunkSize), Environment.NewLine);

				str = str.Substring((int)chunkSize);

				chunkSize = rightMargin;
			}

			buf.Append(str);

			result = buf.ToString();

		Cleanup:

			return result;
		}

		public virtual string GetStringFromNumber(long num, bool addSpace, StringBuilder buf)
		{
			string result;

			if (buf == null)
			{
				result = null;

				// PrintError

				goto Cleanup;
			}

			buf.SetFormat("{0}{1}",
				num >= 0 && num <= 10 ? GetNumberString(num) : num.ToString(),
				addSpace ? " " : "");

			result = buf.ToString();

		Cleanup:

			return result;
		}

		public virtual long GetNumberFromString(string str)
		{
			long result = -1;
			long i;

			if (string.IsNullOrWhiteSpace(str))
			{
				// PrintError

				goto Cleanup;
			}

			for (i = 0; i < NumberStrings.Length; i++)
			{
				if (NumberStrings[i].Equals(str, StringComparison.OrdinalIgnoreCase))
				{
					result = i;

					goto Cleanup;
				}
			}

			if (long.TryParse(str, out i))
			{
				result = i;

				goto Cleanup;
			}

		Cleanup:

			return result;
		}

		public virtual RetCode RollDice(long numDice, long numSides, ref long[] dieRolls)
		{
			RetCode rc;

			if (numDice < 0 || numSides < 0 || dieRolls == null || dieRolls.Length < numDice)
			{
				rc = RetCode.InvalidArg;

				// PrintError

				goto Cleanup;
			}

			rc = RetCode.Success;

			for (var i = 0; i < numDice; i++)
			{
				dieRolls[i] = numSides > 0 ? Rand.Next((int)numSides) + 1 : 0;
			}

		Cleanup:

			return rc;
		}

		public virtual long RollDice(long numDice, long numSides, long modifier)
		{
			var result = 0L;

			var dieRolls = new long[numDice > 0 ? numDice : 1];

			var rc = RollDice(numDice, numSides, ref dieRolls);

			if (IsSuccess(rc))
			{
				result = dieRolls.Sum() + modifier;
			}

			return result;
		}

		public virtual RetCode SumHighestRolls(long[] dieRolls, long numRollsToSum, ref long result)
		{
			RetCode rc;

			if (dieRolls == null || numRollsToSum < 0 || numRollsToSum > dieRolls.Length)
			{
				rc = RetCode.InvalidArg;

				// PrintError

				goto Cleanup;
			}

			rc = RetCode.Success;

			Array.Sort(dieRolls);

			result = dieRolls.Skip((int)(dieRolls.Length - numRollsToSum)).Take((int)numRollsToSum).Sum();

		Cleanup:

			return rc;
		}

		public virtual string Capitalize(string str)
		{
			StringBuilder buf;
			bool spaceSeen;
			int p, q, sz;
			string result;

			if (str == null)
			{
				result = null;

				// PrintError

				goto Cleanup;
			}

			spaceSeen = true;

			buf = new StringBuilder(BufSize);

			buf.Append(str);

			sz = buf.Length;

			for (p = 0, q = sz; p < q; p++)
			{
				if (spaceSeen)
				{
					if (Char.IsLetter(buf[p]))
					{
						buf[p] = Char.ToUpper(buf[p]);

						spaceSeen = false;
					}
				}
				else
				{
					if (Char.IsWhiteSpace(buf[p]))
					{
						spaceSeen = true;
					}
				}
			}

			result = buf.ToString();

		Cleanup:

			return result;
		}

		public virtual void UnlinkOnFailure()
		{
			try
			{
				File.Delete("EAMONCFG.DAT");
			}
			catch (Exception ex)
			{
				if (ex != null)
				{
					// do nothing
				}
			}

			try
			{
				File.Delete("FRESHMEAT.DAT");
			}
			catch (Exception ex)
			{
				if (ex != null)
				{
					// do nothing
				}
			}

			try
			{
				File.Delete("SAVEGAME.DAT");
			}
			catch (Exception ex)
			{
				if (ex != null)
				{
					// do nothing
				}
			}
		}

		public virtual void TruncatePluralTypeEffectDesc(PluralType pluralType, long maxSize)
		{
			if (maxSize < 0)
			{
				// PrintError

				goto Cleanup;
			}

			var effectUid = GetPluralTypeEffectUid(pluralType);

			if (effectUid > 0)
			{
				var effect = EDB[effectUid];

				if (effect != null && effect.Desc.Length > maxSize)
				{
					effect.Desc = effect.Desc.Substring(0, (int)maxSize);
				}
			}

		Cleanup:

			;
		}

		public virtual void TruncatePluralTypeEffectDesc(IEffect effect)
		{
			if (effect == null)
			{
				// PrintError

				goto Cleanup;
			}

			if (effect.Desc.Length > ArtNameLen && Database.ArtifactTable.Records.FirstOrDefault(a => GetPluralTypeEffectUid(a.PluralType) == effect.Uid) != null)
			{
				effect.Desc = effect.Desc.Substring(0, ArtNameLen);
			}

			if (effect.Desc.Length > MonNameLen && Database.MonsterTable.Records.FirstOrDefault(m => GetPluralTypeEffectUid(m.PluralType) == effect.Uid) != null)
			{
				effect.Desc = effect.Desc.Substring(0, MonNameLen);
			}

		Cleanup:

			;
		}

		public virtual RetCode SplitPath(string fullPath, ref string directory, ref string fileName, ref string extension, bool appendDirectorySeparatorChar = true)
		{
			RetCode rc;

			if (fullPath == null)
			{
				rc = RetCode.InvalidArg;

				// PrintError

				goto Cleanup;
			}

			rc = RetCode.Success;

			directory = Path.GetDirectoryName(fullPath);

			fileName = Path.GetFileNameWithoutExtension(fullPath);

			extension = Path.GetExtension(fullPath);

			var directorySeparatorString = Path.DirectorySeparatorChar.ToString();

			if (appendDirectorySeparatorChar && directory.Length > 0 && !directory.EndsWith(directorySeparatorString))
			{
				directory += directorySeparatorString;
			}

		Cleanup:

			return rc;
		}

		public virtual RetCode StripPrepsAndArticles(StringBuilder buf, ref bool mySeen)
		{
			RetCode rc;
			bool found;

			if (buf == null)
			{
				rc = RetCode.InvalidArg;

				// PrintError

				goto Cleanup;
			}

			rc = RetCode.Success;

			mySeen = false;

			while (true)
			{
				found = false;

				foreach (var p in Preps)
				{
					var s = p.Name + " ";

					if (buf.StartsWith(s, true))
					{
						buf.Remove(0, s.Length);

						buf.TrimStart();

						found = true;

						break;
					}
				}

				foreach (var a in Articles)
				{
					var s = a + " ";

					if (buf.StartsWith(s, true))
					{
						buf.Remove(0, s.Length);

						buf.TrimStart();

						if (a == "my" || a == "your")
						{
							mySeen = true;
						}

						found = true;

						break;
					}
				}

				if (!found)
				{
					break;
				}
			}

		Cleanup:

			return rc;
		}

		public virtual void PrintTitle(string title, bool inBox)
		{
			long spaces;
			long size;

			if (string.IsNullOrWhiteSpace(title))
			{
				// PrintError

				goto Cleanup;
			}

			size = title.Length;

			if (inBox)
			{
				Out.Write("{0}{1}|",
					LineSep,
					Environment.NewLine);
			}

			spaces = ((RightMargin - 2) / 2) - (size / 2);

			Out.Write("{0}{1}", new string(' ', (int)spaces), title);

			if (inBox)
			{
				Out.Write("{0}|{1}{2}",
					new string(' ', (int)((RightMargin - 1) - (1 + spaces + size))),
					Environment.NewLine,
					LineSep);
			}

			Out.WriteLine();

		Cleanup:

			;
		}

		public virtual void PrintEffectDesc(IEffect effect, bool printFinalNewLine = true)
		{
			Out.Write("{0}{1}{2}", Environment.NewLine, effect != null ? effect.Desc : "???", printFinalNewLine ? Environment.NewLine : "");
		}

		public virtual void PrintEffectDesc(long effectUid, bool printFinalNewLine = true)
		{
			var effect = EDB[effectUid];

			PrintEffectDesc(effect, printFinalNewLine);
		}

		public virtual void PrintZapDirectHit()
		{
			Out.Print("ZAP!  Direct hit!");
		}

		public virtual RetCode ValidateRecordsAfterDatabaseLoaded()
		{
			RetCode rc;

			rc = RetCode.Success;

			// Note: currently only validating Monster records but could do any type if needed

			var monsterHelper = CreateInstance<IMonsterHelper>();

			var monsterList = Database.MonsterTable.Records.ToList();

			long i = 1;

			foreach (var r in monsterList)
			{
				monsterHelper.Record = r;

				if (monsterHelper.ValidateRecordAfterDatabaseLoaded() == false)
				{
					rc = RetCode.Failure;

					Error.WriteLine("{0}Error: Expected valid [{1} value], found [{2}].", Environment.NewLine, monsterHelper.GetName(monsterHelper.ErrorFieldName), monsterHelper.GetValue(monsterHelper.ErrorFieldName) ?? "null");

					Error.WriteLine("Error: ValidateAfterDatabaseLoaded function call failed for Monster record number {0}.", i);

					goto Cleanup;
				}

				i++;
			}

		Cleanup:

			return rc;
		}

		public virtual RetCode StatDisplay(IStatDisplayArgs args)
		{
			StringBuilder buf01, buf02;
			RetCode rc;
			long i, j;

			IWeapon weapon;
			ISpell spell;

			if (args == null || args.Character == null || args.Monster == null || args.ArmorString == null || args.SpellAbilities == null)
			{
				rc = RetCode.InvalidArg;

				// PrintError

				goto Cleanup;
			}

			rc = RetCode.Success;

			var origPunctSpaceCode = Out.PunctSpaceCode;

			Out.PunctSpaceCode = PunctSpaceCode.None;

			buf01 = new StringBuilder(BufSize);

			buf02 = new StringBuilder(BufSize);

			var omitSkillStats = IsRulesetVersion(15, 25) && GetGameState() != null;

			Out.Print("{0,-36}Gender: {1,-9}Damage Taken: {2}/{3}",
				args.Monster.Name.ToUpper(),
				args.Character.EvalGender("Male", "Female", "Neutral"),
				args.Monster.DmgTaken,
				args.Monster.Hardiness);

			var ibp = GetIntellectBonusPct(args.Character.GetStat(Stat.Intellect));

			buf01.AppendFormat("{0}{1}{2}%)",
				"(Learning: ",
				ibp > 0 ? "+" : "",
				ibp);

			buf02.AppendFormat("{0}{1}",
				args.Speed > 0 ? args.Monster.Agility / 2 : args.Monster.Agility,
				args.Speed > 0 ? "x2" : "");

			Out.WriteLine("{0}{1}{2,-2}{3,20}{4,15}{5}{0}{6}{7,-3}{8,34}{9,-2}{10,15}{11}{12}%)",
				Environment.NewLine,
				"Intellect:  ", args.Character.GetStat(Stat.Intellect),
				buf01.ToString(),
				"Agility :  ", buf02.ToString(),
				"Hardiness:  ", args.Monster.Hardiness,
				"Charisma:  ", args.Character.GetStat(Stat.Charisma),
				"(Charm Mon: ",
				args.CharmMon > 0 ? "+" : "",
				args.CharmMon);

			if (!omitSkillStats)
			{
				Out.Write("{0}{1}{2,39}",
					Environment.NewLine,
					"Weapon Abilities:",
					"Spell Abilities:");

				var weaponValues = EnumUtil.GetValues<Weapon>();

				var spellValues = EnumUtil.GetValues<Spell>();

				i = Math.Min((long)weaponValues[0], (long)spellValues[0]);

				j = Math.Max((long)weaponValues[weaponValues.Count - 1], (long)spellValues[spellValues.Count - 1]);

				while (i <= j)
				{
					Out.WriteLine();

					if (Enum.IsDefined(typeof(Weapon), i))
					{
						weapon = GetWeapon((Weapon)i);

						Debug.Assert(weapon != null);

						Out.Write(" {0,-5}: {1,3}%",
							weapon.Name,
							args.Character.GetWeaponAbility(i));
					}
					else
					{
						Out.Write("{0,12}", "");
					}

					if (Enum.IsDefined(typeof(Spell), i))
					{
						spell = GetSpell((Spell)i);

						Debug.Assert(spell != null);

						Out.Write("{0,29}{1,-5}: {2,3}% / {3}%",
							"",
							spell.Name,
							args.GetSpellAbility(i),
							args.Character.GetSpellAbility(i));
					}

					i++;
				}
			}

			Out.WriteLine("{0}{0}{1}{2,-30}{3}{4,-6}",
				Environment.NewLine,
				"Gold: ",
				args.Character.HeldGold,
				"In bank: ",
				args.Character.BankGold);

			Out.Print("Armor:  {0}{1}",
				args.ArmorString.PadTRight(28, ' '),
				!omitSkillStats ? string.Format(" Armor Expertise:  {0}%", args.Character.ArmorExpertise) : "");

			var wcg = GetWeightCarryableGronds(args.Monster.Hardiness);

			Out.Print("Weight carried: {0}/{1} Gronds (One Grond = Ten DOS)",
				args.Weight,
				wcg);

			Out.PunctSpaceCode = origPunctSpaceCode;

		Cleanup:

			return rc;
		}

		public virtual RetCode GetRecordNameList(IList<IGameBase> recordList, ArticleType articleType, bool showCharOwned, StateDescDisplayCode stateDescCode, bool showContents, bool groupCountOne, StringBuilder buf)
		{
			RetCode rc;
			long i;

			if (recordList == null || buf == null)
			{
				rc = RetCode.InvalidArg;

				// PrintError

				goto Cleanup;
			}

			rc = RetCode.Success;

			for (i = 0; i < recordList.Count; i++)
			{
				var r = recordList[(int)i];

				var a = r as IArtifact;

				var m = r as IMonster;

				var showStateDesc = stateDescCode == StateDescDisplayCode.AllStateDescs;

				if (!showStateDesc && a != null)
				{
					showStateDesc = stateDescCode == StateDescDisplayCode.SideNotesOnly && a.IsStateDescSideNotes();
				}

				if (!showStateDesc && m != null)
				{
					showStateDesc = stateDescCode == StateDescDisplayCode.SideNotesOnly && m.IsStateDescSideNotes();
				}

				buf.AppendFormat("{0}{1}",
					i == 0 ? "" : i == recordList.Count - 1 && recordList.Count > 2 ? ", and " : i == recordList.Count - 1 ? " and " : ", ",
					r.GetDecoratedName
					(
						"Name",
						articleType == ArticleType.None || articleType == ArticleType.The ? articleType : r.ArticleType,
						false,
						showCharOwned,
						showStateDesc,
						showContents,
						groupCountOne
					)
				);
			}

		Cleanup:

			return rc;
		}

		public virtual RetCode GetRecordNameCount(IList<IGameBase> recordList, string name, bool exactMatch, ref long count)
		{
			RetCode rc;

			if (recordList == null || string.IsNullOrWhiteSpace(name))
			{
				rc = RetCode.InvalidArg;

				// PrintError

				goto Cleanup;
			}

			rc = RetCode.Success;

			count = 0;

			foreach (var r in recordList)
			{
				if (exactMatch)
				{
					if (r.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
					{
						count++;
					}
				}
				else
				{
					if (r.Name.Contains(name, StringComparison.OrdinalIgnoreCase))
					{
						count++;
					}
				}
			}

		Cleanup:

			return rc;
		}

		public virtual RetCode ListRecords(IList<IGameBase> recordList, bool capitalize, bool showExtraInfo, StringBuilder buf)
		{
			RetCode rc;
			long i;

			if (recordList == null || buf == null)
			{
				rc = RetCode.InvalidArg;

				// PrintError

				goto Cleanup;
			}

			rc = RetCode.Success;

			for (i = 0; i < recordList.Count; i++)
			{
				var r = recordList[(int)i];

				var a = r as IArtifact;

				if (showExtraInfo && a != null && a.GeneralWeapon != null)
				{
					var ac = a.GeneralWeapon;

					Debug.Assert(ac != null);

					var weapon = GetWeapon((Weapon)ac.Field2);

					Debug.Assert(weapon != null);

					buf.AppendFormat("{0}{1,2}. {2} ({3,-8}/{4,3}%/{5,2}D{6,-2}/{7}H)",
						Environment.NewLine,
						i + 1,
						capitalize ? Capitalize(a.Name.PadTRight(31, ' ')) : a.Name.PadTRight(31, ' '),
						weapon.Name,
						ac.Field1,
						ac.Field3,
						ac.Field4,
						ac.Field5);
				}
				else
				{
					buf.AppendFormat("{0}{1,2}. {2}",
						Environment.NewLine,
						i + 1,
						capitalize ? Capitalize(r.Name) : r.Name);
				}
			}

		Cleanup:

			return rc;
		}

		public virtual RetCode ResolveUidMacros(string str, StringBuilder buf, bool resolveFuncs, bool recurse, ref long invalidUid)
		{
			StringBuilder buf01, buf02, srcBuf, dstBuf;
			MatchCollection matches;
			long numStars, numAts;
			long m, currUid;
			IEffect effect;
			Func<string> func;
			RetCode rc;
			int i;

			if (str == null || buf == null)
			{
				rc = RetCode.InvalidArg;

				// PrintError

				goto Cleanup;
			}

			rc = RetCode.Success;

			invalidUid = 0;

			if (str.Length < 4)
			{
				buf.Append(str);

				goto Cleanup;
			}

			if ((str[0] == '*' || (resolveFuncs && str[0] == '@')) && Char.IsDigit(str[1]) && Char.IsDigit(str[2]) && Char.IsDigit(str[3]) && long.TryParse(str.Substring(1, 3), out currUid) == true)
			{
				if (str[0] == '*')
				{
					effect = EDB[currUid];

					if (effect != null)
					{
						str = " " + str;
					}
				}
				else
				{
					if (MacroFuncs.TryGetValue(currUid, out func) && func != null)
					{
						str = " " + str;
					}
				}
			}

			matches = Regex.Matches(str, resolveFuncs ? ResolveUidMacroRegexPattern : ResolveEffectRegexPattern);

			if (matches.Count == 0)
			{
				buf.Append(str);

				goto Cleanup;
			}

			buf01 = new StringBuilder(BufSize);

			buf02 = new StringBuilder(BufSize);

			buf01.Append(str);

			srcBuf = buf01;

			dstBuf = buf02;

			m = 0;

			do
			{
				dstBuf.Clear();

				i = 0;

				foreach (Match match in matches)
				{
					foreach (Capture capture in match.Captures)
					{
						effect = null;

						func = null;

						numStars = 0;

						numAts = 0;

						if (capture.Value[1] == '*')
						{
							numStars = 1 + (capture.Value[0] == '*' ? 1 : 0);
						}
						else
						{
							numAts = 1 + (capture.Value[0] == '@' ? 1 : 0);
						}

						if (long.TryParse(capture.Value.Substring(2), out currUid) == false || currUid < 0)
						{
							currUid = 0;
						}

						if (numStars > 0)
						{
							effect = EDB[currUid];
						}
						else
						{
							if (MacroFuncs.TryGetValue(currUid, out func) == false)
							{
								func = null;
							}
						}

						dstBuf.Append(srcBuf.ToString().Substring(i, (capture.Index + (numStars == 1 || numAts == 1 ? 1 : 0)) - i));

						if (numStars > 0)
						{
							if (effect != null)
							{
								dstBuf.AppendFormat("{0}{1}{2}",
									numStars == 1 ? Environment.NewLine : "",
									numStars == 1 ? Environment.NewLine : "",
									effect.Desc);
							}
							else
							{
								if (invalidUid == 0 || (currUid > 0 && currUid < invalidUid))
								{
									invalidUid = currUid;
								}

								dstBuf.Append(capture.Value.Substring(numStars == 1 ? 1 : 0));
							}
						}
						else
						{
							if (func != null)
							{
								var desc = func();

								dstBuf.AppendFormat("{0}{1}{2}",
									numAts == 1 && !string.IsNullOrWhiteSpace(desc) ? Environment.NewLine : "",
									numAts == 1 && !string.IsNullOrWhiteSpace(desc) ? Environment.NewLine : "",
									desc);
							}
							else
							{
								dstBuf.Append(capture.Value.Substring(numAts == 1 ? 1 : 0));
							}
						}

						i = capture.Index + capture.Length;
					}
				}

				dstBuf.Append(srcBuf.ToString().Substring(i));

				if (++m >= MaxRecursionLevel)
				{
					recurse = false;
				}

				if (recurse)
				{
					matches = Regex.Matches(dstBuf.ToString(), resolveFuncs ? ResolveUidMacroRegexPattern : ResolveEffectRegexPattern);

					if (matches.Count > 0)
					{
						if (srcBuf == buf01)
						{
							srcBuf = buf02;

							dstBuf = buf01;
						}
						else
						{
							srcBuf = buf01;

							dstBuf = buf02;
						}
					}
				}
			}
			while (recurse && matches.Count > 0);

			buf.Append(dstBuf);

		Cleanup:

			return rc;
		}

		public virtual RetCode ResolveUidMacros(string str, StringBuilder buf, bool resolveFuncs, bool recurse)
		{
			long invalidUid = 0;

			return ResolveUidMacros(str, buf, resolveFuncs, recurse, ref invalidUid);
		}

		public virtual double GetWeaponPriceOrValue(string name, long complexity, Weapon type, long dice, long sides, long numHands, bool calcPrice, ref bool isMarcosWeapon)
		{
			double wp;

			wp = 0.0;

			isMarcosWeapon = false;

			if (!string.IsNullOrWhiteSpace(name))
			{
				name = name.Trim().TrimEnd('#');
			}

			if (string.IsNullOrWhiteSpace(name) || !Enum.IsDefined(typeof(Weapon), type) || dice < 1 || sides < 1)
			{
				// PrintError

				goto Cleanup;
			}

			var weapon = GetWeapon(type);

			Debug.Assert(weapon != null);

			wp = weapon.MarcosPrice;

			if (complexity >= 0 && complexity < 10)
			{
				wp *= 0.80;
			}
			else if (complexity < 0)
			{
				wp *= 0.60;
			}

			isMarcosWeapon = name.Equals(weapon.MarcosName ?? weapon.Name, StringComparison.OrdinalIgnoreCase) && (complexity == -10 || complexity == 0 || complexity == 10) && dice == weapon.MarcosDice && sides == weapon.MarcosSides && numHands == weapon.MarcosNumHands;

			if (!isMarcosWeapon)
			{
				if (complexity > 10)
				{
					wp += (calcPrice ? 40 : 5);
				}

				if (complexity > 15)
				{
					wp += (calcPrice ? 80 : 10);
				}

				if (complexity > 25)
				{
					wp += (calcPrice ? 320 : 20);
				}

				if (complexity > 35)
				{
					wp += (calcPrice ? 960 : 50);
				}

				if (dice * sides > 10)
				{
					wp += (calcPrice ? 40 : 5);
				}

				if (dice * sides > 15)
				{
					wp += (calcPrice ? 80 : 10);
				}

				if (dice * sides > 25)
				{
					wp += (calcPrice ? 320 : 20);
				}

				if (dice * sides > 35)
				{
					wp += (calcPrice ? 960 : 50);
				}
			}

		Cleanup:

			return wp;
		}

		public virtual double GetWeaponPriceOrValue(ICharacterArtifact weapon, bool calcPrice, ref bool isMarcosWeapon)
		{
			Debug.Assert(weapon != null);

			return GetWeaponPriceOrValue(weapon.Name, weapon.Field1, (Weapon)weapon.Field2, weapon.Field3, weapon.Field4, weapon.Field5, calcPrice, ref isMarcosWeapon);
		}

		public virtual double GetArmorPriceOrValue(Armor armor, bool calcPrice, ref bool isMarcosArmor)
		{
			double ap;

			ap = 0.0;

			isMarcosArmor = false;

			if (!Enum.IsDefined(typeof(Armor), armor))
			{
				// PrintError

				goto Cleanup;
			}

			var armor01 = ((long)armor / 2) * 2;

			if (armor01 > 0)
			{
				var armor02 = GetArmor((Armor)armor01);

				Debug.Assert(armor02 != null);

				if (armor02.MarcosPrice > 0)
				{
					if (calcPrice)
					{
						ap = armor02.MarcosPrice;
					}
					else
					{
						ap = armor02.ArtifactValue;
					}

					isMarcosArmor = true;
				}
				else
				{
					if (calcPrice)
					{
						armor02 = GetArmor(Armor.PlateMail);

						Debug.Assert(armor02 != null);

						ap = armor02.MarcosPrice + (((armor01 - (long)Armor.PlateMail) / 2) * 1000);
					}
					else
					{
						ap = armor02.ArtifactValue;
					}
				}
			}

		Cleanup:

			return ap;
		}

		public virtual void AppendFieldDesc(FieldDesc fieldDesc, StringBuilder buf, StringBuilder fullDesc, StringBuilder briefDesc)
		{
			AppendFieldDesc(fieldDesc, buf, fullDesc != null ? fullDesc.ToString() : null, briefDesc != null ? briefDesc.ToString() : null);
		}

		public virtual void AppendFieldDesc(FieldDesc fieldDesc, StringBuilder buf, string fullDesc, string briefDesc)
		{
			Debug.Assert(buf != null && fullDesc != null);

			if (briefDesc != null)
			{
				if (fieldDesc == FieldDesc.Full)
				{
					buf.AppendFormat("{0}{1}{0}{0}{2}{0}", Environment.NewLine, fullDesc, briefDesc);
				}
				else if (fieldDesc == FieldDesc.Brief)
				{
					buf.AppendPrint("{0}", briefDesc);
				}
			}
			else
			{
				if (fieldDesc == FieldDesc.Full)
				{
					buf.AppendPrint("{0}", fullDesc);
				}
			}
		}

		public virtual void CopyCharacterArtifactFields(ICharacterArtifact destCa, ICharacterArtifact sourceCa)
		{
			Debug.Assert(destCa != null);

			Debug.Assert(sourceCa != null);

			destCa.Name = CloneInstance(sourceCa.Name);

			destCa.Desc = CloneInstance(sourceCa.Desc);

			destCa.IsPlural = sourceCa.IsPlural;

			destCa.PluralType = sourceCa.PluralType;

			destCa.ArticleType = sourceCa.ArticleType;

			destCa.Value = sourceCa.Value;

			destCa.Weight = sourceCa.Weight;

			destCa.Type = sourceCa.Type;

			destCa.Field1 = sourceCa.Field1;

			destCa.Field2 = sourceCa.Field2;

			destCa.Field3 = sourceCa.Field3;

			destCa.Field4 = sourceCa.Field4;

			destCa.Field5 = sourceCa.Field5;
		}

		public virtual void CopyArtifactCategoryFields(IArtifactCategory destAc, IArtifactCategory sourceAc)
		{
			Debug.Assert(destAc != null);

			Debug.Assert(sourceAc != null);

			destAc.Field1 = sourceAc.Field1;

			destAc.Field2 = sourceAc.Field2;

			destAc.Field3 = sourceAc.Field3;

			destAc.Field4 = sourceAc.Field4;

			destAc.Field5 = sourceAc.Field5;
		}

		public virtual IList<IArtifact> GetArtifactList(params Func<IArtifact, bool>[] whereClauseFuncs)
		{
			Debug.Assert(whereClauseFuncs != null);

			var artifactList = new List<IArtifact>();

			foreach (var whereClauseFunc in whereClauseFuncs)
			{
				Debug.Assert(whereClauseFunc != null);

				artifactList.AddRange(Database.ArtifactTable.Records.Where(whereClauseFunc));
			}

			return artifactList;
		}

		public virtual IList<IMonster> GetMonsterList(params Func<IMonster, bool>[] whereClauseFuncs)
		{
			Debug.Assert(whereClauseFuncs != null);

			var monsterList = new List<IMonster>();

			foreach (var whereClauseFunc in whereClauseFuncs)
			{
				Debug.Assert(whereClauseFunc != null);

				monsterList.AddRange(Database.MonsterTable.Records.Where(whereClauseFunc));
			}

			return monsterList;
		}

		public virtual IList<IGameBase> GetRecordList(params Func<IGameBase, bool>[] whereClauseFuncs)
		{
			Debug.Assert(whereClauseFuncs != null);

			var recordList = new List<IGameBase>();

			foreach (var whereClauseFunc in whereClauseFuncs)
			{
				Debug.Assert(whereClauseFunc != null);

				recordList.AddRange(Database.ArtifactTable.Records.Where(whereClauseFunc));

				recordList.AddRange(Database.MonsterTable.Records.Where(whereClauseFunc));
			}

			return recordList;
		}

		public virtual IArtifact GetNthArtifact(IList<IArtifact> artifactList, long which, Func<IArtifact, bool> whereClauseFunc)
		{
			Debug.Assert(artifactList != null);

			Debug.Assert(which > 0);

			Debug.Assert(whereClauseFunc != null);

			return artifactList.Where(a => whereClauseFunc(a)).OrderBy(a => a.Uid).Skip((int)(which - 1)).Take(1).FirstOrDefault();
		}

		public virtual IMonster GetNthMonster(IList<IMonster> monsterList, long which, Func<IMonster, bool> whereClauseFunc)
		{
			Debug.Assert(monsterList != null);

			Debug.Assert(which > 0);

			Debug.Assert(whereClauseFunc != null);

			return monsterList.Where(m => whereClauseFunc(m)).OrderBy(m => m.Uid).Skip((int)(which - 1)).Take(1).FirstOrDefault();
		}

		public virtual IGameBase GetNthRecord(IList<IGameBase> recordList, long which, Func<IGameBase, bool> whereClauseFunc)
		{
			Debug.Assert(recordList != null);

			Debug.Assert(which > 0);

			Debug.Assert(whereClauseFunc != null);

			return recordList.Where(r => whereClauseFunc(r)).OrderBy((r) =>
			{

				return string.Format("{0}_{1}", r.Name.ToLower(), r.Uid);

			}).Skip((int)(which - 1)).Take(1).FirstOrDefault();
		}

		public virtual void StripUniqueCharsFromRecordNames(IList<IGameBase> recordList)
		{
			Debug.Assert(recordList != null);

			var sz = recordList.Count();

			for (var i = 0; i < sz; i++)
			{
				recordList[i].Name = recordList[i].Name.TrimEnd(recordList[i] is IArtifact ? '#' : '%');
			}
		}

		public virtual void AddUniqueCharsToRecordNames(IList<IGameBase> recordList)
		{
			long c;

			Debug.Assert(recordList != null);

			var sz = recordList.Count();

			do
			{
				c = 0;

				for (var i = 0; i < sz; i++)
				{
					for (var j = i + 1; j < sz; j++)
					{
						if ((recordList[j] is IArtifact && recordList[i] is IArtifact) || (recordList[j] is IMonster && recordList[i] is IMonster))
						{
							if (recordList[j].Name.Equals(recordList[i].Name, StringComparison.OrdinalIgnoreCase))
							{
								recordList[j].Name += (recordList[j] is IArtifact ? "#" : "%");

								c = 1;
							}
						}
					}
				}
			}
			while (c == 1);
		}

		public virtual void ConvertWeaponToGoldOrTreasure(IArtifact artifact, bool convertToGold)
		{
			RetCode rc;

			Debug.Assert(artifact != null);

			if (artifact.GeneralWeapon != null)
			{
				var ac = artifact.GetArtifactCategory(convertToGold ? ArtifactType.Gold : ArtifactType.Treasure);

				if (ac == null)
				{
					ac = artifact.GeneralWeapon;

					Debug.Assert(ac != null);

					ac.Type = convertToGold ? ArtifactType.Gold : ArtifactType.Treasure;

					ac.Field1 = 0;

					ac.Field2 = 0;

					ac.Field3 = 0;

					ac.Field4 = 0;

					ac.Field5 = 0;
				}
				else
				{
					var acList = artifact.Categories.Where(ac01 => ac01 != null && !ac01.IsWeapon01()).ToList();

					for (var i = 0; i < artifact.Categories.Length; i++)
					{
						artifact.SetCategory(i, acList.Count > i ? acList[i] : null);
					}
				}

				rc = artifact.RemoveStateDesc(artifact.GetReadyWeaponDesc());

				Debug.Assert(IsSuccess(rc));

				var monster = Database.MonsterTable.Records.FirstOrDefault(m => m.Weapon == artifact.Uid || m.Weapon == -artifact.Uid - 1);

				if (monster != null)
				{
					monster.Weapon = -1;
				}
			}
		}

		public virtual void ConvertTreasureToContainer(IArtifact artifact, ContainerType containerType = ContainerType.In)
		{
			Debug.Assert(artifact != null);

			var ac = artifact.Treasure;

			Debug.Assert(ac != null);

			switch (containerType)
			{
				case ContainerType.In:

					ac.Type = ArtifactType.InContainer;

					ac.Field1 = 0;

					ac.Field2 = 1;

					ac.Field3 = 9999;

					ac.Field4 = 1;

					ac.Field5 = 0;

					break;

				case ContainerType.On:

					// TODO: implement

					break;

				case ContainerType.Under:

					// TODO: implement

					break;

				case ContainerType.Behind:

					// TODO: implement

					break;
			}
		}

		public virtual void ConvertContainerToTreasure(IArtifact artifact, ContainerType containerType = ContainerType.In)
		{
			Debug.Assert(artifact != null);

			var ac = EvalContainerType(containerType, artifact.InContainer, artifact.OnContainer, artifact.UnderContainer, artifact.BehindContainer);

			Debug.Assert(ac != null);

			ac.Type = ArtifactType.Treasure;

			ac.Field1 = 0;

			ac.Field2 = 0;

			ac.Field3 = 0;

			ac.Field4 = 0;

			ac.Field5 = 0;
		}

		#endregion

		public Engine()
		{
			CommandSepTokens = new string[] { ".", "!", "?", ";", ",", "and", "then", "also" };

			PronounTokens = new string[] { "him", "her", "it", "that", "them", "those" };

			ToughDesc = string.Format("Monsters usually fall into one of the following categories, but it is possible to create hybrids that are weak in some areas and strong in others:{0}{0}Weak Monsters - wimps and small creatures like rats, kobolds, etc.{0}Medium Monsters - petty thugs, orcs, goblins, etc.{0}Tough Monsters - giants, trolls, highly skilled warriors, etc.{0}Exceptional Monsters - dragons, demons, special villians, etc.{0}{0}For group Monsters, enter data relating to a single member of the group and scale values down lower than usual for groups with five or more members.", Environment.NewLine);

			CourageDesc = string.Format("Courage works as follows:{0}{0}1-100% - the chance the Monster won't flee combat and/or follow a fleeing player (if enemy).  If the Monster is injured or gravely injured, then effective courage is reduced by 5% or 10%, respectively.{0}200% - the Monster will never flee and always follow the player.", Environment.NewLine);

			ProcessMutexName = string.Format(@"Global\Eamon_CS_{0}_Process_Mutex", ProgVersion);

			RulesetVersions = new long[NumRulesetVersions];

			Buf = new StringBuilder(BufSize);

			LineSep = new string('-', (int)RightMargin);

			MacroFuncs = new Dictionary<long, Func<string>>();

			Articles = new string[]			// TODO: fix ???
			{
				"a",
				"an",
				"some",
				"the",
				"this",
				"these",
				"that",
				"those",
				"my",
				"your",
				"his",
				"her",
				"its"
			};

			UnknownName = "???";

			Rand = new Random();

			NumberStrings = new string[]
			{
				"zero",
				"one",
				"two",
				"three",
				"four",
				"five",
				"six",
				"seven",
				"eight",
				"nine",
				"ten"
			};

			FieldDescNames = new string[]
			{
				"None",
				"Brief",
				"Full"
			};

			StatusNames = new string[]
			{
				"Unknown",
				"Alive",
				"Dead",
				"Adventuring"
			};

			ClothingNames = new string[]
			{
				"Armor & Shields",
				"Overclothes",
				"Shoes & Boots",
				"Gloves",
				"Hats & Headwear",
				"Jewelry",
				"Undergarments",
				"Shirts",
				"Pants"
			};

			CombatCodeDescs = new string[]
			{
				"Doesn't fight",
				"Uses weapons or natural weapons",		// "Will use wep. or nat. weapons", 
				"Normal",
				"Uses 'attacks' only"						// "'ATTACKS' only"
			};

			ContainerDisplayCodeDescs = new string[]
			{
				"None",
				"Something/Some Stuff",
				"Artifact Name/Some Stuff",
				"Artifact Name List"
			};

			LightLevelNames = new string[]
			{
				"Dark",
				"Light"
			};
		}
	}
}
