
// Globals.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework;
using Eamon.Framework.DataStorage;
using Eamon.Framework.DataStorage.Generic;
using Eamon.Framework.Plugin;
using Eamon.Framework.Portability;

namespace Eamon.Game.Plugin
{
#pragma warning disable IDE1006 // Naming Styles

	/// <summary></summary>
	/// <remarks></remarks>
	public static class Globals
	{
		/// <summary></summary>
		public static IEngine gEngine { get; set; }

		/// <summary></summary>
		public static ITextWriter gOut
		{
			get 
			{
				return gEngine.Out;
			}
		}

		/// <summary></summary>
		public static IDatabase gDatabase
		{
			get
			{
				return gEngine.Database;
			}
		}

		/// <summary>Gets the current <see cref="IRoom">Room</see> database.</summary>
		/// <remarks>You can get or set a specific Room by providing the Room <see cref="IGameBase.Uid">Uid</see> as an indexer.</remarks>
		public static IRecordDb<IRoom> gRDB
		{
			get 
			{
				return gEngine.RDB;
			}
		}

		/// <summary>Gets the current <see cref="IArtifact">Artifact</see> database.</summary>
		/// <remarks>You can get or set a specific Artifact by providing the Artifact <see cref="IGameBase.Uid">Uid</see> as an indexer.</remarks>
		public static IRecordDb<IArtifact> gADB
		{
			get 
			{
				return gEngine.ADB;
			}
		}

		/// <summary>Gets the current <see cref="IEffect">Effect</see> database.</summary>
		/// <remarks>You can get or set a specific Effect by providing the Effect <see cref="IGameBase.Uid">Uid</see> as an indexer.</remarks>
		public static IRecordDb<IEffect> gEDB
		{
			get 
			{
				return gEngine.EDB;
			}
		}

		/// <summary>Gets the current <see cref="IMonster">Monster</see> database.</summary>
		/// <remarks>You can get or set a specific Monster by providing the Monster <see cref="IGameBase.Uid">Uid</see> as an indexer.</remarks>
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
