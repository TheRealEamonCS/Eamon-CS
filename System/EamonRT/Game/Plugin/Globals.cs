
// Globals.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework;
using Eamon.Framework.DataStorage;
using Eamon.Framework.DataStorage.Generic;
using Eamon.Framework.Portability;
using EamonRT.Framework.Commands;
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

		/// <summary>
		/// Gets the game sentence parser responsible for splitting full sentence commands from the player.
		/// </summary>
		/// <remarks>
		/// Transforms full sentence commands into simpler instructions for the legacy <see cref="ICommandParser">CommandParser</see>.
		/// When the EnhancedParser game setting is enabled, a single instance created during startup is reused throughout gameplay for
		/// improved performance.
		/// </remarks>
		public static ISentenceParser gSentenceParser
		{
			get
			{
				return gEngine.SentenceParser;
			}
		}

		/// <summary>
		/// Gets the game command parser responsible for parsing commands from the player.
		/// </summary>
		/// <remarks>
		/// Provides parsing for traditional Eamon commands of the form "VERB [DOBJ [PREP IOBJ]]". Depending on the EnhancedParser
		/// game setting, it takes its input either from a <see cref="ISentenceParser">SentenceParser</see> or directly from the player.
		/// A single instance is created during startup and reused throughout gameplay for improved performance.
		/// </remarks>
		public static ICommandParser gCommandParser
		{
			get
			{
				return gEngine.CommandParser;
			}
		}

		/// <summary>Gets the game state for the current game.</summary>
		/// <remarks>
		/// Provides access to methods and data both universal to all games and specific to the current one. It allows
		/// for retrieval and modification of the game state as needed during runtime. The data referenced by this
		/// property persists between game sessions through <see cref="ISaveCommand">Save</see> and
		/// <see cref="IRestoreCommand">Restore</see> operations.
		/// </remarks>
		public static IGameState gGameState
		{
			get 
			{
				return gEngine.GameState;
			}
		}

		/// <summary>Gets the player character being used in the current game.</summary>
		/// <remarks>
		/// The player character is selected in the Main Hall and imported into the game using the FRESHMEAT.DAT file. A
		/// <see cref="IMonster">Monster</see> record is built with key pieces of data and used during game play, referenced by
		/// <see cref="gCharMonster">gCharMonster</see>. Generally, after that the two records are synchronized as needed, and
		/// the character record is accessed or updated for data not stored in the Monster record. When the game ends,
		/// FRESHMEAT.DAT is updated before the player character is sent back to the Main Hall.
		/// </remarks>
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
