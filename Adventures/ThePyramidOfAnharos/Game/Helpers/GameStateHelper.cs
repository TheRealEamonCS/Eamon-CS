
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

		public virtual bool ValidateKL()
		{
			return true;		// TODO: implement
		}

		public virtual bool ValidateKN()
		{
			return Record.KN >= 0 && Record.KN <= 1;
		}

		public virtual bool ValidateKO()
		{
			return Record.KO >= 0 && Record.KO <= 1;
		}

		public virtual bool ValidateKP()
		{
			return Record.KP >= 0 && Record.KP <= 1;
		}

		public virtual bool ValidateKQ()
		{
			return Record.KQ >= 0 && Record.KQ <= 1;
		}

		public virtual bool ValidateKR()
		{
			return Record.KR >= 0 && Record.KR <= 4;
		}

		public virtual bool ValidateKS()
		{
			return Record.KS >= 0 && Record.KS <= 7;
		}

		public virtual bool ValidateKT()
		{
			return Record.KT >= 0 && Record.KT <= 1;
		}

		public virtual bool ValidateKU()
		{
			return Record.KU >= 0 && Record.KU <= 2;
		}

		public virtual bool ValidateKV()
		{
			return Record.KV >= 0 && Record.KV <= 1;
		}

		public virtual bool ValidateKW()
		{
			return Record.KW <= 200;
		}

		public GameStateHelper()
		{

		}
	}
}
