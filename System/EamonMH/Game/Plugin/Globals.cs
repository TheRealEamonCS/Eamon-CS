
// Globals.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework.DataStorage;
using Eamon.Framework.DataStorage.Generic;
using Eamon.Framework.Portability;
using EamonMH.Framework.Plugin;

namespace EamonMH.Game.Plugin
{
	public static class Globals
	{
		public static IEngine gEngine
		{
			get
			{
				return (IEngine)Eamon.Game.Plugin.Globals.gEngine;
			}
			set
			{
				Eamon.Game.Plugin.Globals.gEngine = value;
			}
		}

		public static ITextWriter gOut 
		{
			get 
			{
				return Eamon.Game.Plugin.Globals.gOut;
			}
		}

		public static IDatabase gDatabase
		{
			get
			{
				return Eamon.Game.Plugin.Globals.gDatabase;
			}
		}

		public static IRecordDb<Eamon.Framework.IRoom> gRDB 
		{
			get 
			{
				return Eamon.Game.Plugin.Globals.gRDB;
			}
		}

		public static IRecordDb<Eamon.Framework.IArtifact> gADB 
		{
			get 
			{
				return Eamon.Game.Plugin.Globals.gADB;
			}
		}

		public static IRecordDb<Eamon.Framework.IEffect> gEDB 
		{
			get 
			{
				return Eamon.Game.Plugin.Globals.gEDB;
			}
		}

		public static IRecordDb<Eamon.Framework.IMonster> gMDB 
		{
			get 
			{
				return Eamon.Game.Plugin.Globals.gMDB;
			}
		}

		public static Eamon.Framework.ICharacter gCharacter
		{
			get
			{
				return gEngine.Character;
			}
		}
	}
}
