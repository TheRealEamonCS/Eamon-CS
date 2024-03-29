﻿
// GameState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon;
using Eamon.Framework;
using Eamon.Game.Attributes;
using static TheBeginnersCave.Game.Plugin.Globals;

namespace TheBeginnersCave.Game
{
	[ClassMappings(typeof(IGameState))]
	public class GameState : Eamon.Game.GameState, Framework.IGameState
	{
		public long _trollsfire = 0;

		[FieldName(1121)]
		public virtual long Trollsfire
		{
			get
			{
				return _trollsfire;
			}

			set
			{
				RetCode rc;

				// If toggling the Trollsfire effect

				if (gEngine.EnableMutateProperties && _trollsfire != value)
				{
					// Find Trollsfire in the game database

					var trollsfireArtifact = gADB[10];

					Debug.Assert(trollsfireArtifact != null);

					// Look up the data relating to artifact type MagicWeapon

					var ac = trollsfireArtifact.MagicWeapon;

					Debug.Assert(ac != null);

					// If activating the Trollsfire effect

					if (value != 0)
					{
						// Trollsfire is now alight so add " (alight)" to the StateDesc property

						rc = trollsfireArtifact.AddStateDesc(gEngine.AlightDesc);

						Debug.Assert(gEngine.IsSuccess(rc));

						// Trollsfire now does 1d10 damage; change weapon sides to 10

						ac.Field4 = 10;
					}
					else
					{
						// Trollsfire is now extinguished so remove " (alight)" from the StateDesc property

						rc = trollsfireArtifact.RemoveStateDesc(gEngine.AlightDesc);

						Debug.Assert(gEngine.IsSuccess(rc));

						// Trollsfire now does 1d6 damage; change weapon sides to 6

						ac.Field4 = 6;
					}
				}

				// Store change to Trollsfire property in backing field

				_trollsfire = value;
			}
		}

		[FieldName(1122)]
		public virtual long BookWarning { get; set; }

		public GameState()
		{

		}
	}
}
