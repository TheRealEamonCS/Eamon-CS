﻿
// CharacterDb.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Collections.Generic;
using Eamon.Framework;
using Eamon.Framework.DataStorage.Generic;
using Eamon.Game.Attributes;
using static Eamon.Game.Plugin.Globals;

namespace Eamon.Game.DataStorage.QuickAccess
{
	[ClassMappings(typeof(IRecordDb<ICharacter>))]
	public class CharacterDb : IRecordDb<ICharacter>
	{
		public virtual bool CopyAddedRecord { get; set; }

		public virtual ICharacter this[long uid]
		{
			get
			{
				return gDatabase.FindCharacter(uid);
			}

			set
			{
				if (value == null || value.Uid == uid)
				{
					gDatabase.RemoveCharacter(uid);
				}

				if (value != null && value.Uid == uid)
				{
					gDatabase.AddCharacter(value, CopyAddedRecord);
				}
			}
		}

		public virtual ICollection<ICharacter> Records
		{
			get
			{
				return gDatabase?.CharacterTable?.Records;
			}
		}

		public CharacterDb()
		{
			CopyAddedRecord = true;
		}
	}
}
