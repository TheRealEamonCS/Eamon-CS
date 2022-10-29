
// Program.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

/*

Originally based upon Eamon's Adventure #73 MAIN PGM:

 1  REM    EAMON ADVENTURE #73
 2  REM ->  THE DEEP CANYON   <-
 3  REM 
 4  REM     BY KENN BLINCOE
 6  REM        STATELINE, NV
 7  REM        ZIP: 89449-2935

 8  REM  REV 10/07/84


 9  REM 

EAMON ADVENTURER'S GUILD
CLEMMONS, NC 27012

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
