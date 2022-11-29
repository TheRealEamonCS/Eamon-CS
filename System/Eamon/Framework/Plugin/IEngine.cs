
// IEngine.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Eamon.Framework.Args;
using Eamon.Framework.DataStorage;
using Eamon.Framework.DataStorage.Generic;
using Eamon.Framework.Portability;
using Eamon.Framework.Primitive.Classes;
using Eamon.Framework.Primitive.Enums;

namespace Eamon.Framework.Plugin
{
	/// <summary></summary>
	public interface IEngine
	{
		#region Public Properties

		/// <summary></summary>
		string[] CommandSepTokens { get; }

		/// <summary></summary>
		string[] PronounTokens { get; }

		/// <summary></summary>
		string ToughDesc { get; }

		/// <summary></summary>
		string CourageDesc { get; }

		/// <summary></summary>
		int ArtNameLen { get; }

		/// <summary></summary>
		int ArtStateDescLen { get; }

		/// <summary></summary>
		int ArtDescLen { get; }

		/// <summary></summary>
		int CharNameLen { get; }

		/// <summary></summary>
		int CharArtNameLen { get; }

		/// <summary></summary>
		int CharArtDescLen { get; }

		/// <summary></summary>
		int EffDescLen { get; }

		/// <summary></summary>
		int FsNameLen { get; }

		/// <summary></summary>
		int FsFileNameLen { get; }

		/// <summary></summary>
		int HntQuestionLen { get; }

		/// <summary></summary>
		int HntAnswerLen { get; }

		/// <summary></summary>
		int ModNameLen { get; }

		/// <summary></summary>
		int ModDescLen { get; }

		/// <summary></summary>
		int ModAuthorLen { get; }

		/// <summary></summary>
		int ModVolLabelLen { get; }

		/// <summary></summary>
		int ModSerialNumLen { get; }

		/// <summary></summary>
		int MonNameLen { get; }

		/// <summary></summary>
		int MonStateDescLen { get; }

		/// <summary></summary>
		int MonDescLen { get; }

		/// <summary></summary>
		int RmNameLen { get; }

		/// <summary></summary>
		int RmDescLen { get; }

		/// <summary></summary>
		long AxePrice { get; }

		/// <summary></summary>
		long BowPrice { get; }

		/// <summary></summary>
		long MacePrice { get; }

		/// <summary></summary>
		long SpearPrice { get; }

		/// <summary></summary>
		long SwordPrice { get; }

		/// <summary></summary>
		long ShieldPrice { get; }

		/// <summary></summary>
		long LeatherArmorPrice { get; }

		/// <summary></summary>
		long ChainMailPrice { get; }

		/// <summary></summary>
		long PlateMailPrice { get; }

		/// <summary></summary>
		long BlastPrice { get; }

		/// <summary></summary>
		long HealPrice { get; }

		/// <summary></summary>
		long SpeedPrice { get; }

		/// <summary></summary>
		long PowerPrice { get; }

		/// <summary></summary>
		long RecallPrice { get; }

		/// <summary></summary>
		long StatGainPrice { get; }

		/// <summary></summary>
		long WeaponTrainingPrice { get; }

		/// <summary></summary>
		long ArmorTrainingPrice { get; }

		/// <summary></summary>
		long SpellTrainingPrice { get; }

		/// <summary></summary>
		long InfoBoothPrice { get; }

		/// <summary></summary>
		long FountainPrice { get; }

		/// <summary></summary>
		long NumDatabases { get; }

		/// <summary></summary>
		long NumRulesetVersions { get; }

		/// <summary></summary>
		long NumArtifactCategories { get; }

		/// <summary></summary>
		int BufSize { get; }

		/// <summary></summary>
		int BufSize01 { get; }

		/// <summary></summary>
		int BufSize02 { get; }

		/// <summary></summary>
		int BufSize03 { get; }

		/// <summary></summary>
		string ResolveEffectRegexPattern { get; }

		/// <summary></summary>
		string ResolveUidMacroRegexPattern { get; }

		/// <summary></summary>
		string ValidWorkDirRegexPattern { get; }

		/// <summary></summary>
		string CoreLibRegexPattern { get; }

		/// <summary></summary>
		string MscorlibRegexPattern { get; }

		/// <summary></summary>
		string CoreLibName { get; }

		/// <summary></summary>
		string MscorlibName { get; }

		/// <summary></summary>
		string RecIdepErrorFmtStr { get; }

		/// <summary></summary>
		string AndroidAdventuresDir { get; }

		/// <summary></summary>
		string AdventuresDir { get; }

		/// <summary></summary>
		string QuickLaunchDir { get; }

		/// <summary></summary>
		string DefaultWorkDir { get; }

		/// <summary></summary>
		string ProcessMutexName { get; }

		/// <summary></summary>
		string EamonDesktopSlnFile { get; }

		/// <summary></summary>
		string StackTraceFile { get; }

		/// <summary></summary>
		string ProgVersion { get; }

		/// <summary></summary>
		long InfiniteDrinkableEdible { get; }

		/// <summary></summary>
		long DirectionExit { get; }

		/// <summary></summary>
		long LimboLocation { get; }

		/// <summary></summary>
		long MinWeaponComplexity { get; }

		/// <summary></summary>
		long MaxWeaponComplexity { get; }

		/// <summary></summary>
		long MinGoldValue { get; }

		/// <summary></summary>
		long MaxGoldValue { get; }

		/// <summary></summary>
		long MaxPathLen { get; }

		/// <summary></summary>
		long MaxRecursionLevel { get; }

		/// <summary></summary>
		int WindowWidth { get; }

		/// <summary></summary>
		int WindowHeight { get; }

		/// <summary></summary>
		int BufferWidth { get; }

		/// <summary></summary>
		int BufferHeight { get; }

		/// <summary></summary>
		long RightMargin { get; }

		/// <summary></summary>
		long NumRows { get; }

