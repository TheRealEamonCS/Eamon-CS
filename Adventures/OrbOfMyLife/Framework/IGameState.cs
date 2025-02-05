
// IGameState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using static OrbOfMyLife.Game.Plugin.Globals;

namespace OrbOfMyLife.Framework
{
	/// <inheritdoc />
	public interface IGameState : Eamon.Framework.IGameState
	{
		string MW { get; set; }

		string TW { get; set; }

		string CW { get; set; }

		long GOL { get; set; }			// Gate of Light counter

		long SCR { get; set; }			// Scream counter

		long IS { get; set; }			// Insanity counter

		long VC { get; set; }			// Invisibility counter

		long FC { get; set; }           // Falling counter

		long TC { get; set; }			// Darrk Ness stun counter

		long SR { get; set; }			// Stuff Room Uid

		bool ICV { get; set; }          // Eyes closed (variable)

		bool FL { get; set; }			// Darrk Ness killed

		bool RC { get; set; }           // Vortex fixed

		bool IC { get; }				// Eyes closed

		bool IV { get; }                // Invisible
	}
}
