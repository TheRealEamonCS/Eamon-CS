
// PluginContext.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework.DataStorage.Generic;
using Eamon.Framework.Portability;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Parsing;

namespace TheVileGrimoireOfJaldial.Game.Plugin
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

		public static Framework.IMonster gCharMonster
		{
			get
			{
				return (Framework.IMonster)EamonRT.Game.Plugin.PluginContext.gCharMonster;
			}
		}

		public static Framework.IMonster gActorMonster(object obj)
		{
			if (obj is ICommandParser commandParser)
			{
				return (Framework.IMonster)commandParser?.ActorMonster;
			}
			else if (obj is ICommand command)
			{
				return (Framework.IMonster)command?.ActorMonster;
			}
			else
			{
				return null;
			}
		}

		public static Framework.IRoom gActorRoom(object obj)
		{
			if (obj is ICommandParser commandParser)
			{
				return (Framework.IRoom)commandParser?.ActorRoom;
			}
			else if (obj is ICommand command)
			{
				return (Framework.IRoom)command?.ActorRoom;
			}
			else
			{
				return null;
			}
		}

		public static Framework.IArtifact gDobjArtifact(object obj)
		{
			if (obj is ICommandParser commandParser)
			{
				return (Framework.IArtifact)commandParser?.DobjArtifact;
			}
			else if (obj is ICommand command)
			{
				return (Framework.IArtifact)command?.DobjArtifact;
			}
			else
			{
				return null;
			}
		}

		public static Framework.IMonster gDobjMonster(object obj)
		{
			if (obj is ICommandParser commandParser)
			{
				return (Framework.IMonster)commandParser?.DobjMonster;
			}
			else if (obj is ICommand command)
			{
				return (Framework.IMonster)command?.DobjMonster;
			}
			else
			{
				return null;
			}
		}

		public static Framework.IArtifact gIobjArtifact(object obj)
		{
			if (obj is ICommandParser commandParser)
			{
				return (Framework.IArtifact)commandParser?.IobjArtifact;
			}
			else if (obj is ICommand command)
			{
				return (Framework.IArtifact)command?.IobjArtifact;
			}
			else
			{
				return null;
			}
		}

		public static Framework.IMonster gIobjMonster(object obj)
		{
			if (obj is ICommandParser commandParser)
			{
				return (Framework.IMonster)commandParser?.IobjMonster;
			}
			else if (obj is ICommand command)
			{
				return (Framework.IMonster)command?.IobjMonster;
			}
			else
			{
				return null;
			}
		}
	}
}
