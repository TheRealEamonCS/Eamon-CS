
// Delegates.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;

namespace Eamon.Framework
{
	/// <summary>
	/// A collection of C# delegate signatures.
	/// </summary>
	public static class Delegates
	{
		/// <summary>
		/// Queries the game database for a list of <see cref="IGameBase">Record</see>s matching a criteria set.
		/// </summary>
		/// <param name="whereClauseFuncs"></param>
		/// <returns></returns>
		public delegate IList<IGameBase> GetRecordListFunc(params Func<IGameBase, bool>[] whereClauseFuncs);

		/// <summary>
		/// Filters a given <see cref="IGameBase">Record</see> list, returning all records matching a given name.
		/// </summary>
		/// <param name="recordList"></param>
		/// <param name="name"></param>
		/// <returns></returns>
		public delegate IList<IGameBase> FilterRecordListFunc(IList<IGameBase> recordList, string name);

		/// <summary>
		/// Reveals an embedded <see cref="IArtifact">Artifact</see>, moving it into its containing <see cref="IRoom">Room</see>
		/// and printing its <see cref="IGameBase.Desc">Desc</see>ription if necessary.
		/// </summary>
		/// <param name="room"></param>
		/// <param name="artifact"></param>
		public delegate void RevealEmbeddedArtifactFunc(IRoom room, IArtifact artifact);
	}
}
