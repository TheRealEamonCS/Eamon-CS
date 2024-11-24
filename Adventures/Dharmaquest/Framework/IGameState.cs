
// IGameState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using static Dharmaquest.Game.Plugin.Globals;

namespace Dharmaquest.Framework
{
	/// <inheritdoc />
	public interface IGameState : Eamon.Framework.IGameState
	{
		string BlackWizardName { get; set; }

		long Karma { get; set; }

		bool RiddleAnswered { get; set; }

		bool RiddleSolved { get; set; }

		bool SphinxKilled { get; set; }

		bool BlackWizardNameRevealed { get; set; }

		bool BlackWizardMet { get; set; }

		bool AchillesMet { get; set; }

		bool NeoptolemusMet { get; set; }

		bool BullMet { get; set; }

		bool PythonMet { get; set; }

		bool BullFriendly { get; set; }

		bool PythonFriendly { get; set; }
	}
}