		/// <summary>
		/// Gets or sets the Dictionary that stores interface to class mappings as Key/Value pairs, used for dependency injection.
		/// </summary>
		/// <remarks>
		/// This is the heart of the customized dependency injection architecture used by Eamon CS.  If you've ever wondered
		/// how the system seems to "know" which class needs to be instantiated to make any game work, it is all based on the
		/// contents of this Dictionary.  When the system calls <see cref="CreateInstance{T}(Action{T})">CreateInstance</see>,
		/// this Dictionary is consulted and the concrete class corresponding to the provided interface is created.  If you use the
		/// debugger to step through a game while its running, looking in this Dictionary at the Key/Value pairs is very enlightening.
		/// Another piece to this puzzle is how the Dictionary itself is loaded during program bootstrap, and this information will be
		/// provided in the comments for <see cref="LoadPluginClassMappings">LoadPluginClassMappings</see> and 
		/// <see cref="LoadPluginClassMappings01(Assembly)">LoadPluginClassMappings01</see>.
		/// <para>
		/// Side note:  Research was done on dependency injection toolkits (like Ninject, etc.), but that avenue was avoided because it
		/// would have introduced unnecessary dependencies for the project.  There don't seem to be any frameworks that fit what was
		/// done in Eamon CS, so this dependency injection architecture is outside typical patterns.
		/// </para>
		/// </remarks>
		IDictionary<Type, Type> ClassMappingsDictionary { get; set; }

		/// <summary>
		/// Gets or sets the text reader that accepts user input from the console window.
		/// </summary>
		ITextReader In { get; set; }

		/// <summary>
		/// Gets or sets the text writer that prints text to the console window.
		/// </summary>
		ITextWriter Out { get; set; }

		/// <summary>
		/// Gets or sets the text writer that prints error messages to the console window.
		/// </summary>
		ITextWriter Error { get; set; }

		/// <summary>
		/// Gets or sets a mutex (mutual exclusion lock) that ensures only one Eamon CS process runs in a given operating system at a time.
		/// </summary>
		/// <remarks>
		/// Eamon CS loads its datafiles into an in-memory database, requiring the use of a global system mutex to prevent datafile corruption.
		/// This mutex prevents multiple Eamon CS processes from running simultaneously, even if they are run from different repositories on
		/// the same computer.
		/// </remarks>
		IMutex Mutex { get; set; }

		/// <summary></summary>
		ITransferProtocol TransferProtocol { get; set; }

		/// <summary>
		/// Gets or sets the platform-independent Directory manager that Eamon CS relies on for all directory-related operations.
		/// </summary>
		IDirectory Directory { get; set; }

		/// <summary>
		/// Gets or sets the platform-independent File manager that Eamon CS relies on for all file-related operations.
		/// </summary>
		IFile File { get; set; }

		/// <summary>
		/// Gets or sets the platform-independent file system Path manager that Eamon CS relies on for all file system path-related operations.
		/// </summary>
		IPath Path { get; set; }

		/// <summary></summary>
		ISharpSerializer SharpSerializer { get; set; }

		/// <summary>
		/// Gets or sets the platform-independent Thread manager that Eamon CS relies on for all thread-related operations.
		/// </summary>
		IThread Thread { get; set; }

		/// <summary></summary>
		MemoryStream CloneStream { get; set; }

		/// <summary></summary>
		long MutatePropertyCounter { get; set; }

		/// <summary></summary>
		string WorkDir { get; set; }

		/// <summary></summary>
		string FilePrefix { get; set; }

		/// <summary>
		/// Gets a value indicating which Eamon ruleset applies to the current game.
		/// </summary>
		/// <remarks>
		/// Earlier rulesets of Eamon gave a different "vibe" to the gameplay experience, so a means to support them was introduced.  Currently supported
		/// rulesets include 5 for DDD5 and 0 for Eamon Deluxe.  The Temple of Ngurct is the only game using DDD5 at the time of this writing.  Recent
		/// enhancements allow a game to change rulesets dynamically at runtime.
		/// </remarks>
		long RulesetVersion { get; }

		/// <summary>
		/// Gets a value indicating whether "mutating properties" (those that are dynamically calculated) should be enabled.
		/// </summary>
		/// <remarks>
		/// Examples of mutating properties include <see cref="IMonster">Monster</see> <see cref="IMonster.Courage"> Courage</see> and
		/// <see cref="IArtifact">Artifact</see> <see cref="IArtifact.Location"> Location</see>, but there are more scattered through the various games as
		/// well.  You will note that both the getter and setter of a property can mutate if necessary.  In general, you want this behavior enabled during
		/// gameplay so properties can respond to changes in game state, but never when the <see cref="SharpSerializer">SharpSerializer</see> is running
		/// during datafile serialization or deserialization because it can lead to corrupted values.  This value should be checked in all mutating
		/// properties and if it is <c>false</c> they should disable complex calculations and return a simple base value.
		/// </remarks>
		bool EnableMutateProperties { get; }

		/// <summary></summary>
		bool EnableStdio { get; set; }

