
// Program.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

/*
 1  REM      EAMON ADVENTURE #74
 2  REM  ->     DHARMAQUEST     <- 
 3  REM       3 JULY 1984
 4  REM       BY ROGER PENDER
 5  REM 
 6  REM ///  TO MOM AND DAD
 7  REM LAST UPDATE: 11/5/89
*/

using Eamon.Framework.Portability;

namespace Dharmaquest
{
	public class Program : EamonRT.Program, IProgram
	{
		public Program()
		{
			ProgramName = "Dharmaquest";

			EngineType = typeof(Game.Plugin.Engine);
		}
	}
}
