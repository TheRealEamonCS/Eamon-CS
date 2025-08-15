
// Program.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

/*
 1  REM 

EAMON ADVENTURE #229
    FIRESTORM
  BY  PHIL SCHULZ


 6  REM  VERSION 7.1

 7  REM REV. 12/16/94
*/

using Eamon.Framework.Portability;

namespace Firestorm
{
	public class Program : EamonRT.Program, IProgram
	{
		public Program()
		{
			ProgramName = "Firestorm";

			EngineType = typeof(Game.Plugin.Engine);
		}
	}
}
