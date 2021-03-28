
// HintDb.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Collections.Generic;
using Eamon.Framework;
using Eamon.Framework.DataStorage.Generic;
using Eamon.Game.Attributes;
using static Eamon.Game.Plugin.PluginContext;

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
				return Globals.Database.FindHint(uid);
			}

			set
			{
				if (value == null || value.Uid == uid)
				{
					Globals.Database.RemoveHint(uid);
				}

				if (value != null && value.Uid == uid)
				{
					Globals.Database.AddHint(value, CopyAddedRecord);
				}
			}
		}

		public virtual ICollection<IHint> Records
		{
			get
			{
				return Globals?.Database?.HintTable?.Records;
			}
		}

		public HintDb()
		{
			CopyAddedRecord = true;
		}
	}
}
