
// GameStateHelper.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework.Helpers;
using Eamon.Game.Attributes;

namespace ThePyramidOfAnharos.Game.Helpers
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

		public virtual bool ValidateGD()
		{
			return Record.GD >= 0 && Record.GD <= 1;
		}

		public virtual bool ValidateGU()
		{
			return Record.GU >= 0 && Record.GU <= 2;
		}

		public virtual bool ValidateKE()
		{
			return Record.KE >= 0 && Record.KE <= 1;
		}

		public virtual bool ValidateKF()
		{
			return Record.KF >= 0 && Record.KF <= 1;
		}

		public virtual bool ValidateKG()
		{
			return Record.KG >= 0 && Record.KG <= 1;
		}

		public virtual bool ValidateKH()
		{
			return Record.KH >= 0 && Record.KH <= 1;
		}

		public GameStateHelper()
		{

		}
	}
}
