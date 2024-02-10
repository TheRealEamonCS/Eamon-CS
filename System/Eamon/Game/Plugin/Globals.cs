
// Globals.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework;
using Eamon.Framework.DataStorage;
using Eamon.Framework.DataStorage.Generic;
using Eamon.Framework.Portability;

namespace Eamon.Game.Plugin
{
#pragma warning disable IDE1006 // Naming Styles

	/// <summary></summary>
	/// <remarks></remarks>
	public static class Globals
	{
		/// <summary></summary>
		public static Framework.Plugin.IEngine gEngine { get; set; }

		/// <summary></summary>
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
