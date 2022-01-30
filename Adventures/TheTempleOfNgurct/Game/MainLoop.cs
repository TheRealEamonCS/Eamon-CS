
// MainLoop.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework;
using static TheTempleOfNgurct.Game.Plugin.PluginContext;

namespace TheTempleOfNgurct.Game
{
	[ClassMappings]
	public class MainLoop : EamonRT.Game.MainLoop, IMainLoop
	{
		public override void Shutdown()
		{
			var reward = 0L;

			var medallionArtifact = gADB[77];

			Debug.Assert(medallionArtifact != null);

			var carryingMedallion = medallionArtifact.IsCarriedByCharacter();

			medallionArtifact.SetInLimbo();

			base.Shutdown();

			gOut.Print("{0}", Globals.LineSep);

			// Thera's reward

			var theraMonster = gMDB[29];

			Debug.Assert(theraMonster != null);

			var gonzalesMonster = gMDB[55];

			Debug.Assert(gonzalesMonster != null);

			if (theraMonster.Location == gGameState.Ro && theraMonster.Reaction > Friendliness.Enemy)
			{
				var rw = 200 + (long)Math.Round(100 * ((double)(theraMonster.Hardiness - theraMonster.DmgTaken) / (double)theraMonster.Hardiness));

				gOut.Print("In addition, you receive {0} gold piece{1} as a reward for the return of Princess Thera.{2}", 
					rw, 
					rw != 1 ? "s" : "", 
					gonzalesMonster.Location == gGameState.Ro ? "  Of course, Gonzales takes his half." : "");

				if (gonzalesMonster.Location == gGameState.Ro)
				{
					rw /= 2;
				}

				reward += rw;
			}

			// King's reward

			if (carryingMedallion)
			{
				gOut.Print("You also receive the 2,000 gold pieces for the return of the gold medallion of Ngurct.  It is immediately destroyed by the King.");

				reward += 2000;
			}
			else
			{
				gOut.Print("Unfortunately, you have failed in your mission to return the gold medallion of Ngurct.  Shame!  Shame!  (And no money.)");
			}

			gCharacter.HeldGold += reward;

			Globals.In.KeyPress(Globals.Buf);
		}
	}
}
