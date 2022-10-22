
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
				return gEngine.Database.FindRoom(uid);
			}

			set
			{
				if (value == null || value.Uid == uid)
				{
					gEngine.Database.RemoveRoom(uid);
				}

				if (value != null && value.Uid == uid)
				{
					gEngine.Database.AddRoom(value, CopyAddedRecord);
				}
			}
		}

		public virtual ICollection<IRoom> Records
		{
			get
			{
				return gEngine?.Database?.RoomTable?.Records;
			}
		}

		public RoomDb()
		{
			CopyAddedRecord = true;
		}
	}
}
