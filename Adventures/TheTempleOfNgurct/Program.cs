﻿
// Program.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

/*

Originally based upon Eamon's Adventure #23 MAIN PGM:

 1  REM 
EAMON ADVENTURE #23
THE TEMPLE OF NGURCT
BY JAMES & ROBERT PLAMONDON

 3  REM  REV. 2/15/93
 4  REM 
EAMON ADVENTURER'S GUILD
CLEMMONS, NC

*/

using Eamon.Framework.Portability;

namespace TheTempleOfNgurct
{
	public class Program : EamonRT.Program, IProgram
	{
		public override void SetPunctSpaceCode()
		{
			// Do nothing
		}

		public Program()
		{
			ProgramName = "TheTempleOfNgurct";

			EngineType = typeof(Game.Plugin.Engine);
		}
	}
}
