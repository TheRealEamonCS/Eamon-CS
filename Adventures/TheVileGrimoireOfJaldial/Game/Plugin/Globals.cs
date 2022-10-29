
// Globals.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework.DataStorage.Generic;
using Eamon.Framework.Portability;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Parsing;

namespace TheVileGrimoireOfJaldial.Game.Plugin
{
	public static class Globals
	{
		public static Framework.Plugin.IEngine gEngine
		{
			get
			{
				return (Framework.Plugin.IEngine)EamonRT.Game.Plugin.Globals.gEngine;
			}
			set
			{
				EamonRT.Game.Plugin.Globals.gEngine = value;
			}
		}

		public static ITextWriter gOut
		{
			get
			{
				return EamonRT.Game.Plugin.Globals.gOut;
			}
		}

		public static IRecordDb<Eamon.Framework.IRoom> gRDB
		{
			get
			{
				return (IRecordDb<Eamon.Framework.IRoom>)EamonRT.Game.Plugin.Globals.gRDB;
			}
		}

		public static IRecordDb<Eamon.Framework.IArtifact> gADB
		{
			get
			{
				return (IRecordDb<Eamon.Framework.IArtifact>)EamonRT.Game.Plugin.Globals.gADB;
			}
		}

		public static IRecordDb<Eamon.Framework.IEffect> gEDB
		{
			get
			{
				return (IRecordDb<Eamon.Framework.IEffect>)EamonRT.Game.Plugin.Globals.gEDB;
			}
		}

		public static IRecordDb<Eamon.Framework.IMonster> gMDB
		{
			get
			{
				return (IRecordDb<Eamon.Framework.IMonster>)EamonRT.Game.Plugin.Globals.gMDB;
			}
		}

		public static EamonRT.Framework.Parsing.ISentenceParser gSentenceParser
		{
			get
			{
				return (EamonRT.Framework.Parsing.ISentenceParser)EamonRT.Game.Plugin.Globals.gSentenceParser;
			}
		}

		public static EamonRT.Framework.Parsing.ICommandParser gCommandParser
		{
			get
			{
				return (EamonRT.Framework.Parsing.ICommandParser)EamonRT.Game.Plugin.Globals.gCommandParser;
			}
		}

		public static Framework.IGameState gGameState
		{
			get
			{
				return (Framework.IGameState)EamonRT.Game.Plugin.Globals.gGameState;
			}
		}

		public static Eamon.Framework.ICharacter gCharacter
		{
			get
			{
				return (Eamon.Framework.ICharacter)EamonRT.Game.Plugin.Globals.gCharacter;
			}
		}

		public static Framework.IMonster gCharMonster
		{
			get
			{
				return (Framework.IMonster)EamonRT.Game.Plugin.Globals.gCharMonster;
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
