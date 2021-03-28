
// GameStateHelper.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Collections.Generic;
using Eamon.Framework.Helpers;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;

namespace ARuncibleCargo.Game.Helpers
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
		public virtual bool ValidateDreamCounter()
		{
			return Record.DreamCounter >= 0 && Record.DreamCounter <= 13;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateSwarmyCounter()
		{
			return Record.SwarmyCounter >= 1 && Record.SwarmyCounter <= 3;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateCargoOpenCounter()
		{
			return Record.CargoOpenCounter >= 0 && Record.CargoOpenCounter <= 3;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateCargoInRoom()
		{
			return Record.CargoInRoom >= 0 && Record.CargoInRoom <= 1;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateGiveAmazonMoney()
		{
			return Record.GiveAmazonMoney >= 0 && Record.GiveAmazonMoney <= 1;
		}

		public GameStateHelper()
		{
			FieldNameList.AddRange(new List<string>()
			{
				"DreamCounter",
				"SwarmyCounter",
				"CargoOpenCounter",
				"CargoInRoom",
				"GiveAmazonMoney",
			});
		}
	}
}
