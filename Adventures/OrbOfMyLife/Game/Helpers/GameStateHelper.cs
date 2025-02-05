
// GameStateHelper.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework.Helpers;
using Eamon.Game.Attributes;
using static OrbOfMyLife.Game.Plugin.Globals;

namespace OrbOfMyLife.Game.Helpers
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
		public virtual bool ValidateMW()
		{
			return !string.IsNullOrWhiteSpace(Record.MW);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateTW()
		{
			return !string.IsNullOrWhiteSpace(Record.TW);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateCW()
		{
			return !string.IsNullOrWhiteSpace(Record.CW);
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateGOL()
		{
			return Record.GOL >= 0 && Record.GOL <= 4;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateSCR()
		{
			return Record.SCR >= 0 && Record.SCR <= 5;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateIS()
		{
			return Record.IS >= 0 && Record.IS <= 10;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateVC()
		{
			return Record.VC >= 0 && Record.VC <= 5;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateFC()
		{
			return Record.FC >= 0 && Record.FC <= 9;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateTC()
		{
			return Record.TC >= 0 && Record.TC <= 3;
		}

		/// <summary></summary>
		/// <returns></returns>
		public virtual bool ValidateSR()
		{
			var module = gEngine.GetModule();

			Debug.Assert(module != null);

			return Record.SR >= 0 && Record.SR <= module.NumRooms;
		}

		public GameStateHelper()
		{

		}
	}
}
