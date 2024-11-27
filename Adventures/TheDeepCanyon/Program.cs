
// Program.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

/*
 1  REM    EAMON ADVENTURE #73
 2  REM ->  THE DEEP CANYON   <-
 3  REM 
 4  REM     BY KENN BLINCOE

 8  REM  REV 10/07/84
*/

using Eamon.Framework.Portability;
using static TheDeepCanyon.Game.Plugin.Globals;

namespace TheDeepCanyon
{
	public class Program : EamonRT.Program, IProgram
	{
		public Program()
		{
			ProgramName = "TheDeepCanyon";

			EngineType = typeof(Game.Plugin.Engine);
		}
	}
}
