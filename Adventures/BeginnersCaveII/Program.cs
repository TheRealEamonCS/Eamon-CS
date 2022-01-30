
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

using Eamon;
using Eamon.Framework.Portability;
using Eamon.Framework.Primitive.Enums;
using static BeginnersCaveII.Game.Plugin.PluginContext;

namespace BeginnersCaveII
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
			ProgramName = "BeginnersCaveII";

			ConstantsType = typeof(Game.Plugin.PluginConstants);

			ClassMappingsType = typeof(Game.Plugin.PluginClassMappings);

			GlobalsType = typeof(Game.Plugin.PluginGlobals);
		}
	}
}
