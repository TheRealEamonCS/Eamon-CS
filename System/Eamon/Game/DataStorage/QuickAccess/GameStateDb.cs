
// GameStateDb.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Collections.Generic;
using Eamon.Framework;
using Eamon.Framework.DataStorage.Generic;
using Eamon.Game.Attributes;
using static Eamon.Game.Plugin.PluginContext;

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
				return Globals.Database.FindGameState(uid);
			}

			set
			{
				if (value == null || value.Uid == uid)
				{
					Globals.Database.RemoveGameState(uid);
				}

				if (value != null && value.Uid == uid)
				{
					Globals.Database.AddGameState(value, CopyAddedRecord);
				}
			}
		}

		public virtual ICollection<IGameState> Records
		{
			get
			{
				return Globals?.Database?.GameStateTable?.Records;
			}
		}

		public GameStateDb()
		{
			CopyAddedRecord = true;
		}
	}
}
