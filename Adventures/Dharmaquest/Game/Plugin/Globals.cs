﻿
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

		/// <inheritdoc cref="EamonRT.Game.Plugin.Globals.gADB"/>
		public static IRecordDb<IArtifact> gADB
		{
			get
			{
				return EamonRT.Game.Plugin.Globals.gADB;
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
	}

#pragma warning restore IDE1006 // Naming Styles
}
