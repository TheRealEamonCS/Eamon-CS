
// Globals.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework;
using Eamon.Framework.DataStorage.Generic;
using Eamon.Framework.Portability;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Parsing;

namespace RiddlesOfTheDuergarKingdom.Game.Plugin
{
#pragma warning disable IDE1006 // Naming Styles

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

		public static IRecordDb<IRoom> gRDB
		{
			get
			{
				return EamonRT.Game.Plugin.Globals.gRDB;
			}
		}

		public static IRecordDb<IArtifact> gADB
		{
			get
			{
				return EamonRT.Game.Plugin.Globals.gADB;
			}
		}

		public static IRecordDb<IEffect> gEDB
		{
			get
			{
				return EamonRT.Game.Plugin.Globals.gEDB;
			}
		}

		public static IRecordDb<IMonster> gMDB
		{
			get
			{
				return EamonRT.Game.Plugin.Globals.gMDB;
			}
		}

		public static ISentenceParser gSentenceParser
		{
			get
			{
				return EamonRT.Game.Plugin.Globals.gSentenceParser;
			}
		}

		public static ICommandParser gCommandParser
		{
			get
			{
				return EamonRT.Game.Plugin.Globals.gCommandParser;
			}
		}

		public static Framework.IGameState gGameState
		{
			get
			{
				return (Framework.IGameState)EamonRT.Game.Plugin.Globals.gGameState;
			}
		}

		public static ICharacter gCharacter
		{
			get
			{
				return EamonRT.Game.Plugin.Globals.gCharacter;
			}
		}

		public static IMonster gCharMonster
		{
			get
			{
				return EamonRT.Game.Plugin.Globals.gCharMonster;
			}
		}

		public static IMonster gActorMonster(object obj)
		{
			if (obj is ICommandParser commandParser)
			{
				return (IMonster)commandParser?.ActorMonster;
			}
			else if (obj is ICommand command)
			{
				return (IMonster)command?.ActorMonster;
			}
			else
			{
				return null;
			}
		}

		public static IRoom gActorRoom(object obj)
		{
			if (obj is ICommandParser commandParser)
			{
				return (IRoom)commandParser?.ActorRoom;
			}
			else if (obj is ICommand command)
			{
				return (IRoom)command?.ActorRoom;
			}
			else
			{
				return null;
			}
		}

		public static IArtifact gDobjArtifact(object obj)
		{
			if (obj is ICommandParser commandParser)
			{
				return (IArtifact)commandParser?.DobjArtifact;
			}
			else if (obj is ICommand command)
			{
				return (IArtifact)command?.DobjArtifact;
			}
			else
			{
				return null;
			}
		}

		public static IMonster gDobjMonster(object obj)
		{
			if (obj is ICommandParser commandParser)
			{
				return (IMonster)commandParser?.DobjMonster;
			}
			else if (obj is ICommand command)
			{
				return (IMonster)command?.DobjMonster;
			}
			else
			{
				return null;
			}
		}

		public static IArtifact gIobjArtifact(object obj)
		{
			if (obj is ICommandParser commandParser)
			{
				return (IArtifact)commandParser?.IobjArtifact;
			}
			else if (obj is ICommand command)
			{
				return (IArtifact)command?.IobjArtifact;
			}
			else
			{
				return null;
			}
		}

		public static IMonster gIobjMonster(object obj)
		{
			if (obj is ICommandParser commandParser)
			{
				return (IMonster)commandParser?.IobjMonster;
			}
			else if (obj is ICommand command)
			{
				return (IMonster)command?.IobjMonster;
			}
			else
			{
				return null;
			}
		}
	}

#pragma warning restore IDE1006 // Naming Styles
}
