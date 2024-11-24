
// Globals.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework;
using Eamon.Framework.DataStorage;
using Eamon.Framework.DataStorage.Generic;
using Eamon.Framework.Portability;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Parsing;

namespace Dharmaquest.Game.Plugin
{
#pragma warning disable IDE1006 // Naming Styles

	/// <inheritdoc cref="EamonRT.Game.Plugin.Globals"/>
	public static class Globals
	{
		/// <inheritdoc cref="EamonRT.Game.Plugin.Globals.gEngine"/>
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

		/// <inheritdoc cref="EamonRT.Game.Plugin.Globals.gOut"/>
		public static ITextWriter gOut
		{
			get
			{
				return (ITextWriter)EamonRT.Game.Plugin.Globals.gOut;
			}
		}

		/// <inheritdoc cref="EamonRT.Game.Plugin.Globals.gDatabase"/>
		public static IDatabase gDatabase
		{
			get
			{
				return (IDatabase)EamonRT.Game.Plugin.Globals.gDatabase;
			}
		}

		/// <inheritdoc cref="EamonRT.Game.Plugin.Globals.gRDB"/>
		public static IRecordDb<IRoom> gRDB
		{
			get
			{
				return EamonRT.Game.Plugin.Globals.gRDB;
			}
		}

		/// <inheritdoc cref="EamonRT.Game.Plugin.Globals.gADB"/>
		public static IRecordDb<IArtifact> gADB
		{
			get
			{
				return EamonRT.Game.Plugin.Globals.gADB;
			}
		}

		/// <inheritdoc cref="EamonRT.Game.Plugin.Globals.gEDB"/>
		public static IRecordDb<IEffect> gEDB
		{
			get
			{
				return EamonRT.Game.Plugin.Globals.gEDB;
			}
		}

		/// <inheritdoc cref="EamonRT.Game.Plugin.Globals.gMDB"/>
		public static IRecordDb<IMonster> gMDB
		{
			get
			{
				return EamonRT.Game.Plugin.Globals.gMDB;
			}
		}

		/// <inheritdoc cref="EamonRT.Game.Plugin.Globals.gSentenceParser"/>
		public static ISentenceParser gSentenceParser
		{
			get
			{
				return (ISentenceParser)EamonRT.Game.Plugin.Globals.gSentenceParser;
			}
		}

		/// <inheritdoc cref="EamonRT.Game.Plugin.Globals.gCommandParser"/>
		public static ICommandParser gCommandParser
		{
			get
			{
				return (ICommandParser)EamonRT.Game.Plugin.Globals.gCommandParser;
			}
		}

		/// <inheritdoc cref="EamonRT.Game.Plugin.Globals.gGameState"/>
		public static Framework.IGameState gGameState
		{
			get
			{
				return (Framework.IGameState)EamonRT.Game.Plugin.Globals.gGameState;
			}
		}

		/// <inheritdoc cref="EamonRT.Game.Plugin.Globals.gCharacter"/>
		public static ICharacter gCharacter
		{
			get
			{
				return (ICharacter)EamonRT.Game.Plugin.Globals.gCharacter;
			}
		}

		/// <inheritdoc cref="EamonRT.Game.Plugin.Globals.gCharMonster"/>
		public static IMonster gCharMonster
		{
			get
			{
				return (IMonster)EamonRT.Game.Plugin.Globals.gCharMonster;
			}
		}

		/// <inheritdoc cref="EamonRT.Game.Plugin.Globals.gCharRoom"/>
		public static IRoom gCharRoom
		{
			get
			{
				return (IRoom)EamonRT.Game.Plugin.Globals.gCharRoom;
			}
		}

		/// <summary>Gets the ActorMonster property of an object as a game-specific interface.</summary>
		/// <param name="obj">The object with the ActorMonster property.</param>
		/// <remarks>
		/// Use only within <see cref="ICommandParser">CommandParser</see> and <see cref="ICommand">Command</see> classes or
		/// their derivatives, with the "this" keyword as a parameter. It allows easy access to new methods and properties in
		/// a game-specific <see cref="IMonster"/> interface. Adjust the return type or remove this method if no such interface
		/// exists.
		/// </remarks>
		/// <returns>The ActorMonster downcast to a game-specific interface.</returns>
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

		/// <summary>Gets the ActorRoom property of an object as a game-specific interface.</summary>
		/// <param name="obj">The object with the ActorRoom property.</param>
		/// <remarks>
		/// Use only within <see cref="ICommandParser">CommandParser</see> and <see cref="ICommand">Command</see> classes or
		/// their derivatives, with the "this" keyword as a parameter. It allows easy access to new methods and properties in
		/// a game-specific <see cref="IRoom"/> interface. Adjust the return type or remove this method if no such interface
		/// exists.
		/// </remarks>
		/// <returns>The ActorRoom downcast to a game-specific interface.</returns>
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

		/// <summary>Gets the DobjArtifact property of an object as a game-specific interface.</summary>
		/// <param name="obj">The object with the DobjArtifact property.</param>
		/// <remarks>
		/// Use only within <see cref="ICommandParser">CommandParser</see> and <see cref="ICommand">Command</see> classes or
		/// their derivatives, with the "this" keyword as a parameter. It allows easy access to new methods and properties in
		/// a game-specific <see cref="IArtifact"/> interface. Adjust the return type or remove this method if no such interface
		/// exists.
		/// </remarks>
		/// <returns>The DobjArtifact downcast to a game-specific interface.</returns>
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

		/// <summary>Gets the DobjMonster property of an object as a game-specific interface.</summary>
		/// <param name="obj">The object with the DobjMonster property.</param>
		/// <remarks>
		/// Use only within <see cref="ICommandParser">CommandParser</see> and <see cref="ICommand">Command</see> classes or
		/// their derivatives, with the "this" keyword as a parameter. It allows easy access to new methods and properties in
		/// a game-specific <see cref="IMonster"/> interface. Adjust the return type or remove this method if no such interface
		/// exists.
		/// </remarks>
		/// <returns>The DobjMonster downcast to a game-specific interface.</returns>
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

		/// <summary>Gets the IobjArtifact property of an object as a game-specific interface.</summary>
		/// <param name="obj">The object with the IobjArtifact property.</param>
		/// <remarks>
		/// Use only within <see cref="ICommandParser">CommandParser</see> and <see cref="ICommand">Command</see> classes or
		/// their derivatives, with the "this" keyword as a parameter. It allows easy access to new methods and properties in
		/// a game-specific <see cref="IArtifact"/> interface. Adjust the return type or remove this method if no such interface
		/// exists.
		/// </remarks>
		/// <returns>The IobjArtifact downcast to a game-specific interface.</returns>
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

		/// <summary>Gets the IobjMonster property of an object as a game-specific interface.</summary>
		/// <param name="obj">The object with the IobjMonster property.</param>
		/// <remarks>
		/// Use only within <see cref="ICommandParser">CommandParser</see> and <see cref="ICommand">Command</see> classes or
		/// their derivatives, with the "this" keyword as a parameter. It allows easy access to new methods and properties in
		/// a game-specific <see cref="IMonster"/> interface. Adjust the return type or remove this method if no such interface
		/// exists.
		/// </remarks>
		/// <returns>The IobjMonster downcast to a game-specific interface.</returns>
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
