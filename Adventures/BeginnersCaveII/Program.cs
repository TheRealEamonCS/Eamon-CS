
// Program.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

/*

Originally based upon Eamon Deluxe's CAVEII.BAS:

'ÉÍÍÍÍÍÍÍÍÍÍÍ THE MARVELOUS WORLD OF EAMON DELUXE v5.0 ÍÍÍÍÍÍÍÍÍÍÍ»
'º                  by Frank Black Productions                    º
'º                                                                º
'º      Originally based upon the classic Eamon                   º
'º              computerized fantasy role-playing system          º
'º                                                                º
'º              BASE ADVENTURE PROGRAM 5.0 (Build 6610)           º
'º                                                                º
'º  Beginner's Cave II                      Revision: 05 MAY 2012 º
'º    by John Nelson                          Update: 05 MAY 2012 º
'º                                                                º
'ÈÍÍÍÍÍÍÍÍÍÍ ALL NON-COMMERCIAL DISTRIBUTION ENCOURAGED ÍÍÍÍÍÍÍÍÍÍ¼

*/

using Eamon.Framework.Portability;
using static BeginnersCaveII.Game.Plugin.Globals;

namespace BeginnersCaveII
{
	public class Program : EamonRT.Program, IProgram
	{
		public Program()
		{
			ProgramName = "BeginnersCaveII";

			EngineType = typeof(Game.Plugin.Engine);
		}
	}
}
