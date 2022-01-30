
// MonsterDb.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Collections.Generic;
using Eamon.Framework;
using Eamon.Framework.DataStorage.Generic;
using Eamon.Game.Attributes;
using static Eamon.Game.Plugin.PluginContext;

namespace Eamon.Game.DataStorage.QuickAccess
{
	[ClassMappings(typeof(IRecordDb<IMonster>))]
	public class MonsterDb : IRecordDb<IMonster>
	{
		public virtual bool CopyAddedRecord { get; set; }

		public virtual IMonster this[long uid]
		{
			get
			{
				return Globals.Database.FindMonster(uid);
			}

			set
			{
				if (value == null || value.Uid == uid)
				{
					Globals.Database.RemoveMonster(uid);
				}

				if (value != null && value.Uid == uid)
				{
					Globals.Database.AddMonster(value, CopyAddedRecord);
				}
			}
		}

		public virtual ICollection<IMonster> Records
		{
			get
			{
				return Globals?.Database?.MonsterTable?.Records;
			}
		}

		public MonsterDb()
		{
			CopyAddedRecord = true;
		}
	}
}
