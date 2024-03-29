﻿
// Mutex.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Threading;
using Eamon.Framework.Portability;
using static Eamon.Game.Plugin.Globals;

namespace EamonPM.Game.Portability
{
	public class Mutex : IMutex
	{
		public virtual void CreateAndWaitOne()
		{
			// Do nothing
		}
	}
}
