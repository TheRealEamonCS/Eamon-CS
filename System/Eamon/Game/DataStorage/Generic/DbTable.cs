
// DbTable.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Polenter.Serialization;
using Eamon.Framework;
using Eamon.Framework.DataStorage.Generic;
using Eamon.ThirdParty;
using static Eamon.Game.Plugin.Globals;

namespace Eamon.Game.DataStorage.Generic
{
	public abstract class DbTable<T> : IDbTable<T> where T : class, IGameBase
	{
		public virtual ICollection<T> Records { get; set; }

		public virtual SortedSet<long> FreeUids { get; set; }

		[ExcludeFromSerialization]
		public virtual T FindRec { get; set; }

		public virtual long CurrUid { get; set; }

		public virtual RetCode FreeRecords(bool dispose = true)
		{
			Debug.Assert(CurrUid >= 0);

			if (dispose)
			{
				foreach (var record in Records)
				{
					record.Dispose();
				}
			}

			Records.Clear();

			FreeUids.Clear();

			CurrUid = 0;

			return RetCode.Success;
		}

		public virtual long GetRecordCount()
		{
			return Records.Count;
		}

		public virtual T FindRecord(long uid)
		{
			T result;

			FindRec.Uid = uid;

			if (!((BTree<T>)Records).TryGetValue(FindRec, out result))
			{
				result = default(T);
			}

			return result;
		}

		public virtual T FindRecord(Type type, bool exactMatch = false)
		{
			T result;

			result = default(T);

			if (type == null)
			{
				// PrintError

				goto Cleanup;
			}

			result = Records.FirstOrDefault(r => exactMatch ? r.GetType().GetInterfaces(false).Contains(type) : type.IsInstanceOfType(r));

		Cleanup:

			return result;
		}

		public virtual RetCode AddRecord(T record, bool makeCopy = false)
		{
			RetCode rc;
			T result;

			if (record == null)
			{
				rc = RetCode.InvalidArg;

				// PrintError

				goto Cleanup;
			}

			rc = RetCode.Success;

			result = FindRecord(record.Uid);

			if (result != null)
			{
				rc = RetCode.AlreadyExists;

				// PrintError

				goto Cleanup;
			}

			result = makeCopy ? gEngine.CloneInstance(record) : record;

			if (result == null)
			{
				rc = RetCode.OutOfMemory;

				// PrintError

				goto Cleanup;
			}

			Records.Add(result);

			Debug.Assert(CurrUid >= 0);

			while (CurrUid < record.Uid)
			{
				if (CurrUid + 1 < record.Uid)
				{
					FreeUids.Add(CurrUid + 1);
				}

				CurrUid++;
			}

		Cleanup:

			return rc;
		}

		public virtual T RemoveRecord(long uid)
		{
			T result;

			result = FindRecord(uid);

			if (result != null)
			{
				if (!Records.Remove(result))
				{
					result = default(T);

					// PrintError

					goto Cleanup;
				}
			}

		Cleanup:

			return result;
		}

		public virtual T RemoveRecord(Type type, bool exactMatch = false)
		{
			T result;

			result = FindRecord(type, exactMatch);

			if (result != null)
			{
				if (!Records.Remove(result))
				{
					result = default(T);

					// PrintError

					goto Cleanup;
				}
			}

		Cleanup:

			return result;
		}

		public virtual long GetRecordUid(bool allocate = true)
		{
			long result;

			result = -1;

			Debug.Assert(CurrUid >= 0);

			if (FreeUids.Count > 0)
			{
				result = FreeUids.Min;

				if (allocate)
				{
					FreeUids.Remove(result);
				}
			}
			else
			{
				result = CurrUid + 1;

				if (result > gEngine.NumRecords)
				{
					var record = Records.FirstOrDefault();

					var recordTypeName = record != null ? record.GetType().Name : gEngine.UnknownName;

					throw new InvalidOperationException(string.Format("{0} database table has exhausted all available Uids.", recordTypeName));
				}

				if (allocate)
				{
					CurrUid++;
				}
			}

			return result;
		}

		public virtual void FreeRecordUid(long uid)
		{
			if (uid < 1)
			{
				// PrintError

				goto Cleanup;
			}

			Debug.Assert(CurrUid >= 0);

			if (CurrUid == uid)
			{
				CurrUid--;

				while (FreeUids.Count > 0 && FreeUids.Remove(CurrUid))
				{
					CurrUid--;
				}
			}
			else if (CurrUid > uid)
			{
				FreeUids.Add(uid);
			}

		Cleanup:

			;
		}

		public DbTable()
		{
			Records = new BTree<T>(16);

			FreeUids = new SortedSet<long>();

			FindRec = gEngine.CreateInstance<T>();
		}
	}
}
