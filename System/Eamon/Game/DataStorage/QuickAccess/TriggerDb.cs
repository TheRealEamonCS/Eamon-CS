
// TriggerDb.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Collections.Generic;
using Eamon.Framework;
using Eamon.Framework.DataStorage.Generic;
using Eamon.Game.Attributes;
using static Eamon.Game.Plugin.PluginContext;

namespace Eamon.Game.DataStorage.QuickAccess
{
	[ClassMappings(typeof(IRecordDb<ITrigger>))]
	public class TriggerDb : IRecordDb<ITrigger>
	{
		public virtual bool CopyAddedRecord { get; set; }

		public virtual ITrigger this[long uid]
		{
			get
			{
				return Globals.Database.FindTrigger(uid);
			}

			set
			{
				if (value == null || value.Uid == uid)
				{
					Globals.Database.RemoveTrigger(uid);
				}

				if (value != null && value.Uid == uid)
				{
					Globals.Database.AddTrigger(value, CopyAddedRecord);
				}
			}
		}

		public virtual ICollection<ITrigger> Records
		{
			get
			{
				return Globals?.Database?.TriggerTable?.Records;
			}
		}

		public TriggerDb()
		{
			CopyAddedRecord = true;
		}
	}
}
