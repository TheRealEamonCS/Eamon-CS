
// Mutex.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Threading;
using Eamon.Framework.Portability;
using static Eamon.Game.Plugin.PluginContext;

namespace EamonPM.Game.Portability
{
	public class Mutex : IMutex
	{
		/// <summary></summary>
		public virtual System.Threading.Mutex ProcessMutex { get; set; }

		public virtual void CreateAndWaitOne()
		{
			ProcessMutex = new System.Threading.Mutex(false, Constants.ProcessMutexName);

			try
			{
				ProcessMutex.WaitOne();
			}
			catch (AbandonedMutexException)
			{
				// do nothing
			}
		}
	}
}
