
// Program.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

/*

*/

using Eamon.Framework.Portability;

namespace TheWayfarersInn
{
	public class Program : EamonRT.Program, IProgram
	{
		public override void SetPunctSpaceCode()
		{
			// do nothing
		}

		public Program()
		{
			ProgramName = "TheWayfarersInn";

			EngineType = typeof(Game.Plugin.Engine);
		}
	}
}
