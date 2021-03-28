
// IMainLoop.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Text;

namespace EamonRT.Framework
{
	/// <summary></summary>
	public interface IMainLoop
	{
		/// <summary></summary>
		StringBuilder Buf { get; set; }

		/// <summary></summary>
		bool ShouldStartup { get; set; }

		/// <summary></summary>
		bool ShouldShutdown { get; set; }

		/// <summary></summary>
		bool ShouldExecute { get; set; }

		/// <summary></summary>
		void Startup();

		/// <summary></summary>
		void Shutdown();

		/// <summary></summary>
		void Execute();
	}
}
