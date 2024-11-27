
// Program.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

/*
 1  REM       EAMON ADVENTURE #126
 2  REM       THE PYRAMID OF ANHAROS

 3  REM          BY
 4  REM       PATRICK R. HURST
 7  REM 

REV. 4-30-87
*/

using Eamon.Framework.Portability;

namespace ThePyramidOfAnharos
{
	public class Program : EamonRT.Program, IProgram
	{
		public Program()
		{
			ProgramName = "ThePyramidOfAnharos";

			EngineType = typeof(Game.Plugin.Engine);
		}
	}
}
