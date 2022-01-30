
// Program.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

/*

Originally based upon Eamon Deluxe's FOREST.BAS:

'ÉÍÍÍÍÍÍÍÍÍÍÍ THE MARVELOUS WORLD OF EAMON DELUXE v5.0 ÍÍÍÍÍÍÍÍÍÍÍ»
'º                  by Frank Black Productions                    º
'º                                                                º
'º      Originally based upon the classic Eamon                   º
'º              computerized fantasy role-playing system          º
'º                                                                º
'º              BASE ADVENTURE PROGRAM 5.0 (Build 6609)           º
'º                                                                º
'º   Beginner's Forest                  Revision 3.4: 20 OCT 2000 º
'º  by Margaret Anderson                Revision 5.0: 31 MAR 2012 º
'º                                            Update: 04 APR 2012 º
'º                                                                º
'ÈÍÍÍÍÍÍÍÍÍÍ ALL NON-COMMERCIAL DISTRIBUTION ENCOURAGED ÍÍÍÍÍÍÍÍÍÍ¼

*/

using Eamon.Framework.Portability;

namespace BeginnersForest
{
	public class Program : EamonRT.Program, IProgram
	{
		public Program()
		{
			ProgramName = "BeginnersForest";

			ConstantsType = typeof(Game.Plugin.PluginConstants);

			ClassMappingsType = typeof(Game.Plugin.PluginClassMappings);

			GlobalsType = typeof(Game.Plugin.PluginGlobals);
		}
	}
}
