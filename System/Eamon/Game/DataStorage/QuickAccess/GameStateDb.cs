﻿
// GameStateDb.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Collections.Generic;
using Eamon.Framework;
using Eamon.Framework.DataStorage.Generic;
using Eamon.Game.Attributes;
using static Eamon.Game.Plugin.Globals;

namespace Eamon.Game.DataStorage.QuickAccess
{
	[ClassMappings(typeof(IRecordDb<IGameState>))]
	public class GameStateDb : IRecordDb<IGameState>
	{
		public virtual bool CopyAddedRecord { get; set; }

		public virtual IGameState this[long uid]
		{
			get
			{
				return gEngine.Database.FindGameState(uid);
			}

			set
			{
				if (value == null || value.Uid == uid)
				{
					gEngine.Database.RemoveGameState(uid);
				}

				if (value != null && value.Uid == uid)
				{
					gEngine.Database.AddGameState(value, CopyAddedRecord);
				}
			}
		}

		public virtual ICollection<IGameState> Records
		{
			get
			{
				return gEngine.Database?.GameStateTable?.Records;
			}
		}

		public GameStateDb()
		{
			CopyAddedRecord = true;
		}
	}
}
