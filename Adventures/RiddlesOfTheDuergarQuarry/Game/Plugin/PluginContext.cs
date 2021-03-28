
// PluginContext.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework.DataStorage.Generic;
using Eamon.Framework.Portability;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Parsing;

namespace RiddlesOfTheDuergarQuarry.Game.Plugin
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

		public static Framework.Plugin.IPluginClassMappings ClassMappings
		{
			get
			{
				return (Framework.Plugin.IPluginClassMappings)EamonRT.Game.Plugin.PluginContext.ClassMappings;
			}
			set
			{
				EamonRT.Game.Plugin.PluginContext.ClassMappings = value;
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

		public static IRecordDb<Eamon.Framework.IEffect> gEDB
		{
			get
			{
				return (IRecordDb<Eamon.Framework.IEffect>)EamonRT.Game.Plugin.PluginContext.gEDB;
			}
		}

		public static IRecordDb<Eamon.Framework.IMonster> gMDB
		{
			get
			{
				return (IRecordDb<Eamon.Framework.IMonster>)EamonRT.Game.Plugin.PluginContext.gMDB;
			}
		}

		public static IRecordDb<Eamon.Framework.ITrigger> gTDB
		{
			get
			{
				return (IRecordDb<Eamon.Framework.ITrigger>)EamonRT.Game.Plugin.PluginContext.gTDB;
			}
		}

		public static IRecordDb<Eamon.Framework.IScript> gSDB
		{
			get
			{
				return (IRecordDb<Eamon.Framework.IScript>)EamonRT.Game.Plugin.PluginContext.gSDB;
			}
		}

		public static EamonRT.Framework.Parsing.ISentenceParser gSentenceParser
		{
			get
			{
				return (EamonRT.Framework.Parsing.ISentenceParser)EamonRT.Game.Plugin.PluginContext.gSentenceParser;
			}
		}

		public static EamonRT.Framework.Parsing.ICommandParser gCommandParser
		{
			get
			{
				return (EamonRT.Framework.Parsing.ICommandParser)EamonRT.Game.Plugin.PluginContext.gCommandParser;
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

		public static Eamon.Framework.IMonster gActorMonster(object obj)
		{
			if (obj is ICommandParser commandParser)
			{
				return (Eamon.Framework.IMonster)commandParser?.ActorMonster;
			}
			else if (obj is ICommand command)
			{
				return (Eamon.Framework.IMonster)command?.ActorMonster;
			}
			else
			{
				return null;
			}
		}

		public static Eamon.Framework.IRoom gActorRoom(object obj)
		{
			if (obj is ICommandParser commandParser)
			{
				return (Eamon.Framework.IRoom)commandParser?.ActorRoom;
			}
			else if (obj is ICommand command)
			{
				return (Eamon.Framework.IRoom)command?.ActorRoom;
			}
			else
			{
				return null;
			}
		}

		public static Eamon.Framework.IArtifact gDobjArtifact(object obj)
		{
			if (obj is ICommandParser commandParser)
			{
				return (Eamon.Framework.IArtifact)commandParser?.DobjArtifact;
			}
			else if (obj is ICommand command)
			{
				return (Eamon.Framework.IArtifact)command?.DobjArtifact;
			}
			else
			{
				return null;
			}
		}

		public static Eamon.Framework.IMonster gDobjMonster(object obj)
		{
			if (obj is ICommandParser commandParser)
			{
				return (Eamon.Framework.IMonster)commandParser?.DobjMonster;
			}
			else if (obj is ICommand command)
			{
				return (Eamon.Framework.IMonster)command?.DobjMonster;
			}
			else
			{
				return null;
			}
		}

		public static Eamon.Framework.IArtifact gIobjArtifact(object obj)
		{
			if (obj is ICommandParser commandParser)
			{
				return (Eamon.Framework.IArtifact)commandParser?.IobjArtifact;
			}
			else if (obj is ICommand command)
			{
				return (Eamon.Framework.IArtifact)command?.IobjArtifact;
			}
			else
			{
				return null;
			}
		}

		public static Eamon.Framework.IMonster gIobjMonster(object obj)
		{
			if (obj is ICommandParser commandParser)
			{
				return (Eamon.Framework.IMonster)commandParser?.IobjMonster;
			}
			else if (obj is ICommand command)
			{
				return (Eamon.Framework.IMonster)command?.IobjMonster;
			}
			else
			{
				return null;
			}
		}
	}
}
