
// Globals.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework;
using Eamon.Framework.DataStorage;
using Eamon.Framework.DataStorage.Generic;
using Eamon.Framework.Portability;

namespace EamonMH.Game.Plugin
{
#pragma warning disable IDE1006 // Naming Styles

	/// <inheritdoc cref="Eamon.Game.Plugin.Globals"/>
	public static class Globals
	{
		/// <inheritdoc cref="Eamon.Game.Plugin.Globals.gEngine"/>
		public static Framework.Plugin.IEngine gEngine
		{
			get
			{
				return (Framework.Plugin.IEngine)Eamon.Game.Plugin.Globals.gEngine;
			}
			set
			{
				Eamon.Game.Plugin.Globals.gEngine = value;
			}
		}

		/// <inheritdoc cref="Eamon.Game.Plugin.Globals.gOut"/>
		public static ITextWriter gOut 
		{
			get 
			{
				return Eamon.Game.Plugin.Globals.gOut;
			}
		}

		/// <inheritdoc cref="Eamon.Game.Plugin.Globals.gDatabase"/>
		public static IDatabase gDatabase
		{
			get
			{
				return Eamon.Game.Plugin.Globals.gDatabase;
			}
		}

		/// <inheritdoc cref="Eamon.Game.Plugin.Globals.gEDB"/>
		public static IRecordDb<IEffect> gEDB 
		{
			get 
			{
				return Eamon.Game.Plugin.Globals.gEDB;
			}
		}

		/// <summary>Gets the player character being used in the Main Hall.</summary>
		/// <remarks>
		/// The player character is selected (or created) in the Main Hall based on the player's interactions with the Burly
		/// Irishman at the front desk. Existing characters are retrieved from the CHARACTERS.DAT file or recalled from an 
		/// in-progress adventure. The player character record is updated based on activities in the Main Hall and environs.
		/// When sent on an adventure, the status is updated in CHARACTERS.DAT and the character is sent into the game using
		/// the FRESHMEAT.DAT file. When the game ends, the player character is re-imported into the Main Hall fully updated.
		/// </remarks>
        public static ICharacter gCharacter
		{
			get
			{
				return gEngine.Character;
			}
		}
	}

#pragma warning restore IDE1006 // Naming Styles
}
