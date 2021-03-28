
// IGameState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

namespace TheTrainingGround.Framework
{
	/// <summary></summary>
	public interface IGameState : Eamon.Framework.IGameState
	{
		#region Properties

		/// <summary></summary>
		bool RedSunSpeaks { get; set; }

		/// <summary></summary>
		bool JacquesShouts { get; set; }

		/// <summary></summary>
		bool JacquesRecoversRapier { get; set; }

		/// <summary></summary>
		bool KoboldsAppear { get; set; }

		/// <summary></summary>
		bool SylvaniSpeaks { get; set; }

		/// <summary></summary>
		bool ThorsHammerAppears { get; set; }

		/// <summary></summary>
		bool LibrarySecretPassageFound { get; set; }

		/// <summary></summary>
		bool ScuffleSoundsHeard { get; set; }

		/// <summary></summary>
		bool CharismaBoosted { get; set; }

		/// <summary></summary>
		long GenderChangeCounter { get; set; }

		#endregion
	}
}
