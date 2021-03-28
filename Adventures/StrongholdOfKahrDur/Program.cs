
// Program.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

/*

Originally based upon Eamon Deluxe's KAHR-DUR.BAS:

'ÉÍÍÍÍÍÍÍÍÍÍÍ THE MARVELOUS WORLD OF EAMON DELUXE v5.0 ÍÍÍÍÍÍÍÍÍÍÍ»
'º                  by Frank Black Productions                    º
'º                                                                º
'º      Originally based upon the classic Eamon                   º
'º              computerized fantasy role-playing system          º
'º                                                                º
'º             BASE ADVENTURE PROGRAM 5.0-B (Build 6617)          º
'º                                                                º
'º  Stronghold of Kahr-Dur                  Revision: 13 JUN 2012 º
'º    by Derek C. Jeter                       Update: 10 DEC 2012 º
'º                                                                º
'ÈÍÍÍÍÍÍÍÍÍÍ ALL NON-COMMERCIAL DISTRIBUTION ENCOURAGED ÍÍÍÍÍÍÍÍÍÍ¼

*/

using Eamon.Framework.Portability;

namespace StrongholdOfKahrDur
{
	public class Program : EamonRT.Program, IProgram
	{
		public Program()
		{
			ProgramName = "StrongholdOfKahrDur";

			ConstantsType = typeof(Game.Plugin.PluginConstants);

			ClassMappingsType = typeof(Game.Plugin.PluginClassMappings);

			GlobalsType = typeof(Game.Plugin.PluginGlobals);
		}
	}
}
