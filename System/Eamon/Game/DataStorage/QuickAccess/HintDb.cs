
// HintDb.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Collections.Generic;
using Eamon.Framework;
using Eamon.Framework.DataStorage.Generic;
using Eamon.Game.Attributes;
using static Eamon.Game.Plugin.Globals;

namespace Eamon.Game.DataStorage.QuickAccess
{
	[ClassMappings(typeof(IRecordDb<IHint>))]
	public class HintDb : IRecordDb<IHint>
	{
		public virtual bool CopyAddedRecord { get; set; }

		public virtual IHint this[long uid]
		{
			get
			{
				return gEngine.Database.FindHint(uid);
			}

			set
			{
				if (value == null || value.Uid == uid)
				{
					gEngine.Database.RemoveHint(uid);
				}

				if (value != null && value.Uid == uid)
				{
					gEngine.Database.AddHint(value, CopyAddedRecord);
				}
			}
		}

		public virtual ICollection<IHint> Records
		{
			get
			{
				return gEngine.Database?.HintTable?.Records;
			}
		}

		public HintDb()
		{
			CopyAddedRecord = true;
		}
	}
}
