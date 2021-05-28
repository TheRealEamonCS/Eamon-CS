
// PluginContext.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework.DataStorage.Generic;
using Eamon.Framework.Portability;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Parsing;

namespace TheDeepCanyon.Game.Plugin
{
	public static class PluginContext
	{
		public static Framework.Plugin.IPluginConstants Constants
		{
			get
			{
				return (Framework.Plugin.IPluginConstants)EamonRT.Game.Plugin.PluginContext.Constants;
			}
			set
			{
				EamonRT.Game.Plugin.PluginContext.Constants = value;
			}
		}

		public static Framework.Plugin.IPluginGlobals Globals
		{
			get
			{
				return (Framework.Plugin.IPluginGlobals)EamonRT.Game.Plugin.PluginContext.Globals;
			}
			set
			{
				EamonRT.Game.Plugin.PluginContext.Globals = value;
			}
		}

		public static ITextWriter gOut
		{
			get
			{
				return EamonRT.Game.Plugin.PluginContext.gOut;
			}
		}

		public static EamonRT.Framework.IEngine gEngine
		{
			get
			{
				return (EamonRT.Framework.IEngine)EamonRT.Game.Plugin.PluginContext.gEngine;
			}
		}

		public static IRecordDb<Eamon.Framework.IRoom> gRDB
		{
			get
			{
				return (IRecordDb<Eamon.Framework.IRoom>)EamonRT.Game.Plugin.PluginContext.gRDB;
			}
		}

		public static IRecordDb<Eamon.Framework.IArtifact> gADB
		{
			get
			{
				return (IRecordDb<Eamon.Framework.IArtifact>)EamonRT.Game.Plugin.PluginContext.gADB;
			}
		}

		public static IRecordDb<Eamon.Framework.IMonster> gMDB
		{
			get
			{
				return (IRecordDb<Eamon.Framework.IMonster>)EamonRT.Game.Plugin.PluginContext.gMDB;
			}
		}

		public static EamonRT.Framework.Parsing.ISentenceParser gSentenceParser
		{
			get
			{
				return (EamonRT.Framework.Parsing.ISentenceParser)EamonRT.Game.Plugin.PluginContext.gSentenceParser;
			}
		}

		public static Framework.IGameState gGameState
		{
			get
			{
				return (Framework.IGameState)EamonRT.Game.Plugin.PluginContext.gGameState;
			}
		}

		public static Eamon.Framework.ICharacter gCharacter
		{
			get
			{
				return (Eamon.Framework.ICharacter)EamonRT.Game.Plugin.PluginContext.gCharacter;
			}
		}

		public static Eamon.Framework.IMonster gCharMonster
		{
			get
			{
				return (Eamon.Framework.IMonster)EamonRT.Game.Plugin.PluginContext.gCharMonster;
			}
		}
	}
}
