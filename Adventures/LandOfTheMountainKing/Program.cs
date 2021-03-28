
// Program.cs

// Copyright (c) 2014+ by Kenneth Pedersen.  All rights reserved.

/*

*/

using Eamon;
using Eamon.Framework.Portability;
using Eamon.Framework.Primitive.Enums;
using static LandOfTheMountainKing.Game.Plugin.PluginContext;

namespace LandOfTheMountainKing
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
			ProgramName = "LandOfTheMountainKing";

			ConstantsType = typeof(Game.Plugin.PluginConstants);

			ClassMappingsType = typeof(Game.Plugin.PluginClassMappings);

			GlobalsType = typeof(Game.Plugin.PluginGlobals);
		}
	}
}
