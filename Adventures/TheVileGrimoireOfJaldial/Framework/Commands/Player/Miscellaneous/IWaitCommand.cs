
// IWaitCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using EamonRT.Framework.Commands;
using static TheVileGrimoireOfJaldial.Game.Plugin.PluginContext;

namespace TheVileGrimoireOfJaldial.Framework.Commands
{
	public interface IWaitCommand : ICommand
	{
		/// <summary></summary>
		long Minutes { get; set; }
	}
}
