
// Globals.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework;
using Eamon.Framework.DataStorage;
using Eamon.Framework.DataStorage.Generic;
using Eamon.Framework.Portability;

namespace Eamon.Game.Plugin
{
#pragma warning disable IDE1006 // Naming Styles

	/// <summary>
	/// The global variables in this class are utilized throughout Eamon CS to manage game state, provide quick
	/// access to key properties, and facilitate communication between different components of the game engine.
	/// </summary>
	/// <remarks>
	/// The key property, <see cref="gEngine">gEngine</see>, is technically the top of the <see cref="ContextStack.EngineStack">EngineStack</see>,
	/// although it is not stored in the stack itself. All others are quick accessors to various gEngine properties. The Globals classes in higher
	/// levels of the game engine thunk down to expose lower level Globals properties, while often adding additional properties of their own.
	/// </remarks>
	public static class Globals
	{
        /// <summary>
        /// Gets or sets the current Eamon CS engine instance which serves as the primary entry point for accessing and managing
        /// various aspects of the system.
        /// </summary>
        /// <remarks>
        /// This property is pivotal in Eamon CS. It is comparable to the root node of a garbage collection tree, where all game-related
        /// objects (except certain static classes) are accessible either directly or through indirect references. The vast list of
        /// properties and methods on this class can be expanded to include game-specific content. When seeking specific
        /// functionality or data within Eamon CS, the recommendation is to explore its capabilities first.
        /// </remarks>
        public static Framework.Plugin.IEngine gEngine { get; set; }

        /// <summary>Gets the object that outputs text to the player's display device.</summary>
        /// <remarks>
        /// Text is displayed differently on various devices; in Eamon CS Desktop, it goes directly to the console, while in Eamon CS
		/// Mobile, it's appended to a text label. Despite the variation, the output maintains a consistent aesthetic for text
		/// adventures. The object features properties and methods to control its behavior.
        /// </remarks>
        public static ITextWriter gOut
		{
			get 
			{
				return gEngine.Out;
			}
		}

		/// <summary>Gets the current global database.</summary>
		/// <remarks>
		/// All game Records reside in this global database, which can be used either directly or through various quick accessors
		/// (such as <see cref="gRDB">gRDB</see>, etc). The database also exports a variety of services via its many methods.
		/// </remarks>
		public static IDatabase gDatabase
		{
			get
			{
				return gEngine.Database;
			}
		}

		/// <summary>Gets the current <see cref="IRoom">Room</see> database.</summary>
		/// <remarks>
		/// The <see cref="IRecordDb{T}.Records">Records</see> property provides access to the Room collection. Additionally, you can 
		/// get or set a specific Room by using its <see cref="IGameBase.Uid">Uid</see> as an indexer.
		/// </remarks>
		public static IRecordDb<IRoom> gRDB
		{
			get 
			{
				return gEngine.RDB;
			}
		}

		/// <summary>Gets the current <see cref="IArtifact">Artifact</see> database.</summary>
		/// <remarks>
		/// The <see cref="IRecordDb{T}.Records">Records</see> property provides access to the Artifact collection. Additionally, you can 
		/// get or set a specific Artifact by using its <see cref="IGameBase.Uid">Uid</see> as an indexer.
		/// </remarks>
		public static IRecordDb<IArtifact> gADB
		{
			get 
			{
				return gEngine.ADB;
			}
		}

		/// <summary>Gets the current <see cref="IEffect">Effect</see> database.</summary>
		/// <remarks>
		/// The <see cref="IRecordDb{T}.Records">Records</see> property provides access to the Effect collection. Additionally, you can 
		/// get or set a specific Effect by using its <see cref="IGameBase.Uid">Uid</see> as an indexer.
		/// </remarks>
		public static IRecordDb<IEffect> gEDB
		{
			get 
			{
				return gEngine.EDB;
			}
		}

		/// <summary>Gets the current <see cref="IMonster">Monster</see> database.</summary>
		/// <remarks>
		/// The <see cref="IRecordDb{T}.Records">Records</see> property provides access to the Monster collection. Additionally, you can 
		/// get or set a specific Monster by using its <see cref="IGameBase.Uid">Uid</see> as an indexer.
		/// </remarks>
		public static IRecordDb<IMonster> gMDB
		{
			get
			{
				return gEngine.MDB;
			}
		}
	}

#pragma warning restore IDE1006 // Naming Styles
}
