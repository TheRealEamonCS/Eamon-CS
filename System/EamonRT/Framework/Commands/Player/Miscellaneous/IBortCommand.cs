
// IBortCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Collections.Generic;
using Eamon.Framework;

namespace EamonRT.Framework.Commands
{
	/// <summary>
	/// Developers can use this <see cref="ICommand">Command</see> for adventure debugging purposes.
	/// </summary>
	/// <remarks>
	/// This <see cref="ICommand">Command</see> allows the player character to teleport to specific <see cref="IArtifact">Artifact</see>s
	/// and <see cref="IMonster">Monster</see>s and more generally around the game map.  It also will enable Artifacts and Monsters to be
	/// teleported to the current <see cref="IRoom">Room</see>.  However, the Command should NEVER be used as a god-mode cheat for players
	/// since it can leave the game engine in an indeterminate (and possibly crash-prone) state.
	/// </remarks>
	public interface IBortCommand : ICommand
	{
		/// <summary></summary>
		IGameBase Record { get; set; }

		/// <summary></summary>
		string Action { get; set; }
	}
}
