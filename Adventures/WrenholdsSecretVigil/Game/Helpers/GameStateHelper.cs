
// GameStateHelper.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework.Helpers;
using Eamon.Game.Attributes;

namespace WrenholdsSecretVigil.Game.Helpers
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
		public virtual bool ValidateMedallionCharges()
		{
			return Record.MedallionCharges >= 0 && Record.MedallionCharges <= 15;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateSlimeBlasts()
		{
			return Record.SlimeBlasts >= 0 && Record.SlimeBlasts <= 3;
		}

		public GameStateHelper()
		{

		}
	}
}
