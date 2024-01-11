
// Globals.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework;
using Eamon.Framework.DataStorage;
using Eamon.Framework.DataStorage.Generic;
using Eamon.Framework.Plugin;
using Eamon.Framework.Portability;

namespace Eamon.Game.Plugin
{
	public static class Globals
	{
		public static IEngine gEngine { get; set; }

		public static ITextWriter gOut 
		{
			get 
			{
				return gEngine.Out;
			}
		}

		public static IDatabase gDatabase
		{
			get
			{
				return gEngine.Database;
			}
		}

		public static IRecordDb<IRoom> gRDB 
		{
			get 
			{
				return gEngine.RDB;
			}
		}

		public static IRecordDb<IArtifact> gADB 
		{
			get 
			{
				return gEngine.ADB;
			}
		}

		public static IRecordDb<IEffect> gEDB 
		{
			get 
			{
				return gEngine.EDB;
			}
		}

		public static IRecordDb<IMonster> gMDB 
		{
			get
			{
				return gEngine.MDB;
			}
		}
	}
}
