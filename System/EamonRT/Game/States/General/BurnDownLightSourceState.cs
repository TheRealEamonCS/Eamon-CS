
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

			PrintLightAlmostOutCheck();

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

		public virtual void PrintLightAlmostOutCheck()
		{
			Debug.Assert(LsArtifact != null && LsArtAc != null);

			if (LsArtAc.Field1 <= 20)
			{
				gOut.Print("{0}{1}", LsArtifact.GetTheName(true, buf: Globals.Buf01), LsArtAc.Field1 <= 10 ? " is almost out!" : " grows dim!");
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
