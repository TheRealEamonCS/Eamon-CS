
// GameStateHelper.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework.Helpers;
using Eamon.Game.Attributes;

namespace Firestorm.Game.Helpers
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
		public virtual bool ValidateOS()
		{
			var result = Record.OS != null && Record.OS.Length == 3;

			if (result)
			{
				for (var i = 1; i < Record.OS.Length; i++)
				{
					var n = Record.GetOS(i);

					if (n < 1)
					{
						result = false;

						break;
					}
				}
			}

			return result;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidatePG()
		{
			var result = Record.PG != null && Record.PG.Length == 10;

			if (result)
			{
				for (var i = 1; i < Record.PG.Length; i++)
				{
					var n = Record.GetPG(i);

					if (n < 0 || n > 1)
					{
						result = false;

						break;
					}
				}
			}

			return result;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateMP()
		{
			return Record.MP >= 0;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateST()
		{
			return Record.ST >= 0 && Record.ST <= 4;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateGH()
		{
			return Record.GH >= 0 && Record.GH <= 1;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidatePZ()
		{
			return Record.PZ >= 0 && Record.PZ <= 1;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateRZ()
		{
			return Record.RZ >= 0 && Record.RZ <= 1;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateMY()
		{
			return Record.MY >= 0 && Record.MY <= 2;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateNF()
		{
			return Record.NF >= 0 && Record.NF <= 1;
		}

		public GameStateHelper()
		{

		}
	}
}
