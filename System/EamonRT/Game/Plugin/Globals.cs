
// Globals.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework;
using Eamon.Framework.DataStorage;
using Eamon.Framework.DataStorage.Generic;
using Eamon.Framework.Portability;
using EamonRT.Framework.Parsing;

namespace EamonRT.Game.Plugin
{
#pragma warning disable IDE1006 // Naming Styles

	/// <inheritdoc cref="EamonDD.Game.Plugin.Globals"/>
	public static class Globals
	{
		/// <inheritdoc cref="EamonDD.Game.Plugin.Globals.gEngine"/>
		public static Framework.Plugin.IEngine gEngine
		{
			get
			{
				return (Framework.Plugin.IEngine)EamonDD.Game.Plugin.Globals.gEngine;
			}
			set
			{
				EamonDD.Game.Plugin.Globals.gEngine = value;
			}
		}

		/// <inheritdoc cref="EamonDD.Game.Plugin.Globals.gOut"/>
		public static ITextWriter gOut 
		{
			get 
			{
				return EamonDD.Game.Plugin.Globals.gOut;
			}
		}

		/// <inheritdoc cref="EamonDD.Game.Plugin.Globals.gDatabase"/>
		public static IDatabase gDatabase
		{
			get
			{
				return EamonDD.Game.Plugin.Globals.gDatabase;
			}
		}

		/// <inheritdoc cref="EamonDD.Game.Plugin.Globals.gRDB"/>
		public static IRecordDb<IRoom> gRDB 
		{
			get 
			{
				return EamonDD.Game.Plugin.Globals.gRDB;
			}
		}

		/// <inheritdoc cref="EamonDD.Game.Plugin.Globals.gADB"/>
		public static IRecordDb<IArtifact> gADB 
		{
			get 
			{
				return EamonDD.Game.Plugin.Globals.gADB;
			}
		}

		/// <inheritdoc cref="EamonDD.Game.Plugin.Globals.gEDB"/>
		public static IRecordDb<IEffect> gEDB 
		{
			get 
			{
				return EamonDD.Game.Plugin.Globals.gEDB;
			}
		}

		/// <inheritdoc cref="EamonDD.Game.Plugin.Globals.gMDB"/>
		public static IRecordDb<IMonster> gMDB 
		{
			get 
			{
				return EamonDD.Game.Plugin.Globals.gMDB;
			}
		}

		/// <summary></summary>
		/// <remarks></remarks>
		public static ISentenceParser gSentenceParser
		{
			get
			{
				return gEngine.SentenceParser;
			}
		}

		/// <summary></summary>
		/// <remarks></remarks>
		public static ICommandParser gCommandParser
		{
			get
			{
				return gEngine.CommandParser;
			}
		}

		/// <summary></summary>
		/// <remarks></remarks>
		public static IGameState gGameState
		{
			get 
			{
				return gEngine.GameState;
			}
		}

		/// <summary></summary>
		/// <remarks></remarks>
		public static ICharacter gCharacter
		{
			get
			{
				return gEngine.Character;
			}
		}

		/// <summary>
		/// Gets the <see cref="IMonster">Monster</see> in the game corresponding to the player
		/// character, if available.
		/// </summary>
		/// <remarks>
		/// Returns the corresponding Monster when the <see cref="IGameState.Cm">GameState.Cm</see> property is available and valid;
		/// otherwise, returns null.
		/// </remarks>
		public static IMonster gCharMonster
		{
			get
			{
				return gGameState != null ? gMDB[gGameState.Cm] : null;
			}
		}

		/// <summary>
		/// Gets the <see cref="IRoom">Room</see> in the game where the player character is
		/// currently located, if available.
		/// </summary>
		/// <remarks>
		/// Returns the corresponding Room when the <see cref="IGameState.Ro">GameState.Ro</see> property is available and valid;
		/// otherwise, returns null.
		/// </remarks>
		public static IRoom gCharRoom
		{
			get
			{
				return gGameState != null ? gRDB[gGameState.Ro] : null;
			}
		}
	}

#pragma warning restore IDE1006 // Naming Styles
}
