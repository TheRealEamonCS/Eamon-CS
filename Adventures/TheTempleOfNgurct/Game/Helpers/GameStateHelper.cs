
// GameStateHelper.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework.Helpers;
using Eamon.Game.Attributes;

namespace TheTempleOfNgurct.Game.Helpers
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
		public virtual bool ValidateWanderingMonster()
		{
			return Record.WanderingMonster >= 12 && Record.WanderingMonster <= 27;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateDwLoopCounter()
		{
			return Record.DwLoopCounter >= 0 && Record.DwLoopCounter <= 16;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateWandCharges()
		{
			return Record.WandCharges >= 0 && Record.WandCharges <= 5;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateRegenerate()
		{
			return Record.Regenerate >= 0 && Record.Regenerate <= 5;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateKeyRingRoomUid()
		{
			return Record.KeyRingRoomUid >= 0 && Record.KeyRingRoomUid <= 59;
		}

		public GameStateHelper()
		{

		}
	}
}
