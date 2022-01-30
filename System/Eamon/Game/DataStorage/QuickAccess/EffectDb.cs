
// EffectDb.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Collections.Generic;
using Eamon.Framework;
using Eamon.Framework.DataStorage.Generic;
using Eamon.Game.Attributes;
using static Eamon.Game.Plugin.PluginContext;

namespace Eamon.Game.DataStorage.QuickAccess
{
	[ClassMappings(typeof(IRecordDb<IEffect>))]
	public class EffectDb : IRecordDb<IEffect>
	{
		public virtual bool CopyAddedRecord { get; set; }

		public virtual IEffect this[long uid]
		{
			get
			{
				return Globals.Database.FindEffect(uid);
			}

			set
			{
				if (value == null || value.Uid == uid)
				{
					Globals.Database.RemoveEffect(uid);
				}

				if (value != null && value.Uid == uid)
				{
					Globals.Database.AddEffect(value, CopyAddedRecord);
				}
			}
		}

		public virtual ICollection<IEffect> Records
		{
			get
			{
				return Globals?.Database?.EffectTable?.Records;
			}
		}

		public EffectDb()
		{
			CopyAddedRecord = true;
		}
	}
}
