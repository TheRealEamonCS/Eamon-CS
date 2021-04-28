
// IGameState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Collections.Generic;
using static RiddlesOfTheDuergarQuarry.Game.Plugin.PluginContext;

namespace RiddlesOfTheDuergarQuarry.Framework
{
	public interface IGameState : Eamon.Framework.IGameState
	{
		#region Properties

		/// <summary></summary>
		IList<long> ViperPoisonVictimUids { get; set; }

		#endregion
	}
}
