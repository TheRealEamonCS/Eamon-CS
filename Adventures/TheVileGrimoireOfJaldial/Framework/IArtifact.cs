
// IArtifact.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using static TheVileGrimoireOfJaldial.Game.Plugin.PluginContext;

namespace TheVileGrimoireOfJaldial.Framework
{
	public interface IArtifact : Eamon.Framework.IArtifact
	{
		/// <summary></summary>
		bool Seen02 { get; }

		/// <summary></summary>
		bool IsDecoration();

		/// <summary></summary>
		long GetLeverageBonus();
	}
}
