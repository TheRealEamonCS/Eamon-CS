
// IEngine.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Text;
using EamonRT.Framework.States;

namespace Dharmaquest.Framework.Plugin
{
	/// <inheritdoc />
	public interface IEngine : EamonRT.Framework.Plugin.IEngine
	{
		/// <summary></summary>
		new StringBuilder Buf { get; set; }

		/// <summary></summary>
		new StringBuilder Buf01 { get; set; }

		void ApolloCursesPlayer();

		void PoseidonCursesPlayer(IState printPlayerRoomState);
	}
}
