
// RoomDb.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Collections.Generic;
using Eamon.Framework;
using Eamon.Framework.DataStorage.Generic;
using Eamon.Game.Attributes;
using static Eamon.Game.Plugin.Globals;

namespace Eamon.Game.DataStorage.QuickAccess
{
	[ClassMappings(typeof(IRecordDb<IRoom>))]
	public class RoomDb : IRecordDb<IRoom>
	{
		public virtual bool CopyAddedRecord { get; set; }

		public virtual IRoom this[long uid]
		{
			get
			{
				return gDatabase.FindRoom(uid);
			}

			set
			{
				if (value == null || value.Uid == uid)
				{
					gDatabase.RemoveRoom(uid);
				}

				if (value != null && value.Uid == uid)
				{
					gDatabase.AddRoom(value, CopyAddedRecord);
				}
			}
		}

		public virtual ICollection<IRoom> Records
		{
			get
			{
				return gDatabase?.RoomTable?.Records;
			}
		}

		public RoomDb()
		{
			CopyAddedRecord = true;
		}
	}
}
