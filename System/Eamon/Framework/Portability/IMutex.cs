
// IMutex.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

namespace Eamon.Framework.Portability
{
	/// <summary></summary>
	public interface IMutex
	{
		/// <summary></summary>
		void CreateAndWaitOne();
	}
}
