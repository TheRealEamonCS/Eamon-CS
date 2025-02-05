
// Program.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

/*
 1  REM      EAMON ADVENTURE #120
 2  REM          ORB OF MY LIFE
 3  REM  
 4  REM         BY JOHN NELSON 
 5  REM  
 6  REM      DDD VERSION 6.2
 7  REM  6/19/91
 8  REM 
*/

using Eamon.Framework.Portability;

namespace OrbOfMyLife
{
	public class Program : EamonRT.Program, IProgram
	{
		public Program()
		{
			ProgramName = "OrbOfMyLife";

			EngineType = typeof(Game.Plugin.Engine);
		}
	}
}
