
// DbTable.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using Polenter.Serialization;
using Eamon.Framework;
using Eamon.Framework.DataStorage.Generic;
using Eamon.ThirdParty;
using static Eamon.Game.Plugin.PluginContext;

namespace Eamon.Game.DataStorage.Generic
{
	public abstract class DbTable<T> : IDbTable<T> where T : class, IGameBase
	{
		public virtual ICollection<T> Records { get; set; }

		public virtual IList<long> FreeUids { get; set; }

		[ExcludeFromSerialization]
		public virtual T FindRec { get; set; }

		public virtual long CurrUid { get; set; }

		public virtual RetCode FreeRecords(bool dispose = true)
		{
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

		public virtual long GetRecordsCount()
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

			result = makeCopy ? Globals.CloneInstance(record) : record;

			if (result == null)
			{
				rc = RetCode.OutOfMemory;

				// PrintError

				goto Cleanup;
			}

			Records.Add(result);

			if (record.Uid > CurrUid)
			{
				CurrUid = record.Uid;
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

			if (allocate)
			{
				if (FreeUids.Count > 0)
				{
					result = FreeUids[0];

					FreeUids.RemoveAt(0);
				}
				else
				{
					CurrUid++;

					result = CurrUid;
				}
			}
			else
			{
				result = CurrUid;
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

			if (CurrUid == uid)
			{
				CurrUid--;
			}
			else if (!FreeUids.Contains(uid))
			{
				FreeUids.Add(uid);
			}

		Cleanup:

			;
		}

		public DbTable()
		{
			Records = new BTree<T>(16);

			FreeUids = new List<long>();

			FindRec = Globals.CreateInstance<T>();
		}
	}
}
