
// ConfigDb.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Collections.Generic;
using Eamon.Framework;
using Eamon.Framework.DataStorage.Generic;
using Eamon.Game.Attributes;
using static Eamon.Game.Plugin.PluginContext;

namespace Eamon.Game.DataStorage.QuickAccess
{
	[ClassMappings(typeof(IRecordDb<IConfig>))]
	public class ConfigDb : IRecordDb<IConfig>
	{
		public virtual bool CopyAddedRecord { get; set; }

		public virtual IConfig this[long uid]
		{
			get
			{
				return Globals.Database.FindConfig(uid);
			}

			set
			{
				if (value == null || value.Uid == uid)
				{
					Globals.Database.RemoveConfig(uid);
				}

				if (value != null && value.Uid == uid)
				{
					Globals.Database.AddConfig(value, CopyAddedRecord);
				}
			}
		}

		public virtual ICollection<IConfig> Records
		{
			get
			{
				return Globals?.Database?.ConfigTable?.Records;
			}
		}

		public ConfigDb()
		{
			CopyAddedRecord = true;
		}
	}
}
