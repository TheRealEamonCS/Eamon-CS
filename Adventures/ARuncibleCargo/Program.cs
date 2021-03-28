
// Program.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

/*

Originally based upon Eamon Deluxe's RUNCIBLE.BAS:

'ÉÍÍÍÍÍÍÍÍÍÍÍ THE MARVELOUS WORLD OF EAMON DELUXE v5.0 ÍÍÍÍÍÍÍÍÍÍÍ»
'º                  by Frank Black Productions                    º
'º                                                                º
'º      Originally based upon the classic Eamon                   º
'º              computerized fantasy role-playing system          º
'º                                                                º
'º      BASE ADVENTURE PROGRAM 5.0 (Build 6612 - 09 JUN 2012)     º
'º                                                                º
'º A Runcible Cargo                       Revision 1: 31 MAR 2011 º
'º  by Thomas Ferguson                    Revision 2: 05 JUN 2012 º
'º                                            Update: 09 DEC 2012 º
'º                                                                º
'ÈÍÍÍÍÍÍÍÍÍÍ ALL NON-COMMERCIAL DISTRIBUTION ENCOURAGED ÍÍÍÍÍÍÍÍÍÍ¼

*/

using Eamon.Framework.Portability;

namespace ARuncibleCargo
{
	public class Program : EamonRT.Program, IProgram
	{
		public Program()
		{
			ProgramName = "ARuncibleCargo";

			ConstantsType = typeof(Game.Plugin.PluginConstants);

			ClassMappingsType = typeof(Game.Plugin.PluginClassMappings);

			GlobalsType = typeof(Game.Plugin.PluginGlobals);
		}
	}
}
