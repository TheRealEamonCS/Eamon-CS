
// ModuleDb.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Collections.Generic;
using Eamon.Framework;
using Eamon.Framework.DataStorage.Generic;
using Eamon.Game.Attributes;
using static Eamon.Game.Plugin.PluginContext;

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
				return Globals.Database.FindModule(uid);
			}

			set
			{
				if (value == null || value.Uid == uid)
				{
					Globals.Database.RemoveModule(uid);
				}

				if (value != null && value.Uid == uid)
				{
					Globals.Database.AddModule(value, CopyAddedRecord);
				}
			}
		}

		public virtual ICollection<IModule> Records
		{
			get
			{
				return Globals?.Database?.ModuleTable?.Records;
			}
		}

		public ModuleDb()
		{
			CopyAddedRecord = true;
		}
	}
}
