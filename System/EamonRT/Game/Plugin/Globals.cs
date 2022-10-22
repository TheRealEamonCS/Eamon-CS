
// Globals.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework.DataStorage.Generic;
using Eamon.Framework.Portability;
using EamonRT.Framework;
using EamonRT.Framework.Parsing;
using EamonRT.Framework.Plugin;

namespace EamonRT.Game.Plugin
{
	public static class Globals
	{
		public static IEngine gEngine
		{
			get
			{
				return (IEngine)EamonDD.Game.Plugin.Globals.gEngine;
			}
			set
			{
				EamonDD.Game.Plugin.Globals.gEngine = value;
			}
		}

		public static ITextWriter gOut 
		{
			get 
			{
				return EamonDD.Game.Plugin.Globals.gOut;
			}
		}

		public static IRecordDb<Eamon.Framework.IRoom> gRDB 
		{
			get 
			{
				return EamonDD.Game.Plugin.Globals.gRDB;
			}
		}

		public static IRecordDb<Eamon.Framework.IArtifact> gADB 
		{
			get 
			{
				return EamonDD.Game.Plugin.Globals.gADB;
			}
		}

		public static IRecordDb<Eamon.Framework.IEffect> gEDB 
		{
			get 
			{
				return EamonDD.Game.Plugin.Globals.gEDB;
			}
		}

		public static IRecordDb<Eamon.Framework.IMonster> gMDB 
		{
			get 
			{
				return EamonDD.Game.Plugin.Globals.gMDB;
			}
		}

		public static ISentenceParser gSentenceParser
		{
			get
			{
				return gEngine?.SentenceParser;
			}
		}

		public static ICommandParser gCommandParser
		{
			get
			{
				return gEngine?.CommandParser;
			}
		}

		public static Eamon.Framework.IGameState gGameState
		{
			get 
			{
				return gEngine?.GameState;
			}
		}

		public static Eamon.Framework.ICharacter gCharacter
		{
			get
			{
				return gEngine?.Character;
			}
		}

		public static Eamon.Framework.IMonster gCharMonster
		{
			get
			{
				return gGameState != null ? gMDB[gGameState.Cm] : null;
			}
		}
	}
}
