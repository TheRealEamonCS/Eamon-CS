
// BurnDownLightSourceState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework;
using Eamon.Framework.Primitive.Classes;
using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.States
{
	[ClassMappings]
	public class BurnDownLightSourceState : State, IBurnDownLightSourceState
	{
		/// <summary></summary>
		public virtual IArtifactCategory LsArtAc { get; set; }

		/// <summary></summary>
		public virtual IArtifact LsArtifact { get; set; }

		/// <summary></summary>
		public virtual long LsArtifactUid { get; set; }

		public override void Execute()
		{
			LsArtifactUid = gGameState.Ls;

			if (LsArtifactUid <= 0 || (Globals.CommandPromptSeen && !ShouldPreTurnProcess()))
			{
				goto Cleanup;
			}

			LsArtifact = gADB[LsArtifactUid];

			Debug.Assert(LsArtifact != null);

			LsArtAc = LsArtifact.LightSource;

			if (LsArtAc == null || LsArtAc.Field1 == -1)
			{
				goto Cleanup;
			}

			if (LsArtAc.Field1 <= 0)
			{
				gEngine.LightOut(LsArtifact);

				goto Cleanup;
			}

			LightAlmostOutCheck();

			DecrementLightTurnCounter();

			if (LsArtAc.Field1 < 0)
			{
				LsArtAc.Field1 = 0;
			}

		Cleanup:

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IBurnDownSpeedSpellState>();
			}

			Globals.NextState = NextState;
		}

		public virtual void LightAlmostOutCheck()
		{
			Debug.Assert(LsArtifact != null && LsArtAc != null);

			if (LsArtAc.Field1 <= 20)
			{
				PrintLightAlmostOut(LsArtifact, LsArtAc.Field1);
			}
		}

		public virtual void DecrementLightTurnCounter()
		{
			Debug.Assert(LsArtifact != null && LsArtAc != null);

			LsArtAc.Field1--;
		}

		public BurnDownLightSourceState()
		{
			Uid = 1;

			Name = "BurnDownLightSourceState";
		}
	}
}
