
// Program.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

/*

Originally based upon Eamon Deluxe's SUB-AQUA.BAS:

'ÉÍÍÍÍÍÍÍÍÍÍÍ THE MARVELOUS WORLD OF EAMON DELUXE v5.0 ÍÍÍÍÍÍÍÍÍÍÍ»
'º                  by Frank Black Productions                    º
'º                                                                º
'º      Originally based upon the classic Eamon                   º
'º              computerized fantasy role-playing system          º
'º                                                                º
'º              BASE ADVENTURE PROGRAM 5.0 (Build 6615)           º
'º                                                                º
'º The Sub-Aquan Laboratory               Revision 1: 14 FEB 2012 º
'º    by Michael Penner                   Revision 2: 19 AUG 2012 º
'º Converted to Eamon Deluxe by TMF           Update: 10 DEC 2012 º
'º                                                                º
'ÈÍÍÍÍÍÍÍÍÍÍ ALL NON-COMMERCIAL DISTRIBUTION ENCOURAGED ÍÍÍÍÍÍÍÍÍÍ¼

*/

using Eamon.Framework.Portability;

namespace TheSubAquanLaboratory
{
	public class Program : EamonRT.Program, IProgram
	{
		public override void SetPunctSpaceCode()
		{
			// Do nothing
		}

		public Program()
		{
			ProgramName = "TheSubAquanLaboratory";

			EngineType = typeof(Game.Plugin.Engine);
		}
	}
}
