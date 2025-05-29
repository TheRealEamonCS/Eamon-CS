
// ModuleDb.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Collections.Generic;
using Eamon.Framework;
using Eamon.Framework.DataStorage.Generic;
using Eamon.Game.Attributes;
using static Eamon.Game.Plugin.Globals;

namespace Eamon.Game.DataStorage.QuickAccess
{
	[ClassMappings(typeof(IRecordDb<IModule>))]
	public class ModuleDb : IRecordDb<IModule>
	{
		public virtual bool CopyAddedRecord { get; set; }

		public virtual IModule this[long uid]
		{
			get
			{
				return gDatabase?.FindModule(uid);
			}

			set
			{
				if (value == null || value.Uid == uid)
				{
					gDatabase?.RemoveModule(uid);
				}

				if (value != null && value.Uid == uid)
				{
					gDatabase?.AddModule(value, CopyAddedRecord);
				}
			}
		}

		public virtual ICollection<IModule> Records
		{
			get
			{
				return gDatabase?.ModuleTable?.Records;
			}
		}

		public ModuleDb()
		{
			CopyAddedRecord = true;
		}
	}
}
