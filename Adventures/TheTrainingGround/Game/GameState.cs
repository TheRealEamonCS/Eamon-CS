
// GameState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework;
using Eamon.Game.Attributes;

namespace TheTrainingGround.Game
{
	[ClassMappings(typeof(IGameState))]
	public class GameState : Eamon.Game.GameState, Framework.IGameState
	{
		public virtual bool RedSunSpeaks { get; set; }

		public virtual bool JacquesShouts { get; set; }

		public virtual bool JacquesRecoversRapier { get; set; }

		public virtual bool KoboldsAppear { get; set; }

		public virtual bool SylvaniSpeaks { get; set; }

		public virtual bool ThorsHammerAppears { get; set; }

		public virtual bool LibrarySecretPassageFound { get; set; }

		public virtual bool ScuffleSoundsHeard { get; set; }

		public virtual bool CharismaBoosted { get; set; }

		public virtual long GenderChangeCounter { get; set; }

		public GameState()
		{

		}
	}
}
