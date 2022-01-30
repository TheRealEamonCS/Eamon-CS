
// ScriptDb.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Collections.Generic;
using Eamon.Framework;
using Eamon.Framework.DataStorage.Generic;
using Eamon.Game.Attributes;
using static Eamon.Game.Plugin.PluginContext;

namespace Eamon.Game.DataStorage.QuickAccess
{
	[ClassMappings(typeof(IRecordDb<IScript>))]
	public class ScriptDb : IRecordDb<IScript>
	{
		public virtual bool CopyAddedRecord { get; set; }

		public virtual IScript this[long uid]
		{
			get
			{
				return Globals.Database.FindScript(uid);
			}

			set
			{
				if (value == null || value.Uid == uid)
				{
					Globals.Database.RemoveScript(uid);
				}

				if (value != null && value.Uid == uid)
				{
					Globals.Database.AddScript(value, CopyAddedRecord);
				}
			}
		}

		public virtual ICollection<IScript> Records
		{
			get
			{
				return Globals?.Database?.ScriptTable?.Records;
			}
		}

		public ScriptDb()
		{
			CopyAddedRecord = true;
		}
	}
}
