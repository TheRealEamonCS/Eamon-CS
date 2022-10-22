
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
				return gEngine.Database.FindCharacter(uid);
			}

			set
			{
				if (value == null || value.Uid == uid)
				{
					gEngine.Database.RemoveCharacter(uid);
				}

				if (value != null && value.Uid == uid)
				{
					gEngine.Database.AddCharacter(value, CopyAddedRecord);
				}
			}
		}

		public virtual ICollection<ICharacter> Records
		{
			get
			{
				return gEngine?.Database?.CharacterTable?.Records;
			}
		}

		public CharacterDb()
		{
			CopyAddedRecord = true;
		}
	}
}