		/// <summary></summary>
		bool EnableNegativeRoomUidLinks { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the <see cref="Mutex">Mutex</see> should be ignored, allowing unlimited Eamon CS processes
		/// to run simultaneously.
		/// </summary>
		/// <remarks>
		/// This should only be done when there is no chance of datafile corruption.  For example, running processes in different repositories
		/// that always access different datafiles.
		/// </remarks>
		bool IgnoreMutex { get; set; }

		/// <summary></summary>
		bool DisableValidation { get; set; }

		/// <summary></summary>
		bool RunGameEditor { get; set; }

		/// <summary></summary>
		bool DeleteGameStateFromMainHall { get; set; }

		/// <summary></summary>
		bool ConvertDatafileToMscorlib { get; set; }

		/// <summary></summary>
		Action<IDictionary<Type, Type>> LoadPortabilityClassMappings { get; set; }

		/// <summary></summary>
		StringBuilder Buf { get; set; }

		/// <summary></summary>
		IDatabase Database { get; }

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
		string LineSep { get; set; }

		/// <summary></summary>
		bool LineWrapUserInput { get; set; }

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
		IRecordDb<IGameState> GSDB { get; set; }

		/// <summary>
		/// Gets or sets a collection of functions used to resolve macros embedded in <see cref="IGameBase.Desc">Desc</see>, 
		/// <see cref="IArtifact">Artifact</see><see cref="IArtifact.StateDesc"> StateDesc</see> and <see cref="IMonster">Monster</see>
		/// <see cref="IMonster.StateDesc"> StateDesc</see> properties.
		/// </summary>
		IDictionary<long, Func<string>> MacroFuncs { get; set; }

		/// <summary></summary>
		Action<IRoom, IMonster, IArtifact, long, bool> RevealContainerContentsFunc { get; set; }

		/// <summary>
		/// Gets or sets an array of sentence prepositions (e.g., "to", "from", "inside", etc).
		/// </summary>
		IPrep[] Preps { get; set; }

		/// <summary>
		/// Gets or sets an array of sentence articles (e.g., "a", "some", "the", etc).
		/// </summary>
		string[] Articles { get; set; }

		/// <summary>
		/// Gets or sets a generic string representing an unknown name (e.g., "???").
		/// </summary>
		string UnknownName { get; set; }

		#endregion

		#region Public Methods

		/// <summary></summary>
		/// <param name="ex"></param>
		/// <param name="stackTraceFile"></param>
		/// <param name="errorMessage"></param>
		void HandleException(Exception ex, string stackTraceFile, string errorMessage);

		/// <summary></summary>
		void ResolvePortabilityClassMappings();

		/// <summary></summary>
		/// <param name="args"></param>
		void ProcessArgv(string[] args);

		/// <summary>
		/// Loads the <see cref="ClassMappingsDictionary">ClassMappingsDictionary</see> with all interface to class mapping Key/Value
		/// pairs necessary to allow the currently loaded Eamon CS plugin to run.
		/// </summary>
		/// <remarks>
		/// The mechanism used to accomplish this is easy to understand but somewhat hard to describe without providing a concrete
		/// example and referring you to other source code files to inspect their contents.  Let's take a look at the game Wrenhold's
		/// Secret Vigil.  Please inspect the following files in the order listed (they are also executed in this order by Eamon CS -
		/// a simplified call stack):
		/// <list type="bullet">
		/// <item>Adventures\WrenholdsSecretVigil\Game\Plugin\Engine.cs</item>
		/// <item>System\EamonRT\Game\Plugin\Engine.cs</item>
		/// <item>System\EamonDD\Game\Plugin\Engine.cs</item>
		/// <item>System\Eamon\Game\Plugin\Engine.cs</item>
		/// </list>
		/// All of these files contain LoadPluginClassMappings.  In the top three files, it is an override that calls into the level
		/// below by invoking base.LoadPluginClassMappings.  So it immediately drills down to the bottom of the stack (to the Eamon
		/// library).
		/// <para>
		/// Then things get interesting.  The lowest level calls
		/// <see cref="LoadPluginClassMappings01(Assembly)">LoadPluginClassMappings01</see> which probes the current Assembly (the
		/// Eamon library) using Reflection, looking for classes adorned with the
		/// <see cref="Game.Attributes.ClassMappingsAttribute">ClassMappings</see> attribute.  It then adds Key/Value pairs for all
		/// found classes (along with their matching interfaces) to the 
		/// <see cref="ClassMappingsDictionary">ClassMappingsDictionary</see>.  At this point, the call stack unwinds to the next
		/// higher level (EamonDD), and the overridden LoadPluginClassMappings method makes the same call to
		/// LoadPluginClassMappings01 which performs the same function, only this time it probes EamonDD for classes.  This process
		/// repeats until the stack is fully unwound and the original call to LoadPluginClassMappings (in Program.cs) returns.
		/// </para>
		/// <para>
		/// At each level, all newly discovered class/interface pairs are added immediately to the ClassMappingsDictionary; however,
		/// for any new class that maps to a pre-existing interface already in the Dictionary, that new class becomes the Value for
		/// that interface's Key/Value pair.  In other words, classes in higher layers override classes in lower layers when they map
		/// to the same interface.  This is how, for example, in Wrenhold's Secret Vigil, you wind up with
		/// EamonRT.Framework.Components.ICombatComponent mapping to WrenholdsSecretVigil.Game.Components.CombatComponent.  And why a
		/// call made to <see cref="CreateInstance{T}(Action{T})">CreateInstance</see> returns the game-specific CombatComponent object.
		/// </para>
		/// </remarks>
		/// <returns></returns>
		RetCode LoadPluginClassMappings();

		/// <summary>
		/// Probes the currently executing library/plugin (Assembly) for all classes adorned with the
		/// <see cref="Game.Attributes.ClassMappingsAttribute">ClassMappings</see> attribute, pairs each class with its corresponding
		/// interface, and updates the <see cref="ClassMappingsDictionary">ClassMappingsDictionary</see> accordingly.
		/// </summary>
		/// <remarks>
		/// This is the third and final piece of the Eamon CS dependency injection puzzle.  If you haven't done so, please review the
		/// remarks on <see cref="ClassMappingsDictionary">ClassMappingsDictionary</see> and
		/// <see cref="LoadPluginClassMappings">LoadPluginClassMappings</see> for necessary background information before reading these
		/// comments.  This method is responsible for probing the passed in library/plugin (in C# terms, an Assembly) for classes
		/// adorned with the <see cref="Game.Attributes.ClassMappingsAttribute">ClassMappings</see> attribute.  Then it iterates over
		/// each found class and obtains all ClassMappings attribute information for that class.  (While in theory, each class can
		/// have multiple ClassMappings attributes applied to it, this is not currently done.)  At this point, we know the class will
		/// be the Value part of a Key/Value pair, but we still don't know what interface to associate it with (the Key part).  This
		/// is decided based on which ClassMappings constructor is called.  If
		/// <see cref="Game.Attributes.ClassMappingsAttribute.ClassMappingsAttribute(Type)">this constructor</see> is used (e.g.,
		/// [ClassMappings(typeof(Eamon.Framework.IGameState))]) then the passed in interface is used as the Key.  Otherwise, for
		/// <see cref="Game.Attributes.ClassMappingsAttribute.ClassMappingsAttribute">this constructor</see> (e.g., [ClassMappings]),
		/// the name of the interface used as the Key will be synthesized by prepending "I" to the class name.  So, for example,
		/// CombatComponent pairs with ICombatComponent.  In this scenario, the class must always implement or inherit the interface;
		/// otherwise the system won't be able to find it.  If the interface to be used as a Key can't be found, Eamon CS will issue
		/// an error and terminate the program.  Assuming everything checks out, the class is then stored as the Value in the
		/// Dictionary with the interface as the Key.  If the interface already existed in the Dictionary (having been inserted by
		/// a lower level of the system) then the old class associated with the interface is replaced with the new class.  This allows
		/// lower-level interfaces to be associated with increasingly specialized higher-level classes, which is core to the functioning
		/// of Eamon CS.
		/// </remarks>
		/// <param name="plugin"></param>
		/// <returns></returns>
		RetCode LoadPluginClassMappings01(Assembly plugin);

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
		void ConvertDatafileFromXmlToDat(string fileName);

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
		/// <param name="resetCode"></param>
		void ResetProperties(PropertyResetCode resetCode);

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

		/// <summary>
		/// Gets the sentence preposition (e.g., "to", "from", "inside", etc).
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		IPrep GetPrep(long index);

		/// <summary>
		/// Gets the sentence article (e.g., "a", "some", "the", etc).
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		string GetArticle(long index);

		/// <summary></summary>
		/// <param name="index"></param>
		/// <returns></returns>
		string GetNumberString(long index);

		/// <summary></summary>
		/// <param name="index"></param>
		/// <returns></returns>
		string GetFieldDescName(long index);

		/// <summary></summary>
		/// <param name="fieldDesc"></param>
		/// <returns></returns>
		string GetFieldDescName(FieldDesc fieldDesc);

		/// <summary>
		/// Gets the name for a given <see cref="Status"/>.
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		string GetStatusName(long index);

		/// <summary>
		/// Gets the name for a given <see cref="Status"/>.
		/// </summary>
		/// <param name="status"></param>
		/// <returns></returns>
		string GetStatusName(Status status);

		/// <summary>
		/// Gets the name for a given <see cref="Clothing"/>.
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		string GetClothingName(long index);

		/// <summary>
		/// Gets the name for a given <see cref="Clothing"/>.
		/// </summary>
		/// <param name="clothing"></param>
		/// <returns></returns>
		string GetClothingName(Clothing clothing);

		/// <summary>
		/// Gets the description for a given <see cref="CombatCode"/>.
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		string GetCombatCodeDesc(long index);

		/// <summary>
		/// Gets the description for a given <see cref="CombatCode"/>.
		/// </summary>
		/// <param name="combatCode"></param>
		/// <returns></returns>
		string GetCombatCodeDesc(CombatCode combatCode);

		/// <summary></summary>
		/// <param name="index"></param>
		/// <returns></returns>
		string GetContainerDisplayCodeDesc(long index);

		/// <summary></summary>
		/// <param name="containerDisplayCode"></param>
		/// <returns></returns>
		string GetContainerDisplayCodeDesc(ContainerDisplayCode containerDisplayCode);

		/// <summary>
		/// Gets the name for a given <see cref="LightLevel"/>.
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		string GetLightLevelName(long index);

		/// <summary>
		/// Gets the name for a given <see cref="LightLevel"/>.
		/// </summary>
		/// <param name="lightLevel"></param>
		/// <returns></returns>
		string GetLightLevelName(LightLevel lightLevel);

		/// <summary>
		/// Gets the data for a given <see cref="Stat"/>.
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		IStat GetStat(long index);

		/// <summary>
		/// Gets the data for a given <see cref="Stat"/>.
		/// </summary>
		/// <param name="stat"></param>
		/// <returns></returns>
		IStat GetStat(Stat stat);

		/// <summary>
		/// Gets the data for a given <see cref="Spell"/>.
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		ISpell GetSpell(long index);

		/// <summary>
		/// Gets the data for a given <see cref="Spell"/>.
		/// </summary>
		/// <param name="spell"></param>
		/// <returns></returns>
		ISpell GetSpell(Spell spell);

		/// <summary>
		/// Gets the data for a given <see cref="Weapon"/>.
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		IWeapon GetWeapon(long index);

		/// <summary>
		/// Gets the data for a given <see cref="Weapon"/>.
		/// </summary>
		/// <param name="weapon"></param>
		/// <returns></returns>
		IWeapon GetWeapon(Weapon weapon);

		/// <summary>
		/// Gets the data for a given <see cref="Armor"/>.
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		IArmor GetArmor(long index);

		/// <summary>
		/// Gets the data for a given <see cref="Armor"/>.
		/// </summary>
		/// <param name="armor"></param>
		/// <returns></returns>
		IArmor GetArmor(Armor armor);

		/// <summary>
		/// Gets the data for a given <see cref="Direction"/>.
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		IDirection GetDirection(long index);

		/// <summary>
		/// Gets the data for a given <see cref="Direction"/>.
		/// </summary>
		/// <param name="direction"></param>
		/// <returns></returns>
		IDirection GetDirection(Direction direction);

		/// <summary>
		/// Gets the data for a given <see cref="ArtifactType"/>.
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		IArtifactType GetArtifactType(long index);

		/// <summary>
		/// Gets the data for a given <see cref="ArtifactType"/>.
		/// </summary>
		/// <param name="artifactType"></param>
		/// <returns></returns>
		IArtifactType GetArtifactType(ArtifactType artifactType);

		/// <summary>
		/// Indicates whether an operation succeeded.
		/// </summary>
		/// <param name="rc"></param>
		/// <returns></returns>
		bool IsSuccess(RetCode rc);

		/// <summary>
		/// Indicates whether an operation failed.
		/// </summary>
		/// <param name="rc"></param>
		/// <returns></returns>
		bool IsFailure(RetCode rc);

		/// <summary>
		/// Indicates whether a plural type is valid.
		/// </summary>
		/// <param name="pluralType"></param>
		/// <returns></returns>
		bool IsValidPluralType(PluralType pluralType);

		/// <summary>
		/// Indicates whether an artifact type is valid.
		/// </summary>
		/// <param name="artifactType"></param>
		/// <returns></returns>
		bool IsValidArtifactType(ArtifactType artifactType);

		/// <summary>
		/// Indicates whether an armor value is valid for an wearable <see cref="IArtifact">Artifact</see>.
		/// </summary>
		/// <param name="armor"></param>
		/// <returns></returns>
		bool IsValidArtifactArmor(long armor);

		/// <summary>
		/// Indicates whether an armor value is valid for a <see cref="IMonster">Monster</see>.
		/// </summary>
		/// <param name="armor"></param>
		/// <returns></returns>
		bool IsValidMonsterArmor(long armor);

		/// <summary>
		/// Indicates whether a courage value is valid for a <see cref="IMonster">Monster</see>.
		/// </summary>
		/// <param name="courage"></param>
		/// <returns></returns>
		bool IsValidMonsterCourage(long courage);

		/// <summary></summary>
		/// <param name="friendliness"></param>
		/// <returns></returns>
		bool IsValidMonsterFriendliness(Friendliness friendliness);

		/// <summary></summary>
		/// <param name="friendliness"></param>
		/// <returns></returns>
		bool IsValidMonsterFriendlinessPct(Friendliness friendliness);

		/// <summary></summary>
		/// <param name="dir"></param>
		/// <returns></returns>
		bool IsValidDirection(Direction dir);

		/// <summary></summary>
		/// <param name="roomUid"></param>
		/// <returns></returns>
		bool IsValidRoomUid01(long roomUid);

		/// <summary></summary>
		/// <param name="roomUid"></param>
		/// <returns></returns>
		bool IsValidRoomDirectionDoorUid01(long roomUid);

		/// <summary></summary>
		/// <param name="value"></param>
		/// <returns></returns>
		bool IsArtifactFieldStrength(long value);

		/// <summary></summary>
		/// <param name="weight"></param>
		/// <returns></returns>
		bool IsUnmovable(long weight);

		/// <summary></summary>
		/// <param name="weight"></param>
		/// <returns></returns>
		bool IsUnmovable01(long weight);

		/// <summary></summary>
		/// <param name="hardiness"></param>
		/// <returns></returns>
		long GetWeightCarryableGronds(long hardiness);

		/// <summary></summary>
		/// <param name="hardiness"></param>
		/// <returns></returns>
		long GetWeightCarryableDos(long hardiness);

		/// <summary></summary>
		/// <param name="intellect"></param>
		/// <returns></returns>
		long GetIntellectBonusPct(long intellect);

		/// <summary></summary>
		/// <param name="charisma"></param>
		/// <returns></returns>
		long GetCharmMonsterPct(long charisma);

		/// <summary></summary>
		/// <param name="pluralType"></param>
		/// <returns></returns>
		long GetPluralTypeEffectUid(PluralType pluralType);

		/// <summary></summary>
		/// <param name="armorUid"></param>
		/// <param name="shieldUid"></param>
		/// <returns></returns>
		long GetArmorFactor(long armorUid, long shieldUid);

		/// <summary></summary>
		/// <param name="charisma"></param>
		/// <returns></returns>
		long GetCharismaFactor(long charisma);

		/// <summary></summary>
		/// <param name="friendliness"></param>
		/// <returns></returns>
		long GetMonsterFriendlinessPct(Friendliness friendliness);

		/// <summary></summary>
		/// <param name="value"></param>
		/// <returns></returns>
		long GetArtifactFieldStrength(long value);

		/// <summary></summary>
		/// <param name="price"></param>
		/// <param name="rtio"></param>
		/// <returns></returns>
		long GetMerchantAskPrice(double price, double rtio);

		/// <summary></summary>
		/// <param name="price"></param>
		/// <param name="rtio"></param>
		/// <returns></returns>
		long GetMerchantBidPrice(double price, double rtio);

		/// <summary></summary>
		/// <param name="charisma"></param>
		/// <returns></returns>
		long GetMerchantAdjustedCharisma(long charisma);

		/// <summary></summary>
		/// <param name="charisma"></param>
		/// <returns></returns>
		double GetMerchantRtio(long charisma);

		/// <summary>
		/// Indicates whether a character is one of ['Y', 'N'].
		/// </summary>
		/// <param name="ch"></param>
		/// <returns></returns>
		bool IsCharYOrN(char ch);

		/// <summary>
		/// Indicates whether a character is one of ['S', 'T', 'R', 'X'].
		/// </summary>
		/// <param name="ch"></param>
		/// <returns></returns>
		bool IsCharSOrTOrROrX(char ch);

		/// <summary>
		/// Indicates whether a character is one of ['0', '1'].
		/// </summary>
		/// <param name="ch"></param>
		/// <returns></returns>
		bool IsChar0Or1(char ch);

		/// <summary>
		/// Indicates whether a character is one of ['0', '1', '2'].
		/// </summary>
		/// <param name="ch"></param>
		/// <returns></returns>
		bool IsChar0To2(char ch);

		/// <summary>
		/// Indicates whether a character is one of ['0', '1', '2', '3'].
		/// </summary>
		/// <param name="ch"></param>
		/// <returns></returns>
		bool IsChar0To3(char ch);

		/// <summary>
		/// Indicates whether a character is one of ['1', '2', '3'].
		/// </summary>
		/// <param name="ch"></param>
		/// <returns></returns>
		bool IsChar1To3(char ch);

		/// <summary>
		/// Indicates whether a character is a numeric digit.
		/// </summary>
		/// <param name="ch"></param>
		/// <returns></returns>
		bool IsCharDigit(char ch);

		/// <summary>
		/// Indicates whether a character is a numeric digit or 'X'.
		/// </summary>
		/// <param name="ch"></param>
		/// <returns></returns>
		bool IsCharDigitOrX(char ch);

		/// <summary>
		/// Indicates whether a character is a numeric digit or one of ['+', '-'].
		/// </summary>
		/// <param name="ch"></param>
		/// <returns></returns>
		bool IsCharPlusMinusDigit(char ch);

		/// <summary>
		/// Indicates whether a character is alphabetic.
		/// </summary>
		/// <param name="ch"></param>
		/// <returns></returns>
		bool IsCharAlpha(char ch);

		/// <summary>
		/// Indicates whether a character is alphabetic or space.
		/// </summary>
		/// <param name="ch"></param>
		/// <returns></returns>
		bool IsCharAlphaSpace(char ch);

		/// <summary>
		/// Indicates whether a character is alphabetic or numeric digit.
		/// </summary>
		/// <param name="ch"></param>
		/// <returns></returns>
		bool IsCharAlnum(char ch);

		/// <summary>
		/// Indicates whether a character is alphabetic, numeric digit or space.
		/// </summary>
		/// <param name="ch"></param>
		/// <returns></returns>
		bool IsCharAlnumSpace(char ch);

		/// <summary>
		/// Indicates whether a character is alphabetic, numeric digit, period or underscore.
		/// </summary>
		/// <param name="ch"></param>
		/// <returns></returns>
		bool IsCharAlnumPeriodUnderscore(char ch);

		/// <summary>
		/// Indicates whether a character is printable.
		/// </summary>
		/// <param name="ch"></param>
		/// <returns></returns>
		bool IsCharPrint(char ch);

		/// <summary>
		/// Indicates whether a character is '#'.
		/// </summary>
		/// <param name="ch"></param>
		/// <returns></returns>
		bool IsCharPound(char ch);

		/// <summary>
		/// Indicates whether a character is a quote.
		/// </summary>
		/// <param name="ch"></param>
		/// <returns></returns>
		bool IsCharQuote(char ch);

		/// <summary>
		/// Indicates whether a character is any character at all.
		/// </summary>
		/// <param name="ch"></param>
		/// <returns></returns>
		bool IsCharAny(char ch);

		/// <summary>
		/// Indicates whether a character is any character but one of ['"', ',', ':'].
		/// </summary>
		/// <param name="ch"></param>
		/// <returns></returns>
		bool IsCharAnyButDquoteCommaColon(char ch);

		/// <summary>
		/// Indicates whether a character is any character but one of ['\', '/'].
		/// </summary>
		/// <param name="ch"></param>
		/// <returns></returns>
		bool IsCharAnyButBackForwardSlash(char ch);

		/// <summary>
		/// Given a character, produce its upper case equivalent, if any.
		/// </summary>
		/// <param name="ch"></param>
		/// <returns></returns>
		char ModifyCharToUpper(char ch);

		/// <summary>
		/// Given a character, produce either 'X' or '\0'.
		/// </summary>
		/// <param name="ch"></param>
		/// <returns></returns>
		char ModifyCharToNullOrX(char ch);

		/// <summary>
		/// Given a character, produce '\0'.
		/// </summary>
		/// <param name="ch"></param>
		/// <returns></returns>
		char ModifyCharToNull(char ch);

		/// <summary></summary>
		/// <param name="directionName"></param>
		/// <returns></returns>
		Direction GetDirection(string directionName);

		/// <summary></summary>
		/// <param name="artifactType"></param>
		/// <returns></returns>
		ContainerType GetContainerType(ArtifactType artifactType);

		/// <summary></summary>
		/// <returns></returns>
		IConfig GetConfig();

		/// <summary>
		/// Gets the <see cref="IGameState">GameState</see> record.
		/// </summary>
		/// <returns></returns>
		IGameState GetGameState();

		/// <summary>
		/// Gets the <see cref="IModule">Module</see> record.
		/// </summary>
		/// <returns></returns>
		IModule GetModule();

		/// <summary></summary>
		/// <param name="array"></param>
		/// <param name="indexFunc"></param>
		/// <returns></returns>
		T GetRandomElement<T>(T[] array, Func<long> indexFunc = null);

		/// <summary>
		/// Evaluates the <see cref="Friendliness"/>, returning a value of type T.
		/// </summary>
		/// <param name="friendliness"></param>
		/// <param name="enemyValue"></param>
		/// <param name="neutralValue"></param>
		/// <param name="friendValue"></param>
		/// <returns></returns>
		T EvalFriendliness<T>(Friendliness friendliness, T enemyValue, T neutralValue, T friendValue);

		/// <summary>
		/// Evaluates the <see cref="Gender"/>, returning a value of type T.
		/// </summary>
		/// <param name="gender"></param>
		/// <param name="maleValue"></param>
		/// <param name="femaleValue"></param>
		/// <param name="neutralValue"></param>
		/// <returns></returns>
		T EvalGender<T>(Gender gender, T maleValue, T femaleValue, T neutralValue);

		/// <summary>
		/// Evaluates the <see cref="ContainerType"/>, returning a value of type T.
		/// </summary>
		/// <param name="containerType"></param>
		/// <param name="inValue"></param>
		/// <param name="onValue"></param>
		/// <param name="underValue"></param>
		/// <param name="behindValue"></param>
		/// <returns></returns>
		T EvalContainerType<T>(ContainerType containerType, T inValue, T onValue, T underValue, T behindValue);

		/// <summary>
		/// Evaluates the <see cref="RoomType"/>, returning a value of type T.
		/// </summary>
		/// <param name="roomType"></param>
		/// <param name="indoorsValue"></param>
		/// <param name="outdoorsValue"></param>
		/// <returns></returns>
		T EvalRoomType<T>(RoomType roomType, T indoorsValue, T outdoorsValue);

		/// <summary>
		/// Evaluates the <see cref="LightLevel"/>, returning a value of type T.
		/// </summary>
		/// <param name="lightLevel"></param>
		/// <param name="darkValue"></param>
		/// <param name="lightValue"></param>
		/// <returns></returns>
		T EvalLightLevel<T>(LightLevel lightLevel, T darkValue, T lightValue);

		/// <summary>
		/// Evaluates the plural value, returning a value of type T.
		/// </summary>
		/// <param name="isPlural"></param>
		/// <param name="singularValue"></param>
		/// <param name="pluralValue"></param>
		/// <returns></returns>
		T EvalPlural<T>(bool isPlural, T singularValue, T pluralValue);

		/// <summary></summary>
		/// <param name="bufSize"></param>
		/// <param name="fillChar"></param>
		/// <param name="number"></param>
		/// <param name="msg"></param>
		/// <param name="emptyVal"></param>
		/// <returns></returns>
		string BuildPrompt(long bufSize, char fillChar, long number, string msg, string emptyVal);

		/// <summary></summary>
		/// <param name="bufSize"></param>
		/// <param name="fillChar"></param>
		/// <param name="offset"></param>
		/// <param name="longVal"></param>
		/// <param name="stringVal"></param>
		/// <param name="lookupMsg"></param>
		/// <returns></returns>
		string BuildValue(long bufSize, char fillChar, long offset, long longVal, string stringVal, string lookupMsg);

		/// <summary></summary>
		/// <param name="str"></param>
		/// <param name="buf"></param>
		/// <param name="margin"></param>
		/// <param name="args"></param>
		/// <param name="clearBuf"></param>
		/// <returns></returns>
		string WordWrap(string str, StringBuilder buf, long margin, IWordWrapArgs args, bool clearBuf = true);

		/// <summary></summary>
		/// <param name="str"></param>
		/// <param name="buf"></param>
		/// <param name="clearBuf"></param>
		/// <returns></returns>
		string WordWrap(string str, StringBuilder buf, bool clearBuf = true);

		/// <summary></summary>
		/// <param name="str"></param>
		/// <param name="buf"></param>
		/// <param name="startColumn"></param>
		/// <param name="clearBuf"></param>
		/// <returns></returns>
		string LineWrap(string str, StringBuilder buf, long startColumn, bool clearBuf = true);

		/// <summary></summary>
		/// <param name="num"></param>
		/// <param name="addSpace"></param>
		/// <param name="buf"></param>
		/// <returns></returns>
		string GetStringFromNumber(long num, bool addSpace, StringBuilder buf);

		/// <summary></summary>
		/// <param name="str"></param>
		/// <returns></returns>
		long GetNumberFromString(string str);

		/// <summary>
		/// Rolls a number of dice, storing the resulting values in an array.
		/// </summary>
		/// <param name="numDice"></param>
		/// <param name="numSides"></param>
		/// <param name="dieRolls"></param>
		/// <returns></returns>
		RetCode RollDice(long numDice, long numSides, ref long[] dieRolls);

		/// <summary>
		/// Rolls a number of dice, returning a sum of the results.
		/// </summary>
		/// <param name="numDice"></param>
		/// <param name="numSides"></param>
		/// <param name="modifier"></param>
		/// <returns></returns>
		long RollDice(long numDice, long numSides, long modifier);

		/// <summary>
		/// Given an array of die rolls, sum the highest of them and return the result.
		/// </summary>
		/// <param name="dieRolls"></param>
		/// <param name="numRollsToSum"></param>
		/// <param name="result"></param>
		/// <returns></returns>
		RetCode SumHighestRolls(long[] dieRolls, long numRollsToSum, ref long result);

		/// <summary></summary>
		/// <param name="str"></param>
		/// <returns></returns>
		string Capitalize(string str);

		/// <summary>
		/// Deletes a set of game-related files from the filesystem.
		/// </summary>
		void UnlinkOnFailure();

		/// <summary></summary>
		/// <param name="pluralType"></param>
		/// <param name="maxSize"></param>
		void TruncatePluralTypeEffectDesc(PluralType pluralType, long maxSize);

		/// <summary></summary>
		/// <param name="effect"></param>
		void TruncatePluralTypeEffectDesc(IEffect effect);

		/// <summary></summary>
		/// <param name="fullPath"></param>
		/// <param name="directory"></param>
		/// <param name="fileName"></param>
		/// <param name="extension"></param>
		/// <param name="appendDirectorySeparatorChar"></param>
		/// <returns></returns>
		RetCode SplitPath(string fullPath, ref string directory, ref string fileName, ref string extension, bool appendDirectorySeparatorChar = true);

		/// <summary></summary>
		/// <param name="buf"></param>
		/// <param name="mySeen"></param>
		/// <returns></returns>
		RetCode StripPrepsAndArticles(StringBuilder buf, ref bool mySeen);

		/// <summary></summary>
		/// <param name="title"></param>
		/// <param name="inBox"></param>
		void PrintTitle(string title, bool inBox);

		/// <summary></summary>
		/// <param name="effect"></param>
		/// <param name="printFinalNewLine"></param>
		void PrintEffectDesc(IEffect effect, bool printFinalNewLine = true);

		/// <summary></summary>
		/// <param name="effectUid"></param>
		/// <param name="printFinalNewLine"></param>
		void PrintEffectDesc(long effectUid, bool printFinalNewLine = true);

		/// <summary>
		/// Prints the <see cref="Spell.Blast">Blast</see> spell description.
		/// </summary>
		/// <returns></returns>
		void PrintZapDirectHit();

		/// <summary></summary>
		/// <returns></returns>
		RetCode ValidateRecordsAfterDatabaseLoaded();

		/// <summary></summary>
		/// <param name="args"></param>
		/// <returns></returns>
		RetCode StatDisplay(IStatDisplayArgs args);

		/// <summary></summary>
		/// <param name="recordList"></param>
		/// <param name="args"></param>
		/// <param name="buf"></param>
		/// <returns></returns>
		RetCode GetRecordNameList(IList<IGameBase> recordList, IRecordNameListArgs args, StringBuilder buf);

		/// <summary></summary>
		/// <param name="recordList"></param>
		/// <param name="name"></param>
		/// <param name="exactMatch"></param>
		/// <param name="count"></param>
		/// <returns></returns>
		RetCode GetRecordNameCount(IList<IGameBase> recordList, string name, bool exactMatch, ref long count);

		/// <summary></summary>
		/// <param name="recordList"></param>
		/// <param name="capitalize"></param>
		/// <param name="showExtraInfo"></param>
		/// <param name="buf"></param>
		/// <returns></returns>
		RetCode ListRecords(IList<IGameBase> recordList, bool capitalize, bool showExtraInfo, StringBuilder buf);

		/// <summary></summary>
		/// <param name="str"></param>
		/// <param name="buf"></param>
		/// <param name="resolveFuncs"></param>
		/// <param name="recurse"></param>
		/// <param name="invalidUid"></param>
		/// <returns></returns>
		RetCode ResolveUidMacros(string str, StringBuilder buf, bool resolveFuncs, bool recurse, ref long invalidUid);

		/// <summary></summary>
		/// <param name="str"></param>
		/// <param name="buf"></param>
		/// <param name="resolveFuncs"></param>
		/// <param name="recurse"></param>
		/// <returns></returns>
		RetCode ResolveUidMacros(string str, StringBuilder buf, bool resolveFuncs, bool recurse);

		/// <summary></summary>
		/// <param name="name"></param>
		/// <param name="complexity"></param>
		/// <param name="type"></param>
		/// <param name="dice"></param>
		/// <param name="sides"></param>
		/// <param name="numHands"></param>
		/// <param name="calcPrice"></param>
		/// <param name="isMarcosWeapon"></param>
		/// <returns></returns>
		double GetWeaponPriceOrValue(string name, long complexity, Weapon type, long dice, long sides, long numHands, bool calcPrice, ref bool isMarcosWeapon);

		/// <summary></summary>
		/// <param name="weapon"></param>
		/// <param name="calcPrice"></param>
		/// <param name="isMarcosWeapon"></param>
		/// <returns></returns>
		double GetWeaponPriceOrValue(ICharacterArtifact weapon, bool calcPrice, ref bool isMarcosWeapon);

		/// <summary></summary>
		/// <param name="armor"></param>
		/// <param name="calcPrice"></param>
		/// <param name="isMarcosArmor"></param>
		/// <returns></returns>
		double GetArmorPriceOrValue(Armor armor, bool calcPrice, ref bool isMarcosArmor);

		/// <summary></summary>
		/// <param name="fieldDesc"></param>
		/// <param name="buf"></param>
		/// <param name="fullDesc"></param>
		/// <param name="briefDesc"></param>
		void AppendFieldDesc(FieldDesc fieldDesc, StringBuilder buf, StringBuilder fullDesc, StringBuilder briefDesc);

		/// <summary></summary>
		/// <param name="fieldDesc"></param>
		/// <param name="buf"></param>
		/// <param name="fullDesc"></param>
		/// <param name="briefDesc"></param>
		void AppendFieldDesc(FieldDesc fieldDesc, StringBuilder buf, string fullDesc, string briefDesc);

		/// <summary></summary>
		/// <param name="destCa"></param>
		/// <param name="sourceCa"></param>
		void CopyCharacterArtifactFields(ICharacterArtifact destCa, ICharacterArtifact sourceCa);

		/// <summary></summary>
		/// <param name="destAc"></param>
		/// <param name="sourceAc"></param>
		void CopyArtifactCategoryFields(IArtifactCategory destAc, IArtifactCategory sourceAc);

		/// <summary></summary>
		/// <param name="whereClauseFuncs"></param>
		/// <returns></returns>
		IList<IArtifact> GetArtifactList(params Func<IArtifact, bool>[] whereClauseFuncs);

		/// <summary></summary>
		/// <param name="whereClauseFuncs"></param>
		/// <returns></returns>
		IList<IMonster> GetMonsterList(params Func<IMonster, bool>[] whereClauseFuncs);

		/// <summary></summary>
		/// <param name="whereClauseFuncs"></param>
		/// <returns></returns>
		IList<IGameBase> GetRecordList(params Func<IGameBase, bool>[] whereClauseFuncs);

		/// <summary></summary>
		/// <param name="artifactList"></param>
		/// <param name="which"></param>
		/// <param name="whereClauseFunc"></param>
		/// <returns></returns>
		IArtifact GetNthArtifact(IList<IArtifact> artifactList, long which, Func<IArtifact, bool> whereClauseFunc);

		/// <summary></summary>
		/// <param name="monsterList"></param>
		/// <param name="which"></param>
		/// <param name="whereClauseFunc"></param>
		/// <returns></returns>
		IMonster GetNthMonster(IList<IMonster> monsterList, long which, Func<IMonster, bool> whereClauseFunc);

		/// <summary></summary>
		/// <param name="recordList"></param>
		/// <param name="which"></param>
		/// <param name="whereClauseFunc"></param>
		/// <returns></returns>
		IGameBase GetNthRecord(IList<IGameBase> recordList, long which, Func<IGameBase, bool> whereClauseFunc);

		/// <summary></summary>
		/// <param name="recordList"></param>
		void StripUniqueCharsFromRecordNames(IList<IGameBase> recordList);

		/// <summary></summary>
		/// <param name="recordList"></param>
		void AddUniqueCharsToRecordNames(IList<IGameBase> recordList);

		/// <summary></summary>
		/// <param name="artifact"></param>
		/// <param name="convertToGold"></param>
		void ConvertWeaponToGoldOrTreasure(IArtifact artifact, bool convertToGold);

		/// <summary></summary>
		/// <param name="artifact"></param>
		/// <param name="containerType"></param>
		void ConvertTreasureToContainer(IArtifact artifact, ContainerType containerType = ContainerType.In);

		/// <summary></summary>
		/// <param name="artifact"></param>
		/// <param name="containerType"></param>
		void ConvertContainerToTreasure(IArtifact artifact, ContainerType containerType = ContainerType.In);

		#endregion
	}
}
