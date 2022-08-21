
// Program.cs

// Copyright (c) 2014+ by Kenneth Pedersen.  All rights reserved.

/*

*/

using Eamon.Framework.Portability;
using static LandOfTheMountainKing.Game.Plugin.PluginContext;

namespace LandOfTheMountainKing
{
	public class Program : EamonRT.Program, IProgram
	{
		public Program()
		{
			ProgramName = "LandOfTheMountainKing";

			ConstantsType = typeof(Game.Plugin.PluginConstants);

			ClassMappingsType = typeof(Game.Plugin.PluginClassMappings);

			GlobalsType = typeof(Game.Plugin.PluginGlobals);
		}
	}
}
