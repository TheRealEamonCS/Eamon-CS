
// Program.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

/*

*/

using Eamon.Framework.Portability;

namespace ThePyramidOfAnharos
{
	public class Program : EamonRT.Program, IProgram
	{
		public Program()
		{
			ProgramName = "ThePyramidOfAnharos";

			EngineType = typeof(Game.Plugin.Engine);
		}
	}
}
