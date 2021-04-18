
// Program.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

/*

Originally based upon Eamon Deluxe's ALTCAVE.BAS:

'ÉÍÍÍÍÍÍÍÍÍÍÍ THE MARVELOUS WORLD OF EAMON DELUXE v5.0 ÍÍÍÍÍÍÍÍÍÍÍ»
'º                  by Frank Black Productions                    º
'º                                                                º
'º      Originally based upon the classic Eamon                   º
'º              computerized fantasy role-playing system          º
'º                                                                º
'º              BASE ADVENTURE PROGRAM 5.0 (Build 6608)           º
'º                                                                º
'º  Alternate Beginner's Cave           4.2 Revision: 20 OCT 2000 º
'º    by Rick Volberding                5.0 Revision: 26 MAR 2012 º
'º                                            Update: 26 MAR 2012 º
'º                                                                º
'ÈÍÍÍÍÍÍÍÍÍÍ ALL NON-COMMERCIAL DISTRIBUTION ENCOURAGED ÍÍÍÍÍÍÍÍÍÍ¼

*/

using Eamon;
using Eamon.Framework.Portability;
using Eamon.Framework.Primitive.Enums;
using static AlternateBeginnersCave.Game.Plugin.PluginContext;

namespace AlternateBeginnersCave
{
	public class Program : EamonRT.Program, IProgram
	{
		public override RetCode RtMain(string[] args)
		{
			gOut.PunctSpaceCode = PunctSpaceCode.Single;

			return base.RtMain(args);
		}

		public Program()
		{
			ProgramName = "AlternateBeginnersCave";

			ConstantsType = typeof(Game.Plugin.PluginConstants);

			ClassMappingsType = typeof(Game.Plugin.PluginClassMappings);

			GlobalsType = typeof(Game.Plugin.PluginGlobals);
		}
	}
}
