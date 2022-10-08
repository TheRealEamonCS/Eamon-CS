
// IPluginClassMappings.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Eamon.Framework.Portability;

namespace Eamon.Framework.Plugin
{
	/// <summary></summary>
	public interface IPluginClassMappings
	{
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
		/// <item>Adventures\WrenholdsSecretVigil\Game\Plugin\PluginClassMappings.cs</item>
		/// <item>System\EamonRT\Game\Plugin\PluginClassMappings.cs</item>
		/// <item>System\EamonDD\Game\Plugin\PluginClassMappings.cs</item>
		/// <item>System\Eamon\Game\Plugin\PluginClassMappings.cs</item>
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
		/// [ClassMappings(typeof(Eamon.Framework.IEngine))]) then the passed in interface is used as the Key.  Otherwise, for
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
	}
}
