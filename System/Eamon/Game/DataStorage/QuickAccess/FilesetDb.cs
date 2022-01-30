
// FilesetDb.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Collections.Generic;
using Eamon.Framework;
using Eamon.Framework.DataStorage.Generic;
using Eamon.Game.Attributes;
using static Eamon.Game.Plugin.PluginContext;

namespace Eamon.Game.DataStorage.QuickAccess
{
	[ClassMappings(typeof(IRecordDb<IFileset>))]
	public class FilesetDb : IRecordDb<IFileset>
	{
		public virtual bool CopyAddedRecord { get; set; }

		public virtual IFileset this[long uid]
		{
			get
			{
				return Globals.Database.FindFileset(uid);
			}

			set
			{
				if (value == null || value.Uid == uid)
				{
					Globals.Database.RemoveFileset(uid);
				}

				if (value != null && value.Uid == uid)
				{
					Globals.Database.AddFileset(value, CopyAddedRecord);
				}
			}
		}

		public virtual ICollection<IFileset> Records
		{
			get
			{
				return Globals?.Database?.FilesetTable?.Records;
			}
		}

		public FilesetDb()
		{
			CopyAddedRecord = true;
		}
	}
}
