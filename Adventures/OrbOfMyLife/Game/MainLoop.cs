
// MainLoop.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework;
using static OrbOfMyLife.Game.Plugin.Globals;

namespace OrbOfMyLife.Game
{
	[ClassMappings]
	public class MainLoop : EamonRT.Game.MainLoop, IMainLoop
	{
		public override void Shutdown()
		{
			Debug.Assert(gCharMonster != null);

			// Extro with Sagonne

			var orbOfLifeArtifact = gADB[23];

			Debug.Assert(orbOfLifeArtifact != null);

			gOut.Print("{0}", gEngine.LineSep);

			if (orbOfLifeArtifact.IsCarriedByMonster(gCharMonster))
			{
				if (gGameState.RC)
				{
					gOut.Print("The wizard takes back his orb and congratulates you on a job well done.");
				}
				else
				{
					gOut.Print("The wizard uses the power of the orb to set the Eamon world aright. Well done.");
				}

				orbOfLifeArtifact.SetInLimbo();
			}
			else
			{
				gOut.Print("The wizard is very disappointed in you. He asks the king to send him another volunteer.");
			}

			gEngine.In.KeyPress(gEngine.Buf);

			base.Shutdown();
		}
	}
}
