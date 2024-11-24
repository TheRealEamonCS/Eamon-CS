
// IEngine.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using EamonRT.Framework.States;

namespace Dharmaquest.Framework.Plugin
{
	/// <inheritdoc />
	public interface IEngine : EamonRT.Framework.Plugin.IEngine
	{
		void ApolloCursesPlayer();

		void PoseidonCursesPlayer(IState printPlayerRoomState);
	}
}
