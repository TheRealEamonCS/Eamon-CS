
// Thread.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework.Portability;
using Eamon.Game.Utilities;
using static Eamon.Game.Plugin.Globals;

namespace EamonPM.Game.Portability
{
	public class Thread : IThread
	{
		public virtual void Sleep(long milliseconds)
		{
            WindowRepainter.RepaintWindow(gEngine.ConsoleHandle);

            System.Threading.Thread.Sleep((int)milliseconds);

            WindowRepainter.RepaintWindow(gEngine.ConsoleHandle);
        }
    }
}
