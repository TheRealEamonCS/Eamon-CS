
// IDbTable.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;

namespace Eamon.Framework.DataStorage.Generic
{
	/// <summary>
	/// Represents a collection of records of type T in a database.
	/// </summary>
	public interface IDbTable<T> where T : class, IGameBase
	{
		/// <summary>
		/// Gets or sets the collection of records stored in the database table.
		/// </summary>
		ICollection<T> Records { get; set; }

		/// <summary>
		/// Gets or sets the collection of Uids available for reuse by new instances of this record type; may be empty.
		/// </summary>
		IList<long> FreeUids { get; set; }

		/// <summary>
		/// Gets or sets an array of records stored in most recently used (MRU) order, a quick-lookup cache.
		/// </summary>
		T[] Cache { get; set; }

		/// <summary>
		/// Gets or sets a sequence number representing the last Uid (unique ID) allocated to a record of type T.
		/// </summary>
		long CurrUid { get; set; }

		/// <summary>
		/// Fully reinitializes the IDbTable and restores it to its initial (empty) state.
		/// </summary>
		/// <param name="dispose"></param>
		/// <returns></returns>
		RetCode FreeRecords(bool dispose = true);

		/// <summary>
		/// Gets the number of records of type T stored in the <see cref="Records">Records</see> collection.
		/// </summary>
		/// <returns></returns>
		long GetRecordsCount();

		/// <summary>
		/// Gets a record from the <see cref="Records">Records</see> collection.
		/// </summary>
		/// <param name="uid"></param>
		/// <returns></returns>
		T FindRecord(long uid);

		/// <summary>
		/// Gets a record from the <see cref="Records">Records</see> collection based on interface type.
		/// </summary>
		/// <param name="type"></param>
		/// <param name="exactMatch"></param>
		/// <returns></returns>
		T FindRecord(Type type, bool exactMatch = false);

		/// <summary>
		/// Adds a record to the <see cref="Records">Records</see> collection.
		/// </summary>
		/// <param name="record"></param>
		/// <param name="makeCopy"></param>
		/// <returns></returns>
		RetCode AddRecord(T record, bool makeCopy = false);

		/// <summary>
		/// Removes a record from the <see cref="Records">Records</see> collection.
		/// </summary>
		/// <param name="uid"></param>
		/// <returns></returns>
		T RemoveRecord(long uid);

		/// <summary>
		/// Removes a record from the <see cref="Records">Records</see> collection based on interface type.
		/// </summary>
		/// <param name="type"></param>
		/// <param name="exactMatch"></param>
		/// <returns></returns>
		T RemoveRecord(Type type, bool exactMatch = false);

		/// <summary>
		/// Gets the next available record Uid.
		/// </summary>
		/// <param name="allocate"></param>
		/// <returns></returns>
		long GetRecordUid(bool allocate = true);

		/// <summary>
		/// Frees a record Uid, making it available again for use.
		/// </summary>
		/// <param name="uid"></param>
		void FreeRecordUid(long uid);
	}
}
