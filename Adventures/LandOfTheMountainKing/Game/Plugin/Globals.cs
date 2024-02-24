
// Globals.cs

// Copyright (c) 2014+ by Kenneth Pedersen.  All rights reserved.

using Eamon.Framework;
using Eamon.Framework.DataStorage.Generic;
using Eamon.Framework.Portability;

namespace LandOfTheMountainKing.Game.Plugin
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

		/// <inheritdoc cref="EamonRT.Game.Plugin.Globals.gCharRoom"/>
		public static IRoom gCharRoom
		{
			get
			{
				return EamonRT.Game.Plugin.Globals.gCharRoom;
			}
		}

		/// <summary></summary>
		/// <remarks></remarks>
		public static Framework.ILMKKP1 gLMKKP1
		{
			get
			{
				return gGameState?.LMKKP1;
			}
		}
	}

#pragma warning restore IDE1006 // Naming Styles
}
