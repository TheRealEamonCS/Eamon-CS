
// IArtifact.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

namespace WrenholdsSecretVigil.Framework
{
	/// <summary></summary>
	public interface IArtifact : Eamon.Framework.IArtifact
	{
		/// <summary>Indicates whether this <see cref="IArtifact">Artifact</see> is buried in a given <see cref="Eamon.Framework.IRoom">Room</see>.</summary>
		/// <param name="roomUid">The <see cref="Eamon.Framework.IGameBase.Uid">Uid</see> of the <see cref="Eamon.Framework.IRoom">Room</see> to check.</param>
		/// <returns>If so then <c>true</c>, otherwise <c>false</c>.</returns>
		bool IsBuriedInRoomUid(long roomUid);

		/// <summary>Indicates whether this <see cref="IArtifact">Artifact</see> is buried in a given <see cref="Eamon.Framework.IRoom">Room</see>.</summary>
		/// <param name="room">The <see cref="Eamon.Framework.IRoom">Room</see> to check.</param>
		/// <returns>If so then <c>true</c>, otherwise <c>false</c>.</returns>
		bool IsBuriedInRoom(Eamon.Framework.IRoom room);
	}
}
