
// IRoom.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using static RiddlesOfTheDuergarKingdom.Game.Plugin.PluginContext;

namespace RiddlesOfTheDuergarKingdom.Framework
{
	public interface IRoom : Eamon.Framework.IRoom
	{
		/// <summary></summary>
		bool GradStudentCompanionSeen { get; set; }
	}
}
