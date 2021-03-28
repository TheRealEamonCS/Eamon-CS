
// IRecordDb.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Collections.Generic;

namespace Eamon.Framework.DataStorage.Generic
{
	/// <summary></summary>
	public interface IRecordDb<T>
	{
		/// <summary></summary>
		bool CopyAddedRecord { get; set; }

		/// <summary></summary>
		T this[long uid] { get; set; }

		/// <summary></summary>
		ICollection<T> Records { get; }
	}
}
