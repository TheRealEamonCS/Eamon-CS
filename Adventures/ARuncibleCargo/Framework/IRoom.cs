
// IRoom.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

namespace ARuncibleCargo.Framework
{
	/// <summary></summary>
	public interface IRoom : Eamon.Framework.IRoom
	{
		/// <summary>Indicates whether this <see cref="IRoom">Room</see> consists of open water (Malphigian Sea).</summary>
		/// <returns>If so then <c>true</c>, otherwise <c>false</c>.</returns>
		bool IsWaterRoom();
	}
}
