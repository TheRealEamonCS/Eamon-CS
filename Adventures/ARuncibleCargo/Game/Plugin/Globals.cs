
// Globals.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework;
using Eamon.Framework.DataStorage;
using Eamon.Framework.DataStorage.Generic;
using Eamon.Framework.Portability;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Parsing;

namespace ARuncibleCargo.Game.Plugin
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
				return EamonRT.Game.Plugin.Globals.gOut;
			}
		}

		/// <inheritdoc cref="EamonRT.Game.Plugin.Globals.gDatabase"/>
		public static IDatabase gDatabase
		{
			get
			{
				return EamonRT.Game.Plugin.Globals.gDatabase;
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
				return EamonRT.Game.Plugin.Globals.gCharacter;
			}
		}

		/// <inheritdoc cref="EamonRT.Game.Plugin.Globals.gCharMonster"/>
		public static IMonster gCharMonster
		{
			get
			{
				return EamonRT.Game.Plugin.Globals.gCharMonster;
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
	}

#pragma warning restore IDE1006 // Naming Styles
}
