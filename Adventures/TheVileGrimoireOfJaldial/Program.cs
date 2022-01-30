
// Program.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework.Portability;

namespace TheVileGrimoireOfJaldial
{
	public class Program : EamonRT.Program, IProgram
	{
		public Program()
		{
			ProgramName = "TheVileGrimoireOfJaldial";

			ConstantsType = typeof(Game.Plugin.PluginConstants);

			ClassMappingsType = typeof(Game.Plugin.PluginClassMappings);

			GlobalsType = typeof(Game.Plugin.PluginGlobals);
		}
	}
}
