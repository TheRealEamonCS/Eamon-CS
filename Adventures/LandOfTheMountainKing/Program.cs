
// Program.cs

// Copyright (c) 2014+ by Kenneth Pedersen.  All rights reserved.

/*

*/

using Eamon.Framework.Portability;
using static LandOfTheMountainKing.Game.Plugin.Globals;

namespace LandOfTheMountainKing
{
	public class Program : EamonRT.Program, IProgram
	{
		public Program()
		{
			ProgramName = "LandOfTheMountainKing";

			EngineType = typeof(Game.Plugin.Engine);
		}
	}
}
