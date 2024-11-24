
// GameState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework;
using Eamon.Game.Attributes;
using static Dharmaquest.Game.Plugin.Globals;

namespace Dharmaquest.Game
{
	[ClassMappings(typeof(IGameState))]
	public class GameState : Eamon.Game.GameState, Framework.IGameState
	{
		public virtual string BlackWizardName { get; set; }

		public virtual long Karma { get; set; }

		public virtual bool RiddleAnswered { get; set; }

		public virtual bool RiddleSolved { get; set; }

		public virtual bool SphinxKilled { get; set; }

		public virtual bool BlackWizardNameRevealed { get; set; }

		public virtual bool BlackWizardMet { get; set; }

		public virtual bool AchillesMet { get; set; }

		public virtual bool NeoptolemusMet { get; set; }

		public virtual bool BullMet { get; set; }

		public virtual bool PythonMet { get; set; }

		public virtual bool PoseidonCurses { get; set; }

		public virtual bool ApolloCurses { get; set; }

		public GameState()
		{
			var blackWizardNamesArray = new string[]
			{
				"Rikor",
				"Rajii",
				"Renul",
				"Regor",
				"Rojac"
			};

			BlackWizardName = gEngine.GetRandomElement(blackWizardNamesArray);
		}
	}
}
