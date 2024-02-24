
// Globals.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework;
using Eamon.Framework.DataStorage;
using Eamon.Framework.DataStorage.Generic;
using Eamon.Framework.Portability;

namespace EamonDD.Game.Plugin
{
#pragma warning disable IDE1006 // Naming Styles

	/// <inheritdoc cref="Eamon.Game.Plugin.Globals"/>
	public static class Globals
	{
		/// <inheritdoc cref="Eamon.Game.Plugin.Globals.gEngine"/>
		public static Framework.Plugin.IEngine gEngine
		{
			get
			{
				return (Framework.Plugin.IEngine)Eamon.Game.Plugin.Globals.gEngine;
			}
			set
			{
				Eamon.Game.Plugin.Globals.gEngine = value;
			}
		}

		/// <inheritdoc cref="Eamon.Game.Plugin.Globals.gOut"/>
		public static ITextWriter gOut 
		{
			get 
			{
				return Eamon.Game.Plugin.Globals.gOut;
			}
		}

		/// <inheritdoc cref="Eamon.Game.Plugin.Globals.gDatabase"/>
		public static IDatabase gDatabase
		{
			get
			{
				return Eamon.Game.Plugin.Globals.gDatabase;
			}
		}

		/// <inheritdoc cref="Eamon.Game.Plugin.Globals.gRDB"/>
		public static IRecordDb<IRoom> gRDB 
		{
			get 
			{
				return Eamon.Game.Plugin.Globals.gRDB;
			}
		}

		/// <inheritdoc cref="Eamon.Game.Plugin.Globals.gADB"/>
		public static IRecordDb<IArtifact> gADB 
		{
			get 
			{
				return Eamon.Game.Plugin.Globals.gADB;
			}
		}

		/// <inheritdoc cref="Eamon.Game.Plugin.Globals.gEDB"/>
		public static IRecordDb<IEffect> gEDB 
		{
			get 
			{
				return Eamon.Game.Plugin.Globals.gEDB;
			}
		}

		/// <inheritdoc cref="Eamon.Game.Plugin.Globals.gMDB"/>
		public static IRecordDb<IMonster> gMDB 
		{
			get 
			{
				return Eamon.Game.Plugin.Globals.gMDB;
			}
		}
	}

#pragma warning restore IDE1006 // Naming Styles
}
