
// GameStateHelper.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework.Helpers;
using Eamon.Game.Attributes;

namespace StrongholdOfKahrDur.Game.Helpers
{
	[ClassMappings]
	public class GameStateHelper : Eamon.Game.Helpers.GameStateHelper, IGameStateHelper
	{
		public virtual new Framework.IGameState Record
		{
			get
			{
				return (Framework.IGameState)base.Record;
			}

			set
			{
				if (base.Record != value)
				{
					base.Record = value;
				}
			}
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateLichState()
		{
			return Record.LichState >= 0 && Record.LichState <= 2;
		}

		public GameStateHelper()
		{

		}
	}
}
