
// EffectDb.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Collections.Generic;
using Eamon.Framework;
using Eamon.Framework.DataStorage.Generic;
using Eamon.Game.Attributes;
using static Eamon.Game.Plugin.Globals;

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
				return gDatabase.FindEffect(uid);
			}

			set
			{
				if (value == null || value.Uid == uid)
				{
					gDatabase.RemoveEffect(uid);
				}

				if (value != null && value.Uid == uid)
				{
					gDatabase.AddEffect(value, CopyAddedRecord);
				}
			}
		}

		public virtual ICollection<IEffect> Records
		{
			get
			{
				return gDatabase?.EffectTable?.Records;
			}
		}

		public EffectDb()
		{
			CopyAddedRecord = true;
		}
	}
}
