
// Globals.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework.DataStorage.Generic;
using Eamon.Framework.Portability;

namespace BeginnersCaveII.Game.Plugin
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

		public static Eamon.Framework.IGameState gGameState
		{
			get
			{
				return (Eamon.Framework.IGameState)EamonRT.Game.Plugin.Globals.gGameState;
			}
		}

		public static Eamon.Framework.ICharacter gCharacter
		{
			get
			{
				return (Eamon.Framework.ICharacter)EamonRT.Game.Plugin.Globals.gCharacter;
			}
		}
	}
}
