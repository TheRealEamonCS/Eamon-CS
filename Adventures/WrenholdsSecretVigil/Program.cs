
// Program.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

/*

Originally based upon Eamon's Adventure #121 MAIN PGM:

 1  REM    EAMON ADVENTURE #121
 2  REM  WRENHOLD'S SECRET VIGIL
 3  REM  
 4  REM       BY BOB DAVIS
 5  REM  
 7  REM    WRITTEN: 10-31-85
 8  REM   (UNDER CLOSE ADULT
 9  REM      SUPERVISION.)
 10  REM 
 11  REM 
 12  REM LAST REV: 11/5/89
 13  REM 
 14  REM 
EAMON ADVENTURER'S GUILD
CLEMMONS, NC 27012


*/

using Eamon.Framework.Portability;

namespace WrenholdsSecretVigil
{
	public class Program : EamonRT.Program, IProgram
	{
		public Program()
		{
			ProgramName = "WrenholdsSecretVigil";

			ConstantsType = typeof(Game.Plugin.PluginConstants);

			ClassMappingsType = typeof(Game.Plugin.PluginClassMappings);

			GlobalsType = typeof(Game.Plugin.PluginGlobals);
		}
	}
}
