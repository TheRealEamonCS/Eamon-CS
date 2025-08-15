
// MainLoop.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Game.Attributes;
using EamonRT.Framework;
using static Firestorm.Game.Plugin.Globals;

namespace Firestorm.Game
{
	[ClassMappings]
	public class MainLoop : EamonRT.Game.MainLoop, IMainLoop
	{
		public override void Shutdown()
		{
			base.Shutdown();

			gOut.Print("{0}", gEngine.LineSep);

			gCharacter.HeldGold += gEngine.SecretBonus;		// Secret bonus!

			// Nightmare

			gEngine.PrintEffectDesc(39);

			gEngine.In.KeyPress(gEngine.Buf);
		}
	}
}
